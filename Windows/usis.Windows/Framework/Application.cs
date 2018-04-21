//
//  @(#) Application.cs
//
//  Project:    usis.Windows
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using usis.Framework;
using usis.Platform;

namespace usis.Windows.Framework
{
    #region Application class

    //  -----------------
    //  Application class
    //  -----------------

    /// <summary>
    /// Represents a modular WPF application that hosts snap-ins.
    /// </summary>

    public class Application : Application<System.Windows.Application>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class
        /// with the specified application configuration.
        /// </summary>
        /// <param name="configuration">
        /// The application configuration.
        /// </param>

        public Application(usis.Framework.Configuration.IApplicationConfiguration configuration) : base(configuration) { }
    }

    #endregion Application class

    #region Application<TApplication> class

    //  -------------------------------
    //  Application<TApplication> class
    //  -------------------------------

    /// <summary>
    /// Provides a class for a modular WPF application that hosts snap-ins.
    /// </summary>
    /// <typeparam name="TApplication">
    /// The type of the WPF application.
    /// </typeparam>

    public class Application<TApplication> : usis.Framework.Application where TApplication : System.Windows.Application, new()
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TApplication}"/> class
        /// with the specified application configuration.
        /// </summary>
        /// <param name="configuration">
        /// The application configuration.
        /// </param>

        public Application(usis.Framework.Configuration.IApplicationConfiguration configuration) : base(null, configuration)
        {
            Extensions.Add(new ApplicationExtension(new TApplication()));
        }

        #endregion construction

        #region methods

        //  ----------
        //  Run method
        //  ----------

        /// <summary>
        /// Start the WPF application
        /// by loading and connecting all registered snap-ins.
        /// </summary>
        /// <remarks>
        /// When the WPF application shuts down,
        /// all snap-ins are disconnected and unloaded.
        /// </remarks>

        public void Run()
        {
            Startup();
            var application = this.With<ApplicationExtension>(true).WindowsApplication;
            if (application?.MainWindow != null)
            {
                application.MainWindow.Show();
                application.Run();
            }
            Shutdown();
        }

        #endregion methods
    }

    #endregion Application<TApplication> class

    #region ApplicationExtension

    //  --------------------------
    //  ApplicationExtension class
    //  --------------------------

    /// <summary>
    /// Provides an application extensions
    /// that allows snap-ins to access the running WPF application.
    /// </summary>

    public class ApplicationExtension : IExtension<IApplication>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationExtension" /> class.
        /// </summary>

        public ApplicationExtension() { WindowsApplication = new System.Windows.Application(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationExtension" /> class
        /// with the specified WPF application.
        /// </summary>
        /// <param name="application">
        /// The WPF application.
        /// </param>

        public ApplicationExtension(System.Windows.Application application)
        {
            WindowsApplication = application ?? throw new ArgumentNullException(nameof(application));
        }

        #endregion construction

        //  ---------------------------
        //  WindowsApplication property
        //  ---------------------------

        /// <summary>
        /// Gets the running WPF application.
        /// </summary>
        /// <value>
        /// The running WPF application.
        /// </value>

        public System.Windows.Application WindowsApplication { get; }

        //  -------------
        //  Attach method
        //  -------------

        /// <summary>
        /// Called when the extension is added to the <see cref="IApplication"/>s
        /// <see cref="IExtensibleObject{T}.Extensions"/> collection.
        /// </summary>
        /// <param name="owner">
        /// The application.
        /// </param>

        public void Attach(IApplication owner) { }

        //  -------------
        //  Detach method
        //  -------------

        /// <summary>
        /// Called when the extension is removed from the <see cref="IApplication"/>s
        /// <see cref="IExtensibleObject{T}.Extensions"/> collection.
        /// </summary>
        /// <param name="owner">
        /// The application.
        /// </param>

        public void Detach(IApplication owner) { }
    }

    #endregion ApplicationExtension
}

// eof "Application.cs"
