//
//	@(#) SolutionService.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceModel;
using System.ServiceProcess;
using usis.Framework;

namespace usis.Server
{
	//	---------------------
	//	SolutionService class
	//	---------------------

	internal class SolutionService : IWindowsService, ISolution
	{
		#region fields

		private readonly SnapInHost snapInHost = new SnapInHost();
		private ExtensionCollection<ISolution> extensions;

		#endregion // fields

		#region constructor

		//	-----------
		//	constructor
		//	-----------

		internal SolutionService()
		{
			this.snapInHost.RegisterSnapIn(typeof(usis.Server.PushNotification.SnapIn));

			this.Extensions.Add(new EventLogExtension());

		} // constructor

		#endregion constructor

		#region IWindowsService methods

		#region OnStart method

		//	--------------
		//	OnStart method
		//	--------------

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		void IWindowsService.OnStart()
		{
			this.snapInHost.LoadSnapIns(this);
			this.snapInHost.ConnectSnapIns(this);

			// start solution extensions

			if (this.extensions == null) return;
			foreach (var extension in this.extensions)
			{
				ISolutionExtension solutionExtension = extension as ISolutionExtension;
				if (solutionExtension != null)
				{
					try
					{
						solutionExtension.Start(this);
					}
					catch (Exception exception)
					{
						Debug.Print("Failed to start solution extension:\n{0}", exception);
						
						this.UseExtension<EventLogExtension>().Write(exception);
					}
				}
			}

		} // OnStart method

		#endregion OnStart method

		#region OnStop method

		//	-------------
		//	OnStop method
		//	-------------

		void IWindowsService.OnStop()
		{
			// detach all extensions
			if (this.extensions != null) this.extensions.Clear();

			this.snapInHost.DisconnectSnapIns(this);
			this.snapInHost.UnloadSnapIns(this);

		} // OnStop method

		#endregion OnStop method

		#region ConfigureInstaller method

		//	-------------------------
		//	ConfigureInstaller method
		//	-------------------------

		void IWindowsService.ConfigureInstaller(ServiceInstaller installer)
		{
			installer.ServiceName = "usisSolutionSvc";
			installer.DisplayName = "usis Solution Service";
		
		} // ConfigureInstaller method

		#endregion ConfigureInstaller method

		#endregion IWindowsService methods

		#region ISolution methods

		//	-------------------
		//	Extensions property
		//	-------------------

		public IExtensionCollection<ISolution> Extensions
		{
			get
			{
				if (this.extensions == null)
				{
					this.extensions = new ExtensionCollection<ISolution>(this);
				}
				return this.extensions;
			}

		} // Extensions property

		//	----------------------------
		//	ConnectRequiredSnapIn method
		//	----------------------------

		bool ISolution.ConnectRequiredSnapIn(Type snapInType, ISnapIn instance)
		{
			return this.snapInHost.ConnectRequiredSnapIn(this, instance, snapInType);

		} // ConnectRequiredSnapIn method

		//	-------------------
		//	UseExtension method
		//	-------------------

		public TExtension UseExtension<TExtension>() where TExtension : IExtension<ISolution>, new()
		{
			TExtension extension = this.Extensions.Find<TExtension>();
			if (extension == null)
			{
				extension = new TExtension();
				this.Extensions.Add(extension);
			}
			return extension;

		} // UseExtension method

		#endregion ISolution methods

	} // SolutionService class

} // usis.Server namespace

// eof "SolutionService.cs"
