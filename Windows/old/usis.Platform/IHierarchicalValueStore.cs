//
//  @(#) IHierarchicalValueStore.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform
{
    //  ---------------------------------
    //  IHierarchicalValueStore interface
    //  ---------------------------------

    /// <summary>
    /// Defines the properties and methods that have to be implemented
    /// by a class to provide a hierarchically organized store for named values.
    /// </summary>

    [Obsolete("Use usis.Platform.Portable.IHierarchicalValueStore instead.")]
    public interface IHierarchicalValueStore : IValueStore
    {
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

        //  -------------------
        //  StoreNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub store names in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all sub store names in the store.
        /// </value>

        IEnumerable<string> StoreNames
        {
            get;
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

        IEnumerable<IHierarchicalValueStore> Stores
        {
            get;
        }
    }

    #region HierarchicalValueStoreInterfaceExtension class

    //  ----------------------------------------------
    //  HierarchicalValueStoreInterfaceExtension class
    //  ----------------------------------------------

    /// <summary>
    /// Provides extension to class that implement the
    /// <see cref="IHierarchicalValueStore"/> interface.
    /// </summary>

    [Obsolete("Use usis.Platform.Portable.IHierarchicalValueStore instead.")]
    public static class HierarchicalValueStoreInterfaceExtension
    {
        //  -------------------
        //  OpenSubStore method
        //  -------------------

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

    #endregion HierarchicalValueStoreInterfaceExtension class
}

// eof "IHierarchicalValueStore.cs"
