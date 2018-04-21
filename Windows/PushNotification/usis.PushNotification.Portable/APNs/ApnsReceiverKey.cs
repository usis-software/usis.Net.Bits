//
//  @(#) ApnsReceiverKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;

namespace usis.PushNotification
{
    //  ---------------------
    //  ApnsReceiverKey class
    //  ---------------------

    /// <summary>
    /// Provides a key to identify an APNs receiver (device) uniquely.
    /// </summary>
    /// <seealso cref="ReceiverKey" />

    public class ApnsReceiverKey : ReceiverKey
    {
        #region properties

        //  --------------------
        //  DeviceToken property
        //  --------------------

        /// <summary>
        /// Gets the device token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>

        public ApnsDeviceToken DeviceToken { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ApnsReceiverKey" /> class.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="deviceToken">The device token.</param>

        public ApnsReceiverKey(ApnsChannelKey channelKey, ApnsDeviceToken deviceToken) : base(channelKey)
        {
            DeviceToken = deviceToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApnsReceiverKey" /> class
        /// with the specified device token string.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="hexDeviceToken">The hexadecimal device token.</param>
        /// <exception cref="ArgumentException">
        /// Either <paramref name="deviceToken" /> or <paramref name="hexDeviceToken" /> is a null reference.
        /// </exception>

        public ApnsReceiverKey(ApnsChannelKey channelKey, string deviceToken, string hexDeviceToken) : base(channelKey)
        {
            if (string.IsNullOrWhiteSpace(deviceToken) && string.IsNullOrWhiteSpace(hexDeviceToken))
            {
                throw new ArgumentException(
                    Strings.ArgumentCannotBeNullOrWhiteSpace,
                    string.Format(CultureInfo.CurrentCulture, "{0} and {1}", nameof(deviceToken), nameof(hexDeviceToken)));
            }
            DeviceToken = ApnsDeviceToken.FromString(deviceToken, hexDeviceToken);
        }

        #endregion construction

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentUICulture, "{0}, DeviceToken='{1}'", ChannelKey, DeviceToken);
        }

        #endregion overrides
    }
}

// eof "ApnsReceiverKey.cs"
