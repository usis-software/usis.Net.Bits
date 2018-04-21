//
//  @(#) GcmReceiverInfo.cs
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
    //  ---------------------
    //  GcmReceiverInfo class
    //  ---------------------

    /// <summary>
    /// Provides informations about a registered WNS device.
    /// </summary>

    [DataContract]
    public class GcmReceiverInfo : IReceiverInfo, IEquatable<GcmReceiverInfo>
    {
        #region properties

        //  --------------------------
        //  RegistrationToken property
        //  --------------------------

        /// <summary>
        /// Gets or sets an application specific registration token.
        /// </summary>
        /// <value>
        /// An application specific registration token.
        /// </value>

        [DataMember]
        public string RegistrationToken { get; set; }

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
        /// Gets or sets the name of the receiver.
        /// </summary>
        /// <value>
        /// The name of the receiver.
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
        /// Gets or sets additional information for the receiver.
        /// </summary>
        /// <value>
        /// Additional information for the receiver.
        /// </value>

        [DataMember]
        public string Info { get; set; }

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
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Name='{0}', RegistrationToken='{1}'", Name, RegistrationToken);
        }

        #endregion methods

        #region IEquatable implementation

        //  -------------
        //  Equals method
        //  -------------

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>

        public override bool Equals(object obj)
        {
            var other = obj as GcmReceiverInfo;
            if (other == null) return false;
            else return RegistrationToken.Equals(other.RegistrationToken, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>

        bool IEquatable<GcmReceiverInfo>.Equals(GcmReceiverInfo other) { return Equals(other); }

        //  ------------------
        //  GetHashCode method
        //  ------------------

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>

        public override int GetHashCode() { return RegistrationToken.GetHashCode(); }

        #endregion IEquatable implementation
    }
}

// eof "GcmReceiverInfo.cs"
