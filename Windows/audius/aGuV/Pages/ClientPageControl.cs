//
//  @(#) ClientPageControl.cs
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
    //  -----------------------
    //  ClientPageControl class
    //  -----------------------

    internal partial class ClientPageControl : UserControl, IInjectable<Model>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ClientPageControl() { InitializeComponent(); }

        #endregion construction

        #region properties

        //  -----------------------
        //  SelectedClient property
        //  -----------------------

        public Client SelectedClient { get { return listBoxClients.SelectedItem as Client; } }

        #endregion properties

        #region events

        //  ---------------------------
        //  SelectedClientChanged event
        //  ---------------------------

        public event EventHandler SelectedClientChanged;

        #endregion events

        #region methods

        //  -------------
        //  Inject method
        //  -------------

        void IInjectable<Model>.Inject(Model dependency)
        {
            listBoxClients.DataSource = dependency?.Clients;
        }

        //  -----------------------------------------
        //  ListBoxClientsSelectedIndexChanged method
        //  -----------------------------------------

        private void ListBoxClientsSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedClientChanged?.Invoke(this, e);
        }

        #endregion methods
    }
}

// eof "ClientPageControl.cs"
