//
//  @(#) Application.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using usis.Framework.Configuration;
using usis.Framework.Windows;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.Framework
{
    //  -----------------
    //  Application class
    //  -----------------

    /// <summary>
    /// Provides a base class for a modular application that hosts snap-ins.
    /// </summary>
    /// <seealso cref="Application{SnapInActivator}" />

    public abstract class Application : Application<SnapInActivator>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>

        protected Application() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application" /> class
        /// with the specified command line and configuration.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <param name="configuration">The configuration for the application.</param>

        protected Application(CommandLine commandLine, IApplicationConfiguration configuration) : base(commandLine, configuration) { }

        #endregion construction

        #region overrides

        //  ----------------------
        //  ReportException method
        //  ----------------------

        /// <summary>
        /// Allows the application to receive notifications about exceptions that occurred.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>

        public override void ReportException(Exception exception)
        {
            Debug.Print("{0}", exception);

            using (var eventLog = new EventLog())
            {
                eventLog.Log = "Application";
                eventLog.Source = this.GetEventLogSource();
                eventLog.WriteException(exception);
            }
        }

        #endregion overrides

        #region methods

        //  ----------------------------
        //  ReadConfigurationFile method
        //  ----------------------------

        /// <summary>
        /// Reads a configuration from the specified section of the application configuration file.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>
        /// The configuration that was read from the application configuration file.
        /// </returns>
        /// <exception cref="ArgumentNullOrWhiteSpaceException"><c>sectionName</c> is a null reference (<c>Nothing</c> in Visual Basic), empty, or consists only of white-space characters.</exception>

        public static ApplicationConfiguration ReadConfigurationFile(string sectionName)
        {
            var configuration = new ApplicationConfiguration();
            ReadConfigurationFile(configuration, sectionName);
            return configuration;
        }

        /// <summary>
        /// Reads configuration informations from the specified section of the application configuration file.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <exception cref="ArgumentNullException"><c>configuration</c> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        /// <exception cref="ArgumentNullOrWhiteSpaceException"><c>sectionName</c> is a null reference (<c>Nothing</c> in Visual Basic), empty, or consists only of white-space characters.</exception>

        public static void ReadConfigurationFile(ApplicationConfiguration configuration, string sectionName)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentNullOrWhiteSpaceException(nameof(sectionName));

            if (ConfigurationManager.GetSection(sectionName) is ApplicationConfigurationSection section)
            {
                foreach (var item in section.SnapIns.Cast<SnapInConfigurationElement>())
                {
                    var snapIn = new SnapInConfiguration()
                    {
                        TypeName = item.TypeName,
                        AssemblyFile = item.AssemblyFile
                    };
                    configuration.SnapInsInternal.Add(snapIn);
                }
            }
        }

        #endregion methods
    }
}

// eof "Application.cs"
