//
//  @(#) Constants.cs
//
//  Project:    usis.Middleware.SAP
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

#region usis.Platform namespace

namespace usis.Platform
{
    //  ---------------------
    //  StringConstants class
    //  ---------------------

    internal static partial class StringConstants
    {
        public const string Period = ".";
    }

    //  ------------------
    // CharConstants class
    //  ------------------

    internal static partial class CharConstants
    {
        public const char Space = ' ';
        public const char Period = '.';
        public static readonly char[] WhiteSpace = new char[] { ' ', '\t', '\n', '\r' };
    }
}

#endregion usis.Platform namespace

namespace usis.Middleware.SAP
{
    //  --------------------
    //  XmlElementName class
    //  --------------------

    internal static partial class XmlElementName
    {
        public const string IDoc = "idoc";
        public const string Segment = "segment";
        public const string Field = "field";
    }

    //  ----------------------
    //  XmlAttributeName class
    //  ----------------------

    internal static partial class XmlAttributeName
    {
        public const string DocumentType = "documentType";
        public const string Type = "type";
        public const string Name = "name";
        public const string Definition = "definition";
        public const string Length = "length";
        public const string MinOccurs = "minOccurs";
        public const string MaxOccurs = "maxOccurs";
    }
}

// eof "Constants.cs"
