//
//  @(#) JobProgressControl.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using usis.Platform;

namespace usis.Net.Bits.Administration
{
    //  ------------------------
    //  JobProgressControl class
    //  ------------------------

    internal sealed partial class JobProgressControl : UserControl, IInjectable<JobNode>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public JobProgressControl() { InitializeComponent(); }

        #endregion construction

        #region IInjectable<JobNode> implementation

        //  -------------
        //  Inject method
        //  -------------

        void IInjectable<JobNode>.Inject(JobNode node)
        {
            if (node != null)
            {
                node.Updated += (sender, e) => { Update(node.Job); };
                Update(node.Job);
            }
        }

        #endregion IInjectable<JobNode> implementation

        #region private methods

        //  -------------
        //  Update method
        //  -------------

        private void Update(BackgroundCopyJob job)
        {
            var times = job.RetrieveTimes();
            var time = times.TransferCompletionTime;

            textBoxCreationTime.Text = times.CreationTime.ToString("U", CultureInfo.CurrentCulture);
            textBoxModificationTime.Text = times.ModificationTime.ToString("U", CultureInfo.CurrentCulture);
            textBoxTransferCompletionTime.Text = time == DateTime.MinValue ? string.Empty : time.ToString("U", CultureInfo.CurrentCulture);

            var progress = job.RetrieveProgress();
            float percent;

            percent = progress.FilesTotal == 0 ? 0.0f : 100.0f * progress.FilesTransferred / progress.FilesTotal;
            textBoxFiles.Text = string.Format(CultureInfo.CurrentCulture, "{0} of {1} ({2}%)",
                progress.FilesTransferred, progress.FilesTotal, percent);

            percent = progress.BytesTotal == 0 ? 0.0f : 100.0f * progress.BytesTransferred / progress.BytesTotal;
            textBoxBytes.Text = string.Format(CultureInfo.CurrentCulture, "{0:N0} of {1:N0} ({2:N0}%)",
                progress.BytesTransferred, progress.BytesTotal, percent);

            Debug.Print("percent={0}%", percent);

            var error = job.RetrieveError();
            if (error == null) return;

            Debug.Print("Context: {0}", error.Context);
            Debug.Print("Code: {0}", error.Code);
            Debug.Print("Context Description: {0}", error.ContextDescription);
            Debug.Print("Description: {0}", error.Description);
            Debug.Print("Protocol: {0}", error.Protocol);

            var file = error.RetrieveFile();
            Debug.Print("Error Remote Name: {0}", file.RemoteName);
            Debug.Print("Error Local Name: {0}", file.LocalName);

            textBoxError.Text = error.Description;
        }

        #endregion private methods
    }
}

// eof "JobProgressControl.cs"
