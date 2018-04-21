//
//  @(#) IWnsRouter.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Framework;

namespace usis.PushNotification
{
    //  --------------------
    //  IWnsRouter interface
    //  --------------------

    /// <summary>
    /// Provides methods for an application to handle push notifications to Windows devices.
    /// </summary>

    [ServiceContract]
    public interface IWnsRouter
    {
        //  -----------------------
        //  SendNotification method
        //  -----------------------

        /// <summary>
        /// Sends a push notification to the specified device.
        /// </summary>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <param name="payload">A JSON string that represents the payload of the push notification.</param>
        /// <returns>
        /// An <b>OperationResult</b> that holds the identifier of the newly created notification
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>
        ///// <remarks>
        ///// The payload for a notification can be created by using the <see cref="APNsNotification" /> class.
        ///// </remarks>
        ///// <seealso cref="APNsNotification" />

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResult<Guid> SendNotification(
            string deviceIdentifier,
            string payload);

        //  ---------------------------
        //  GetNotificationState method
        //  ---------------------------

        /// <summary>
        /// Retrieves the state of the notification specified by an identifier
        /// that was returned by <see cref="SendNotification(string, string)" />.
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
        /// An <b>OperationResult</b> that holds the channel list
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        OperationResult<IEnumerable<WnsChannelInfo>> ListChannels();

        //  ------------------
        //  ListDevices method
        //  ------------------

        /// <summary>
        /// Return a list of devices which are registered to receive notifications
        /// for the specified channel.
        /// </summary>
        /// <param name="packageSid">The package sid.</param>
        /// <param name="firstRegistration">The date and time of the first registration.</param>
        /// <returns>
        /// An <b>OperationResult</b> that holds the device list
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        OperationResult<IEnumerable<WnsReceiverInfo>> ListDevices(
            string packageSid,
            DateTime? firstRegistration);
    }
}

// eof "IWnsRouter.cs"
