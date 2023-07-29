using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraTab;
using Server.Helper;

namespace Server.Forms;

public class FormRegValueEditMultiString : XtraForm
{
	private readonly RegistrySeeker.RegValueData _value;

	private IContainer components;

	private Label label2;

	private Label label1;

	private SimpleButton simpleButton1;

	private SimpleButton simpleButton3;

	private TextEdit valueNameTxtBox;

	private RichEditControl valueDataTxtBox;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormRegValueEditMultiString(RegistrySeeker.RegValueData value)
	{
		_value = value;
		InitializeComponent();
		valueNameTxtBox.Text = value.Name;
		valueDataTxtBox.Text = string.Join("\r\n", Server.Helper.ByteConverter.ToStringArray(value.Data));
	}

	private void okButton_Click(object sender, EventArgs e)
	{
		_value.Data = Server.Helper.ByteConverter.GetBytes(valueDataTxtBox.Text.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
		base.Tag = _value;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void FormRegValueEditMultiString_Load(object sender, EventArgs e)
	{
	}

	private void richEditControl1_Click(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormRegValueEditMultiString));
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.valueNameTxtBox = new DevExpress.XtraEditors.TextEdit();
		this.valueDataTxtBox = new DevExpress.XtraRichEdit.RichEditControl();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(13, 84);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 13);
		this.label2.TabIndex = 8;
		this.label2.Text = "Value data:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(13, 36);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(66, 13);
		this.label1.TabIndex = 10;
		this.label1.Text = "Value name:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.simpleButton1.Location = new System.Drawing.Point(240, 297);
		this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton1.Name = "simpleButton1";
		this.simpleButton1.Size = new System.Drawing.Size(121, 30);
		this.simpleButton1.TabIndex = 30;
		this.simpleButton1.Text = "Cancel";
		this.simpleButton3.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.simpleButton3.Location = new System.Drawing.Point(104, 297);
		this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.Size = new System.Drawing.Size(121, 30);
		this.simpleButton3.TabIndex = 29;
		this.simpleButton3.Text = "OK";
		this.valueNameTxtBox.Location = new System.Drawing.Point(16, 51);
		this.valueNameTxtBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.valueNameTxtBox.Name = "valueNameTxtBox";
		this.valueNameTxtBox.Size = new System.Drawing.Size(345, 28);
		this.valueNameTxtBox.TabIndex = 31;
		this.valueDataTxtBox.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
		this.valueDataTxtBox.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
		this.valueDataTxtBox.Location = new System.Drawing.Point(19, 99);
		this.valueDataTxtBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.valueDataTxtBox.Name = "valueDataTxtBox";
		this.valueDataTxtBox.Size = new System.Drawing.Size(343, 185);
		this.valueDataTxtBox.TabIndex = 33;
		this.valueDataTxtBox.Click += new System.EventHandler(richEditControl1_Click);
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(377, 372);
		this.xtraTabControl1.TabIndex = 34;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.label1);
		this.xtraTabPage1.Controls.Add(this.valueDataTxtBox);
		this.xtraTabPage1.Controls.Add(this.label2);
		this.xtraTabPage1.Controls.Add(this.valueNameTxtBox);
		this.xtraTabPage1.Controls.Add(this.simpleButton3);
		this.xtraTabPage1.Controls.Add(this.simpleButton1);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(375, 341);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(377, 372);
		base.Controls.Add(this.xtraTabControl1);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormRegValueEditMultiString.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormRegValueEditMultiString.IconOptions.Image");
		this.MaximumSize = new System.Drawing.Size(379, 406);
		this.MinimumSize = new System.Drawing.Size(379, 406);
		base.Name = "FormRegValueEditMultiString";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Reg Value Edit Multi String";
		base.Load += new System.EventHandler(FormRegValueEditMultiString_Load);
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		base.ResumeLayout(false);
	}
}
