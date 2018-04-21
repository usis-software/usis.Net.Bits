//
//  @(#) ApnsChannelInfo.cs
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
    //  ---------------------
    //  ApnsChannelInfo class
    //  ---------------------

    /// <summary>
    /// Provides informations about an APNs channel.
    /// </summary>

    [DataContract]
    public class ApnsChannelInfo : IChannelInfo
    {
        #region properties

        //  ------------
        //  Key property
        //  ------------

        /// <summary>
        /// Gets or sets the APNs channel's key.
        /// </summary>
        /// <value>
        /// The APNs channel's key.
        /// </value>

        [DataMember]
        public ApnsChannelKey Key { get; set; }

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

        //  -----------------------
        //  HasCertificate property
        //  -----------------------

        /// <summary>
        /// Gets or sets a value indicating whether the channel has a certificate.
        /// </summary>
        /// <value>
        /// <c>true</c> if the channel has a certificate.; otherwise, <c>false</c>.
        /// </value>

        [DataMember]
        public bool HasCertificate { get; set; }

        //  ------------------------------
        //  CertificateThumbprint property
        //  ------------------------------

        /// <summary>
        /// Gets or sets the thumbprint of the channel's certificate.
        /// </summary>
        /// <value>
        /// The thumbprint of the channel's certificate.
        /// </value>
        /// <remarks>
        /// The thumbprint is dynamically generated when the certificate is
        /// installed in the certificate store.
        /// It does not physically exist in the certificate.
        /// The push notification router can find the certificate that is needed
        /// to open a channel in the local certificate store by this thumbprint.
        /// </remarks>

        [DataMember]
        public string CertificateThumbprint { get; set; }

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

// eof "ApnsChannelInfo.cs"
