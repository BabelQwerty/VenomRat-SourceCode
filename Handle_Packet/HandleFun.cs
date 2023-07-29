using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;

namespace Server.Handle_Packet;

public class HandleFun
{
	public void Fun(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			FormFun formFun = (FormFun)Application.OpenForms["fun:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
			if (formFun != null && formFun.Client == null)
			{
				formFun.Client = client;
				formFun.timer1.Enabled = true;
			}
		}
		catch
		{
		}
	}
}
