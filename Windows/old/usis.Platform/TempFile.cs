//
//  @(#) TempFile.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.IO;

namespace usis.Platform
{
    //  --------------
    //  TempFile class
    //  --------------

    /// <summary>
    /// Provides access to the <see cref="FileStream"/> of a temporary
    /// file on the disk.
    /// </summary>
    /// <remarks>
    /// The <see cref="Path.GetTempFileName"/> method is used to
    /// create the temporary file on disk.
    /// The temporary file is deleted when the <b>TempFile</b> object
    /// is closed or disposed.
    /// </remarks>

    [Obsolete("TODO: Refactor a new TempFile class.")]
    public sealed class TempFile : IDisposable
    {
        #region private fields

        private string path = Path.GetTempFileName();
        private FileStream stream;

        #endregion

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>

        public TempFile()
        {
            stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.None);
        }

        //  -----------
        //  destruction
        //  -----------

        /// <summary>
        /// This member overrides <see cref="object.Finalize" />.
        /// </summary>

        ~TempFile()
        {
            Dispose();
        }

        #endregion construction/destruction

        #region IDisposable members

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            CloseStream();
            DeleteFile();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable members

        #region public properties

        //  -----------------
        //  FileName property
        //  -----------------

        /// <summary>
        /// Gets the full path of the temporary file.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The <b>TempFile</b> object is disposed.
        /// </exception>

        public string FileName
        {
            get
            {
                CheckDisposed(); return path;
            }
        }

        //  ---------------
        //  Stream property
        //  ---------------

        /// <summary>
        /// Gets the <b>FileStream</b> object to access the temporary file.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The <b>TempFile</b> object is disposed.
        /// </exception>

        public FileStream Stream
        {
            get
            {
                CheckDisposed(); return stream;
            }
        }

        #endregion public properties

        #region public methods

        //  ------------
        //  Close method
        //  ------------

        /// <summary>
        /// Closes the temporary file.
        /// </summary>

        public void Close()
        {
            Dispose();
        }

        //  -------------
        //  Detach method
        //  -------------

        /// <summary>
        /// Detaches the encapsulated temporary file from this instance.
        /// </summary>
        /// <returns>
        /// The path and name of the temp. file.
        /// </returns>

        public string Detach()
        {
            CheckDisposed();
            CloseStream();
            string s = path;
            path = null;
            return s;
        }

        #endregion public methods

        #region private methods

        //  ---------------------
        //  CheckDisposed methods
        //  ---------------------

        private void CheckDisposed()
        {
            if (path == null || stream == null)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        //  ------------------
        //  CloseStream method
        //  ------------------

        private void CloseStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        //  -----------------
        //  DeleteFile method
        //  -----------------

        private void DeleteFile()
        {
            if (path != null)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                path = null;
            }
        }

        #endregion private methods
    }
}

// eof "TempFile.cs"
