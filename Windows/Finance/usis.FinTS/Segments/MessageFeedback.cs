//
//  @(#) MessageFeedback.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;

namespace usis.FinTS.Segments
{
    //  ---------------------
    //  MessageFeedback class
    //  ---------------------

    internal sealed class MessageFeedback : Segment
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public MessageFeedback() : base(SegmentIdentifiers.MessageFeedback, 2) { }

        #endregion construction
    }
}

// eof "MessageFeedback.cs"
