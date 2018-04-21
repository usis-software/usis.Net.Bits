//
//  @(#) HttpException.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;
using usis.Framework.Resources;
using usis.Platform;

namespace usis.Framework.Net
{
    //  -------------------
    //  HttpException class
    //  -------------------

    /// <summary>
    /// Represents an error that occurred during a HTTP request.
    /// </summary>
    /// <seealso cref="Exception" />

#if !WINDOWS_UWP
    [Serializable]
#endif
    public class HttpException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class.
        /// </summary>

        public HttpException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class
        /// with the specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public HttpException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class
        /// with the specified message an inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>

        public HttpException(string message, Exception innerException) : base(message, innerException) { }

#if !WINDOWS_UWP

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class.
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
        /// The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The info parameter is null. 
        /// </exception>

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context) { }

#endif // !WINDOWS_UWP

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class
        /// with the specified status code and text.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusText">The status text.</param>

        public HttpException(int statusCode, string statusText) : base(string.Format(CultureInfo.CurrentCulture, Strings.HttpError, statusCode, statusText))
        {
            StatusCode = statusCode;
        }

        #endregion construction

        #region properties

        //  -------------------
        //  StatusCode property
        //  -------------------

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code.
        /// </value>

        public int StatusCode { get; private set; }

        #endregion properties

#if !WINDOWS_UWP

        //  --------------------
        //  GetObjectData method
        //  --------------------

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            StatusCode = info.GetValue<int>(nameof(StatusCode));

            base.GetObjectData(info, context);
        }

#endif // !WINDOWS_UWP

    }
}

// eof "HttpException.cs"
