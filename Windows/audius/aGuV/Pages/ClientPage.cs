//
//  @(#) ClientPage.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using System;
using System.ComponentModel;
using usis.Windows.Forms;

namespace audius.GuV.Wizard
{
    //  ----------------
    //  ClientPage class
    //  ----------------

    internal sealed class ClientPage : WizardPage<ClientPageControl, Model>
    {
        #region overrides

        //  ---------------------
        //  InitializeView method
        //  ---------------------

        protected override void InitializeView(ClientPageControl view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            base.InitializeView(view);

            view.SelectedClientChanged += (sender, e) => Dialog.UpdateButtons();
        }

        //  ----------------------
        //  CanGoToNextPage method
        //  ----------------------

        public override bool CanGoToNextPage()
        {
            return View.SelectedClient != null && base.CanGoToNextPage();
        }

        //  -------------
        //  OnNext method
        //  -------------

        public override void OnNext(CancelEventArgs e)
        {
            // set selected client
            Model.SelectClient(View.SelectedClient);

            base.OnNext(e);
        }

        #endregion overrides
    }
}

// eof "ClientPage.cs"
