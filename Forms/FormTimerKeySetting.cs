using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace Server.Forms;

public class FormTimerKeySetting : XtraForm
{
	private IContainer components;

	private LabelControl labelControl1;

	private LabelControl labelControl4;

	private TextEdit txtFilter;

	private SpinEdit spinEditInterval;

	private LabelControl labelControl2;

	private SimpleButton btnApply;

	private SimpleButton simpleButton1;

	public int interval => (int)spinEditInterval.Value;

	public string filter => txtFilter.Text;

	public FormTimerKeySetting()
	{
		InitializeComponent();
	}

	private void FormTimerKeySetting_Load(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormTimerKeySetting));
		this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
		this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
		this.txtFilter = new DevExpress.XtraEditors.TextEdit();
		this.spinEditInterval = new DevExpress.XtraEditors.SpinEdit();
		this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
		this.btnApply = new DevExpress.XtraEditors.SimpleButton();
		this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
		((System.ComponentModel.ISupportInitialize)this.txtFilter.Properties).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.spinEditInterval.Properties).BeginInit();
		base.SuspendLayout();
		this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl1.Appearance.Options.UseFont = true;
		this.labelControl1.Location = new System.Drawing.Point(12, 13);
		this.labelControl1.Name = "labelControl1";
		this.labelControl1.Size = new System.Drawing.Size(52, 16);
		this.labelControl1.TabIndex = 61;
		this.labelControl1.Text = "Interval :";
		this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl4.Appearance.Options.UseFont = true;
		this.labelControl4.Location = new System.Drawing.Point(165, 13);
		this.labelControl4.Name = "labelControl4";
		this.labelControl4.Size = new System.Drawing.Size(16, 16);
		this.labelControl4.TabIndex = 63;
		this.labelControl4.Text = "(s)";
		this.txtFilter.Location = new System.Drawing.Point(12, 78);
		this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtFilter.Name = "txtFilter";
		this.txtFilter.Size = new System.Drawing.Size(346, 28);
		this.txtFilter.TabIndex = 59;
		this.spinEditInterval.EditValue = new decimal(new int[4] { 5, 0, 0, 0 });
		this.spinEditInterval.Location = new System.Drawing.Point(83, 8);
		this.spinEditInterval.Name = "spinEditInterval";
		this.spinEditInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
		{
			new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
		});
		this.spinEditInterval.Size = new System.Drawing.Size(76, 28);
		this.spinEditInterval.TabIndex = 60;
		this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelControl2.Appearance.Options.UseFont = true;
		this.labelControl2.Location = new System.Drawing.Point(12, 57);
		this.labelControl2.Name = "labelControl2";
		this.labelControl2.Size = new System.Drawing.Size(253, 16);
		this.labelControl2.TabIndex = 62;
		this.labelControl2.Text = "Filter String (ProcessName or Window Title)";
		this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btnApply.Location = new System.Drawing.Point(60, 125);
		this.btnApply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnApply.Name = "btnApply";
		this.btnApply.Size = new System.Drawing.Size(99, 32);
		this.btnApply.TabIndex = 64;
		this.btnApply.Text = "Ok";
		this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.simpleButton1.Location = new System.Drawing.Point(219, 125);
		this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.simpleButton1.Name = "simpleButton1";
		this.simpleButton1.Size = new System.Drawing.Size(99, 32);
		this.simpleButton1.TabIndex = 65;
		this.simpleButton1.Text = "Cancel";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(370, 175);
		base.Controls.Add(this.simpleButton1);
		base.Controls.Add(this.btnApply);
		base.Controls.Add(this.labelControl1);
		base.Controls.Add(this.labelControl4);
		base.Controls.Add(this.txtFilter);
		base.Controls.Add(this.spinEditInterval);
		base.Controls.Add(this.labelControl2);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("FormTimerKeySetting.IconOptions.SvgImage");
		base.Name = "FormTimerKeySetting";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Timer Keylogger Setting";
		base.Load += new System.EventHandler(FormTimerKeySetting_Load);
		((System.ComponentModel.ISupportInitialize)this.txtFilter.Properties).EndInit();
		((System.ComponentModel.ISupportInitialize)this.spinEditInterval.Properties).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
