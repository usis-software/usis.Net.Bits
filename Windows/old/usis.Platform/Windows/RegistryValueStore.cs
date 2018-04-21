//
//  @(#) RegistryValueStore.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using usis.Platform.Portable;

namespace usis.Platform.Windows
{
    #region RegistryRoot enumeration

    //  ------------------------
    //  RegistryRoot enumeration
    //  ------------------------

    internal enum RegistryRoot
    {
        ClassesRoot,
        CurrentUser,
        LocalMachine,
    };

    #endregion RegistryRoot enumeration

    //  ------------------------
    //  RegistryValueStore class
    //  ------------------------

    /// <summary>
    /// Implements the <see cref="IHierarchicalValueStore"/> interface to provide
    /// access to the Windows registry as a hierachical value store.
    /// </summary>

    [Obsolete("Use type from usis.Platform.Windows assembly instead.")]
    public sealed class RegistryValueStore : IHierarchicalValueStore
    {
        #region fields

        private RegistryRoot root;
        private string path;
        private bool isWritable;

        #endregion fields

        #region construction and private methods

        //  ---------
        //  constants
        //  ---------

        private const char separator = '\\';

        //  ------------
        //  construction
        //  ------------

        private RegistryValueStore(RegistryRoot root, string path, bool writable)
        {
            this.root = root;
            this.path = path;
            isWritable = writable;
        }

        //  ----------------------
        //  OpenRegistryKey method
        //  ----------------------

        private RegistryKey OpenRegistryKey(bool? writable = null)
        {
            RegistryKey key = null;
            switch (root)
            {
                case RegistryRoot.ClassesRoot:
                    key = Registry.ClassesRoot;
                    break;
                case RegistryRoot.CurrentUser:
                    key = Registry.CurrentUser;
                    break;
                case RegistryRoot.LocalMachine:
                    key = Registry.LocalMachine;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if (path != null && path.Length > 0)
            {
                key = key.OpenSubKey(path, writable.HasValue ? writable.Value : isWritable);
            }
            return key;
        }

        //  ----------------
        //  BuildPath method
        //  ----------------

        private static string BuildPath(string[] path)
        {
            StringBuilder s = new StringBuilder();
            foreach (var name in path)
            {
                if (s.Length > 0) s.Append(separator);
                s.Append(name);
            }
            return s.ToString();
        }

        //  ----------------
        //  SplitPath method
        //  ----------------

        private static string[] SplitPath(string path)
        {
            var a = path.Split(separator);
            return a.Skip(1).ToArray();
        }

        //  ---------------------------
        //  BuildPathFromKeyName method
        //  ---------------------------

        private static string BuildPathFromKeyName(string name)
        {
            return BuildPath(SplitPath(name));
        }

        //  ----------------
        //  OpenStore method
        //  ----------------

        private IHierarchicalValueStore OpenStore(RegistryKey registryKey, string name, bool writable)
        {
            using (var subKey = registryKey.OpenSubKey(name, writable))
            {
                if (subKey != null)
                {
                    return new RegistryValueStore(root, BuildPathFromKeyName(subKey.Name), writable);
                }
                else return null;
            }
        }

        #endregion construction and private methods

        #region methods

        //  -----------------------
        //  OpenLocalMachine method
        //  -----------------------

        /// <summary>
        /// Opens the HKEY_LOCAL_MACHINE registry base key
        /// as a hierachical value store.
        /// </summary>
        /// <returns>
        /// A <i>RegistryValueStore</i> that represents the HKEY_LOCAL_MACHINE registry base key.
        /// </returns>

        public static RegistryValueStore OpenLocalMachine()
        {
            return new RegistryValueStore(RegistryRoot.LocalMachine, null, true);
        }

        //  ----------------------
        //  OpenClassesRoot method
        //  ----------------------

        /// <summary>
        /// Opens the HKEY_CLASSES_ROOT registry base key
        /// as a hierachical value store.
        /// </summary>
        /// <returns>
        /// A <i>RegistryValueStore</i> that represents the HKEY_CLASSES_ROOT registry base key.
        /// </returns>

        public static RegistryValueStore OpenClassesRoot()
        {
            return new RegistryValueStore(RegistryRoot.ClassesRoot, null, true);
        }

        #endregion methods

        #region values

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value store.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <exception cref="InvalidOperationException"></exception>

        public string Name
        {
            get
            {
                if (path == null)
                {
                    switch (root)
                    {
                        case RegistryRoot.LocalMachine:
                            return "HKEY_LOCAL_MACHINE";
                        case RegistryRoot.CurrentUser:
                            return "HKEY_CURRENT_USER";
                        default:
                            throw new InvalidOperationException();
                    }
                }
                else return path.Split(separator).Last();
            }
        }

        //  -------------------
        //  ValueNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all value names in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all value names in the store.
        /// </value>

        public IEnumerable<string> ValueNames
        {
            get
            {
                using (var registryKey = OpenRegistryKey())
                {
                    return registryKey.GetValueNames();
                }
            }
        }

        //  ---------------
        //  Values property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all named values in the store.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all named values in the store.
        /// </value>

        public IEnumerable<INamedValue> Values
        {
            get
            {
                using (var registryKey = OpenRegistryKey())
                {
                    foreach (var name in registryKey.GetValueNames())
                    {
                        yield return GetValue(name);
                    }
                }
            }
        }

        #endregion properties

        #region methods

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>
        /// A type that implements <b>INamedValue</b> and represents the specified value.
        /// </returns>

        public INamedValue GetValue(string name)
        {
            using (var registryKey = OpenRegistryKey())
            {
                return new NamedValue(name, registryKey.GetValue(name));
            }
        }

        //  ---------------
        //  SetValue method
        //  ---------------

        /// <summary>
        /// Saves the specified named value in the store.
        /// </summary>
        /// <param name="value">The named value to save.</param>
        /// <exception cref="ArgumentNullException">
        /// <i>value</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        public void SetValue(INamedValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            using (var registryKey = OpenRegistryKey())
            {
                registryKey.SetValue(value.Name, value.Value);
            }
        }

        //  ------------------
        //  DeleteValue method
        //  ------------------

        /// <summary>
        /// Deletes the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to delete.</param>
        /// <returns>
        /// <b>true</b> when value was deleted
        /// or
        /// <b>false</b> when a value with the specified name does not exist.
        /// </returns>

        public bool DeleteValue(string name)
        {
            using (var registryKey = OpenRegistryKey(true))
            {
                try
                {
                    registryKey.DeleteValue(name, false);
                    return true;
                }
                catch (ArgumentException)
                {
                    if (name != null) return false;
                    throw;
                }
            }
        }

        #endregion methods

        #endregion values

        #region stores

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

        public IEnumerable<string> StoreNames
        {
            get
            {
                using (var registryKey = OpenRegistryKey())
                {
                    return registryKey.GetSubKeyNames();
                }
            }
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

        public IEnumerable<IHierarchicalValueStore> Stores { get { return EnumerateStores(false); } }

        #endregion properties

        #region methods

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

        public IHierarchicalValueStore OpenStore(string name, bool writable)
        {
            using (var registryKey = OpenRegistryKey())
            {
                return OpenStore(registryKey, name, writable);
            }
        }

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

        public IHierarchicalValueStore CreateStore(string name, bool writable)
        {
            using (var registryKey = OpenRegistryKey())
            {
                using (var key = registryKey.CreateSubKey(name, writable))
                {
                    return key == null ? null : new RegistryValueStore(root, BuildPathFromKeyName(key.Name), true);
                }
            }

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

        public IEnumerable<IHierarchicalValueStore> EnumerateStores(bool writable)
        {
            using (var registryKey = OpenRegistryKey())
            {
                foreach (var name in registryKey.GetSubKeyNames())
                {
                    IHierarchicalValueStore store;
                    try
                    {
                        store = OpenStore(registryKey, name, writable);
                    }
                    catch (System.Security.SecurityException)
                    {
                        store = null;
                    }
                    if (store != null) yield return store;
                }
            }
        }

        //  -------------------
        //  DeleteStore methods
        //  -------------------

        /// <summary>
        /// Deletes the store with the specified name.
        /// </summary>
        /// <param name="name">The name of the store to delete.</param>

        public void DeleteStore(string name)
        {
            throw new NotImplementedException();
        }

        #endregion methods

        #endregion stores

        #region overrides

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
            var sb = new StringBuilder();
            switch (root)
            {
                case RegistryRoot.ClassesRoot:
                    sb.Append("HKEY_CLASSES_ROOT");
                    break;
                case RegistryRoot.CurrentUser:
                    sb.Append("HKEY_CURRENT_USER");
                    break;
                case RegistryRoot.LocalMachine:
                    sb.Append("HKEY_CURRENT_USER");
                    break;
                default:
                    break;
            }
            sb.Append('\\');
            sb.Append(path);
            return sb.ToString();
        }

        #endregion overrides
    }
}

// eof "RegistryValueStore.cs"
