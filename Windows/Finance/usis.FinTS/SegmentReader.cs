//
//  @(#) SegmentReader.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;

namespace usis.FinTS.Base
{
    //  --------------------
    //  SegmentReader method
    //  --------------------

    internal abstract class SegmentReader : IDisposable
    {
        #region fields

        private SegmentParser parser = new SegmentParser();

        #endregion fields

        #region properties

        //  ---------------
        //  Stream property
        //  ---------------

        private Stream Stream { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal SegmentReader(Stream stream)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (parser != null) { parser.Dispose(); parser = null; }
        }

        #endregion IDisposable implementation

        #region methods

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads the next segment from the input stream.
        /// </summary>
        /// <returns>The segment readed.</returns>
        /// <remarks>
        /// This method returns <c>null</c> at the end of the input stream
        /// (when the underlying network connection was closed).
        /// </remarks>

        internal Segment Read()
        {
            int c;
            while ((c = Stream.ReadByte()) != -1)
            {
                if (parser.Parse(Convert.ToByte(c)) == SegmentState.Completed)
                {
                    // create and return segement

                    var identifier = parser.Identifier;
                    using (var reader = parser.GetStreamReader())
                    {
                        return CreateSegment(identifier, reader);
                    }
                }
                else if (parser.State == SegmentState.Error)
                {
                    parser.Reset(); // ignore syntax errors

                    Trace.WriteLine("FinTS segment syntax error ignored by reader.");
                }
            }
            return null;
        }

        //  --------------------
        //  CreateSegment method
        //  --------------------

        private Segment CreateSegment(string identifier, StreamReader reader)
        {
            return CreateSegment(identifier).Deserialize(reader);
        }

        internal abstract Segment CreateSegment(string identifier);

        #endregion methods
    }

    #region CustomerSegmentReader class

    //  ---------------------------
    //  CustomerSegmentReader class
    //  ---------------------------

    internal sealed class CustomerSegmentReader : SegmentReader
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CustomerSegmentReader(Stream stream) : base(stream) { }

        #endregion construction

        #region overrides

        //  --------------------
        //  CreateSegment method
        //  --------------------

        internal override Segment CreateSegment(string identifier) { return SegmentFactory.CreateCustomerSegment(identifier); }

        #endregion overrides
    }

    #endregion CustomerSegmentReader class

    #region BankSegmentReader class

    //  -----------------------
    //  BankSegmentReader class
    //  -----------------------

    internal class BankSegmentReader : SegmentReader
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BankSegmentReader(Stream stream) : base(stream) { }

        #endregion construction

        #region overrides

        //  --------------------
        //  CreateSegment method
        //  --------------------

        internal override Segment CreateSegment(string identifier) { return SegmentFactory.CreateBankSegment(identifier); }

        #endregion overrides
    }

    #endregion BankSegmentReader class
}

// eof "SegmentReader.cs"
