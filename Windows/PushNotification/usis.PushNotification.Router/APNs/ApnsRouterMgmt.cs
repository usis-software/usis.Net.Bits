//
//  @(#) ApnsRouterMgmt.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using usis.Framework.Portable;
using usis.Framework.ServiceModel;

namespace usis.PushNotification
{
    //  --------------------
    //  ApnsRouterMgmt class
    //  --------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class ApnsRouterMgmt : ServiceBase, IApnsRouterMgmt
    {
        #region CreateChannel method

        //  --------------------
        //  CreateChannel method
        //  --------------------

        OperationResult IApnsRouterMgmt.CreateChannel(ApnsChannelKey channelKey)
        {
            return OperationResult.Invoke(() => Model.FindOrCreateChannel(channelKey));
        }

        #endregion CreateChannel method

        #region UpdateChannel method

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        OperationResult IApnsRouterMgmt.UpdateChannel(ApnsChannelInfo channelInfo)
        {
            return OperationResult.Invoke(() => Router.UpdateChannel(channelInfo));
        }

        #endregion UpdateChannel method

        #region ListChannels method

        //  -------------------
        //  ListChannels method
        //  -------------------

        IEnumerable<ApnsChannelInfo> IApnsRouterMgmt.ListChannels()
        {
            return Model.ListChannels(ChannelType.ApplePushNotificationService).Cast<ApnsChannelInfo>();
        }

        #endregion ListChannels method

        #region ListDevices method

        //  ------------------
        //  ListDevices method
        //  ------------------

        IEnumerable<ApnsReceiverInfo> IApnsRouterMgmt.ListDevices(ApnsChannelKey channelKey, bool includeExpired)
        {
            return Model.ListReceivers(channelKey, null, includeExpired).Cast<ApnsReceiverInfo>();
        }

        #endregion ListDevices method

        #region DeleteChannel method

        //  --------------------
        //  DeleteChannel method
        //  --------------------

        OperationResult IApnsRouterMgmt.DeleteChannel(ApnsChannelKey channelKey)
        {
            return OperationResult.Invoke(() => Router.DeleteChannel(channelKey));
        }

        #endregion DeleteChannel method

        #region certificate methods

        //  ------------------------
        //  UploadCertificate method
        //  ------------------------

        OperationResult IApnsRouterMgmt.UploadCertificate(ApnsChannelKey channelKey, string certificateData)
        {
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));
            if (certificateData == null) throw new ArgumentNullException(nameof(certificateData));

            byte[] data = Convert.FromBase64String(certificateData);
            return OperationResult.Invoke(() => Router.ClosePusher(
                ApnsModel.SaveCertificate(Model, channelKey, data)));
        }

        //  -------------------------
        //  InstallCertificate method
        //  -------------------------

        OperationResult IApnsRouterMgmt.InstallCertificate(ApnsChannelKey channelKey, string password)
        {
            return OperationResult.Invoke((result) => Router.ClosePusher(ApnsModel.InstallCertificate(Model, result, channelKey, password)));
        }

        //  ---------------------------
        //  UninstallCertificate method
        //  ---------------------------

        OperationResult IApnsRouterMgmt.UninstallCertificate(ApnsChannelKey channelKey)
        {
            return OperationResult.Invoke((result) => Router.ClosePusher(ApnsModel.UninstallCertificate(Model, result, channelKey)));
        }

        //  ------------------------
        //  DeleteCertificate method
        //  ------------------------

        OperationResult IApnsRouterMgmt.DeleteCertificate(ApnsChannelKey channelKey)
        {
            return OperationResult.Invoke((result) => Router.ClosePusher(ApnsModel.DeleteCertificate(Model, result, channelKey)));
        }

        #endregion certificate methods

        #region SendNotification method

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        OperationResult<Guid> IApnsRouterMgmt.SendNotification(string deviceToken, string payload)
        {
            return OperationResult.Invoke((result) => Router.EnqueueNotification(
                ApnsModel.SaveNotification(result, Model, ApnsDeviceToken.FromBase64(deviceToken), payload)));
        }

        #endregion SendNotification method
    }

    #region ApnsRouterMgmtSnapIn class

    //  --------------------------
    //  ApnsRouterMgmtSnapIn class
    //  --------------------------

    internal class ApnsRouterMgmtSnapIn : NamedPipeServiceHostSnapIn<ApnsRouterMgmt, IApnsRouterMgmt>
    {
        public ApnsRouterMgmtSnapIn() : base(false) { }
    }

    #endregion ApnsRouterMgmtSnapIn class
}

// eof "ApnsRouterMgmt.cs"
