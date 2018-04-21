//
//  @(#) Program.cs
//
//  Project:    CsvRouter
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.ServiceProcess;
using usis.Platform.Windows;

namespace usis.Server.CsvRouter
{
    internal class Program : IService
    {
        #region Main method

        //  -----------
        //  Main method
        //  -----------

        internal static int Main(string[] args)
        {
            var commandLine = new Platform.CommandLine(args);
            if (commandLine.HasOption("b"))
            {
                return Batch();
            }
            else return ServicesHost.StartServices(args, new Program());
        }

        #endregion Main method

        #region service name and configuration

        //  -------------
        //  Name property
        //  -------------

        public string Name => "CsvRouter";

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        public void ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            installer.ServiceName = Name;
        }

        #endregion service name and configuration

        #region pause and continue

        public bool CanPauseAndContinue => false;

        public void OnPause()
        {
            throw new NotImplementedException();
        }

        public void OnContinue()
        {
            throw new NotImplementedException();
        }

        #endregion pause and continue

        #region fields

        private Dictionary<string, Channel> channels = new Dictionary<string, Channel>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region start and stop

        public void OnStart()
        {
            LoadChannels();
            foreach (var channel in channels.Values)
            {
                channel.StartWatching();
            }
        }

        public void OnStop()
        {
            StopChannels();
        }

        public void OnShutdown()
        {
            StopChannels();
        }

        private void StopChannels()
        {
            foreach (var channel in channels.Values)
            {
                channel.StopWatching();
            }
        }

        #endregion start and stop

        private void LoadChannels()
        {
            channels.Add("Test", new Channel("C:\\", "*.log", true));
        }

        private static int Batch()
        {
            var router = new Program();
            router.LoadChannels();
            foreach (var channel in router.channels)
            {
                channel.Value.ProcessFiles();
            }
            return 0;
        }
    }
}

// eof "Program.cs"
