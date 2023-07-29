using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using Server.Helper;

namespace Server.Forms;

public class FormRegValueEditString : XtraForm
{
	private readonly RegistrySeeker.RegValueData _value;

	private IContainer components;

	private Label label2;

	private Label label1;

	private SimpleButton cancelBtn;

	private SimpleButton okBtn;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	private TextEdit valueDataTxtBox;

	private TextEdit valueNameTxtBox;

	public FormRegValueEditString(RegistrySeeker.RegValueData value)
	{
		_value = value;
		InitializeComponent();
		valueNameTxtBox.Text = RegValueHelper.GetName(value.Name);
		valueDataTxtBox.Text = Server.Helper.ByteConverter.ToString(value.Data);
	}

	private void okButton_Click(object sender, EventArgs e)
	{
		_value.Data = Server.Helper.ByteConverter.GetBytes(valueDataTxtBox.Text);
		base.Tag = _value;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void FormRegValueEditString_Load(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormRegValueEditString));
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cancelBtn = new DevExpress.XtraEditors.SimpleButton();
		this.okBtn = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		this.valueDataTxtBox = new DevExpress.XtraEditors.TextEdit();
		this.valueNameTxtBox = new DevExpress.XtraEditors.TextEdit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.valueDataTxtBox.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).BeginInit();
		base.SuspendLayout();
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(24, 96);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(74, 16);
		this.label2.TabIndex = 8;
		this.label2.Text = "Value data:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(24, 32);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 16);
		this.label1.TabIndex = 10;
		this.label1.Text = "Value name:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelBtn.Location = new System.Drawing.Point(339, 152);
		this.cancelBtn.Name = "cancelBtn";
		this.cancelBtn.Size = new System.Drawing.Size(123, 30);
		this.cancelBtn.TabIndex = 30;
		this.cancelBtn.Text = "Cancel";
		this.okBtn.Location = new System.Drawing.Point(201, 152);
		this.okBtn.Name = "okBtn";
		this.okBtn.Size = new System.Drawing.Size(123, 30);
		this.okBtn.TabIndex = 29;
		this.okBtn.Text = "OK";
		this.okBtn.Click += new System.EventHandler(okButton_Click);
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(501, 253);
		this.xtraTabControl2.TabIndex = 31;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.valueDataTxtBox);
		this.xtraTabPage2.Controls.Add(this.label1);
		this.xtraTabPage2.Controls.Add(this.valueNameTxtBox);
		this.xtraTabPage2.Controls.Add(this.label2);
		this.xtraTabPage2.Controls.Add(this.okBtn);
		this.xtraTabPage2.Controls.Add(this.cancelBtn);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(499, 222);
		this.valueDataTxtBox.Location = new System.Drawing.Point(28, 116);
		this.valueDataTxtBox.Name = "valueDataTxtBox";
		this.valueDataTxtBox.Size = new System.Drawing.Size(434, 30);
		this.valueDataTxtBox.TabIndex = 33;
		this.valueNameTxtBox.Location = new System.Drawing.Point(28, 61);
		this.valueNameTxtBox.Name = "valueNameTxtBox";
		this.valueNameTxtBox.Size = new System.Drawing.Size(434, 30);
		this.valueNameTxtBox.TabIndex = 32;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(501, 253);
		base.Controls.Add(this.xtraTabControl2);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormRegValueEditString.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormRegValueEditString.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "FormRegValueEditString";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "FormRegValueEditString";
		base.Load += new System.EventHandler(FormRegValueEditString_Load);
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		this.xtraTabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.valueDataTxtBox.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).EndInit();
		base.ResumeLayout(false);
	}
}
