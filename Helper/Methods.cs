using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Helper;

public static class Methods
{
	private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	public static Random Random = new Random();

	private const int LVM_FIRST = 4096;

	private const int LVM_SETITEMSTATE = 4139;

	private const int WM_VSCROLL = 277;

	private static readonly IntPtr SB_PAGEBOTTOM = new IntPtr(7);

	public static void RecursiveDelete(string path)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		if (!directoryInfo.Exists)
		{
			return;
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			RecursiveDelete(directories[i].FullName);
		}
		try
		{
			directoryInfo.Delete(recursive: true);
		}
		catch
		{
		}
	}

	public static void Log(string msg)
	{
		try
		{
			File.AppendAllText("C:\\Temp\\server.log", DateTime.Now.ToString() + " : " + msg + "\n");
		}
		catch (Exception)
		{
		}
	}

	public static void LogEx(Exception ex)
	{
		try
		{
			File.AppendAllText("C:\\Temp\\server_ex.log", DateTime.Now.ToString() + " : ex " + ex.Message + " \n" + ex.StackTrace + "\n " + ex.Source + "\n");
		}
		catch (Exception)
		{
		}
	}

	public static string BytesToString(long byteCount)
	{
		string[] array = new string[7] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
		if (byteCount == 0L)
		{
			return "0" + array[0];
		}
		long num = Math.Abs(byteCount);
		int num2 = Convert.ToInt32(Math.Floor(Math.Log(num, 1024.0)));
		double num3 = Math.Round((double)num / Math.Pow(1024.0, num2), 1);
		return (double)Math.Sign(byteCount) * num3 + array[num2];
	}

	public static async Task FadeIn(Form o, int interval = 80)
	{
		while (o.Opacity < 1.0)
		{
			await Task.Delay(interval);
			o.Opacity += 0.05;
		}
	}

	public static double DiffSeconds(DateTime startTime, DateTime endTime)
	{
		return Math.Abs(new TimeSpan(endTime.Ticks - startTime.Ticks).TotalSeconds);
	}

	public static string GetRandomString(int length)
	{
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[Random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)]);
		}
		return stringBuilder.ToString();
	}

	public static int MakeWin32Long(short wLow, short wHigh)
	{
		return (wLow << 16) | wHigh;
	}

	public static void SetItemState(IntPtr handle, int itemIndex, int mask, int value)
	{
		NativeMethods.LVITEM lVITEM = default(NativeMethods.LVITEM);
		lVITEM.stateMask = mask;
		lVITEM.state = value;
		NativeMethods.LVITEM lParam = lVITEM;
		NativeMethods.SendMessageListViewItem(handle, 4139u, new IntPtr(itemIndex), ref lParam);
	}

	public static void ScrollToBottom(IntPtr handle)
	{
		NativeMethods.SendMessage(handle, 277u, SB_PAGEBOTTOM, IntPtr.Zero);
	}
}
