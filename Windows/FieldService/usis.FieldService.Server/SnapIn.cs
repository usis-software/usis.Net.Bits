//
//  @(#) SnapIn.cs
//
//  Project:    usis.FieldService.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using usis.Framework;
using usis.Framework.Data;
using usis.Framework.Data.Entity;
using usis.Framework.ServiceModel;

namespace usis.FieldService.Server
{
    //  ------------
    //  SnapIn class
    //  ------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class SnapIn : ExtensionSnapIn<Model>
    {
        #region properties

        //  -----------------------------
        //  ProgramDataDirectory property
        //  -----------------------------

        internal static string ProgramDataDirectory
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    Constants.CompanyDirectory,
                    Constants.ProductDirectory);
            }
        }

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            // create and define the directory for storing data
            if (!Directory.Exists(ProgramDataDirectory)) Directory.CreateDirectory(ProgramDataDirectory);
            AppDomain.CurrentDomain.SetData(Constants.DataDirectoryPropertyName, ProgramDataDirectory);

            // add data source as an extension and configure the Entity Framework
            var extension = Application.AddDataSourceExtensionFromConnectionStrings(Constants.ConnectionStringName);
            DBConfiguration.SetDataSourceConfiguration(extension.DataSource);

            // connect to the web service snap-ins
            Application.ConnectRequiredSnapIns(this, typeof(ServiceHostSnapIn<WcfServiceHostFactory<Server>>));

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
