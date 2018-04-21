//
//  @(#) AdminService.cs
//
//  Project:    usisAppSvr
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.ServiceProcess;
using usis.Framework;
using usis.Framework.ServiceModel;
using usis.Platform.Windows;

namespace usis.ApplicationServer
{
    //  ------------------
    //  AdminService class
    //  ------------------

    internal class AdminService : Application, IService
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        public AdminService(Service service) : base(null, new Framework.Configuration.ApplicationConfiguration(typeof(AdministrationSnapIn))) { Service = service;}

        #endregion construction

        #region properties

        //  ----------------
        //  Service property
        //  ----------------

        public Service Service { get; }

        #endregion properties

        #region IWindowsService implementation

        //  -------------
        //  Name property
        //  -------------

        public string Name => Constants.ApplicationServerAdministrationServiceName;

        //  ----------------------------
        //  CanPauseAndContinue property
        //  ----------------------------

        public bool CanPauseAndContinue => false;

        //  -------------------------
        //  ConfigureInstaller method
        //  -------------------------

        public void ConfigureInstaller(ServiceInstaller installer)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));

            installer.ServiceName = Name;
            installer.DisplayName = "usis Application Server Administration Service";
            installer.Description = "Provides services to administrate an usis Application Server.";
        }

        //  --------------
        //  OnStart method
        //  --------------

        public void OnStart()
        {
            Extensions.Add(new AdminExtension(this));
            Startup();
        }

        //  --------------
        //  OnPause method
        //  --------------

        public void OnPause() { Pause(); }

        //  -----------------
        //  OnContinue method
        //  -----------------

        public void OnContinue() { Resume(); }

        //  -------------
        //  OnStop method
        //  -------------

        public void OnStop() { Shutdown(); }

        //  -----------------
        //  OnShutdown method
        //  -----------------

        public void OnShutdown() { Shutdown(); }

        #endregion IWindowsService implementation
    }

    #region AdminExtension method

    //  ---------------------
    //  AdminExtension method
    //  ---------------------

    internal class AdminExtension : IApplicationExtension
    {
        //  -----------
        //  constructor
        //  -----------

        internal AdminExtension(AdminService service) { Service = service; }

        //  ----------------
        //  Service property
        //  ----------------

        public AdminService Service { get; private set; }

        #region IApplicationExtension methods

        //  ------------
        //  Start method
        //  ------------

        public void Start(IApplication owner) { }

        //  -------------
        //  Attach method
        //  -------------

        public void Attach(IApplication owner) { }

        //  -------------
        //  Detach method
        //  -------------

        public void Detach(IApplication owner) { }

        #endregion IApplicationExtension methods
    }

    #endregion AdminExtension method

    #region AdministrationSnapIn class

    //  --------------------------
    //  AdministrationSnapIn class
    //  --------------------------

    internal class AdministrationSnapIn : NamedPipeServiceHostSnapIn<AppSvrMgmt, IAppSvrMgmt> { }

    #endregion AdministrationSnapIn class
}

// eof "AdminService.cs"
