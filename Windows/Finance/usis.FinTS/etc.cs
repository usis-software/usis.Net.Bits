//
//  @(#) etc.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using usis.FinTS.Base;

namespace usis.FinTS
{
    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        //  ---------------------------------------
        //  IEnumerable<Segment>.HasSegments method
        //  ---------------------------------------

        internal static bool HasSegments(this IEnumerable<Segment> segments, params string[] segmentidentifiers)
        {
            foreach (var identifier in segmentidentifiers)
            {
                if (segments.Find(identifier) == null) return false;
            }
            return true;
        }

        //  --------------------------------
        //  IEnumerable<Segment>.Find method
        //  --------------------------------

        private static Segment Find(this IEnumerable<Segment> segments, string identifier)
        {
            return segments.FirstOrDefault(s => s.HasIdentifier(identifier));
        }

        internal static TSegment Find<TSegment>(this IEnumerable<Segment> segments) where TSegment : Segment
        {
            return segments.OfType<TSegment>().FirstOrDefault();
        }

        //  ---------------
        //  AddRange method
        //  ---------------

        internal static void AddRange(this IList<DataElementItem> list, params ISegmentElement[] items)
        {
            foreach (var item in items)
            {
                list.Add(new DataElementItem(item, false));
            }
        }

        //  ---------------------------
        //  IsSeparatorCharacter method
        //  ---------------------------

        internal static bool IsSeparatorCharacter(this char c)
        {
            return
                c == Constants.DataElementSeparatorCharacter ||
                c == Constants.DataElementGroupSeparatorCharacter ||
                c == Constants.SegmentEndCharacter;
        }
    }
}

// eof "etc.cs"
