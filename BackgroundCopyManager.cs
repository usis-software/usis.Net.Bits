//
//  @(#) BackgroundCopyManager.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2022
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2023 usis GmbH. All rights reserved.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  ---------------------------
    //  BackgroundCopyManager class
    //  ---------------------------

    /// <summary>
    /// Provides methods to create transfer jobs,
    /// to enumerate the jobs in the queue, and to retrieve individual jobs from the queue.
    /// </summary>
    /// <seealso cref="IDisposable" />

    public sealed class BackgroundCopyManager : IDisposable
    {
        #region fields

        private IBackgroundCopyManager? manager;
        private Version? version;

        #endregion

        #region construction

        //  ------------
        //  construction
        //  ------------

        private BackgroundCopyManager() => manager = CreateComObject<IBackgroundCopyManager>(new Guid(CLSID.BackgroundCopyManager));

        #endregion

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (manager != null)
            {
                // free unmanaged resources
                _ = Marshal.ReleaseComObject(manager);
                manager = null;
            }
            GC.SuppressFinalize(this);
        }

        //  ---------
        //  finalizer
        //  ---------

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyManager"/> class.
        /// </summary>

        ~BackgroundCopyManager() { Dispose(); }

        #endregion

        #region properties

        //  ----------------
        //  Version property
        //  ----------------

        /// <summary>
        /// Gets the version of BITS on the client computer.
        /// </summary>
        /// <value>
        /// The version of BITS on the client computer.
        /// </value>

        public Version? Version => version ??= DetermineVersion();

        //  -----------------------
        //  LibraryVersion property
        //  -----------------------

        /// <summary>
        /// Gets the file version of the Windows <c>QMgr.dll</c> file.
        /// </summary>
        /// <value>
        /// The QMgr.dll file version number.
        /// </value>

        public static Version? LibraryVersion => DetermineQMgrVersion();

        #endregion

        #region methods

        //  --------------
        //  Connect method
        //  --------------

        /// <summary>
        /// Connects to the BITS service and returns an instance of the <see cref="BackgroundCopyManager" /> class.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="BackgroundCopyManager" /> class.
        /// </returns>
        /// <exception cref="COMException">The connection attempt to the BITS service failed.</exception>
        /// <remarks>
        /// The returned <see cref="BackgroundCopyManager"/> object implements the <see cref="IDisposable"/> interface.
        /// Therefore you have to call <see cref="IDisposable.Dispose"/> to disconnect from the BITS service and to free all resources.
        /// </remarks>

        public static BackgroundCopyManager Connect() => new();

        //  ----------------
        //  CreateJob method
        //  ----------------

        /// <summary>
        /// Creates a transfer job.
        /// </summary>
        /// <param name="displayName">A string that contains a display name for the job.
        /// Typically, the display name is used to identify the job in a user interface.
        /// Note that more than one job may have the same display name.
        /// Must not be <c>null</c>. The name is limited to 256 characters.</param>
        /// <param name="type">The type of transfer job.</param>
        /// <returns>
        /// An instance of the <see cref="BackgroundCopyJob" /> class
        /// that represent the newly created job.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="displayName" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        /// <exception cref="ObjectDisposedException">The method was called after the object was disposed.</exception>
        /// <exception cref="BackgroundCopyException">An error occured during a BITS method call.</exception>

        public BackgroundCopyJob CreateJob(string displayName, BackgroundCopyJobType type)
        {
            return displayName == null
                ? throw new ArgumentNullException(nameof(displayName))
                : manager == null
                    ? throw new ObjectDisposedException(nameof(BackgroundCopyManager))
                    : InvokeComMethod(() => new BackgroundCopyJob(this, manager.CreateJob(displayName, type, out var jobId)));
        }

        //  --------------------
        //  EnumerateJobs method
        //  --------------------

        /// <summary>
        /// Enumerates the jobs in the transfer queue.
        /// The order of the jobs in the enumerator is arbitrary.
        /// </summary>
        /// <param name="forAllUsers">
        /// A value that indicates whether to include all jobs in the transfer queue
        /// - those owned by the user and those owned by others.
        /// The user must be an administrator when this flag is <c>true</c>.
        /// </param>
        /// <returns>
        /// An enumerator to iterate through the list of
        /// <see cref="BackgroundCopyJob" /> objects that represent the jobs in the queue.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The method was called after the object was disposed.</exception>
        /// <remarks>
        /// <para>
        /// The enumerator provides a snapshot of the jobs in the transfer queue
        /// at the time you call the <c>EnumerateJobs</c> method.
        /// If you retrieve property values from a job in the list, however,
        /// the values reflect the current property values of the job.
        /// </para>
        /// <para>
        /// Each <see cref="BackgroundCopyJob"/> object that is returned by the enumerator holds a reference to a COM interface
        /// and implements the <see cref="IDisposable"/> intrface. Therefore you should call <see cref="IDisposable.Dispose"/>
        /// when your are finished with it.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code language="cs">
        /// using System;
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
        ///                 Console.WriteLine("BITS Version {0}", manager.Version);
        ///                 Console.WriteLine();
        ///                 
        ///                 // list all BITS jobs of the current user
        ///                 foreach (var job in manager.EnumerateJobs(false))
        ///                 {
        ///                     Console.WriteLine("{0} '{1}' {2}", job.Id.ToString("B"), job.DisplayName, job.State.ToString());
        ///                     job.Dispose();
        ///                 }
        ///             }
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>

        public IEnumerable<BackgroundCopyJob> EnumerateJobs(bool forAllUsers)
        {
            if (manager == null) throw new ObjectDisposedException(nameof(BackgroundCopyManager));
            IEnumBackgroundCopyJobs? jobs = null;
            try
            {
                jobs = manager.EnumJobs(forAllUsers ? Interop.Constants.BG_JOB_ENUM_ALL_USERS : 0);
                while (jobs.Next(1, out var job, IntPtr.Zero) == HResult.Ok)
                {
                    yield return new BackgroundCopyJob(this, job);
                }
            }
            finally { if (jobs != null) _ = Marshal.ReleaseComObject(jobs); }
        }

        /// <summary>
        /// Enumerates the jobs in the transfer queue for the current user.
        /// </summary>
        /// <returns>
        /// An enumerator to iterate through the list of
        /// <see cref="BackgroundCopyJob" /> objects that represent the jobs in the queue.
        /// </returns>
        /// <seealso cref="EnumerateJobs(bool)"/>

        public IEnumerable<BackgroundCopyJob> EnumerateJobs() => EnumerateJobs(false);

        //  -------------
        //  GetJob method
        //  -------------

        /// <summary>
        /// Retrieves a given job from the transfer queue.
        /// </summary>
        /// <param name="jobId">A <see cref="Guid" /> that identifies the job to retrieve from the transfer queue.</param>
        /// <param name="throwNotFoundException">If set to <c>true</c> an exception is thrown when the job was not found.</param>
        /// <returns>
        /// A <see cref="BackgroundCopyJob" /> object that represents the job specified by <paramref name="jobId" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The method was called after the object was disposed.</exception>
        /// <exception cref="BackgroundCopyException">An error occured during a BITS method call.</exception>
        /// <remarks>
        /// Typically, your application persists the job identifier,
        /// so you can later retrieve the job from the queue.
        /// </remarks>

        public BackgroundCopyJob? GetJob(Guid jobId, bool throwNotFoundException)
        {
            if (manager == null) throw new ObjectDisposedException(nameof(BackgroundCopyManager));

            var result = manager.GetJob(jobId, out var job);
            return result == HResult.Ok
                ? new BackgroundCopyJob(this, job)
                : result == HResult.BG_E_NOT_FOUND && !throwNotFoundException
                    ? null
                    : throw new BackgroundCopyException(this, result);
        }

        /// <summary>
        /// Retrieves a given job from the transfer queue.
        /// </summary>
        /// <param name="jobId">A <see cref="Guid" /> that identifies the job to retrieve from the transfer queue.</param>
        /// <returns>
        /// A <see cref="BackgroundCopyJob" /> object that represents the job specified by <paramref name="jobId" />.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The method was called after the object was disposed.</exception>
        /// <exception cref="BackgroundCopyException">An error occured during a BITS method call.</exception>

        public BackgroundCopyJob? GetJob(Guid jobId) => GetJob(jobId, true);

        //  --------------------------
        //  GetErrorDescription method
        //  --------------------------

        internal string GetErrorDescription(COMException exception) => (exception.ErrorCode & 0xFFFF0000) == 0x80200000 ? GetErrorDescription((uint)exception.ErrorCode) : exception.Message;

        internal string GetErrorDescription(uint hResult) => GetErrorDescription(hResult, Thread.CurrentThread.CurrentCulture.LCID);

        //  ----------------------
        //  InvokeComMethod method
        //  ----------------------

        internal T InvokeComMethod<T>(Func<T> method)
        {
            try { return method(); }
            catch (COMException exception) { throw new BackgroundCopyException(this, exception); }
        }

        internal void InvokeComMethod(Action method)
        {
            try { method(); }
            catch (COMException exception) { throw new BackgroundCopyException(this, exception); }
        }

        #endregion

        #region private methods

        //  --------------------------
        //  GetErrorDescription method
        //  --------------------------

        private string GetErrorDescription(uint hResult, int languageId)
        {
            if (manager == null) throw new ObjectDisposedException(nameof(BackgroundCopyManager));
            var result = manager.GetErrorDescription(hResult, languageId, out var description);
            return result == HResult.Ok
                ? description
                : result == Win32Error.ERROR_MUI_FILE_NOT_LOADED
                    ? GetErrorDescription(hResult, 0x0C00)
                    : throw new BackgroundCopyException(Strings.FailedErrorDescription, result);
        }

        //  ---------------------------
        //  DetermineQMgrVersion method
        //  ---------------------------

        private static Version? DetermineQMgrVersion()
        {
            const string QMgrFileName = "QMgr.dll";
            var path = Path.Combine(Environment.SystemDirectory, QMgrFileName);
            if (File.Exists(path))
            {
                var info = FileVersionInfo.GetVersionInfo(path);
                return new Version(info.FileMajorPart, info.FileMinorPart);
            }
            return null;
        }

        //  -----------------------
        //  DetermineVersion method
        //  -----------------------

        private static Version? DetermineVersion()
        {
            var dictionary = new SortedDictionary<Version, Guid>
            {
                [new Version(10, 1)] = new Guid(CLSID.BackgroundCopyManager10_1),
                [new Version(10, 0)] = new Guid(CLSID.BackgroundCopyManager10_0),
                [new Version(5, 0)] = new Guid(CLSID.BackgroundCopyManager5_0),
                [new Version(4, 0)] = new Guid(CLSID.BackgroundCopyManager4_0),
                [new Version(3, 0)] = new Guid(CLSID.BackgroundCopyManager3_0),
                [new Version(2, 5)] = new Guid(CLSID.BackgroundCopyManager2_5),
                [new Version(2, 0)] = new Guid(CLSID.BackgroundCopyManager2_0),
                [new Version(1, 5)] = new Guid(CLSID.BackgroundCopyManager1_5),
                [new Version(1, 0)] = new Guid(CLSID.BackgroundCopyManager),
            };
            return dictionary.Keys.Reverse().FirstOrDefault(v => IsCLSIDRegistered(dictionary[v]));
        }

        //  ------------------------
        //  IsCLSIDRegistered method
        //  ------------------------

        private static bool IsCLSIDRegistered(Guid clsid)
        {
            var name = string.Format(CultureInfo.InvariantCulture, @"CLSID\{0}", clsid.ToString("B", CultureInfo.InvariantCulture));
            using var registryKey = Registry.ClassesRoot.OpenSubKey(name);
            return registryKey != null;
        }

        //  ----------------------
        //  CreateComObject method
        //  ----------------------

        private static TInterface CreateComObject<TInterface>(Guid clsid) where TInterface : class
        {
            var o = CreateComObject(clsid);
            if (o is not TInterface i)
            {
                if (o is not null) _ = Marshal.FinalReleaseComObject(o);
                throw new InvalidCastException();
            }
            else return i;
        }

        private static object? CreateComObject(Guid clsid) => CreateComObject(Type.GetTypeFromCLSID(clsid));

        private static object? CreateComObject(Type? type)
        {
            object? @object = null;
            try
            {
                if (type is not null) @object = Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                if (@object != null) _ = Marshal.FinalReleaseComObject(@object);
                throw;
            }
            return @object;
        }

        #endregion
    }
}

// eof "BackgroundCopyManager.cs"
