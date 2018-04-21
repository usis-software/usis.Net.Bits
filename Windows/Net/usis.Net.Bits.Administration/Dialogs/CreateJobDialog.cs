//
//  @(#) CreateJobDialog.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using usis.Windows.Forms;

namespace usis.Net.Bits.Administration
{
    //  ---------------------
    //  CreateJobDialog class
    //  ---------------------

    internal partial class CreateJobDialog : ModalDialog
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CreateJobDialog()
        {
            InitializeComponent();

            comboBoxType.DataSource = Enum.GetValues(typeof(BackgroundCopyJobType));
        }

        #endregion construction

        #region properties

        //  ----------------
        //  JobName property
        //  ----------------

        internal string JobName => textBoxName.Text;

        //  ----------------
        //  JobType property
        //  ----------------

        internal BackgroundCopyJobType JobType => (BackgroundCopyJobType)comboBoxType.SelectedValue;

        #endregion properties
    }
}

// eof "CreateJobDialog.cs"
