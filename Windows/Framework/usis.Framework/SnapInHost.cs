//
//  @(#) SnapInHost.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using usis.Framework.Configuration;
using usis.Platform;

namespace usis.Framework
{
    //  ----------------------------
    //  SnapInHost<TActivator> class
    //  ----------------------------

    /// <summary>
    /// Hosts snap-ins as core of a modular application.
    /// </summary>
    /// <typeparam name="TActivator">The type of the activator to create snap-in instances.</typeparam>

    public class SnapInHost<TActivator> where TActivator : SnapInActivator, new()
    {
        #region fields

        private List<SnapInCollectionItem> registeredSnapIns = new List<SnapInCollectionItem>();
        private List<SnapInCollectionItem> connectedSnapIns = new List<SnapInCollectionItem>();

        private SnapInActivator activator = new TActivator();

        #endregion fields

        #region properties

        #region RegisteredSnapIns property

        //  --------------------------
        //  RegisteredSnapIns property
        //  --------------------------

        /// <summary>
        /// Gets an enumerator to iterate all registered snap-ins.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all registered snap-ins.
        /// </value>

        public IEnumerable<SnapInCollectionItem> RegisteredSnapIns
        {
            get
            {
                foreach (var item in registeredSnapIns)
                {
                    yield return item;
                }
            }
        }

        #endregion RegisteredSnapIns property

        #region ConnectedSnapIns property

        //  -------------------------
        //  ConnectedSnapIns property
        //  -------------------------

        /// <summary>
        /// Gets a collection of snap-ins that are connected by the application.
        /// </summary>
        /// <value>
        /// An enumeration of connected snap-ins.
        /// </value>

        public IEnumerable<ISnapIn> ConnectedSnapIns
        {
            get
            {
                foreach (var item in RegisteredSnapIns)
                {
                    if (item.IsConnected) yield return item.Instance;
                }
            }
        }

        #endregion ConnectedSnapIns property

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

        public void RegisterSnapIn(string typeName, string assemblyFile)
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
            foreach (var item in registeredSnapIns) item.Load(application, activator);
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
                if (item.Connect(application, activator)) connectedSnapIns.Add(item);
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
        /// <param name="force">if set to <c>true</c> all snap-ins are forced to disconnect.</param>
        /// <returns>
        ///   <c>false</c> if at least one snap-in refused to disconnect, otherwise <c>true</c>.
        /// </returns>

        public bool DisconnectSnapIns(IApplication application, bool force)
        {
            if (!force)
            {
                foreach (var item in registeredSnapIns)
                {
                    if (!item.CanDisconnect(application)) return false;
                }
            }
            foreach (var item in registeredSnapIns)
            {
                if (item.Disconnect(application)) connectedSnapIns.Remove(item);
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
            foreach (var item in registeredSnapIns) item.Unload(application);
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

        /// <summary>
        /// Called to pause all operations performed by the connected snap-ins.
        /// </summary>
        /// <param name="application">The application that is hosting the snap-ins.</param>

        public void PauseSnapIns(IApplication application)
        {
            foreach (var item in registeredSnapIns) item.Pause(application);
        }

        //  --------------------
        //  ResumeSnapIns method
        //  --------------------

        /// <summary>
        /// Called to resume all operations performed by the connected snap-ins.
        /// </summary>
        /// <param name="application">The application that is hosting the snap-ins.</param>

        public void ResumeSnapIns(IApplication application)
        {
            foreach (var item in registeredSnapIns) item.Resume(application);
        }

        #endregion PauseSnapIns/ResumeSnapIns methods

        #region Configure method

        //  ----------------
        //  Configure method
        //  ----------------

        /// <summary>
        /// Configures the snap-in host with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>

        public void Configure(IApplicationConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            foreach (var item in configuration.SnapIns)
            {
                RegisterSnapIn(item.TypeName, item.AssemblyFile);
            }
        }

        #endregion Configure method

        #region ConnectRequiredSnapIns method

        //  -----------------------------
        //  ConnectRequiredSnapIns method
        //  -----------------------------

        /// <summary>
        /// Connects the specified required snap-ins.
        /// </summary>
        /// <param name="application">An application interface that is passed to the snap-in.</param>
        /// <param name="instance">The snap-in that depends on the snap-ins to connect.</param>
        /// <param name="snapInTypes">The types of the snap-ins to connect.</param>
        /// <exception cref="ArgumentNullException"><c>snapInTypes</c> is a null reference.</exception>

        public void ConnectRequiredSnapIns(IApplication application, ISnapIn instance, params Type[] snapInTypes)
        {
            if (snapInTypes == null) throw new ArgumentNullException(nameof(snapInTypes));
            foreach (var type in snapInTypes)
            {
                if (type == null) continue;
                ConnectRequiredSnapIn(application, instance, type);
            }
        }

        #endregion ConnectRequiredSnapIns method

        #region Startup method

        //  --------------
        //  Startup method
        //  --------------

        /// <summary>
        /// Starts an application by loading and connecting all snap-ins.
        /// After that, each registered extension is also started.
        /// </summary>
        /// <param name="application">The application to start.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>application</c> is a null reference.
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Startup(IApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            LoadSnapIns(application);
            ConnectSnapIns(application);

            // start application extensions

            foreach (var extension in application.Extensions)
            {
                if (extension is IApplicationExtension solutionExtension)
                {
                    try
                    {
                        solutionExtension.Start(application);
                    }
                    catch (Exception exception)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, "Failed to start application extension:\n{0}\nSee inner exception for details.", exception.Message);
                        application.ReportException(new ExtensionException<TActivator>(this, extension, message, exception));
                    }
                }
            }
        }

        #endregion Startup method

        #region Shutdown method

        //  ---------------
        //  Shutdown method
        //  ---------------

        /// <summary>
        /// Shuts down an application.
        /// </summary>
        /// <param name="application">The application to shutdown.</param>
        /// <param name="force">if set to <c>true</c> all connected snap-ins a forced to disconnect.</param>
        /// <returns>
        ///   <c>false</c> if at least one snap-in refused to disconnect, otherwise <c>true</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><c>application</c> is a null reference.</exception>

        public bool Shutdown(IApplication application, bool force)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));

            if (!DisconnectSnapIns(application, force)) return false;

            UnloadSnapIns(application);

            // detach all extensions
            if (application.Extensions != null)
            {
                var extensions = application.Extensions.ToArray();
                application.Extensions.Clear();
                foreach (var extension in extensions)
                {
                    (extension as IDisposable)?.Dispose();
                }
            }

            return true;
        }

        #endregion Shutdown method

        #endregion methods
    }

    #region SnapInCollectionItem class

    //  --------------------------
    //  SnapInCollectionItem class
    //  --------------------------

    /// <summary>
    /// Represents a snap-in registered in a snap-in host.
    /// </summary>

    public class SnapInCollectionItem
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
        /// <value>
        ///   <c>true</c> if the snap-in is loaded; otherwise, <c>false</c>.
        /// </value>

        internal bool IsLoaded => Instance != null;

        //  --------------------
        //  IsConnected property
        //  --------------------

        /// <summary>
        /// Gets a value indicating whether the snap-in is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if the snap-in is connected; otherwise, <c>false</c>.
        /// </value>

        public bool IsConnected { get; private set; }

        //  -----------------
        //  Instance property
        //  -----------------

        /// <summary>
        /// Gets the snap-in instance.
        /// </summary>
        /// <value>
        /// The snap-in instance.
        /// </value>

        public ISnapIn Instance { get; private set; }

        //  ------------------
        //  Exception property
        //  ------------------

        internal Exception Exception { get; private set; }

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
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        #endregion construction

        #region internal methods

        #region Load method

        //  -----------
        //  Load method
        //  -----------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal bool Load(IApplication application, SnapInActivator activator)
        {
            if (!IsLoaded && Exception == null)
            {
                try
                {
                    var context = new SnapInActivatorContext(type, typeName, assemblyFile);
                    var instance = activator.CreateInstance(application, context);
                    Instance = instance as ISnapIn;
                    if (Instance == null && instance != null)
                    {
                        if (instance is IDisposable disposable) disposable.Dispose();

                        throw new NotImplementedException(string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.Strings.SnapInInterfaceNotImplemented,
                            instance.GetType().FullName,
                            typeof(ISnapIn).FullName));
                    }
                    Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Snap-in loaded: '{0}'", Instance));
                }
                catch (Exception exception)
                {
                    ReportException(application, exception, Resources.Strings.FailedToLoadSnapIn);
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
        internal bool Connect(IApplication application, SnapInActivator activator)
        {
            if (Load(application, activator))
            {
                if (!IsConnected)
                {
                    try
                    {
                        IsConnected = Instance.OnConnection(application);

                        Debug.WriteLine(string.Format(CultureInfo.CurrentCulture,
                            "Snap-in {1}connected: '{0}'", Instance, IsConnected ? string.Empty : "not "));
                    }
                    catch (Exception exception)
                    {
                        ReportException(application, exception, Resources.Strings.FailedToConnectSnapIn);
                    }
                }
            }
            return IsConnected;
        }

        #endregion Connect method

        #region Pause method

        //  ------------
        //  Pause method
        //  ------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal void Pause(IApplication application)
        {
            try { (Instance as IServiceSnapIn)?.Pause(); }
            catch (Exception exception) { ReportException(application, exception, Resources.Strings.FailedToPauseSnapIn); }
        }

        #endregion Pause method

        #region Resume method

        //  -------------
        //  Resume method
        //  -------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal void Resume(IApplication application)
        {
            try { (Instance as IServiceSnapIn)?.Resume(); }
            catch (Exception exception) { ReportException(application, exception, Resources.Strings.FailedToResumeSnapIn); }
        }

        #endregion Resume method

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
                ReportException(application, exception, Resources.Strings.FailedToDisconnectSnapIn);
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
                    ReportException(application, exception, Resources.Strings.FailedToDisconnectSnapIn);
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
                    if (Instance is IDisposable disposable)
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

        internal bool IsEqualInstance(ISnapIn snapIn) { return Instance == snapIn; }

        #endregion IsEqualInstance method

        #endregion internal methods

        #region private methods

        //  ----------------------
        //  ReportException method
        //  ----------------------

        private void ReportException(IApplication application, Exception exception, string message)
        {
            Exception = exception;

            string description = Instance == null ? type == null ? typeName : type.FullName : Instance.ToString();
            var exceptionMessage = string.Format(CultureInfo.CurrentCulture, message, description, exception.Message);

            if (application != null) application.ReportException(new SnapInException(Instance, exceptionMessage, exception));

            Debug.WriteLine(exceptionMessage);
        }

        #endregion private methods
    }

    #endregion SnapInCollectionItem class

    #region SnapInException class

    //  ---------------------
    //  SnapInException class
    //  ---------------------

    /// <summary>
    /// Represents the exception that is thrown when the <see cref="SnapInHost{TActivator}"/>
    /// fails when calling snap-in methods.
    /// </summary>
    /// <seealso cref="Exception" />

#if !WINDOWS_UWP
    [Serializable]
#endif
    public class SnapInException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInException"/> class.
        /// </summary>

        public SnapInException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public SnapInException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>

        public SnapInException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInException"/> class.
        /// </summary>
        /// <param name="snapIn">The snap-in that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>

        public SnapInException(ISnapIn snapIn, string message, Exception innerException) : base(message, innerException) { SnapIn = snapIn; }

#if !WINDOWS_UWP

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"></see>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see>
        /// that contains contextual information about the source or destination.
        /// </param>

        protected SnapInException(SerializationInfo info, StreamingContext context) : base(info, context) { }

#endif // !WINDOWS_UWP

        #endregion construction

        #region properties

        //  ---------------
        //  SnapIn property
        //  ---------------

        /// <summary>
        /// Gets the snap-in that caused the exception.
        /// </summary>
        /// <value>
        /// The snap-in that caused the exception.
        /// </value>

        public ISnapIn SnapIn { get; }

        #endregion properties

        #region overrides

#if !WINDOWS_UWP

        //  --------------------
        //  GetObjectData method
        //  --------------------

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

#endif // !WINDOWS_UWP

        #endregion overrides
    }

    #endregion SnapInException class

    #region SnapInHostException<TActivator> class

    //  -------------------------------------
    //  SnapInHostException<TActivator> class
    //  -------------------------------------

    /// <summary>
    /// Represents an exception that is thrown by a <see cref="SnapInHost{TActivator}" />
    /// </summary>
    /// <typeparam name="TActivator">The type of the snap-in activator.</typeparam>
    /// <seealso cref="Exception" />

#if !WINDOWS_UWP
    [Serializable]
#endif
    public abstract class SnapInHostException<TActivator> : Exception where TActivator : SnapInActivator, new()
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal SnapInHostException() { }

        internal SnapInHostException(string message) : base(message) { }

        internal SnapInHostException(string message, Exception innerException) : base(message, innerException) { }

#if !WINDOWS_UWP

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapInHostException{TActivator}"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"></see>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see>
        /// that contains contextual information about the source or destination.
        /// </param>

        internal protected SnapInHostException(SerializationInfo info, StreamingContext context) : base(info, context) { }

#endif // !WINDOWS_UWP

        internal SnapInHostException(SnapInHost<TActivator> host, string message, Exception innerException) : base(message, innerException) { Host = host; }

        #endregion construction

        #region properties

        //  -------------
        //  Host property
        //  -------------

        /// <summary>
        /// Gets the <see cref="SnapInHost{TActivator}"/> that threw the exception.
        /// </summary>
        /// <value>
        /// The <see cref="SnapInHost{TActivator}"/> that threw the exception.
        /// </value>

        public SnapInHost<TActivator> Host { get; }

        #endregion properties

        #region overrides

#if !WINDOWS_UWP

        //  --------------------
        //  GetObjectData method
        //  --------------------

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

#endif // !WINDOWS_UWP

        #endregion overrides
    }

    #endregion SnapInHostException<TActivator> class

    #region ExtensionException<TActivator> class

    //  ------------------------------------
    //  ExtensionException<TActivator> class
    //  ------------------------------------

    /// <summary>
    /// Represents an exception that was caused by an application extension.
    /// </summary>
    /// <typeparam name="TActivator">The type of the activator.</typeparam>
    /// <seealso cref="SnapInHostException{TActivator}" />
    /// <seealso cref="IApplicationExtension"/>

#if !WINDOWS_UWP
    [Serializable]
#endif
    public class ExtensionException<TActivator> : SnapInHostException<TActivator> where TActivator : SnapInActivator, new()
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException{TActivator}"/> class.
        /// </summary>

        public ExtensionException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException{TActivator}"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public ExtensionException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException{TActivator}"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>

        public ExtensionException(string message, Exception innerException) : base(message, innerException) { }

#if !WINDOWS_UWP
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException{TActivator}"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"></see>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see>
        /// that contains contextual information about the source or destination.
        /// </param>

        protected ExtensionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

#endif // !WINDOWS_UWP

        internal ExtensionException(
            SnapInHost<TActivator> host,
            IExtension<IApplication> extension,
            string message,
            Exception innerException) :
            base(host, message, innerException)
        { Extension = extension; }

        #endregion construction

        #region properties

        //  ------------------
        //  Extension property
        //  ------------------

        /// <summary>
        /// Gets the extension that caused the exception.
        /// </summary>
        /// <value>
        /// The extension that caused the exception.
        /// </value>

        public IExtension<IApplication> Extension { get; }

        #endregion properties

        #region overrides

#if !WINDOWS_UWP

        //  --------------------
        //  GetObjectData method
        //  --------------------

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

#endif // !WINDOWS_UWP

        #endregion overrides
    }

    #endregion ExtensionException<TActivator> class
}

// eof "SnapInHost.cs"
