//
//  @(#) DataExport.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace usis.PushNotification
{
    //  ----------------
    //  DataExport class
    //  ----------------

    internal sealed class DataExport : IDisposable
    {
        #region fields

        private ZipArchive archive;

        #endregion fields

        #region construction/dispose

        //  ------------
        //  construction
        //  ------------

        public DataExport(string path)
        {
            if (File.Exists(path)) File.Delete(path);
            archive = ZipFile.Open(path, ZipArchiveMode.Create);
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            archive.Dispose();
            archive = null;
        }

        #endregion construction/dispose

        #region methods

        //  -----------
        //  Save method
        //  -----------

        internal void Save<T>(IEnumerable<T> items)
        {
            var entry = archive.CreateEntry(DataImport.CsvEntryName<T>());
            using (var stream = entry.Open())
            {
                var writer = new StreamWriter(stream);
                ToCsv(items, writer);
            }
        }

        //  ------------
        //  ToCsv method
        //  ------------

        internal static int ToCsv<T>(IEnumerable<T> items, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                return ToCsv(items, writer);
            }
        }

        internal static int ToCsv<T>(IEnumerable<T> items, TextWriter writer)
        {
            int count = 0;
            var configuration = new CsvHelper.Configuration.CsvConfiguration()
            {
                CultureInfo = CultureInfo.InvariantCulture
            };
            var csv = new CsvHelper.CsvWriter(writer, configuration);
            csv.WriteHeader<T>();
            foreach (var item in items)
            {
                csv.WriteRecord(item);
                count++;
            }
            csv.Dispose();
            return count;
        }

        #endregion methods
    }
}

// eof "DataExport.cs"
