//
//  @(#) ServiceHostSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using usis.Platform;

namespace usis.Framework.ServiceModel
{
    #region ServiceHostSnapIn<TFactory> class

    //  --------------------------------------
    //  ServiceHostSnapIn<TFactory> class
    //  --------------------------------------

    /// <summary>
    /// Provides a generic base class for snap-ins, that hosts services.
    /// </summary>
    /// <typeparam name="TFactory">The type of the service host factory.</typeparam>
    /// <seealso cref="ServiceSnapIn" />
    /// <seealso cref="IDisposable" />

    public class ServiceHostSnapIn<TFactory> : ServiceHostSnapIn where TFactory : ServiceHostFactory, new()
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostSnapIn{TConfigurator}"/> class.
        /// </summary>

        public ServiceHostSnapIn() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostSnapIn{TConfigurator}"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>

        protected ServiceHostSnapIn(bool canPauseAndResume) : base(canPauseAndResume) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostSnapIn{TConfigurator}"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>
        /// <param name="openOnConnect">if set to <c>true</c> the service host is opened on connection.</param>

        protected ServiceHostSnapIn(bool canPauseAndResume, bool openOnConnect) : base(canPauseAndResume, openOnConnect) { }

        #endregion construction

        #region overrides

        //  ------------------------
        //  CreateServiceHost method
        //  ------------------------

        /// <summary>
        /// Creates an instance of a <see cref="ServiceHostBase" /> object.
        /// </summary>
        /// <returns>
        /// An newly created <see cref="ServiceHostBase" /> instance.
        /// </returns>

        protected override ServiceHostBase CreateServiceHost()
        {
            var configurator = new TFactory();
            var serviceHostBase = configurator.CreateServiceHost();
            return serviceHostBase;
        }

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            var configurator = new TFactory();
            return string.Format(CultureInfo.CurrentCulture, "{0} - ServiceType={{{1}}}", GetType().Name, configurator.ServiceFullName);
        }

        #endregion overrides
    }

    #endregion ServiceHostSnapIn<TFactory> class

    #region ServiceHostSnapIn class

    //  -----------------------
    //  ServiceHostSnapIn class
    //  -----------------------

    /// <summary>
    /// Provides a base class for snap-ins that hosts services.
    /// </summary>
    /// <seealso cref="ServiceSnapIn" />
    /// <seealso cref="IDisposable" />

    public abstract class ServiceHostSnapIn : ServiceSnapIn, IDisposable
    {
        #region fields

        private ServiceHostBase serviceHost;

        #endregion fields

        #region properties

        //  ----------------------
        //  OpenOnConnect property
        //  ----------------------

        private bool OpenOnConnect { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostSnapIn"/> class.
        /// </summary>

        protected ServiceHostSnapIn() { OpenOnConnect = true; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostSnapIn"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>

        protected ServiceHostSnapIn(bool canPauseAndResume) : base(canPauseAndResume) { OpenOnConnect = true; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostSnapIn"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>
        /// <param name="openOnConnect">if set to <c>true</c> the service host is opened on connection.</param>

        protected ServiceHostSnapIn(bool canPauseAndResume, bool openOnConnect) : base(canPauseAndResume) { OpenOnConnect = openOnConnect; }

        #endregion construction

        #region dispose pattern

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases the unmanaged resources used by the snap-in
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
        /// </param>

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) { Close(); }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceHostSnapIn"/> class.
        /// </summary>

        ~ServiceHostSnapIn()
        {
            Dispose(false);
        }

        #endregion dispose pattern

        #region SnapIn overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            if (OpenOnConnect) Open();
            base.OnConnecting(e);
        }

        //  ----------------
        //  OnPausing method
        //  ----------------

        /// <summary>
        /// Raises the <see cref="ServiceSnapIn.Pausing" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>

        protected override void OnPausing(CancelEventArgs e)
        {
            Close();
            base.OnPausing(e);
        }

        //  -----------------
        //  OnResuming method
        //  -----------------

        /// <summary>
        /// Raises the <see cref="ServiceSnapIn.Resuming" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>

        protected override void OnResuming(CancelEventArgs e)
        {
            Open();
            base.OnResuming(e);
        }

        //  ---------------------
        //  OnDisconnected method
        //  ---------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Disconnected" /> event.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs" /> object that contains the event data.</param>

        protected override void OnDisconnected(EventArgs e)
        {
            Close();
            base.OnDisconnected(e);
        }

        #endregion SnapIn overrides

        #region protected methods

        //  ------------------------
        //  CreateServiceHost method
        //  ------------------------

        /// <summary>
        /// Creates an instance of a <see cref="ServiceHostBase" /> object.
        /// </summary>
        /// <returns>
        /// An newly created <see cref="ServiceHostBase" /> instance.
        /// </returns>
        /// <remarks>
        /// This method must be overridden in a derived class.
        /// </remarks>

        protected abstract ServiceHostBase CreateServiceHost();

        #endregion protected methods

        #region public methods

        //  -----------
        //  Open method
        //  -----------

        /// <summary>
        /// Opens the service host.
        /// </summary>

        public void Open()
        {
            if (serviceHost == null)
            {
                serviceHost = CreateServiceHost();

                // this behavior injects the application object
                serviceHost.Description.Behaviors.Add(new ApplicationServiceBehavior(Application));
            }
            if (serviceHost.State.IsOneOf(CommunicationState.Created, CommunicationState.Closed))
            {
                serviceHost.Open();

                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0} '{1}' opened:", serviceHost.GetType().Name, serviceHost.Description.ServiceType.FullName));
                foreach (var endpoint in serviceHost.Description.Endpoints)
                {
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "- endpoint address: '{0}'", endpoint.Address));
                }
            }
        }

        //  ------------
        //  Close method
        //  ------------

        /// <summary>
        /// Closes the service host.
        /// </summary>

        public void Close()
        {
            if (serviceHost != null)
            {
                if (serviceHost.State == CommunicationState.Opened)
                {
                    serviceHost.Close();
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0} '{1}' closed.", serviceHost.GetType().Name, serviceHost.Description.ServiceType.FullName));
                }
                serviceHost = null;
            }
        }

        #endregion public methods
    }

    #endregion ServiceHostSnapIn class
}

// eof "ServiceHostSnapIn.cs"
