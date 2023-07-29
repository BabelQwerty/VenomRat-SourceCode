using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Server.Handle_Packet;

public class HandleLogs
{
	public static List<LogMsg> LogMsgs = new List<LogMsg>();

	public void Addmsg(string Msg, Color color)
	{
		try
		{
			LogMsgs.Insert(0, new LogMsg
			{
				Time = DateTime.Now.ToLongTimeString(),
				Msg = Msg
			});
			Program.mainform.Invoke((MethodInvoker)delegate
			{
				lock (Settings.LockListviewLogs)
				{
					Program.mainform.gridControlLog.BeginUpdate();
					Program.mainform.gridControlLog.EndUpdate();
				}
			});
		}
		catch
		{
		}
	}
}
