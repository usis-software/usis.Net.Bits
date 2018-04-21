//
//  @(#) SnapIn.cs
//
//  Project:    usis Workflow Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.IO;
using usis.Framework;
using usis.Framework.Data;
using usis.Framework.Data.Entity;
using usis.Framework.Windows;

namespace usis.Workflow.Engine
{
    //  ------------
    //  SnapIn class
    //  ------------

    public sealed class SnapIn : ServiceSnapIn
    {
        #region constants

        private const string CompanyDirectory = "usis";
        private const string ProductDirectory = "WFEngine";
        private const string DataDirectoryPropertyName = "DataDirectory";

        #endregion constants

        #region properties

        //  -----------------------------
        //  ProgramDataDirectory property
        //  -----------------------------

        internal static string ProgramDataDirectory => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    CompanyDirectory, ProductDirectory);

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            // set the name of the event log source
            Application.SetEventLogSource("usis.Workflow.Engine");

            // create and define the directory for storing data
            if (!Directory.Exists(ProgramDataDirectory)) Directory.CreateDirectory(ProgramDataDirectory);
            AppDomain.CurrentDomain.SetData(DataDirectoryPropertyName, ProgramDataDirectory);

            // configure registry key and settings
            Application.AddRegistrySettings(@"Software\usis\Workflow");

            // get data source from registry
            var dataSourceExtension = Application.AddDataSourceExtensionFromRegistrySettings(@"Engine\DataSource");
            if (dataSourceExtension != null)
            {
                // configure Entity Framework
                DBConfiguration.SetDataSourceConfiguration(dataSourceExtension.DataSource);
            }
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Migrations.Configuration>(true));

            // add Engine application extension
            Application.Extensions.Add(new Engine());

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
