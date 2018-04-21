//
//  @(#) CodeDataElement.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;

namespace usis.FinTS.DataElements
{
    //  ---------------------
    //  CodeDataElement class
    //  ---------------------

    internal class CodeDataElement : DataElement
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal CodeDataElement(int length) : base(DataElementType.CreateCodeDataElementType(length)) { }

        internal CodeDataElement(int length, string value) : this(length) { Value = value; }

        #endregion construction

        #region properties

        //  --------------
        //  Value property
        //  --------------

        internal string Value { set => SetValue(value); }

        #endregion properties
    }
}

// eof "CodeDataElement.cs"
