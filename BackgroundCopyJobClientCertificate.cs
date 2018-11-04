//
//  @(#) BackgroundCopyJobClientCertificate.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

namespace usis.Net.Bits
{
    //  ----------------------------------------
    //  BackgroundCopyJobClientCertificate class
    //  ----------------------------------------

    /// <summary>
    /// Provides properties that specify the client certificate for a job.
    /// </summary>

    public sealed class BackgroundCopyJobClientCertificate
    {
        #region properties

        //  ----------------------
        //  StoreLocation property
        //  ----------------------

        /// <summary>
        /// Gets the store location.
        /// </summary>
        /// <value>
        /// The store location.
        /// </value>

        public BackgroundCopyCertificateStoreLocation StoreLocation { get; internal set; }

        //  ------------------
        //  StoreName property
        //  ------------------

        /// <summary>
        /// Gets the name of the store.
        /// </summary>
        /// <value>
        /// The name of the store.
        /// </value>

        public string StoreName { get; internal set; }

        //  -------------------
        //  Thumbprint property
        //  -------------------

        /// <summary>
        /// Gets the SHA1 hash that identifies the certificate.
        /// </summary>
        /// <value>
        /// The SHA1 hash that identifies the certificate.
        /// </value>

        internal byte[] Thumbprint { get; set; }

        //  --------------------
        //  SubjectName property
        //  --------------------

        /// <summary>
        /// Gets the simple subject name of the certificate.
        /// </summary>
        /// <value>
        /// The simple subject name of the certificate.
        /// </value>

        public string SubjectName { get; internal set; }

        #endregion properties

        #region methods

        //  --------------------
        //  GetThumbprint method
        //  --------------------

        /// <summary>
        /// Gets the SHA1 hash that identifies the certificate.
        /// </summary>
        /// <returns>
        /// The SHA1 hash that identifies the certificate.
        /// </returns>

        public byte[] GetThumbprint() => Thumbprint;

        #endregion methods
    }
}

// eof "BackgroundCopyJobClientCertificate.cs"
