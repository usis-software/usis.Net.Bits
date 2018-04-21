namespace audius.GuV.Wizard
{
    partial class DataSourcePageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSourcePageControl));
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelDataSources = new System.Windows.Forms.Label();
            this.listBoxDataSources = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.Name = "labelTitle";
            // 
            // labelDataSources
            // 
            resources.ApplyResources(this.labelDataSources, "labelDataSources");
            this.labelDataSources.Name = "labelDataSources";
            // 
            // listBoxDataSources
            // 
            resources.ApplyResources(this.listBoxDataSources, "listBoxDataSources");
            this.listBoxDataSources.FormattingEnabled = true;
            this.listBoxDataSources.Name = "listBoxDataSources";
            this.listBoxDataSources.Sorted = true;
            this.listBoxDataSources.SelectedIndexChanged += new System.EventHandler(this.ListBoxDataSourcesSelectedIndexChanged);
            // 
            // DataSourcePageControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.listBoxDataSources);
            this.Controls.Add(this.labelDataSources);
            this.Controls.Add(this.labelTitle);
            resources.ApplyResources(this, "$this");
            this.Name = "DataSourcePageControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelDataSources;
        private System.Windows.Forms.ListBox listBoxDataSources;
    }
}
