//
//  @(#) Router.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;
using usis.Platform;
using usis.Platform.Data;
using usis.Platform.ServiceModel;
using usis.Platform.Windows;

namespace usis.PushNotification.Administration
{
    #region Router class

    //  ------------
    //  Router class
    //  ------------

    internal sealed class Router : IDisposable
    {
        #region fields

        private ServiceStatusMonitor monitor;

        #endregion fields

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        internal Router()
        {
            monitor = new ServiceStatusMonitor(Constants.ServiceName, 1000);
            monitor.StatusChanged += Monitor_StatusChanged; ;
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (monitor != null)
            {
                monitor.Dispose();
                monitor = null;
            }
        }

        #endregion construction/destruction

        #region properties

        //  -------------------
        //  ServerName property
        //  -------------------

        private string serverName = string.Empty;

        internal string ServerName => string.IsNullOrWhiteSpace(serverName) ? "localhost" : serverName;

        //  ----------------------
        //  ServiceStatus property
        //  ----------------------

        internal ServiceStatus ServiceStatus => monitor.Status;

        //  ---------------------
        //  ApnsChannels property
        //  ---------------------

        private ReloadableCollection<ApnsChannelInfo> apnsChannels;

        internal ReloadableCollection<ApnsChannelInfo> ApnsChannels
        {
            get
            {
                if (apnsChannels == null)
                {
                    apnsChannels = new ReloadableCollection<ApnsChannelInfo>();
                    apnsChannels.PerformReload += (sender, e) =>
                    {
                        using (var client = CreateApnsClient())
                        {
                            apnsChannels.Replace(client.Service.ListChannels());
                        }
                    };
                }
                return apnsChannels;
            }
        }

        //  --------------------
        //  WnsChannels property
        //  --------------------

        private ReloadableCollection<WnsChannelInfo> wnsChannels;

        internal ReloadableCollection<WnsChannelInfo> WnsChannels
        {
            get
            {
                if (wnsChannels == null)
                {
                    wnsChannels = new ReloadableCollection<WnsChannelInfo>();
                    wnsChannels.PerformReload += (sender, e) =>
                    {
                        using (var client = CreateWnsClient())
                        {
                            wnsChannels.Replace(client.Service.ListChannels());
                        }
                    };
                }
                return wnsChannels;
            }
        }

        //  -------------------
        //  DataSource property
        //  -------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal DataSource DataSource
        {
            get
            {
                try
                {
                    using (var client = CreateMgmtClient())
                    {
                        return client.Service.LoadDataSource().ReturnValue;
                    }
                }
#if DEBUG
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                    return null;
                }
#else
                catch (Exception) { return null; }
#endif
            }
        }

        internal bool IsConnected => DataSource != null;

        //  -------------------------------
        //  RegisteredChannelTypes property
        //  -------------------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal IEnumerable<ChannelType> RegisteredChannelTypes
        {
            get
            {
                try
                {
                    using (var client = CreateMgmtClient())
                    {
                        return client.Service.LoadRegisteredChannelTypes().ReturnValue;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion properties

        #region event handlers

        //  ----------------------------
        //  Monitor_StatusChanged method
        //  ----------------------------

        private void Monitor_StatusChanged(object sender, ServiceStatusEventArgs e)
        {
            ServiceStatusChanged?.Invoke(this, e);
        }

        #endregion event handlers

        #region methods

        //  ------------------
        //  LoadDevices method
        //  ------------------

        internal IEnumerable<ApnsReceiverInfo> LoadDevices(ApnsChannelKey channelKey, bool includeExpired)
        {
            using (var client = CreateApnsClient())
            {
                return client.Service.ListDevices(channelKey, includeExpired);
            }
        }

        internal IEnumerable<WnsReceiverInfo> LoadDevices(WnsChannelKey channelKey)
        {
            using (var client = CreateWnsClient())
            {
                return client.Service.ListDevices(channelKey).Iterate();
            }
        }

        //  ------------------
        //  GetJobState method
        //  ------------------

        internal OperationResult<JobState> GetJobState(Guid jobId)
        {
            using (var client = CreateMgmtClient())
            {
                return client.Service.GetJobState(jobId);
            }
        }

        //  ---------------------
        //  GetJobProgress method
        //  ---------------------

        internal OperationResult<JobProgress> GetJobProgress(Guid jobId)
        {
            using (var client = CreateMgmtClient())
            {
                return client.Service.GetJobProgress(jobId);
            }
        }

        //  -------------
        //  Backup method
        //  -------------

        internal OperationResult<Guid> Backup(string path)
        {
            return OperationResult.Invoke<Guid>((result) =>
            {
                return result.ReportResult(RouterMgmtClient.Backup(ServerName, path)).ReturnValue;
            });
        }

        //  --------------
        //  Restore method
        //  --------------

        internal OperationResult<Guid> Restore(string path)
        {
            return OperationResult.Invoke<Guid>((result) =>
            {
                return result.ReportResult(RouterMgmtClient.Restore(ServerName, path)).ReturnValue;
            });
        }

        //  -----------------------
        //  CreateApnsClient method
        //  -----------------------

        internal ApnsRouterMgmtClient CreateApnsClient() { return new ApnsRouterMgmtClient(ServerName); }

        //  ----------------------
        //  CreateWnsClient method
        //  ----------------------

        internal WnsRouterMgmtClient CreateWnsClient() { return new WnsRouterMgmtClient(ServerName); }

        //  -----------------------
        //  CreateMgmtClient method
        //  -----------------------

        internal RouterMgmtClient CreateMgmtClient() { return new RouterMgmtClient(ServerName); }

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        internal OperationResult UpdateChannel(IChannelInfo channelInfo)
        {
            switch (channelInfo.BaseKey.ChannelType)
            {
                case ChannelType.ApplePushNotificationService:
                    using (var client = CreateApnsClient())
                    {
                        return client.Service.UpdateChannel(channelInfo as ApnsChannelInfo);
                    }
                case ChannelType.WindowsNotificationService:
                    using (var client = CreateWnsClient())
                    {
                        return client.Service.UpdateChannel(channelInfo as WnsChannelInfo);
                    }
                case ChannelType.GoogleCloudMessaging:
                    break;
                default:
                    break;
            }
            return null;
        }

        //  ---------------------
        //  ReloadChannels method
        //  ---------------------

        internal void ReloadChannels(ChannelType channelType)
        {
            switch (channelType)
            {
                case ChannelType.ApplePushNotificationService:
                    ApnsChannels.Reload();
                    break;
                case ChannelType.WindowsNotificationService:
                    WnsChannels.Reload();
                    break;
                case ChannelType.GoogleCloudMessaging:
                    break;
                default:
                    break;
            }
        }

        #region CreateChannel method

        //  --------------------
        //  CreateChannel method
        //  --------------------

        internal OperationResult CreateChannel(WnsChannelKey channelKey)
        {
            return OperationResult.Invoke((result) =>
            {
                using (var client = CreateWnsClient())
                {
                    result.ReportResult(client.Service.CreateChannel(channelKey));
                }
            });
        }

        #endregion CreateChannel method

        #region DeleteChannel method

        //  --------------------
        //  DeleteChannel method
        //  --------------------

        internal OperationResult DeleteChannel(WnsChannelKey channelKey)
        {
            return OperationResult.Invoke((result) =>
            {
                using (var client = CreateWnsClient())
                {
                    result.ReportResult(client.Service.DeleteChannel(channelKey));
                }
            });
        }

        #endregion DeleteChannel method

        #endregion methods

        #region events

        //  --------------------------
        //  ServiceStatusChanged event
        //  --------------------------

        internal event EventHandler<ServiceStatusEventArgs> ServiceStatusChanged;

        #endregion events
    }

    #endregion Router class

    #region ApnsRouterMgmtClient class

    //  --------------------------
    //  ApnsRouterMgmtClient class
    //  --------------------------

    internal class ApnsRouterMgmtClient : NamedPipeServiceClient<IApnsRouterMgmt>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ApnsRouterMgmtClient(string serverName) : base(serverName, "ApnsRouterMgmt") { }

        #endregion construction
    }

    #endregion ApnsRouterMgmtClient class

    #region WnsRouterMgmtClient class

    //  -------------------------
    //  WnsRouterMgmtClient class
    //  -------------------------

    internal class WnsRouterMgmtClient : NamedPipeServiceClient<IWnsRouterMgmt>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal WnsRouterMgmtClient(string serverName) : base(serverName, "WnsRouterMgmt") { }

        #endregion construction
    }

    #endregion WnsRouterMgmtClient class

    #region RouterMgmtClient class

    //  ----------------------
    //  RouterMgmtClient class
    //  ----------------------

    internal class RouterMgmtClient : NamedPipeServiceClient<IRouterMgmt>
    {
        #region constants

        private const string ServicePath = "RouterMgmt";

        #endregion constants

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal RouterMgmtClient(string serverName) : base(serverName, ServicePath) { }

        #endregion construction

        #region methods

        //  -------------
        //  Backup method
        //  -------------

        internal static OperationResult<Guid> Backup(string serverName, string path)
        {
            return Invoke(serverName, ServicePath, (s) => { return s.Backup(path); });
        }

        //  --------------
        //  Restore method
        //  --------------

        internal static OperationResult<Guid> Restore(string serverName, string path)
        {
            return Invoke(serverName, ServicePath, (s) => { return s.Restore(path); });
        }

        #endregion methods
    }

    #endregion RouterMgmtClient class
}

// eof "Router.cs"
