//
//  @(#) IApns.cs
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
    //  ---------------
    //  IApns interface
    //  ---------------

    /// <summary>
    /// Provides methods for the registration of iOS devices as receiver of push notifications.
    /// </summary>

    [ServiceContract]
    public interface IApns
    {
        //  -----------------------
        //  RegisterReceiver method
        //  -----------------------

        /// <summary>
        /// Registers a device with the specified device token as a receiver of push notifications.
        /// </summary>
        /// <param name="bundleId">The app's bundle identifier.</param>
        /// <param name="environment">A value that indicates whether to use the development or production environment.</param>
        /// <param name="deviceToken">The device token to register.</param>
        /// <param name="hexDeviceToken">The hexadecimal device token.</param>
        /// <param name="name">The name of the device.</param>
        /// <param name="account">The user account.</param>
        /// <param name="groups">The groups that the account belongs to.</param>
        /// <param name="info">Additional information for the device.</param>
        /// <returns>
        /// An <b>OperationResult</b> that contains the identifier for the newly created receiver as
        /// <see cref="OperationResult{TReturnValue}.ReturnValue" />.
        /// </returns>
        /// <remarks>
        /// A device token is an array of 32 bytes provided by Apple's UIKit.
        /// <b>deviceToken</b> is a string representation of a device token that is encoded with base-64 digits.
        /// </remarks>

        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OperationResult<Guid> RegisterReceiver(
            string bundleId,
            Environment environment,
            string deviceToken,
            string hexDeviceToken,
            string name,
            string account,
            string groups,
            string info);
    }
}

// eof "IApns.cs"
