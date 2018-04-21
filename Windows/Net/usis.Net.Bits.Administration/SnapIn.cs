//
//  @(#) SnapIn.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace usis.Net.Bits.Administration
{
    //  ------------
    //  SnapIn class
    //  ------------

    [SnapInSettings(
        "6f713e7f-022f-4f47-a126-29d8f437f2e9",
         DisplayName = "usis BITS Administration",
         Description = "The usis BITS Administration MMC Snap-In allows you to manage the Background Intelligent Transfer Service.",
         Vendor = "usis GmbH")]
    [SnapInAbout("usis.Net.Bits.Administration.About.dll",
        ApplicationBaseRelative = true,
        DisplayNameId = 101,
        VersionId = 102)]
    //[SnapInAbout("usis.Net.Bits.Administration.About.dll",
    //    ApplicationBaseRelative = true,
    //    DisplayNameId = 101,
    //    VersionId = 102,
    //    DescriptionId = 103,
    //    VendorId = 104,
    //    IconId = 105,
    //    SmallFolderBitmapId = 106,
    //    LargeFolderBitmapId = 107)]
    public sealed class SnapIn : Microsoft.ManagementConsole.SnapIn
    {
        #region properties

        //  ----------------
        //  Manager property
        //  ----------------

        internal BackgroundCopyManager Manager { get; private set; }

        internal Settings Settings { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public SnapIn()
        {
            SmallImages.Add(Icons.data_copy);
            SmallImages.Add(Icons.data_down);
            SmallImages.Add(Icons.data_up);
            SmallImages.Add(Icons.document_down);
            SmallImages.Add(Icons.document_up);
            SmallImages.Add(Icons.data_new);
            SmallImages.Add(Icons.data_delete);
            SmallImages.Add(Icons.data_ok);
            SmallImages.Add(Icons.media_pause);
            SmallImages.Add(Icons.media_play);
            SmallImages.Add(Icons.document_add);
            SmallImages.Add(Icons.user1_back);

            LargeImages.Add(Icons.data_copy);
            LargeImages.Add(Icons.data_down);
            LargeImages.Add(Icons.data_up);
            LargeImages.Add(Icons.document_down);
            LargeImages.Add(Icons.document_up);
            LargeImages.Add(Icons.data_new);
            LargeImages.Add(Icons.data_delete);
            LargeImages.Add(Icons.data_ok);
            LargeImages.Add(Icons.media_pause);
            LargeImages.Add(Icons.media_play);
            LargeImages.Add(Icons.document_add);
            LargeImages.Add(Icons.user1_back);

            Manager = BackgroundCopyManager.Connect();
            Settings = new Settings();
        }

        #endregion construction

        #region ImageIndex enumeration

        //  ----------------------
        //  ImageIndex enumeration
        //  ----------------------

        internal enum ImageIndex
        {
            Manager,
            DownloadJob,
            UploadJob,
            DownloadFile,
            UploadFile,
            CreateJob,
            CancelJob,
            CompleteJob,
            SuspendJob,
            ResumeJob,
            AddFile,
            TakeOwner
        }

        #endregion ImageIndex enumeration

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize() { RootNode = new RootNode(); }

        //  -----------------
        //  OnShutdown method
        //  -----------------

        protected override void OnShutdown(AsyncStatus status)
        {
            (RootNode as IDisposable)?.Dispose();
            Manager.Dispose();
        }

        //  -----------------------
        //  OnLoadCustomData method
        //  -----------------------

        protected override void OnLoadCustomData(AsyncStatus status, byte[] persistenceData)
        {
            using (var stream = new MemoryStream(persistenceData))
            {
                if (new BinaryFormatter().Deserialize(stream) is Settings settings) Settings = settings;
            }
        }

        //  -----------------------
        //  OnSaveCustomData method
        //  -----------------------

        protected override byte[] OnSaveCustomData(SyncStatus status)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, Settings);
                return stream.ToArray();
            }
        }

        #endregion overrides

        #region Reconnect method

        //  ----------------
        //  Reconnect method
        //  ----------------

        public void Reconnect()
        {
            Manager.Dispose();
            Manager = BackgroundCopyManager.Connect();
        }

        #endregion Reconnect method
    }

    #region Settings class

    //  --------------
    //  Settings class
    //  --------------

    [Serializable]
    internal sealed class Settings
    {
        public bool ShowAllUsers { get; set; }
    }

    #endregion Settings class
}

// eof "SnapIn.cs"
