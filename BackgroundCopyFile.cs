//
//  @(#) BackgroundCopyFile.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2022
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2022 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  -------------------------
    //  BackgroundCopyFile method
    //  -------------------------

    /// <summary>
    /// Contains information about a file that is part of a job.
    /// </summary>

    public sealed class BackgroundCopyFile : IDisposable
    {
        #region fields

        private IBackgroundCopyFile interop;

        #endregion

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyFile(BackgroundCopyManager manager, IBackgroundCopyFile i)
        {
            Manager = manager;
            interop = i ?? throw new ArgumentNullException(nameof(i));
        }

        #endregion

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            Release();
            GC.SuppressFinalize(this);
        }

        //  --------------
        //  Release method
        //  --------------

        private void Release()
        {
            if (interop != null) { _ = Marshal.ReleaseComObject(interop); interop = null; }
        }

        //  ----------
        //  finalizing
        //  ----------

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyFile"/> class.
        /// </summary>

        ~BackgroundCopyFile() { Release(); }

        #endregion

        #region properties

        #region public properties

        //  ------------------
        //  LocalName property
        //  ------------------

        /// <summary>
        /// Gets the local name of the file.
        /// </summary>
        /// <value>
        /// The local name of the file.
        /// </value>

        public string LocalName => Manager.InvokeComMethod(Interface.GetLocalName);

        //  -------------------
        //  RemoteName property
        //  -------------------

        /// <summary>
        /// Gets the remote name of the file.
        /// </summary>
        /// <value>
        /// The remote name of the file.
        /// </value>

        public string RemoteName
        {
            get => Manager.InvokeComMethod(Interface.GetRemoteName);
            set => Manager.InvokeComMethod(() => Interface2.SetRemoteName(value));
        }

        //  ----------------------
        //  TemporaryName property
        //  ----------------------

        /// <summary>
        /// Gets the full path of the temporary file that contains the content
        /// of the download.
        /// </summary>
        /// <value>
        /// The full path of the temporary file that contains the content of the
        /// download.
        /// </value>

        public string TemporaryName => Manager.InvokeComMethod(Interface3.GetTemporaryName);

        //  ------------------------
        //  ValidationState property
        //  ------------------------

        /// <summary>
        /// Gets or sets the current validation state of this file.
        /// </summary>
        /// <value>
        /// <c>true</c> if the contents of the file is valid; otherwise,
        /// <c>false</c>.
        /// </value>

        public bool ValidationState
        {
            get => Manager.InvokeComMethod(Interface3.GetValidationState);
            set => Manager.InvokeComMethod(() => Interface3.SetValidationState(value));
        }

        //  -----------------------------
        //  IsDownloadedFromPeer property
        //  -----------------------------

        /// <summary>
        /// Gets a value that determines if any part of the file was downloaded
        /// from a peer.
        /// </summary>
        /// <value>
        /// <c>true</c> if any part of the file is downloaded from peer;
        /// otherwise, <c>false</c>.
        /// </value>

        public bool IsDownloadedFromPeer => Manager.InvokeComMethod(Interface3.IsDownloadedFromPeer);

        #endregion

        #region private properties

        //  ----------------
        //  Manager property
        //  ----------------

        private BackgroundCopyManager Manager { get; }

        //  ------------------
        //  Interface property
        //  ------------------

        private IBackgroundCopyFile Interface => interop ?? throw new ObjectDisposedException(nameof(BackgroundCopyFile));

        //  -------------------
        //  Interface2 property
        //  -------------------

        private IBackgroundCopyFile2 Interface2 => Extensions.QueryInterface<IBackgroundCopyFile2>(Interface);

        //  -------------------
        //  Interface3 property
        //  -------------------

        private IBackgroundCopyFile3 Interface3 => Extensions.QueryInterface<IBackgroundCopyFile3>(Interface);

        #endregion

        #endregion

        #region methods

        //  -----------------------
        //  RetrieveProgress method
        //  -----------------------

        /// <summary>
        /// Retrieves information on the progress of the file transfer.
        /// </summary>
        /// <returns>A <c>BackgroundCopyFileProgress</c> object whose members
        ///     indicate the progress of the file transfer.</returns>

        public BackgroundCopyFileProgress RetrieveProgress() => new(Manager.InvokeComMethod(Interface.GetProgress));

        //  ---------------------
        //  RetrieveRanges method
        //  ---------------------

        /// <summary>
        /// Retrieves the ranges that you want to download from the remote file.
        /// </summary>
        /// <returns>An enumerator to iterate the
        ///     <see cref="BackgroundCopyFileRange"/> objects that specify the
        ///     ranges to download.</returns>

        public IEnumerable<BackgroundCopyFileRange> RetrieveRanges()
        {
            BG_FILE_RANGE[] ranges = null;
            Manager.InvokeComMethod(() => Interface2.GetFileRanges(out var count, out ranges));
            foreach (var item in ranges)
            {
                yield return new BackgroundCopyFileRange(item);
            }
        }

        #endregion
    }

    #region BackgroundCopyFileEventArgs class

    //  ---------------------------------
    //  BackgroundCopyFileEventArgs class
    //  ---------------------------------

    /// <summary>
    /// Provides arguments for the
    /// <see cref="BackgroundCopyJob.FileTransferred"/> event.
    /// </summary>
    /// <seealso cref="EventArgs"/>

    public class BackgroundCopyFileEventArgs : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyFileEventArgs(BackgroundCopyFile file) => File = file;

        #endregion

        #region properties

        //  -------------
        //  File property
        //  -------------

        /// <summary>
        /// Gets an object that provides informations about the file.
        /// </summary>
        /// <value>
        /// An object that provides informations about the file.
        /// </value>

        public BackgroundCopyFile File { get; }

        #endregion
    }

    #endregion
}

// eof "BackgroundCopyFile.cs"
