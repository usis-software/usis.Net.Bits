namespace usis.PushNotification.Administration
{
	partial class ApnsNewChannelDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApnsNewChannelDialog));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.textBoxBundleId = new System.Windows.Forms.TextBox();
            this.labelBundleId = new System.Windows.Forms.Label();
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
            // textBoxBundleId
            // 
            resources.ApplyResources(this.textBoxBundleId, "textBoxBundleId");
            this.textBoxBundleId.Name = "textBoxBundleId";
            this.textBoxBundleId.TextChanged += new System.EventHandler(this.Changed);
            // 
            // labelBundleId
            // 
            resources.ApplyResources(this.labelBundleId, "labelBundleId");
            this.labelBundleId.Name = "labelBundleId";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.labelBundleId, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxBundleId, 1, 0);
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
            // ApnsNewChannelDialog
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApnsNewChannelDialog";
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
		private System.Windows.Forms.TextBox textBoxBundleId;
		private System.Windows.Forms.Label labelBundleId;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.ComboBox comboBoxEnvironment;
		private System.Windows.Forms.Label labelEnvironment;
	}
}