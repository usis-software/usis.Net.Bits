//
//  @(#) BackgroundCopyJob.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2019
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2019 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  -----------------------
    //  BackgroundCopyJob class
    //  -----------------------

    /// <summary>
    /// Provides methods and properties to add files to the job,
    /// set the priority level of the job,
    /// determine the state of the job, and to start and stop the job.
    /// </summary>
    /// <seealso cref="IDisposable" />

    public sealed class BackgroundCopyJob : IDisposable
    {
        #region fields

        private IBackgroundCopyJob interop;
        private Callback callback;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyJob(BackgroundCopyManager manager, IBackgroundCopyJob i)
        {
            Manager = manager;
            interop = i ?? throw new ArgumentNullException(nameof(i));
            HttpOptions = new BackgroundCopyJobHttpOptions(this);
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (interop != null)
            {
                // free unmanaged resources
                if (callback != null)
                {
                    var hr = interop.SetNotifyInterface(null);
                    if (HResult.Succeeded(hr) || hr == HResult.RPC_E_DISCONNECTED) callback = null;
                }
                _ = Marshal.ReleaseComObject(interop);
                interop = null;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyJob"/> class.
        /// </summary>

        ~BackgroundCopyJob() { Dispose(); } // Do not change this code. Put cleanup code in Dispose(bool disposing) above.

        #endregion IDisposable implementation

        #region properties

        #region public properties

        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets the identifier of the job in the queue.
        /// </summary>
        /// <value>
        /// The identifier of the job in the queue.
        /// </value>

        public Guid Id => Manager.InvokeComMethod(Interface.GetId);

        //  --------------------
        //  DisplayName property
        //  --------------------

        /// <summary>
        /// Gets or sets the display name that identifies the job.
        /// </summary>
        /// <value>
        /// The display name that identifies the job.
        /// </value>

        public string DisplayName
        {
            get => Manager.InvokeComMethod(Interface.GetDisplayName);
            set => Manager.InvokeComMethod(() => Interface.SetDisplayName(value));
        }

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets or sets the description of the job.
        /// </summary>
        /// <value>
        /// The description of the job.
        /// </value>

        public string Description
        {
            get => Manager.InvokeComMethod(Interface.GetDescription);
            set => Manager.InvokeComMethod(() => Interface.SetDescription(value));
        }

        //  ----------------
        //  JobType property
        //  ----------------

        /// <summary>
        /// Gets the type of transfer being performed, such as a file download or upload.
        /// </summary>
        /// <value>
        /// The type of transfer being performed, such as a file download or upload.
        /// </value>

        public BackgroundCopyJobType JobType => Manager.InvokeComMethod(Interface.GetType);

        //  -----------------
        //  Priority property
        //  -----------------

        /// <summary>
        /// Gets or sets the priority level for the job.
        /// </summary>
        /// <value>
        /// The priority level for the job.
        /// </value>
        /// <remarks>
        /// The priority level determines when the job is processed relative to other jobs in the transfer queue.
        /// </remarks>

        public BackgroundCopyJobPriority Priority
        {
            get => Manager.InvokeComMethod(Interface.GetPriority);
            set => Manager.InvokeComMethod(() => Interface.SetPriority(value));
        }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets the state of the job.
        /// </summary>
        /// <value>
        /// The state of the job.
        /// </value>

        public BackgroundCopyJobState State => Manager.InvokeComMethod(Interface.GetState);

        //  --------------
        //  Owner property
        //  --------------

        /// <summary>
        /// Gets the identity of the job's owner.
        /// </summary>
        /// <value>
        /// The identity of the job's owner.
        /// </value>

        public string Owner => Manager.InvokeComMethod(Interface.GetOwner);

        //  --------------------------
        //  MinimumRetryDelay property
        //  --------------------------

        /// <summary>
        /// Gets or sets the minimum length of time that BITS waits after encountering
        /// a transient error condition before trying to transfer the file.
        /// </summary>
        /// <value>
        /// Length of time, in seconds, that the service waits after encountering
        /// a transient error before trying to transfer the file.
        /// </value>
        /// <remarks>
        /// The default retry delay is 600 seconds (10 minutes).
        /// The minimum retry delay that you can specify is 5 seconds.
        /// If you specify a value less than 5 seconds, BITS changes the value to 5 seconds.
        /// If the value exceeds the no-progress-timeout value set by the <see cref="NoProgressTimeout"/> property,
        /// BITS will not retry the transfer and moves the job to the <see cref="BackgroundCopyJobState.Error"/> state.
        /// </remarks>

        public int MinimumRetryDelay
        {
            get => Manager.InvokeComMethod(Interface.GetMinimumRetryDelay);
            set => Manager.InvokeComMethod(() => Interface.SetMinimumRetryDelay(value));
        }

        //  --------------------------
        //  NoProgressTimeout property
        //  --------------------------

        /// <summary>
        /// Gets or sets the length of time that BITS tries to transfer the file
        /// after a transient error condition occurs. If there is progress, the timer is reset.
        /// </summary>
        /// <value>
        /// Length of time, in seconds, that the service tries to transfer the file after a transient error occurs.
        /// </value>

        public int NoProgressTimeout
        {
            get => Manager.InvokeComMethod(Interface.GetNoProgressTimeout);
            set => Manager.InvokeComMethod(() => Interface.SetNoProgressTimeout(value));
        }

        //  ----------------------
        //  Notifications property
        //  ----------------------

        /// <summary>
        /// Gets the event notification flags for your application.
        /// </summary>
        /// <value>
        /// The event notification flags for your application.
        /// </value>

        public BackgroundCopyJobNotifications Notifications
        {
            get => Manager.InvokeComMethod(GetNotifyFlags);
            private set => Manager.InvokeComMethod(() => Interface.SetNotifyFlags(value));
        }

        //  -------------------
        //  ErrorCount property
        //  -------------------

        /// <summary>
        /// Gets the number of times BITS tried to transfer the job and an error occurred.
        /// </summary>
        /// <value>
        /// The number of times BITS tried to transfer the job and an error occurred.
        /// </value>

        public int ErrorCount => Manager.InvokeComMethod(Interface.GetErrorCount);

        //  ----------------------
        //  ProxySettings property
        //  ----------------------

        /// <summary>
        /// Gets or sets the proxy information that the job uses to transfer the files.
        /// </summary>
        /// <value>
        /// The proxy information that the job uses to transfer the files.
        /// </value>

        public BackgroundCopyJobProxySettings ProxySettings
        {
            get => RetrieveProxySettings();
            set => SetProxySettings(value);
        }

        //  --------------------------
        //  NotifyCommandLine property
        //  --------------------------

        /// <summary>
        /// Gets or sets the program to execute when the job enters the error or transferred state.
        /// </summary>
        /// <value>
        /// The program to execute when the job enters the error or transferred state.
        /// </value>

        public BackgroundCopyNotifyCommandLine NotifyCommandLine
        {
            get => Manager.InvokeComMethod(() =>
            {
                Interface2.GetNotifyCmdLine(out var program, out var parameters);
                return new BackgroundCopyNotifyCommandLine(program, parameters);
            });
            set => Manager.InvokeComMethod(() =>
            {
                if (value == null) Interface2.SetNotifyCmdLine(null, null);
                else Interface2.SetNotifyCmdLine(value.Program, value.Parameters);
            });
        }

        //  ----------------------
        //  ReplyFileName property
        //  ----------------------

        /// <summary>
        /// Gets or sets the name of the file to contain the reply data of an upload-reply job.
        /// </summary>
        /// <value>
        /// The name of the file to contain the reply data of an upload-reply job.
        /// </value>

        public string ReplyFileName
        {
            get
            {
                var hr = Interface2.GetReplyFileName(out var replyFileName);
                return hr == HResult.Ok ? replyFileName : null;
            }
            set => Manager.InvokeComMethod(() => Interface2.SetReplyFileName(value));
        }

        //  ----------------
        //  FileAcl property
        //  ----------------

        /// <summary>
        /// Gets or sets the flags that identify the owner and ACL information to maintain when transferring a file using SMB.
        /// </summary>
        /// <value>
        /// The flags that identify the owner and ACL information to maintain when transferring a file using SMB.
        /// </value>

        public BackgroundCopyJobFileAclOptions FileAcl
        {
            get => (BackgroundCopyJobFileAclOptions)Interface3.GetFileACLFlags();
            set => Interface3.SetFileACLFlags(Convert.ToUInt32(value, CultureInfo.InvariantCulture));
        }

        //  --------------------
        //  HttpOptions property
        //  --------------------

        /// <summary>
        /// Gets the HTTP options to specify client certificates for certificate-based client authentication
        /// and custom headers for HTTP requests.
        /// </summary>
        /// <value>
        /// The HTTP options.
        /// </value>

        public BackgroundCopyJobHttpOptions HttpOptions { get; }

        //  -----------------------------
        //  PeerCachingOptions properties
        //  -----------------------------

        /// <summary>
        ///   Gets or sets options that determine if the files of the job can be cached
        ///   and served to peers and if BITS can download content for the job from peers.
        /// </summary>
        /// <value>
        ///   Options that determine if the files of the job can be cached and served to peers
        ///   and if BITS can download content for the job from peers.
        /// </value>

        public BackgroundCopyJobPeerCachingOptions PeerCachingOptions
        {
            get => Manager.InvokeComMethod(() => Interface4.GetPeerCachingFlags());
            set => Manager.InvokeComMethod(() => Interface4.SetPeerCachingFlags(value));
        }

        //  ----------------------------
        //  MaximumDownloadTime property
        //  ----------------------------

        /// <summary>
        /// Gets or sets the maximum time that BITS will spend transferring the files in the job.
        /// </summary>
        /// <value>
        /// The maximum time that BITS will spend transferring the files in the job.
        /// </value>

        public int MaximumDownloadTime
        {
            get => Manager.InvokeComMethod(() => Convert.ToInt32(Interface4.GetMaximumDownloadTime()));
            set => Manager.InvokeComMethod(() => Interface4.SetMaximumDownloadTime(Convert.ToUInt32(value)));
        }

        //  ----------------------------
        //  OwnerElevationState property
        //  ----------------------------

        /// <summary>
        /// Gets a value that determines if the token of the owner was elevated at the time they created or took ownership of the job.
        /// </summary>
        /// <value>
        /// <c>true</c> if the token of the owner was elevated at the time they created or took ownership of the job; otherwise, <c>false</c>.
        /// </value>

        public bool OwnerElevationState => Manager.InvokeComMethod(Interface4.GetOwnerElevationState);

        //  ----------------------------
        //  OwnerIntegrityLevel property
        //  ----------------------------

        /// <summary>
        /// Gets the integrity level of the token of the owner that created or took ownership of the job.
        /// </summary>
        /// <value>
        /// The integrity level of the token of the owner that created or took ownership of the job.
        /// </value>

        public int OwnerIntegrityLevel => Convert.ToInt32(Manager.InvokeComMethod(Interface4.GetOwnerIntegrityLevel));

        #endregion public properties

        #region private properties

        //  ----------------
        //  Manager property
        //  ----------------

        internal BackgroundCopyManager Manager { get; }

        //  ------------------
        //  Interface property
        //  ------------------

        private IBackgroundCopyJob Interface => interop ?? throw new ObjectDisposedException(nameof(BackgroundCopyJob));

        //  -------------------
        //  Interface2 property
        //  -------------------

        private IBackgroundCopyJob2 Interface2 => Extensions.QueryInterface<IBackgroundCopyJob2>(Interface);

        //  -------------------
        //  Interface3 property
        //  -------------------

        private IBackgroundCopyJob3 Interface3 => Extensions.QueryInterface<IBackgroundCopyJob3>(Interface);

        //  -------------------
        //  Interface4 property
        //  -------------------

        private IBackgroundCopyJob4 Interface4 => Extensions.QueryInterface<IBackgroundCopyJob4>(Interface);

        //  -----------------------------
        //  HttpOptionsInterface property
        //  -----------------------------

        internal IBackgroundCopyJobHttpOptions HttpOptionsInterface => Extensions.QueryInterface<IBackgroundCopyJobHttpOptions>(Interface);

        #endregion private properties

        #endregion properties

        #region events

        #region Failed event

        //  ------------
        //  Failed event
        //  ------------

        private EventHandler<BackgroundCopyErrorEventArgs> failedHandler;

        /// <summary>
        /// Occurs when the state of the job changes to <see cref="BackgroundCopyJobState.Error"/>.
        /// </summary>
        /// <remarks>
        /// BITS implements job notifications by callbacks.
        /// When an event handler is added or removed a corresponding callback interface is set.
        /// If the access to set a notification callback is denied an exception is <b>not</b> thrown
        /// and the event handler is added anyway.
        /// Use the <see cref="Notifications"/> property to check what notifications you receive.
        /// </remarks>

        public event EventHandler<BackgroundCopyErrorEventArgs> Failed
        {
            add
            {
                if (CheckCallback()) Notifications |= BackgroundCopyJobNotifications.Error;
                failedHandler += value;
            }
            remove
            {
                RemoveEvent(failedHandler, BackgroundCopyJobNotifications.Error, value);
            }
        }

        //  ---------------
        //  OnFailed method
        //  ---------------

        private uint OnFailed(IBackgroundCopyError error)
        {
            using (var e = new BackgroundCopyError(Manager, error))
            {
                return InvokeHandler(failedHandler, new BackgroundCopyErrorEventArgs(e));
            }
        }

        #endregion Failed event

        #region Modified event

        //  --------------
        //  Modified event
        //  --------------

        private EventHandler<EventArgs> modifiedHandler;

        /// <summary>
        /// Occurs when a job is modified.
        /// </summary>
        /// <remarks>
        /// BITS implements job notifications by callbacks.
        /// When an event handler is added or removed a corresponding callback interface is set.
        /// If the access to set a notification callback is denied an exception is <b>not</b> thrown
        /// and the event handler is added anyway.
        /// Use the <see cref="Notifications"/> property to check what notifications you receive.
        /// </remarks>

        public event EventHandler<EventArgs> Modified
        {
            add
            {
                if (CheckCallback()) Notifications |= BackgroundCopyJobNotifications.Modification;
                modifiedHandler += value;
            }
            remove
            {
                RemoveEvent(modifiedHandler, BackgroundCopyJobNotifications.Modification, value);
            }
        }

        private uint OnModified(/*int reserved*/) => InvokeHandler(modifiedHandler, EventArgs.Empty);

        #endregion Modified event

        #region Transferred event

        //  -----------------
        //  Transferred event
        //  -----------------

        private EventHandler<EventArgs> transferredHandler;

        /// <summary>
        /// Occurs when all of the files in the job have successfully transferred.
        /// </summary>
        /// <remarks>
        /// BITS implements job notifications by callbacks.
        /// When an event handler is added or removed a corresponding callback interface is set.
        /// If the access to set a notification callback is denied an exception is <b>not</b> thrown
        /// and the event handler is added anyway.
        /// Use the <see cref="Notifications"/> property to check what notifications you receive.
        /// </remarks>

        public event EventHandler<EventArgs> Transferred
        {
            add
            {
                if (CheckCallback()) Notifications |= BackgroundCopyJobNotifications.Transferred;
                transferredHandler += value;
            }
            remove
            {
                RemoveEvent(transferredHandler, BackgroundCopyJobNotifications.Transferred, value);
            }
        }

        private uint OnTransferred() => InvokeHandler(transferredHandler, EventArgs.Empty);

        #endregion Transferred event

        #region FileTransferred event

        //  ---------------------
        //  FileTransferred event
        //  ---------------------

        private EventHandler<BackgroundCopyFileEventArgs> fileTransferredHandler;

        /// <summary>
        /// Occurs when BITS successfully finishes transferring a file.
        /// </summary>

        public event EventHandler<BackgroundCopyFileEventArgs> FileTransferred
        {
            add
            {
                if (CheckCallback()) Notifications |= BackgroundCopyJobNotifications.FileTransferred;
                fileTransferredHandler += value;
            }
            remove
            {
                RemoveEvent(fileTransferredHandler, BackgroundCopyJobNotifications.FileTransferred, value);
            }
        }

        //  ------------------------
        //  OnFileTransferred method
        //  ------------------------

        private uint OnFileTransferred(IBackgroundCopyFile file)
        {
            using (var f = new BackgroundCopyFile(Manager, file))
            {
                return InvokeHandler(fileTransferredHandler, new BackgroundCopyFileEventArgs(f));
            }
        }

        #endregion FileTransferred event

        #endregion events

        #region methods

        #region public methods

        //  -------------
        //  Cancel method
        //  -------------

        /// <summary>
        /// Deletes the job from the transfer queue and removes related temporary files
        /// from the client (downloads) and server (uploads).
        /// </summary>

        public void Cancel() => Manager.InvokeComMethod(Interface.Cancel);

        //  ---------------
        //  Complete method
        //  ---------------

        /// <summary>
        /// Ends the job and saves the transferred files on the client.
        /// </summary>

        public void Complete() => Manager.InvokeComMethod(Interface.Complete);

        //  --------------
        //  Suspend method
        //  --------------

        /// <summary>
        /// Suspends a job. New jobs, jobs that are in error,
        /// and jobs that have finished transferring files are automatically suspended.
        /// </summary>

        public void Suspend() => Manager.InvokeComMethod(Interface.Suspend);

        //  -------------
        //  Resume method
        //  -------------

        /// <summary>
        /// Activates a new job or restarts a job that has been suspended.
        /// </summary>

        public void Resume() => Manager.InvokeComMethod(Interface.Resume);

        //  ---------------------
        //  EnumerateFiles method
        //  ---------------------

        /// <summary>
        /// Enumerates the files in the job.
        /// </summary>
        /// <returns>
        /// An enumerator that is used to iterate the files in the job.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The method was called after the object was disposed.</exception>

        public IEnumerable<BackgroundCopyFile> EnumerateFiles()
        {
            if (interop == null) throw new ObjectDisposedException(nameof(BackgroundCopyJob));
            IEnumBackgroundCopyFiles files = null;
            try
            {
                files = interop.EnumFiles();
                while (files.Next(1, out var file, IntPtr.Zero) == HResult.Ok)
                {
                    try { yield return new BackgroundCopyFile(Manager, file); }
                    finally { _ = Marshal.ReleaseComObject(file); }
                }
            }
            finally { if (files != null) _ = Marshal.ReleaseComObject(files); }
        }

        //  -----------------------
        //  RetrieveProgress method
        //  -----------------------

        /// <summary>
        /// Retrieves job-related progress information, such as the number of bytes and files transferred.
        /// </summary>
        /// <returns>
        /// A <see cref="BackgroundCopyJobProgress"/> object that contains data that you can use
        /// to calculate the percentage of the job that is complete.
        /// </returns>

        public BackgroundCopyJobProgress RetrieveProgress() => new BackgroundCopyJobProgress(Interface.GetProgress());

        //  ----------------------------
        //  RetrieveReplyProgress method
        //  ----------------------------

        /// <summary>
        /// Retrieves progress information that indicates how many bytes of the reply file have been downloaded to the client.
        /// </summary>
        /// <returns>
        /// A <see cref="BackgroundCopyJobReplyProgress"/> object that contains information that you use
        /// to calculate the percentage of the reply file transfer that is complete.
        /// </returns>

        public BackgroundCopyJobReplyProgress RetrieveReplyProgress() => new BackgroundCopyJobReplyProgress(Manager.InvokeComMethod(() => Interface2.GetReplyProgress()));

        //  --------------------
        //  RetrieveTimes method
        //  --------------------

        /// <summary>
        /// Retrieves job-related time stamps, such as the time that the job was created or last modified.
        /// </summary>
        /// <returns>
        /// A <c>BackgroundCopyJobTimes</c> structure that contains job-related time stamps.
        /// </returns>

        public BackgroundCopyJobTimes RetrieveTimes() => new BackgroundCopyJobTimes(Interface.GetTimes());

        //  --------------
        //  AddFile method
        //  --------------

        /// <summary>
        /// Adds a single file to the job.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>

        public void AddFile(string remoteUrl, string localName) => AddFile(new Uri(remoteUrl), localName);

        /// <summary>
        /// Adds a single file to the job.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>

        public void AddFile(Uri remoteUrl, string localName) => Manager.InvokeComMethod(() => Interface.AddFile(remoteUrl.ToString(), localName));

        /// <summary>
        /// Adds a file to a download job and specifies the range of the file you want to download.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>
        /// <param name="offset">Zero-based offset to the beginning of the range of bytes to download from a file.</param>
        /// <param name="length">The length of the range, in bytes. Do not specify a zero byte length.
        /// To indicate that the range extends to the end of the file, specify <see cref="Constants.LengthToEndOfFile" />.</param>

        public void AddFile(string remoteUrl, string localName, long offset, long length) => AddFile(new Uri(remoteUrl), localName, offset, length);

        /// <summary>
        /// Adds a file to a download job and specifies the range of the file you want to download.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>
        /// <param name="offset">Zero-based offset to the beginning of the range of bytes to download from a file.</param>
        /// <param name="length">The length of the range, in bytes. Do not specify a zero byte length.
        /// To indicate that the range extends to the end of the file, specify <see cref="Constants.LengthToEndOfFile" />.</param>

        public void AddFile(Uri remoteUrl, string localName, long offset, long length) => AddFile(remoteUrl, localName, new BackgroundCopyFileRange(offset, length));

        /// <summary>
        /// Adds a file to a download job and specifies the ranges of the file you want to download.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>
        /// <param name="ranges">An array of one or more <see cref="BackgroundCopyFileRange" /> structures that specify the ranges to download.
        /// Do not specify duplicate or overlapping ranges.</param>

        public void AddFile(string remoteUrl, string localName, params BackgroundCopyFileRange[] ranges) => AddFile(new Uri(remoteUrl), localName, ranges);

        /// <summary>
        /// Adds a file to a download job and specifies the ranges of the file you want to download.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>
        /// <param name="ranges">An array of one or more <see cref="BackgroundCopyFileRange" /> structures that specify the ranges to download.
        /// Do not specify duplicate or overlapping ranges.</param>

        public void AddFile(Uri remoteUrl, string localName, params BackgroundCopyFileRange[] ranges)
        {
            var fileRanges = ranges.Select(r => r.ToFileRange()).ToArray();
            Manager.InvokeComMethod(() => Interface3.AddFileWithRanges(remoteUrl.ToString(), localName, Convert.ToUInt32(fileRanges.Length), fileRanges));
        }

        //  ---------------
        //  AddFiles method
        //  ---------------

        /// <summary>
        /// Adds multiple files to the job.
        /// </summary>
        /// <param name="files">The files to add to the job.</param>
        /// <example>
        /// The following sample creates a job and adds three files to it.
        /// <code language="cs">
        /// using usis.Net.Bits;
        ///
        /// namespace BitsTest
        /// {
        ///     internal static class Sample
        ///     {
        ///         internal static void Main()
        ///         {
        ///             using (var manager = BackgroundCopyManager.Connect())
        ///             {
        ///                 using (var job = manager.CreateJob("Test", BackgroundCopyJobType.Download))
        ///                 {
        ///                     job.AddFiles(
        ///                         new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits1", LocalName = @"C:\tmp\test1.dat" },
        ///                         new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits2", LocalName = @"C:\tmp\test2.dat" },
        ///                         new BackgroundCopyFileInfo { RemoteName = "http://localhost/bits3", LocalName = @"C:\tmp\test3.dat" });
        ///                     job.Resume();
        ///                 }
        ///             }
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>

        public void AddFiles(params BackgroundCopyFileInfo[] files) => Manager.InvokeComMethod(() => Interface.AddFileSet(files.Length, files.Select(e => e.fileInfo).ToArray()));

        //  --------------------
        //  TakeOwnership method
        //  --------------------

        /// <summary>
        /// Changes ownership of the job to the current user.
        /// </summary>

        public void TakeOwnership() => Manager.InvokeComMethod(() => Interface.TakeOwnership());

        //  --------------------
        //  RetrieveError method
        //  --------------------

        /// <summary>
        /// Retrieves error informations after an error occurs.
        /// </summary>
        /// <returns>
        /// An object that provides error informations.
        /// </returns>

        public BackgroundCopyError RetrieveError() => RetrieveError(false);

        //  ------------------------
        //  RetrieveReplyData method
        //  ------------------------

        /// <summary>
        /// Retrieves the reply data from the server application.
        /// </summary>
        /// <returns>
        /// The reply data from the server application.
        /// </returns>
        /// <exception cref="BackgroundCopyException">Failed to retrieve reply data.</exception>

        public byte[] RetrieveReplyData()
        {
            var hr = Interface2.GetReplyData(out var buffer, out var lenght);
            if (HResult.Succeeded(hr))
            {
                var data = new byte[lenght];
                Marshal.Copy(buffer, data, 0, (int)lenght);
                Marshal.FreeCoTaskMem(buffer);
                return data;
            }
            else throw new BackgroundCopyException(Manager, hr);
        }

        //  ---------------------
        //  SetCredentials method
        //  ---------------------

        /// <summary>
        /// Specifies the credentials to use for a proxy or remote server user authentication request.
        /// </summary>
        /// <param name="target">Identifies whether to use the credentials for a proxy or server authentication request.</param>
        /// <param name="scheme">Identifies the scheme to use for authentication (for example, Basic or NTLM).</param>
        /// <param name="userName">
        /// The user name to authenticate. The user name is limited to 300 characters, not including the null terminator.
        /// The format of the user name depends on the authentication scheme requested.
        /// For example, for Basic, NTLM, and Negotiate authentication, the user name is of the form <i>DomainName\UserName</i>.
        /// For Passport authentication, the user name is an email address.
        /// </param>
        /// <param name="password">
        /// The password in plaintext. The password is limited to 65536 characters,
        /// not including the null terminator. The password can be blank. Set it to <c>null</c> if <b>UserName</b> is <c>null</c>.
        /// BITS encrypts the password before persisting the job if a network disconnect occurs or the user logs off.
        /// </param>

        public void SetCredentials(BackgroundCopyAuthenticationTarget target, BackgroundCopyAuthenticationScheme scheme, string userName, string password)
        {
            Manager.InvokeComMethod(() => Interface2.SetCredentials(new BG_AUTH_CREDENTIALS()
            {
                Target = target,
                Scheme = scheme,
                UserName = userName,
                Password = password
            }));
        }

        //  ------------------------
        //  RemoveCredentials method
        //  ------------------------

        /// <summary>
        /// Removes credentials set by the <see cref="SetCredentials"/> method.
        /// </summary>
        /// <param name="target">Identifies whether to use the credentials for proxy or server authentication.</param>
        /// <param name="scheme">Identifies the authentication scheme to use (basic or one of several challenge-response schemes).</param>

        public void RemoveCredentials(BackgroundCopyAuthenticationTarget target, BackgroundCopyAuthenticationScheme scheme) => Manager.InvokeComMethod(() => Interface2.RemoveCredentials(target, scheme));

        //  --------------------------
        //  ReplaceRemotePrefix method
        //  --------------------------

        /// <summary>
        /// Replaces the beginning text of all remote names in the download job with the specified string.
        /// </summary>
        /// <param name="oldPrefix">Identifies the text to replace in the remote name. The text must start at the beginning of the remote name.</param>
        /// <param name="newPrefix">The replacement text.</param>

        public void ReplaceRemotePrefix(string oldPrefix, string newPrefix) => Manager.InvokeComMethod(() => Interface3.ReplaceRemotePrefix(oldPrefix, newPrefix));

        #endregion public methods

        #region private methods

        //  ----------------------------
        //  RetrieveProxySettings method
        //  ----------------------------

        /// <summary>
        /// Retrieves the proxy information that the job uses to transfer the files.
        /// </summary>
        /// <returns>
        /// A <c>BackgroundCopyJobProxySettings</c> class with the proxy settings.
        /// </returns>

        private BackgroundCopyJobProxySettings RetrieveProxySettings()
        {
            Interface.GetProxySettings(out var usage, out var list, out var bypassList);
            return new BackgroundCopyJobProxySettings(usage, list, bypassList);
        }

        //  -----------------------
        //  SetProxySettings method
        //  -----------------------

        /// <summary>
        /// Specifies which proxy to use to transfer files.
        /// </summary>
        /// <param name="settings">The proxy settings.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="settings"/> is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        private void SetProxySettings(BackgroundCopyJobProxySettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            Manager.InvokeComMethod(() => Interface.SetProxySettings(settings.ProxyUsage, settings.ProxyList, settings.ProxyBypassList));
        }

        //  --------------------
        //  RetrieveError method
        //  --------------------

        internal BackgroundCopyError RetrieveError(bool throwException)
        {
            var hr = Interface.GetError(out var error);
            if (hr == HResult.Ok) return new BackgroundCopyError(Manager, error);
            else if (hr == HResult.BG_E_ERROR_INFORMATION_UNAVAILABLE && !throwException) return null;
            else throw new BackgroundCopyException(Manager, hr);
        }

        //  ------------------
        //  RemoveEvent method
        //  ------------------

        private void RemoveEvent<TEventArgs>(EventHandler<TEventArgs> handler, BackgroundCopyJobNotifications flags, EventHandler<TEventArgs> value)
        {
            handler -= value;
            if (handler == null)
            {
                var hr = Interface.GetNotifyFlags(out var notifyFlags);
                if (hr != HResult.RPC_E_DISCONNECTED &&
                    hr != Win32Error.RPC_S_SERVER_UNAVAILABLE)
                {
                    if (HResult.Succeeded(hr)) Notifications = notifyFlags & ~flags;
                    else throw new BackgroundCopyException(Manager, hr);
                }
            }
        }

        //  ---------------------
        //  GetNotifyFlags method
        //  ---------------------

        private BackgroundCopyJobNotifications GetNotifyFlags()
        {
            var hr = Interface.GetNotifyFlags(out var notifyFlags);
            if (!HResult.Succeeded(hr)) throw new BackgroundCopyException(Manager, hr);
            return notifyFlags;
        }

        //  --------------------
        //  CheckCallback method
        //  --------------------

        private bool CheckCallback()
        {
            if (callback == null)
            {
                callback = new Callback(this);
                var hr = interop.SetNotifyInterface(callback);
                if (!HResult.Succeeded(hr))
                {
                    callback = null;
                    if (hr != HResult.E_ACCESSDENIED) throw new BackgroundCopyException(Manager, hr);
                    else return false;
                }
            }
            return true;
        }

        //  --------------------
        //  InvokeHandler method
        //  --------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private uint InvokeHandler<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
        {
            try
            {
                handler?.Invoke(this, e);
                if (Interface is IBackgroundCopyJob2 job2)
                {
                    job2.GetNotifyCmdLine(out var program, out var parameters);
                    if (program != null) return HResult.Fail;
                }
                return HResult.Ok;
            }
            catch (Exception exception)
            {
                return (uint)exception.HResult;
            }
        }

        #endregion private methods

        #endregion methods

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

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0}: Id={1}, Name='{2}', Notifications={3}", nameof(BackgroundCopyJob), Id, DisplayName, Notifications);

        #endregion overrides

        #region Callback class

        //  --------------
        //  Callback class
        //  --------------

        [ComVisible(true)]
        private class Callback : IBackgroundCopyCallback, IBackgroundCopyCallback2
        {
            #region properties

            //  ------------
            //  Job property
            //  ------------

            private BackgroundCopyJob Job { get; }

            #endregion properties

            #region construction

            //  ------------
            //  construction
            //  ------------

            internal Callback(BackgroundCopyJob job) => Job = job;

            #endregion construction

            #region IBackgroundCopyCallback implementation

            //  ---------------------
            //  JobTransferred method
            //  ---------------------

            uint IBackgroundCopyCallback.JobTransferred(IBackgroundCopyJob job) => Job.OnTransferred();

            //  ---------------
            //  JobError method
            //  ---------------

            uint IBackgroundCopyCallback.JobError(IBackgroundCopyJob job, IBackgroundCopyError error) => Job.OnFailed(error);

            //  ----------------------
            //  JobModification method
            //  ----------------------

            uint IBackgroundCopyCallback.JobModification(IBackgroundCopyJob job, int reserved) => Job.OnModified(/*reserved*/);

            #endregion IBackgroundCopyCallback implementation

            #region IBackgroundCopyCallback2 implementation

            //  ---------------------
            //  JobTransferred method
            //  ---------------------

            uint IBackgroundCopyCallback2.JobTransferred(IBackgroundCopyJob job) => Job.OnTransferred();

            //  ---------------
            //  JobError method
            //  ---------------

            uint IBackgroundCopyCallback2.JobError(IBackgroundCopyJob job, IBackgroundCopyError error) => Job.OnFailed(error);

            //  ----------------------
            //  JobModification method
            //  ----------------------

            uint IBackgroundCopyCallback2.JobModification(IBackgroundCopyJob job, int reserved) => Job.OnModified(/*reserved*/);

            //  ----------------------
            //  FileTransferred method
            //  ----------------------

            uint IBackgroundCopyCallback2.FileTransferred(IBackgroundCopyJob job, IBackgroundCopyFile file) => Job.OnFileTransferred(file);

            #endregion IBackgroundCopyCallback2 implementation
        }

        #endregion Callback class
    }
}

// eof "BackgroundCopyJob.cs"
