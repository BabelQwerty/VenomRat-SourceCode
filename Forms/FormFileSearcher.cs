using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;

namespace Server.Forms;

public class FormFileSearcher : XtraForm
{
	private IContainer components;

	private Label label1;

	private Label label2;

	private Label label3;

	private SimpleButton btnOk;

	public TextEdit txtExtnsions;

	public SpinEdit numericUpDown1;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public FormFileSearcher()
	{
		InitializeComponent();
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(txtExtnsions.Text) && numericUpDown1.Value > 0m)
		{
			base.DialogResult = DialogResult.OK;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormFileSearcher));
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnOk = new DevExpress.XtraEditors.SimpleButton();
		this.txtExtnsions = new DevExpress.XtraEditors.TextEdit();
		this.numericUpDown1 = new DevExpress.XtraEditors.SpinEdit();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.txtExtnsions.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(17, 18);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(41, 16);
		this.label1.TabIndex = 0;
		this.label1.Text = "Type:";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(17, 86);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 16);
		this.label2.TabIndex = 3;
		this.label2.Text = "Max size:";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f);
		this.label3.Location = new System.Drawing.Point(133, 116);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(23, 13);
		this.label3.TabIndex = 6;
		this.label3.Text = "MB";
		this.btnOk.Location = new System.Drawing.Point(312, 101);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(131, 35);
		this.btnOk.TabIndex = 7;
		this.btnOk.Text = "OK";
		this.btnOk.Click += new System.EventHandler(btnOk_Click);
		this.txtExtnsions.EditValue = ".txt .pdf .doc";
		this.txtExtnsions.Location = new System.Drawing.Point(20, 40);
		this.txtExtnsions.Name = "txtExtnsions";
		this.txtExtnsions.Size = new System.Drawing.Size(423, 30);
		this.txtExtnsions.TabIndex = 8;
		this.numericUpDown1.EditValue = new decimal(new int[4] { 5, 0, 0, 0 });
		this.numericUpDown1.Location = new System.Drawing.Point(20, 106);
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.numericUpDown1.Size = new System.Drawing.Size(100, 30);
		this.numericUpDown1.TabIndex = 10;
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(465, 207);
		this.xtraTabControl2.TabIndex = 11;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.txtExtnsions);
		this.xtraTabPage2.Controls.Add(this.numericUpDown1);
		this.xtraTabPage2.Controls.Add(this.label1);
		this.xtraTabPage2.Controls.Add(this.label2);
		this.xtraTabPage2.Controls.Add(this.btnOk);
		this.xtraTabPage2.Controls.Add(this.label3);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(463, 176);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(465, 207);
		base.Controls.Add(this.xtraTabControl2);
		this.DoubleBuffered = true;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FormFileSearcher.IconOptions.Icon");
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormFileSearcher.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(2);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(467, 241);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(467, 241);
		base.Name = "FormFileSearcher";
		base.ShowInTaskbar = false;
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "File Searcher";
		((System.ComponentModel.ISupportInitialize)this.txtExtnsions.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		this.xtraTabPage2.PerformLayout();
		base.ResumeLayout(false);
	}
}
