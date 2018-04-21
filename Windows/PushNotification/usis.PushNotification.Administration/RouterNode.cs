//
//  @(#) RouterNode.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using usis.Framework.ManagementConsole;
using usis.ManagementConsole;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.PushNotification.Administration
{
    //  ----------------
    //  RouterNode class
    //  ----------------

    internal sealed class RouterNode : ScopeNode<SnapIn>
    {
        #region fields

        private Action actionStart;
        private Action actionStop;
        private Action actionRestart;
        private Action actionBackup;
        private Action actionRestore;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public RouterNode() : base(true)
        {
            DisplayName = Strings.RootNodeDisplayName;

            // allow refresh
            EnabledStandardVerbs = StandardVerbs.Refresh;

            // add start view
            ViewDescriptions.Add(StartView.Description);

            // update image
            UpdateImage(SnapIn.Router.ServiceStatus);
            SnapIn.Router.ServiceStatusChanged += Router_ServiceStatusChanged;

            // add scope nodes
            var channelTypes = SnapIn.Router.RegisteredChannelTypes;
            if (channelTypes != null)
            {
                if (channelTypes.Contains(ChannelType.ApplePushNotificationService)) Children.Add(new ApnsChannelListNode());
                if (channelTypes.Contains(ChannelType.WindowsNotificationService)) Children.Add(new WnsChannelListNode());
            }

            // add service actions
            actionStart = ActionsPaneItems.AddAction(Strings.ActionStartService, SnapIn.ImageListIndexStart, (e) => ControlService(ServiceCommand.Start));
            actionStop = ActionsPaneItems.AddAction(Strings.ActionStopService, SnapIn.ImageListIndexStop, (e) => ControlService(ServiceCommand.Stop));
            actionRestart = ActionsPaneItems.AddAction(Strings.ActionRestartService, SnapIn.ImageListIndexRestart, (e) => ControlService(ServiceCommand.Restart));

            // add backup/restore actions
            actionBackup =  ActionsPaneItems.AddAction(Strings.ActionBackup, (e) => Backup());
            actionRestore = ActionsPaneItems.AddAction(Strings.ActionRestore, (e) => Restore());

            // set action state
            UpdateActions(SnapIn.Router.ServiceStatus);
        }

        #endregion construction

        #region event handlers

        //  ----------------------------------
        //  Router_ServiceStatusChanged method
        //  ----------------------------------

        private void Router_ServiceStatusChanged(object sender, ServiceStatusEventArgs e)
        {
            SnapIn.Invoke(new MethodInvoker(delegate { Update(e.Status); }));
        }

        #endregion event handlers

        #region overrides

        //  ----------------
        //  OnRefresh method
        //  ----------------

        protected override void OnRefresh(AsyncStatus status)
        {
            Update(SnapIn.Router.ServiceStatus);
        }

        #endregion overrides

        #region methods

        //  -------------
        //  Update method
        //  -------------

        private void Update(ServiceStatus status)
        {
            UpdateImage(status);
            UpdateActions(status);
            SnapIn.Router.ApnsChannels.Reload(false);
            SnapIn.Router.WnsChannels.Reload(false);
        }

        //  ------------------
        //  UpdateImage method
        //  ------------------

        private void UpdateImage(ServiceStatus status)
        {
            int imageIndex;
            switch (status)
            {
                case ServiceStatus.Error:
                    imageIndex = SnapIn.ImageListIndexGearError;
                    break;
                case ServiceStatus.Stopped:
                case ServiceStatus.StartPending:
                case ServiceStatus.StopPending:
                    imageIndex = SnapIn.ImageListIndexGearStop;
                    break;
                case ServiceStatus.Running:
                case ServiceStatus.ContinuePending:
                case ServiceStatus.PausePending:
                case ServiceStatus.Paused:
                    imageIndex = SnapIn.ImageListIndexGearRun;
                    break;
                case ServiceStatus.NotInstalled:
                default:
                    imageIndex = SnapIn.ImageListIndexUsisIcon;
                    break;
            }
            if (imageIndex != ImageIndex)
            {
                SelectedImageIndex = ImageIndex = imageIndex;
            }
        }

        //  --------------------
        //  UpdateActions method
        //  --------------------

        private void UpdateActions(ServiceStatus status)
        {
            actionStart.Enabled = status == ServiceStatus.Stopped;
            actionStop.Enabled = status == ServiceStatus.Running;
            actionRestart.Enabled = status == ServiceStatus.Running;

            actionBackup.Enabled = SnapIn.Router.IsConnected;
            actionRestore.Enabled = SnapIn.Router.IsConnected;
        }

        //  --------------------------
        //  ServiceCommand enumeration
        //  --------------------------

        private enum ServiceCommand
        {
            Start,
            Stop,
            Restart
        }

        //  ---------------------
        //  ControlService method
        //  ---------------------

        private void ControlService(ServiceCommand command)
        {
            SnapIn.Console.Invoke(() =>
            {
                using (var controller = new ServiceController(Constants.ServiceName))
                {
                    switch (command)
                    {
                        case ServiceCommand.Start:
                            controller.Start();
                            controller.WaitForStatus(ServiceControllerStatus.Running);
                            break;
                        case ServiceCommand.Stop:
                            controller.Stop();
                            controller.WaitForStatus(ServiceControllerStatus.Stopped);
                            break;
                        case ServiceCommand.Restart:
                            controller.Stop();
                            controller.WaitForStatus(ServiceControllerStatus.Stopped);
                            controller.Start();
                            controller.WaitForStatus(ServiceControllerStatus.Running);
                            break;
                        default:
                            break;
                    }
                }
            });
        }

        //  -------------
        //  Backup method
        //  -------------

        private void Backup()
        {
            SnapIn.Console.Invoke(() =>
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Title = Strings.TitleBackup;
                    dialog.FileName = Strings.BackupFileName;
                    dialog.Filter = Strings.BackupFilter;
                    if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                    {
                        var result = SnapIn.Router.Backup(dialog.FileName);
                        if (result.Succeeded)
                        {
                            using (var waitDialog = new WaitDialog())
                            {
                                waitDialog.DisplayDelay = System.TimeSpan.Zero;
                                waitDialog.Title = Strings.TitleBackup;
                                waitDialog.WaitForJob(SnapIn, () =>
                                {
                                    return SnapIn.Router.GetJobProgress(result.ReturnValue).ReturnValue;
                                });
                            }
                        }
                        else SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
                    }
                }
            });
        }

        //  --------------
        //  Restore method
        //  --------------

        private void Restore()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = Strings.TitleRestore;
                dialog.Filter = Strings.BackupFilter;
                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    var result = SnapIn.Router.Restore(dialog.FileName);
                    if (result.Succeeded)
                    {
                        using (var waitDialog = new WaitDialog())
                        {
                            waitDialog.DisplayDelay = System.TimeSpan.Zero;
                            waitDialog.Title = Strings.TitleRestore;
                            waitDialog.WaitForJob(() => { return SnapIn.Router.GetJobState(result.ReturnValue).ReturnValue; }, Strings.PerformingRestore);
                        }
                    }
                    else SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
                }
            }
        }

        #endregion methods
    }
}

// eof "RouterNode.cs"
