//
//  @(#) IChannelInfo.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;

namespace usis.PushNotification
{
    //  ----------------------
    //  IChannelInfo interface
    //  ----------------------

    /// <summary>
    /// Describes common properties of push notification channels.
    /// </summary>

    public interface IChannelInfo
    {
        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets or sets a description for the channel.
        /// </summary>
        /// <value>
        /// A description for the channel.
        /// </value>

        string Description { get; set; }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the channel was created.
        /// </summary>
        /// <value>
        /// The date and time when the channel was created.
        /// </value>
        /// <remarks>
        /// The time is provided as UTC.
        /// Use <see cref="DateTime.ToLocalTime"/> to display it in your local time.
        /// </remarks>

        DateTime Created { get; set; }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the channel was last changed.
        /// </summary>
        /// <value>
        /// The date and time when the channel was last changed.
        /// </value>
        /// <remarks>
        /// The time is provided as UTC.
        /// Use <see cref="DateTime.ToLocalTime"/> to display it in your local time.
        /// </remarks>

        DateTime? Changed { get; set; }

        //  ----------------
        //  BaseKey property
        //  ----------------

        /// <summary>
        /// Gets the channel's key.
        /// </summary>
        /// <returns>The channel's key.</returns>

        ChannelKey BaseKey { get; }
    }
}

// eof "IChannelInfo.cs"
