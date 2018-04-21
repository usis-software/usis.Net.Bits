//
//  @(#) IHierarchicalValueStore.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform.Portable
{
    //  ---------------------------------
    //  IHierarchicalValueStore interface
    //  ---------------------------------

    /// <summary>
    /// Defines the properties and methods that have to be implemented
    /// by a class to provide a hierarchically organized store for named values.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public interface IHierarchicalValueStore : IValueStore
    {
        #region properties

        //  -------------------
        //  StoreNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub store names in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub store names in the store.
        /// </value>

        IEnumerable<string> StoreNames { get; }

        //  ---------------
        //  Stores property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all sub stores in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub stores in the store.
        /// </value>

        IEnumerable<IHierarchicalValueStore> Stores { get; }

        #endregion properties

        #region store methods

        //  ----------------
        //  OpenStore method
        //  ----------------

        /// <summary>
        /// Retrieves the store with the specified name,
        /// and specifies whether write access is to be applied to the store. 
        /// </summary>
        /// <param name="name">
        /// The name of the store to retrieve.
        /// </param>
        /// <param name="writable">
        /// Set to <b>true</b> if you need write access to the store. 
        /// </param>
        /// <returns>
        /// The store requested, or <b>null</b> if the operation failed.
        /// </returns>

        IHierarchicalValueStore OpenStore(string name, bool writable);

        /// <summary>
        /// Creates a new store or opens an existing store with the specified access.
        /// </summary>
        /// <param name="name">
        /// The name of the store to create or open. This string is not case-sensitive.
        /// </param>
        /// <param name="writable">
        /// <b>true</b> to indicate the new subkey is writable; otherwise, <b>false</b>.
        /// </param>
        /// <returns>
        /// The newly created store, or <b>null</b> if the operation failed.
        /// If a zero-length string is specified for <i>name</i>,
        /// the current store object is returned.
        /// </returns>

        IHierarchicalValueStore CreateStore(string name, bool writable);

        //  ----------------------
        //  EnumerateStores method
        //  ----------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub stores in the store,
        /// and specifies whether write access is to be applied to the stores. 
        /// </summary>
        /// <param name="writable">
        /// Set to <b>true</b> if you need write access to the stores. 
        /// </param>
        /// <returns>
        /// An enumerator to iterate all sub stores in the store.
        /// </returns>

        IEnumerable<IHierarchicalValueStore> EnumerateStores(bool writable);

        //  ------------------
        //  DeleteStore method
        //  ------------------

        /// <summary>
        /// Deletes the store with the specified name.
        /// </summary>
        /// <param name="name">The name of the store to delete.</param>

        void DeleteStore(string name);

        #endregion store methods
    }

    #region HierarchicalValueStore class

    //  ----------------------------
    //  HierarchicalValueStore class
    //  ----------------------------

    /// <summary>
    /// a hierarchically organized store for named values.
    /// </summary>
    /// <seealso cref="ValueStore" />
    /// <seealso cref="IHierarchicalValueStore" />

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public class HierarchicalValueStore : ValueStore, IHierarchicalValueStore
    {
        #region fields

        private Dictionary<string, HierarchicalValueStore> stores = new Dictionary<string, HierarchicalValueStore>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region properties

        //  -------------------
        //  StoreNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub store names in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub store names in the store.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>

        public IEnumerable<string> StoreNames
        {
            get { foreach (var name in stores.Keys) yield return name; }
        }

        //  ---------------
        //  Stores property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all sub stores in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub stores in the store.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>

        public IEnumerable<IHierarchicalValueStore> Stores
        {
            get { foreach (var store in stores.Values) yield return store; }
        }

        #endregion properties

        #region methods

        //  ------------------
        //  CreateStore method
        //  ------------------

        /// <summary>
        /// Creates a new store or opens an existing store with the specified access.
        /// </summary>
        /// <param name="name">The name of the store to create or open. This string is not case-sensitive.</param>
        /// <param name="writable"><b>true</b> to indicate the new subkey is writable; otherwise, <b>false</b>.</param>
        /// <returns>
        /// The newly created store, or <b>null</b> if the operation failed.
        /// If a zero-length string is specified for <i>name</i>,
        /// the current store object is returned.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>

        public IHierarchicalValueStore CreateStore(string name, bool writable)
        {
            var store = OpenStore(name, writable);
            if (store == null)
            {
                var hStore = new HierarchicalValueStore() { Name = name };
                stores.Add(hStore.Name, hStore);
                store = hStore;
            }
            return store;
        }

        //  ----------------------
        //  EnumerateStores method
        //  ----------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub stores in the store,
        /// and specifies whether write access is to be applied to the stores.
        /// </summary>
        /// <param name="writable">Set to <b>true</b> if you need write access to the stores.</param>
        /// <returns>
        /// An enumerator to iterate all sub stores in the store.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>

        public IEnumerable<IHierarchicalValueStore> EnumerateStores(bool writable)
        {
            return stores.Values;
        }

        //  ----------------
        //  OpenStore method
        //  ----------------

        /// <summary>
        /// Retrieves the store with the specified name,
        /// and specifies whether write access is to be applied to the store.
        /// </summary>
        /// <param name="name">The name of the store to retrieve.</param>
        /// <param name="writable">Set to <b>true</b> if you need write access to the store.</param>
        /// <returns>
        /// The store requested, or <b>null</b> if the operation failed.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>

        public IHierarchicalValueStore OpenStore(string name, bool writable)
        {
            if (string.IsNullOrEmpty(name)) return this;
            HierarchicalValueStore store = null;
            stores.TryGetValue(name, out store);
            return store;
        }

        //  ------------------
        //  DeleteStore method
        //  ------------------

        /// <summary>
        /// Deletes the store with the specified name.
        /// </summary>
        /// <param name="name">The name of the store to delete.</param>

        public void DeleteStore(string name)
        {
            stores.Remove(name);
        }

        #endregion methods
    }

    #endregion HierarchicalValueStore class

    #region HierarchicalValueStoreInterfaceExtensions class

    //  -----------------------------------------------
    //  HierarchicalValueStoreInterfaceExtensions class
    //  -----------------------------------------------

    /// <summary>
    /// Provides extension to class that implement the
    /// <see cref="IHierarchicalValueStore"/> interface.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public static class HierarchicalValueStoreInterfaceExtensions
    {
        //  ----------------
        //  OpenStore method
        //  ----------------

        /// <summary>
        /// Retrieves the store with the specified name,
        /// and specifies whether write access is to be applied to the store.
        /// </summary>
        /// <param name="store">The parent store.</param>
        /// <param name="name">The name of the store to retrieve.</param>
        /// <returns>
        /// The store requested, or <b>null</b> if the operation failed.
        /// </returns>

        public static IHierarchicalValueStore OpenStore(this IHierarchicalValueStore store, string name)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            return store.OpenStore(name, false);
        }

        //  -------------------
        //  OpenSubStore method
        //  -------------------

        /// <summary>
        /// Opens the store with the specified path read-only.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The store requested, or <b>null</b> if the operation failed.
        /// </returns>

        public static IHierarchicalValueStore OpenSubStore(this IHierarchicalValueStore store, string path)
        {
            return store.GetSubStore(path, false, false);
        }

        /// <summary>
        /// Retrieves the store from the specified path,
        /// and specifies whether write access is to be applied to the store.
        /// </summary>
        /// <param name="store">The parent store.</param>
        /// <param name="path">
        /// The path of the store to open relative to the parent store.
        /// </param>
        /// <param name="writable">
        /// Set to <b>true</b> if you need write access to the store.
        /// </param>
        /// <returns>
        /// The store requested, or <b>null</b> if the operation failed.
        /// </returns>

        public static IHierarchicalValueStore OpenSubStore(this IHierarchicalValueStore store, string path, bool writable)
        {
            return store.GetSubStore(path, false, writable);
        }

        //  ---------------------
        //  CreateSubStore method
        //  ---------------------

        /// <summary>
        /// Creates a new store or opens an existing store at the specified path.
        /// </summary>
        /// <param name="store">The parent store.</param>
        /// <param name="path">
        /// The path of the store to create or open relative to the parent store.
        /// </param>
        /// <param name="writable">
        /// Set to <b>true</b> if you need write access to the store.
        /// </param>
        /// <returns>
        /// The store requested, or <b>null</b> if the operation failed.
        /// </returns>

        public static IHierarchicalValueStore CreateSubStore(this IHierarchicalValueStore store, string path, bool writable)
        {
            return store.GetSubStore(path, true, writable);
        }

        //  ---------------------------------
        //  EnumerateValuesInSubStores method
        //  ---------------------------------

        /// <summary>
        /// Enumerates all values in the store and all sub stores.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>
        /// An enumerator to iterate thru all values.
        /// </returns>

        public static IEnumerable<StoreValuePair> EnumerateValuesInSubStores(this IHierarchicalValueStore store)
        {
            foreach (var namedValue in store.Values)
            {
                yield return new StoreValuePair(store, namedValue);
            }
            foreach (var subStore in store.Stores)
            {
                foreach (var namedValue in subStore.EnumerateValuesInSubStores()) yield return namedValue;
            }
        }

        #region private methods

        //  ------------------
        //  GetSubStore method
        //  ------------------

        private static IHierarchicalValueStore GetSubStore(
            this IHierarchicalValueStore store,
            string path,
            bool create,
            bool writable)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            var resultStore = store;
            foreach (var name in path.Split('\\'))
            {
                resultStore = create ? resultStore.CreateStore(name, writable) : resultStore.OpenStore(name, writable);
                if (resultStore == null) break;
            }
            return resultStore;
        }

        #endregion private methods
    }

    #endregion HierarchicalValueStoreInterfaceExtensions class

    #region StoreValuePair class

    //  --------------------
    //  StoreValuePair class
    //  --------------------

    /// <summary>
    /// Provides a tuple to hold a value store and a named value reference.
    /// </summary>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public class StoreValuePair : Tuple<IValueStore, INamedValue>
    {
        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreValuePair"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="namedValue">The named value.</param>

        public StoreValuePair(IValueStore store, INamedValue namedValue) : base(store, namedValue) { }
    }

    #endregion StoreValuePair class
}

// eof "IHierarchicalValueStore.cs"
