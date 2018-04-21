//
//  @(#) ReceiverKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

namespace usis.PushNotification
{
    //  -----------------
    //  ReceiverKey class
    //  -----------------

    /// <summary>
    /// Provides a base class to identify a receiver uniquely.
    /// </summary>

    public abstract class ReceiverKey
    {
        #region properties

        //  -------------------
        //  ChannelKey property
        //  -------------------

        /// <summary>
        /// Gets the channel key.
        /// </summary>
        /// <value>
        /// The channel key.
        /// </value>

        public ChannelKey ChannelKey { get; private set; }

        #endregion properties

        #region construction

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiverKey" /> class
        /// with the specified channel key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>

        protected ReceiverKey(ChannelKey channelKey) { ChannelKey = channelKey; }

        #endregion construction
    }
}

// eof "ReceiverKey.cs"
