namespace usis.PushNotification.Administration
{
    partial class StartViewControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartViewControl));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTitleProduct = new System.Windows.Forms.Label();
            this.labelTitleCompany = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelRouterStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxProvider = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxConnectionString = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panelHeader.Controls.Add(this.labelTitleProduct);
            this.panelHeader.Controls.Add(this.labelTitleCompany);
            this.panelHeader.Controls.Add(this.pictureBoxLogo);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            // 
            // labelTitleProduct
            // 
            resources.ApplyResources(this.labelTitleProduct, "labelTitleProduct");
            this.labelTitleProduct.Name = "labelTitleProduct";
            // 
            // labelTitleCompany
            // 
            resources.ApplyResources(this.labelTitleCompany, "labelTitleCompany");
            this.labelTitleCompany.Name = "labelTitleCompany";
            // 
            // pictureBoxLogo
            // 
            resources.ApplyResources(this.pictureBoxLogo, "pictureBoxLogo");
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelRouterStatus, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxProvider, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBoxConnectionString, 2, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // labelStatus
            // 
            resources.ApplyResources(this.labelStatus, "labelStatus");
            this.tableLayoutPanel1.SetColumnSpan(this.labelStatus, 2);
            this.labelStatus.Name = "labelStatus";
            // 
            // labelRouterStatus
            // 
            resources.ApplyResources(this.labelRouterStatus, "labelRouterStatus");
            this.tableLayoutPanel1.SetColumnSpan(this.labelRouterStatus, 2);
            this.labelRouterStatus.Name = "labelRouterStatus";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxProvider
            // 
            this.textBoxProvider.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxProvider.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxProvider, "textBoxProvider");
            this.textBoxProvider.Name = "textBoxProvider";
            this.textBoxProvider.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxConnectionString
            // 
            this.textBoxConnectionString.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxConnectionString, "textBoxConnectionString");
            this.textBoxConnectionString.Name = "textBoxConnectionString";
            this.textBoxConnectionString.ReadOnly = true;
            // 
            // StartViewControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panelHeader);
            this.Name = "StartViewControl";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitleProduct;
        private System.Windows.Forms.Label labelTitleCompany;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelRouterStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxProvider;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxConnectionString;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
