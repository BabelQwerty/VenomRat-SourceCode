using System.Drawing;
using Server.Connection;
using Server.Properties;

namespace Server.Handle_Packet;

public class HandleReportWindow
{
	public HandleReportWindow(Clients client, string title)
	{
		new HandleLogs().Addmsg("Client " + client.Ip + " opened [" + title + "]", Color.Blue);
		_ = Server.Properties.Settings.Default.Notification;
	}
}
