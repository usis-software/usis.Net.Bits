//
//	@(#) ServiceServerEngine.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace usis.Server
{
	#region WindowsService class

	//	--------------------
	//	WindowsService class
	//	--------------------

	internal class WindowsService : ServiceBase
	{
		private readonly IWindowsService service;

		public WindowsService(IWindowsService service)
		{
			this.service = service;
		}

		#region ServiceBase methods

		protected override void OnStart(string[] args)
		{
			this.service.OnStart();
		}

		protected override void OnStop()
		{
			this.service.OnStop();
		}

		#endregion  ServiceBase methods

	} // WindowsService class

	#endregion WindowsService class

	//	--------------------------
	//	ServiceServerEngine class
	//	--------------------------

	internal class ServiceServerEngine : ServerEngine, IServerEngine
	{
		#region Run method

		//	----------
		//	Run method
		//	----------

		int IServerEngine.Run()
		{
			List<ServiceBase> services = new List<ServiceBase>();
			foreach (var service in this.Services)
			{
				services.Add(new WindowsService(service));
			}
			ServiceBase.Run(services.ToArray());
		
			return 0;

		} // Run method

		#endregion Run method

		#region IDisposable methods

		//	--------------
		//	Dispose method
		//	--------------

		void IDisposable.Dispose()
		{
		}

		#endregion IDisposable methods

	} // ServiceServerEngine class

} // usis.Server

// eof "ServiceServerEngine.cs"
