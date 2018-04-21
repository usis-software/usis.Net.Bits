//
//  @(#) Application.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using usis.Framework.Portable.Configuration;
using usis.Platform.Windows;

namespace usis.Framework
{
    //  -----------------
    //  Application class
    //  -----------------

    /// <summary>
    /// Provides a base class for a modular application that hosts snap-ins.
    /// </summary>
    /// <seealso cref="Portable.Application{SnapInActivator}" />

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public abstract class Application : Portable.Application<SnapInActivator>
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
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for the application.</param>

        protected Application(IApplicationConfiguration configuration) : base(configuration) { }

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
        /// <exception cref="Platform.ArgumentNullOrWhiteSpaceException">
        /// <i>sectionName</i> is <b>null</b>, empty, or consists only of white-space characters.
        /// </exception>

        public static ApplicationConfiguration ReadConfigurationFile(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) throw new Platform.ArgumentNullOrWhiteSpaceException(nameof(sectionName));

            var section = ConfigurationManager.GetSection(sectionName) as Configuration.ApplicationConfigurationSection;
            if (section != null)
            {
                var application = new ApplicationConfiguration();
                foreach (var item in section.SnapIns.Cast<Configuration.SnapInConfigurationElement>())
                {
                    var snapIn = new SnapInConfiguration()
                    {
                        TypeName = item.TypeName,
                        AssemblyFile = item.AssemblyFile
                    };
                    application.SnapInsInternal.Add(snapIn);
                }
                return application;
            }
            return null;
        }

        #endregion methods
    }
}

// eof "Application.cs"
