//
//  @(#) NotificationContext.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;

namespace usis.PushNotification
{
    //  -------------------------
    //  NotificationContext class
    //  -------------------------

    /// <summary>
    /// Provides necessary information in the context of sending notifications.
    /// </summary>

    public class NotificationContext
    {
        //  ---------------------
        //  Notification property
        //  ---------------------

        /// <summary>
        /// Gets or sets the notification to send.
        /// </summary>
        /// <value>
        /// The notification to send.
        /// </value>

        public Notification Notification { get; set; }

        //  ------------------
        //  ChannelId property
        //  ------------------

        /// <summary>
        /// Gets or sets the identifier of the channel that is used to send the notification.
        /// </summary>
        /// <value>
        /// The identifier of the channel that is used to send the notification.
        /// </value>

        internal Guid? ChannelId { get; set; }

        //  -----------------
        //  Receiver property
        //  -----------------

        /// <summary>
        /// Gets or sets the receiver of the notification.
        /// </summary>
        /// <value>
        /// The receiver of the notification.
        /// </value>

        public IReceiver Receiver { get; set; }
    }
}

// eof "NotificationContext.cs"
