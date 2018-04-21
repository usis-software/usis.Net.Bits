namespace usis.PushNotification.Administration
{
	partial class WnsNewChannelDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WnsNewChannelDialog));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.textBoxPackageSid = new System.Windows.Forms.TextBox();
            this.labelPackageSid = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxEnvironment = new System.Windows.Forms.ComboBox();
            this.labelEnvironment = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonOK);
            this.panelButtons.Controls.Add(this.buttonCancel);
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.Name = "panelButtons";
            // 
            // textBoxPackageSid
            // 
            resources.ApplyResources(this.textBoxPackageSid, "textBoxPackageSid");
            this.textBoxPackageSid.Name = "textBoxPackageSid";
            this.textBoxPackageSid.TextChanged += new System.EventHandler(this.Changed);
            // 
            // labelPackageSid
            // 
            resources.ApplyResources(this.labelPackageSid, "labelPackageSid");
            this.labelPackageSid.Name = "labelPackageSid";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.labelPackageSid, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxPackageSid, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.panelButtons, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxEnvironment, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelEnvironment, 0, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // comboBoxEnvironment
            // 
            this.comboBoxEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEnvironment.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxEnvironment, "comboBoxEnvironment");
            this.comboBoxEnvironment.Name = "comboBoxEnvironment";
            // 
            // labelEnvironment
            // 
            resources.ApplyResources(this.labelEnvironment, "labelEnvironment");
            this.labelEnvironment.Name = "labelEnvironment";
            // 
            // WnsNewChannelDialog
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WnsNewChannelDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panelButtons.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Panel panelButtons;
		private System.Windows.Forms.TextBox textBoxPackageSid;
		private System.Windows.Forms.Label labelPackageSid;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.ComboBox comboBoxEnvironment;
		private System.Windows.Forms.Label labelEnvironment;
	}
}