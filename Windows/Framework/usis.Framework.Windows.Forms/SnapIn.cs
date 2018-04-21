//
//  @(#) SnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System.Windows.Forms;

namespace usis.Framework.Windows.Forms
{
    //  ------------
    //  SnapIn class
    //  ------------

    /// <summary>
    /// Provides a base class for snap-in in a Windows Forms application that need to access the applications main form.
    /// </summary>
    /// <seealso cref="Framework.SnapIn" />

    public abstract class SnapIn : Framework.SnapIn
    {
        //  -----------------
        //  MainForm property
        //  -----------------

        /// <summary>
        /// Gets or sets the application's main form.
        /// </summary>
        /// <value>
        /// The main form of the Windows Forms application.
        /// </value>

        protected Form MainForm
        {
            get => Application.With<ApplicationExtension>(true).MainForm;
            set => Application.With<ApplicationExtension>(true).MainForm = value;
        }
    }
}

// eof "SnapIn.cs"
