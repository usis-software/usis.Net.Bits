namespace audius.GuV.Wizard
{
    partial class ClientPageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientPageControl));
            this.listBoxClients = new System.Windows.Forms.ListBox();
            this.labelClients = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxClients
            // 
            resources.ApplyResources(this.listBoxClients, "listBoxClients");
            this.listBoxClients.FormattingEnabled = true;
            this.listBoxClients.Name = "listBoxClients";
            this.listBoxClients.Sorted = true;
            this.listBoxClients.SelectedIndexChanged += new System.EventHandler(this.ListBoxClientsSelectedIndexChanged);
            // 
            // labelClients
            // 
            resources.ApplyResources(this.labelClients, "labelClients");
            this.labelClients.Name = "labelClients";
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.Name = "labelTitle";
            // 
            // ClientPageControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.listBoxClients);
            this.Controls.Add(this.labelClients);
            this.Controls.Add(this.labelTitle);
            resources.ApplyResources(this, "$this");
            this.Name = "ClientPageControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxClients;
        private System.Windows.Forms.Label labelClients;
        private System.Windows.Forms.Label labelTitle;
    }
}
