namespace usis.Net.Bits.Administration
{
    partial class AddFileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddFileDialog));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelRemoteUrl = new System.Windows.Forms.Label();
            this.labelLocalName = new System.Windows.Forms.Label();
            this.textBoxRemoteUrl = new System.Windows.Forms.TextBox();
            this.textBoxLocalName = new System.Windows.Forms.TextBox();
            this.buttonLocalName = new System.Windows.Forms.Button();
            this.DialogPanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DialogPanel
            // 
            this.DialogPanel.Controls.Add(this.tableLayoutPanel);
            resources.ApplyResources(this.DialogPanel, "DialogPanel");
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.labelRemoteUrl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelLocalName, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxRemoteUrl, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxLocalName, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonLocalName, 2, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // labelRemoteUrl
            // 
            resources.ApplyResources(this.labelRemoteUrl, "labelRemoteUrl");
            this.labelRemoteUrl.Name = "labelRemoteUrl";
            // 
            // labelLocalName
            // 
            resources.ApplyResources(this.labelLocalName, "labelLocalName");
            this.labelLocalName.Name = "labelLocalName";
            // 
            // textBoxRemoteUrl
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxRemoteUrl, 2);
            resources.ApplyResources(this.textBoxRemoteUrl, "textBoxRemoteUrl");
            this.textBoxRemoteUrl.Name = "textBoxRemoteUrl";
            this.textBoxRemoteUrl.TextChanged += new System.EventHandler(this.Changed);
            // 
            // textBoxLocalName
            // 
            resources.ApplyResources(this.textBoxLocalName, "textBoxLocalName");
            this.textBoxLocalName.Name = "textBoxLocalName";
            this.textBoxLocalName.TextChanged += new System.EventHandler(this.Changed);
            // 
            // buttonLocalName
            // 
            resources.ApplyResources(this.buttonLocalName, "buttonLocalName");
            this.buttonLocalName.Name = "buttonLocalName";
            this.buttonLocalName.UseVisualStyleBackColor = true;
            this.buttonLocalName.Click += new System.EventHandler(this.ButtonLocalNameClick);
            // 
            // AddFileDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AddFileDialog";
            this.DialogPanel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelRemoteUrl;
        private System.Windows.Forms.Label labelLocalName;
        private System.Windows.Forms.TextBox textBoxRemoteUrl;
        private System.Windows.Forms.TextBox textBoxLocalName;
        private System.Windows.Forms.Button buttonLocalName;
    }
}