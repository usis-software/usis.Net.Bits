//
//  @(#) RootNode.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using usis.ManagementConsole;

namespace usis.Net.Bits.Administration
{
    //  --------------
    //  RootNode class
    //  --------------

    internal sealed class RootNode : ScopeNode<SnapIn, JobNode>, IDisposable
    {
        #region fields

        private Dictionary<Guid, JobNode> nodes = new Dictionary<Guid, JobNode>();

        #endregion fields

        #region properties

        //  -----------------------
        //  AllUsersAction property
        //  -----------------------

        private Microsoft.ManagementConsole.Action AllUsersAction { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal RootNode()
        {
            DisplayName = Strings.RootNodeDisplayName;
            EnabledStandardVerbs = StandardVerbs.Refresh;
            ViewDescriptions.Add(JobListView.Description);

            AllUsersAction = ActionsPaneItems.AddAction(Strings.ShowAllJobs, (e) =>
            {
                SnapIn.Settings.ShowAllUsers = AllUsersAction.Checked = !AllUsersAction.Checked;
                ReloadJobs();
                SnapIn.IsModified = true;
            });

            ActionsPaneItems.AddAction(Strings.CreateJobActionDisplayName, SnapIn.ImageIndex.CreateJob, (e) => CreateJob());
            ActionsPaneItems.AddAction(Strings.CancelAlJobsActionDisplayName, (e) => CancelAllJobs());
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose() { Children.DisposeEach(); }

        #endregion IDisposable implementation

        #region overrides

        //  ---------------
        //  OnExpand method
        //  ---------------

        protected override void OnExpand(AsyncStatus status)
        {
            AllUsersAction.Checked = SnapIn.Settings.ShowAllUsers;
            LoadJobs();
        }

        //  ----------------
        //  OnRefresh method
        //  ----------------

        protected override void OnRefresh(AsyncStatus status) { ReloadJobs(); }

        #endregion overrides

        #region private methods

        //  ----------------
        //  CreateJob method
        //  ----------------

        private void CreateJob()
        {
            using (var dialog = new CreateJobDialog())
            {
                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    CreateJob(dialog.JobName, dialog.JobType);
                }
            }
        }

        private void CreateJob(string name, BackgroundCopyJobType type)
        {
            SnapIn.Console.Invoke(() =>
            {
                var node = AddJob(SnapIn.Manager.CreateJob(name, type));
                CurrentView?.SelectScopeNode(node);
            });
        }

        //  ---------------
        //  LoadJobs method
        //  ---------------

        private void LoadJobs()
        {
            SnapIn.Console.Invoke(() =>
            {
                var removed = new HashSet<Guid>(nodes.Keys);
                var jobs = new List<BackgroundCopyJob>();
                try
                {
                    jobs.AddRange(SnapIn.Manager.EnumerateJobs(SnapIn.Settings.ShowAllUsers));
                    foreach (var job in jobs.ToArray())
                    {
                        if (!nodes.ContainsKey(job.Id))
                        {
                            AddJob(job);
                            jobs.Remove(job);
                        }
                        removed.Remove(job.Id);
                    }
                }
                finally
                {
                    jobs.DisposeEach();
                }
                foreach (var id in removed)
                {
                    if (nodes.TryGetValue(id, out JobNode node))
                    {
                        Children.Remove(node);
                        nodes.Remove(id);
                        node.Job.Dispose();
                    }
                }
            });
        }

        //  -----------------
        //  ReloadJobs method
        //  -----------------

        private void ReloadJobs()
        {
            SnapIn.Reconnect();
            LoadJobs();
        }

        //  -------------
        //  AddJob method
        //  -------------

        private JobNode AddJob(BackgroundCopyJob job)
        {
            var node = JobNode.Create(job);
            Children.Add(node);
            nodes.Add(job.Id, node);
            return node;
        }

        //  --------------------
        //  CancelAllJobs method
        //  --------------------

        private void CancelAllJobs()
        {
            var parameters = new MessageBoxParameters()
            {
                Caption = Strings.CancelAllJobsDialogCaption,
                Text = Strings.CancelAllJobsDialogText,
                Buttons = MessageBoxButtons.YesNo,
                Icon = MessageBoxIcon.Question
            };
            if (SnapIn.Console.ShowDialog(parameters) == DialogResult.Yes)
            {
                foreach (var node in Nodes)
                {
                    if (node.Job.State != BackgroundCopyJobState.Canceled &&
                        node.Job.State != BackgroundCopyJobState.Acknowledged)
                    {
                        node.Job.Cancel();
                    }
                }
            }
        }

        #endregion private methods
    }
}

// eof "RootNode.cs"
