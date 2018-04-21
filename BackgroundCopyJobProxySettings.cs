//
//  @(#) BackgroundCopyJobProxySettings.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

namespace usis.Net.Bits
{
    //  ------------------------------------
    //  BackgroundCopyJobProxySettings class
    //  ------------------------------------

    /// <summary>
    /// Provides the proxy information that the job uses to transfer the files.
    /// </summary>

    public sealed class BackgroundCopyJobProxySettings
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyJobProxySettings"/> class.
        /// </summary>
        /// <param name="proxyUsage">The proxy to use for file transfer.</param>
        /// <param name="proxyList">A list of one or more proxies to use to transfer files.</param>
        /// <param name="proxyBypassList">
        /// An optional list of host names or IP addresses, or both, that were not routed through the proxy.
        /// </param>

        public BackgroundCopyJobProxySettings(BackgroundCopyJobProxyUsage proxyUsage, string proxyList, string proxyBypassList)
        {
            ProxyUsage = proxyUsage;
            ProxyList = proxyList;
            ProxyBypassList = proxyBypassList;
        }

        #endregion construction

        #region properties

        //  -------------------
        //  ProxyUsage property
        //  -------------------

        /// <summary>
        /// Gets or sets the proxy to use for file transfer.
        /// </summary>
        /// <value>
        /// The proxy to use for file transfer.
        /// </value>

        public BackgroundCopyJobProxyUsage ProxyUsage { get; }

        //  ------------------
        //  ProxyList property
        //  ------------------

        /// <summary>
        /// Gets or sets a list of one or more proxies to use to transfer files.
        /// </summary>
        /// <value>
        /// A string that contains one or more proxies to use to transfer files. The list is space-delimited. 
        /// </value>

        public string ProxyList { get; }

        //  ------------------------
        //  ProxyBypassList property
        //  ------------------------

        /// <summary>
        /// Gets or sets an optional list of host names or IP addresses, or both, that were not routed through the proxy.
        /// </summary>
        /// <value>
        /// A string that contains an optional list of host names or IP addresses, or both,
        /// that were not routed through the proxy. The list is space-delimited. 
        /// </value>

        public string ProxyBypassList { get; }

        #endregion properties
    }
}

// eof "BackgroundCopyJobProxySettings.cs"
