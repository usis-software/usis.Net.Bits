//
//  @(#) Constants.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Globalization;

namespace usis.FinTS
{
    //  ---------------
    //  Constants class
    //  ---------------

    internal static class Constants
    {
        internal const string AnonymousCustomerId = "9999999999";
        internal const string AnonymousCustomerSystemId = "0";
        internal const string AnonymousCustomerSystemStatus = "0";

        internal const string CancellationDialogId = "unbekannt";

        internal const int MessageCodePage = 28591;

        internal const char DataElementSeparatorCharacter = '+';
        internal const char DataElementGroupSeparatorCharacter = ':';
        internal const char SegmentEndCharacter = '\'';
        internal const char EscapeCharacter = '?';

        internal static string DataElementSeparator = string.Format(CultureInfo.InvariantCulture, "{0}", DataElementSeparatorCharacter);
        internal static string DataElementGroupSeparator = string.Format(CultureInfo.InvariantCulture, "{0}", DataElementGroupSeparatorCharacter);
        internal static string SegmentEnd = string.Format(CultureInfo.InvariantCulture, "{0}", SegmentEndCharacter);
        internal static string Escape =  string.Format(CultureInfo.InvariantCulture, "{0}", EscapeCharacter);

        internal static string EscapedDataElementSeparator = string.Format(CultureInfo.InvariantCulture, "{0}{1}", EscapeCharacter, DataElementSeparatorCharacter);
        internal static string EscapedDataElementGroupSeparator = string.Format(CultureInfo.InvariantCulture, "{0}{1}", EscapeCharacter, DataElementGroupSeparatorCharacter);
        internal static string EscapedSegmentEnd = string.Format(CultureInfo.InvariantCulture, "{0}{1}", EscapeCharacter, SegmentEndCharacter);
        internal static string EscapedEscape = string.Format(CultureInfo.InvariantCulture, "{0}{1}", EscapeCharacter, EscapeCharacter);
    }

    //  ------------------------
    //  SegmentIdentifiers class
    //  ------------------------

    internal static class SegmentIdentifiers
    {
        internal const string MessageHeader = "HNHBK";
        internal const string MessageFooter = "HNHBS";
        internal const string MessageFeedback = "HIRMG";

        internal const string SignatureHeader = "HNSHK";
        internal const string SignatureFooter = "HNSHA";

        internal const string Identification = "HKIDN";
        internal const string ProcessingPreparation = "HKVVB";

        internal const string DialogEnd = "HKEND";
    }
}

// eof "Constants.cs"
