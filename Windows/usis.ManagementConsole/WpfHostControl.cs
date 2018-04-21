//
//  @(#) WpfHostControl.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System.Windows;
using System.Windows.Forms.Integration;
using Microsoft.ManagementConsole;

namespace usis.ManagementConsole
{
    //  --------------------
    //  WpfHostControl class
    //  --------------------

    /// <summary>
    /// Provides a form view control that can be used to host a WPF control.
    /// </summary>
    /// <typeparam name="TControl">The type of the WPF control.</typeparam>
    /// <typeparam name="TSnapIn">The type of the snap-in.</typeparam>
    /// <seealso cref="ManagementConsole.FormViewControl{TSnapIn}" />

    public class WpfHostControl<TControl, TSnapIn> : FormViewControl<TSnapIn>
        where TSnapIn : NamespaceSnapInBase
        where TControl : UIElement, new()
    {
        #region fields

        private ElementHost elementHost;
        private UIElement hostedControl;

        #endregion fields

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        /// <summary>
        /// Called when the control is initialized.
        /// </summary>

        protected override void OnInitialize()
        {
            elementHost = new ElementHost();
            hostedControl = new TControl();

            SuspendLayout();
            elementHost.Name = nameof(elementHost);
            elementHost.Location = new System.Drawing.Point(0, 0);
            elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            elementHost.TabIndex = 0;
            elementHost.Child = hostedControl;
            Controls.Add(elementHost);
            Name = nameof(WpfHostControl<TControl, TSnapIn>);
            ResumeLayout();
        }

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (elementHost != null)
                {
                    elementHost.Dispose();
                    elementHost = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion overrides
    }
}

// eof "WpfHostControl.cs"
