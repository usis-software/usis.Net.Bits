//
//  @(#) Application.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using usis.Framework.Configuration;
using usis.Platform;

namespace usis.Framework.Windows.Forms
{
    //  -----------------
    //  Application class
    //  -----------------

    /// <summary>
    /// Represents a modular Windows Forms application that hosts snap-ins.
    /// </summary>
    /// <seealso cref="Framework.Application" />

    public sealed class Application : Framework.Application
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        private Application(CommandLine commandLine, IApplicationConfiguration configuration) : base(commandLine, configuration) { }

        #endregion construction

        #region methods

        //  -------------------------
        //  EnableVisualStyles method
        //  -------------------------

        /// <summary>
        /// Enables visual styles for the application.
        /// </summary>

        public static void EnableVisualStyles() { System.Windows.Forms.Application.EnableVisualStyles(); }

        //  ----------
        //  Run method
        //  ----------

        /// <summary>
        /// Starts a Windows Forms application by loading and connecting the specified snap-ins..
        /// </summary>
        /// <param name="args">The command line arguments to pass to the application.</param>
        /// <param name="snapInTypes">The types of the snap-ins to connect to.</param>
        /// <remarks>
        /// When the Windows Forms application shuts down, all snap-ins are disconnected and unloaded.
        /// </remarks>

        public static void Run(string[] args, params Type[] snapInTypes)
        {
            var application = new Application(new CommandLine(args), new ApplicationConfiguration(snapInTypes));
            application.Startup();
            var extension = application.With<ApplicationExtension>(true);
            if (extension?.MainForm != null)
            {
                System.Windows.Forms.Application.Run(extension.MainForm);
            }
            application.Shutdown();
        }

        #endregion methods
    }

    #region ApplicationExtension

    //  --------------------------
    //  ApplicationExtension class
    //  --------------------------

    /// <summary>
    /// Provides an application extensions
    /// that allows snap-ins to access the main form of a Windows Forms application.
    /// </summary>

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ApplicationExtension : Framework.ApplicationExtension
    {
        //  -----------------
        //  MainForm property
        //  -----------------

        /// <summary>
        /// Gets or sets the main form of the Windows Forms application.
        /// </summary>
        /// <value>
        /// The main form of the Windows Forms application.
        /// </value>

        public Form MainForm { get; set; }
    }

    #endregion ApplicationExtension
}

// eof "Application.cs"
