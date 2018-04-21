//
//  @(#) StartViewControl.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System.Globalization;
using System.Windows.Forms;
using usis.ManagementConsole;

namespace usis.PushNotification.Administration
{
    //  ----------------------
    //  StartViewControl class
    //  ----------------------

    public partial class StartViewControl : FormViewControl<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public StartViewControl()
        {
            InitializeComponent();
        }

        #endregion construction

        #region fields

        private string formatRouterStatus;

        #endregion fields

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize()
        {
            base.OnInitialize();

            formatRouterStatus = labelRouterStatus.Text;

            RefreshContent();

            SnapIn.Router.ServiceStatusChanged += (sender, e) => SnapIn.Invoke(new MethodInvoker( delegate { RefreshContent(); }));
        }

        #endregion overrides

        #region methods

        //  ---------------------
        //  RefreshContent method
        //  ---------------------

        private void RefreshContent()
        {
            labelRouterStatus.Text = string.Format(CultureInfo.CurrentCulture, formatRouterStatus, SnapIn.Router.ServiceStatus);

            textBoxProvider.Text = SnapIn.Router.DataSource?.ProviderInvariantName;
            toolTip.SetToolTip(textBoxProvider, textBoxProvider.Text);
            textBoxConnectionString.Text = SnapIn.Router.DataSource?.ConnectionString;
            toolTip.SetToolTip(textBoxConnectionString, textBoxConnectionString.Text);
        }

        #endregion methods
    }
}

// eof "StartViewControl.cs"
