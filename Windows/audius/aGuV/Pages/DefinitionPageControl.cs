//
//  @(#) DefinitionPageControl.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using System;
using System.Windows.Forms;
using usis.Platform;

namespace audius.GuV.Wizard
{
    //  ---------------------------
    //  DefinitionPageControl class
    //  ---------------------------

    internal partial class DefinitionPageControl : UserControl, IInjectable<Model>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public DefinitionPageControl() { InitializeComponent(); }

        #endregion construction

        #region properties

        //  ---------------------------
        //  SelectedDefinition property
        //  ---------------------------

        public Definition SelectedDefinition { get { return listBoxDefinitions.SelectedItem as Definition; } }

        #endregion properties

        #region events

        //  -------------------------------
        //  SelectedDefinitionChanged event
        //  -------------------------------

        public event EventHandler SelectedDefinitionChanged;

        #endregion events

        #region methods

        //  -------------
        //  Inject method
        //  -------------

        void IInjectable<Model>.Inject(Model dependency)
        {
            listBoxDefinitions.DataSource = dependency?.Definitions;
        }

        //  ---------------------------------------------
        //  ListBoxDefinitionsSelectedIndexChanged method
        //  ---------------------------------------------

        private void ListBoxDefinitionsSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDefinitionChanged?.Invoke(this, e);
        }

        #endregion methods
    }
}

// eof "DefinitionPageControl.cs"
