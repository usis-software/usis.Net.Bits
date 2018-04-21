//
//  @(#) DataSourcePageControl.cs
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
    //  DataSourcePageControl class
    //  ---------------------------

    internal partial class DataSourcePageControl : UserControl, IInjectable<Model>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public DataSourcePageControl() { InitializeComponent(); }

        #endregion construction

        #region properties

        //  ---------------------------
        //  SelectedDataSource property
        //  ---------------------------

        public DataSource SelectedDataSource { get { return listBoxDataSources.SelectedItem as DataSource; } }

        #endregion properties

        #region events

        //  -------------------------------
        //  SelectedDataSourceChanged event
        //  -------------------------------

        public event EventHandler SelectedDataSourceChanged;

        #endregion events

        #region methods

        //  -------------
        //  Inject method
        //  -------------

        void IInjectable<Model>.Inject(Model dependency)
        {
            listBoxDataSources.DataSource = dependency?.DataSources;
        }

        //  ---------------------------------------------
        //  ListBoxDataSourcesSelectedIndexChanged method
        //  ---------------------------------------------

        private void ListBoxDataSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDataSourceChanged?.Invoke(this, e);
        }

        #endregion methods
    }
}

// eof "DataSourcePageControl.cs"
