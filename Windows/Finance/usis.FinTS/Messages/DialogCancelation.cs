//
//  @(#) DialogCancellation.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using usis.FinTS.Base;

namespace usis.FinTS.Messages
{
    //  ------------------------
    //  DialogCancellation class
    //  ------------------------

    internal sealed class DialogCancellation : BankMessage
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DialogCancellation(IEnumerable<Segment> segments) : base(segments) { }

        internal DialogCancellation(int hbciVersion, int referenceMessageNumber) : base(hbciVersion, referenceMessageNumber)
        {
            AddSegments(new Segments.MessageFeedback());
        }

        #endregion construction
    }
}

// eof "DialogCancellation.cs"
