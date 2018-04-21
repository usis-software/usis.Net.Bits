//
//  @(#) ChannelKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  ----------------
    //  ChannelKey class
    //  ----------------

    /// <summary>
    /// Provides a base class to identify a channel uniquely.
    /// </summary>

    [DataContract]
    public abstract class ChannelKey
    {
        #region properties

        //  --------------------
        //  ChannelType property
        //  --------------------

        /// <summary>
        /// Gets or sets the type of the channel.
        /// </summary>
        /// <value>
        /// The type of the channel.
        /// </value>

        [DataMember]
        public ChannelType ChannelType { get; set; }

        #endregion properties
    }
}

// eof "ChannelKey.cs"
