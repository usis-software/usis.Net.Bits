//
//  @(#) GcmReceiverKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Globalization;

namespace usis.PushNotification
{
    //  --------------------
    //  GcmReceiverKey class
    //  --------------------

    /// <summary>
    /// Provides a key to identify a GCM receiver uniquely.
    /// </summary>
    /// <seealso cref="ReceiverKey" />

    public class GcmReceiverKey : ReceiverKey
    {
        #region properties

        //  --------------------------
        //  RegistrationToken property
        //  --------------------------

        /// <summary>
        /// Gets or sets an application specific registration token.
        /// </summary>
        /// <value>
        /// An application specific registration token.
        /// </value>

        public string RegistrationToken { get; set; }

        #endregion properties

        #region construction

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="GcmReceiverKey" /> class
        /// with the specified registration token.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="registrationToken">The registration token.</param>

        public GcmReceiverKey(WnsChannelKey channelKey, string registrationToken) : base(channelKey)
        {
            RegistrationToken = registrationToken;
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
            return string.Format(CultureInfo.CurrentCulture, "{0}, RegistrationToken='{1}'", ChannelKey, RegistrationToken);
        }

        #endregion overrides
    }
}

// eof "GcmReceiverKey.cs"
