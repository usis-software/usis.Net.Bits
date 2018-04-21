//
//  @(#) IApnsRouterMgmt.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.ServiceModel;
using usis.Framework;

namespace usis.PushNotification
{
    //  -------------------------
    //  IApnsRouterMgmt interface
    //  -------------------------

    /// <summary>
    /// Provides methods to manage APNs channels.
    /// </summary>

    [ServiceContract]
    public interface IApnsRouterMgmt
    {
        //  --------------------
        //  CreateChannel method
        //  --------------------

        /// <summary>
        /// Creates an APNs channel with the specified channel key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult CreateChannel(ApnsChannelKey channelKey);

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        /// <summary>
        /// Updates a channel with the specified informations.
        /// </summary>
        /// <param name="channelInfo">The information to update.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult UpdateChannel(ApnsChannelInfo channelInfo);

        //  -------------------
        //  ListChannels method
        //  -------------------

        /// <summary>
        /// Lists all channels.
        /// </summary>
        /// <returns>
        /// An enumerator to iterate thru all channels.
        /// </returns>

        [OperationContract]
        IEnumerable<ApnsChannelInfo> ListChannels();

        /// <summary>
        /// Lists the devices of a channel.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <param name="includeExpired">if set to <c>true</c> expired devices are also listed.</param>
        /// <returns>
        /// An enumerator to iterate thru all devices of the specified channel.
        /// </returns>

        [OperationContract]
        IEnumerable<ApnsReceiverInfo> ListDevices(ApnsChannelKey channelKey, bool includeExpired);

        //  ------------------------
        //  UploadCertificate method
        //  ------------------------

        /// <summary>
        /// Uploads certificate data as base-64 string to the specified channel.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <param name="certificateData">The certificate data as base-64 string.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult UploadCertificate(ApnsChannelKey channelKey, string certificateData);

        //  -------------------------
        //  InstallCertificate method
        //  -------------------------

        /// <summary>
        /// Installs the channel's certificate in the server's local certificate store.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <param name="password">The password for the certificate.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult InstallCertificate(ApnsChannelKey channelKey, string password);

        //  ---------------------------
        //  UninstallCertificate method
        //  ---------------------------

        /// <summary>
        /// Removes the channel's certificate from the server's local certificate store.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult UninstallCertificate(ApnsChannelKey channelKey);

        //  ------------------------
        //  DeleteCertificate method
        //  ------------------------

        /// <summary>
        /// Deletes  the channel's certificate.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult DeleteCertificate(ApnsChannelKey channelKey);

        //  --------------------
        //  DeleteChannel method
        //  --------------------

        /// <summary>
        /// Deletes the specified channel.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult DeleteChannel(ApnsChannelKey channelKey);

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        /// <summary>
        /// Sends a push notification to the specified device.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="payload">A JSON string that represents the payload of the push notification.</param>
        /// <returns>
        /// An <b>OperationResult</b> that holds the identifier of the newly created notification
        /// as <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>

        [OperationContract]
        OperationResult<Guid> SendNotification(string deviceToken, string payload);
    }
}

// eof "IApnsRouterMgmt.cs"
