//
//  @(#) ServerModel.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using usis.Platform;
using usis.Platform.ServiceModel;
using usis.Platform.Windows;

namespace usis.ApplicationServer.Administration
{
    //  -----------------
    //  ServerModel class
    //  -----------------

    internal class ServerModel : IDisposable
    {
        #region fields

        private string serverName = "localhost";
        private ServiceStatusMonitor monitorAppSvc;
        private ServiceStatusMonitor monitorAdminSvc;

        #endregion fields

        #region properties

        //  ----------------------
        //  ServiceStatus property
        //  ----------------------

        internal ServiceStatus ServiceStatus
        {
            get
            {
                if (monitorAdminSvc.Status != ServiceStatus.Running)
                {
                    return ServiceStatus.Error;
                }
                else return monitorAppSvc.Status;
            }
        }

        //  ---------------------
        //  Applications property
        //  ---------------------

        internal ReloadableCollection<ApplicationInstanceInfo> Applications { get; }

        #endregion properties

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        public ServerModel()
        {
            monitorAppSvc = new ServiceStatusMonitor(Constants.ApplicationServerServiceName, 1000);
            monitorAppSvc.StatusChanged += ChangeServiceStatus;
            monitorAdminSvc = new ServiceStatusMonitor(Constants.ApplicationServerAdministrationServiceName, 1000);
            monitorAdminSvc.StatusChanged += ChangeServiceStatus;

            Applications = new ReloadableCollection<ApplicationInstanceInfo>();
            Applications.PerformReload += (sender, e) =>
            {
                using (var client = CreateClient())
                {
                    Applications.Replace(client.Service.ListApplicationInstances().Iterate());
                }
            };
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (monitorAppSvc != null)
            {
                monitorAppSvc.Dispose();
                monitorAppSvc = null;
            }
            if (monitorAdminSvc != null)
            {
                monitorAdminSvc.Dispose();
                monitorAdminSvc = null;
            }
        }

        #endregion construction/destruction

        #region methods

        //  --------------------------------
        //  ExecuteApplicationCommand method
        //  --------------------------------

        internal ApplicationInstanceInfo ExecuteApplicationCommand(ApplicationInstanceInfo application, ApplicationCommand command)
        {
            using (var client = CreateClient())
            {
                var result = client.Service.ExecuteCommand(application.Id, command);
                Debug.WriteLineIf(result.ReturnCode != 0, result);
                return client.Service.GetApplicationInstanceInfo(application.Id).ReturnValue;
            }
        }

        #endregion methods

        #region private methods

        //  -------------------
        //  CreateClient method
        //  -------------------

        private AppSvrMgmtClient CreateClient()
        {
            return new AppSvrMgmtClient(serverName);
        }

        //  --------------------------
        //  ChangeServiceStatus method
        //  --------------------------

        private void ChangeServiceStatus(object sender, ServiceStatusEventArgs e)
        {
            var status = new ServiceStatusEventArgs(monitorAdminSvc.Status != ServiceStatus.Running ? ServiceStatus.Error : monitorAppSvc.Status);
            ServiceStatusChanged?.Invoke(this, status);
        }

        #endregion private methods

        #region events

        //  --------------------------
        //  ServiceStatusChanged event
        //  --------------------------

        internal event EventHandler<ServiceStatusEventArgs> ServiceStatusChanged;

        #endregion events

        #region AppSvrMgmtClient class

        //  ----------------------
        //  AppSvrMgmtClient class
        //  ----------------------

        private class AppSvrMgmtClient : NamedPipeServiceClient<IAppSvrMgmt>
        {
            internal AppSvrMgmtClient(string server) : base(server, "AppSvrMgmt") { }
        }

        #endregion AppSvrMgmtClient class
    }
}

// eof "ServerModel.cs"
