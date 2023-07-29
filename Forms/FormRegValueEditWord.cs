using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using Microsoft.Win32;
using Server.Helper;

namespace Server.Forms;

public class FormRegValueEditWord : XtraForm
{
	private readonly RegistrySeeker.RegValueData _value;

	private const string DWORD_WARNING = "The decimal value entered is greater than the maximum value of a DWORD (32-bit number). Should the value be truncated in order to continue?";

	private const string QWORD_WARNING = "The decimal value entered is greater than the maximum value of a QWORD (64-bit number). Should the value be truncated in order to continue?";

	private IContainer components;

	private Label label2;

	private Label label1;

	private WordTextBox valueDataTxtBox;

	private SimpleButton cancelBtn;

	private SimpleButton okBtn;

	private TextEdit valueNameTxtBox;

	private RadioGroup radioGroupBase;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormRegValueEditWord(RegistrySeeker.RegValueData value)
	{
		_value = value;
		InitializeComponent();
		valueNameTxtBox.Text = value.Name;
		if (value.Kind == RegistryValueKind.DWord)
		{
			Text = "Edit DWORD (32-bit) Value";
			valueDataTxtBox.Type = WordTextBox.WordType.DWORD;
			valueDataTxtBox.Text = Server.Helper.ByteConverter.ToUInt32(value.Data).ToString("x");
		}
		else
		{
			Text = "Edit QWORD (64-bit) Value";
			valueDataTxtBox.Type = WordTextBox.WordType.QWORD;
			valueDataTxtBox.Text = Server.Helper.ByteConverter.ToUInt64(value.Data).ToString("x");
		}
	}

	private void okButton_Click(object sender, EventArgs e)
	{
		if (valueDataTxtBox.IsConversionValid() || IsOverridePossible())
		{
			_value.Data = ((_value.Kind == RegistryValueKind.DWord) ? Server.Helper.ByteConverter.GetBytes(valueDataTxtBox.UIntValue) : Server.Helper.ByteConverter.GetBytes(valueDataTxtBox.ULongValue));
			base.Tag = _value;
			base.DialogResult = DialogResult.OK;
		}
		else
		{
			base.DialogResult = DialogResult.None;
		}
		Close();
	}

	private DialogResult ShowWarning(string msg, string caption)
	{
		return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
	}

	private bool IsOverridePossible()
	{
		string msg = ((_value.Kind == RegistryValueKind.DWord) ? "The decimal value entered is greater than the maximum value of a DWORD (32-bit number). Should the value be truncated in order to continue?" : "The decimal value entered is greater than the maximum value of a QWORD (64-bit number). Should the value be truncated in order to continue?");
		return ShowWarning(msg, "Overflow") == DialogResult.Yes;
	}

	private void radioGroupBase_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (valueDataTxtBox.IsHexNumber != (Convert.ToInt32(radioGroupBase.EditValue) == 16))
		{
			if (valueDataTxtBox.IsConversionValid() || IsOverridePossible())
			{
				valueDataTxtBox.IsHexNumber = Convert.ToInt32(radioGroupBase.EditValue) == 16;
			}
			else
			{
				radioGroupBase.EditValue = 10;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormRegValueEditWord));
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cancelBtn = new DevExpress.XtraEditors.SimpleButton();
		this.okBtn = new DevExpress.XtraEditors.SimpleButton();
		this.valueDataTxtBox = new Server.Helper.WordTextBox();
		this.valueNameTxtBox = new DevExpress.XtraEditors.TextEdit();
		this.radioGroupBase = new DevExpress.XtraEditors.RadioGroup();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.valueDataTxtBox.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.radioGroupBase.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(13, 65);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 13);
		this.label2.TabIndex = 15;
		this.label2.Text = "Value data:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(13, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(66, 13);
		this.label1.TabIndex = 16;
		this.label1.Text = "Value name:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelBtn.Location = new System.Drawing.Point(105, 131);
		this.cancelBtn.Name = "cancelBtn";
		this.cancelBtn.Size = new System.Drawing.Size(83, 26);
		this.cancelBtn.TabIndex = 30;
		this.cancelBtn.Text = "Cancel";
		this.okBtn.Location = new System.Drawing.Point(17, 131);
		this.okBtn.Name = "okBtn";
		this.okBtn.Size = new System.Drawing.Size(83, 26);
		this.okBtn.TabIndex = 29;
		this.okBtn.Text = "OK";
		this.okBtn.Click += new System.EventHandler(okButton_Click);
		this.valueDataTxtBox.IsHexNumber = false;
		this.valueDataTxtBox.Location = new System.Drawing.Point(17, 83);
		this.valueDataTxtBox.MaxLength = 8;
		this.valueDataTxtBox.Name = "valueDataTxtBox";
		this.valueDataTxtBox.Properties.MaxLength = 8;
		this.valueDataTxtBox.Size = new System.Drawing.Size(171, 28);
		this.valueDataTxtBox.TabIndex = 17;
		this.valueDataTxtBox.Type = Server.Helper.WordTextBox.WordType.DWORD;
		this.valueNameTxtBox.Location = new System.Drawing.Point(17, 35);
		this.valueNameTxtBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.valueNameTxtBox.Name = "valueNameTxtBox";
		this.valueNameTxtBox.Size = new System.Drawing.Size(333, 28);
		this.valueNameTxtBox.TabIndex = 31;
		this.radioGroupBase.EditValue = 16;
		this.radioGroupBase.Location = new System.Drawing.Point(217, 72);
		this.radioGroupBase.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.radioGroupBase.Name = "radioGroupBase";
		this.radioGroupBase.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
		this.radioGroupBase.Properties.Appearance.Options.UseBackColor = true;
		this.radioGroupBase.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[2]
		{
			new DevExpress.XtraEditors.Controls.RadioGroupItem(16, "Hex"),
			new DevExpress.XtraEditors.Controls.RadioGroupItem(10, "Dec")
		});
		this.radioGroupBase.Properties.NullText = "Base";
		this.radioGroupBase.Size = new System.Drawing.Size(143, 85);
		this.radioGroupBase.TabIndex = 32;
		this.radioGroupBase.SelectedIndexChanged += new System.EventHandler(radioGroupBase_SelectedIndexChanged);
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(379, 207);
		this.xtraTabControl1.TabIndex = 33;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.label1);
		this.xtraTabPage1.Controls.Add(this.radioGroupBase);
		this.xtraTabPage1.Controls.Add(this.label2);
		this.xtraTabPage1.Controls.Add(this.valueNameTxtBox);
		this.xtraTabPage1.Controls.Add(this.valueDataTxtBox);
		this.xtraTabPage1.Controls.Add(this.cancelBtn);
		this.xtraTabPage1.Controls.Add(this.okBtn);
		this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(377, 176);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(379, 207);
		base.Controls.Add(this.xtraTabControl1);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormRegValueEditWord.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormRegValueEditWord.IconOptions.Image");
		base.Name = "FormRegValueEditWord";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "FormRegValueEditWord";
		((System.ComponentModel.ISupportInitialize)this.valueDataTxtBox.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.valueNameTxtBox.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.radioGroupBase.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		base.ResumeLayout(false);
	}
}
