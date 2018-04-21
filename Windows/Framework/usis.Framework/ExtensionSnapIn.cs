//
//  @(#) ExtensionSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.Framework
{
    //  ---------------------------------
    //  ExtensionSnapIn<TExtension> class
    //  ---------------------------------

    /// <summary>
    /// Provides a base class for a snap-in that adds an application extemsion.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <seealso cref="SnapIn" />

    public abstract class ExtensionSnapIn<TExtension> : SnapIn where TExtension : IApplicationExtension, new()
    {
        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            // add extension
            Application.Extensions.Add(new TExtension());

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "ExtensionSnapIn.cs"
