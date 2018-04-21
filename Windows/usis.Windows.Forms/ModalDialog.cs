//
//  @(#) ModalDialog.cs
//
//  Project:    usis.Windows.Forms
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

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

        #region methods

        //  ---------------------
        //  EnableOkButton method
        //  ---------------------

        /// <summary>
        /// Enables or disabled the OK button control.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> the OK button is enabled.</param>

        protected void EnableOkButton(bool enabled)
        {
            buttonOk.Enabled = enabled;
        }

        #endregion methods
    }
}

// eof "ModalDialog.cs"
