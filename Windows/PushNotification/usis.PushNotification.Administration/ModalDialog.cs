//
//  @(#) ModalDialog.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Windows.Forms;

namespace usis.Windows.Forms
{
    //  -----------------
    //  ModalDialog class
    //  -----------------

    /// <summary>
    /// Provides a base class for modal dialogs.
    /// </summary>
    /// <seealso cref="Form" />

    public partial class ModalDialog : Form
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalDialog"/> class.
        /// </summary>

        public ModalDialog()
        {
            InitializeComponent();
        }

        #endregion construction
    }
}

// eof "ModalDialog.cs"
