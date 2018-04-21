//
//  @(#) CsvRecord.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace usis.Platform
{
    //  ---------------
    //  CsvRecord class
    //  ---------------

    /// <summary>
    /// Represents one line in a comma-separated values (CSV) file.
    /// </summary>

    [Obsolete("Use CsvHelper instead.")]
    public class CsvRecord
    {
        #region properties

        #region private properties

        private CsvMetadata Metadata { get; set; }

        #endregion private properties

        //  ---------------
        //  Fields property
        //  ---------------

        /// <summary>
        /// Gets the fields in a record of a CSV file.
        /// </summary>
        /// <value>
        /// The fields in a record of a CSV file.
        /// </value>

        public IReadOnlyCollection<object> Fields { get; private set; }

        //  -------
        //  Indexer
        //  -------

        /// <summary>
        /// Gets the value of the column with the specified key.
        /// </summary>
        /// <value>
        /// The value of the column as an <see cref="object"/>.
        /// </value>
        /// <param name="key">The key of the column.</param>

        public object this[string key]
        {
            get
            {
                if (Metadata != null)
                {
                    int index = -1;
                    if (Metadata.TryGetValue(key, out index))
                    {
                        if (index >= 0 && index < Fields.Count)
                        {
                            return Fields.ElementAt(index);
                        }
                    }
                }
                return null;
            }
        }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CsvRecord(string line, CsvMetadata metadata, char separator)
        {
            var fields = line.Split(separator);
            Fields = new ReadOnlyCollection<object>(fields);
            Metadata = metadata;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRecord"/> class
        /// with the specified fields.
        /// </summary>
        /// <param name="fields">The fields.</param>

        public CsvRecord(IEnumerable<object> fields)
        {
            Fields = new ReadOnlyCollection<object>(new List<object>(fields));
        }

        #endregion construction
    }
}

// eof "CsvRecord.cs"
