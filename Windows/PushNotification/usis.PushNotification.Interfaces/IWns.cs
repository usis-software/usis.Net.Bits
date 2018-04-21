//
//  @(#) IWns.cs
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
    //  --------------
    //  IWns interface
    //  --------------

    /// <summary>
    /// Provides methods for the registeration of Windows devices as receiver of push notifications.
    /// </summary>

    [ServiceContract]
    public interface IWns
    {
        //  -----------------------
        //  RegisterReceiver method
        //  -----------------------

        /// <summary>
        /// Registers a device with the specified attributes as a receiver of push notifications.
        /// </summary>
        /// <param name="packageSid">The package SID.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <param name="channelUri">The channel URI.</param>
        /// <returns>
        /// An <b>OperationResult</b> that contains the identifier for the newly created receiver as
        /// <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResult<Guid> RegisterReceiver(
            string packageSid,
            string deviceIdentifier,
            Uri channelUri);
    }
}

// eof "IWns.cs"
