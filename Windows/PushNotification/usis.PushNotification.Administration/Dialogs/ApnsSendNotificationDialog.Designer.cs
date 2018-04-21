namespace usis.PushNotification.Administration
{
    partial class ApnsSendNotificationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApnsSendNotificationDialog));
            this.textBoxAlert = new System.Windows.Forms.TextBox();
            this.labelAlert = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelBadge = new System.Windows.Forms.Label();
            this.labelSound = new System.Windows.Forms.Label();
            this.comboBoxSound = new System.Windows.Forms.ComboBox();
            this.numericUpDownBadge = new System.Windows.Forms.NumericUpDown();
            this.checkBoxContentAvailable = new System.Windows.Forms.CheckBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.textBoxCategory = new System.Windows.Forms.TextBox();
            this.DialogPanel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBadge)).BeginInit();
            this.SuspendLayout();
            // 
            // DialogPanel
            // 
            this.DialogPanel.Controls.Add(this.tableLayoutPanel);
            resources.ApplyResources(this.DialogPanel, "DialogPanel");
            // 
            // textBoxAlert
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxAlert, 2);
            resources.ApplyResources(this.textBoxAlert, "textBoxAlert");
            this.textBoxAlert.Name = "textBoxAlert";
            // 
            // labelAlert
            // 
            resources.ApplyResources(this.labelAlert, "labelAlert");
            this.tableLayoutPanel.SetColumnSpan(this.labelAlert, 2);
            this.labelAlert.Name = "labelAlert";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.labelAlert, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxAlert, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelBadge, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelSound, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.comboBoxSound, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownBadge, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.checkBoxContentAvailable, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelCategory, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.textBoxCategory, 1, 5);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // labelBadge
            // 
            resources.ApplyResources(this.labelBadge, "labelBadge");
            this.labelBadge.Name = "labelBadge";
            // 
            // labelSound
            // 
            resources.ApplyResources(this.labelSound, "labelSound");
            this.labelSound.Name = "labelSound";
            // 
            // comboBoxSound
            // 
            resources.ApplyResources(this.comboBoxSound, "comboBoxSound");
            this.comboBoxSound.FormattingEnabled = true;
            this.comboBoxSound.Name = "comboBoxSound";
            // 
            // numericUpDownBadge
            // 
            resources.ApplyResources(this.numericUpDownBadge, "numericUpDownBadge");
            this.numericUpDownBadge.Name = "numericUpDownBadge";
            // 
            // checkBoxContentAvailable
            // 
            resources.ApplyResources(this.checkBoxContentAvailable, "checkBoxContentAvailable");
            this.checkBoxContentAvailable.Name = "checkBoxContentAvailable";
            this.checkBoxContentAvailable.UseVisualStyleBackColor = true;
            // 
            // labelCategory
            // 
            resources.ApplyResources(this.labelCategory, "labelCategory");
            this.labelCategory.Name = "labelCategory";
            // 
            // textBoxCategory
            // 
            resources.ApplyResources(this.textBoxCategory, "textBoxCategory");
            this.textBoxCategory.Name = "textBoxCategory";
            // 
            // SendNotificationDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SendNotificationDialog";
            this.DialogPanel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBadge)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAlert;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelAlert;
        private System.Windows.Forms.Label labelBadge;
        private System.Windows.Forms.Label labelSound;
        private System.Windows.Forms.ComboBox comboBoxSound;
        private System.Windows.Forms.NumericUpDown numericUpDownBadge;
        private System.Windows.Forms.CheckBox checkBoxContentAvailable;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.TextBox textBoxCategory;
    }
}