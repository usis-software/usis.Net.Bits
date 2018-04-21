//
//  @(#) Exceptions.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using usis.FinTS.Base;

namespace usis.FinTS
{
    //  -----------------------------
    //  InvalidMessageException class
    //  -----------------------------

    /// <summary>
    /// Represents errors that occur during message receiption.
    /// </summary>
    /// <seealso cref="Exception" />

    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class InvalidMessageException : Exception
    {
        //  -----------------
        //  Segments property
        //  -----------------

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal IEnumerable<Segment> Segments { get; }

        internal InvalidMessageException(string message, IEnumerable<Segment> segments) : base(message)
        {
            Segments = segments;
        }

        #region overrides

        //  --------------------
        //  GetObjectData method
        //  --------------------

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion overrides
    }
}

// eof "Exceptions.cs"
