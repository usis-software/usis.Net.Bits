//
//  @(#) IHierarchicalValueStorage.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform
{
    //  -----------------------------------
    //  IHierarchicalValueStorage interface
    //  -----------------------------------

    /// <summary>
    /// Defines the properties and methods that have to be implemented
    /// by a class to provide a hierarchically organized storage for named values.
    /// </summary>

    public interface IHierarchicalValueStorage : IValueStorage
    {
        #region properties

        //  ---------------------
        //  StorageNames property
        //  ---------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storage names in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub storage names in the store.
        /// </value>

        IEnumerable<string> StorageNames { get; }

        //  ---------------
        //  Stores property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storages in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub storages in the storage.
        /// </value>

        IEnumerable<IHierarchicalValueStorage> Storages { get; }

        #endregion properties

        #region storage methods

        //  --------------------
        //  CreateStorage method
        //  --------------------

        /// <summary>
        /// Creates a new storage or opens an existing storage with the specified access.
        /// </summary>
        /// <param name="name">
        /// The name of the storage to create or open. This string is not case-sensitive.
        /// </param>
        /// <param name="writable">
        /// <c>true</c> to indicate that the new storage is writable; otherwise, <c>false</c>.
        /// </param>
        /// <returns>
        /// The newly created storage, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the operation failed.
        /// If a zero-length string is specified for <paramref name="name"/>,
        /// the current storage is returned.
        /// </returns>

        IHierarchicalValueStorage CreateStorage(string name, bool writable);

        //  ------------------------
        //  EnumerateStorages method
        //  ------------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storages in the storage,
        /// and specifies whether write access is to be applied to the storages. 
        /// </summary>
        /// <param name="writable">
        /// Set to <c>true</c> if you need write access to the storages. 
        /// </param>
        /// <returns>
        /// An enumerator to iterate all sub storages in the storage.
        /// </returns>

        IEnumerable<IHierarchicalValueStorage> EnumerateStorages(bool writable);

        //  ------------------
        //  OpenStorage method
        //  ------------------

        /// <summary>
        /// Retrieves the storage with the specified name,
        /// and specifies whether write access is to be applied to the storage. 
        /// </summary>
        /// <param name="name">
        /// The name of the storage to retrieve.
        /// </param>
        /// <param name="writable">
        /// Set to <c>true</c> if you need write access to the storage. 
        /// </param>
        /// <returns>
        /// The storage requested, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the operation failed.
        /// </returns>

        IHierarchicalValueStorage OpenStorage(string name, bool writable);

        //  --------------------
        //  DeleteStorage method
        //  --------------------

        /// <summary>
        /// Deletes the storage with the specified name.
        /// </summary>
        /// <param name="name">The name of the storage to delete.</param>

        void DeleteStorage(string name);

        #endregion storage methods
    }

    #region HierarchicalValueStorage class

    //  ------------------------------
    //  HierarchicalValueStorage class
    //  ------------------------------

    /// <summary>
    /// Provides a hierarchically organized storage for named values.
    /// </summary>
    /// <seealso cref="ValueStorage" />
    /// <seealso cref="IHierarchicalValueStorage" />

    public class HierarchicalValueStorage : ValueStorage, IHierarchicalValueStorage
    {
        #region fields

        private Dictionary<string, HierarchicalValueStorage> storages = new Dictionary<string, HierarchicalValueStorage>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region properties

        //  ---------------------
        //  StorageNames property
        //  ---------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storage names in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub storage names in the storage.
        /// </value>

        public IEnumerable<string> StorageNames
        {
            get { foreach (var name in storages.Keys) yield return name; }
        }

        //  -----------------
        //  Storages property
        //  -----------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storages in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub storages in the storage.
        /// </value>

        public IEnumerable<IHierarchicalValueStorage> Storages
        {
            get { foreach (var storage in storages.Values) yield return storage; }
        }

        #endregion properties

        #region methods

        //  --------------------
        //  CreateStorage method
        //  --------------------

        /// <summary>
        /// Creates a new storage or opens an existing storage with the specified access.
        /// </summary>
        /// <param name="name">The name of the storage to create or open. This string is not case-sensitive.</param>
        /// <param name="writable"><c>true</c> to indicate taht the new storage is writable; otherwise, <c>false</c>.</param>
        /// <returns>
        /// The newly created storage, or <c>null</c> if the operation failed.
        /// If a zero-length string is specified for <paramref name="name"/>,
        /// the current storage is returned.
        /// </returns>

        public IHierarchicalValueStorage CreateStorage(string name, bool writable)
        {
            var storage = OpenStorage(name, writable);
            if (storage == null)
            {
                var hStorage = new HierarchicalValueStorage() { Name = name };
                storages.Add(hStorage.Name, hStorage);
                storage = hStorage;
            }
            return storage;
        }

        //  ------------------------
        //  EnumerateStorages method
        //  ------------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storages in the storage,
        /// and specifies whether write access is to be applied to the storage.
        /// </summary>
        /// <param name="writable">Set to <c>true</c> if you need write access to the storages.</param>
        /// <returns>
        /// An enumerator to iterate all sub storages in the storage.
        /// </returns>

        public IEnumerable<IHierarchicalValueStorage> EnumerateStorages(bool writable) { return storages.Values; }

        //  ------------------
        //  OpenStorage method
        //  ------------------

        /// <summary>
        /// Retrieves the storage with the specified name,
        /// and specifies whether write access is to be applied to the storage.
        /// </summary>
        /// <param name="name">The name of the storage to retrieve.</param>
        /// <param name="writable">Set to <c>true</c> if you need write access to the storage.</param>
        /// <returns>
        /// The storage requested, or <c>null</c> if the operation failed.
        /// </returns>

        public IHierarchicalValueStorage OpenStorage(string name, bool writable)
        {
            if (string.IsNullOrEmpty(name)) return this;
            storages.TryGetValue(name, out HierarchicalValueStorage storage);
            return storage;
        }

        //  --------------------
        //  DeleteStorage method
        //  --------------------

        /// <summary>
        /// Deletes the storage with the specified name.
        /// </summary>
        /// <param name="name">The name of the storage to delete.</param>

        public void DeleteStorage(string name) { storages.Remove(name); }

        #endregion methods
    }

    #endregion HierarchicalValueStorage class

    #region HierarchicalValueStorageInterfaceExtensions class

    //  -----------------------------------------------
    //  HierarchicalValueStoreInterfaceExtensions class
    //  -----------------------------------------------

    /// <summary>
    /// Provides extension methods to classes that implement the
    /// <see cref="IHierarchicalValueStorage"/> interface.
    /// </summary>

    public static class HierarchicalValueStorageInterfaceExtensions
    {
        #region public methods

        //  ------------------
        //  OpenStorage method
        //  ------------------

        /// <summary>
        /// Retrieves the storage with the specified name,
        /// and specifies whether write access is to be applied to the storage.
        /// </summary>
        /// <param name="storage">The parent storage.</param>
        /// <param name="name">The name of the storage to retrieve.</param>
        /// <returns>
        /// The storage requested, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the operation failed.
        /// </returns>

        public static IHierarchicalValueStorage OpenStorage(this IHierarchicalValueStorage storage, string name)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            return storage.OpenStorage(name, false);
        }

        //  ---------------------
        //  OpenSubStorage method
        //  ---------------------

        /// <summary>
        /// Opens the storage with the specified path read-only.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The storage requested, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the operation failed.
        /// </returns>

        public static IHierarchicalValueStorage OpenSubStorage(this IHierarchicalValueStorage storage, string path)
        {
            return storage.GetSubStorage(path, false, false);
        }

        /// <summary>
        /// Retrieves the storage from the specified path,
        /// and specifies whether write access is to be applied to the storage.
        /// </summary>
        /// <param name="storage">The parent storage.</param>
        /// <param name="path">
        /// The path of the storage to open relative to the parent storage.
        /// </param>
        /// <param name="writable">
        /// Set to <c>true</c> if you need write access to the storage.
        /// </param>
        /// <returns>
        /// The storage requested, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the operation failed.
        /// </returns>

        public static IHierarchicalValueStorage OpenSubStorage(this IHierarchicalValueStorage storage, string path, bool writable)
        {
            return storage.GetSubStorage(path, false, writable);
        }

        //  -----------------------
        //  CreateSubStorage method
        //  -----------------------

        /// <summary>
        /// Creates a new storage or opens an existing storage at the specified path.
        /// </summary>
        /// <param name="storage">The parent storage.</param>
        /// <param name="path">
        /// The path of the storage to create or open relative to the parent storage.
        /// </param>
        /// <param name="writable">
        /// Set to <c>true</c> if you need write access to the storage.
        /// </param>
        /// <returns>
        /// The storage requested, or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the operation failed.
        /// </returns>

        public static IHierarchicalValueStorage CreateSubStorage(this IHierarchicalValueStorage storage, string path, bool writable)
        {
            return storage.GetSubStorage(path, true, writable);
        }

        //  -----------------------------------
        //  EnumerateValuesInSubStorages method
        //  -----------------------------------

        /// <summary>
        /// Enumerates all values in the storage and all sub storages.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <returns>
        /// An enumerator to iterate recursively thru all values.
        /// </returns>

        public static IEnumerable<StorageValuePair> EnumerateValuesInSubStorages(this IHierarchicalValueStorage storage)
        {
            foreach (var namedValue in storage.Values)
            {
                yield return new StorageValuePair(storage, namedValue);
            }
            foreach (var subStorage in storage.Storages)
            {
                foreach (var namedValue in subStorage.EnumerateValuesInSubStorages()) yield return namedValue;
            }
        }

        #endregion public methods

        #region private methods

        //  --------------------
        //  GetSubStorage method
        //  --------------------

        private static IHierarchicalValueStorage GetSubStorage(
            this IHierarchicalValueStorage storage,
            string path,
            bool create,
            bool writable)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            var resultStorage = storage;
            foreach (var name in path.Split('\\'))
            {
                resultStorage = create ? resultStorage.CreateStorage(name, writable) : resultStorage.OpenStorage(name, writable);
                if (resultStorage == null) break;
            }
            return resultStorage;
        }

        #endregion private methods
    }

    #endregion HierarchicalValueStorageInterfaceExtensions class

    #region StorageValuePair class

    //  ----------------------
    //  StorageValuePair class
    //  ----------------------

    /// <summary>
    /// Provides a tuple to hold a value storage and a named value reference.
    /// </summary>

    public class StorageValuePair
    {
        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageValuePair"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="value">The named value.</param>

        public StorageValuePair(IValueStorage storage, INamedValue value) { Storage = storage; Value = value; }

        //  ----------------
        //  Storage property
        //  ----------------

        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>

        public IValueStorage Storage { get; }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets the named value.
        /// </summary>
        /// <value>
        /// The named value.
        /// </value>

        public INamedValue Value { get; }
    }

    #endregion StorageValuePair class
}

// eof "IHierarchicalValueStorage.cs"
