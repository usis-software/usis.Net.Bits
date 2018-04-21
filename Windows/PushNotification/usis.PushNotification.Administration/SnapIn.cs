//
//  @(#) SnapIn.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;

namespace usis.PushNotification.Administration
{
    //  ------------
    //  SnapIn class
    //  ------------

    /// <summary>
    /// Provides a MMC Snap-In to administrate an <i>usis Push Notification Router</i>.
    /// </summary>
    /// <remarks>
    /// Some of the values from the resource DLL specified by the <see cref="SnapInAboutAttribute"/>
    /// are cached in the registry HKEY_CLASSES_ROOT\Local Settings\MuiCache.
    /// </remarks>

    [SnapInSettings(
        "164826E4-0DFE-443C-8AB3-74F99EDF351C",
         DisplayName = "usis Push Notification Router",
         Description = "The usis Push Notification Router snap-in allows you to manage channels for sending push notifications to mobile devices.",
         Vendor = "usis GmbH")]
    [SnapInAbout("usis.PushNotification.Administration.About.dll",
        ApplicationBaseRelative = true,
        DisplayNameId = 101,
        VersionId = 102,
        DescriptionId = 103,
        VendorId = 104,
        IconId = 105,
        SmallFolderBitmapId = 106,
        LargeFolderBitmapId = 107)]
    [SnapInHelpTopic("usisPNRouter.chm", ApplicationBaseRelative = true)]
    [SnapInLinkedHelpTopic("usisPNRouter.chm", ApplicationBaseRelative = true)]
    public sealed class SnapIn : Microsoft.ManagementConsole.SnapIn, IDisposable
    {
        #region image list indexes

        internal const int ImageListIndexGear = (int)ImageListIndex.Gear;
        internal const int ImageListIndexGearError = (int)ImageListIndex.GearError;
        internal const int ImageListIndexGearRun = (int)ImageListIndex.GearRun;
        internal const int ImageListIndexGearStop = (int)ImageListIndex.GearStop;
        internal const int ImageListIndexUsisIcon = (int)ImageListIndex.usisIcon;
        internal const int ImageListIndexStart = (int)ImageListIndex.Start;
        internal const int ImageListIndexStop = (int)ImageListIndex.Stop;
        internal const int ImageListIndexRestart = (int)ImageListIndex.Restart;

        #region ImageListIndex enumeration

        //  --------------------------
        //  ImageListIndex enumeration
        //  --------------------------

        private enum ImageListIndex
        {
            Gear,
            GearError,
            GearRun,
            GearStop,
            iPhone,
            Notification,
            Channel,
            NewChannel,
            CertificateUpload,
            CertificateInstall,
            CertificateUninstall,
            CertificateDelete,
            usisIcon,
            Start,
            Stop,
            Restart
        }

        #endregion ImageListIndex enumeration

        #endregion image list indexes

        #region construction/destruction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapIn"/> class.
        /// </summary>

        public SnapIn()
        {
            LargeImages.Add(Images32x32.Gear);
            LargeImages.Add(Images32x32.GearError);
            LargeImages.Add(Images32x32.GearRun);
            LargeImages.Add(Images32x32.GearStop);
            LargeImages.Add(Images32x32.iPhone);
            LargeImages.Add(Images32x32.Notification);
            LargeImages.Add(Images32x32.Channel);
            LargeImages.Add(Images32x32.NewChannel);
            LargeImages.Add(Images32x32.CertificateUpload);
            LargeImages.Add(Images32x32.CertificateInstall);
            LargeImages.Add(Images32x32.CertificateUninstall);
            LargeImages.Add(Images32x32.CertificateDelete);
            LargeImages.Add(Images32x32.usisIcon);

            SmallImages.Add(Images16x16.Gear);
            SmallImages.Add(Images16x16.GearError);
            SmallImages.Add(Images16x16.GearRun);
            SmallImages.Add(Images16x16.GearStop);
            SmallImages.Add(Images16x16.iPhone);
            SmallImages.Add(Images16x16.Notification);
            SmallImages.Add(Images16x16.Channel);
            SmallImages.Add(Images16x16.NewChannel);
            SmallImages.Add(Images16x16.CertificateUpload);
            SmallImages.Add(Images16x16.CertificateInstall);
            SmallImages.Add(Images16x16.CertificateUninstall);
            SmallImages.Add(Images16x16.CertificateDelete);
            SmallImages.Add(Images16x16.usisIcon);
            SmallImages.Add(Images16x16.Start);
            SmallImages.Add(Images16x16.Stop);
            SmallImages.Add(Images16x16.Restart);

            Router = new Router();
            RootNode = new RouterNode();
        }

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (Router != null) { Router.Dispose(); Router = null; }
        }

        #endregion construction/destruction

        #region properties

        //  ---------------
        //  Router property
        //  ---------------

        internal Router Router { get; private set; }

        #endregion properties

        #region overrides

        //  -----------------
        //  OnShutdown method
        //  -----------------

        protected override void OnShutdown(AsyncStatus status)
        {
            Dispose();
            base.OnShutdown(status);
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
