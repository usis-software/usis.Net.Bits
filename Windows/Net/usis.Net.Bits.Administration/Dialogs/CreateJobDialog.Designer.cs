namespace usis.Net.Bits.Administration
{
    partial class CreateJobDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateJobDialog));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
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
            this.tableLayoutPanel.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelType, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxType, 1, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            // 
            // textBoxName
            // 
            resources.ApplyResources(this.textBoxName, "textBoxName");
            this.textBoxName.Name = "textBoxName";
            // 
            // labelType
            // 
            resources.ApplyResources(this.labelType, "labelType");
            this.labelType.Name = "labelType";
            // 
            // comboBoxType
            // 
            resources.ApplyResources(this.comboBoxType, "comboBoxType");
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Name = "comboBoxType";
            // 
            // CreateJobDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CreateJobDialog";
            this.DialogPanel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
    }
}