//
//  @(#) SnapInHost.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using usis.Framework.Portable;
using usis.Platform;

namespace usis.Framework
{
    #region SnapInHost class

    //  ----------------
    //  SnapInHost class
    //  ----------------

    /// <summary>
    /// Hosts snap-ins as core of a modular application.
    /// </summary>

    internal class SnapInHost<TActivator> where TActivator : Portable.SnapInActivator, new()
    {
        #region fields

        private List<SnapInCollectionItem> registeredSnapIns = new List<SnapInCollectionItem>();
        private List<SnapInCollectionItem> connectedSnapIns = new List<SnapInCollectionItem>();

        private Portable.SnapInActivator activator = new TActivator();

        #endregion fields

        #region properties

        //  --------------------------
        //  RegisteredSnapIns property
        //  --------------------------

        internal IEnumerable<SnapInCollectionItem> RegisteredSnapIns
        {
            get
            {
                foreach (var item in registeredSnapIns)
                {
                    yield return item;
                }
            }
        }

        #endregion properties

        #region methods

        #region RegisterSnapIn method

        //  ---------------------
        //  RegisterSnapIn method
        //  ---------------------

        /// <summary>
        /// Registers the specified type as a snap-in.
        /// </summary>
        /// <param name="typeName">The type name of the snap-in.</param>
        /// <param name="assemblyFile">
        /// The name, including the path,
        /// of a file that contains an assembly that defines the requested type.
        /// </param>
        /// <remarks>
        /// You should provide a full assembly qualified name to specify the snap-in.
        /// </remarks>

        internal void RegisterSnapIn(string typeName, string assemblyFile)
        {
            registeredSnapIns.Add(new SnapInCollectionItem(typeName, assemblyFile));
        }

        #endregion RegisterSnapIn method

        #region LoadSnapIns method

        //  ------------------
        //  LoadSnapIns method
        //  ------------------

        /// <summary>
        /// Loads all registered snap-ins.
        /// </summary>
        /// <param name="application">
        /// An application interface that is used to report exceptions.
        /// </param>

        public void LoadSnapIns(IApplication application)
        {
            foreach (var item in registeredSnapIns)
            {
                item.Load(application, activator);
            }
        }

        #endregion LoadSnapIns method

        #region ConnectSnapIns method

        //  ---------------------
        //  ConnectSnapIns method
        //  ---------------------

        /// <summary>
        /// Connects all loaded snap-ins.
        /// </summary>
        /// <param name="application">
        /// An application interface that is passed to each snap-in.
        /// </param>

        public void ConnectSnapIns(IApplication application)
        {
            var items = new List<SnapInCollectionItem>(registeredSnapIns);
            foreach (var item in items)
            {
                if (item.Connect(application, activator))
                {
                    connectedSnapIns.Add(item);
                }
            }
        }

        #endregion ConnectSnapIns method

        #region DisconnectSnapIns method

        //  ------------------------
        //  DisconnectSnapIns method
        //  ------------------------

        /// <summary>
        /// Disconnects all connected snap-ins.
        /// </summary>
        /// <param name="application">An application interface that is used to report exceptions.</param>
        /// <returns>
        /// <b>false</b> if at least one snap-in refused to disconnect, otherwise <b>true</b>.
        /// </returns>

        public bool DisconnectSnapIns(IApplication application)
        {
            foreach (var item in registeredSnapIns)
            {
                if (!item.CanDisconnect(application)) return false;
            }
            foreach (var item in registeredSnapIns)
            {
                if (item.Disconnect(application))
                {
                    connectedSnapIns.Remove(item);
                }
            }
            return true;
        }

        #endregion DisconnectSnapIns method

        #region UnloadSnapIns method

        //  --------------------
        //  UnloadSnapIns method
        //  --------------------

        /// <summary>
        /// Unloads all loaded snap-ins.
        /// </summary>
        /// <param name="application">
        /// An application interface that is used to report exceptions.
        /// </param>

        public void UnloadSnapIns(IApplication application)
        {
            foreach (var item in registeredSnapIns)
            {
                item.Unload(application);
            }
        }

        #endregion UnloadSnapIns method

        #region ConnectRequiredSnapIn method

        //  ----------------------------
        //  ConnectRequiredSnapIn method
        //  ----------------------------

        /// <summary>
        /// Connects a snap-in that is required by the calling snap-in.
        /// </summary>
        /// <param name="application">
        /// An application interface that is passed to the snap-in.
        /// </param>
        /// <param name="instance">
        /// The snap-in that depends on the snap-in to connect.
        /// </param>
        /// <param name="type">
        /// The type of the snap-in to connect.
        /// </param>
        /// <returns>
        /// <b>true</b> if the snap-in is connected; otherwise, <b>false</b>.
        /// </returns>

        public bool ConnectRequiredSnapIn(IApplication application, ISnapIn instance, Type type)
        {
            var newItem = new SnapInCollectionItem(type);
            int i = -1;
            foreach (var item in registeredSnapIns)
            {
                i++;
                if (item.IsEqualInstance(instance)) break;
            }
            if (i >= 0) registeredSnapIns.Insert(i, newItem);
            else registeredSnapIns.Add(newItem);

            return newItem.Connect(application, activator);
        }

        #endregion ConnectRequiredSnapIn method

        #region PauseSnapIns/ResumeSnapIns methods

        //  -------------------
        //  PauseSnapIns method
        //  -------------------

        public void PauseSnapIns()
        {
            Debug.WriteLine("Pausing snap-ins ...");
            foreach (var item in registeredSnapIns)
            {
                var snapIn = item.Instance as IServiceSnapIn;
                if (snapIn != null) snapIn.Pause();
            }
        }

        //  --------------------
        //  ResumeSnapIns method
        //  --------------------

        public void ResumeSnapIns()
        {
            foreach (var item in registeredSnapIns)
            {
                var snapIn = item.Instance as IServiceSnapIn;
                if (snapIn != null) snapIn.Resume();
            }
        }

        #endregion PauseSnapIns/ResumeSnapIns methods

        #endregion methods
    }

    #endregion SnapInHost class

    #region SnapInCollectionItem class

    //  --------------------------
    //  SnapInCollectionItem class
    //  --------------------------

    internal class SnapInCollectionItem
    {
        #region fields

        private string typeName;
        private string assemblyFile;
        private Type type;

        #endregion fields

        #region properties

        //  -----------------
        //  IsLoaded property
        //  -----------------

        /// <summary>
        /// Gets a value that indicates whether the snap-in is loaded.
        /// </summary>

        internal bool IsLoaded
        {
            get { return Instance != null; }
        }

        //  --------------------
        //  IsConnected property
        //  --------------------

        internal bool IsConnected
        {
            get;
            private set;
        }

        //  -----------------
        //  Instance property
        //  -----------------

        internal ISnapIn Instance
        {
            get;
            private set;
        }

        //  ------------------
        //  Exception property
        //  ------------------

        internal Exception Exception
        {
            get;
            private set;
        }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal SnapInCollectionItem(string typeName, string assemblyFile)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentNullOrWhiteSpaceException(nameof(typeName));
            this.typeName = typeName;
            this.assemblyFile = assemblyFile;
        }

        internal SnapInCollectionItem(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            this.type = type;
        }

        #endregion construction

        #region Load method

        //  -----------
        //  Load method
        //  -----------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal bool Load(IApplication application, Portable.SnapInActivator activator)
        {
            if (!IsLoaded && Exception == null)
            {
                try
                {
                    var instance = activator.CreateInstance(application, new SnapInActivatorContext(type, typeName, assemblyFile));
                    Instance = instance as ISnapIn;
                    if (Instance == null && instance != null)
                    {
                        var disposable = instance as IDisposable;
                        if (disposable != null) disposable.Dispose();

                        throw new NotImplementedException(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.SnapInInterfaceNotImplemented,
                            instance.GetType().FullName,
                            typeof(ISnapIn).FullName));
                    }
                    Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Snap-in loaded: '{0}'", Instance));
                }
                catch (Exception exception)
                {
                    ReportException(application, exception, Strings.FailedToLoadSnapIn);
                }
            }
            return IsLoaded;
        }

        #endregion Load method

        #region Connect method

        //  --------------
        //  Connect method
        //  --------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal bool Connect(IApplication application, Portable.SnapInActivator activator)
        {
            if (Load(application, activator))
            {
                if (!IsConnected)
                {
                    try
                    {
                        IsConnected = Instance.OnConnection(application);

                        Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Snap-in {1}connected: '{0}'", Instance, IsConnected ? string.Empty : "not "));
                    }
                    catch (Exception exception)
                    {
                        ReportException(application, exception, Strings.FailedToConnectSnapIn);
                    }
                }
            }
            return IsConnected;
        }

        #endregion Connect method

        #region CanDisconnect method

        //  --------------------
        //  CanDisconnect method
        //  --------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal bool CanDisconnect(IApplication application)
        {
            if (!IsConnected) return true;
            try
            {
                return Instance.CanDisconnect();
            }
            catch (Exception exception)
            {
                ReportException(application, exception, Strings.FailedToDisconnectSnapIn);
            }
            return false;
        }

        #endregion CanDisconnect method

        #region Disconnect method

        //  -----------------
        //  Disconnect method
        //  -----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal bool Disconnect(IApplication application)
        {
            if (IsConnected)
            {
                try
                {
                    Instance.OnDisconnection();
                    IsConnected = false;

                    Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Snap-in disconnected: '{0}'", Instance));
                }
                catch (Exception exception)
                {
                    ReportException(application, exception, Strings.FailedToDisconnectSnapIn);
                }
            }
            return !IsConnected;
        }

        #endregion Disconnect method

        #region Unload method

        //  -------------
        //  Unload method
        //  -------------

        internal bool Unload(IApplication application)
        {
            if (IsLoaded)
            {
                if (Disconnect(application))
                {
                    string name = Instance.ToString();

                    // dispose snap-in instance
                    IDisposable disposable = Instance as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }

                    // release instance
                    Instance = null;

                    Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Snap-in unloaded: '{0}'", name));
                }
            }
            return !IsLoaded;
        }

        #endregion Unload method

        #region IsEqualInstance method

        //  ----------------------
        //  IsEqualInstance method
        //  ----------------------

        internal bool IsEqualInstance(ISnapIn snapIn)
        {
            return Instance == snapIn;
        }

        #endregion IsEqualInstance method

        #region CreateConfiguration method

        ////  --------------------------
        ////  CreateConfiguration method
        ////  --------------------------

        //internal SnapInConfiguration CreateConfiguration()
        //{
        //    var configuration = new SnapInConfiguration();
        //    if (type != null)
        //    {
        //        configuration.TypeName = type.AssemblyQualifiedName;
        //    }
        //    else if (!string.IsNullOrWhiteSpace(typeName))
        //    {
        //        configuration.TypeName = typeName;
        //    }
        //    return configuration;
        //}

        #endregion CreateConfiguration method

        #region private methods

        #region CreateInstance method

        //  ---------------------
        //  CreateInstance method
        //  ---------------------

        //private void CreateInstance()
        //{
        //    Type snapInType = null;
        //    object instance = null;

        //    if (!string.IsNullOrWhiteSpace(typeName))
        //    {
        //        if (!string.IsNullOrWhiteSpace(assemblyFile))
        //        {
        //            var handle = Activator.CreateInstanceFrom(assemblyFile, typeName);
        //            instance = handle?.Unwrap();
        //        }
        //        else snapInType = Type.GetType(typeName, true);
        //    }
        //    else if (type != null)
        //    {
        //        snapInType = type;
        //    }
        //    else throw new InvalidOperationException();

        //    // create instance from type
        //    if (snapInType != null) instance = Activator.CreateInstance(snapInType);

        //    //  failed to create instance
        //    if (instance == null) throw new TypeLoadException();

        //    var snapIn = instance as ISnapIn;
        //    if (snapIn == null)
        //    {
        //        var disposable = instance as IDisposable;
        //        if (disposable != null) disposable.Dispose();

        //        throw new NotImplementedException(string.Format(
        //            CultureInfo.CurrentCulture,
        //            Strings.SnapInInterfaceNotImplemented,
        //            snapInType.FullName,
        //            typeof(ISnapIn).FullName));
        //    }
        //    Instance = snapIn;
        //}

        #endregion CreateInstance method

        #region ReportException method

        //  ----------------------
        //  ReportException method
        //  ----------------------

        private void ReportException(IApplication application, Exception exception, string message)
        {
            string description = Instance == null ?
                type == null ? typeName : type.FullName :
                Instance.ToString();
            string format = string.Format(CultureInfo.CurrentCulture, message, description, "{0}");

            Exception = exception;

            if (application != null) application.ReportException(exception);

            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, format, exception.Message));
        }

        #endregion ReportException method

        #endregion private methods
    }

    #endregion SnapInCollectionItem class
}

// eof "SnapInHost.cs"
