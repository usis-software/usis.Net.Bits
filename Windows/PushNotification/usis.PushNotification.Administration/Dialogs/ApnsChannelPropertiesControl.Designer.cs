namespace usis.PushNotification.Administration
{
	partial class ApnsChannelPropertiesControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApnsChannelPropertiesControl));
            this.labelBundleId = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelEnvironment = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxBundleId = new System.Windows.Forms.TextBox();
            this.textBoxEnvironment = new System.Windows.Forms.TextBox();
            this.labelCertificate = new System.Windows.Forms.Label();
            this.textBoxCertificate = new System.Windows.Forms.TextBox();
            this.labelThumbprint = new System.Windows.Forms.Label();
            this.textBoxThumbprint = new System.Windows.Forms.TextBox();
            this.labelCreated = new System.Windows.Forms.Label();
            this.labelChanged = new System.Windows.Forms.Label();
            this.textBoxCreated = new System.Windows.Forms.TextBox();
            this.textBoxChanged = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelBundleId
            // 
            resources.ApplyResources(this.labelBundleId, "labelBundleId");
            this.labelBundleId.Name = "labelBundleId";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelBundleId, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelEnvironment, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxBundleId, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxEnvironment, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCertificate, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxCertificate, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.labelThumbprint, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxThumbprint, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelCreated, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.labelChanged, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.textBoxCreated, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.textBoxChanged, 1, 6);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.TextChanged += new System.EventHandler(this.DescriptionChanged);
            // 
            // labelEnvironment
            // 
            resources.ApplyResources(this.labelEnvironment, "labelEnvironment");
            this.labelEnvironment.Name = "labelEnvironment";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            // 
            // textBoxBundleId
            // 
            this.textBoxBundleId.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxBundleId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxBundleId, "textBoxBundleId");
            this.textBoxBundleId.Name = "textBoxBundleId";
            this.textBoxBundleId.ReadOnly = true;
            // 
            // textBoxEnvironment
            // 
            this.textBoxEnvironment.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxEnvironment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxEnvironment, "textBoxEnvironment");
            this.textBoxEnvironment.Name = "textBoxEnvironment";
            this.textBoxEnvironment.ReadOnly = true;
            // 
            // labelCertificate
            // 
            resources.ApplyResources(this.labelCertificate, "labelCertificate");
            this.labelCertificate.Name = "labelCertificate";
            // 
            // textBoxCertificate
            // 
            this.textBoxCertificate.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxCertificate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxCertificate, "textBoxCertificate");
            this.textBoxCertificate.Name = "textBoxCertificate";
            this.textBoxCertificate.ReadOnly = true;
            // 
            // labelThumbprint
            // 
            resources.ApplyResources(this.labelThumbprint, "labelThumbprint");
            this.labelThumbprint.Name = "labelThumbprint";
            // 
            // textBoxThumbprint
            // 
            this.textBoxThumbprint.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxThumbprint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxThumbprint, "textBoxThumbprint");
            this.textBoxThumbprint.Name = "textBoxThumbprint";
            this.textBoxThumbprint.ReadOnly = true;
            // 
            // labelCreated
            // 
            resources.ApplyResources(this.labelCreated, "labelCreated");
            this.labelCreated.Name = "labelCreated";
            // 
            // labelChanged
            // 
            resources.ApplyResources(this.labelChanged, "labelChanged");
            this.labelChanged.Name = "labelChanged";
            // 
            // textBoxCreated
            // 
            this.textBoxCreated.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxCreated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxCreated, "textBoxCreated");
            this.textBoxCreated.Name = "textBoxCreated";
            this.textBoxCreated.ReadOnly = true;
            // 
            // textBoxChanged
            // 
            this.textBoxChanged.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxChanged.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxChanged, "textBoxChanged");
            this.textBoxChanged.Name = "textBoxChanged";
            this.textBoxChanged.ReadOnly = true;
            // 
            // ApnsChannelPropertiesControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ApnsChannelPropertiesControl";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelBundleId;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label labelEnvironment;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.TextBox textBoxBundleId;
		private System.Windows.Forms.TextBox textBoxEnvironment;
		internal System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelCertificate;
        private System.Windows.Forms.TextBox textBoxCertificate;
        private System.Windows.Forms.Label labelThumbprint;
        private System.Windows.Forms.TextBox textBoxThumbprint;
        private System.Windows.Forms.Label labelCreated;
        private System.Windows.Forms.Label labelChanged;
        private System.Windows.Forms.TextBox textBoxCreated;
        private System.Windows.Forms.TextBox textBoxChanged;
    }
}
