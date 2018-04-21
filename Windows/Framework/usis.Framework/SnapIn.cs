//
//  @(#) SnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;

namespace usis.Framework
{
    //  ------------
    //  SnapIn class
    //  ------------

    /// <summary>
    /// Provides a base class that implements the <see cref="ISnapIn"/>
    /// interface.
    /// </summary>

    public class SnapIn : ISnapIn
    {
        #region Application property

        //  --------------------
        //  Application property
        //  --------------------

        /// <summary>
        /// Gets the application that connected this snap-in.
        /// </summary>

        public IApplication Application { get; private set; }

        #endregion Application property

        #region events

        //  ----------------
        //  Connecting event
        //  ----------------

        /// <summary>
        /// Occurs when the snap-in is about to be connected by an application.
        /// </summary>

        public event EventHandler<CancelEventArgs> Connecting;

        //  ---------------
        //  Connected event
        //  ---------------

        /// <summary>
        /// Occurs when the snap-in was connected by an application.
        /// </summary>

        public event EventHandler Connected;

        //  -------------------
        //  Disconnecting event
        //  -------------------

        /// <summary>
        /// Occurs when the snap-in is about to be disconnected by an application.
        /// </summary>

        public event EventHandler<CancelEventArgs> Disconnecting;

        //  ------------------
        //  Disconnected event
        //  ------------------

        /// <summary>
        /// Occurs when the snap-in was disconnected by an application.
        /// </summary>

        public event EventHandler Disconnected;

        #endregion events

        #region virtual methods

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="Connecting"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="CancelEventArgs"/> object that contains the event data.
        /// </param>

        protected virtual void OnConnecting(CancelEventArgs e)
        {
            Connecting?.Invoke(this, e);
        }

        //  ------------------
        //  OnConnected method
        //  ------------------

        /// <summary>
        /// Raises the <see cref="Connected"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="EventArgs"/> object that contains the event data.
        /// </param>

        protected virtual void OnConnected(EventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        //  ----------------------
        //  OnDisconnecting method
        //  ----------------------

        /// <summary>
        /// Raises the <see cref="Disconnecting"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="CancelEventArgs"/> object that contains the event data.
        /// </param>

        protected virtual void OnDisconnecting(CancelEventArgs e)
        {
            Disconnecting?.Invoke(this, e);
        }

        //  ---------------------
        //  OnDisconnected method
        //  ---------------------

        /// <summary>
        /// Raises the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="EventArgs"/> object that contains the event data.
        /// </param>

        protected virtual void OnDisconnected(EventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        #endregion virtual methods

        #region ISnapIn implementation

        //  -------------------
        //  OnConnection method
        //  -------------------

        /// <summary>
        /// An object that implements <see cref="IApplication" /> calls this method during startup -
        /// after an instance of the implementing class has been created.
        /// </summary>
        /// <param name="application">
        /// The application that connects the snap-in.
        /// </param>
        /// <returns>
        /// <b>true</b> if the snap-in is connected; otherwise, <b>false</b>.
        /// </returns>

        public bool OnConnection(IApplication application)
        {
            Application = application;

            var e = new CancelEventArgs();
            OnConnecting(e);

            if (!e.Cancel) OnConnected(EventArgs.Empty);

            return !e.Cancel;
        }

        //  --------------------
        //  CanDisconnect method
        //  --------------------

        /// <summary>
        /// Called to determine whether the implementing snap-in
        /// is ready to be disconnnected.
        /// </summary>
        /// <returns>
        /// <b>true</b> when the snap-in is ready to be disconnected;
        /// otherwise, <b>false</b>.
        /// </returns>

        public bool CanDisconnect()
        {
            var e = new CancelEventArgs();
            OnDisconnecting(e);

            return !e.Cancel;
        }

        //  ----------------------
        //  OnDisconnection method
        //  ----------------------

        /// <summary>
        /// Called to disconnect the implementing snap-in.
        /// </summary>

        public void OnDisconnection()
        {
            OnDisconnected(EventArgs.Empty);

            Application = null;
        }

        #endregion ISnapIn implementation
    }
}

// eof "SnapIn.cs"
