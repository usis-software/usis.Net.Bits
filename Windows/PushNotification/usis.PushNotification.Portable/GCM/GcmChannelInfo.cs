//
//  @(#) GcmChannelInfo.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  --------------------
    //  GcmChannelInfo class
    //  --------------------

    /// <summary>
    /// Provides informations about an GCM channel.
    /// </summary>

    [DataContract]
    public class GcmChannelInfo : IChannelInfo
    {
        #region properties

        //  ------------
        //  Key property
        //  ------------

        /// <summary>
        /// Gets or sets the GCM channel's key.
        /// </summary>
        /// <value>
        /// The APNs channel's key.
        /// </value>

        [DataMember]
        public GcmChannelKey Key { get; set; }

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets or sets a description for the channel.
        /// </summary>
        /// <value>
        /// A description for the channel.
        /// </value>

        [DataMember]
        public string Description { get; set; }

        //  ------------------
        //  ServerKey property
        //  ------------------

        /// <summary>
        /// Gets or sets the server key.
        /// </summary>
        /// <value>
        /// The server key.
        /// </value>

        [DataMember]
        public string ServerKey { get; set; }

        //  ----------------------
        //  ApplicationId property
        //  ----------------------

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>

        [DataMember]
        public string ApplicationId { get; set; }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the channel was created.
        /// </summary>
        /// <value>
        /// The date and time when the channel was created.
        /// </value>

        [DataMember]
        public DateTime Created { get; set; }

        //  ----------------
        //  Changed property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the channel was last changed.
        /// </summary>
        /// <value>
        /// The date and time when the channel was last changed.
        /// </value>

        [DataMember]
        public DateTime? Changed { get; set; }

        //  ----------------
        //  BaseKey property
        //  ----------------

        /// <summary>
        /// Gets the channel's key.
        /// </summary>

        public ChannelKey BaseKey => Key;

        #endregion properties

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

        public override string ToString() { return Key.ToString(); }

        #endregion overrides
    }
}

// eof "GcmChannelInfo.cs"
