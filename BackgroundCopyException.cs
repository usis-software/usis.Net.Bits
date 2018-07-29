//
//  @(#) BackgroundCopyException.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace usis.Net.Bits
{
    //  -----------------------------
    //  BackgroundCopyException class
    //  -----------------------------

    /// <summary>
    /// The exception that is thrown when a BITS operation fails.
    /// </summary>

    [Serializable]
    public class BackgroundCopyException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyException" /> class.
        /// </summary>

        public BackgroundCopyException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyException"/> class
        /// with the specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>

        public BackgroundCopyException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyException"/> class,
        /// with the specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception.
        /// </param>

        public BackgroundCopyException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCopyException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"></see>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see>
        /// that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="SerializationException">
        /// The class name is a <c>null</c> reference or <see cref="Exception.HResult"></see> is zero (0).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> is a <c>null</c> reference.
        /// </exception>

        protected BackgroundCopyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        internal BackgroundCopyException(BackgroundCopyManager manager, COMException inner) : base(manager.GetErrorDescription(inner), inner) { }

        internal BackgroundCopyException(BackgroundCopyManager manager, uint hResult) : base(manager.GetErrorDescription(hResult)) => HResult = (int)hResult;

        internal BackgroundCopyException(string message, uint hResult) : base(message) => HResult = (int)hResult;

        #endregion construction
    }
}

// eof "BackgroundCopyJobException.cs"
