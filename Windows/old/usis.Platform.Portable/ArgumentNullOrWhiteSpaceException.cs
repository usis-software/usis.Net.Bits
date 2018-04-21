//
//  @(#) ArgumentNullOrWhiteSpaceException.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using usis.Platform.Portable.Resources;

namespace usis.Platform.Portable
{
    //  ---------------------------------------
    //  ArgumentNullOrWhiteSpaceException class
    //  ---------------------------------------

    /// <summary>
    /// The exception that is thrown when the value of an argument
    /// is <b>null</b>, empty, or consists only of white-space characters.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public class ArgumentNullOrWhiteSpaceException : ArgumentException
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ArgumentNullOrWhiteSpaceException"/> class.
        /// </summary>

        public ArgumentNullOrWhiteSpaceException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNullOrWhiteSpaceException"/> class
        /// with the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="parameterName">
        /// The name of the parameter that caused the current exception.
        /// </param>

        public ArgumentNullOrWhiteSpaceException(string parameterName) : base(ExceptionMessage.ArgumentCannotBeNullOrWhiteSpace, parameterName) { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ArgumentNullOrWhiteSpaceException"/> class
        /// with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception,
        /// or a null reference (<b>Nothing</b> in Visual Basic)
        /// if no inner exception is specified.
        /// </param>

        public ArgumentNullOrWhiteSpaceException(string message, Exception inner) : base(message, inner) { }

        #endregion construction
    }
}

// eof "ArgumentNullOrWhiteSpaceException.cs"
