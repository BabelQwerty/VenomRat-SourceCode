using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;

namespace Server.Forms;

public class FormInputString : XtraForm
{
	private IContainer components;

	private SimpleButton simpleButton3;

	private SimpleButton simpleButton1;

	private TextEdit txtResult;

	private XtraTabControl xtraTabControl2;

	private XtraTabPage xtraTabPage2;

	public string note
	{
		get
		{
			return txtResult.Text;
		}
		set
		{
			txtResult.Text = value;
		}
	}

	public FormInputString()
	{
		InitializeComponent();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormInputString));
		this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		this.txtResult = new DevExpress.XtraEditors.TextEdit();
		this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
		this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
		((System.ComponentModel.ISupportInitialize)this.txtResult.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
		this.xtraTabControl2.SuspendLayout();
		this.xtraTabPage2.SuspendLayout();
		base.SuspendLayout();
		this.simpleButton3.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.simpleButton3.Location = new System.Drawing.Point(57, 72);
		this.simpleButton3.Name = "simpleButton3";
		this.simpleButton3.Size = new System.Drawing.Size(83, 26);
		this.simpleButton3.TabIndex = 27;
		this.simpleButton3.Text = "OK";
		this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.simpleButton1.Location = new System.Drawing.Point(195, 72);
		this.simpleButton1.Name = "simpleButton1";
		this.simpleButton1.Size = new System.Drawing.Size(83, 26);
		this.simpleButton1.TabIndex = 28;
		this.simpleButton1.Text = "Cancel";
		this.txtResult.Location = new System.Drawing.Point(40, 27);
		this.txtResult.Name = "txtResult";
		this.txtResult.Size = new System.Drawing.Size(273, 30);
		this.txtResult.TabIndex = 29;
		this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
		this.xtraTabControl2.MultiLine = DevExpress.Utils.DefaultBoolean.True;
		this.xtraTabControl2.Name = "xtraTabControl2";
		this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
		this.xtraTabControl2.Size = new System.Drawing.Size(351, 171);
		this.xtraTabControl2.TabIndex = 30;
		this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[1] { this.xtraTabPage2 });
		this.xtraTabPage2.Controls.Add(this.txtResult);
		this.xtraTabPage2.Controls.Add(this.simpleButton3);
		this.xtraTabPage2.Controls.Add(this.simpleButton1);
		this.xtraTabPage2.Name = "xtraTabPage2";
		this.xtraTabPage2.Size = new System.Drawing.Size(349, 140);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(351, 171);
		base.Controls.Add(this.xtraTabControl2);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.IconOptions.Image = (System.Drawing.Image)resources.GetObject("FormInputString.IconOptions.Image");
		base.Margin = new System.Windows.Forms.Padding(4);
		this.MaximumSize = new System.Drawing.Size(353, 205);
		this.MinimumSize = new System.Drawing.Size(353, 205);
		base.Name = "FormInputString";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Please input note...";
		((System.ComponentModel.ISupportInitialize)this.txtResult.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
		this.xtraTabControl2.ResumeLayout(false);
		this.xtraTabPage2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
