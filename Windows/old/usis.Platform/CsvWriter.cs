//
//  @(#) CsvWriter.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 14
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace usis.Platform
{
    //  ---------------
    //  CsvWriter class
    //  ---------------

    /// <summary>
    /// Provides functionality to write data to a comma-separated values (CSV) file.
    /// </summary>

    [Obsolete("Use CsvHelper instead.")]
    public sealed class CsvWriter : IDisposable
    {
        #region fields

        private TextWriter writer;
        private char separator = ',';

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvWriter" /> class for the specified file.
        /// </summary>
        /// <param name="path">The complete file path to write to. <i>path</i> can be a file name.</param>

        public CsvWriter(string path) : this(path, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvWriter" /> class for the specified file
        /// and writes a header record with the specified column names.
        /// </summary>
        /// <param name="path">The complete file path to write to. <i>path</i> can be a file name.</param>
        /// <param name="columnNames">The column names to write as header record.</param>

        public CsvWriter(string path, IEnumerable<string> columnNames)
        {
            writer = new StreamWriter(path);
            if (columnNames != null && columnNames.Count() > 0)
            {
                var header = new CsvRecord(columnNames);
                WriteRecord(header);
            }
        }

        #endregion construction

        #region methods

        //  ------------------
        //  WriteRecord method
        //  ------------------

        /// <summary>
        /// Writes the specified record.
        /// </summary>
        /// <param name="record">The record to write.</param>
        /// <exception cref="ArgumentNullException">The <i>record</i> argument cannot be null.</exception>

        public void WriteRecord(CsvRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var field in record.Fields)
            {
                if (stringBuilder.Length > 0) stringBuilder.Append(separator);
                stringBuilder.Append(StringFromField(field));
            }
            System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
            writer.WriteLine(stringBuilder.ToString());
        }

        //  ----------------------
        //  StringFromField method
        //  ----------------------

        private string StringFromField(object field)
        {
            var s = string.Format(CultureInfo.InvariantCulture, "{0}", field);
            if (s.Contains(separator) || s.Contains('"'))
            {
                s = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", s.Replace("\"", "\"\""));
            }
            return s;
        }

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose() { if (writer != null) writer.Close(); }

        #endregion IDisposable implementation
    }
}

// eof "CsvWriter.cs"
