//
//  @(#) Enumerations.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Sch�fer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;

namespace usis.Net.Bits
{
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
}

// eof "Enumerations.cs"
