//
//  @(#) BackgroundCopyFile.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
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

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyFile(IBackgroundCopyFile i) => interop = i;

        #endregion construction

        #region properties

        //  ------------------
        //  LocalName property
        //  ------------------

        /// <summary>
        /// Gets the local name of the file.
        /// </summary>
        /// <value>
        /// The local name of the file.
        /// </value>

        public string LocalName => Interface.GetLocalName();

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
            get => Interface.GetRemoteName();
            set => Interface2.SetRemoteName(value);
        }

        //  ------------------
        //  Interface property
        //  ------------------

        private IBackgroundCopyFile Interface => interop ?? throw new ObjectDisposedException(nameof(BackgroundCopyFile));

        //  -------------------
        //  Interface2 property
        //  -------------------

        private IBackgroundCopyFile2 Interface2 => GetInterface<IBackgroundCopyFile2>();

        #endregion properties

        #region methods

        //  -----------------------
        //  RetrieveProgress method
        //  -----------------------

        /// <summary>
        /// Retrieves information on the progress of the file transfer.
        /// </summary>
        /// <returns>
        /// A <c>BackgroundCopyFileProgress</c> object whose members indicate the progress of the file transfer.
        /// </returns>

        public BackgroundCopyFileProgress RetrieveProgress() => new BackgroundCopyFileProgress(Interface.GetProgress());

        #region private methods

        //  -------------------
        //  GetInterface method
        //  -------------------

        private TInterface GetInterface<TInterface>() where TInterface : class => (Interface as TInterface) ?? throw new NotSupportedException(Strings.NotSupported);

        #endregion private methods

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
            if (interop != null) { Marshal.ReleaseComObject(interop); interop = null; }
        }

        //  ----------
        //  finalizing
        //  ----------

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyFile"/> class.
        /// </summary>

        ~BackgroundCopyFile() { Release(); }

        #endregion IDisposable implementation
    }
}

// eof "BackgroundCopyFile.cs"
