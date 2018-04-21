//
//  @(#) CountryCodeDataElement.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;

namespace usis.FinTS.DataElements
{
    //  ----------------------------
    //  CountryCodeDataElement class
    //  ----------------------------

    internal sealed class CountryCodeDataElement : DigitsDataElement
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CountryCodeDataElement() : base(DataElementType.CountryCode) { }

        //internal CountryCodeDataElement(int countryCode) : base(DataElementType.CountryCode) { Value = countryCode; }

        #endregion construction
    }
}

// eof "CountryCodeDataElement.cs"
