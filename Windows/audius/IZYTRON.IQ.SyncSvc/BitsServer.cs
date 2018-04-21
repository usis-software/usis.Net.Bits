//
//  @(#) BitsServer.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using usis.Framework;
using usis.Net.Bits;
using usis.Platform;

namespace IZYTRON.IQ
{
    //  ----------------
    //  BitsServer class
    //  ----------------

    internal class BitsServer : BackgroundCopyServer
    {
        #region properties

        //  --------------------
        //  Application property
        //  --------------------

        private IApplication Application { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BitsServer(IApplication application) { Application = application; }

        #endregion construction

        #region overrides

        //  ------------------------
        //  GetDownloadStream method
        //  ------------------------

        protected override Stream GetDownloadStream(Uri url)
        {
            return File.OpenRead(@"Z:\usis\Movies\video.mp4");
        }

        //  --------------------------
        //  CreateUploadSession method
        //  --------------------------

        protected override string CreateUploadSession(Uri url)
        {
            // create sessionId
            var sessionId = Guid.NewGuid().ToString("B");

            // create inbound file
            using (File.Create(GetSessionFilePath(GetInboudDirectoryPath(), sessionId))) { }

            return sessionId;
        }

        //  ----------------------
        //  GetUploadStream method
        //  ----------------------

        protected override Stream GetUploadStream(string sessionId)
        {
            return File.Open(GetSessionFilePath(GetInboudDirectoryPath(), sessionId), FileMode.Append);
        }

        //  -------------------------
        //  CloseUploadSession method
        //  -------------------------

        protected override void CloseUploadSession(string sessionId)
        {
            base.CloseUploadSession(sessionId);
        }

        //  --------------------------
        //  CancelUploadSession method
        //  --------------------------

        protected override void CancelUploadSession(string sessionId)
        {
            File.Delete(GetSessionFilePath(GetInboudDirectoryPath(), sessionId));
        }

        #endregion overrides

        protected override bool StartDownload(Uri url)
        {
            return true;
        }

        protected override void Process(HttpListenerContext context)
        {
            base.Process(context);
        }

        #region internal methods

        //  ----------------------------
        //  CheckInboundDirectory method
        //  ----------------------------

        internal void CheckInboundDirectory()
        {
            var path = GetInboudDirectoryPath();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            Debug.Print("Inboud directory: {0}", path);
        }

        #endregion internal methods

        #region private methods

        //  -------------------------
        //  GetSessionFilePath method
        //  -------------------------

        private static string GetSessionFilePath(string inboundDirectory, string sessionId)
        {
            return Path.ChangeExtension(Path.Combine(inboundDirectory, sessionId), "tmp");
        }

        //  -----------------------------
        //  GetInboudDirectoryPath method
        //  -----------------------------

        private string GetInboudDirectoryPath()
        {
            return Path.Combine(
                Application.Properties.GetString(nameof(SnapIn.ProgramDataDirectory)),
                Constants.InboundDirectory);
        }

        #endregion private methods
    }
}

// eof "BitsServer.cs"
