//
//  @(#) SnapIn.cs
//
//  Project:    usis.Windows
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;

namespace usis.Windows.Framework
{
    //  ------------
    //  SnapIn class
    //  ------------

    /// <summary>
    /// Provides a base class for a snap-in
    /// that can access the running WPF application.
    /// </summary>

    public class SnapIn : usis.Framework.SnapIn
    {
        #region properties

        //  ---------------------------
        //  WindowsApplication property
        //  ---------------------------

        /// <summary>
        /// Gets the WPF application.
        /// </summary>
        /// <value>
        /// The WPF application.
        /// </value>

        public System.Windows.Application WindowsApplication
        {
            get;
            private set;
        }

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="usis.Framework.SnapIn.Connecting"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="CancelEventArgs"/> instance containing the event data.
        /// </param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            base.OnConnecting(e);

            if (WindowsApplication == null)
            {
                WindowsApplication = Application.Extensions.Find<ApplicationExtension>()?.WindowsApplication;
                if (WindowsApplication != null) return;
            }
            e.Cancel = true;
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
