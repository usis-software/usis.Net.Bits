//
//  @(#) NSErrorException.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using System;
using System.Runtime.Serialization;
using usis.Platform;

namespace usis.Cocoa.Foundation
{
    //  -----------------------
    //  NSErrorException method
    //  -----------------------

    /// <summary>
    /// The exception that is thrown when a Cocoa method returns an <see cref="NSError"/>.
    /// </summary>
    /// <seealso cref="Exception" />

    [Serializable]
    public class NSErrorException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NSErrorException"/> class.
        /// </summary>

        public NSErrorException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSErrorException"/> class
        /// with the specified message.
        /// </summary>
        /// <param name="message">The message for the exception.</param>

        public NSErrorException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSErrorException"/> class
        /// with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>

        public NSErrorException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSErrorException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"></see>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see>
        /// that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> is a null reference.
        /// </exception>

        protected NSErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            info.AddValue(nameof(ErrorCode), ErrorCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NSErrorException"/> class
        /// with the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <exception cref="ArgumentNullException">
        /// <b>exception</b> is a null reference.
        /// </exception>

        public NSErrorException(NSError error) : base(error?.LocalizedDescription)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            ErrorCode = error.Code;
        }

        #endregion construction

        #region properties

        //  ------------------
        //  ErrorCode property
        //  ------------------

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>

        public nint ErrorCode { get; private set; }

        #endregion properties

        #region overrides

        //  --------------------
        //  GetObjectData method
        //  --------------------

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> is a null reference.
        /// </exception>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            ErrorCode = info.GetValue<nint>(nameof(ErrorCode));

            base.GetObjectData(info, context);
        }

        #endregion overrides
    }

    #region NSErrorExtension class

    //  ----------------------
    //  NSErrorExtensions class
    //  ----------------------

    /// <summary>
    /// Provides extension methods to the <see cref="NSError"/> class.
    /// </summary>

    public static class NSErrorExtensions
    {
        //  ----------------------
        //  CreateException method
        //  ----------------------

        /// <summary>
        /// Creates an <see cref="NSErrorException"/> for an error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        /// A newly created <c>NSErrorException</c> object.
        /// </returns>

        public static NSErrorException CreateException(this NSError error)
        {
            return new NSErrorException(error);
        }
    }

    #endregion NSErrorExtensions class
}

// eof "NSErrorException.cs"
