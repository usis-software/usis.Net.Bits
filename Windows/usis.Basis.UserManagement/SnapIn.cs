//
//  @(#) SnapIn.cs
//
//  Project:    Basis - User Management
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;
using usis.Framework.Data;
using usis.Framework.Entity;
using usis.Framework.Windows;

namespace usis.Basis.UserManagement
{
    //  ------------
    //  SnapIn class
    //  ------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class SnapIn : ExtensionSnapIn<Model>
    {
        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="Framework.Portable.SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            // configure registry key and settings
            Application.AddRegistrySettings(@"SOFTWARE\usis\Basis\UserManagement");

            // get data source from registry
            var dataSource = Application.AddDataSourceExtensionFromRegistrySettings("DataSource").DataSource;

            // configure Entity Framework
            DBConfiguration.SetDataSourceConfiguration(dataSource);

            // base class implementation adds extension
            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
