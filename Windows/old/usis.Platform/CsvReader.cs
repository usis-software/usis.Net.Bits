//
//  @(#) CsvReader.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace usis.Platform
{
    #region CsvReaderOptions enumeration

    //  ----------------------------
    //  CsvReaderOptions enumeration
    //  ----------------------------

    /// <summary>
    /// Contains options that describe how a <see cref="CsvReader"/> should operate.
    /// </summary>

    [Obsolete("Use CsvHelper instead.")]
    [Flags]
    public enum CsvReaderOptions
    {
        /// <summary>
        /// No special options are set.
        /// </summary>

        None,

        /// <summary>
        /// The CSV file has header record.
        /// </summary>

        HeaderRecord,

        /// <summary>
        /// The reader should check whether there is a value for each columnn in the header record.
        /// </summary>

        CheckFieldCount
    }

    #endregion CsvReaderOptions enumeration

    #region CsvReader class

    //  ---------------
    //  CsvReader class
    //  ---------------

    /// <summary>
    /// Provides forward-only, read-only access to the records of a comma-separated values (CSV) file.
    /// </summary>

    [Obsolete("Use CsvHelper instead.")]
    public sealed class CsvReader : IDisposable
    {
        #region fields and private properties

        private TextReader reader;

        private StreamReader StreamReader { get { return reader as StreamReader; } }

        private CsvMetadata Metadata { get; set; }

        private CsvReaderOptions Options { get; set; }

        private char separator = ',';

        #endregion fields and private properties

        #region public properties

        //  ----------------
        //  Records property
        //  ----------------

        /// <summary>
        /// Gets an enumerator for the records in the CSV file.
        /// </summary>
        /// <value>
        /// The enumerator for the records in the CSV file.
        /// </value>

        public IEnumerable<CsvRecord> Records
        {
            get
            {
                CsvRecord record = null;
                while ((record = ReadRecord()) != null) yield return record;
            }
        }

        //  ------------------
        //  ColumnNames method
        //  ------------------

        /// <summary>
        /// Gets an enumerator for the column names defined by a header record.
        /// </summary>
        /// <value>
        /// The enumerator for the column names.
        /// </value>

        public IEnumerable<string> ColumnNames
        {
            get
            {
                foreach (var key in Metadata.Keys) yield return key;
            }
        }

        //  ----------------------
        //  CurrentEncoding method
        //  ----------------------

        /// <summary>
        /// Gets the current character encoding that the current <see cref="CsvReader"/> object is using.
        /// </summary>
        /// <value>
        /// The current character encoding used by the current reader.
        /// The value can be different after the first call to the ReadRecord method of <see cref="CsvReader"/>,
        ///  since encoding autodetection is not done until the first call to the ReadRecord method.
        /// </value>

        public Encoding CurrentEncoding
        {
            get
            {
                StreamReader streamReader = StreamReader;
                return streamReader == null ? Encoding.Default : streamReader.CurrentEncoding;
            }
        }

        #endregion public properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="path">
        /// The path of the file to read.
        /// </param>
        /// <param name="options">
        /// The options that describe how the reader should work.
        /// </param>

        public CsvReader(string path, CsvReaderOptions options)
        {
            Options = options;
            reader = new StreamReader(path, Encoding.UTF7);

            if (options.HasFlag(CsvReaderOptions.HeaderRecord))
            {
                var headerRecord = ReadRecord();
                if (headerRecord != null)
                {
                    Metadata = new CsvMetadata(headerRecord);
                }
            }
        }

        #endregion construction

        #region methods

        //  -----------------
        //  ReadRecord method
        //  -----------------

        /// <summary>
        /// Reads the next record from the CSV file.
        /// </summary>
        /// <returns>
        /// The record readed or <b>null</b> if there's no other record.
        /// </returns>

        public CsvRecord ReadRecord()
        {
            CsvRecord record = null;
            do
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    record = new CsvRecord(line, Metadata, separator);
                    if (Options.HasFlag(CsvReaderOptions.CheckFieldCount) && Metadata != null)
                    {
                        if (record.Fields.Count != Metadata.Count) record = null;
                    }
                }
                else break;
            }
            while (record == null);
            return record;
        }

        #endregion methods

        #region IDisposable members

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        ///  releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (reader != null) reader.Close();
        }

        #endregion IDisposable members
    }

    #endregion CsvReader class

    #region CsvMetadata class

    //  -----------------
    //  CsvMetadata class
    //  -----------------

    [Obsolete("Use CsvHelper instead.")]
    internal class CsvMetadata : Dictionary<string, int>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public CsvMetadata(CsvRecord headerRecord)
        {
            for (int i = 0; i < headerRecord.Fields.Count; i++)
            {
                string header = headerRecord.Fields.ElementAt(i) as string;
                if (!string.IsNullOrWhiteSpace(header) && !ContainsKey(header))
                {
                    Add(header, i);
                }
            }
        }

        //public CsvMetadata(IEnumerable<string> columnNames)
        //{
        //    int i = 0;
        //    foreach (var columnName in columnNames)
        //    {
        //        Add(columnName, i++);
        //    }
        //}

        #endregion construction
    }

    #endregion CsvMetadata class
}

// eof "CsvReader.cs"
