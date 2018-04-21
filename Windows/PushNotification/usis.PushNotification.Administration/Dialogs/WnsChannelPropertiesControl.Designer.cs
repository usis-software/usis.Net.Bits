namespace usis.PushNotification.Administration
{
    partial class WNsChannelPropertiesControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WNsChannelPropertiesControl));
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelPackageName = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelPackageSid = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxPackageName = new System.Windows.Forms.TextBox();
            this.textBoxPackageSid = new System.Windows.Forms.TextBox();
            this.labelClientSecret = new System.Windows.Forms.Label();
            this.textBoxClientSecret = new System.Windows.Forms.TextBox();
            this.labelCreated = new System.Windows.Forms.Label();
            this.labelChanged = new System.Windows.Forms.Label();
            this.textBoxCreated = new System.Windows.Forms.TextBox();
            this.textBoxChanged = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.TextChanged += new System.EventHandler(this.Changed);
            // 
            // labelPackageName
            // 
            resources.ApplyResources(this.labelPackageName, "labelPackageName");
            this.labelPackageName.Name = "labelPackageName";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelPackageName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelPackageSid, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxPackageName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxPackageSid, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelClientSecret, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxClientSecret, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.labelCreated, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.labelChanged, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.textBoxCreated, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxChanged, 1, 5);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // labelPackageSid
            // 
            resources.ApplyResources(this.labelPackageSid, "labelPackageSid");
            this.labelPackageSid.Name = "labelPackageSid";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            // 
            // textBoxPackageName
            // 
            this.textBoxPackageName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.textBoxPackageName, "textBoxPackageName");
            this.textBoxPackageName.Name = "textBoxPackageName";
            this.textBoxPackageName.TextChanged += new System.EventHandler(this.Changed);
            // 
            // textBoxPackageSid
            // 
            this.textBoxPackageSid.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPackageSid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxPackageSid, "textBoxPackageSid");
            this.textBoxPackageSid.Name = "textBoxPackageSid";
            this.textBoxPackageSid.ReadOnly = true;
            // 
            // labelClientSecret
            // 
            resources.ApplyResources(this.labelClientSecret, "labelClientSecret");
            this.labelClientSecret.Name = "labelClientSecret";
            // 
            // textBoxClientSecret
            // 
            resources.ApplyResources(this.textBoxClientSecret, "textBoxClientSecret");
            this.textBoxClientSecret.Name = "textBoxClientSecret";
            this.textBoxClientSecret.TextChanged += new System.EventHandler(this.Changed);
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
            // WNsChannelPropertiesControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "WNsChannelPropertiesControl";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelPackageName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelPackageSid;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxPackageName;
        private System.Windows.Forms.TextBox textBoxPackageSid;
        private System.Windows.Forms.Label labelClientSecret;
        private System.Windows.Forms.TextBox textBoxClientSecret;
        private System.Windows.Forms.Label labelCreated;
        private System.Windows.Forms.Label labelChanged;
        private System.Windows.Forms.TextBox textBoxCreated;
        private System.Windows.Forms.TextBox textBoxChanged;
    }
}
