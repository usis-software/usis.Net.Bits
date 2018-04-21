//
//  @(#) SnapIn.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.IO;
using usis.ApplicationServer;
using usis.Framework.Configuration;
using usis.Platform.Windows;
using usis.Platform;
using usis.Framework.ServiceModel;

namespace IZYTRON.IQ
{
    //  ------------
    //  SnapIn class
    //  ------------

    internal sealed class SnapIn : usis.Framework.SnapIn
    {
        #region constants

        private const string ServiceName = "IZYTRON.IQ.SyncSvc";
        private const string DataDirectoryPropertyName = "DataDirectory";

        #endregion constants

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

        #region Main method

        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine(Strings.ServiceStarted);
            Console.WriteLine();

            ServicesHost.StartServicesOrConsole(args, new Service(ServiceName, new ConfigurationRoot(typeof(SnapIn))));
        }

        #endregion Main method

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            // create and define the directory for storing data
            if (!Directory.Exists(ProgramDataDirectory)) Directory.CreateDirectory(ProgramDataDirectory);
            AppDomain.CurrentDomain.SetData(DataDirectoryPropertyName, ProgramDataDirectory);
            Application.Properties.SetValue(nameof(ProgramDataDirectory), ProgramDataDirectory);

            // create model (an application extension)
            Application.Extensions.Add(new Model() { IsLoggingEnabled = true });

            // set event log source

            // connect snap-ins
            Application.ConnectRequiredSnapIns(this,
                typeof(ServiceHostSnapIn<WcfServiceHostFactory<SyncService>>),
                typeof(BitsSnapIn));

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
