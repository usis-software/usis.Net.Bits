//
//  @(#) APNsRouterMgmtClient.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System.Collections.Generic;
using usis.Framework.Portable;
using usis.Framework.ServiceModel;

namespace usis.Server.PushNotification.Administration
{
    //  --------------------------
    //  APNsRouterMgmtClient class
    //  --------------------------

    internal class APNsRouterMgmtClient : NamedPipeClientBase<IAPNsRouterMgmt>, IAPNsRouterMgmt
    {
        #region constructor

        //  -----------
        //  constructor
        //  -----------

        internal APNsRouterMgmtClient(string serverName) : base(serverName, "APNsRouterMgmt") { }

        #endregion constructor

        #region IAPNsRouterMgmt methods

        public OperationResult CreateChannel(APNsChannelKey channelKey)
        {
            return Channel.CreateChannel(channelKey);
        }

        public OperationResult UpdateChannel(APNsChannelInfo channelInfo)
        {
            return Channel.UpdateChannel(channelInfo);
        }

        public IEnumerable<APNsChannelInfo> ListChannels()
        {
            return Channel.ListChannels();
        }

        public OperationResult UploadCertificate(APNsChannelKey channelKey, string certificateData)
        {
            return Channel.UploadCertificate(channelKey, certificateData);
        }

        public OperationResult InstallCertificate(APNsChannelKey channelKey, string password)
        {
            return Channel.InstallCertificate(channelKey, password);
        }

        public OperationResult UninstallCertificate(APNsChannelKey channelKey)
        {
            return Channel.UninstallCertificate(channelKey);
        }

        public OperationResult DeleteCertificate(APNsChannelKey channelKey)
        {
            return Channel.DeleteCertificate(channelKey);
        }

        public OperationResult DeleteChannel(APNsChannelKey channelKey)
        {
            return Channel.DeleteChannel(channelKey);
        }

        #endregion  IAPNsRouterMgmt methods
    }
}

// eof "APNsRouterMgmtClient.cs"
