//
//  @(#) MessageParser.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Collections.Generic;

namespace usis.FinTS.Base
{
    //  -------------------
    //  MessageParser class
    //  -------------------

    internal sealed class MessageParser
    {
        #region fields

        private List<Segment> segments = new List<Segment>();

        #endregion fields

        #region properties

        //  --------------
        //  State property
        //  --------------

        private MessageParserState State { get; set; }

        #endregion properties

        #region methods

        //  -----------
        //  Next method
        //  -----------

        internal Message Next(Segment segment)
        {
            Message message = null;
            if (segment.IsMessageHeader)
            {
                if (segments.Count != 0) State = MessageParserState.Error;
            }
            else if (segment.IsMessageFooter)
            {
                if (segments.Count <= 1) State = MessageParserState.Error;
                else State = MessageParserState.Terminated;
            }
            if (State != MessageParserState.Error)
            {
                segments.Add(segment);
            }
            if (State == MessageParserState.Terminated)
            {
                message = MessageFactory.CreateMessage(segments);
            }
            if (State != MessageParserState.Initial)
            {
                segments.Clear();
            }
            return message;
        }

        #endregion methods
    }

    #region MessageParserState enumeration

    //  ------------------------------
    //  MessageParserState enumeration
    //  ------------------------------

    internal enum MessageParserState
    {
        Initial,
        Terminated,
        Error
    }

    #endregion MessageParserState enumeration
}

// eof "MessageParser.cs"
