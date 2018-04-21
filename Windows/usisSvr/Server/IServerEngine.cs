//
//	@(#) IServerEngine.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.ServiceProcess;
using usis.Platform;

namespace usis.Server
{
	#region IServerEngine interface

	//	------------------------
	//	IServerEngine interface
	//	------------------------

	internal interface IServerEngine : IDisposable
	{
		int Run();

	} // IServerEngine interface

	#endregion // IServerEngine interface

	#region IWindowsService interface

	//	-------------------------
	//	IWindowsService interface
	//	-------------------------

	internal interface IWindowsService
	{
		//string Name
		//{
		//	get;
		//}

		void OnStart();
		void OnStop();

		void ConfigureInstaller(ServiceInstaller installer);

	} // IWindowsService interface

	#endregion IWindowsService interface

	#region ServerEngine class

	//	------------------
	//	ServerEngine class
	//	------------------

	internal abstract class ServerEngine
	{
		#region fields

		private List<IWindowsService> services = new List<IWindowsService>();

		#endregion fields

		#region properties

		//	-----------------
		//	Services property
		//	-----------------

		internal protected IEnumerable<IWindowsService> Services
		{
			get
			{
				return this.services;
			}

		} // Services property

		#endregion properties

		#region construction

		//	------------
		//	construction
		//	------------

		internal ServerEngine()
		{
			this.services.AddRange(ServerEngine.LoadServicesConfiguration());
		
		} // construction

		#endregion construction

		//	--------------------------------
		//	LoadServicesConfiguration method
		//	--------------------------------

		internal static IEnumerable<IWindowsService> LoadServicesConfiguration()
		{
			yield return new SolutionService();
		}

		#region GetServerEngine method

		//	----------------------
		//	GetServerEngine method
		//	----------------------

		private static IServerEngine GetServerEngine(string[] args)
		{
			CommandLine.Initialize(args);

			if (CommandLine.Arguments.IsSwitched("c") ||
				CommandLine.Arguments.IsSwitched("console"))
			{
				return new ConsoleServerEngine();
			}
			else
			{
				return new ServiceServerEngine();
			}

		} // GetServerEngine method

		#endregion GetServerEngine method

		#region Main method

		//	-----------
		//	Main method
		//	-----------

		internal static int Main(string[] args)
		{
			using (IServerEngine engine = ServerEngine.GetServerEngine(args))
			{
				return engine.Run();
			}

		} // Main method

		#endregion Main method

	} // ServerEngine class

	#endregion // ServerEngine class

} // usis.Server namespace

// eof "IServerEngine.cs"
