//
//  @(#) WnsChannelKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  -------------------
    //  WnsChannelKey class
    //  -------------------

    /// <summary>
    /// Provides a key to identify a WNS channel uniquely.
    /// </summary>
    /// <seealso cref="ChannelKey" />
    /// <seealso cref="IEquatable{WnsChannelKey}" />

    [DataContract]
    public class WnsChannelKey : ChannelKey, IEquatable<WnsChannelKey>
    {
        #region properties

        //  -------------------
        //  PackageSid property
        //  -------------------

        /// <summary>
        /// Gets or sets the unique identifier for your Windows Store app.
        /// </summary>
        /// <value>
        /// The unique identifier for your Windows Store app.
        /// </value>

        [DataMember]
        public string PackageSid { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="WnsChannelKey" /> class from being created.
        /// </summary>

        private WnsChannelKey() { ChannelType = ChannelType.WindowsNotificationService; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WnsChannelKey" /> class
        /// with the specified Package SID.
        /// </summary>
        /// <param name="packageSid">The package SID.</param>

        public WnsChannelKey(string packageSid) : this() { PackageSid = packageSid; }

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
            return string.Format(CultureInfo.CurrentCulture, "ChannelType={0}, PackageSid='{1}'", ChannelType, PackageSid);
        }

        #endregion overrides

        #region IEquatable implementation

        //  -------------
        //  Equals method
        //  -------------

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>

        bool IEquatable<WnsChannelKey>.Equals(WnsChannelKey other) { return Equals(other); }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>

        public override bool Equals(object obj)
        {
            var channelKey = obj as WnsChannelKey;
            if (channelKey == null) return false;

            return
                ChannelType == channelKey.ChannelType &&
                PackageSid.Equals(channelKey.PackageSid, StringComparison.OrdinalIgnoreCase);
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
            return new { A = ChannelType, B = PackageSid }.GetHashCode();
        }

        #endregion IEquatable implementation
    }
}

// eof "WnsChannelKey.cs"
