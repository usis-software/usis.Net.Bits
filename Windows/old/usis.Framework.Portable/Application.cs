//
//  @(#) Application.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Framework.Portable.Configuration;
using usis.Platform.Portable;

namespace usis.Framework.Portable
{
    //  -----------------------------
    //  Application<TActivator> class
    //  -----------------------------

    /// <summary>
    /// Provides a base class for a modular application that hosts snap-ins.
    /// </summary>
    /// <typeparam name="TActivator">The type of the activator to create snap-in instances.</typeparam>
    /// <seealso cref="IApplication" />

    [Obsolete("Use type from usis.Framework namespace instead.")]
    public abstract class Application<TActivator> : IApplication where TActivator : SnapInActivator, new()
    {
        #region fields

        private readonly SnapInHost<TActivator> snapInHost = new SnapInHost<TActivator>();
        private ExtensionCollection<IApplication> extensions;
        private readonly HierarchicalValueStore properties = new HierarchicalValueStore();

        #endregion fields

        #region properties

        //  -----------------
        //  IsPaused property
        //  -----------------

        /// <summary>
        /// Gets a value indicating whether the application is paused.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application is paused; otherwise, <c>false</c>.
        /// </value>

        public bool IsPaused { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TActivator}"/> class.
        /// </summary>

        protected Application() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TActivator}"/> class
        /// with the specified configuration.
        /// </summary>
        /// <param name="configuration">
        /// The configuration for the application.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <c>configuration</c> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        protected Application(IApplicationConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            foreach (var item in configuration.Properties)
            {
                properties.SetValue(new NamedValue(item.Name, item.Value));
            }
            snapInHost.Configure(configuration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application{TActivator}"/> class
        /// with the specified snap-in types.
        /// </summary>
        /// <param name="snapInTypes">The snap-in types.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>snapInTypes</c> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        protected Application(params Type[] snapInTypes) : this(new ApplicationConfiguration(snapInTypes)) { }

        #endregion construction

        #region IApplication implementation

        //  -------------------
        //  Extensions property
        //  -------------------        

        /// <summary>
        /// Gets a collection of all registered extensions.
        /// </summary>
        /// <value>
        /// An <see cref="IExtensionCollection{T}"/> that contains
        /// all registered application extensions.
        /// </value>

        public IExtensionCollection<IApplication> Extensions
        {
            get
            {
                if (extensions == null)
                {
                    extensions = new ExtensionCollection<IApplication>(this);
                }
                return extensions;
            }
        }

        //  -------------------
        //  Properties property
        //  -------------------

        /// <summary>
        /// Gets a collection of application-scope properties.
        /// </summary>
        /// <value>
        /// An <b>IHierarchicalValueStore</b> that contains the application-scope properties.
        /// </value>

        public IHierarchicalValueStore Properties { get { return properties; } }

        //  -------------------------
        //  ConnectedSnapIns property
        //  -------------------------

        /// <summary>
        /// Gets a collection of snap-ins that are connected by the application.
        /// </summary>
        /// <value>
        /// An enumeration of connected snap-ins.
        /// </value>

        public IEnumerable<ISnapIn> ConnectedSnapIns { get { return snapInHost.ConnectedSnapIns; } }

        //  ----------------------------
        //  ConnectRequiredSnapIn method
        //  ----------------------------

        /// <summary>
        /// Connects the specified required snap-ins.
        /// </summary>
        /// <param name="instance">The snap-in that depends on the snap-ins to connect.</param>
        /// <param name="snapInTypes">The types of the snap-ins to connect.</param>
        /// <exception cref="ArgumentNullException"><c>snapInTypes</c> is a null reference.</exception>

        public void ConnectRequiredSnapIns(ISnapIn instance, params Type[] snapInTypes)
        {
            snapInHost.ConnectRequiredSnapIns(this, instance, snapInTypes);
        }

        //  ----------------------
        //  ReportException method
        //  ----------------------

        /// <summary>
        /// Allows the application to receive notifications about exceptions that occurred.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>

        public abstract void ReportException(Exception exception);

        #endregion IApplication implementation

        #region protected methods

        //  --------------
        //  Startup method
        //  --------------        

        /// <summary>
        /// Starts the application by loading and connecting all snap-ins.
        /// After that, each registered extension is also started.
        /// </summary>

        protected void Startup() { snapInHost.Startup(this); }

        //  ---------------
        //  Shutdown method
        //  ---------------

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <returns>
        /// <b>false</b> if at least one snap-in refused to disconnect, otherwise <b>true</b>.
        /// </returns>

        protected bool Shutdown() { return snapInHost.Shutdown(this, false); }

        #endregion protected methods

        #region public methods

        //  ------------
        //  Pause method
        //  ------------

        /// <summary>
        /// Pauses all operations performed by the application and its snap-ins.
        /// </summary>

        public void Pause()
        {
            snapInHost.PauseSnapIns(this);
            IsPaused = true;
        }

        //  -------------
        //  Resume method
        //  -------------

        /// <summary>
        /// Resumes all operations performed by the application and its snap-ins.
        /// </summary>

        public void Resume()
        {
            snapInHost.ResumeSnapIns(this);
            IsPaused = false;
        }

        #endregion public methods
    }
}

// eof "Application.cs"
