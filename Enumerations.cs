//
//  @(#) Enumerations.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2019
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2019 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;

namespace usis.Net.Bits
{
    #region Constants class

    //  ---------------
    //  Constants class
    //  ---------------

    /// <summary>
    /// Provides constants for the BITS class library.
    /// </summary>

    public static class Constants
    {
        /// <summary>
        /// Indicates that a range extends to the end of the file.
        /// </summary>

        public const long LengthToEndOfFile = -1;
    }

    #endregion Constants class

    #region BackgroundCopyJobType enumeration

    //  ---------------------------------
    //  BackgroundCopyJobType enumeration
    //  ---------------------------------

    /// <summary>
    /// Defines constant values that specify the type of transfer job, such as download.
    /// </summary>

    public enum BackgroundCopyJobType
    {
        /// <summary>
        /// Specifies that the job downloads files to the client.
        /// </summary>

        Download = 0,

        /// <summary>
        /// Specifies that the job uploads a file to the server.
        /// </summary>

        Upload = 1,

        /// <summary>
        /// Specifies that the job uploads a file to the server
        /// and receives a reply file from the server application.
        /// </summary>

        UploadReply = 2,
    }

    #endregion BackgroundCopyJobType enumeration

    #region BackgroundCopyJobState enumeration

    //  ----------------------------------
    //  BackgroundCopyJobState enumeration
    //  ----------------------------------

    /// <summary>
    /// Defines constant values for the different states of a job.
    /// </summary>

    public enum BackgroundCopyJobState
    {
        /// <summary>
        /// Specifies that the job is in the queue and waiting to run.
        /// If a user logs off while their job is transferring,
        /// the job transitions to the queued state.
        /// </summary>

        Queued = 0,

        /// <summary>
        /// Specifies that BITS is trying to connect to the server.
        /// If the connection succeeds,
        /// the state of the job becomes <see cref="Transferring"/>;
        /// otherwise, the state becomes <see cref="Error"/>.
        /// </summary>

        Connecting = 1,

        /// <summary>
        /// Specifies that BITS is transferring data for the job.
        /// </summary>

        Transferring = 2,

        /// <summary>
        /// Specifies that the job is suspended (paused).
        /// To suspend a job, call the
        /// <see cref="BackgroundCopyJob.Suspend"/> method.
        /// BITS automatically suspends a job when it is created.
        /// The job remains suspended until you call the
        /// <see cref="BackgroundCopyJob.Resume"/>,
        /// <see cref="BackgroundCopyJob.Complete"/>, or
        /// <see cref="BackgroundCopyJob.Cancel"/> method.
        /// </summary>

        Suspended = 3,

        /// <summary>
        /// Specifies that a non-recoverable error occurred
        /// (the service is unable to transfer the file).
        /// If the error, such as an access-denied error,
        /// can be corrected, call the
        /// <see cref="BackgroundCopyJob.Resume"/>
        /// method after the error is fixed. However,
        /// if the error cannot be corrected, call the
        /// <see cref="BackgroundCopyJob.Cancel"/> method
        /// to cancel the job, or call the
        /// <see cref="BackgroundCopyJob.Complete"/> method
        /// to accept the portion of a download job
        /// that transferred successfully.
        /// </summary>

        Error = 4,

        /// <summary>
        /// Specifies that a recoverable error occurred.
        /// BITS will retry jobs in the transient error state
        /// based on the retry interval you specify
        /// (see <see cref="BackgroundCopyJob.MinimumRetryDelay"/>).
        /// The state of the job changes to <see cref="Error"/>
        /// if the job fails to make progress
        /// (see <see cref="BackgroundCopyJob.NoProgressTimeout"/>).
        /// BITS does not retry the job if a network disconnect
        /// or disk lock error occurred
        /// (for example, chkdsk is running)
        /// or the MaxInternetBandwidth Group Policy is zero.
        /// </summary>

        TransientError = 5,

        /// <summary>
        /// Specifies that your job was successfully processed.
        /// You must call the <see cref="BackgroundCopyJob.Complete"/>
        /// method to acknowledge completion of the job
        /// and to make the files available to the client.
        /// </summary>

        Transferred = 6,

        /// <summary>
        /// Specifies that you called the <see cref="BackgroundCopyJob.Complete"/>
        /// method to acknowledge that your job completed successfully.
        /// </summary>

        Acknowledged = 7,

        /// <summary>
        /// Specifies that you called the <see cref="BackgroundCopyJob.Cancel"/>
        /// method to cancel the job (remove the job from the transfer queue).
        /// </summary>

        Canceled = 8,
    }

    #endregion BackgroundCopyJobState enumeration

    #region BackgroundCopyJobPriority enumeration

    //  -------------------------------------
    //  BackgroundCopyJobPriority enumeration
    //  -------------------------------------

    /// <summary>
    /// Defines the constant values that specify the priority level of a job.
    /// </summary>

    public enum BackgroundCopyJobPriority
    {
        /// <summary>
        /// Transfers the job in the foreground.
        /// Foreground transfers compete for network bandwidth
        /// with other applications,
        /// which can impede the user's network experience.
        /// This is the highest priority level.
        /// </summary>

        Foreground = 0,

        /// <summary>
        /// Transfers the job in the background with a high priority.
        /// Background transfers use idle network bandwidth
        /// of the client to transfer files.
        /// This is the highest background priority level.
        /// </summary>

        High = 1,

        /// <summary>
        /// Transfers the job in the background with a normal priority.
        /// Background transfers use idle network bandwidth
        /// of the client to transfer files.
        /// This is the default priority level.
        /// </summary>

        Normal = 2,

        /// <summary>
        /// Transfers the job in the background with a low priority.
        /// Background transfers use idle network bandwidth
        /// of the client to transfer files.
        /// This is the lowest background priority level.
        /// </summary>

        Low = 3,
    }

    #endregion BackgroundCopyJobPriority enumeration

    #region BackgroundCopyJobProxyUsage enumeration

    //  ---------------------------------------
    //  BackgroundCopyJobProxyUsage enumeration
    //  ---------------------------------------

    /// <summary>
    /// defines constant values that specify which proxy to use for file transfers.
    /// You can define different proxy settings for each job.
    /// </summary>

    public enum BackgroundCopyJobProxyUsage
    {
        /// <summary>
        /// Use the proxy and proxy bypass list settings defined by each user to transfer files.
        /// Settings are user-defined from Control Panel, Internet Options, Connections,
        /// Local Area Network (LAN) settings (or Dial-up settings, depending on the network connection).
        /// </summary>

        Preconfigured = 0,

        /// <summary>
        /// Do not use a proxy to transfer files. Use this option when you transfer files within a LAN.
        /// </summary>

        NoProxy = 1,

        /// <summary>
        /// Use the application's proxy and proxy bypass list to transfer files.
        /// Use this option when you cannot trust that the system settings are correct.
        /// Also use this option when you want to transfer files using a special account,
        /// such as LocalSystem, to which the system settings do not apply.
        /// </summary>

        Override = 2,

        /// <summary>
        /// Automatically detect proxy settings. BITS detects proxy settings for each file in the job.
        /// </summary>
        /// <remarks>
        /// Not available for BITS 1.5 and earlier.
        /// </remarks>

        AutoDetect = 3,
    }

    #endregion BackgroundCopyJobProxyUsage enumeration

    #region BackgroundCopyJobNotifications enumeration

    //  ------------------------------------------
    //  BackgroundCopyJobNotifications enumeration
    //  ------------------------------------------

    /// <summary>
    /// Defines the constant values that specify the type of
    /// events you will receive, such as job transferred events.
    /// </summary>

    [Flags]
    public enum BackgroundCopyJobNotifications
    {
        /// <summary>
        /// All of the files in the job have been transferred.
        /// </summary>

        Transferred = 1,

        /// <summary>
        /// An error has occurred.
        /// </summary>

        Error = 2,

        /// <summary>
        /// Event notification is disabled.
        /// </summary>

        Disabled = 4,

        /// <summary>
        /// The job has been modified.
        /// For example, a property value changed,
        /// the state of the job changed,
        /// or progress is made transferring the files.
        /// </summary>

        Modification = 8,

        /// <summary>
        /// A file in the job has been transferred. This flag is ignored in
        /// command-line callbacks if command line notification is specified.
        /// </summary>

        FileTransferred = 0x10,
    }

    #endregion BackgroundCopyJobNotifications enumeration

    #region BackgroundCopyErrorContext enumeration

    //  --------------------------------------
    //  BackgroundCopyErrorContext enumeration
    //  --------------------------------------

    /// <summary>
    /// Defines the constant values that specify the context in which the error occurred.
    /// </summary>

    public enum BackgroundCopyErrorContext
    {
        /// <summary>
        /// An error has not occurred.
        /// </summary>

        None,

        /// <summary>
        /// The error context is unknown.
        /// </summary>

        Unknown,

        /// <summary>
        /// The transfer queue manager generated the error.
        /// </summary>

        GeneralQueueManager,

        /// <summary>
        /// The error was generated while the queue manager was notifying the client of an event.
        /// </summary>

        QueueManagerNotification,

        /// <summary>
        /// The error was related to the specified local file.
        /// For example, permission was denied or the volume was unavailable.
        /// </summary>

        LocalFile,

        /// <summary>
        /// he error was related to the specified remote file.
        /// For example, the URL was not accessible.
        /// </summary>

        RemoteFile,

        /// <summary>
        /// The transport layer generated the error.
        /// These errors are general transport failures (these errors are not specific to the remote file).
        /// </summary>

        GeneralTransport,

        /// <summary>
        /// The transport layer generated the error.
        /// These errors are general transport failures (these errors are not specific to the remote file).
        /// </summary>

        RemoteApplication
    }

    #endregion BackgroundCopyErrorContext enumeration

    #region BackgroundCopyAuthenticationTarget enumeration

    //  ----------------------------------------------
    //  BackgroundCopyAuthenticationTarget enumeration
    //  ----------------------------------------------

    /// <summary>
    /// Defines the constant values that specify whether the credentials
    /// are used for proxy or server user authentication requests.
    /// </summary>

    public enum BackgroundCopyAuthenticationTarget
    {
        /// <summary>
        /// Undefined
        /// </summary>

        None = 0,

        /// <summary>
        /// Use credentials for server requests.
        /// </summary>

        Server = 1,

        /// <summary>
        /// Use credentials for proxy requests.
        /// </summary>

        Proxy = 2
    }

    #endregion BackgroundCopyAuthenticationTarget enumeration

    #region BackgroundCopyAuthenticationScheme enumeration

    //  ----------------------------------------------
    //  BackgroundCopyAuthenticationScheme enumeration
    //  ----------------------------------------------

    /// <summary>
    /// Defines the constant values that specify the authentication scheme
    /// to use when a proxy or server requests user authentication.
    /// </summary>

    public enum BackgroundCopyAuthenticationScheme
    {
        /// <summary>
        /// No authentication scheme specified.
        /// </summary>

        None,

        /// <summary>
        /// Basic is a scheme in which the user name and password are sent in clear-text to the server or proxy.
        /// </summary>

        Basic,

        /// <summary>
        /// Digest is a challenge-response scheme that uses a server-specified data string for the challenge.
        /// </summary>

        Digest,

        /// <summary>
        /// NTLM is a challenge-response scheme that uses the credentials of the user for authentication in a Windows network environment.
        /// </summary>

        Ntlm,

        /// <summary>
        /// Simple and Protected Negotiation protocol (Snego) is a challenge-response scheme that negotiates with the server or proxy to determine which scheme to use for authentication. Examples are the Kerberos protocol and NTLM.
        /// </summary>

        Negotiate,

        /// <summary>
        /// Passport is a centralized authentication service provided by Microsoft that offers a single logon for member sites.
        /// </summary>

        Passport
    }

    #endregion BackgroundCopyAuthenticationScheme enumeration

    #region BackgroundCopyJobFileAclOptions enumeration

    //  -------------------------------------------
    //  BackgroundCopyJobFileAclOptions enumeration
    //  -------------------------------------------

    /// <summary>
    /// Flags that identify the owner and ACL information to maintain when transferring a file using SMB.
    /// </summary>

    [Flags]
    public enum BackgroundCopyJobFileAclOptions
    {
        /// <summary>
        /// If set, the file's owner information is maintained. Otherwise, the job's owner becomes the owner of the file.
        /// </summary>

        FileOwner = 1,

        /// <summary>
        /// If set, the file's group information is maintained.
        /// Otherwise, BITS uses the job owner's primary group to assign the group information to the file.
        /// </summary>

        FileGroup = 2,

        /// <summary>
        /// If set, BITS copies the explicit ACEs from the source file and inheritable ACEs from the destination parent folder.
        /// Otherwise, BITS copies the inheritable ACEs from the destination parent folder.
        /// If the parent folder does not contain inheritable ACEs, BITS uses the default DACL from the account.
        /// </summary>

        FileDacl = 4,

        /// <summary>
        /// If set, BITS copies the explicit ACEs from the source file and inheritable ACEs from the destination parent folder.
        /// Otherwise, BITS copies the inheritable ACEs from the destination parent folder.
        /// </summary>

        FileSacl = 8,

        /// <summary>
        /// If set, BITS copies the owner and ACL information. This is the same as setting all the flags individually.
        /// </summary>

        FileAll = FileOwner | FileGroup | FileDacl | FileSacl
    }

    #endregion BackgroundCopyJobFileAclOptions enumeration

    #region BackgroundCopyCertificateStoreLocation enumeration

    //  --------------------------------------------------
    //  BackgroundCopyCertificateStoreLocation enumeration
    //  --------------------------------------------------
    //  BG_CERT_STORE_LOCATION

    /// <summary>
    /// Defines the location of the certificate store.
    /// </summary>

    public enum BackgroundCopyCertificateStoreLocation
    {
        /// <summary>
        /// Use the current user's certificate store.
        /// </summary>

        CurrentUser,

        /// <summary>
        /// Use the local computer's certificate store.
        /// </summary>

        LocalMachine,

        /// <summary>
        /// Use the current service's certificate store.
        /// </summary>

        CurrentService,

        /// <summary>
        /// Use a specific service's certificate store.
        /// </summary>

        Services,

        /// <summary>
        /// Use a specific user's certificate store.
        /// </summary>

        Users,

        /// <summary>
        /// Use the current user's group policy certificate store.
        /// In a network setting, stores in this location are downloaded to the client computer
        /// from the Group Policy Template (GPT) during computer startup or user logon.
        /// </summary>

        CurrentUserGroupPolicy,

        /// <summary>
        /// Use the local computer's certificate store.
        /// In a network setting, stores in this location are downloaded to the client computer
        /// from the Group Policy Template (GPT) during computer startup or user logon.
        /// </summary>

        LocalMachineGroupPolicy,

        /// <summary>
        /// Use the enterprise certificate store.
        /// The enterprise store is shared across domains in the enterprise and downloaded
        /// from the global enterprise directory.
        /// </summary>

        LocalMachineEnterprise
    }

    #endregion BackgroundCopyCertificateStoreLocation enumeration

    #region BackgroundCopyJobHttpSecurityFlags enumeration

    //  ----------------------------------------------
    //  BackgroundCopyJobHttpSecurityFlags enumeration
    //  ----------------------------------------------

    /// <summary>
    /// HTTP security flags that indicate which errors to ignore when connecting to the server.
    /// </summary>

    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags]
    public enum BackgroundCopyJobHttpSecurityOptions
    {
        /// <summary>
        /// Allows the server to redirect your request to another server. This is the default.
        /// </summary>

        None = 0x000,

        /// <summary>
        /// Check the certificate revocation list (CRL) to verify that the server certificate has not been revoked.
        /// </summary>

        EnableCertificateRevocationListCheck = 0x0001,

        /// <summary>
        /// Ignores errors caused when the certificate host name of the server does not match the host name in the request.
        /// </summary>

        IgnoreCertificateHostNameInvalid = 0x0002,

        /// <summary>
        /// Ignores errors caused by an expired certificate.
        /// </summary>

        IgnoreCertificateDateInvalid = 0x0004,

        /// <summary>
        /// Ignore errors associated with an unknown certification authority (CA).
        /// </summary>

        IgnoreUnknownCertificationAuthority = 0x0008,

        /// <summary>
        /// Ignore errors associated with the use of a certificate.
        /// </summary>

        IgnoreCertificateWrongUsage = 0x0010,

        /// <summary>
        /// Allows the server to redirect your request to another server.
        /// BITS updates the remote name with the final URL.
        /// </summary>

        HttpRedirectPolicyAllowReport = 0x0100,

        /// <summary>
        /// Places the job in the fatal error state when the server redirects your request to another server.
        /// BITS updates the remote name with the redirected URL.
        /// </summary>

        HttpRedirectPolicyDisallow = 0x0200,

        /// <summary>
        /// Bitmask that you can use with the security flag value to determine which redirect policy is in effect.
        /// It does not include the flag <see cref="HttpRedirectPolicyAllowHttpsToHttp"/>.
        /// </summary>

        HttpRedirectPolicyMask = 0x0700,

        /// <summary>
        /// Allows the server to redirect an HTTPS request to an HTTP URL.
        /// You can combine this flag with <see cref="HttpRedirectPolicyAllowReport"/>.
        /// </summary>

        HttpRedirectPolicyAllowHttpsToHttp = 0x0800,
    }

    #endregion BackgroundCopyJobHttpSecurityFlags enumeration

    #region BackgroundCopyJobPeerCachingOptions enumeration

    //  -----------------------------------------------
    //  BackgroundCopyJobPeerCachingOptions enumeration
    //  -----------------------------------------------

    /// <summary>
    /// Flags that indicate determine if the files of the job can be cached and served to peers
    /// and if BITS can download content for the job from peers
    /// </summary>

    [Flags]
    public enum BackgroundCopyJobPeerCachingOptions
    {
        /// <summary>
        /// The job can download content from peers.
        /// </summary>

        EnableClient = 0x0001,

        /// <summary>
        /// The files of the job can be cached and served to peers.
        /// </summary>

        EnableServer = 0x0002
    }

    #endregion BackgroundCopyJobPeerCachingOptions enumeration
}

// eof "Enumerations.cs"
