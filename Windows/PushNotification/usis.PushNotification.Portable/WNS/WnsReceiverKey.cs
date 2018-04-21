//
//  @(#) WnsReceiverKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;

namespace usis.PushNotification
{
    //  --------------------
    //  WnsReceiverKey class
    //  --------------------

    /// <summary>
    /// Provides a key to identify a WNS receiver uniquely.
    /// </summary>
    /// <seealso cref="ReceiverKey" />

    public class WnsReceiverKey : ReceiverKey
    {
        #region properties

        //  -------------------------
        //  DeviceIdentifier property
        //  -------------------------

        /// <summary>
        /// Gets or sets an application specific device identifier.
        /// </summary>
        /// <value>
        /// An application specific device identifier.
        /// </value>

        public string DeviceIdentifier { get; set; }

        //  -------------------
        //  ChannelUri property
        //  -------------------

        /// <summary>
        /// Gets or sets the channel URI.
        /// </summary>
        /// <value>
        /// The channel URI.
        /// </value>

        public Uri ChannelUri { get; set; }

        #endregion properties

        #region construction

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="WnsReceiverKey" /> class
        /// with the specified channel key and device idetifier.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>

        public WnsReceiverKey(WnsChannelKey channelKey, string deviceIdentifier) : base(channelKey)
        {
            DeviceIdentifier = deviceIdentifier;
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
            return string.Format(CultureInfo.CurrentCulture, "{0}, DeviceIdentifier='{1}', ChannelUri='{2}'", ChannelKey, DeviceIdentifier, ChannelUri);
        }

        #endregion overrides
    }
}

// eof "WnsReceiverKey.cs"
