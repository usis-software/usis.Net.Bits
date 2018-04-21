//
//  @(#) Segment.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using usis.FinTS.DataElementGroups;

namespace usis.FinTS.Base
{
    //  -------------
    //  Segment class
    //  -------------

    internal abstract class Segment
    {
        #region fields

        private List<ISegmentElement> items = new List<ISegmentElement>();

        #endregion fields

        #region properties

        //  ---------------
        //  Header property
        //  ---------------

        private SegmentHeader Header => items.First() as SegmentHeader;

        //  -------------------
        //  Identifier property
        //  -------------------

        internal string Identifier { set => Header.Identifier = value; }

        //  ----------------
        //  Version property
        //  ----------------

        internal int Version { set => Header.Version = value; }

        //  ---------------
        //  Number property
        //  ---------------

        internal int Number { set => Header.Number = value; }

        //  ------------------------
        //  IsMessageHeader property
        //  ------------------------

        internal bool IsMessageHeader => HasIdentifier(SegmentIdentifiers.MessageHeader);

        //  ------------------------
        //  IsMessageFooter property
        //  ------------------------

        internal bool IsMessageFooter => HasIdentifier(SegmentIdentifiers.MessageFooter);

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal protected Segment()
        {
            AddElements(new SegmentHeader());
        }

        internal protected Segment(string identifier, int version)
        {
            AddElements(new SegmentHeader(identifier, version));
        }

        #endregion construction

        #region methods

        //  ------------------
        //  AddElements method
        //  ------------------

        protected void AddElements(params ISegmentElement[] elements)
        {
            items.AddRange(elements);
        }

        //  ----------------
        //  Serialize method
        //  ----------------

        internal void Serialize(StreamWriter writer)
        {
            items.First().Serialize(writer);
            foreach (var element in items.Skip(1))
            {
                writer.Write(Constants.DataElementSeparatorCharacter);
                element.Serialize(writer);
            }
            writer.Write(Constants.SegmentEndCharacter);
        }

        //  ------------------
        //  Deserialize method
        //  ------------------

        internal Segment Deserialize(StreamReader reader)
        {
            var last = items.LastOrDefault();
            foreach (var item in items)
            {
                var terminator = item.Equals(last) ? Constants.SegmentEndCharacter : Constants.DataElementSeparatorCharacter;
                item.Deserialize(reader, terminator, false);
            }
            return this;
        }

        //  --------------------
        //  HasIdentifier method
        //  --------------------

        internal bool HasIdentifier(string identifier)
        {
            return identifier.Equals(Header.Identifier, StringComparison.Ordinal);
        }

        #endregion methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString()
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (var writer = new StreamWriter(stream, System.Text.Encoding.Unicode))
                {
                    stream = null;
                    Serialize(writer);
                    writer.Flush();
                    return System.Text.Encoding.Unicode.GetString((writer.BaseStream as MemoryStream).ToArray());
                }
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        #endregion overrides
    }

    #region ISegmentElement interface

    //  -------------------------
    //  ISegmentElement interface
    //  -------------------------

    internal interface ISegmentElement
    {
        //  ----------------
        //  Serialize method
        //  ----------------

        void Serialize(StreamWriter writer);

        //  ------------------
        //  Deserialize method
        //  ------------------

        bool Deserialize(StreamReader reader, char terminator, bool optional);
    }

    #endregion ISegmentElement interface
}

// eof "Segment.cs"
