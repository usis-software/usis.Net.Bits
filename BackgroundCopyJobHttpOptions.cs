//
//  @(#) BackgroundCopyJobHttpOptions.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2022
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018-2023 usis GmbH. All rights reserved.

using System;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  ----------------------------------
    //  BackgroundCopyJobHttpOptions class
    //  ----------------------------------

    /// <summary>
    /// Allows to specify client certificates for certificate-based client authentication
    /// and custom headers for HTTP requests.
    /// </summary>

    public sealed class BackgroundCopyJobHttpOptions
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyJobHttpOptions(BackgroundCopyJob job) => Job = job;

        #endregion

        #region properties

        #region private properties

        //  ------------
        //  Job property
        //  ------------

        private BackgroundCopyJob Job { get; set; }

        #endregion

        #region public properties

        //  ----------------------
        //  CustomHeaders property
        //  ----------------------

        /// <summary>
        /// Gets or sets custom HTTP headers to include in HTTP requests.
        /// </summary>
        /// <value>
        /// The custom HTTP headers.
        /// </value>

        public string CustomHeaders
        {
            get => Job.Manager.InvokeComMethod(Job.HttpOptionsInterface.GetCustomHeaders);
            set => Job.Manager.InvokeComMethod(() => Job.HttpOptionsInterface.SetCustomHeaders(value));
        }

        //  ------------------------
        //  SecurityOptions property
        //  ------------------------

        /// <summary>
        /// Gets or sets flags for HTTP that determine whether the certificate revocation list is checked and certain certificate errors are ignored,
        /// and the policy to use when a server redirects the HTTP request.
        /// </summary>
        /// <value>
        /// The flags for HTTP that determine whether the certificate revocation list is checked and certain certificate errors are ignored,
        /// and the policy to use when a server redirects the HTTP request.
        /// </value>

        public BackgroundCopyJobHttpSecurityOptions SecurityOptions
        {
            get => Job.Manager.InvokeComMethod(Job.HttpOptionsInterface.GetSecurityFlags);
            set => Job.Manager.InvokeComMethod(() => Job.HttpOptionsInterface.SetSecurityFlags(value));
        }

        //  --------------------------
        //  ClientCertificate property
        //  --------------------------

        /// <summary>
        /// Gets the client certificate to use for authentication in an HTTPS (SSL) request.
        /// </summary>
        /// <value>
        /// An object that specifies the client certificate.
        /// </value>

        public BackgroundCopyJobClientCertificate? ClientCertificate
        {
            get
            {
                var hr = Job.HttpOptionsInterface.GetClientCertificate(out var storeLocation, out var storeName, out var thumbprint, out var subjectName);
                return HResult.Succeeded(hr)
                    ? hr != HResult.Ok ? null : new BackgroundCopyJobClientCertificate(storeLocation, storeName, thumbprint, subjectName)
                    : throw new InvalidOperationException(Job.Manager.GetErrorDescription(hr));
            }
        }

        #endregion

        #endregion

        #region methods

        //  -------------------------------
        //  SetClientCertificateById method
        //  -------------------------------

        /// <summary>
        /// Specifies the identifier of the client certificate
        /// to use for client authentication in an HTTPS (SSL) request.
        /// </summary>
        /// <param name="storeLocation">Identifies the location of a system store to use for looking up the certificate.</param>
        /// <param name="storeName">The name of the certificate store.</param>
        /// <param name="thumbprint">SHA1 hash that identifies the certificate. Use a 20 byte buffer for the hash.</param>
        /// <exception cref="ArgumentNullException"><paramref name="thumbprint" /> is a <c>null</c> reference.</exception>

        public void SetClientCertificateById(BackgroundCopyCertificateStoreLocation storeLocation, string storeName, byte[] thumbprint)
        {
            if (thumbprint == null) throw new ArgumentNullException(nameof(thumbprint));

            var data = new byte[20];
            for (var i = 0; i < Math.Min(thumbprint.Length, data.Length); i++)
            {
                data[i] = thumbprint[i];
            }
            Job.Manager.InvokeComMethod(() => Job.HttpOptionsInterface.SetClientCertificateByID(storeLocation, storeName, thumbprint));
        }

        //  ---------------------------------
        //  SetClientCertificateByName method
        //  ---------------------------------

        /// <summary>
        /// Specifies the subject name of the client certificate
        /// to use for client authentication in an HTTPS (SSL) request.
        /// </summary>
        /// <param name="storeLocation">Identifies the location of a system store to use for looking up the certificate.</param>
        /// <param name="storeName">The name of the certificate store.</param>
        /// <param name="subjectName">The simple subject name of the certificate.</param>

        public void SetClientCertificateByName(BackgroundCopyCertificateStoreLocation storeLocation, string storeName, string subjectName) => Job.Manager.InvokeComMethod(() => Job.HttpOptionsInterface.SetClientCertificateByName(storeLocation, storeName, subjectName));

        //  ------------------------------
        //  RemoveClientCertificate method
        //  ------------------------------

        /// <summary>
        /// Removes the client certificate from the job.
        /// </summary>

        public void RemoveClientCertificate() => Job.Manager.InvokeComMethod(Job.HttpOptionsInterface.RemoveClientCertificate);

        #endregion
    }
}

// eof "BackgroundCopyJobHttpOptions.cs"
