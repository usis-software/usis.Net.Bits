//
//  @(#) MessageFactory.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using usis.FinTS.Messages;

namespace usis.FinTS.Base
{
    //  --------------------
    //  MessageFactory class
    //  --------------------

    internal static class MessageFactory
    {
        //  --------------------
        //  CreateMessage method
        //  --------------------

        internal static Message CreateMessage(IEnumerable<Segment> segments)
        {
            // check for header and footer
            if (!segments.HasSegments(SegmentIdentifiers.MessageHeader, SegmentIdentifiers.MessageFooter))
            {
                throw new InvalidMessageException(Strings.MessageWithoutHeaderOrFooter, segments);
            }

            //bool signed = segments.HasSegments(SegmentIdentifiers.SignatureHeader, SegmentIdentifiers.SignatureFooter);

            if (segments.HasSegments(SegmentIdentifiers.Identification, SegmentIdentifiers.ProcessingPreparation))
            {
                return new DialogInitialization(segments);
            }
            if (segments.HasSegments(SegmentIdentifiers.DialogEnd))
            {
                return new DialogTermination(segments);
            }
            if (segments.HasSegments(SegmentIdentifiers.MessageFeedback) && segments.Count() == 3)
            {
                return new DialogCancellation(segments);
            }
            return null;
        }
    }
}

// eof "MessageFactory.cs"
