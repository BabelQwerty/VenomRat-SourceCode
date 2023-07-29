using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Connection;
using Server.Helper;

namespace Server.ReverseProxy;

public class ReverseProxyClient
{
	public enum ProxyType
	{
		Unknown,
		SOCKS5,
		HTTPS
	}

	public const int SOCKS5_DEFAULT_PORT = 3218;

	public const byte SOCKS5_VERSION_NUMBER = 5;

	public const byte SOCKS5_RESERVED = 0;

	public const byte SOCKS5_AUTH_NUMBER_OF_AUTH_METHODS_SUPPORTED = 2;

	public const byte SOCKS5_AUTH_METHOD_NO_AUTHENTICATION_REQUIRED = 0;

	public const byte SOCKS5_AUTH_METHOD_GSSAPI = 1;

	public const byte SOCKS5_AUTH_METHOD_USERNAME_PASSWORD = 2;

	public const byte SOCKS5_AUTH_METHOD_IANA_ASSIGNED_RANGE_BEGIN = 3;

	public const byte SOCKS5_AUTH_METHOD_IANA_ASSIGNED_RANGE_END = 127;

	public const byte SOCKS5_AUTH_METHOD_RESERVED_RANGE_BEGIN = 128;

	public const byte SOCKS5_AUTH_METHOD_RESERVED_RANGE_END = 254;

	public const byte SOCKS5_AUTH_METHOD_REPLY_NO_ACCEPTABLE_METHODS = byte.MaxValue;

	public const byte SOCKS5_CMD_REPLY_SUCCEEDED = 0;

	public const byte SOCKS5_CMD_REPLY_GENERAL_SOCKS_SERVER_FAILURE = 1;

	public const byte SOCKS5_CMD_REPLY_CONNECTION_NOT_ALLOWED_BY_RULESET = 2;

	public const byte SOCKS5_CMD_REPLY_NETWORK_UNREACHABLE = 3;

	public const byte SOCKS5_CMD_REPLY_HOST_UNREACHABLE = 4;

	public const byte SOCKS5_CMD_REPLY_CONNECTION_REFUSED = 5;

	public const byte SOCKS5_CMD_REPLY_TTL_EXPIRED = 6;

	public const byte SOCKS5_CMD_REPLY_COMMAND_NOT_SUPPORTED = 7;

	public const byte SOCKS5_CMD_REPLY_ADDRESS_TYPE_NOT_SUPPORTED = 8;

	public const byte SOCKS5_ADDRTYPE_IPV4 = 1;

	public const byte SOCKS5_ADDRTYPE_DOMAIN_NAME = 3;

	public const byte SOCKS5_ADDRTYPE_IPV6 = 4;

	public const int BUFFER_SIZE = 8192;

	private bool _receivedConnResponse;

	private MemoryStream _handshakeStream;

	private byte[] _buffer;

	private bool _isBindCommand;

	private bool _isUdpCommand;

	private bool _isConnectCommand;

	private bool _isIpType;

	private bool _isIPv6NameType;

	private bool _isDomainNameType;

	private bool _disconnectIsSend;

	private ReverseProxyServer Server;

	public Socket Handle { get; private set; }

	public string UserIP
	{
		get
		{
			try
			{
				return ((IPEndPoint)Handle.RemoteEndPoint).Address.ToString();
			}
			catch
			{
			}
			return "Unknown";
		}
	}

	public string ClientIP => Client.Ip;

	public string TypeStr
	{
		get
		{
			if (Type == ProxyType.SOCKS5)
			{
				return "SOCK5";
			}
			if (Type == ProxyType.HTTPS)
			{
				return "HTTPS";
			}
			return "Unknown";
		}
	}

	public string UserCountry => Utils.GetCountryName(UserIP);

	public string ClientCountry => Utils.GetCountryName(ClientIP);

	public string ReceivedStr => Utils.GetSizeString(LengthReceived);

	public string SendStr => Utils.GetSizeString(LengthSent);

	public string TargetStr => $"{TargetServer} : {TargetPort}";

	public Clients Client { get; private set; }

	public long PacketsReceived { get; private set; }

	public long PacketsSended { get; private set; }

	public long LengthReceived { get; private set; }

	public long LengthSent { get; private set; }

	public int ConnectionId => Handle.Handle.ToInt32();

	public string TargetServer { get; private set; }

	public ushort TargetPort { get; private set; }

	public bool IsConnected { get; private set; }

	public ProxyType Type { get; private set; }

	public string HostName { get; private set; }

	public bool ProxySuccessful { get; private set; }

	public ReverseProxyClient(Clients client, Socket socket, ReverseProxyServer server)
	{
		Handle = socket;
		Client = client;
		_handshakeStream = new MemoryStream();
		_buffer = new byte[8192];
		IsConnected = true;
		TargetServer = "";
		Type = ProxyType.Unknown;
		Server = server;
		try
		{
			socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, AsyncReceive, null);
		}
		catch
		{
			Disconnect();
		}
	}

	private void AsyncReceive(IAsyncResult ar)
	{
		try
		{
			int num = Handle.EndReceive(ar);
			if (num <= 0)
			{
				Disconnect();
				return;
			}
			if (num > 5000 || _handshakeStream.Length + num > 5000)
			{
				Disconnect();
				return;
			}
			LengthReceived += num;
			_handshakeStream.Write(_buffer, 0, num);
		}
		catch
		{
			Disconnect();
			return;
		}
		byte[] array = _handshakeStream.ToArray();
		switch (PacketsReceived)
		{
		case 0L:
		{
			if (array.Length < 3)
			{
				break;
			}
			string @string = Encoding.ASCII.GetString(array);
			if (array[0] == 5)
			{
				Type = ProxyType.SOCKS5;
			}
			if (!@string.StartsWith("CONNECT") || !@string.Contains(":"))
			{
				break;
			}
			Type = ProxyType.HTTPS;
			using StreamReader streamReader = new StreamReader(new MemoryStream(array));
			string text = streamReader.ReadLine();
			if (text == null)
			{
				break;
			}
			string[] array2 = text.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length != 0)
			{
				try
				{
					string text2 = array2[1];
					TargetServer = text2.Split(':')[0];
					TargetPort = ushort.Parse(text2.Split(':')[1]);
					_isConnectCommand = true;
					_isDomainNameType = true;
					Client.SendProxyCommand(new ReverseProxyConnect
					{
						ConnectionId = ConnectionId,
						Target = TargetServer,
						Port = TargetPort
					}, ReverseProxyCommands.CONNECT);
					Server.CallonConnectionEstablished(this);
					return;
				}
				catch
				{
					Disconnect();
				}
			}
			goto IL_01cf;
		}
		case 1L:
			{
				int num2 = 6;
				if (array.Length < num2)
				{
					break;
				}
				if (!CheckProxyVersion(array))
				{
					return;
				}
				_isConnectCommand = array[1] == 1;
				_isBindCommand = array[1] == 2;
				_isUdpCommand = array[1] == 3;
				_isIpType = array[3] == 1;
				_isDomainNameType = array[3] == 3;
				_isIPv6NameType = array[3] == 4;
				Array.Reverse(array, array.Length - 2, 2);
				TargetPort = BitConverter.ToUInt16(array, array.Length - 2);
				if (_isConnectCommand)
				{
					if (_isIpType)
					{
						TargetServer = array[4] + "." + array[5] + "." + array[6] + "." + array[7];
					}
					else if (_isDomainNameType)
					{
						int num3 = array[4];
						if (num2 + num3 < array.Length)
						{
							TargetServer = Encoding.ASCII.GetString(array, 5, num3);
						}
					}
					if (TargetServer.Length > 0)
					{
						Client.SendProxyCommand(new ReverseProxyConnect
						{
							ConnectionId = ConnectionId,
							Target = TargetServer,
							Port = TargetPort
						}, ReverseProxyCommands.CONNECT);
					}
					Server.CallonUpdateConnection(this);
				}
				else
				{
					SendFailToClient();
				}
				return;
			}
			IL_01cf:
			if (CheckProxyVersion(array))
			{
				SendSuccessToClient();
				PacketsReceived++;
				_handshakeStream.SetLength(0L);
				Server.CallonConnectionEstablished(this);
			}
			break;
		}
		try
		{
			Handle.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, AsyncReceive, null);
		}
		catch
		{
			Disconnect();
		}
	}

	public void Disconnect()
	{
		if (!_disconnectIsSend)
		{
			_disconnectIsSend = true;
			Client.SendProxyCommand(new ReverseProxyDisconnect
			{
				ConnectionId = ConnectionId
			}, ReverseProxyCommands.DISCONNECT);
		}
		try
		{
			Handle.Close();
		}
		catch
		{
		}
		IsConnected = false;
		Server.CallonUpdateConnection(this);
	}

	public void SendToClient(byte[] payload)
	{
		lock (Handle)
		{
			try
			{
				LengthSent += payload.Length;
				Handle.Send(payload);
			}
			catch
			{
				Disconnect();
			}
		}
		Server.CallonUpdateConnection(this);
	}

	private void SendFailToClient()
	{
		if (Type == ProxyType.HTTPS)
		{
			Disconnect();
		}
		if (Type == ProxyType.SOCKS5)
		{
			SendToClient(new byte[2] { 5, 255 });
			Disconnect();
		}
	}

	private void SendSuccessToClient()
	{
		if (Type == ProxyType.SOCKS5)
		{
			SendToClient(new byte[2] { 5, 0 });
		}
	}

	private bool CheckProxyVersion(byte[] payload)
	{
		if (Type == ProxyType.HTTPS)
		{
			return true;
		}
		if (payload.Length != 0 && payload[0] != 5)
		{
			SendFailToClient();
			Disconnect();
			return false;
		}
		return true;
	}

	public void HandleCommandResponse(ReverseProxyConnectResponse response)
	{
		if (_receivedConnResponse)
		{
			return;
		}
		_receivedConnResponse = true;
		if (response.IsConnected)
		{
			HostName = response.HostName;
			if (Type == ProxyType.HTTPS)
			{
				SendToClient(Encoding.ASCII.GetBytes("HTTP/1.0 200 Connection established\r\n\r\n"));
			}
			else if (Type == ProxyType.SOCKS5)
			{
				try
				{
					List<byte> list = new List<byte>();
					list.Add(5);
					list.Add(0);
					list.Add(0);
					list.Add(1);
					list.AddRange(response.LocalAddress);
					list.Add((byte)Math.Floor((decimal)response.LocalPort / 256m));
					list.Add((byte)(response.LocalPort % 256));
					SendToClient(list.ToArray());
				}
				catch
				{
					SendToClient(new byte[10] { 5, 0, 0, 1, 0, 0, 0, 0, 0, 0 });
				}
			}
			_handshakeStream.Close();
			ProxySuccessful = true;
			try
			{
				Handle.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, AsyncReceiveProxy, null);
			}
			catch
			{
				Disconnect();
			}
		}
		else if (Type == ProxyType.HTTPS)
		{
			Disconnect();
		}
		else if (Type == ProxyType.SOCKS5)
		{
			SendToClient(new byte[10] { 5, 5, 0, 1, 0, 0, 0, 0, 0, 0 });
		}
		Server.CallonUpdateConnection(this);
	}

	private void AsyncReceiveProxy(IAsyncResult ar)
	{
		try
		{
			int num = Handle.EndReceive(ar);
			if (num <= 0)
			{
				Disconnect();
				return;
			}
			LengthReceived += num;
			ReverseProxyData reverseProxyData = new ReverseProxyData
			{
				ConnectionId = ConnectionId
			};
			reverseProxyData.Data = new byte[num];
			Array.Copy(_buffer, reverseProxyData.Data, num);
			Client.SendProxyCommand(reverseProxyData, ReverseProxyCommands.DATA);
			LengthSent += num;
			PacketsSended++;
		}
		catch
		{
			Disconnect();
			return;
		}
		PacketsReceived++;
		Server.CallonUpdateConnection(this);
		try
		{
			Handle.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, AsyncReceiveProxy, null);
		}
		catch
		{
		}
	}
}
