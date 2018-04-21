//
//  @(#) BackgroundCopyJob.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

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

        private IBackgroundCopyJob job;
        private Callback callback;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyJob(BackgroundCopyManager manager, IBackgroundCopyJob job)
        {
            Manager = manager;
            this.job = job ?? throw new ArgumentNullException(nameof(job));
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
            if (job != null)
            {
                // free unmanaged resources
                if (callback != null)
                {
                    var hr = job.SetNotifyInterface(null);
                    if (HResult.Succeeded(hr) || hr == HResult.RPC_E_DISCONNECTED) callback = null;
                }
                Marshal.ReleaseComObject(job);
                job = null;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyJob"/> class.
        /// </summary>

        ~BackgroundCopyJob()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose();
        }

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

        public Guid Id => Manager.InvokeComMethod(() => Job.GetId());

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
            get => Manager.InvokeComMethod(() => Job.GetDisplayName());
            set => Manager.InvokeComMethod(() => Job.SetDisplayName(value));
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
            get => Manager.InvokeComMethod(() => Job.GetDescription());
            set => Manager.InvokeComMethod(() => Job.SetDescription(value));
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

        public BackgroundCopyJobType JobType => Manager.InvokeComMethod(() => Job.GetType());

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
            get => Manager.InvokeComMethod(() => Job.GetPriority());
            set => Manager.InvokeComMethod(() => Job.SetPriority(value));
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

        public BackgroundCopyJobState State => Manager.InvokeComMethod(Job.GetState);

        //  --------------
        //  Owner property
        //  --------------

        /// <summary>
        /// Gets the identity of the job's owner.
        /// </summary>
        /// <value>
        /// The identity of the job's owner.
        /// </value>

        public string Owner => Manager.InvokeComMethod(Job.GetOwner);

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
            get => Manager.InvokeComMethod(() => Job.GetMinimumRetryDelay());
            set => Manager.InvokeComMethod(() => Job.SetMinimumRetryDelay(value));
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
            get => Manager.InvokeComMethod(() => Job.GetNoProgressTimeout());
            set => Manager.InvokeComMethod(() => Job.SetNoProgressTimeout(value));
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
            get => Manager.InvokeComMethod(() => GetNotifyFlags());
            private set => Manager.InvokeComMethod(() => Job.SetNotifyFlags(value));
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

        public int ErrorCount => Manager.InvokeComMethod(() => Job.GetErrorCount());

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

        //  ------------------------
        //  NotifyCommandLine method
        //  ------------------------

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
                Job2.GetNotifyCmdLine(out string program, out string parameters);
                return new BackgroundCopyNotifyCommandLine(program, parameters);
            });
            set => Manager.InvokeComMethod(() =>
            {
                if (value == null) Job2.SetNotifyCmdLine(null, null);
                else Job2.SetNotifyCmdLine(value.Program, value.Parameters);
            });
        }

        //  --------------------
        //  ReplyFileName method
        //  --------------------

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
                var hr = Job2.GetReplyFileName(out string replyFileName);
                return hr == HResult.Ok ? replyFileName : null;
            }
            set => Manager.InvokeComMethod(() => Job2.SetReplyFileName(value));
        }

        #endregion public properties

        #region private properties

        //  ----------------
        //  Manager property
        //  ----------------

        private BackgroundCopyManager Manager { get; set; }

        //  ------------
        //  Job property
        //  ------------

        private IBackgroundCopyJob Job
        {
            get
            {
                if (job == null) throw new ObjectDisposedException(nameof(BackgroundCopyJob));
                return job;
            }
        }

        //  -------------
        //  Job2 property
        //  -------------

        private IBackgroundCopyJob2 Job2 => GetJob<IBackgroundCopyJob2>();

        #endregion private properties

        #endregion properties

        #region events

        #region Failed event

        //  ------------
        //  Failed event
        //  ------------

        private EventHandler failedHandler;

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

        public event EventHandler Failed
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

        private uint OnFailed() { return InvokeHandler(failedHandler); }

        #endregion Failed event

        #region Modified event

        //  --------------
        //  Modified event
        //  --------------

        private EventHandler modifiedHandler;

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

        public event EventHandler Modified
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

        private uint OnModified() { return InvokeHandler(modifiedHandler); }

        #endregion Modified event

        #region Transferred event

        //  -----------------
        //  Transferred event
        //  -----------------

        private EventHandler transferredHandler;

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

        public event EventHandler Transferred
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

        private uint OnTransferred() { return InvokeHandler(transferredHandler); }

        #endregion Transferred event

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

        public void Cancel() { Manager.InvokeComMethod(Job.Cancel); }

        //  ---------------
        //  Complete method
        //  ---------------

        /// <summary>
        /// Ends the job and saves the transferred files on the client.
        /// </summary>

        public void Complete() { Manager.InvokeComMethod(Job.Complete); }

        //  --------------
        //  Suspend method
        //  --------------

        /// <summary>
        /// Suspends a job. New jobs, jobs that are in error,
        /// and jobs that have finished transferring files are automatically suspended.
        /// </summary>

        public void Suspend() { Manager.InvokeComMethod(Job.Suspend); }

        //  -------------
        //  Resume method
        //  -------------

        /// <summary>
        /// Activates a new job or restarts a job that has been suspended.
        /// </summary>

        public void Resume() { Manager.InvokeComMethod(Job.Resume); }

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
            if (job == null) throw new ObjectDisposedException(nameof(BackgroundCopyJob));
            IEnumBackgroundCopyFiles files = null;
            try
            {
                files = job.EnumFiles();
                while (files.Next(1, out IBackgroundCopyFile file, IntPtr.Zero) == HResult.Ok)
                {
                    try { yield return new BackgroundCopyFile(file); }
                    finally { Marshal.ReleaseComObject(file); }
                }
            }
            finally { if (files != null) Marshal.ReleaseComObject(files); }
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

        public BackgroundCopyJobProgress RetrieveProgress()
        {
            return new BackgroundCopyJobProgress(Job.GetProgress());
        }

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

        public BackgroundCopyJobReplyProgress RetrieveReplyProgress()
        {
            return new BackgroundCopyJobReplyProgress(Manager.InvokeComMethod(() => Job2.GetReplyProgress()));
        }

        //  --------------------
        //  RetrieveTimes method
        //  --------------------

        /// <summary>
        /// Retrieves job-related time stamps, such as the time that the job was created or last modified.
        /// </summary>
        /// <returns>
        /// A <c>BackgroundCopyJobTimes</c> structure that contains job-related time stamps.
        /// </returns>

        public BackgroundCopyJobTimes RetrieveTimes()
        {
            return new BackgroundCopyJobTimes(Job.GetTimes());
        }

        //  --------------
        //  AddFile method
        //  --------------

        /// <summary>
        /// Adds a single file to the job.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>

        public void AddFile(string remoteUrl, string localName)
        {
            AddFile(new Uri(remoteUrl), localName);
        }

        /// <summary>
        /// Adds a single file to the job.
        /// </summary>
        /// <param name="remoteUrl">The URL of the file on the server.</param>
        /// <param name="localName">The name of the file on the client.</param>

        public void AddFile(Uri remoteUrl, string localName)
        {
            Manager.InvokeComMethod(() => Job.AddFile(remoteUrl.ToString(), localName));
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

        public void AddFiles(params BackgroundCopyFileInfo[] files)
        {
            Manager.InvokeComMethod(() => Job.AddFileSet(files.Length, files.Select(e => e.fileInfo).ToArray()));
        }

        //  --------------------
        //  TakeOwnership method
        //  --------------------

        /// <summary>
        /// Changes ownership of the job to the current user.
        /// </summary>

        public void TakeOwnership() { Manager.InvokeComMethod(() => Job.TakeOwnership()); }

        //  --------------------
        //  RetrieveError method
        //  --------------------

        /// <summary>
        /// Retrieves error informations after an error occurs.
        /// </summary>
        /// <returns>
        /// An object that provides error informations.
        /// </returns>

        public BackgroundCopyError RetrieveError() { return RetrieveError(false); }

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
            var hr = Job2.GetReplyData(out IntPtr buffer, out ulong lenght);
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

        public void SetCredentials(
            BackgroundCopyAuthenticationTarget target,
            BackgroundCopyAuthenticationScheme scheme,
            string userName, string password)
        {
            Manager.InvokeComMethod(() => Job2.SetCredentials(new BG_AUTH_CREDENTIALS()
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

        public void RemoveCredentials(BackgroundCopyAuthenticationTarget target, BackgroundCopyAuthenticationScheme scheme)
        {
            Manager.InvokeComMethod(() => Job2.RemoveCredentials(target, scheme));
        }

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
            Job.GetProxySettings(out BackgroundCopyJobProxyUsage usage, out string list, out string bypassList);
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
            Manager.InvokeComMethod(() => Job.SetProxySettings(settings.ProxyUsage, settings.ProxyList, settings.ProxyBypassList));
        }

        //  --------------------
        //  RetrieveError method
        //  --------------------

        internal BackgroundCopyError RetrieveError(bool throwException)
        {
            var hr = Job.GetError(out IBackgroundCopyError error);
            if (hr == HResult.Ok) return new BackgroundCopyError(error);
            else if (hr == HResult.BG_E_ERROR_INFORMATION_UNAVAILABLE && !throwException) return null;
            else throw new BackgroundCopyException(Manager, hr);
        }

        //  ------------------
        //  RemoveEvent method
        //  ------------------

        private void RemoveEvent(EventHandler handler, BackgroundCopyJobNotifications flags, EventHandler value)
        {
            handler -= value;
            if (handler == null)
            {
                var hr = Job.GetNotifyFlags(out BackgroundCopyJobNotifications notifyFlags);
                if (hr != HResult.RPC_E_DISCONNECTED &&
                    hr != Win32Error.RPC_S_SERVER_UNAVAILABLE)
                {
                    if (HResult.Succeeded(hr)) Notifications = notifyFlags & ~flags;
                    else throw new BackgroundCopyException(Manager, hr);
                }
            }
        }

        //  -------------
        //  GetJob method
        //  -------------

        private TInterface GetJob<TInterface>() where TInterface : class
        {
            var job2 = Job as TInterface;
            if (job2 == null) throw new NotSupportedException();
            return job2;
        }

        //  ---------------------
        //  GetNotifyFlags method
        //  ---------------------

        private BackgroundCopyJobNotifications GetNotifyFlags()
        {
            var hr = Job.GetNotifyFlags(out BackgroundCopyJobNotifications notifyFlags);
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
                var hr = job.SetNotifyInterface(callback);
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
        private uint InvokeHandler(EventHandler handler)
        {
            try
            {
                handler?.Invoke(this, EventArgs.Empty);
                if (Job is IBackgroundCopyJob2 job2)
                {
                    job2.GetNotifyCmdLine(out string program, out string parameters);
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

        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0}: Id={1}, Name='{2}', Notifications={3}",
                nameof(BackgroundCopyJob), Id, DisplayName, Notifications);
        }

        #endregion overrides

        #region Callback class

        //  --------------
        //  Callback class
        //  --------------

        [ComVisible(true)]
        private class Callback : IBackgroundCopyCallback
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

            internal Callback(BackgroundCopyJob job) { Job = job; }

            #endregion construction

            #region IBackgroundCopyCallback implementation

            //  ---------------------
            //  JobTransferred method
            //  ---------------------

            uint IBackgroundCopyCallback.JobTransferred(IBackgroundCopyJob job) { return Job.OnTransferred(); }

            //  ---------------
            //  JobError method
            //  ---------------

            uint IBackgroundCopyCallback.JobError(IBackgroundCopyJob job, IBackgroundCopyError error) { return Job.OnFailed(); }

            //  ----------------------
            //  JobModification method
            //  ----------------------

            uint IBackgroundCopyCallback.JobModification(IBackgroundCopyJob job, int reserved) { return Job.OnModified(); }

            #endregion IBackgroundCopyCallback implementation
        }

        #endregion Callback class
    }
}

// eof "BackgroundCopyJob.cs"
