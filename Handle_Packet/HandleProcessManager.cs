using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;

namespace Server.Handle_Packet;

public class HandleProcessManager
{
	public class ProcItem
	{
		public string Name { get; set; }

		public string Pid { get; set; }
	}

	public void GetProcess(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			FormProcessManager formProcessManager = (FormProcessManager)Application.OpenForms["processManager:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
			if (formProcessManager != null)
			{
				if (formProcessManager.Client == null)
				{
					formProcessManager.Client = client;
					formProcessManager.timer1.Enabled = true;
				}
				string asString = unpack_msgpack.ForcePathObject("Message").AsString;
				formProcessManager.LoadList(asString);
			}
		}
		catch
		{
		}
	}
}
