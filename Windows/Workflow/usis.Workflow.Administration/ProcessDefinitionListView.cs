//
//  @(#) ProcessDefinitionListView.cs
//
//  Project:    usis Workflow Management System
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Diagnostics;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.Workflow.Administration
{
    internal sealed class ProcessDefinitionListView : ListView<SnapIn>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static ViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.ProcessDefinitions,
            ViewType = typeof(ProcessDefinitionListView),
            Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect
        };

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            Columns[0].SetWidth(250);
            Columns.Add(Strings.ColumnNameState, 75);

            ActionsPaneItems.AddAction("New Process Definition...", (args) =>
            {
                var p = SnapIn.Engine.CreateProcessDefinition("New Workflow");
                SnapIn.Engine.ProcessDefinitions.Reload();
                SelectItem(p);
                //SnapIn.ShowDialog(new Microsoft.ManagementConsole.Advanced.MessageBoxParameters() { Text = args.Action.ToString() });
            });

            Attach(SnapIn.Engine.ProcessDefinitions);
            SnapIn.Engine.ProcessDefinitions.Reload(false);
        }

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
                SelectionData.Update(SelectedNodes[0], false, null, null);
                SelectionData.EnabledStandardVerbs = StandardVerbs.Delete;
            }
        }

        protected override void OnDelete(SyncStatus status)
        {
            SnapIn.Console.Invoke(() =>
            {
                (FirstSelectedNode?.Tag as ProcessDefinition)?.Delete();
                SnapIn.Engine.ProcessDefinitions.Reload();
            });
        }

        //private void SelectItem(object item)
        //{
        //    foreach (var node in ResultNodes.Cast<ResultNode>())
        //    {
        //        if (node.Tag != null && node.Tag.Equals(item)) node.SendSelectionRequest(true);
        //    }
        //}

        //  -----------------------
        //  CreateResultNode method
        //  -----------------------

        protected override ResultNode CreateResultNode(object item)
        {
            return new ProcessDefintionResultNode(item as ProcessDefinition);
        }

        #endregion overrides
    }

    #region ProcessDefintionResultNode class

    //  --------------------------------
    //  ProcessDefintionResultNode class
    //  --------------------------------

    internal sealed class ProcessDefintionResultNode : ResultNode
    {
        internal ProcessDefintionResultNode(ProcessDefinition item)
        {
            DisplayName = string.IsNullOrWhiteSpace(item.Name) ? item.Id.ToString() : item.Name;
            Tag = item;

            SubItemDisplayNames.Add(item.State.ToString());
        }
    }

    #endregion ProcessDefintionResultNode class

    #region ProcessDefinitionListNode class

    //  -------------------------------
    //  ProcessDefinitionListNode class
    //  -------------------------------

    internal sealed class ProcessDefinitionListNode : ScopeNode<SnapIn>
    {
        internal ProcessDefinitionListNode() : base(true)
        {
            DisplayName = Strings.ProcessDefinitions;
            EnabledStandardVerbs = StandardVerbs.Refresh;

            ViewDescriptions.Add(ProcessDefinitionListView.Description);
        }

        protected override void OnRefresh(AsyncStatus status)
        {
            SnapIn.Console.Invoke(SnapIn.Engine.ProcessDefinitions.Reload);
        }
    }

    #endregion ProcessDefinitionListNode class
}

// eof "ProcessDefinitionListView.cs"
