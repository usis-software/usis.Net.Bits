//
//  @(#) ApplicationListView.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Diagnostics;
using System.Linq;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.ApplicationServer.Administration
{
    internal class ApplicationListView : ListView<SnapIn, ResultNode, ApplicationInstanceInfo, ServerNode>
    {
        #region fields

        private Action startAction = new Action(Strings.StartApplicationAction, Strings.StartApplicationDescription);
        private Action stopAction = new Action(Strings.StopApplicationAction, Strings.StopApplicationDescription);
        private Action pauseAction = new Action(Strings.PauseApplicationAction, Strings.PauseApplicationDescription);
        private Action resumeAction = new Action(Strings.ResumeApplicationAction, Strings.ResumeApplicationDescription);

        #endregion fields

        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.Applications,
            ViewType = typeof(ApplicationListView),
            Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect
        };

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            Columns[0].Title = Strings.ApplicationListColumnName;
            Columns[0].SetWidth(150);

            Columns.Add(new MmcListViewColumn(Strings.ApplicationListColumnStatus, 75));

            Attach(SnapIn.ServerModel.Applications);
            SnapIn.ServerModel.Applications.Reload(false);

            startAction.Triggered += (sender, e) => ExecuteCommand(/*e, */ApplicationCommand.Start);
            stopAction.Triggered += (sender, e) => ExecuteCommand(/*e, */ApplicationCommand.Stop);
            pauseAction.Triggered += (sender, e) => ExecuteCommand(/*e, */ApplicationCommand.Pause);
            resumeAction.Triggered += (sender, e) => ExecuteCommand(/*e, */ApplicationCommand.Resume);
        }

        //  -----------------------
        //  CreateResultNode method
        //  -----------------------

        protected override ResultNode CreateResultNode(ApplicationInstanceInfo item)
        {
            var node = new ResultNode();
            node.SubItemDisplayNames.Add(string.Empty);
            UpdateResultNode(node, item);
            return node;
        }

        //  -------------------------
        //  OnSelectionChanged method
        //  -------------------------

        protected override void OnSelectionChanged(SyncStatus status)
        {
            if (SelectedNodes.Count == 0)
            {
                SelectionData.Clear();
                SelectionData.ActionsPaneItems.Clear();
            }
            else
            {
                Debug.Assert(SelectedNodes.Count == 1);
                var application = SelectedResultNodes.FirstOrDefault()?.Tag as ApplicationInstanceInfo;
                SelectionData.Update(application, false, null, null);
                //SelectionData.EnabledStandardVerbs = StandardVerbs.Delete | StandardVerbs.Properties;

                SelectionData.ActionsPaneItems.Add(startAction);
                SelectionData.ActionsPaneItems.Add(stopAction);
                SelectionData.ActionsPaneItems.Add(pauseAction);
                SelectionData.ActionsPaneItems.Add(resumeAction);
                UpdateActions(application);
            }
            //SelectionData.HelpTopic = null;
        }

        #endregion overrides

        #region private methods

        //  ---------------------
        //  ExecuteCommand method
        //  ---------------------

        private void ExecuteCommand(/*ActionEventArgs e, */ApplicationCommand command)
        {
            var node = SelectedResultNodes.FirstOrDefault();
            if (node == null) return;
            var application = node.Tag as ApplicationInstanceInfo;
            if (application == null) return;

            SnapIn.Console.Invoke(() =>
            {
                application = SnapIn.ServerModel.ExecuteApplicationCommand(application, command);
            });
            UpdateResultNode(node, application);
            UpdateActions(application);
        }

        //  -----------------------
        //  UpdateResultNode method
        //  -----------------------

        private static void UpdateResultNode(ResultNode node, ApplicationInstanceInfo info)
        {
            if (node == null) return;
            node.ImageIndex = 3;
            node.DisplayName = info.Name;
            node.SubItemDisplayNames[0] = info.State.ToString();
            node.Tag = info;
        }

        //  --------------------
        //  UpdateActions method
        //  --------------------

        private void UpdateActions(ApplicationInstanceInfo info)
        {
            startAction.Enabled =
                info.State == ApplicationInstanceState.Stopped ||
                info.State == ApplicationInstanceState.Failed;

            stopAction.Enabled =
                info.State == ApplicationInstanceState.Running ||
                info.State == ApplicationInstanceState.Paused;

            pauseAction.Enabled = info.State == ApplicationInstanceState.Running;

            resumeAction.Enabled = info.State == ApplicationInstanceState.Paused;
        }

        #endregion private methods
    }
}

// eof "ApplicationListView.cs"
