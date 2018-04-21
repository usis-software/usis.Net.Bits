//
//  @(#) AddFileDialog.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Windows.Forms;

namespace usis.Net.Bits.Administration
{
    //  -------------------
    //  AddFileDialog class
    //  -------------------

    internal partial class AddFileDialog : Windows.Forms.ModalDialog
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public AddFileDialog(BackgroundCopyJob job)
        {
            Job = job;

            InitializeComponent();
            EnableOkButton(false);
        }

        #endregion construction

        #region properties

        //  ------------
        //  Job property
        //  ------------

        private BackgroundCopyJob Job { get; }

        //  ------------------
        //  RemoteUrl property
        //  ------------------

        internal string RemoteUrl => textBoxRemoteUrl.Text;

        //  ------------------
        //  LocalName property
        //  ------------------

        internal string LocalName => textBoxLocalName.Text;

        #endregion properties

        #region event handlers

        //  --------------
        //  Changed method
        //  --------------

        private void Changed(object sender, EventArgs e)
        {
            EnableOkButton(
                !string.IsNullOrWhiteSpace(textBoxRemoteUrl.Text) &&
                !string.IsNullOrWhiteSpace(textBoxLocalName.Text));
        }

        //  ---------------------------
        //  ButtonLocalNameClick method
        //  ---------------------------

        private void ButtonLocalNameClick(object sender, EventArgs e)
        {
            using (var dialog = CreateFileDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxLocalName.Text = dialog.FileName;
                }
            }
        }

        #endregion event handlers

        #region private methods

        //  -----------------------
        //  CreateFileDialog method
        //  -----------------------

#pragma warning disable IDE0017 // Simplify object initialization

        private FileDialog CreateFileDialog()
        {
            FileDialog temp = null;
            FileDialog dialog;
            try
            {
                if (Job.JobType == BackgroundCopyJobType.Download)
                {
                    temp = new SaveFileDialog();
                    temp.Title = Strings.FileDialogDownlaodLocalNameTitle;
                }
                else
                {
                    temp = new OpenFileDialog();
                    temp.Title = Strings.FIleDialogUploadLocalNameTitle;
                }
                dialog = temp;
                temp = null;
            }
            finally
            {
                if (temp != null) temp.Dispose();
            }
            return dialog;
        }

#pragma warning restore IDE0017 // Simplify object initialization

        #endregion private methods
    }
}

// eof "AddFileDialog.cs"
