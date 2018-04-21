//
//  @(#) IApnsRouter.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Framework;

namespace usis.PushNotification
{
    //  ---------------------
    //  IApnsRouter interface
    //  ---------------------

    /// <summary>
    /// Provides methods for an application to handle push notifications to iOS devices.
    /// </summary>

    [ServiceContract]
    public interface IApnsRouter
    {
        //  -----------------------
        //  SendNotification method
        //  -----------------------

        /// <summary>
        /// Sends a push notification to the specified device.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="hexDeviceToken">The hexadecimal device token.</param>
        /// <param name="payload">A JSON string that represents the payload of the push notification.</param>
        /// <returns>
        /// An <b>OperationResult</b> that holds the identifier of the newly created notification
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>
        /// <remarks>
        /// The payload for a notification can be created by using the <see cref="ApnsNotification" /> class.
        /// </remarks>
        /// <seealso cref="ApnsNotification" />

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResult<Guid> SendNotification(string deviceToken, string hexDeviceToken, string payload);

        //  ---------------------------
        //  GetNotificationState method
        //  ---------------------------

        /// <summary>
        /// Retrieves the state of the notification specified by an identifier
        /// that was returned by <see cref="SendNotification(string, string, string)" />.
        /// </summary>
        /// <param name="id">The identifier of the notification.</param>
        /// <returns>
        /// An value the indicates whether the notification was sent or not.
        /// </returns>

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResult<NotificationState> GetNotificationState(Guid id);

        //  -------------------
        //  ListChannels method
        //  -------------------

        /// <summary>
        /// Lists all channels.
        /// </summary>
        /// <returns>
        /// An <see cref="OperationResultList" /> that holds the channel list
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResultList<ApnsChannelInfo> ListChannels();

        //  ------------------
        //  ListDevices method
        //  ------------------

        /// <summary>
        /// Return a list of devices which are registered to receive notifications
        /// for the specified channel.
        /// </summary>
        /// <param name="bundleId">The channel's bundle identifier.</param>
        /// <param name="environment">The channel's environment.</param>
        /// <param name="firstRegistration">The date and time of the first registration.</param>
        /// <returns>
        /// An <b>OperationResult</b> that holds the device list
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResultList<ApnsReceiverInfo> ListDevices(string bundleId, Environment environment, DateTime? firstRegistration);
    }
}

// eof "IApnsRouter.cs"
