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

        private IBackgroundCopyFile file;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyFile(IBackgroundCopyFile file) => this.file = file;

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

        public string LocalName => File.GetLocalName();

        //  -------------------
        //  RemoteName property
        //  -------------------

        /// <summary>
        /// Gets the remote name of the file.
        /// </summary>
        /// <value>
        /// The remote name of the file.
        /// </value>

        public string RemoteName => File.GetRemoteName();

        //  -------------
        //  File property
        //  -------------

        private IBackgroundCopyFile File => file ?? throw new ObjectDisposedException(nameof(BackgroundCopyFile));

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

        public BackgroundCopyFileProgress RetrieveProgress() => new BackgroundCopyFileProgress(File.GetProgress());

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        private bool disposedValue = false; // to detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                if (file != null) { Marshal.ReleaseComObject(file); file = null; }
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyFile"/> class.
        /// </summary>

        ~BackgroundCopyFile()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion IDisposable implementation
    }
}

// eof "BackgroundCopyFile.cs"
