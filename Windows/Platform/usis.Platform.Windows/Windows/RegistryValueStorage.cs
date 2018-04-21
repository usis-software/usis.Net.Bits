//
//  @(#) RegistryValueStorage.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    //  --------------------------
    //  RegistryValueStorage class
    //  --------------------------

    /// <summary>
    /// Implements the <see cref="IHierarchicalValueStorage"/> interface to provide
    /// access to the Windows registry as a hierachical value storage.
    /// </summary>

    public sealed class RegistryValueStorage : IHierarchicalValueStorage
    {
        #region constants

        //  ---------
        //  constants
        //  ---------

        private const char separator = '\\';

        private const string HKeyLocalMachine = "HKEY_LOCAL_MACHINE";
        private const string HKeyCurrentUser = "HKEY_CURRENT_USER";
        private const string HKeyClassesRoot = "HKEY_CLASSES_ROOT";

        #endregion constants

        #region fields

        private RegistryRoot root;
        private string path;
        private bool isWritable;

        #endregion fields

        #region construction and private methods

        //  ------------
        //  construction
        //  ------------

        private RegistryValueStorage(RegistryRoot root, string path, bool writable)
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
                var w = writable ?? isWritable;
                var k = key;
                key = k.OpenSubKey(path, w);
#if DOTNET_4
                if (key == null) key = k.CreateSubKey(path, w ? RegistryKeyPermissionCheck.ReadWriteSubTree : RegistryKeyPermissionCheck.ReadSubTree);
#else
                if (key == null) key = k.CreateSubKey(path, w);
#endif
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

        private static string[] SplitPath(string path, bool skipFirst)
        {
            var a = path.Split(separator);
            return a.Skip(skipFirst ? 1 : 0).ToArray();
        }

        //  ---------------------------
        //  BuildPathFromKeyName method
        //  ---------------------------

        private static string BuildPathFromKeyName(string name)
        {
            return BuildPath(SplitPath(name, true));
        }

        //  ------------------
        //  OpenStorage method
        //  ------------------

        private IHierarchicalValueStorage OpenStorage(RegistryKey registryKey, string name, bool writable)
        {
            using (var subKey = registryKey.OpenSubKey(name, writable))
            {
                if (subKey != null)
                {
                    return new RegistryValueStorage(root, BuildPathFromKeyName(subKey.Name), writable);
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
        /// as a hierachical value storage.
        /// </summary>
        /// <returns>
        /// A <see cref="RegistryValueStorage"/> that represents the HKEY_LOCAL_MACHINE registry base key.
        /// </returns>

        public static RegistryValueStorage OpenLocalMachine()
        {
            return new RegistryValueStorage(RegistryRoot.LocalMachine, null, true);
        }

        //  ----------------------
        //  OpenCurrentUser method
        //  ----------------------

        /// <summary>
        /// Opens a value storage for the specified path in the HKEY_CURRENT_USER registry hive.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="RegistryValueStorage"/> that represents the specified path in the HKEY_CURRENT_USER registry hive.
        /// </returns>

        public static RegistryValueStorage OpenCurrentUser(string path)
        {
            return new RegistryValueStorage(RegistryRoot.CurrentUser, path, false);
        }

        /// <summary>
        /// Opens a value storage for the specified path in the HKEY_CURRENT_USER registry hive.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="writable">Set to <c>true</c> if you need write access to the storage.</param>
        /// <returns>
        /// A <see cref="RegistryValueStorage"/> that represents the specified path in the HKEY_CURRENT_USER registry hive.
        /// </returns>

        public static RegistryValueStorage OpenCurrentUser(string path, bool writable)
        {
            return new RegistryValueStorage(RegistryRoot.CurrentUser, path, writable);
        }

        //  ----------------------
        //  OpenClassesRoot method
        //  ----------------------

        /// <summary>
        /// Opens the HKEY_CLASSES_ROOT registry base key
        /// as a hierachical value storage.
        /// </summary>
        /// <returns>
        /// A <see cref="RegistryValueStorage"/> that represents the HKEY_CLASSES_ROOT registry base key.
        /// </returns>

        public static RegistryValueStorage OpenClassesRoot()
        {
            return new RegistryValueStorage(RegistryRoot.ClassesRoot, null, true);
        }

        //  ----------------------
        //  FromRegistryKey method
        //  ----------------------

        /// <summary>
        /// Creates a <see cref="RegistryValueStorage" /> from a given registry key.
        /// </summary>
        /// <param name="registryKey">The registry key.</param>
        /// <returns>
        /// A <see cref="RegistryValueStorage" /> that represents the given registry key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="registryKey"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public static RegistryValueStorage FromRegistryKey(RegistryKey registryKey)
        {
            if (registryKey == null) throw new ArgumentNullException(nameof(registryKey));

            var names = SplitPath(registryKey.Name, false);
            var path = BuildPath(names.Skip(1).ToArray());
            switch (names.FirstOrDefault())
            {
                case HKeyLocalMachine:
                    return new RegistryValueStorage(RegistryRoot.LocalMachine, path, true);
                case HKeyCurrentUser:
                    return new RegistryValueStorage(RegistryRoot.CurrentUser, path, true);
                case HKeyClassesRoot:
                    return new RegistryValueStorage(RegistryRoot.ClassesRoot, path, true);
                default:
                    break;
            }
            return null;
        }

        #endregion methods

        #region value storage members

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value storage.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <exception cref="InvalidOperationException">
        /// The internal state of the storage is inconsistant.
        /// </exception>

        public string Name
        {
            get
            {
                if (path == null)
                {
                    switch (root)
                    {
                        case RegistryRoot.LocalMachine:
                            return HKeyLocalMachine;
                        case RegistryRoot.CurrentUser:
                            return HKeyCurrentUser;
                        case RegistryRoot.ClassesRoot:
                            return HKeyClassesRoot;
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
        /// Gets an enumerator to iterate all value names in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all value names in the storage.
        /// </value>

        public IEnumerable<string> ValueNames
        {
            get
            {
                using (var registryKey = OpenRegistryKey())
                {
                    if (registryKey == null) throw new InvalidOperationException();
                    return registryKey.GetValueNames();
                }
            }
        }

        //  ---------------
        //  Values property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all named values in the storage.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all named values in the storage.
        /// </value>

        public IEnumerable<INamedValue> Values
        {
            get
            {
                using (var registryKey = OpenRegistryKey())
                {
                    if (registryKey == null) throw new InvalidOperationException();
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
        /// Retrieves the value with the specified name from the storage.
        /// </summary>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>
        /// A type that implements <see cref="INamedValue"/> and represents the specified value.
        /// </returns>

        public INamedValue GetValue(string name)
        {
            using (var registryKey = OpenRegistryKey())
            {
                if (registryKey == null) throw new InvalidOperationException();
                return new NamedValue(name, registryKey.GetValue(name));
            }
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
            using (var registryKey = OpenRegistryKey())
            {
                if (registryKey == null) throw new InvalidOperationException();
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
        /// <c>true</c> when value was deleted
        /// or
        /// <c>false</c> when a value with the specified name does not exist.
        /// </returns>

        public bool DeleteValue(string name)
        {
            using (var registryKey = OpenRegistryKey(true))
            {
                if (registryKey == null) throw new InvalidOperationException();
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

        #endregion value storage members

        #region hierarchical storage members

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
            get
            {
                using (var registryKey = OpenRegistryKey())
                {
                    if (registryKey == null) throw new InvalidOperationException();
                    return registryKey.GetSubKeyNames();
                }
            }
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

        public IEnumerable<IHierarchicalValueStorage> Storages => EnumerateStorages(false);

        #endregion properties

        #region methods

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
            using (var registryKey = OpenRegistryKey())
            {
                if (registryKey == null) throw new InvalidOperationException();
                return OpenStorage(registryKey, name, writable);
            }
        }

        //  --------------------
        //  CreateStorage method
        //  --------------------

        /// <summary>
        /// Creates a new storage or opens an existing storage with the specified access.
        /// </summary>
        /// <param name="name">The name of the storage to create or open. This string is not case-sensitive.</param>
        /// <param name="writable"><c>true</c> to indicate that the new sub storage is writable; otherwise, <c>false</c>.</param>
        /// <returns>
        /// The newly created storage, or <c>null</c> if the operation failed.
        /// If a zero-length string is specified for <paramref name="name" />,
        /// the current storage object is returned.
        /// </returns>

        public IHierarchicalValueStorage CreateStorage(string name, bool writable)
        {
            using (var registryKey = OpenRegistryKey())
            {
                if (registryKey == null) throw new InvalidOperationException();
#if DOTNET_46_OR_HIGHER
                using (var key = registryKey.CreateSubKey(name, writable))
#else
                using (var key = registryKey.CreateSubKey(name, writable ? RegistryKeyPermissionCheck.ReadWriteSubTree : RegistryKeyPermissionCheck.ReadSubTree))
#endif
                {
                    return key == null ? null : new RegistryValueStorage(root, BuildPathFromKeyName(key.Name), true);
                }
            }

        }

        //  ------------------------
        //  EnumerateStorages method
        //  ------------------------

        /// <summary>
        /// Gets an enumerator to iterate all sub storages in the storage,
        /// and specifies whether write access is to be applied to the storages.
        /// </summary>
        /// <param name="writable">Set to <c>true</c> if you need write access to the storages.</param>
        /// <returns>
        /// An enumerator to iterate all sub storages in the storage.
        /// </returns>

        public IEnumerable<IHierarchicalValueStorage> EnumerateStorages(bool writable)
        {
            using (var registryKey = OpenRegistryKey())
            {
                if (registryKey == null) throw new InvalidOperationException();
                foreach (var name in registryKey.GetSubKeyNames())
                {
                    IHierarchicalValueStorage storage;
                    try
                    {
                        storage = OpenStorage(registryKey, name, writable);
                    }
                    catch (System.Security.SecurityException)
                    {
                        storage = null;
                    }
                    if (storage != null) yield return storage;
                }
            }
        }

        //  ---------------------
        //  DeleteStorage methods
        //  ---------------------

        /// <summary>
        /// Deletes the storage with the specified name.
        /// </summary>
        /// <param name="name">The name of the storage to delete.</param>

        [Obsolete("TODO")]
        public void DeleteStorage(string name)
        {
            throw new NotImplementedException();
        }

        #endregion methods

        #endregion hierarchical storage members

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
                    sb.Append(HKeyClassesRoot);
                    break;
                case RegistryRoot.CurrentUser:
                    sb.Append(HKeyCurrentUser);
                    break;
                case RegistryRoot.LocalMachine:
                    sb.Append(HKeyLocalMachine);
                    break;
                default:
                    break;
            }
            sb.Append(separator);
            sb.Append(path);
            return sb.ToString();
        }

        #endregion overrides
    }
}

// eof "RegistryValueStorage.cs"
