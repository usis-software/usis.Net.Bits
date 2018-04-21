//
//  @(#) DataSourcePage.cs
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
    //  --------------------
    //  DataSourcePage class
    //  --------------------

    internal sealed class DataSourcePage : WizardPage<DataSourcePageControl, Model>
    {
        #region overrides

        //  ---------------------
        //  InitializeView method
        //  ---------------------

        protected override void InitializeView(DataSourcePageControl view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            base.InitializeView(view);

            view.SelectedDataSourceChanged += (sender, e) => Dialog.UpdateButtons();
        }

        //  ----------------------
        //  CanGoToNextPage method
        //  ----------------------

        public override bool CanGoToNextPage()
        {
            return View.SelectedDataSource != null && base.CanGoToNextPage();
        }

        //  -------------
        //  OnNext method
        //  -------------

        public override void OnNext(CancelEventArgs e)
        {
            // set selected data source
            Model.SelectDataSource(View.SelectedDataSource);

            base.OnNext(e);
        }

        #endregion overrides
    }
}

// eof "DataSourcePage.cs"
