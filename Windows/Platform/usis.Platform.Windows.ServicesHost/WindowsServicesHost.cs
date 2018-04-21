//
//  @(#) WindowsServicesHost.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Linq;
using System.ServiceProcess;

namespace usis.Platform.Windows
{
    //  -------------------------
    //  WindowsServicesHost class
    //  -------------------------

    internal class WindowsServicesHost : ServicesHostBase, IServicesHost
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal WindowsServicesHost(params IService[] services) : base(services) { }
        
        #endregion construction

        #region Run method

        //  ----------
        //  Run method
        //  ----------

        int IServicesHost.Run()
        {
            ServiceBase.Run(Services.Select((s) => new Service(s)).ToArray());
            return 0;
        }

        #endregion Run method

        #region IDisposable methods

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose() { }

        #endregion IDisposable methods

        #region Service class

        //  -------------
        //  Service class
        //  -------------

        private class Service : ServiceBase
        {
            #region fields

            private readonly WindowsService service;

            #endregion fields

            #region construction

            //  ------------
            //  construction
            //  ------------

            public Service(WindowsService service)
            {
                this.service = service;
                ServiceName = service.Service.Name;
                CanPauseAndContinue = service.Service.CanPauseAndContinue;
                CanShutdown = true;
            }

            #endregion construction

            #region ServiceBase methods

            //  --------------
            //  OnStart method
            //  --------------

            protected override void OnStart(string[] args) { service.Start(); }

            //  -------------
            //  OnStop method
            //  -------------

            protected override void OnStop() { service.Stop(); }

            //  --------------
            //  OnPause method
            //  --------------

            protected override void OnPause() { service.Pause(); }

            //  -----------------
            //  OnContinue method
            //  -----------------

            protected override void OnContinue() { service.Continue(); }

            //  -----------------
            //  OnShutdown method
            //  -----------------

            protected override void OnShutdown() { service.Shutdown(); }

            #endregion ServiceBase methods
        }

        #endregion Service class
    }
}

// eof "WindowsServicesHost.cs"
