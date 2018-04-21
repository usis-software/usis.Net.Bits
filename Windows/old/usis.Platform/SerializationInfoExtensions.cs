//
//  @(#) SerializationInfoExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.Serialization;

namespace usis.Platform
{
    //  ---------------------------------
    //  SerializationInfoExtensions class
    //  ---------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="SerializationInfo"/> class.
    /// </summary>

    public static class SerializationInfoExtensions
    {
        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves a value with a specified type from the <see cref="SerializationInfo"/> store.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="info">The <b>SerializationInfo</b> store.</param>
        /// <param name="name">The name of the value.</param>
        /// <returns>The object of the specified type associated with <i>name</i>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <b>info</b> is a null reference (<b>Nothing</b> is Visual Basic).
        /// </exception>

        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            return (T)info.GetValue(name, typeof(T));
        }
    }
}

// eof "SerializationInfoExtensions.cs"
