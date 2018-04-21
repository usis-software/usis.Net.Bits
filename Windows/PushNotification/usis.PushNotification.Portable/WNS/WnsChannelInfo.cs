//
//  @(#) WnsChannelInfo.cs
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
    //  WnsChannelInfo class
    //  --------------------

    /// <summary>
    /// Provides informations about a WNS channel.
    /// </summary>

    [DataContract]
    public class WnsChannelInfo : IChannelInfo, IEquatable<WnsChannelInfo>
    {
        #region properties

        //  ------------
        //  Key property
        //  ------------

        /// <summary>
        /// Gets or sets the WNS channel's key.
        /// </summary>
        /// <value>
        /// The WNS channel's key.
        /// </value>

        [DataMember]
        public WnsChannelKey Key { get; set; }

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
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the channel was last changed.
        /// </summary>
        /// <value>
        /// The date and time when the channel was last changed.
        /// </value>

        [DataMember]
        public DateTime? Changed { get; set; }

        //  --------------------
        //  PackageName property
        //  --------------------

        /// <summary>
        /// Gets or sets the name of the package contained in the application manifest.
        /// </summary>
        /// <value>
        /// The name of the package contained in the application manifest.
        /// </value>

        [DataMember]
        public string PackageName { get; set; }

        //  ---------------------
        //  ClientSecret property
        //  ---------------------

        /// <summary>
        /// Gets or sets the secet key that is used to authenticate against WNS.
        /// </summary>
        /// <value>
        /// The secet key that is used to authenticate against WNS.
        /// </value>

        [DataMember]
        public string ClientSecret { get; set; }

        #endregion properties

        #region overrides

        //  ----------------
        //  BaseKey property
        //  ----------------

        /// <summary>
        /// Gets the channel's key.
        /// </summary>

        public ChannelKey BaseKey => Key;

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

        #region IEquatable implementation

        //  -------------
        //  Equals method
        //  -------------

        bool IEquatable<WnsChannelInfo>.Equals(WnsChannelInfo other) { return Equals(other); }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>

        public override bool Equals(object obj)
        {
            var channelInfo = obj as WnsChannelInfo;
            if (channelInfo == null) return false;

            return Key.Equals(channelInfo.Key);
        }

        //  ------------------
        //  GetHashCode method
        //  ------------------

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>

        public override int GetHashCode() { return Key.GetHashCode(); }

        #endregion IEquatable implementation
    }
}

// eof "WnsChannelInfo.cs"
