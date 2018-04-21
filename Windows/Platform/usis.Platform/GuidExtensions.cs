//
//  @(#) GuidExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform
{
    //  --------------------
    //  GuidExtensions class
    //  --------------------

    /// <summary>
    /// Provides extension methods to the <see cref="Guid"/> type.
    /// </summary>

    public static class GuidExtensions
    {
        //  --------------
        //  IsEmpty method
        //  --------------

        /// <summary>
        /// Determines whether this instance is an empty GUID.
        /// </summary>
        /// <param name="value">The unique identifier.</param>
        /// <returns>
        /// <c>true</c> when the GUID is an empty GUID, otherwise <c>false</c>.
        /// </returns>

        public static bool IsEmpty(this Guid value) { return value == Guid.Empty; }

        //  ------------------
        //  NullIfEmpty method
        //  ------------------

        /// <summary>
        /// Returns a <c>null</c> value when this instance is an empty GUID.
        /// </summary>
        /// <param name="value">The unique identifier.</param>
        /// <returns>
        /// <c>true</c> when the GUID is an empty GUID, otherwise the GUID's value.
        /// </returns>

        public static Guid? NullIfEmpty(this Guid value) { return value.IsEmpty() ? null : (Guid?)value; }
    }
}

// eof "GuidExtensions.cs"
