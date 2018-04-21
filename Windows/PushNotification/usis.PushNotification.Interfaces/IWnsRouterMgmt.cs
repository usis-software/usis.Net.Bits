//
//  @(#) IWnsRouterMgmt.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.ServiceModel;
using usis.Framework;

namespace usis.PushNotification
{
    //  ------------------------
    //  IWnsRouterMgmt interface
    //  ------------------------

    /// <summary>
    /// Provides methods to manage <b>Windows Notification Service</b> (WNS) channels.
    /// </summary>

    [ServiceContract]
    public interface IWnsRouterMgmt
    {
        //  --------------------
        //  CreateChannel method
        //  --------------------

        /// <summary>
        /// Creates an WNS channel with the specified channel key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// An <b>OperationResult</b> with information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult CreateChannel(WnsChannelKey channelKey);

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
        OperationResult UpdateChannel(WnsChannelInfo channelInfo);

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
        IEnumerable<WnsChannelInfo> ListChannels();

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
        OperationResult DeleteChannel(WnsChannelKey channelKey);

        /// <summary>
        /// Lists the devices of a channel.
        /// </summary>
        /// <param name="channelKey">The channel's key.</param>
        /// <returns>
        /// An <see cref="OperationResultList" /> with an enumerator to iterate thru all devices of the specified channel.
        /// </returns>

        [OperationContract]
        OperationResultList<WnsReceiverInfo> ListDevices(WnsChannelKey channelKey);
    }
}

// eof "IWnsRouterMgmt.cs"
