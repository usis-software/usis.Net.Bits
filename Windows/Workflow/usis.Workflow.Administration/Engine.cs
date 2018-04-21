//
//  @(#) Engine.cs
//
//  Project:    usis Workflow Management System
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Platform;

namespace usis.Workflow.Administration
{
    //  ------------
    //  Engine class
    //  ------------

    internal sealed class Engine : IDisposable
    {
        #region fields

        private Session session = new Session();

        #endregion fields

        #region properties

        //  ---------------------------
        //  ProcessDefinitions property
        //  ---------------------------

        internal ReloadableCollection<ProcessDefinition> ProcessDefinitions { get; }

        //  -------------------------
        //  ProcessInstances property
        //  -------------------------

        internal ReloadableCollection<ProcessInstance> ProcessInstances { get; }

        #endregion properties

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        public Engine()
        {
            ProcessDefinitions = new ReloadableCollection<ProcessDefinition>();
            ProcessDefinitions.PerformReload += (sender, e) =>
            {
                ProcessDefinitions.Replace(session.QueryProcessDefinitions());
            };

            ProcessInstances = new ReloadableCollection<ProcessInstance>();
            ProcessInstances.PerformReload += (sender, e) =>
            {
                ProcessInstances.Replace(session.QueryProcessInstances(null));
            };
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (session != null) { session.Dispose(); session = null; }
        }

        #endregion construction/destruction

        #region methods

        //  ------------------------------
        //  CreateProcessDefinition method
        //  ------------------------------

        internal ProcessDefinition CreateProcessDefinition(string name)
        {
            return session.CreateProcessDefinition(name);
        }

        #endregion methods
    }

    #region CollectionInterfaceExtensions

    internal static class CollectionInterfaceExtensions
    {
        internal static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items) { collection.Add(item); }
        }

        internal static void Replace<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.Add(items);
        }
    }

    #endregion CollectionInterfaceExtensions
}

// eof "Engine.cs"
