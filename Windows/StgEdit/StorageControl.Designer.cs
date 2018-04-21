namespace usis.StorageEditor
{
    partial class StorageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StorageControl));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splitter = new System.Windows.Forms.Splitter();
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(200, 480);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder_closed.png");
            this.imageList.Images.SetKeyName(1, "folder.png");
            this.imageList.Images.SetKeyName(2, "text_binary.png");
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter.Location = new System.Drawing.Point(200, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(8, 480);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(208, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(432, 480);
            this.panel.TabIndex = 2;
            // 
            // StorageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.treeView);
            this.Name = "StorageControl";
            this.Size = new System.Drawing.Size(640, 480);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.ImageList imageList;
    }
}
