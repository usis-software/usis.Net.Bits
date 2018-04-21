//
//  @(#) RfcServerApplicationExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using usis.Framework;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  -----------------------------------
    //  RfcServerApplicationExtension class
    //  -----------------------------------

    /// <summary>
    /// Provides an application extension to register RFC handler classes
    /// and start RFC servers.
    /// </summary>
    /// <seealso cref="IApplicationExtension"/>

    internal class RfcServerApplicationExtension : IApplicationExtension
    {
        #region fields

        private Dictionary<string, List<Type>> registeredHandlers;
        private Dictionary<string, RfcServer> servers;

        #endregion fields

        #region IApplicationExtension methods

        //  -------------
        //  Attach method
        //  -------------

        /// <summary>
        /// Enables the extension object to find out when it has been aggregated. Called when
        /// the extension is added to the <seealso cref="System.ServiceModel.IExtensibleObject{T}.Extensions"/>
        /// property.
        /// </summary>
        /// <param name="owner">
        /// The extensible object that aggregates this extension.
        /// </param>

        public void Attach(IApplication owner)
        {
            registeredHandlers = new Dictionary<string, List<Type>>(StringComparer.Ordinal);
        }

        //  -------------
        //  Detach method
        //  -------------

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated. Called when an
        /// extension is removed from the <seealso cref="System.ServiceModel.IExtensibleObject{T}.Extensions"/>
        /// property.
        /// </summary>
        /// <param name="owner">
        /// The extensible object that aggregates this extension.
        /// </param>

        public void Detach(IApplication owner)
        {
            foreach (var server in servers.Values)
            {
                server.Shutdown(false);
            }
        }

        //  ------------
        //  Start method
        //  ------------

        /// <summary>
        /// Allows the extension to intialize on startup.
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>
        /// <param name="owner">
        /// The extensible object that aggregates this extension.
        /// </param>

        public void Start(IApplication owner)
        {
            if (registeredHandlers == null) throw new InvalidOperationException();
            servers = new Dictionary<string, RfcServer>(StringComparer.Ordinal);
            foreach (var serverName in registeredHandlers.Keys)
            {
                var handlers = registeredHandlers[serverName];
                var server = RfcServerManager.GetServer(serverName, handlers.ToArray());
                servers.Add(serverName, server);
            }
            foreach (var server in servers.Values)
            {
                server.Start();
            }
        }

        #endregion IApplicationExtension methods

        #region public methods

        //  ----------------------
        //  RegisterHandler method
        //  ----------------------

        /// <summary>
        /// Registers the specified type as a handler for a RFC server.
        /// </summary>
        /// <param name="serverName">
        /// The name of the RFC server.
        /// </param>
        /// <param name="handler">
        /// The type that implements the RFC handler methods.
        /// </param>
        /// <exception cref="ArgumentNullOrWhiteSpaceException">
        /// The <i>serverName</i> is <b>null</b>, empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <i>handler</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The extension was not yet attached.
        /// </exception>

        public void RegisterHandler(string serverName, Type handler)
        {
            if (string.IsNullOrWhiteSpace(serverName)) throw new ArgumentNullOrWhiteSpaceException(nameof(serverName));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (registeredHandlers == null) throw new InvalidOperationException();
            if (!registeredHandlers.TryGetValue(serverName, out List<Type> handlers))
            {
                handlers = new List<Type>();
                registeredHandlers.Add(serverName, handlers);
            }
            handlers.Add(handler);
        }

        #endregion public methods
    }
}

// eof "RfcServerApplicationExtension.cs"
