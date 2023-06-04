//
//  @(#) BackgroundCopyJobClientCertificate.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2022
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018-2023 usis GmbH. All rights reserved.

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
        /// <value>The store location.</value>

        public BackgroundCopyCertificateStoreLocation StoreLocation { get; }

        //  ------------------
        //  StoreName property
        //  ------------------

        /// <summary>
        /// Gets the name of the store.
        /// </summary>
        /// <value>The name of the store.</value>

        public string StoreName { get; }

        //  -------------------
        //  Thumbprint property
        //  -------------------

        /// <summary>
        /// Gets the SHA1 hash that identifies the certificate.
        /// </summary>
        /// <value>The SHA1 hash that identifies the certificate.</value>

        internal byte[] Thumbprint { get; }

        //  --------------------
        //  SubjectName property
        //  --------------------

        /// <summary>
        /// Gets the simple subject name of the certificate.
        /// </summary>
        /// <value>The simple subject name of the certificate.</value>

        public string SubjectName { get; }

        #endregion

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyJobClientCertificate"/> class.
        /// </summary>
        /// <param name="location">The store location.</param>
        /// <param name="storeName">vThe name of the store.</param>
        /// <param name="thumbprint">The SHA1 hash that identifies the certificate.</param>
        /// <param name="subjectName">The simple subject name of the certificate.</param>

        public BackgroundCopyJobClientCertificate(
            BackgroundCopyCertificateStoreLocation location,
            string storeName,
            byte[] thumbprint,
            string subjectName)
        {
            StoreLocation = location;
            StoreName = storeName;
            Thumbprint = thumbprint;
            SubjectName = subjectName;
        }

        #endregion

        #region methods

        //  --------------------
        //  GetThumbprint method
        //  --------------------

        /// <summary>
        /// Gets the SHA1 hash that identifies the certificate.
        /// </summary>
        /// <returns>The SHA1 hash that identifies the certificate.</returns>

        public byte[] GetThumbprint() => Thumbprint;

        #endregion methods
    }
}

// eof "BackgroundCopyJobClientCertificate.cs"
