//
//  @(#) SnapIn.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using Microsoft.ManagementConsole;

namespace usis.ApplicationServer.Administration
{
    //  ------------
    //  SnapIn class
    //  ------------

    [SnapInSettings(
        "7A7E1B13-7471-411F-BE3E-8B7DF465EE73",
         DisplayName = "usis Application Server Administration",
         Description = "The usis Application Server Administration snap-in allows you to configure, monitor and manage application servers.",
         Vendor = "usis GmbH")]
    public sealed class SnapIn : Microsoft.ManagementConsole.SnapIn, IDisposable
    {
        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        public SnapIn()
        {
            SmallImages.Add(Images16x16.window_gear);
            SmallImages.Add(Images16x16.window_gear_stop);
            SmallImages.Add(Images16x16.window_warning);
            SmallImages.Add(Images16x16.window);

            LargeImages.Add(Images32x32.window_gear);
            LargeImages.Add(Images32x32.window_gear_stop);
            LargeImages.Add(Images32x32.window_warning);
            LargeImages.Add(Images32x32.window);
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose() { if (model != null) { model.Dispose(); model = null; } }

        #endregion construction/destruction

        #region properties

        //  --------------------
        //  ServerModel property
        //  --------------------

        private ServerModel model = new ServerModel();

        internal ServerModel ServerModel => model;

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize()
        {
            RootNode = new ServerNode();
        }

        //  -----------------
        //  OnShutdown method
        //  -----------------

        protected override void OnShutdown(AsyncStatus status) { Dispose(); }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
