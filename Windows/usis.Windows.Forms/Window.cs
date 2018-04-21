//
//  @(#) Window.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using usis.Platform;

namespace usis.Windows.Forms
{
    #region IWindow interface

    //  -----------------
    //  IWindow interface
    //  -----------------

    /// <summary>
    /// Defines the members a form must implement to benefit from extensions provided by the usis.Windows.Forms namespace.
    /// </summary>

    public interface IWindow
    {
        //  ---------------------
        //  LastLocation property
        //  ---------------------

        /// <summary>
        /// Gets the last location of the window.
        /// </summary>
        /// <value>
        /// The last location of the window.
        /// </value>

        Point LastLocation { get; }

        //  -----------------
        //  LastSize property
        //  -----------------

        /// <summary>
        /// Gets the last size of the window.
        /// </summary>
        /// <value>
        /// The last size of the window.
        /// </value>

        Size LastSize { get; }

        //  -------------------
        //  SaveSettings method
        //  -------------------

        /// <summary>
        /// Allows the implementing window to save its settings to the specified value storage.
        /// </summary>
        /// <param name="storage">A value storage to save the settings.</param>

        void SaveSettings(IValueStorage storage);

        //  ----------------------
        //  RestoreSettings method
        //  ----------------------

        /// <summary>
        /// Allows the implementing window to restore its settings from the specified value storage.
        /// Restores the settings.
        /// </summary>
        /// <param name="storage">A value storage to restore the settings.</param>

        void RestoreSettings(IValueStorage storage);
    }

    #endregion IWindow interface

    #region Window class

    //  ------------
    //  Window class
    //  ------------

    /// <summary>
    /// A base class for forms that implement the <see cref="IWindow"/> interface.
    /// </summary>
    /// <seealso cref="Form" />
    /// <seealso cref="IWindow" />

    public class Window : Form, IWindow
    {
        #region IWindow implementation

        //  ---------------------
        //  LastLocation property
        //  ---------------------

        /// <summary>
        /// Gets the last location of the window.
        /// </summary>
        /// <value>
        /// The last location of the window.
        /// </value>

        public Point LastLocation { get; private set; }

        //  -----------------
        //  LastSize property
        //  -----------------

        /// <summary>
        /// Gets the last size of the window.
        /// </summary>
        /// <value>
        /// The last size of the window.
        /// </value>

        public Size LastSize { get; private set; }

        //  -------------------
        //  SaveSettings method
        //  -------------------

        /// <summary>
        /// Called by the framework to save settings to the specified value storage.
        /// </summary>
        /// <param name="storage">A value storage to save the settings.</param>

        public void SaveSettings(IValueStorage storage) { OnSaveSettings(storage); }

        //  ----------------------
        //  RestoreSettings method
        //  ----------------------

        /// <summary>
        /// Called by the framework to restore settings from the specified value storage.
        /// Restores the settings.
        /// </summary>
        /// <param name="storage">A value storage to restore the settings.</param>

        public void RestoreSettings(IValueStorage storage) { OnRestoreSettings(storage); }

        #endregion IWindow implementation

        #region overrides

        //  ------------------------
        //  OnLocationChanged method
        //  ------------------------

        /// <summary>
        /// Raises the <see cref="Control.LocationChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>

        protected override void OnLocationChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Normal) LastLocation = Location;

            base.OnLocationChanged(e);

        }

        //  --------------------
        //  OnSizeChanged method
        //  --------------------

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Normal) LastSize = Size;

            base.OnSizeChanged(e);
        }

        #endregion overrides

        #region virtual methods

        //  ---------------------
        //  OnSaveSettings method
        //  ---------------------

        /// <summary>
        /// Allows a derived class to save its settings.
        /// </summary>
        /// <param name="storage">The value storage to save settings.</param>

        protected virtual void OnSaveSettings(IValueStorage storage) { }

        //  ------------------------
        //  OnRestoreSettings method
        //  ------------------------

        /// <summary>
        /// Allows a derived class to restore its settings.
        /// </summary>
        /// <param name="storage">The vaöue storage to restore settings.</param>

        protected virtual void OnRestoreSettings(IValueStorage storage) { }

        #endregion virtual methods
    }

    #endregion Window class
}

// eof "Window.cs"
