using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Forms;

public class FormFun : XtraForm
{
	private IContainer components;

	private Label label2;

	private Label label1;

	private Label label3;

	private Label label4;

	public System.Windows.Forms.Timer timer1;

	private ToggleSwitch toggleSwitchTaskbar;

	private Label label5;

	private ToggleSwitch toggleSwitchDesktop;

	private Label label6;

	private ToggleSwitch toggleSwitchClock;

	private Label label7;

	private ToggleSwitch toggleSwitchMouse;

	private Label label8;

	private ToggleSwitch toggleSwitchOpenDC;

	private Label label9;

	private ToggleSwitch toggleSwitchLock;

	private Label label10;

	private ToggleSwitch toggleSwitchCamera;

	private Label label11;

	private SimpleButton btnOk;

	private SimpleButton simpleButton1;

	private SimpleButton simpleButton2;

	private SimpleButton simpleButton3;

	private SimpleButton simpleButton4;

	private SpinEdit numericUpDown1;

	private SpinEdit numericUpDown2;

	private GroupControl groupControl1;

	private GroupControl groupControl2;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	internal Clients ParentClient { get; set; }

	public FormFun()
	{
		InitializeComponent();
	}

	private void Timer1_Tick(object sender, EventArgs e)
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
		}
	}

	private void button11_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "blockInput";
		msgPack.ForcePathObject("Time").AsString = numericUpDown1.Value.ToString();
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void button15_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "holdMouse";
		msgPack.ForcePathObject("Time").AsString = numericUpDown2.Value.ToString();
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void button12_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "monitorOff";
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void button14_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "hangSystem";
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void button13_Click(object sender, EventArgs e)
	{
	}

	private void FormFun_FormClosed(object sender, FormClosedEventArgs e)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			Client?.Disconnected();
		});
	}

	private void button19_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "webcamlight+";
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void button16_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "webcamlight-";
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void button13_Click_1(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "(*.wav)|*.wav";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			byte[] asBytes = File.ReadAllBytes(openFileDialog.FileName);
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "playAudio";
			msgPack.ForcePathObject("wavfile").SetAsBytes(asBytes);
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		else
		{
			MessageBox.Show("Please choose a wav file.");
		}
	}

	private void toggleSwitchTaskbar_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchTaskbar.IsOn ? "Taskbar-" : "Taskbar+");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void toggleSwitchDesktop_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchDesktop.IsOn ? "Desktop-" : "Desktop+");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void toggleSwitchClock_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchClock.IsOn ? "Clock-" : "Clock+");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void toggleSwitchMouse_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchMouse.IsOn ? "swapMouseButtons" : "restoreMouseButtons");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void toggleSwitchOpenDC_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchOpenDC.IsOn ? "openCD-" : "openCD+");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void toggleSwitchLock_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchLock.IsOn ? "blankscreen-" : "blankscreen+");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void toggleSwitchCamera_Toggled(object sender, EventArgs e)
	{
		string asString = (toggleSwitchCamera.IsOn ? "webcamlight-" : "webcamlight+");
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = asString;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormFun));
		this.numericUpDown1 = new DevExpress.XtraEditors.SpinEdit();
		this.label2 = new System.Windows.Forms.Label();
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.label1 = new System.Windows.Forms.Label();
		this.numericUpDown2 = new DevExpress.XtraEditors.SpinEdit();
		this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.toggleSwitchTaskbar = new DevExpress.XtraEditors.ToggleSwitch();
		this.label5 = new System.Windows.Forms.Label();
		this.toggleSwitchDesktop = new DevExpress.XtraEditors.ToggleSwitch();
		this.label6 = new System.Windows.Forms.Label();
		this.toggleSwitchClock = new DevExpress.XtraEditors.ToggleSwitch();
		this.label7 = new System.Windows.Forms.Label();
		this.toggleSwitchMouse = new DevExpress.XtraEditors.ToggleSwitch();
		this.label8 = new System.Windows.Forms.Label();
		this.toggleSwitchOpenDC = new DevExpress.XtraEditors.ToggleSwitch();
		this.label9 = new System.Windows.Forms.Label();
		this.toggleSwitchLock = new DevExpress.XtraEditors.ToggleSwitch();
		this.label10 = new System.Windows.Forms.Label();
		this.toggleSwitchCamera = new DevExpress.XtraEditors.ToggleSwitch();
		this.label11 = new System.Windows.Forms.Label();
		this.btnOk = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
		this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
		this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchTaskbar.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchDesktop.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchClock.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchMouse.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchOpenDC.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchLock.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchCamera.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl1).BeginInit();
		this.groupControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl2).BeginInit();
		this.groupControl2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.numericUpDown1.EditValue = new decimal(new int[4]);
		this.numericUpDown1.Location = new System.Drawing.Point(39, 37);
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown1.Properties.Appearance.Options.UseFont = true;
		this.numericUpDown1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numericUpDown1.Size = new System.Drawing.Size(89, 28);
		this.numericUpDown1.TabIndex = 27;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(135, 43);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(54, 16);
		this.label2.TabIndex = 2;
		this.label2.Text = "seconds";
		this.simpleButton3.Location = new System.Drawing.Point(202, 39);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.Size = new System.Drawing.Size(83, 26);
		this.simpleButton3.TabIndex = 26;
		this.simpleButton3.Text = "Start";
		this.simpleButton3.Click += new System.EventHandler(button11_Click);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(9, 43);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(24, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "for";
		this.numericUpDown2.EditValue = new decimal(new int[4]);
		this.numericUpDown2.Location = new System.Drawing.Point(39, 39);
		this.numericUpDown2.Name = "numericUpDown2";
		this.numericUpDown2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown2.Properties.Appearance.Options.UseFont = true;
		this.numericUpDown2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numericUpDown2.Size = new System.Drawing.Size(89, 28);
		this.numericUpDown2.TabIndex = 28;
		this.simpleButton4.Location = new System.Drawing.Point(202, 41);
		this.simpleButton4.Name = "simpleButton4";
		this.simpleButton4.Size = new System.Drawing.Size(83, 26);
		this.simpleButton4.TabIndex = 27;
		this.simpleButton4.Text = "Start";
		this.simpleButton4.Click += new System.EventHandler(button15_Click);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(135, 46);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(54, 16);
		this.label3.TabIndex = 2;
		this.label3.Text = "seconds";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(9, 46);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(24, 16);
		this.label4.TabIndex = 1;
		this.label4.Text = "for";
		this.timer1.Interval = 2000;
		this.timer1.Tick += new System.EventHandler(Timer1_Tick);
		this.toggleSwitchTaskbar.Location = new System.Drawing.Point(146, 47);
		this.toggleSwitchTaskbar.Name = "toggleSwitchTaskbar";
		this.toggleSwitchTaskbar.Properties.OffText = "Off";
		this.toggleSwitchTaskbar.Properties.OnText = "On";
		this.toggleSwitchTaskbar.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchTaskbar.TabIndex = 10;
		this.toggleSwitchTaskbar.Toggled += new System.EventHandler(toggleSwitchTaskbar_Toggled);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(23, 53);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(83, 16);
		this.label5.TabIndex = 1;
		this.label5.Text = "Hide Taskbar";
		this.toggleSwitchDesktop.Location = new System.Drawing.Point(146, 94);
		this.toggleSwitchDesktop.Name = "toggleSwitchDesktop";
		this.toggleSwitchDesktop.Properties.OffText = "Off";
		this.toggleSwitchDesktop.Properties.OnText = "On";
		this.toggleSwitchDesktop.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchDesktop.TabIndex = 12;
		this.toggleSwitchDesktop.Toggled += new System.EventHandler(toggleSwitchDesktop_Toggled);
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(23, 99);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(82, 16);
		this.label6.TabIndex = 11;
		this.label6.Text = "Hide Desktop";
		this.toggleSwitchClock.Location = new System.Drawing.Point(146, 143);
		this.toggleSwitchClock.Name = "toggleSwitchClock";
		this.toggleSwitchClock.Properties.OffText = "Off";
		this.toggleSwitchClock.Properties.OnText = "On";
		this.toggleSwitchClock.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchClock.TabIndex = 14;
		this.toggleSwitchClock.Toggled += new System.EventHandler(toggleSwitchClock_Toggled);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(23, 149);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 16);
		this.label7.TabIndex = 13;
		this.label7.Text = "Hide Clock";
		this.toggleSwitchMouse.Location = new System.Drawing.Point(146, 194);
		this.toggleSwitchMouse.Name = "toggleSwitchMouse";
		this.toggleSwitchMouse.Properties.OffText = "Off";
		this.toggleSwitchMouse.Properties.OnText = "On";
		this.toggleSwitchMouse.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchMouse.TabIndex = 16;
		this.toggleSwitchMouse.Toggled += new System.EventHandler(toggleSwitchMouse_Toggled);
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(23, 199);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(81, 16);
		this.label8.TabIndex = 15;
		this.label8.Text = "Swap Mouse";
		this.toggleSwitchOpenDC.Location = new System.Drawing.Point(146, 245);
		this.toggleSwitchOpenDC.Name = "toggleSwitchOpenDC";
		this.toggleSwitchOpenDC.Properties.OffText = "Off";
		this.toggleSwitchOpenDC.Properties.OnText = "On";
		this.toggleSwitchOpenDC.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchOpenDC.TabIndex = 18;
		this.toggleSwitchOpenDC.Toggled += new System.EventHandler(toggleSwitchOpenDC_Toggled);
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(23, 249);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(91, 16);
		this.label9.TabIndex = 17;
		this.label9.Text = "Open CD Drive";
		this.toggleSwitchLock.Location = new System.Drawing.Point(146, 294);
		this.toggleSwitchLock.Name = "toggleSwitchLock";
		this.toggleSwitchLock.Properties.OffText = "Off";
		this.toggleSwitchLock.Properties.OnText = "On";
		this.toggleSwitchLock.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchLock.TabIndex = 20;
		this.toggleSwitchLock.Toggled += new System.EventHandler(toggleSwitchLock_Toggled);
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(23, 298);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 16);
		this.label10.TabIndex = 19;
		this.label10.Text = "Lock Screen";
		this.toggleSwitchCamera.Location = new System.Drawing.Point(146, 342);
		this.toggleSwitchCamera.Name = "toggleSwitchCamera";
		this.toggleSwitchCamera.Properties.OffText = "Off";
		this.toggleSwitchCamera.Properties.OnText = "On";
		this.toggleSwitchCamera.Size = new System.Drawing.Size(119, 24);
		this.toggleSwitchCamera.TabIndex = 22;
		this.toggleSwitchCamera.Toggled += new System.EventHandler(toggleSwitchCamera_Toggled);
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(23, 346);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(114, 16);
		this.label11.TabIndex = 21;
		this.label11.Text = "Web Camera Light";
		this.btnOk.Location = new System.Drawing.Point(288, 282);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(290, 40);
		this.btnOk.TabIndex = 23;
		this.btnOk.Text = "Play Audio";
		this.btnOk.Click += new System.EventHandler(button13_Click_1);
		this.simpleButton1.Location = new System.Drawing.Point(288, 235);
		this.simpleButton1.Name = "simpleButton1";
		this.simpleButton1.Size = new System.Drawing.Size(290, 40);
		this.simpleButton1.TabIndex = 24;
		this.simpleButton1.Text = "Turn Off Monitor";
		this.simpleButton1.Click += new System.EventHandler(button12_Click);
		this.simpleButton2.Location = new System.Drawing.Point(288, 328);
		this.simpleButton2.Name = "simpleButton2";
		this.simpleButton2.Size = new System.Drawing.Size(290, 40);
		this.simpleButton2.TabIndex = 25;
		this.simpleButton2.Text = "Hang System";
		this.simpleButton2.Click += new System.EventHandler(button14_Click);
		this.groupControl1.AutoSize = true;
		this.groupControl1.Controls.Add(this.numericUpDown1);
		this.groupControl1.Controls.Add(this.label1);
		this.groupControl1.Controls.Add(this.label2);
		this.groupControl1.Controls.Add(this.simpleButton3);
		this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupControl1.Location = new System.Drawing.Point(286, 30);
		this.groupControl1.Name = "groupControl1";
		this.groupControl1.Size = new System.Drawing.Size(292, 91);
		this.groupControl1.TabIndex = 27;
		this.groupControl1.Text = "Block Input";
		this.groupControl2.AutoSize = true;
		this.groupControl2.Controls.Add(this.numericUpDown2);
		this.groupControl2.Controls.Add(this.label4);
		this.groupControl2.Controls.Add(this.simpleButton4);
		this.groupControl2.Controls.Add(this.label3);
		this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupControl2.Location = new System.Drawing.Point(286, 130);
		this.groupControl2.Name = "groupControl2";
		this.groupControl2.Size = new System.Drawing.Size(292, 91);
		this.groupControl2.TabIndex = 27;
		this.groupControl2.Text = "Hold Mouse";
		this.xtraTabControl1.Location = new System.Drawing.Point(14, 15);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(602, 431);
		this.xtraTabControl1.TabIndex = 28;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.label5);
		this.xtraTabPage1.Controls.Add(this.groupControl2);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchTaskbar);
		this.xtraTabPage1.Controls.Add(this.groupControl1);
		this.xtraTabPage1.Controls.Add(this.label6);
		this.xtraTabPage1.Controls.Add(this.simpleButton2);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchDesktop);
		this.xtraTabPage1.Controls.Add(this.simpleButton1);
		this.xtraTabPage1.Controls.Add(this.label7);
		this.xtraTabPage1.Controls.Add(this.btnOk);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchClock);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchCamera);
		this.xtraTabPage1.Controls.Add(this.label8);
		this.xtraTabPage1.Controls.Add(this.label11);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchMouse);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchLock);
		this.xtraTabPage1.Controls.Add(this.label9);
		this.xtraTabPage1.Controls.Add(this.label10);
		this.xtraTabPage1.Controls.Add(this.toggleSwitchOpenDC);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(600, 400);
		this.xtraTabPage1.Text = "Fun Function";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(630, 464);
		base.Controls.Add(this.xtraTabControl1);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormFun.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormFun.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.MaximumSize = new System.Drawing.Size(632, 498);
		this.MinimumSize = new System.Drawing.Size(632, 498);
		base.Name = "FormFun";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fun";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormFun_FormClosed);
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchTaskbar.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchDesktop.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchClock.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchMouse.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchOpenDC.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchLock.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.toggleSwitchCamera.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl1).EndInit();
		this.groupControl1.ResumeLayout(false);
		this.groupControl1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl2).EndInit();
		this.groupControl2.ResumeLayout(false);
		this.groupControl2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		base.ResumeLayout(false);
	}
}
