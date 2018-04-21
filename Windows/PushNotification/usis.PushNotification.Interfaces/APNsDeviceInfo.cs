//
//  @(#) APNsDeviceInfo.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  --------------------
    //  APNsDeviceInfo class
    //  --------------------

    /// <summary>
    /// Provides informations about a registered APNs device.
    /// </summary>

    [DataContract]
    public class APNsDeviceInfo
    {
        #region properties

        //  --------------------------
        //  Base64DeviceToken property
        //  --------------------------

        /// <summary>
        /// Gets or sets the base-64 string representation of the device token.
        /// </summary>
        /// <value>
        /// The base-64 string representation of the device token.
        /// </value>

        [DataMember]
        public string Base64DeviceToken { get; set; }

        //  --------------------
        //  DeviceToken property
        //  --------------------

        /// <summary>
        /// Gets the device token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>

        public APNsDeviceToken DeviceToken
        {
            get { return APNsDeviceToken.FromBase64(Base64DeviceToken); }
        }

        //  -------------------
        //  ReceiverId property
        //  -------------------

        /// <summary>
        /// Gets or sets the receiver identifier.
        /// </summary>
        /// <value>
        /// The receiver identifier.
        /// </value>

        [DataMember]
        public Guid ReceiverId { get; set; }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>

        [DataMember]
        public string Name { get; set; }

        //  ----------------
        //  Account property
        //  ----------------

        /// <summary>
        /// Gets or sets the user account.
        /// </summary>
        /// <value>
        /// The user account.
        /// </value>

        [DataMember]
        public string Account { get; set; }

        //  ---------------
        //  Groups property
        //  ---------------

        /// <summary>
        /// Gets or sets the groups that the account belongs to.
        /// </summary>
        /// <value>
        /// The groups that the account belongs to.
        /// </value>

        [DataMember]
        public string Groups { get; set; }

        //  -------------
        //  Info property
        //  -------------

        /// <summary>
        /// Gets or sets additional information for the device.
        /// </summary>
        /// <value>
        /// Additional information for the device.
        /// </value>

        [DataMember]
        public string Info { get; set; }

        //  ----------------
        //  Expired property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the device registration expired.
        /// </summary>
        /// <value>
        /// The date and time when the device registration expired.
        /// </value>

        [DataMember]
        public DateTime? Expired { get; set; }

        //  --------------------------
        //  FirstRegistration property
        //  --------------------------

        /// <summary>
        /// Gets or sets the date and time of the first registration.
        /// </summary>
        /// <value>
        /// The date and time of the first registration.
        /// </value>

        [DataMember]
        public DateTime FirstRegistration { get; set; }

        //  -------------------------
        //  LastRegistration property
        //  -------------------------

        /// <summary>
        /// Gets or sets the date and time of the last registration.
        /// </summary>
        /// <value>
        /// The date and time of the last registration.
        /// </value>

        [DataMember]
        public DateTime LastRegistration { get; set; }

        #endregion properties

        #region methods

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Name='{0}', DeviceToken='{1}'", Name, DeviceToken.HexString);
        }

        #endregion methods
    }
}

// eof "APNsDeviceInfo.cs"
