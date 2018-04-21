namespace usis.Data.Registry
{
    partial class DataSourceControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSourceControl));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelDbProvider = new System.Windows.Forms.Label();
            this.comboBoxDataProvider = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxConnectionString = new System.Windows.Forms.TextBox();
            this.labelConnectionString = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.labelDbProvider, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxDataProvider, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxConnectionString, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.labelConnectionString, 0, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // labelDbProvider
            // 
            resources.ApplyResources(this.labelDbProvider, "labelDbProvider");
            this.labelDbProvider.Name = "labelDbProvider";
            // 
            // comboBoxDataProvider
            // 
            this.comboBoxDataProvider.DisplayMember = "Name";
            resources.ApplyResources(this.comboBoxDataProvider, "comboBoxDataProvider");
            this.comboBoxDataProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataProvider.FormattingEnabled = true;
            this.comboBoxDataProvider.Name = "comboBoxDataProvider";
            this.comboBoxDataProvider.ValueMember = "InvariantName";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            // 
            // textBoxConnectionString
            // 
            resources.ApplyResources(this.textBoxConnectionString, "textBoxConnectionString");
            this.textBoxConnectionString.Name = "textBoxConnectionString";
            // 
            // labelConnectionString
            // 
            resources.ApplyResources(this.labelConnectionString, "labelConnectionString");
            this.labelConnectionString.Name = "labelConnectionString";
            // 
            // DataSourceControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "DataSourceControl";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelDbProvider;
        private System.Windows.Forms.ComboBox comboBoxDataProvider;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxConnectionString;
        private System.Windows.Forms.Label labelConnectionString;
    }
}
