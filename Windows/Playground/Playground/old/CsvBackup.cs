using System;
using System.IO.Compression;

namespace Playground
{
    internal static class CsvBackup
    {
        internal static void Main()
        {
            //var stream = new System.IO.StreamReader(@"C:\tmp\usisPNRouter.Backup.zip");
            var backup = CsvBackupArchive.Open(@"C:\tmp\usisPNRouter.Backup.zip");
            var reader = backup.OpenReader("Receiver.csv");
            while (reader.Read())
            {
                var record = reader.CurrentRecord;
                //foreach (var field in record)
                //{
                //    Console.Write(field);
                //}
                Console.WriteLine(string.Join(";", record));
            }
            ConsoleTool.PressAnyKey();
        }
    }

    public class CsvBackupArchive
    {
        #region fields

        private ZipArchive archive;

        #endregion fields

        private CsvBackupArchive() { }

        internal static CsvBackupArchive Open(string path)
        {
            return new CsvBackupArchive()
            {
                archive = ZipFile.Open(path, ZipArchiveMode.Read)
            };
        }

        internal CsvHelper.CsvReader OpenReader(string entryName)
        {
            var entry = archive.GetEntry(entryName);
            return new CsvHelper.CsvReader(new System.IO.StreamReader(entry.Open()));
        }
    }

    internal sealed class TemporaryFile : IDisposable
    {
        public string Path
        {
            get;
            private set;
        }

        private TemporaryFile()
        {
            this.Path = System.IO.Path.GetTempFileName();
        }

        public static TemporaryFile Create()
        {
            return new TemporaryFile();
        }

        public void Dispose()
        {
            File.Delete(this.Path);
        }

        public override string ToString()
        {
            return this.Path;
        }

        //public TextReader CreateReader()
        //{
        //    return new StreamReader(this.Path);
        //}
    }
}
