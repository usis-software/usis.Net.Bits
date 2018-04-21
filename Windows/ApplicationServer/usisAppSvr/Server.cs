//
//  @(#) Server.cs
//
//  Project:    usisAppSvr
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2018 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using usis.Framework.Configuration;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.ApplicationServer
{
    //  -------------
    //  Service class
    //  -------------

    internal sealed class Server : Service
    {
        #region overrides

        #region OnStart method

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called when the service starts.
        /// </summary>
        /// <remarks>
        /// On startup the application server read its configuration file <b>usisAppSvrCfg.xml</b>.
        /// </remarks>

        public override void OnStart()
        {
            if (!CommandLineConfiguration()) ReadAppServerConfigurationFile();

            // start applications
            base.OnStart();
        }

        #endregion OnStart method

        #region ConfigureInstaller method

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        public override void ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            var assembly = Assembly.GetExecutingAssembly();

            installer.ServiceName = Name;
            installer.DisplayName = assembly.GetTitle();;
            installer.Description = assembly.GetDescription();
            installer.StartType = ServiceStartMode.Automatic;
        }

        #endregion ConfigureInstaller method

        #endregion overrides

        #region private methods

        //  -------------------------------
        //  CommandLineConfiguration method
        //  -------------------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool CommandLineConfiguration()
        {
            ConfigurationRoot configuration = null;

            var commandLine = new CommandLine(Environment.GetCommandLineArgs());
            var cfg = commandLine.GetValue("cfg");
            if (!string.IsNullOrWhiteSpace(cfg) && File.Exists(cfg))
            {
                try
                {
                    configuration = Load(cfg);
                    if (configuration != null)
                    {
                        foreach (var application in configuration.Applications)
                        {
                            application.StartType = ApplicationStartMode.Automatic;
                            if (string.IsNullOrWhiteSpace(application.ApplicationBase))
                            {
                                application.ApplicationBase = Path.GetDirectoryName(Path.GetFullPath(cfg));
                            }
                        }
                        Configure(configuration);
                    }
                }
                catch (Exception exception)
                {
                    ReportException(exception);
                }
            }
            return configuration != null;
        }

        //  -------------------------------------
        //  ReadAppServerConfigurationFile method
        //  -------------------------------------

        private void ReadAppServerConfigurationFile()
        {
            ConfigurationRoot configuration = null;

            // read configuration file from application data folder
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var filePath = Path.Combine(path, "usisAppSvrCfg.xml");
            if (File.Exists(filePath))
            {
                configuration = Load(filePath);
                var bakFile = Path.ChangeExtension(filePath, "bak");
                File.Copy(filePath, bakFile, true);
            }
            if (configuration != null) Configure(configuration);

            // recreate the configuration from hosted applications
            var tmpFile = Path.ChangeExtension(filePath, "tmp");
            configuration = CreateConfiguration();
            using (var writer = new StreamWriter(tmpFile)) configuration.Save(writer);

            // put updated configuration in place
            File.Copy(tmpFile, filePath, true);
            File.Delete(tmpFile);
        }

        //  -----------
        //  Load method
        //  -----------

        private static ConfigurationRoot Load(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return ConfigurationRoot.Load(reader);
            }
        }

        //  ----------------------
        //  ReportException method
        //  ----------------------

        private static void ReportException(Exception exception)
        {
            Debug.WriteLine(exception);

            using (var eventLog = new EventLog())
            {
                eventLog.Log = "Application";
                eventLog.Source = "usisAppSvr";
                eventLog.WriteException(exception);
            }
        }

        #endregion private methods
    }
}

// eof "Server.cs"
