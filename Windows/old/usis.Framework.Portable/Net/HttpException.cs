//
//  @(#) HttpException.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Globalization;

namespace usis.Framework.Portable.Net
{
    //  -------------------
    //  HttpException class
    //  -------------------

    /// <summary>
    /// Represents an error that occurred during a HTTP request.
    /// </summary>
    /// <seealso cref="Exception" />

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

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class
        /// with the specified status code and text.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusText">The status text.</param>

        public HttpException(int statusCode, string statusText) :
            base(string.Format(CultureInfo.CurrentCulture, Strings.HttpError, statusCode, statusText))
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
    }
}

// eof "HttpException.cs"
