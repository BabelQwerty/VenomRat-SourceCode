using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using Server.Helper;
using Server.Helper.HexEditor;

namespace Server.Forms;

public class FormRegValueEditBinary : XtraForm
{
	private readonly RegistrySeeker.RegValueData _value;

	private const string INVALID_BINARY_ERROR = "The binary value was invalid and could not be converted correctly.";

	private IContainer components;

	private Label label2;

	private Label label1;

	private SimpleButton simpleButton1;

	private SimpleButton simpleButton3;

	private TextEdit valueNameTxtBox;

	private HexEditor hexEditor;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormRegValueEditBinary(RegistrySeeker.RegValueData value)
	{
		_value = value;
		InitializeComponent();
		valueNameTxtBox.Text = RegValueHelper.GetName(value.Name);
		hexEditor.HexTable = value.Data;
	}

	private void okButton_Click(object sender, EventArgs e)
	{
		byte[] hexTable = hexEditor.HexTable;
		if (hexTable != null)
		{
			try
			{
				_value.Data = hexTable;
				base.DialogResult = DialogResult.OK;
				base.Tag = _value;
			}
			catch
			{
				ShowWarning("The binary value was invalid and could not be converted correctly.", "Warning");
				base.DialogResult = DialogResult.None;
			}
		}
		Close();
	}

	private void ShowWarning(string msg, string caption)
	{
		MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	}

	private void FormRegValueEditBinary_Load(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormRegValueEditBinary));
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.valueNameTxtBox = new DevExpress.XtraEditors.TextEdit();
		this.hexEditor = new Server.Helper.HexEditor.HexEditor();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(11, 64);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 13);
		this.label2.TabIndex = 7;
		this.label2.Text = "Value data:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(11, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(66, 13);
		this.label1.TabIndex = 9;
		this.label1.Text = "Value name:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.simpleButton1.Location = new System.Drawing.Point(289, 337);
		this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton1.Name = "simpleButton1";
		this.simpleButton1.Size = new System.Drawing.Size(122, 30);
		this.simpleButton1.TabIndex = 30;
		this.simpleButton1.Text = "Cancel";
		this.simpleButton3.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.simpleButton3.Location = new System.Drawing.Point(141, 337);
		this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.Size = new System.Drawing.Size(122, 30);
		this.simpleButton3.TabIndex = 29;
		this.simpleButton3.Text = "OK";
		this.valueNameTxtBox.Location = new System.Drawing.Point(14, 28);
		this.valueNameTxtBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.valueNameTxtBox.Name = "valueNameTxtBox";
		this.valueNameTxtBox.Size = new System.Drawing.Size(397, 28);
		this.valueNameTxtBox.TabIndex = 31;
		this.hexEditor.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.hexEditor.BorderColor = System.Drawing.Color.Empty;
		this.hexEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.hexEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.hexEditor.Location = new System.Drawing.Point(12, 81);
		this.hexEditor.Margin = new System.Windows.Forms.Padding(4);
		this.hexEditor.Name = "hexEditor";
		this.hexEditor.SelectionBackColor = System.Drawing.Color.Green;
		this.hexEditor.Size = new System.Drawing.Size(399, 250);
		this.hexEditor.TabIndex = 32;
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(428, 409);
		this.xtraTabControl1.TabIndex = 33;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.label1);
		this.xtraTabPage1.Controls.Add(this.hexEditor);
		this.xtraTabPage1.Controls.Add(this.label2);
		this.xtraTabPage1.Controls.Add(this.valueNameTxtBox);
		this.xtraTabPage1.Controls.Add(this.simpleButton3);
		this.xtraTabPage1.Controls.Add(this.simpleButton1);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(426, 378);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(428, 409);
		base.Controls.Add(this.xtraTabControl1);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormRegValueEditBinary.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormRegValueEditBinary.IconOptions.Image");
		this.MaximumSize = new System.Drawing.Size(430, 443);
		this.MinimumSize = new System.Drawing.Size(430, 443);
		base.Name = "FormRegValueEditBinary";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Reg Value Edit Binary";
		base.Load += new System.EventHandler(FormRegValueEditBinary_Load);
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		base.ResumeLayout(false);
	}
}
