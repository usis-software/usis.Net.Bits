//
//  @(#) JobNode.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using System;
using System.Globalization;
using System.Security.Principal;
using System.Windows.Forms;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.Net.Bits.Administration
{
    //  -------------
    //  JobNode class
    //  -------------

    internal sealed class JobNode : ScopeNode<SnapIn>, IDisposable
    {
        #region properties

        //  ------------
        //  Job property
        //  ------------

        internal BackgroundCopyJob Job { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        private JobNode(BackgroundCopyJob job) : base(true)
        {
            Job = job;

            Update(job);
            ViewDescriptions.Add(FileListView.Description);

            this.AddActionPaneItem(Strings.AddFileActionDisplayName, SnapIn.ImageIndex.AddFile, e => { AddFile(); });
            ActionsPaneItems.Add(new ActionSeparator());
            this.AddActionPaneItem(Strings.SuspendJobActionDisplayName, SnapIn.ImageIndex.SuspendJob, e => { SuspendJob(); });
            this.AddActionPaneItem(Strings.ResumeJobActionDisplayName, SnapIn.ImageIndex.ResumeJob, e => { ResumeJob(); });
            this.AddActionPaneItem(Strings.CancelJobActionDisplayName, SnapIn.ImageIndex.CancelJob, e => { CancelJob(); });
            this.AddActionPaneItem(Strings.CompleteJobActionDisplayName, SnapIn.ImageIndex.CompleteJob, e => { CompleteJob(); });
            ActionsPaneItems.Add(new ActionSeparator());
            this.AddActionPaneItem(Strings.TakeOwnershipJobActionDisplayName, SnapIn.ImageIndex.TakeOwner, e => { TakeOwnership(); });

            EnabledStandardVerbs = StandardVerbs.Properties | StandardVerbs.Rename | StandardVerbs.Refresh;

            Job.Modified += Modified;
            Job.Failed += Failed;
            Job.Transferred += Transferred;
        }

        #endregion construction

        #region IDisposable

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose()
        {
            Job.Modified -= Modified;
            Job.Failed -= Failed;
            Job.Transferred -= Transferred;
            Job.Dispose();
        }

        #endregion IDisposable

        #region event handlers

        //  ---------------
        //  Modified method
        //  ---------------

        private void Modified(object sender, EventArgs e) { OnUpdated(); }

        //  -------------
        //  Failed method
        //  -------------

        private void Failed(object sender, EventArgs e) { OnUpdated(); }

        //  ------------------
        //  Transferred method
        //  ------------------

        private void Transferred(object sender, EventArgs e) { OnUpdated(); }

        #endregion event handlers

        #region events

        //  -------------
        //  Updated event
        //  -------------

        internal event EventHandler Updated;

        //  ----------------
        //  OnUpdated method
        //  ----------------

        private void OnUpdated()
        {
            Update(Job);
            System.Diagnostics.Debug.Assert(SnapIn.InvokeRequired);
            SnapIn.Invoke(new MethodInvoker(delegate { Updated?.Invoke(this, EventArgs.Empty); }));
        }

        #endregion events

        #region overrides

        //  ---------------
        //  OnRename method
        //  ---------------

        protected override void OnRename(string newText, SyncStatus status)
        {
            SnapIn.Console.Invoke(() => { Job.DisplayName = newText; });
        }

        //  -------------------------
        //  OnAddPropertyPages method
        //  -------------------------

        protected override void OnAddPropertyPages(PropertyPageCollection propertyPageCollection)
        {
            if (propertyPageCollection == null) throw new ArgumentNullException(nameof(propertyPageCollection));

            propertyPageCollection.Add(new JobGeneralPropertyPage(this));
            propertyPageCollection.Add(new JobProgressPropertyPage(this));
        }

        //  ----------------
        //  OnRefresh method
        //  ----------------

        protected override void OnRefresh(AsyncStatus status)
        {
            (CurrentView as FileListView)?.Refresh(status);
        }

        #endregion overrides

        #region methods

        //  -------------
        //  Create method
        //  -------------

        internal static JobNode Create(BackgroundCopyJob job) { return new JobNode(job); }

        //  -------------
        //  Update method
        //  -------------

        private void Update(BackgroundCopyJob job)
        {
            lock (this)
            {
                DisplayName = string.IsNullOrWhiteSpace(job.DisplayName) ? job.Id.ToString() : job.DisplayName;
                ImageIndex = job.JobType == BackgroundCopyJobType.Download ? 1 : 2;
                SelectedImageIndex = ImageIndex;
                SubItemDisplayNames.Clear();
                SubItemDisplayNames.Add(
                    job.Description,
                    job.JobType.ToString(),
                    string.Format(CultureInfo.CurrentCulture, "{0:F0}%", job.GetPercentBytesTransferred()),
                    job.State.ToString(),
                    job.Priority.ToString(),
                    AccountNameFromSidString(job.Owner));
            }
        }

        //  --------------
        //  AddFile method
        //  --------------

        private void AddFile()
        {
            using (var dialog = new AddFileDialog(Job))
            {
                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    Job.AddFile(dialog.RemoteUrl, dialog.LocalName);
                    (CurrentView as FileListView)?.Refresh(null);
                }
            }
        }

        //  -----------------
        //  SuspendJob method
        //  -----------------

        private void SuspendJob()
        {
            MessageBoxWithJob(new MessageBoxParameters()
            {
                Caption = Strings.SuspendJobDialogCaption,
                Text = string.Format(CultureInfo.CurrentCulture, Strings.SuspendJobDialogText, DisplayName),
                Buttons = MessageBoxButtons.YesNo,
                Icon = MessageBoxIcon.Question
            },
            DialogResult.Yes, job => job.Suspend());
        }

        //  ----------------
        //  ResumeJob method
        //  ----------------

        private void ResumeJob()
        {
            MessageBoxWithJob(new MessageBoxParameters()
            {
                Caption = Strings.ResumeJobDialogCaption,
                Text = string.Format(CultureInfo.CurrentCulture, Strings.ResumeJobDialogText, DisplayName),
                Buttons = MessageBoxButtons.YesNo,
                Icon = MessageBoxIcon.Question
            },
            DialogResult.Yes, job => job.Resume());
        }

        //  ----------------
        //  CancelJob method
        //  ----------------

        private void CancelJob()
        {
            MessageBoxWithJob(new MessageBoxParameters()
            {
                Caption = Strings.CancelJobDialogCaption,
                Text = string.Format(CultureInfo.CurrentCulture, Strings.CancelJobDialogText, DisplayName),
                Buttons = MessageBoxButtons.YesNo,
                Icon = MessageBoxIcon.Question
            },
            DialogResult.Yes, job => job.Cancel());
        }

        //  ------------------
        //  CompleteJob method
        //  ------------------

        private void CompleteJob()
        {
            MessageBoxWithJob(new MessageBoxParameters()
            {
                Caption = Strings.CompleteJobDialogCaption,
                Text = string.Format(CultureInfo.CurrentCulture, Strings.CompleteJobDialogText, DisplayName),
                Buttons = MessageBoxButtons.YesNo,
                Icon = MessageBoxIcon.Question
            },
            DialogResult.Yes, job =>
            {
                job.Complete();
                Parent.Children.Remove(this);
            });
        }

        //  --------------------
        //  TakeOwnership method
        //  --------------------

        private void TakeOwnership()
        {
            MessageBoxWithJob(new MessageBoxParameters()
            {
                Caption = Strings.TakeOwnershipJobDialogCaption,
                Text = string.Format(CultureInfo.CurrentCulture, Strings.TakeOwnershipJobDialogText, DisplayName),
                Buttons = MessageBoxButtons.YesNo,
                Icon = MessageBoxIcon.Question
            },
            DialogResult.Yes, job => job.TakeOwnership());
        }

        //  ------------------------
        //  MessageBoxWithJob method
        //  ------------------------

        private void MessageBoxWithJob(MessageBoxParameters parameters, DialogResult result, Action<BackgroundCopyJob> action)
        {
            if (SnapIn.Console.ShowDialog(parameters) == result) action(Job);
        }

        //  -------------------------------
        //  AccountNameFromSidString method
        //  -------------------------------

        internal static string AccountNameFromSidString(string sddlForm)
        {
            var sid = new SecurityIdentifier(sddlForm);
            var accountName = sid.Translate(typeof(NTAccount));
            return accountName.ToString();
        }

        #endregion methods
    }
}

// eof "JobNode.cs"
