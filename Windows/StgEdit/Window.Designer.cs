namespace usis.StorageEditor
{
    partial class Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.storageControl = new usis.StorageEditor.StorageControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isDirtyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // storageControl
            // 
            this.storageControl.BackColor = System.Drawing.Color.Magenta;
            resources.ApplyResources(this.storageControl, "storageControl");
            this.storageControl.Name = "storageControl";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.editToolStripMenuItem});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileNew,
            this.menuItemFileOpen,
            this.menuItemFileSave,
            this.menuItemFileSaveAs,
            this.toolStripSeparator1,
            this.menuItemFileExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            resources.ApplyResources(this.toolStripMenuItemFile, "toolStripMenuItemFile");
            // 
            // menuItemFileNew
            // 
            this.menuItemFileNew.Name = "menuItemFileNew";
            resources.ApplyResources(this.menuItemFileNew, "menuItemFileNew");
            this.menuItemFileNew.Click += new System.EventHandler(this.FileNewClick);
            // 
            // menuItemFileOpen
            // 
            this.menuItemFileOpen.Name = "menuItemFileOpen";
            resources.ApplyResources(this.menuItemFileOpen, "menuItemFileOpen");
            this.menuItemFileOpen.Click += new System.EventHandler(this.FileOpenClick);
            // 
            // menuItemFileSave
            // 
            this.menuItemFileSave.Name = "menuItemFileSave";
            resources.ApplyResources(this.menuItemFileSave, "menuItemFileSave");
            this.menuItemFileSave.Click += new System.EventHandler(this.FileSaveClick);
            // 
            // menuItemFileSaveAs
            // 
            this.menuItemFileSaveAs.Name = "menuItemFileSaveAs";
            resources.ApplyResources(this.menuItemFileSaveAs, "menuItemFileSaveAs");
            this.menuItemFileSaveAs.Click += new System.EventHandler(this.FileSaveAsClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // menuItemFileExit
            // 
            this.menuItemFileExit.Name = "menuItemFileExit";
            resources.ApplyResources(this.menuItemFileExit, "menuItemFileExit");
            this.menuItemFileExit.Click += new System.EventHandler(this.FileExitClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isDirtyToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // isDirtyToolStripMenuItem
            // 
            this.isDirtyToolStripMenuItem.CheckOnClick = true;
            this.isDirtyToolStripMenuItem.Name = "isDirtyToolStripMenuItem";
            resources.ApplyResources(this.isDirtyToolStripMenuItem, "isDirtyToolStripMenuItem");
            this.isDirtyToolStripMenuItem.Click += new System.EventHandler(this.isDirtyToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // Window
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.storageControl);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Window";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StorageControl storageControl;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileSave;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isDirtyToolStripMenuItem;
    }
}

