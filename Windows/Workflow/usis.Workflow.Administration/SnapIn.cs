//
//  @(#) SnapIn.cs
//
//  Project:    usis Workflow Management System
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.ManagementConsole;
using usis.ManagementConsole;

namespace usis.Workflow.Administration
{
    //  ------------
    //  SnapIn class
    //  ------------

    /// <summary>
    /// Provides a MMC Snap-In to administrate an <i>usis Workflow Engine</i>.
    /// </summary>

    [SnapInSettings(
        "5219BD1F-127E-43DD-9C67-65E4C737DFCE",
         DisplayName = "usis Workflow Engine",
         Description = "The usis Workflow Management Console allows you to administrate an usis Worklow Engine.",
         Vendor = "usis GmbH")]
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class SnapIn : Microsoft.ManagementConsole.SnapIn, IDisposable
    {
        #region properties

        //  ---------------
        //  Engine property
        //  ---------------

        private Engine engine = new Engine();

        internal Engine Engine { get { return engine; } }

        #endregion properties

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        public SnapIn() { }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (engine != null) { engine.Dispose(); engine = null; }
        }

        #endregion construction/destruction

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize()
        {
            RootNode = new Microsoft.ManagementConsole.ScopeNode(true)
            {
                DisplayName = Strings.WorkflowEngine
            };
            RootNode.ViewDescriptions.Add(new FormViewDescription()
            {
                DisplayName = Strings.WorkflowEngine,
                ViewType = typeof(FormView),
                ControlType = typeof(FormViewControl)
            });

            RootNode.Children.Add(new ProcessDefinitionListNode(), new ProcessInstanceListNode());
        }

        //  -----------------
        //  OnShutdown method
        //  -----------------

        protected override void OnShutdown(AsyncStatus status) { Dispose(); }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
