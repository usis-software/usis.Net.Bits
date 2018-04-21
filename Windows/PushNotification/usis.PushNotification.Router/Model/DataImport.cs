//
//  @(#) DataImport.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace usis.PushNotification
{
    //  ----------------
    //  DataImport class
    //  ----------------

    internal class DataImport
    {
        #region fields

        private ZipArchive archive;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DataImport(string path)
        {
            archive = ZipFile.OpenRead(path);
        }

        #endregion construction

        #region methods

        //  -----------
        //  Load method
        //  -----------

        internal void Load<T>(IDbSet<T> items) where T : class
        {
            var entry = archive.GetEntry(CsvEntryName<T>());
            if (entry == null) return;
            var configuration = new CsvHelper.Configuration.CsvConfiguration()
            {
                CultureInfo = CultureInfo.InvariantCulture,
                WillThrowOnMissingField = false
            };
            using (var csv = new CsvHelper.CsvReader(new StreamReader(entry.Open()), configuration))
            {
                while (csv.Read())
                {
                    var item = csv.GetRecord<T>();
                    items.Add(item);
                }
            }
            System.Threading.Thread.Sleep(1000);
        }

        #endregion methods

        #region private methods

        //  -------------------
        //  CsvEntryName method
        //  -------------------

        internal static string CsvEntryName<T>()
        {
            return typeof(T).Name + ".csv";
        }

        #endregion private methods
    }
}

// eof "DataImport.cs"
