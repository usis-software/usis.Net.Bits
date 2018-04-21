namespace usis.Net.Bits.Administration
{
    partial class JobPropertiesControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobPropertiesControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelDisplayName = new System.Windows.Forms.Label();
            this.labelId = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelPriority = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.labelErrorCount = new System.Windows.Forms.Label();
            this.labelMinimumRetryDelay = new System.Windows.Forms.Label();
            this.labelNoProgressTimeout = new System.Windows.Forms.Label();
            this.textBoxDisplayName = new System.Windows.Forms.TextBox();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.textBoxType = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.textBoxErrorCount = new System.Windows.Forms.TextBox();
            this.numericUpDownMinimumRetryDelay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownNoProgressTimeout = new System.Windows.Forms.NumericUpDown();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.labelOwner = new System.Windows.Forms.Label();
            this.textBoxOwner = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumRetryDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoProgressTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.labelDisplayName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelId, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelDescription, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelPriority, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelState, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelErrorCount, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelMinimumRetryDelay, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelNoProgressTimeout, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.textBoxDisplayName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxId, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxType, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxDescription, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxState, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBoxErrorCount, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownMinimumRetryDelay, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownNoProgressTimeout, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxPriority, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelOwner, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxOwner, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // labelDisplayName
            // 
            resources.ApplyResources(this.labelDisplayName, "labelDisplayName");
            this.labelDisplayName.Name = "labelDisplayName";
            // 
            // labelId
            // 
            resources.ApplyResources(this.labelId, "labelId");
            this.labelId.Name = "labelId";
            // 
            // labelType
            // 
            resources.ApplyResources(this.labelType, "labelType");
            this.labelType.Name = "labelType";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            // 
            // labelPriority
            // 
            resources.ApplyResources(this.labelPriority, "labelPriority");
            this.labelPriority.Name = "labelPriority";
            // 
            // labelState
            // 
            resources.ApplyResources(this.labelState, "labelState");
            this.labelState.Name = "labelState";
            // 
            // labelErrorCount
            // 
            resources.ApplyResources(this.labelErrorCount, "labelErrorCount");
            this.labelErrorCount.Name = "labelErrorCount";
            // 
            // labelMinimumRetryDelay
            // 
            resources.ApplyResources(this.labelMinimumRetryDelay, "labelMinimumRetryDelay");
            this.labelMinimumRetryDelay.Name = "labelMinimumRetryDelay";
            // 
            // labelNoProgressTimeout
            // 
            resources.ApplyResources(this.labelNoProgressTimeout, "labelNoProgressTimeout");
            this.labelNoProgressTimeout.Name = "labelNoProgressTimeout";
            // 
            // textBoxDisplayName
            // 
            resources.ApplyResources(this.textBoxDisplayName, "textBoxDisplayName");
            this.textBoxDisplayName.Name = "textBoxDisplayName";
            this.textBoxDisplayName.Tag = "DisplayName";
            this.textBoxDisplayName.TextChanged += new System.EventHandler(this.Changed);
            // 
            // textBoxId
            // 
            this.textBoxId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxId, "textBoxId");
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.ReadOnly = true;
            this.textBoxId.TabStop = false;
            // 
            // textBoxType
            // 
            this.textBoxType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxType, "textBoxType");
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.ReadOnly = true;
            this.textBoxType.TabStop = false;
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Tag = "Description";
            this.textBoxDescription.TextChanged += new System.EventHandler(this.Changed);
            // 
            // textBoxState
            // 
            this.textBoxState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxState, "textBoxState");
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.TabStop = false;
            // 
            // textBoxErrorCount
            // 
            this.textBoxErrorCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxErrorCount, "textBoxErrorCount");
            this.textBoxErrorCount.Name = "textBoxErrorCount";
            this.textBoxErrorCount.ReadOnly = true;
            this.textBoxErrorCount.TabStop = false;
            // 
            // numericUpDownMinimumRetryDelay
            // 
            resources.ApplyResources(this.numericUpDownMinimumRetryDelay, "numericUpDownMinimumRetryDelay");
            this.numericUpDownMinimumRetryDelay.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMinimumRetryDelay.Name = "numericUpDownMinimumRetryDelay";
            this.numericUpDownMinimumRetryDelay.Tag = "MinimumRetryDelay";
            this.numericUpDownMinimumRetryDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMinimumRetryDelay.ValueChanged += new System.EventHandler(this.Changed);
            // 
            // numericUpDownNoProgressTimeout
            // 
            resources.ApplyResources(this.numericUpDownNoProgressTimeout, "numericUpDownNoProgressTimeout");
            this.numericUpDownNoProgressTimeout.Name = "numericUpDownNoProgressTimeout";
            this.numericUpDownNoProgressTimeout.Tag = "NoProgressTimeout";
            this.numericUpDownNoProgressTimeout.ValueChanged += new System.EventHandler(this.Changed);
            // 
            // comboBoxPriority
            // 
            this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBoxPriority, "comboBoxPriority");
            this.comboBoxPriority.Name = "comboBoxPriority";
            this.comboBoxPriority.Tag = "Priority";
            this.comboBoxPriority.SelectionChangeCommitted += new System.EventHandler(this.Changed);
            // 
            // labelOwner
            // 
            resources.ApplyResources(this.labelOwner, "labelOwner");
            this.labelOwner.Name = "labelOwner";
            // 
            // textBoxOwner
            // 
            this.textBoxOwner.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxOwner, "textBoxOwner");
            this.textBoxOwner.Name = "textBoxOwner";
            this.textBoxOwner.ReadOnly = true;
            this.textBoxOwner.TabStop = false;
            // 
            // JobPropertiesControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "JobPropertiesControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumRetryDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoProgressTimeout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelDisplayName;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelPriority;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label labelErrorCount;
        private System.Windows.Forms.Label labelMinimumRetryDelay;
        private System.Windows.Forms.Label labelNoProgressTimeout;
        private System.Windows.Forms.TextBox textBoxDisplayName;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.TextBox textBoxType;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TextBox textBoxState;
        private System.Windows.Forms.TextBox textBoxErrorCount;
        private System.Windows.Forms.NumericUpDown numericUpDownMinimumRetryDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownNoProgressTimeout;
        private System.Windows.Forms.ComboBox comboBoxPriority;
        private System.Windows.Forms.Label labelOwner;
        private System.Windows.Forms.TextBox textBoxOwner;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
