namespace audius.GuV.Wizard
{
    partial class DefinitionPageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefinitionPageControl));
            this.listBoxDefinitions = new System.Windows.Forms.ListBox();
            this.labelDefinition = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxDefinitions
            // 
            resources.ApplyResources(this.listBoxDefinitions, "listBoxDefinitions");
            this.listBoxDefinitions.FormattingEnabled = true;
            this.listBoxDefinitions.Name = "listBoxDefinitions";
            this.listBoxDefinitions.Sorted = true;
            this.listBoxDefinitions.SelectedIndexChanged += new System.EventHandler(this.ListBoxDefinitionsSelectedIndexChanged);
            // 
            // labelDefinition
            // 
            resources.ApplyResources(this.labelDefinition, "labelDefinition");
            this.labelDefinition.Name = "labelDefinition";
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.Name = "labelTitle";
            // 
            // DefinitionPageControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.listBoxDefinitions);
            this.Controls.Add(this.labelDefinition);
            this.Controls.Add(this.labelTitle);
            resources.ApplyResources(this, "$this");
            this.Name = "DefinitionPageControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxDefinitions;
        private System.Windows.Forms.Label labelDefinition;
        private System.Windows.Forms.Label labelTitle;
    }
}
