//
//  @(#) ApnsChannelKey.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  --------------------
    //  ApnsChannelKey class
    //  --------------------

    /// <summary>
    /// Provides a key to identify an APNs channel uniquely.
    /// </summary>
    /// <seealso cref="ChannelKey" />
    /// <seealso cref="System.IEquatable{APNsChannelKey}" />

    [DataContract]
    public class ApnsChannelKey : ChannelKey, IEquatable<ApnsChannelKey>
    {
        #region properties

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
        [JsonProperty(PropertyName = "bundleId", NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonProperty(PropertyName = "environment", NullValueHandling = NullValueHandling.Ignore)]
        public Environment Environment { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="ApnsChannelKey"/> class from being created.
        /// </summary>

        private ApnsChannelKey() { ChannelType = ChannelType.ApplePushNotificationService; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApnsChannelKey" /> class
        /// with the specified application identifier and environment.
        /// </summary>
        /// <param name="bundleId">The bundle identifier.</param>
        /// <param name="environment">The environment.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="environment"/> is out of range.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="bundleId" /> is a null reference, or consists only of white-space caracters.
        /// </exception>

        public ApnsChannelKey(string bundleId, Environment environment) : this()
        {
            if (string.IsNullOrWhiteSpace(bundleId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrWhiteSpace, nameof(bundleId));
            }
            if (!Enum.IsDefined(typeof(Environment), environment)) throw new ArgumentOutOfRangeException(nameof(environment));

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
            return string.Format(CultureInfo.CurrentCulture, "ChannelType={0}, BundleId='{1}', Environment={2}", ChannelType, BundleId, Environment);
        }

        #endregion overrides

        #region IEquatable<APNsChannelKey>

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
            var channelKey = obj as ApnsChannelKey;
            if (channelKey == null) return false;

            return
                ChannelType == channelKey.ChannelType &&
                BundleId.Equals(channelKey.BundleId, StringComparison.OrdinalIgnoreCase) &&
                Environment == channelKey.Environment;
        }

        bool IEquatable<ApnsChannelKey>.Equals(ApnsChannelKey other)
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
            return new { A = BundleId, B = Environment }.GetHashCode();
        }

        #endregion IEquatable<APNsChannelKey>
    }
}

// eof "ApnsChannelKey.cs"
