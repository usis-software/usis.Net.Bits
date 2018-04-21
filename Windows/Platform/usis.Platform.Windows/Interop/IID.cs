//
//  @(#) IID.cs - interface identifiers
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

namespace usis.Platform.Interop
{
    //  ---------
    //  IID class
    //  ---------

    internal static class IID
    {
        public const string IStorage = "0000000b-0000-0000-c000-000000000046";
        public const string IEnumSTATSTG = "0000000d-0000-0000-c000-000000000046";
        public const string IRootStorage = "00000012-0000-0000-c000-000000000046";
        public const string IPropertyStorage = "00000138-0000-0000-c000-000000000046";
        public const string IEnumSTATPROPSTG = "00000139-0000-0000-c000-000000000046";
        public const string IPropertySetStorage = "0000013a-0000-0000-c000-000000000046";
        public const string IEnumSTATPROPSETSTG = "0000013b-0000-0000-c000-000000000046";

        public const string IPropertyPage = "b196b28d-bab4-101a-b69c-00aa00341d07";
        public const string IPropertyPageSite = "b196b28c-bab4-101a-b69c-00aa00341d07";
        public const string ISpecifyPropertyPages = "b196b28b-bab4-101a-b69c-00aa00341d07";

        public const string IInternalDataObject = "0000010e-0000-0000-c000-000000000046";
    }
}

// eof "IID.cs"
