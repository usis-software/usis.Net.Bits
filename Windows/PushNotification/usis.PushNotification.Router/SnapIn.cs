//
//  @(#) SnapIn.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using PushSharp.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using usis.Framework;
using usis.Framework.Data;
using usis.Framework.Data.Entity;
using usis.Framework.Windows;

namespace usis.PushNotification
{
    //  ------------
    //  SnapIn class
    //  ------------

    /// <summary>
    /// Configures the usis Push Notification Router when connected.
    /// </summary>

    public class SnapIn : ServiceSnapIn
    {
        #region constants

        private const string CompanyDirectory = "usis";
        private const string ProductDirectory = "PNRouter";
        private const string DataDirectoryPropertyName = "DataDirectory";

        #endregion constants

        #region properties

        //  -----------------------------
        //  ProgramDataDirectory property
        //  -----------------------------

        internal static string ProgramDataDirectory => Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData),
                    CompanyDirectory, ProductDirectory);

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="Framework.SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            // configure PushSharp logging
            ConfigurePushSharp();

            // set the name of the event log source
            Application.SetEventLogSource(Constants.ServiceName);

            // create and define the directory for storing data
            if (!Directory.Exists(ProgramDataDirectory)) Directory.CreateDirectory(ProgramDataDirectory);
            AppDomain.CurrentDomain.SetData(DataDirectoryPropertyName, ProgramDataDirectory);

            // configure registry key and settings
            Application.AddRegistrySettings(@"Software\usis\PNRouter");

            // get data source from registry
            var dataSourceExtension = Application.AddDataSourceExtensionFromRegistrySettings("DataSource");
            if (dataSourceExtension != null)
            {
                // configure Entity Framework
                DBConfiguration.SetDataSourceConfiguration(dataSourceExtension.DataSource);
            }

            // add model and router extensions (business logic)
            var model = new Model();
            model.DatabaseInitialized += Model_DatabaseInitialized;
            Application.Extensions.Add(model);
            Application.Extensions.Add(new Router());

            // connect WCF service snap-ins
            Application.ConnectRequiredSnapIns(this, typeof(RouterMgmtSnapIn));

            base.OnConnecting(e);
        }

        //  ---------------
        //  OnPaused method
        //  ---------------

        /// <summary>
        /// Raises the <see cref="E:usis.Framework.ServiceSnapIn.Paused" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>

        protected override void OnPaused(EventArgs e)
        {
            Application.Extensions.Find<Router>()?.Pause();

            base.OnPaused(e);
        }

        //  ----------------
        //  OnResumed method
        //  ----------------

        /// <summary>
        /// Raises the <see cref="E:usis.Framework.ServiceSnapIn.Resumed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>

        protected override void OnResumed(EventArgs e)
        {
            Application.Extensions.Find<Router>()?.Resume();

            base.OnResumed(e);
        }

        #endregion overrides

        #region event handlers

        //  --------------------------------
        //  Model_DatabaseInitialized method
        //  --------------------------------

        private void Model_DatabaseInitialized(object sender, EventArgs e)
        {
            foreach (var snapIn in Application.ConnectedSnapIns)
            {
                var serviceHostSnapIn = snapIn as Framework.ServiceModel.ServiceHostSnapIn;
                serviceHostSnapIn?.Open();
            }
        }

        #endregion event handlers

        #region PushSharp

        //  -------------------------
        //  ConfigurePushSharp method
        //  -------------------------

        private static void ConfigurePushSharp()
        {
            Log.ClearLoggers();
            Log.AddLogger(new TraceLogger());
        }

        //  -----------------
        //  TraceLogger class
        //  -----------------

        private class TraceLogger : ILogger
        {
            //  ------------
            //  Write method
            //  ------------

            public void Write(LogLevel level, string msg, params object[] args)
            {
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture,
                    "PushSharp[{0}] {1}", level,
                    string.Format(CultureInfo.CurrentCulture, msg, args)));
            }
        }

        #endregion PushSharp
    }
}

// eof "SnapIn.cs"
