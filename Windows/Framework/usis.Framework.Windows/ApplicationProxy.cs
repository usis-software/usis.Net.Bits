//
//  @(#) ApplicationProxy.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System;
using System.IO;
using usis.Framework.Configuration;
using System.Xml.Serialization;

namespace usis.Framework
{
    //  ----------------------
    //  ApplicationProxy class
    //  ----------------------

    /// <summary>
    /// Provides a proxy object to access an application in another application domain.
    /// </summary>
    /// <seealso cref="MarshalByRefObject" />

    public class ApplicationProxy : MarshalByRefObject
    {
        #region properties

        //  --------------------
        //  Application property
        //  --------------------

        private HostedApplication Application { get; set; }

        //  -----------------
        //  IsPaused property
        //  -----------------

        /// <summary>
        /// Gets a value indicating whether the application is paused.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application is paused; otherwise, <c>false</c>.
        /// </value>

        public bool IsPaused => Application.IsPaused;

        #endregion properties

        #region methods

        //  ------------------------
        //  CreateApplication method
        //  ------------------------

        /// <summary>
        /// Creates an application with the specified application configuration.
        /// </summary>
        /// <param name="configuration">The application configuration as a XML string.</param>

        public void CreateApplication(string configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration)) throw new Platform.ArgumentNullOrWhiteSpaceException(nameof(configuration));
            Application = new HostedApplication(configuration.FromXmlTo<ApplicationConfiguration>());
        } 

        //  ------------
        //  Start method
        //  ------------

        /// <summary>
        /// Starts the hosted application by loading and connecting all snap-ins
        /// and by starting all extensions.
        /// </summary>

        public void Start() { if (Application != null) Application.Start(); }

        //  -----------
        //  Stop method
        //  -----------

        /// <summary>
        /// Stops the hosted application.
        /// </summary>

        public void Stop() { if (Application != null) Application.Stop(); }

        //  ---------------------
        //  PauseOperation method
        //  ---------------------

        /// <summary>
        /// Pauses all operations performed by the hosted application.
        /// </summary>

        public void PauseOperation() { if (Application != null) Application.Pause(); }

        //  ----------------------
        //  ResumeOperation method
        //  ----------------------

        /// <summary>
        /// Resumes all operations performed by the hosted application.
        /// </summary>

        public void ResumeOperation() { if (Application != null) Application.Resume(); }

        #endregion methods

        #region overrides

        //  --------------------------------
        //  InitializeLifetimeService method
        //  --------------------------------

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy
        /// for this instance. This is the current lifetime service object for this instance if one exists;
        /// otherwise, a new lifetime service object initialized to the value of the
        /// <see cref="System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.
        /// </returns>

        public override object InitializeLifetimeService()
        {
            // provide a infinite lifetime
            return null;
        }

        #endregion overrides

        #region HostedApplication class

        //  -----------------------
        //  HostedApplication class
        //  -----------------------

        /// <summary>
        /// The <b>HostedApplication</b> class is needed
        /// because the base class <see cref="Framework.Application" /> is abstract.
        /// </summary>
        /// <seealso cref="Framework.Application" />

        private class HostedApplication : Application
        {
            #region construction

            //  ------------
            //  construction
            //  ------------

            public HostedApplication(IApplicationConfiguration configuration) : base(null, configuration) { }

            #endregion construction

            #region methods

            //  ------------
            //  Start method
            //  ------------

            public void Start() { Startup(); }

            //  -----------
            //  Stop method
            //  -----------

            public bool Stop() { return Shutdown(); }

            #endregion methods
        }

        #endregion HostedApplication class
    }

    #region StringExtensions class

    //  ----------------------
    //  StringExtensions class
    //  ----------------------

    internal static class StringExtensions
    {
        //  -------------------
        //  FromXmlTo<T> method
        //  -------------------

        internal static T FromXmlTo<T>(this string xml) where T : class
        {
            using (var reader = new StringReader(xml))
            {
                return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
            }
        }
    }

    #endregion StringExtensions class
}

// eof "ApplicationProxy.cs"
