using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using Server.Properties;

namespace Server.Forms;

public class FormSetting : XtraForm
{
	private IContainer components;

	private Label label3;

	private Label label4;

	private CheckEdit checkBoxTelegram;

	private SimpleButton simpleButton3;

	private TextEdit textBoxTgHook;

	private TextEdit textBoxTgchatID;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormSetting()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if ((checkBoxTelegram.Checked && string.IsNullOrEmpty(textBoxTgHook.Text)) || string.IsNullOrEmpty(textBoxTgchatID.Text))
		{
			MessageBox.Show("Input Telegram Hook and ChatID");
		}
		Server.Properties.Settings.Default.TelegramEnabled = checkBoxTelegram.Checked;
		Server.Properties.Settings.Default.TelegramToken = textBoxTgHook.Text;
		Server.Properties.Settings.Default.TelegramChatId = textBoxTgchatID.Text;
		Server.Properties.Settings.Default.Save();
		Close();
	}

	private void FormSetting_Load(object sender, EventArgs e)
	{
		checkBoxTelegram.Checked = Server.Properties.Settings.Default.TelegramEnabled;
		textBoxTgHook.Text = Server.Properties.Settings.Default.TelegramToken;
		textBoxTgchatID.Text = Server.Properties.Settings.Default.TelegramChatId;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormSetting));
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.checkBoxTelegram = new DevExpress.XtraEditors.CheckEdit();
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.textBoxTgHook = new DevExpress.XtraEditors.TextEdit();
		this.textBoxTgchatID = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.checkBoxTelegram.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textBoxTgHook.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textBoxTgchatID.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(53, 72);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 16);
		this.label3.TabIndex = 4;
		this.label3.Text = "Token:";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(53, 116);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 16);
		this.label4.TabIndex = 5;
		this.label4.Text = "ChatID:";
		this.checkBoxTelegram.Location = new System.Drawing.Point(53, 31);
		this.checkBoxTelegram.Margin = new System.Windows.Forms.Padding(4);
		this.checkBoxTelegram.Name = "checkBoxTelegram";
		this.checkBoxTelegram.Properties.Caption = "Telegram";
		this.checkBoxTelegram.Size = new System.Drawing.Size(88, 22);
		this.checkBoxTelegram.TabIndex = 6;
		this.simpleButton3.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.simpleButton3.Location = new System.Drawing.Point(107, 154);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.Size = new System.Drawing.Size(320, 30);
		this.simpleButton3.TabIndex = 30;
		this.simpleButton3.Text = "Apply";
		this.simpleButton3.Click += new System.EventHandler(button1_Click);
		this.textBoxTgHook.Location = new System.Drawing.Point(107, 65);
		this.textBoxTgHook.Name = "textBoxTgHook";
		this.textBoxTgHook.Size = new System.Drawing.Size(320, 30);
		this.textBoxTgHook.TabIndex = 31;
		this.textBoxTgchatID.Location = new System.Drawing.Point(107, 109);
		this.textBoxTgchatID.Name = "textBoxTgchatID";
		this.textBoxTgchatID.Size = new System.Drawing.Size(320, 30);
		this.textBoxTgchatID.TabIndex = 32;
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(488, 254);
		this.xtraTabControl2.TabIndex = 33;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.checkBoxTelegram);
		this.xtraTabPage2.Controls.Add(this.textBoxTgchatID);
		this.xtraTabPage2.Controls.Add(this.label3);
		this.xtraTabPage2.Controls.Add(this.textBoxTgHook);
		this.xtraTabPage2.Controls.Add(this.label4);
		this.xtraTabPage2.Controls.Add(this.simpleButton3);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(486, 223);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(488, 254);
		base.Controls.Add(this.xtraTabControl2);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormSetting.IconOptions.Icon");
		base.IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("FormSetting.IconOptions.SvgImage");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(490, 288);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(490, 288);
		base.Name = "FormSetting";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Setting";
		base.Load += new System.EventHandler(FormSetting_Load);
		((System.ComponentModel.ISupportInitialize)this.checkBoxTelegram.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textBoxTgHook.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textBoxTgchatID.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		this.xtraTabPage2.PerformLayout();
		base.ResumeLayout(false);
	}
}
