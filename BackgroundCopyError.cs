//
//  @(#) BackgroundCopyError.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2019
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017-2019 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using usis.Net.Bits.Interop;

namespace usis.Net.Bits
{
    //  -------------------------
    //  BackgroundCopyError class
    //  -------------------------

    /// <summary>
    /// Provides informations about the cause of an error and if the transfer
    /// process can proceed.
    /// </summary>

    public sealed class BackgroundCopyError : IDisposable
    {
        #region constants

        private const int FallbackLanguageCodeId = 0x0c00;

        #endregion constants

        #region fields

        private IBackgroundCopyError interop;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyError(BackgroundCopyManager manager, IBackgroundCopyError i)
        {
            Manager = manager;
            interop = i ?? throw new ArgumentNullException(nameof(i));
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        private bool disposed = false; // To detect redundant calls

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (!disposed)
            {
                if (interop != null) { _ = Marshal.ReleaseComObject(interop); interop = null; }
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        //  ---------
        //  finalizer
        //  ---------

        /// <summary>
        /// Finalizes an instance of the <see cref="BackgroundCopyError"/>
        /// class.
        /// </summary>

        ~BackgroundCopyError()
        {
            Dispose();
        }

        #endregion IDisposable implementation

        #region properties

        #region public properties

        //  ----------------
        //  Context property
        //  ----------------

        /// <summary>
        /// Gets the context in which the error occurred.
        /// </summary>
        /// <value>
        /// The context in which the error occurred.
        /// </value>

        public BackgroundCopyErrorContext Context => Manager.InvokeComMethod(() =>
        {
            Interface.GetError(out var context, out var code);
            return context;
        });

        //  -------------
        //  Code property
        //  -------------

        /// <summary>
        /// Gets the error code of the error that occurred.
        /// </summary>
        /// <value>
        /// The error code of the error that occurred.
        /// </value>

        public int Code => Manager.InvokeComMethod(() =>
        {
            Interface.GetError(out var context, out var code);
            return code;
        });

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets the error text associated with the error.
        /// </summary>
        /// <value>
        /// The error text associated with the error.
        /// </value>

        public string Description => GetErrorDescription(Thread.CurrentThread.CurrentCulture.LCID);

        //  ---------------------------
        //  ContextDescription property
        //  ---------------------------

        /// <summary>
        /// Gets a description of the context in which the error occurred.
        /// </summary>
        /// <value>
        /// A description of the context in which the error occurred.
        /// </value>

        public string ContextDescription => GetErrorContextDescription(Thread.CurrentThread.CurrentCulture.LCID);

        //  -----------------
        //  Protocol property
        //  -----------------

        /// <summary>
        /// Gets the protocol used to transfer the file.
        /// </summary>
        /// <value>
        /// The protocol used to transfer the file.
        /// </value>

        public string Protocol => Manager.InvokeComMethod(Interface.GetProtocol);

        #endregion public properties

        #region private properties

        //  ----------------
        //  Manager property
        //  ----------------

        private BackgroundCopyManager Manager { get; }

        //  ------------------
        //  Interface property
        //  ------------------

        private IBackgroundCopyError Interface => interop ?? throw new ObjectDisposedException(nameof(BackgroundCopyError));

        #endregion private properties

        #endregion properties

        #region methods

        //  -------------------
        //  RetrieveFile method
        //  -------------------

        /// <summary>
        /// Retrieves the file object associated with the error.
        /// </summary>
        /// <returns>The file object associated with the error.</returns>

        public BackgroundCopyFile RetrieveFile() => new BackgroundCopyFile(Manager, Manager.InvokeComMethod(Interface.GetFile));

        #endregion methods

        #region private methods

        //  --------------------------
        //  GetErrorDescription method
        //  --------------------------

        private string GetErrorDescription(int lcid)
        {
            string description = null;
            var result = Manager.InvokeComMethod(() => Interface.GetErrorDescription(lcid, out description));
            if (result == HResult.Ok) return description;
            else if (result == Win32Error.ERROR_MUI_FILE_NOT_LOADED || result == Win32Error.ERROR_MUI_FILE_NOT_FOUND)
            {
                return GetErrorDescription(FallbackLanguageCodeId);
            }
            else throw new BackgroundCopyException(Strings.FailedErrorDescription, result);
        }

        //  ---------------------------------
        //  GetErrorContextDescription method
        //  ---------------------------------

        private string GetErrorContextDescription(int lcid)
        {
            string description = null;
            var result = Manager.InvokeComMethod(() => Interface.GetErrorContextDescription(lcid, out description));
            if (result == HResult.Ok) return description;
            else if (result == Win32Error.ERROR_MUI_FILE_NOT_LOADED || result == Win32Error.ERROR_MUI_FILE_NOT_FOUND)
            {
                return GetErrorContextDescription(FallbackLanguageCodeId);
            }
            else throw new BackgroundCopyException(Strings.FailedErrorDescription, result);
        }

        #endregion private methods
    }

    #region BackgroundCopyErrorInfo class

    //  -----------------------------
    //  BackgroundCopyErrorInfo class
    //  -----------------------------

    /// <summary>
    /// Provides informations about an error.
    /// </summary>

    public sealed class BackgroundCopyErrorInfo
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyErrorInfo(BackgroundCopyError error)
        {
            Code = error.Code;
            Description = error.Description;
        }

        #endregion construction

        //  -------------
        //  Code property
        //  -------------

        /// <summary>
        /// Gets the error code of the error that occurred.
        /// </summary>
        /// <value>
        /// The error code of the error that occurred.
        /// </value>

        public int Code { get; }

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets the error text associated with the error.
        /// </summary>
        /// <value>
        /// The error text associated with the error.
        /// </value>

        public string Description { get; }
    }

    #endregion BackgroundCopyErrorInfo class

    #region BackgroundCopyErrorEventArgs class

    //  ----------------------------------
    //  BackgroundCopyErrorEventArgs class
    //  ----------------------------------

    /// <summary>
    /// Provides arguments for the <see cref="BackgroundCopyJob.Failed"/> event.
    /// event.
    /// </summary>
    /// <seealso cref="EventArgs" />

    public class BackgroundCopyErrorEventArgs : EventArgs
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BackgroundCopyErrorEventArgs(BackgroundCopyError error) => Error = error;

        #endregion construction

        #region properties

        //  --------------
        //  Error property
        //  --------------

        /// <summary>
        /// Gets the error that caused the
        /// <see cref="BackgroundCopyJob.Failed"/> event.
        /// </summary>
        /// <value>
        /// The error that caused the <see cref="BackgroundCopyJob.Failed"/>
        /// event.
        /// </value>

        public BackgroundCopyError Error { get; }

        #endregion properties
    }

    #endregion BackgroundCopyErrorEventArgs class
}

// eof "BackgroundCopyError.cs"
