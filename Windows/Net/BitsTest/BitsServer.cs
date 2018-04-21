using System;
using System.IO;

namespace BitsTest
{
    internal class BitsServer : usis.Net.Bits.BackgroundCopyServer
    {
        #region properties

        private static string DataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BitsTest");
        private static string DownloadFolder => Path.Combine(DataDirectory, "Downloads");

        #endregion properties

        #region private methods

        private static string FileNameFromUrlQuery(Uri url)
        {
            return url.Query.StartsWith("?", StringComparison.Ordinal) ? url.Query.Remove(0, 1) : null;
        }

        private static string PathFromDownloadUrl(Uri url)
        {
            var fileName = FileNameFromUrlQuery(url);
            return fileName == null ? null : Path.Combine(DownloadFolder, fileName);
        }

        private static string UploadPathFromSessionId(string sessionId)
        {
            return Path.Combine(DataDirectory, "Uploads", sessionId);
        }

        #endregion private methods

        #region overrides

        //  --------------------
        //  StartDownload method
        //  --------------------

        protected override bool StartDownload(Uri url)
        {
            var path = PathFromDownloadUrl(url);
            System.Diagnostics.Debug.Print("download: {0}", path);
            return File.Exists(path);
        }

        //  ------------------------
        //  GetDownloadStream method
        //  ------------------------

        protected override Stream GetDownloadStream(Uri url)
        {
            var path = PathFromDownloadUrl(url);
            return path == null ? null : new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        //  --------------------------
        //  CreateUploadSession method
        //  --------------------------

        protected override string CreateUploadSession(Uri url)
        {
            return FileNameFromUrlQuery(url);
        }

        //  ----------------------
        //  GetUploadStream method
        //  ----------------------

        protected override Stream GetUploadStream(string sessionId)
        {
            return File.Open(UploadPathFromSessionId(sessionId), FileMode.Append);
        }

        //  --------------------------
        //  CancelUploadSession method
        //  --------------------------

        protected override void CancelUploadSession(string sessionId)
        {
            File.Delete(UploadPathFromSessionId(sessionId));
        }

        #endregion overrides
    }
}
