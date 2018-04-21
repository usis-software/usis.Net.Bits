//
//  @(#) ChannelException.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.Serialization;

namespace usis.PushNotification
{
    //  ----------------------
    //  ChannelException class
    //  ----------------------

    /// <summary>
    /// The exception that is thrown when a channel operation fails.
    /// </summary>
    /// <seealso cref="Exception" />

    [Serializable]
    public class ChannelException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelException"/> class.
        /// </summary>

        public ChannelException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelException"/> class
        /// with the specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public ChannelException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelException"/> class
        /// with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>

        public ChannelException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>

        protected ChannelException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion construction
    }
}

// eof "ChannelException.cs"
