using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using usis.Platform;

namespace Playground
{
    internal static class Compression
    {
        internal static void Main()
        {
            using (var zipArchive = ZipFile.OpenRead("Z:\\usis\\Downloads\\fints_institute.zip"))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    var extension = Path.GetExtension(entry.Name);
                    if (".csv".Equals(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        using (var temp = entry.ExtractToTemporaryFile())
                        {
                            //using (var reader = new CsvReader(temp.Path, CsvReaderOptions.HeaderRecord))
                            //{
                            //    foreach (var record in reader.Records)
                            //    {
                            //        var name  = record["Institut"] as string;
                            //        if (name != null && name.Contains("Stuttgart"))
                            //        {
                            //            Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                            //                "BLZ: {0} - {1}", record["BLZ"], record["Institut"]));
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey(false);
        }
    }

    public static class ZipArchiveEntryExtension
    {
        public static TemporaryFile ExtractToTemporaryFile(this ZipArchiveEntry entry)
        {
            //TemporaryFile temp = null;
            TemporaryFile file = null;

            //try
            //{
            //    temp = TemporaryFile.Create();
            //    entry.ExtractToFile(temp.Path, true);
            //    file = temp;
            //    temp = null;
            //}
            //finally
            //{
            //    if (temp != null)
            //    {
            //        temp.Dispose();
            //    }
            //}
            try
            {
                file = TemporaryFile.Create();
                entry.ExtractToFile(file.Path, true);
                //file = temp;
                //temp = null;
            }
            catch (Exception)
            {
                if (file != null)
                {
                    file.Dispose();
                }
                throw;
            }
            return file;
        }
    }

    public sealed class TemporaryFile : IDisposable
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
