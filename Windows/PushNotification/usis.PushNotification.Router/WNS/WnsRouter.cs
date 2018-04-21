//
//  @(#) WnsRouter.cs
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
using usis.Framework.ServiceModel.Web;

namespace usis.PushNotification
{
    //  ---------------
    //  WnsRouter class
    //  ---------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class WnsRouter : ServiceBase, IWnsRouter
    {
        #region IWnsRouter implementation

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        OperationResult<Guid> IWnsRouter.SendNotification(string deviceIdentifier, string payload)
        {
            return OperationResult.Invoke((result) => Router.EnqueueNotification(
                WnsModel.SaveNotification(result, Model, deviceIdentifier, payload)));
        }

        //  ---------------------------
        //  GetNotificationState method
        //  ---------------------------

        OperationResult<NotificationState> IWnsRouter.GetNotificationState(Guid notificationId)
        {
            return OperationResult.Invoke(() => Model.GetNotificationState(notificationId));
        }

        //  -------------------
        //  ListChannels method
        //  -------------------

        OperationResult<IEnumerable<WnsChannelInfo>> IWnsRouter.ListChannels()
        {
            return OperationResult.Invoke(ListChannelInfos);
        }

        //  ------------------
        //  ListDevices method
        //  ------------------

        OperationResult<IEnumerable<WnsReceiverInfo>> IWnsRouter.ListDevices(string packageSid, DateTime? firstRegistration)
        {
            throw new NotImplementedException();
        }

        #endregion IWnsRouter implementation

        #region private method

        //  -----------------------
        //  ListChannelInfos method
        //  -----------------------

        private IEnumerable<WnsChannelInfo> ListChannelInfos()
        {
            return Model.ListChannelInfos(ChannelType.WindowsNotificationService).Cast<WnsChannelInfo>();
        }

        #endregion private method
    }

    #region WnsRouterSnapIn class

    //  ---------------------
    //  WnsRouterSnapIn class
    //  ---------------------

    internal class WnsRouterSnapIn : ServiceHostBaseSnapIn<SecureWebServiceHostConfigurator<WnsRouter, IWnsRouter>> { }

    #endregion WnsRouterSnapIn class
}

// eof "WnsRouter.cs"
