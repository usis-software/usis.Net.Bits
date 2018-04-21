//
//  @(#) SegmentParser.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.IO;
using System.Text;

namespace usis.FinTS.Base
{
    //  -------------------
    //  SegmentParser class
    //  -------------------

    internal sealed class SegmentParser : IDisposable
    {
        #region fields

        private MemoryStream data = new MemoryStream();

        private Encoding encoding = Encoding.GetEncoding(Constants.MessageCodePage);
        private byte end;
        private byte group;
        private byte element;
        private byte escape;

        private bool escaped;

        #endregion fields

        #region properties

        //  --------------
        //  State property
        //  --------------

        public SegmentState State { get; private set; }

        //  -------------------
        //  Identifier property
        //  -------------------

        public string Identifier { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public SegmentParser()
        {
            var bytes = encoding.GetBytes(new char[]
            {
                Constants.SegmentEndCharacter,
                Constants.DataElementGroupSeparatorCharacter,
                Constants.DataElementSeparatorCharacter,
                Constants.EscapeCharacter
            });
            end = bytes[0];
            group = bytes[1];
            element = bytes[2];
            escape = bytes[3];
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (data != null) { data.Dispose(); data = null; }
        }

        #endregion IDisposable implementation

        #region methods

        //  ------------
        //  Parse method
        //  ------------

        internal SegmentState Parse(byte next)
        {
            if (State == SegmentState.Completed) Reset();

            data.WriteByte(next);

            switch (State)
            {
                case SegmentState.Parsing:
                    if (next == end && !escaped)
                    {
                        State = SegmentState.Completed;
                    }
                    escaped = ((next == escape) && !escaped);
                    return State;
                case SegmentState.AwaitingIdentifier:
                    if (next == group)
                    {
                        State = SegmentState.Parsing;
                        var bytes = data.ToArray();
                        Identifier = encoding.GetString(bytes, 0, bytes.Length - 1);
                        return State;
                    }
                    else if (next != end && next != element)
                    {
                        if (data.Length > 6) State = SegmentState.Error;
                        return State;
                    }
                    break;
                case SegmentState.Completed:
                case SegmentState.Initial:
                    if (next != end && next != group)
                    {
                        State = SegmentState.AwaitingIdentifier;
                        Identifier = null;
                        return State;
                    }
                    break;
                case SegmentState.Error:
                    break;
                default:
                    break;
            }
            return State = SegmentState.Error;
        }

        //  ----------------------
        //  GetStreamReader method
        //  ----------------------

        internal StreamReader GetStreamReader()
        {
            if (State != SegmentState.Completed) throw new InvalidOperationException();

            data.Seek(0, SeekOrigin.Begin);
            State = SegmentState.Initial;
            Identifier = null;

            var reader = new StreamReader(data, Encoding.GetEncoding(Constants.MessageCodePage));
            data = new MemoryStream();
            return reader;
        }

        //  ------------
        //  Reset method
        //  ------------

        internal void Reset()
        {
            State = SegmentState.Initial;
            Identifier = null;
            data.SetLength(0);
        }

        #endregion methods
    }

    #region SegmentState enumeration

    //  ------------------------
    //  SegmentState enumeration
    //  ------------------------

    internal enum SegmentState
    {
        Initial,
        AwaitingIdentifier,
        Parsing,
        Completed,
        Error
    }

    #endregion SegmentState enumeration
}

// eof "SegmentParser.cs"
