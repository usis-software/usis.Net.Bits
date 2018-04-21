//
//  @(#) ServiceHostBaseSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using usis.Framework.Portable;
using usis.Platform.Portable;

namespace usis.Framework.ServiceModel
{
    #region ServiceHostBaseSnapIn<TConfigurator> class

    //  ------------------------------------------
    //  ServiceHostBaseSnapIn<TConfigurator> class
    //  ------------------------------------------

    /// <summary>
    /// Provides a generic base class for snap-ins, that hosts services.
    /// </summary>
    /// <typeparam name="TConfigurator">The type of the service host configurator.</typeparam>
    /// <seealso cref="ServiceSnapIn" />
    /// <seealso cref="IDisposable" />

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public class ServiceHostBaseSnapIn<TConfigurator> : ServiceHostBaseSnapInBase
        where TConfigurator : ServiceHostBaseConfigurator, new()
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBaseSnapIn{TConfigurator}"/> class.
        /// </summary>

        public ServiceHostBaseSnapIn() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBaseSnapIn{TConfigurator}"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>

        protected ServiceHostBaseSnapIn(bool canPauseAndResume) : base(canPauseAndResume) { }

        #endregion construction

        #region overrides

        //  --------------------------------
        //  CreateServiceHostInstance method
        //  --------------------------------

        /// <summary>
        /// Creates an <see cref="ServiceHostBase" /> instance.
        /// </summary>
        /// <returns>
        /// An newly created <see cref="ServiceHostBase" /> instance.
        /// </returns>

        protected override ServiceHostBase CreateServiceHostInstance()
        {
            var configurator = new TConfigurator();
            var serviceHostBase = configurator.CreateServiceHostInstance();
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
            var configurator = new TConfigurator();
            return string.Format(CultureInfo.CurrentCulture, "{0} - ServiceType={{{1}}}", GetType().Name, configurator.ServiceFullName);
        }

        #endregion overrides
    }

    #endregion ServiceHostBaseSnapIn<TConfigurator> class

    #region ServiceHostBaseSnapInBase class

    //  -------------------------------
    //  ServiceHostBaseSnapInBase class
    //  -------------------------------

    /// <summary>
    /// Provides a base class for snap-ins, that hosts services.
    /// </summary>
    /// <seealso cref="ServiceSnapIn" />
    /// <seealso cref="IDisposable" />

    [Obsolete("Use type from usis.Framework.ServiceModel namespace instead.")]
    public abstract class ServiceHostBaseSnapInBase : ServiceSnapIn, IDisposable
    {
        #region fields

        private ServiceHostBase serviceHostBase;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBaseSnapInBase"/> class.
        /// </summary>

        protected ServiceHostBaseSnapInBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBaseSnapInBase"/> class.
        /// </summary>
        /// <param name="canPauseAndResume">if set to <c>true</c> the snap-in can pause and resume.</param>

        protected ServiceHostBaseSnapInBase(bool canPauseAndResume) : base(canPauseAndResume) { }

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
        /// Finalizes an instance of the <see cref="ServiceHostBaseSnapInBase"/> class.
        /// </summary>

        ~ServiceHostBaseSnapInBase()
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
            Open();
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

        //  --------------------------------
        //  CreateServiceHostInstance method
        //  --------------------------------

        /// <summary>
        /// Creates an <see cref="ServiceHostBase"/> instance.
        /// </summary>
        /// <returns>
        /// An newly created <see cref="ServiceHostBase"/> instance.
        /// </returns>

        protected abstract ServiceHostBase CreateServiceHostInstance();

        #endregion protected methods

        #region private methods

        //  -----------
        //  Open method
        //  -----------

        private void Open()
        {
            serviceHostBase = CreateServiceHostInstance();
            serviceHostBase.Description.Behaviors.Add(new ApplicationServiceBehavior(Application));
            serviceHostBase.Open();

#if DEBUG
            Debug.Print("{0} '{1}' opened:", serviceHostBase.GetType().Name, serviceHostBase.Description.ServiceType.FullName);
            foreach (var endpoint in serviceHostBase.Description.Endpoints)
            {
                Debug.Print("- endpoint address: '{0}'", endpoint.Address);
            }
#endif
        }

        //  ------------
        //  Close method
        //  ------------

        private void Close()
        {
            if (serviceHostBase != null)
            {
                if (serviceHostBase.State == CommunicationState.Opened)
                {
                    serviceHostBase.Close();
                    Debug.Print("{0} '{1}' closed.", serviceHostBase.GetType().Name, serviceHostBase.Description.ServiceType.FullName);
                }
                serviceHostBase = null;
            }
        }

        #endregion private methods
    }

    #endregion ServiceHostBaseSnapInBase class

    #region ServiceHostBaseConfigurator<TService> class

    //  -------------------------------------------
    //  ServiceHostBaseConfigurator<TService> class
    //  -------------------------------------------

    /// <summary>
    /// Provides a generic base class to configure service hosts.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>

    public abstract class ServiceHostBaseConfigurator<TService> : ServiceHostBaseConfigurator
    {
        //  ------------------------
        //  ServiceFullName property
        //  ------------------------

        /// <summary>
        /// Gets the full type name of the service, including namespace.
        /// </summary>
        /// <value>
        /// The full type name of the service, including namespace.
        /// </value>

        public override string ServiceFullName { get { return typeof(TService).FullName; } }
    }

    #endregion ServiceHostBaseConfigurator<TService> class

    #region ServiceHostBaseConfigurator class

    //  ---------------------------------
    //  ServiceHostBaseConfigurator class
    //  ---------------------------------

    /// <summary>
    /// Provides a base class to configure service hosts.
    /// </summary>

    public abstract class ServiceHostBaseConfigurator
    {
        //  -------------------------
        //  CreateServiceHostInstance
        //  -------------------------

        /// <summary>
        /// Creates an instance of a class that derives from <see cref="ServiceHostBase" />
        /// and hosts the <b>TService</b> service.
        /// </summary>
        /// <returns>
        /// An <b>ServiceHostBase</b> that hosts the service of this snap-in.
        /// </returns>

        public abstract ServiceHostBase CreateServiceHostInstance();

        //  ---------------
        //  Scheme property
        //  ---------------

        /// <summary>
        /// Gets the scheme name through which the service is accessed.
        /// </summary>
        /// <value>
        /// The scheme name through which the service is accessed.
        /// </value>

        protected abstract string Scheme { get; }

        //  -------------
        //  Port property
        //  -------------

        /// <summary>
        /// Gets the port number through which the service is accessed.
        /// </summary>
        /// <value>
        /// The port number through which the service is accessed.
        /// </value>

        protected virtual int? Port { get { return null; } }

        //  ------------------------
        //  CreateBaseAddress method
        //  ------------------------

        /// <summary>
        /// Creates the base address for the service.
        /// </summary>
        /// <returns>
        /// The base address for the service.
        /// </returns>

        public virtual Uri CreateBaseAddress()
        {
            var host = System.Net.Dns.GetHostName();
            var port = Port.HasValue ? string.Format(CultureInfo.InvariantCulture, ":{0}", Port.Value) : string.Empty;
            return new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}/", Scheme, host, port));
        }

        //  ---------------------------
        //  CreateServiceAddress method
        //  ---------------------------

        /// <summary>
        /// Creates the service address.
        /// </summary>
        /// <param name="servicePath">The path part of the service URL.</param>
        /// <returns>
        /// The full URL of the service.
        /// </returns>

        public virtual Uri CreateServiceAddress(string servicePath)
        {
            var relative = new Uri(servicePath, UriKind.Relative);
            return new Uri(CreateBaseAddress(), relative);
        }

        //  ---------------------------
        //  CreateChannelBinding method
        //  ---------------------------

        /// <summary>
        /// Creates a binding for the service endpoint.
        /// </summary>
        /// <returns>
        /// A newly created binding for the service endpoint.
        /// </returns>

        public virtual Binding CreateChannelBinding() { return null; }

        //  ------------------------
        //  ServiceFullName property
        //  ------------------------

        /// <summary>
        /// Gets the full type name of the service, including namespace.
        /// </summary>
        /// <value>
        /// The full type name of the service, including namespace.
        /// </value>

        public abstract string ServiceFullName { get; }
    }

    #endregion ServiceHostBaseConfigurator class

    #region ApplicationServiceBehavior class

    //  --------------------------------
    //  ApplicationServiceBehavior class
    //  --------------------------------

    [Obsolete("Use type from usis.Framework.ServiceModel namespace instead.")]
    internal class ApplicationServiceBehavior : IServiceBehavior
    {
        #region fields

        private IApplication application;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ApplicationServiceBehavior(IApplication application)
        {
            this.application = application;
        }

        #endregion construction

        #region IServiceBehavior methods

        //  ---------------------------
        //  AddBindingParameters method
        //  ---------------------------

        void IServiceBehavior.AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        { }

        //  ----------------------------
        //  ApplyDispatchBehavior method
        //  ----------------------------

        void IServiceBehavior.ApplyDispatchBehavior(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            if (serviceHostBase == null) throw new ArgumentNullException(nameof(serviceHostBase));

            IInstanceProvider ip = new InstanceProvider(application);
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (var epd in cd.Endpoints)
                {
                    epd.DispatchRuntime.InstanceProvider = ip;
                }
            }
        }

        //  ---------------
        //  Validate method
        //  ---------------

        void IServiceBehavior.Validate(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        { }

        #endregion IServiceBehavior methods
    }

    #endregion ApplicationServiceBehavior class

    #region InstanceProvider class

    //  ----------------------
    //  InstanceProvider class
    //  ----------------------

    [Obsolete("Use type from usis.Framework namespace instead.")]
    internal class InstanceProvider : IInstanceProvider
    {
        #region fields

        private IApplication application;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal InstanceProvider(IApplication application)
        {
            this.application = application;
        }

        #endregion construction

        #region IInstanceProvider methods

        //  ------------------
        //  GetInstance method
        //  ------------------

        object IInstanceProvider.GetInstance(InstanceContext instanceContext, Message message)
        {
            return ((IInstanceProvider)this).GetInstance(instanceContext);
        }

        object IInstanceProvider.GetInstance(InstanceContext instanceContext)
        {
            if (instanceContext == null) throw new ArgumentNullException(nameof(instanceContext));

            var type = instanceContext.Host.Description.ServiceType;
            var instance = Activator.CreateInstance(type);
            var injectable = instance as IInjectable<IApplication>;
            if (injectable != null) injectable.Inject(application);
            return instance;
        }

        //  ----------------------
        //  ReleaseInstance method
        //  ----------------------

        void IInstanceProvider.ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null) disposable.Dispose();
        }

        #endregion IInstanceProvider methods
    }

    #endregion InstanceProvider class
}

// eof "ServiceHostBaseSnapIn.cs"
