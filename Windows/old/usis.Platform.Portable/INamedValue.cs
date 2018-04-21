//
//  @(#) INamedValue.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace usis.Platform.Portable
{
    //  ---------------------
    //  INamedValue interface
    //  ---------------------

    /// <summary>
    /// Represents a value with an associated name.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public interface INamedValue
    {
        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value.
        /// </summary>
        /// <value>
        /// The name of the value.
        /// </value>

        string Name { get; }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>

        object Value { get; }
    }

    #region NamedValue class

    //  ----------------
    //  NamedValue class
    //  ----------------

    /// <summary>
    /// Implements a value with an associated name.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    [DataContract]
    public class NamedValue : INamedValue
    {
        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedValue"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>

        public NamedValue(string name, object value)
        {
            Name = name;
            Value = value;
        }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value.
        /// </summary>
        /// <value>
        /// The name of the value.
        /// </value>

        [DataMember]
        public string Name
        {
            get; private set;
        }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>

        [DataMember]
        public object Value
        {
            get; private set;
        }

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Name = \"{0}\"; Value = '{1}'", Name, Value);
        }
    }

    #endregion NamedValue class

    #region NamedValueInterfaceExtensions class

    //  -----------------------------------
    //  NamedValueInterfaceExtensions class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods to types that implement the <see cref="INamedValue"/> interface.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public static class NamedValueInterfaceExtensions
    {
        //  -------------
        //  Get<T> method
        //  -------------

        /// <summary>
        /// Gets the value of the <see cref="INamedValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="namedValue">The named value to get the value from.</param>
        /// <returns>The value of the specified named value or the type's default value,
        /// if there is a type mismatch.</returns>
        /// <exception cref="ArgumentNullException">
        /// <i>namedValue</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static T Get<T>(this INamedValue namedValue)
        {
            return namedValue.Get(default(T));
        }

        /// <summary>
        /// Gets the value of the <see cref="INamedValue" />.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="namedValue">The named value to get the value from.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value of the specified named value or the default value,
        /// if there is a type mismatch.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>namedValue</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public static T Get<T>(this INamedValue namedValue, T defaultValue)
        {
            if (namedValue == null) throw new ArgumentNullException(nameof(namedValue));
            return namedValue.Value is T ? (T)namedValue.Value : defaultValue;
        }

        //  --------------
        //  GetByte method
        //  --------------

        /// <summary>
        /// Gets the value as a <see cref="byte" />.
        /// </summary>
        /// <param name="namedValue">The named value to get the value from.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value as a <see cref="byte" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><i>namedValue</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        public static byte GetByte(this INamedValue namedValue, IFormatProvider provider)
        {
            if (namedValue == null) throw new ArgumentNullException(nameof(namedValue));
            return Convert.ToByte(namedValue.Value, provider);
        }

        //  ---------------
        //  GetInt16 method
        //  ---------------

        /// <summary>
        /// Gets the value as a <see cref="short" />.
        /// </summary>
        /// <param name="namedValue">The named value to get the value from.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value as a <see cref="short" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><i>namedValue</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        public static short GetInt16(this INamedValue namedValue, IFormatProvider provider)
        {
            if (namedValue == null) throw new ArgumentNullException(nameof(namedValue));
            return Convert.ToInt16(namedValue.Value, provider);
        }

        //  ---------------
        //  GetInt32 method
        //  ---------------

        /// <summary>
        /// Gets the value as a <see cref="int" />.
        /// </summary>
        /// <param name="namedValue">The named value to get the value from.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value as a <see cref="int" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><i>namedValue</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        public static int GetInt32(this INamedValue namedValue, IFormatProvider provider)
        {
            if (namedValue == null) throw new ArgumentNullException(nameof(namedValue));
            return Convert.ToInt32(namedValue.Value, provider);
        }

        //  ---------------
        //  GetInt64 method
        //  ---------------

        /// <summary>
        /// Gets the value as a <see cref="long" />.
        /// </summary>
        /// <param name="namedValue">The named value to get the value from.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value as a <see cref="long" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><i>namedValue</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        public static long GetInt64(this INamedValue namedValue, IFormatProvider provider)
        {
            if (namedValue == null) throw new ArgumentNullException(nameof(namedValue));
            return Convert.ToInt64(namedValue.Value, provider);
        }
    }

    #endregion NamedValueInterfaceExtensions class
}

// eof "INamedValue.cs"
