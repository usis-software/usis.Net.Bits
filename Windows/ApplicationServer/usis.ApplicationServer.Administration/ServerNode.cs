//
//  @(#) ServerNode.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Windows.Forms;
using usis.ManagementConsole;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.ApplicationServer.Administration
{
    //  ----------------
    //  ServerNode class
    //  ----------------

    internal class ServerNode : ScopeNode<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ServerNode() : base(true)
        {
            DisplayName = Strings.ApplicationsNode;
            ViewDescriptions.Add(ApplicationListView.Description);
            EnabledStandardVerbs = StandardVerbs.Refresh;

            Update(SnapIn.ServerModel.ServiceStatus);
            SnapIn.ServerModel.ServiceStatusChanged += ServerNode_ServiceStatusChanged;
        }

        #endregion construction

        #region overrides

        //  ----------------
        //  OnRefresh method
        //  ----------------

        protected override void OnRefresh(AsyncStatus status)
        {
            SnapIn.Console.Invoke(SnapIn.ServerModel.Applications.Reload);
        }

        #endregion overrides

        #region event handlers

        //  --------------------------------------
        //  ServerNode_ServiceStatusChanged method
        //  --------------------------------------

        private void ServerNode_ServiceStatusChanged(object sender, ServiceStatusEventArgs e)
        {
            SnapIn.Invoke(new MethodInvoker(delegate { UpdateApplications(e.Status); }));
        }

        #endregion event handlers

        #region private methods

        //  -------------------------
        //  UpdateApplications method
        //  -------------------------

        private void UpdateApplications(ServiceStatus status)
        {
            Update(status);
            if (status >= ServiceStatus.Running)
            {
                SnapIn.ServerModel.Applications.Reload();
            }
            else SnapIn.ServerModel.Applications.Clear();
        }

        //  -------------
        //  Update method
        //  -------------

        private void Update(ServiceStatus status)
        {
            int imageIndex = 0;
            switch (status)
            {
                case ServiceStatus.Error:
                    imageIndex = 2;
                    break;
                case ServiceStatus.Stopped:
                    imageIndex = 1;
                    break;
                case ServiceStatus.NotInstalled:
                case ServiceStatus.StartPending:
                case ServiceStatus.StopPending:
                case ServiceStatus.Running:
                case ServiceStatus.ContinuePending:
                case ServiceStatus.PausePending:
                case ServiceStatus.Paused:
                default:
                    break;
            }
            SelectedImageIndex = ImageIndex = imageIndex;
        }

        #endregion private methods
    }
}

// eof "ServerNode.cs"
