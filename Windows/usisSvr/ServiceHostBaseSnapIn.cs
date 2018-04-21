//
//	@(#) ServiceHostBaseSnapIn.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using usis.Framework;
using usis.Platform;

namespace usis.Server.Services
{
	#region WebServiceHostSnapIn<TService> class

	//	------------------------------------
	//	WebServiceHostSnapIn<TService> class
	//	------------------------------------

	//internal class WebServiceHostSnapIn<TService> : ServiceHostBaseSnapIn<TService, WebServiceHost> where TService : class
	//{
	//	protected internal override WebServiceHost CreateServiceHostInstance()
	//	{
	//		Uri baseAddress = new Uri(string.Format(CultureInfo.InvariantCulture, "http://localhost/{0}", typeof(TService).Name));
	//		WebServiceHost host = new WebServiceHost(typeof(TService), baseAddress);
	//		return host;
	//	}
	
	//} // WebServiceHostSnapIn<TService> class

	#endregion WebServiceHostSnapIn<TService> class

	#region ServiceHostSnapIn<TService> class

	//internal class ServiceHostSnapIn<TService> : ServiceHostBaseSnapIn<TService, ServiceHost>
	//{
	//	protected internal override ServiceHost CreateServiceHostInstance()
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

	#endregion ServiceHostSnapIn<TService> class

	#region ServiceHostBaseSnapIn<TService, TServiceHost>

	//	---------------------------------------------
	//	ServiceHostBaseSnapIn<TService, TServiceHost>
	//	---------------------------------------------

	//internal abstract class ServiceHostBaseSnapIn<TService, TServiceHost> : SnapIn, IDisposable
	//	where TService : class
	//	where TServiceHost : ServiceHostBase
	//{
	//	#region fields

	//	private TServiceHost serviceHostBase;

	//	#endregion fields

	//	#region ISnapIn interface methods

	//	//	-------------------
	//	//	OnConnecting method
	//	//	-------------------

	//	protected override void OnConnecting(CancelEventArgs e)
	//	{
	//		//if (e == null) throw new ArgumentNullException("e");

	//		//base.OnConnecting(e);
			
	//		//if (!e.Cancel)
	//		{
	//			this.serviceHostBase = this.CreateServiceHostInstance();
	//			this.serviceHostBase.Description.Behaviors.Add(new SolutionServiceBehavior(this.Solution));
	//			this.serviceHostBase.Open();
	//		}
		
	//	} // OnConnecting method

	//	protected override void OnDisconnected(EventArgs e)
	//	{
	//		this.Close();

	//		base.OnDisconnected(e);
	//	}

	//	#endregion ISnapIn interface methods

	//	#region IDisposable interface methods

	//	//	--------------
	//	//	Dispose method
	//	//	--------------

	//	void IDisposable.Dispose()
	//	{
	//		this.Close();
		
	//	} // Dispose method

	//	#endregion IDisposable interface methods

	//	#region abstract methods

	//	//	-------------------------------
	//	// CreateServiceHostInstance method
	//	//	-------------------------------

	//	internal protected abstract TServiceHost CreateServiceHostInstance();

	//	#endregion abstract methods

	//	#region private methods

	//	//	------------
	//	//	Close method
	//	//	------------

	//	private void Close()
	//	{
	//		if (this.serviceHostBase != null)
	//		{
	//			if (this.serviceHostBase.State == CommunicationState.Opened)
	//			{
	//				this.serviceHostBase.Close();
	//			}
	//			this.serviceHostBase = null;
	//		}

	//	} // Close method

	//	#endregion private methods

	//} // ServiceHostBaseSnapIn<TService, TServiceHost>

	#endregion ServiceHostBaseSnapIn<TService, TServiceHost>

	#region InstanceProvider class
	
	//internal class InstanceProvider : SolutionObject, IInstanceProvider
	//{
	//	internal InstanceProvider(ISolution solution) : base(solution) {}

	//	#region IInstanceProvider methods

	//	object IInstanceProvider.GetInstance(InstanceContext instanceContext, Message message)
	//	{
	//		return ((IInstanceProvider)this).GetInstance(instanceContext);
	//	}

	//	object IInstanceProvider.GetInstance(InstanceContext instanceContext)
	//	{
	//		if (instanceContext == null) throw new ArgumentNullException("instanceContext");

	//		Type type = instanceContext.Host.Description.ServiceType;
	//		object instance = Activator.CreateInstance(type);
	//		if (typeof(IInjectable<ISolution>).IsAssignableFrom(type))
	//		{
	//			var injobj = instance as IInjectable<ISolution>;
	//			injobj.Inject(this.Solution);
	//		}
	//		return instance;
	//	}

	//	void IInstanceProvider.ReleaseInstance(InstanceContext instanceContext, object instance)
	//	{
	//		var disposable = instance as IDisposable;
	//		if (disposable != null) disposable.Dispose();
	//	}

	//	#endregion IInstanceProvider methods
	//}

	#endregion InstanceProvider class

	#region SolutionServiceBehavior class

	//internal class SolutionServiceBehavior : SolutionObject, IServiceBehavior
	//{
	//	internal SolutionServiceBehavior(ISolution solution) : base(solution) {}

	//	#region IServiceBehavior methods

	//	void IServiceBehavior.AddBindingParameters(
	//		ServiceDescription serviceDescription,
	//		ServiceHostBase serviceHostBase,
	//		Collection<ServiceEndpoint> endpoints,
	//		BindingParameterCollection bindingParameters)
	//	{
	//	}

	//	//	----------------------------
	//	//	ApplyDispatchBehavior method
	//	//	----------------------------

	//	void IServiceBehavior.ApplyDispatchBehavior(
	//		ServiceDescription serviceDescription,
	//		ServiceHostBase serviceHostBase)
	//	{
	//		if (serviceHostBase == null) throw new ArgumentNullException("serviceHostBase");

	//		IInstanceProvider ip = new InstanceProvider(this.Solution);
	//		foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
	//		{
	//			foreach (var epd in cd.Endpoints)
	//			{
	//				epd.DispatchRuntime.InstanceProvider = ip;
	//			}
	//		}
		
	//	} // ApplyDispatchBehavior method

	//	void IServiceBehavior.Validate(
	//		ServiceDescription serviceDescription,
	//		ServiceHostBase serviceHostBase)
	//	{
	//	}

	//	#endregion IServiceBehavior methods
	//}

	#endregion SolutionServiceBehavior class

} // usis.Server.Services namespace

// eof "ServiceHostBaseSnapIn.cs"