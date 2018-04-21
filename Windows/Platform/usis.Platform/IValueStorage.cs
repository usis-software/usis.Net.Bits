//
//  @(#) IValueStorage.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace usis.Platform
{
    //  -----------------------
    //  IValueStorage interface
    //  -----------------------

    /// <summary>
    /// Defines the properties and methods that have to be implemented
    /// by a class to provide a storage for named values.
    /// </summary>

    public interface IValueStorage
    {
        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value storage.
        /// </summary>
        /// <value>
        /// The name of the value storage.
        /// </value>

        string Name { get; }

        //  -------------------
        //  ValueNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all value names in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all value names in the storage.
        /// </value>

        IEnumerable<string> ValueNames { get; }

        //  ---------------
        //  Values property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all named values in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all named values in the storage.
        /// </value>

        IEnumerable<INamedValue> Values { get; }

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>
        /// A type that implements <see cref="INamedValue"/> and represents the specified value,
        /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the value does not exist.
        /// </returns>

        INamedValue GetValue(string name);

        //  ---------------
        //  SetValue method
        //  ---------------

        /// <summary>
        /// Saves the specified named value in the storage.
        /// </summary>
        /// <param name="value">
        /// The named value to save.
        /// </param>

        void SetValue(INamedValue value);

        //  ------------------
        //  DeleteValue method
        //  ------------------

        /// <summary>
        /// Deletes the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to delete.</param>
        /// <returns>
        /// <c>true</c> when value was deleted
        /// or
        /// <c>false</c> when a value with the specified name does not exist.
        /// </returns>

        bool DeleteValue(string name);
    }

    #region ValueStorage class

    //  ----------------
    //  ValueStore class
    //  ----------------

    /// <summary>
    /// Implements the interface <see cref="IValueStorage"/> as a value storage
    /// that keeps all values in memory.
    /// </summary>

    [DataContract]
    public class ValueStorage : IValueStorage
    {
        #region fields

        private Dictionary<string, INamedValue> values = new Dictionary<string, INamedValue>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region IValueStorage members

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value storage.
        /// </summary>
        /// <value>
        /// The name of the value storage.
        /// </value>

        [DataMember]
        public string Name { get; set; }

        //  -------------------
        //  ValueNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all value names in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all value names in the storage.
        /// </value>

        public IEnumerable<string> ValueNames { get { foreach (var name in values.Keys) yield return name; } }

        //  ---------------
        //  Values property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all named values in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all named values in the storage.
        /// </value>

        public IEnumerable<INamedValue> Values { get { foreach (var item in values.Values) yield return item; } }

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>
        /// A type that implements <see cref="INamedValue"/> and represents the specified value,
        /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the value does not exist.
        /// </returns>

        public INamedValue GetValue(string name)
        {
            if (values.TryGetValue(name, out INamedValue namedValue))
            {
                return namedValue;
            }
            else return null;
        }

        //  ---------------
        //  SetValue method
        //  ---------------

        /// <summary>
        /// Saves the specified named value in the storage.
        /// </summary>
        /// <param name="value">The named value to save.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public void SetValue(INamedValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var name = value.Name;
            values[name] = value;
        }

        //  ------------------
        //  DeleteValue method
        //  ------------------

        /// <summary>
        /// Deletes the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to delete.</param>
        /// <returns>
        /// <c>true</c> when value was deleted
        /// or
        /// <c>false</c> when a value with the specified name does not exist.
        /// </returns>

        public bool DeleteValue(string name)
        {
            return values.Remove(name);
        }

        #endregion IValueStorage members
    }

    #endregion ValueStorage class

    #region ValueStorageInterfaceExtensions class

    //  -------------------------------------
    //  ValueStorageInterfaceExtensions class
    //  -------------------------------------

    /// <summary>
    /// Provides extension to class that implement the
    /// <see cref="IValueStorage"/> interface.
    /// </summary>

    public static class ValueStorageInterfaceExtensions
    {
        //  -------------
        //  Get<T> method
        //  -------------

        /// <summary>
        /// Gets value with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="storage">The storage to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <returns>
        /// The value with the specified name as type T.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static T Get<T>(this IValueStorage storage, string name) { return storage.Get(name, default(T)); }

        /// <summary>
        /// Gets value with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="storage">The storage to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value with the specified name as type T.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static T Get<T>(this IValueStorage storage, string name, T defaultValue)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            var namedValue = storage.GetValue(name);
            if (namedValue == null) return defaultValue;
            return namedValue.Get(defaultValue);
        }

        //  --------------
        //  GetByte method
        //  --------------

        /// <summary>
        /// Gets the value with the specified name as a <see cref="byte" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="defaultValue">The default value to return when the specified value does not exist.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value with the specified name as <see cref="byte" /> or <paramref name="defaultValue"/> if the value does not exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static byte GetByte(this IValueStorage storage, string name, byte defaultValue, IFormatProvider provider)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            var namedValue = storage.GetValue(name);
            if (namedValue == null) return defaultValue;
            return namedValue.GetByte(provider);
        }

        /// <summary>
        /// Gets the value with the specified name as a <see cref="byte" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value with the specified name as <see cref="byte" /> or <c>0</c> if the value does not exist.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).</exception>

        public static byte GetByte(this IValueStorage storage, string name, IFormatProvider provider)
        {
            return storage.GetByte(name, 0, provider);
        }

        //  ---------------
        //  GetInt32 method
        //  ---------------

        /// <summary>
        /// Gets the value with the specified name as a <see cref="int" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="defaultValue">The default value to return when the specified value does not exist.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value with the specified name as <see cref="int" /> or <paramref name="defaultValue"/> if the value does not exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static int GetInt32(this IValueStorage storage, string name, int defaultValue, IFormatProvider provider)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            var namedValue = storage.GetValue(name);
            if (namedValue?.Value == null) return defaultValue;
            return namedValue.GetInt32(provider);
        }

        /// <summary>
        /// Gets the value with the specified name as a <see cref="int" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value with the specified name as <see cref="int" /> or <c>0</c> if the value does not exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static int GetInt32(this IValueStorage storage, string name, IFormatProvider provider)
        {
            return storage.GetInt32(name, 0, provider);
        }

        //  ---------------
        //  GetInt64 method
        //  ---------------

        /// <summary>
        /// Gets the value with the specified name as a <see cref="long" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="defaultValue">The default value to return when the specified value does not exist.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value with the specified name as <see cref="long" /> or <paramref name="defaultValue"/> if the value does not exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static long GetInt64(this IValueStorage storage, string name, long defaultValue, IFormatProvider provider)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            var namedValue = storage.GetValue(name);
            if (namedValue == null) return defaultValue;
            return namedValue.GetInt64(provider);
        }

        /// <summary>
        /// Gets the value with the specified name as a <see cref="long" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>
        /// The value with the specified name as <see cref="long" /> or <c>0</c> if the value does not exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static long GetInt64(this IValueStorage storage, string name, IFormatProvider provider)
        {
            return storage.GetInt64(name, 0, provider);
        }

        //  ----------------
        //  GetString method
        //  ----------------

        /// <summary>
        /// Gets the value with the specified name as a <see cref="string" />.
        /// </summary>
        /// <param name="storage">The store to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <returns>
        /// The value with the specified name.
        /// If a value with the specified name does not exists an empty string is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static string GetString(this IValueStorage storage, string name)
        {
            return storage.Get(name, string.Empty);
        }

        //  ---------------
        //  SetValue method
        //  ---------------

        /// <summary>
        /// Sets the value with the specified name.
        /// </summary>
        /// <param name="storage">The store.</param>
        /// <param name="name">The name of the value to set.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storage"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static void SetValue(this IValueStorage storage, string name, object value)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            
            // TODO: get an existing value with the name
            storage.SetValue(new NamedValue(name, value));
        }
    }

    #endregion ValueStorageInterfaceExtensions class
}

// eof "IValueStorage.cs"
