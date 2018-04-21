//
//  @(#) IDocFileReader.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.IO;

namespace usis.Middleware.SAP
{
    //  --------------------
    //  IDocFileReader class
    //  --------------------

    /// <summary>
    /// Provides forward-only, read-only access to the records (segments)
    /// of an intermediate document (IDoc) file.
    /// </summary>

    public sealed class IDocFileReader : IDocReader, IDisposable
    {
        #region fields

        private TextReader reader;
        private IDocControlRecord controlRecord;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocFileReader"/> class
        /// with the specified path.
        /// </summary>
        /// <param name="path">
        /// The path of the file to read.
        /// </param>

        public IDocFileReader(string path)
        {
            reader = new StreamReader(path, System.Text.Encoding.Default);
            var line = reader.ReadLine();
            if (line != null)
            {
                controlRecord = new IDocControlRecord(line);
            }
        }

        #endregion construction

        #region methods

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads the next the segment from the reader.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a segment was read; otherwise <c>false</c>.
        /// </returns>

        public override bool Read()
        {
            do
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    CurrentControlRecord = controlRecord = null;
                    CurrentDataRecord = null;
                    return false;
                }
                else if (line.StartsWith(IDocControlRecord.TableName, StringComparison.Ordinal))
                {
                    controlRecord = new IDocControlRecord(line);
                }
                else
                {
                    CurrentControlRecord = controlRecord;
                    CurrentDataRecord = new IDocDataRecord(line);
                    return true;
                }
            }
            while (true);
        }

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        #endregion methods
    }
}

// eof "IDocFileReader.cs"
