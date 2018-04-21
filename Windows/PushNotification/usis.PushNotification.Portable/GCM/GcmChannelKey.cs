//
//  @(#) GcmChannelKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  -------------------
    //  GcmChannelKey class
    //  -------------------

    /// <summary>
    /// Provides a key to identify an GCM channel uniquely.
    /// </summary>
    /// <seealso cref="ChannelKey" />
    /// <seealso cref="IEquatable{GcmChannelKey}" />

    [DataContract]
    public class GcmChannelKey : ChannelKey, IEquatable<GcmChannelKey>
    {
        #region properties

        //  -----------------
        //  SenderId property
        //  -----------------

        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>
        /// The sender identifier.
        /// </value>

        [DataMember]
        public string SenderId { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="GcmChannelKey"/> class from being created.
        /// </summary>

        private GcmChannelKey() { ChannelType = ChannelType.GoogleCloudMessaging; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GcmChannelKey" /> class
        /// with the specified sender identifier.
        /// </summary>
        /// <param name="senderId">The sender identifier.</param>
        /// <exception cref="ArgumentException"><paramref name="senderId" /> is a null reference or isempty.</exception>

        public GcmChannelKey(string senderId) : this()
        {
            if (string.IsNullOrWhiteSpace(senderId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrWhiteSpace, nameof(senderId));
            }
            SenderId = senderId;
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
            return string.Format(CultureInfo.CurrentCulture, "ChannelType={0}, SenderId='{1}'", ChannelType, SenderId);
        }

        #endregion overrides

        #region IEquatable<GcmChannelKey>

        //  -------------
        //  Equals method
        //  -------------

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <b>false</b>.
        /// </returns>

        public override bool Equals(object obj)
        {
            var channelKey = obj as GcmChannelKey;
            if (channelKey == null) return false;

            return
                ChannelType == channelKey.ChannelType &&
                SenderId.Equals(channelKey.SenderId, StringComparison.OrdinalIgnoreCase);
        }

        bool IEquatable<GcmChannelKey>.Equals(GcmChannelKey other)
        {
            return Equals(other);
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

        public override int GetHashCode()
        {
            return SenderId.GetHashCode();
        }

        #endregion IEquatable<GcmChannelKey>
    }
}

// eof "GcmChannelKey.cs"
