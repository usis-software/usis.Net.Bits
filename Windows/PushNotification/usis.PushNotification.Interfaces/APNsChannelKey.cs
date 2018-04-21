//
//  @(#) APNsChannelKey.cs
//
//  Project:    usis Push Notification System
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;
using usis.Platform.Portable;

namespace usis.PushNotification
{
    //  --------------------
    //  APNsChannelKey class
    //  --------------------

    /// <summary>
    /// Provides a key to identify an APNs channel uniquely.
    /// </summary>

    [DataContract]
    public class APNsChannelKey : IEquatable<APNsChannelKey>
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

        public ChannelType ChannelType { get; set; }

        //  -----------------
        //  BundleId property
        //  -----------------

        /// <summary>
        /// Gets or sets the bundle identifier.
        /// </summary>
        /// <value>
        /// The bundle identifier.
        /// </value>

        [DataMember]
        public string BundleId { get; set; }

        //  --------------------
        //  Environment property
        //  --------------------

        /// <summary>
        /// Gets or sets the environment for push notifications.
        /// </summary>
        /// <value>
        /// The environment for push notifications.
        /// </value>

        [DataMember]
        public usis.PushNotification.Environment Environment { get; set; }

        #endregion properties

        #region IEquatable<ChannelKey>

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
            var channelKey = obj as APNsChannelKey;
            if (channelKey == null) return false;

            return
                ChannelType == channelKey.ChannelType &&
                BundleId.Equals(channelKey.BundleId) &&
                Environment == channelKey.Environment;
        }

        bool IEquatable<APNsChannelKey>.Equals(APNsChannelKey other)
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
            return new
            {
                A = BundleId,
                B = Environment

            }.GetHashCode();
        }

        #endregion IEquatable<ChannelKey>

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="APNsChannelKey"/> class.
        /// </summary>

        protected APNsChannelKey() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="APNsChannelKey"/> class
        /// with the specified application identifier and environment.
        /// </summary>
        /// <param name="bundleId">The bundle identifier.</param>
        /// <param name="environment">The environment.</param>
        /// <exception cref="ArgumentNullOrWhiteSpaceException">
        /// <b>appId</b> is a null reference, or consists only of white-space caracters.
        /// </exception>

        public APNsChannelKey(string bundleId, usis.PushNotification.Environment environment) : this()
        {
            if (string.IsNullOrWhiteSpace(bundleId)) throw new ArgumentNullOrWhiteSpaceException(nameof(bundleId));
            if (!Enum.IsDefined(typeof(usis.PushNotification.Environment), environment)) throw new ArgumentOutOfRangeException(nameof(environment));

            ChannelType = ChannelType.ApplePushNotificationService;
            BundleId = bundleId;
            Environment = environment;
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
            return string.Format(
                CultureInfo.CurrentCulture,
                "ChannelType={0}, BundleId='{1}', Environment={2}",
                ChannelType, BundleId, Environment);
        }

        #endregion overrides
    }
}

// eof "APNsChannelKey.cs"
