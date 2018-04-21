//
//  @(#) RegistryKeyExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using Microsoft.Win32;
using System;
using System.Globalization;

namespace usis.Platform.Windows
{
    //  --------------------------
    //  RegistryKeyExtension class
    //  --------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="RegistryKey"/> class.
    /// </summary>

    public static class RegistryKeyExtensions
    {
        #region GetInt32 method

        //  ---------------
        //  GetInt32 method
        //  ---------------

        /// <summary>
        /// Retrieves the 32-bit integer value associated with the specified name.
        /// Returns the specified default value if the name/value pair does not exist in the registry.
        /// </summary>
        /// <param name="key">
        /// The registry key from which the value should retrieved.
        /// </param>
        /// <param name="name">
        /// The name of the value to retrieve. This string is not case-sensitive.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The integer value associated with name, or the default value if name is not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <i>key</i> argument cannot be null.
        /// </exception>

        public static int GetInt32(this RegistryKey key, string name, int defaultValue)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Convert.ToInt32(key.GetValue(name, defaultValue), CultureInfo.InvariantCulture);
        }

        #endregion GetInt32 method

        #region GetDouble method

        //  ----------------
        //  GetDouble method
        //  ----------------

        /// <summary>
        /// Retrieves the double value associated with the specified name.
        /// Returns the specified default value if the name/value pair does not exist in the registry.
        /// </summary>
        /// <param name="key">
        /// The registry key from which the value should retrieved.
        /// </param>
        /// <param name="name">
        /// The name of the value to retrieve. This string is not case-sensitive.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The double value associated with name, or the default value if name is not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <i>key</i> argument cannot be null.
        /// </exception>

        public static double GetDouble(this RegistryKey key, string name, double defaultValue)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Convert.ToDouble(key.GetValue(name, defaultValue), CultureInfo.InvariantCulture);
        }

        #endregion GetDouble method

        #region GetEnum<TEnum> method

        //  ---------------------
        //  GetEnum<TEnum> method
        //  ---------------------

        /// <summary>
        /// Retrieves the enumeration value associated with the specified name.
        /// Returns the specified default value if the name/value pair does not exist in the registry.
        /// </summary>
        /// <typeparam name="TEnum">
        /// The type of the enumeration.
        /// </typeparam>
        /// <param name="key">
        /// The registry key from which the value should retrieved.
        /// </param>
        /// <param name="name">
        /// The name of the value to retrieve. This string is not case-sensitive.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The enumeration value associated with name, or the default value if name is not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <i>key</i> argument cannot be null.
        /// </exception>

        public static TEnum GetEnum<TEnum>(this RegistryKey key, string name, TEnum defaultValue) where TEnum : struct
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            TEnum value = defaultValue;
            if (Enum.TryParse(key.GetValue(name, defaultValue).ToString(), out value))
            {
                return value;
            }
            return defaultValue;
        }

        #endregion GetEnum<TEnum> method
    }
}

// eof "RegistryKeyExtensions.cs"
