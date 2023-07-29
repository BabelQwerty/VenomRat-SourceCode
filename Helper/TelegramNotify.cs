using System;
using System.Net;
using Server.Properties;

namespace Server.Helper;

public class TelegramNotify
{
	public static void SendNotify(string _content)
	{
		if (!Server.Properties.Settings.Default.TelegramEnabled)
		{
			return;
		}
		try
		{
			string address = "https://api.telegram.org/bot" + Server.Properties.Settings.Default.TelegramToken + "/sendMessage?chat_id=" + Server.Properties.Settings.Default.TelegramChatId + "&text=" + _content;
			new WebClient().DownloadString(address);
		}
		catch (Exception)
		{
		}
	}
}
