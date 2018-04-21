//
//  @(#) ModalDialog.Designer.cs
//
//  Project:    usis.Windows.Forms
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;

namespace usis.Windows.Forms
{
    //  -----------------
    //  ModalDialog class
    //  -----------------

    partial class ModalDialog
    {
        #region overrides

        //  --------------
        //  Dispose method
        //  --------------

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

        #endregion overrides

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModalDialog));
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panel = new System.Windows.Forms.Panel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.DialogPanel = new System.Windows.Forms.Panel();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.panel);
            this.panelButtons.Controls.Add(this.buttonOk);
            this.panelButtons.Controls.Add(this.buttonCancel);
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.Name = "panelButtons";
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // DialogPanel
            // 
            this.DialogPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.DialogPanel, "DialogPanel");
            this.DialogPanel.Name = "DialogPanel";
            // 
            // ModalDialog
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.DialogPanel);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalDialog";
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region fields

        //  ----------------
        //  components field
        //  ----------------

        /// <summary>
        /// Required designer variable.
        /// </summary>

        private System.ComponentModel.IContainer components = null;

        #region form controls

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Panel panel;

        #endregion form controls

        //  -----------------
        //  DialogPanel field
        //  -----------------

        /// <summary>
        /// The container for dialog controls.
        /// </summary>

        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected System.Windows.Forms.Panel DialogPanel;

        #endregion fields
    }
}

// eof "ModalDialog.Designer.cs"
