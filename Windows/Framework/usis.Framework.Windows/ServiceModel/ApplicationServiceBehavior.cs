//
//  @(#) ApplicationServiceBehavior.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using usis.Platform;

namespace usis.Framework.ServiceModel
{
    //  --------------------------------
    //  ApplicationServiceBehavior class
    //  --------------------------------

    internal class ApplicationServiceBehavior : IServiceBehavior
    {
        #region properties

        //  --------------------
        //  Application property
        //  --------------------

        private IApplication Application { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ApplicationServiceBehavior(IApplication application) { Application = application; }

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

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceHostBase == null) throw new ArgumentNullException(nameof(serviceHostBase));

            IInstanceProvider ip = new InstanceProvider(Application);
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

    #region InstanceProvider class

    //  ----------------------
    //  InstanceProvider class
    //  ----------------------

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
            if (instance is IInjectable<IApplication> injectable) injectable.Inject(application);
            return instance;
        }

        //  ----------------------
        //  ReleaseInstance method
        //  ----------------------

        void IInstanceProvider.ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance is IDisposable disposable) disposable.Dispose();
        }

        #endregion IInstanceProvider methods
    }

    #endregion InstanceProvider class
}

// eof "ApplicationServiceBehavior.cs"
