//
//  @(#) ApnsRouter.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using usis.Framework;
using usis.Framework.ServiceModel;

namespace usis.PushNotification
{
    //  ----------------
    //  ApnsRouter class
    //  ----------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ApnsRouter : ServiceBase, IApnsRouter
    {
        #region IAPNsRouter implementation

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        OperationResult<Guid> IApnsRouter.SendNotification(string deviceToken, string hexDeviceToken, string payload)
        {
            return OperationResult.Invoke<Guid>(
                (result) => Router.EnqueueNotification(
                    ApnsModel.SaveNotification(result, Model, ApnsDeviceToken.FromString(deviceToken, hexDeviceToken), payload)));
        }

        //  ---------------------------
        //  GetNotificationState method
        //  ---------------------------

        OperationResult<NotificationState> IApnsRouter.GetNotificationState(Guid id)
        {
            return OperationResult.Invoke(() => Model.GetNotificationState(id));
        }

        //  -------------------
        //  ListChannels method
        //  -------------------

        OperationResultList<ApnsChannelInfo> IApnsRouter.ListChannels()
        {
            return OperationResultList.Invoke(ListChannelInfos);
        }

        //  ------------------
        //  ListDevices method
        //  ------------------

        OperationResultList<ApnsReceiverInfo> IApnsRouter.ListDevices(string bundleId, Environment environment, DateTime? firstRegistration)
        {
            return OperationResultList.Invoke(() => Model.ListReceivers(new ApnsChannelKey(bundleId, environment), firstRegistration, false).Cast<ApnsReceiverInfo>());
        }

        #endregion IAPNsRouter implementation

        #region private method

        //  -----------------------
        //  ListChannelInfos method
        //  -----------------------

        private IEnumerable<ApnsChannelInfo> ListChannelInfos()
        {
            return Model.ListChannels(ChannelType.ApplePushNotificationService).Cast<ApnsChannelInfo>();
        }

        #endregion private method
    }

    #region ApnsRouterSnapIn class

    //  ----------------------
    //  ApnsRouterSnapIn class
    //  ----------------------

    internal class ApnsRouterSnapIn : ServiceHostSnapIn<WcfServiceHostFactory<ApnsRouter>>
    {
        public ApnsRouterSnapIn() : base(true, false) { }
    }

    #endregion ApnsRouterSnapIn class
}

// eof "ApnsRouter.cs"
