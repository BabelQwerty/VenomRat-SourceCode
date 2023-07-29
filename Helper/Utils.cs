using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using cGeoIp;
using Server.Properties;

namespace Server.Helper;

public class Utils
{
	private static cGeoMain cGeo = new cGeoMain();

	public static Resources rc = new Resources();

	public static string LocalIP
	{
		get
		{
			try
			{
				string result;
				using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
				{
					socket.Connect("8.8.8.8", 65530);
					result = (socket.LocalEndPoint as IPEndPoint).Address.ToString();
					socket.Close();
				}
				return result;
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	private static bool IsPrivate(string ipAddress)
	{
		try
		{
			int[] array = (from s in ipAddress.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries)
				select int.Parse(s)).ToArray();
			if (array[0] == 10 || (array[0] == 192 && array[1] == 168) || (array[0] == 172 && array[1] >= 16 && array[1] <= 31))
			{
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	public static string GetSizeString(long size)
	{
		string[] array = new string[5] { "B", "KB", "MB", "GB", "TB" };
		int num = 0;
		float num2 = size;
		while (num2 >= 1024f)
		{
			num2 /= 1024f;
			num++;
		}
		return $"{num2:F1}{array[num]}";
	}

	public static string GetCountryName(string ip)
	{
		try
		{
			return cGeo.GetIpInf(ip).Split(':')[1];
		}
		catch
		{
		}
		if (!IsPrivate(ip))
		{
			return "UnKnown";
		}
		return "Private";
	}

	public static string GetCountryCode(string ip)
	{
		try
		{
			return cGeo.GetIpInf(ip).Split(':')[2];
		}
		catch
		{
		}
		try
		{
			return cGeo.GetIpInf(LocalIP).Split(':')[2];
		}
		catch
		{
		}
		return "";
	}

	public static Bitmap GetCountryFlag(string code)
	{
		PropertyInfo property = rc.GetType().GetProperty(code);
		if (property != null)
		{
			return (Bitmap)property.GetValue(code);
		}
		return null;
	}
}
