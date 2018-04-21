//
//  @(#) IdentifierDataElement.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;

namespace usis.FinTS.DataElements
{
    //  ---------------------------
    //  IdentifierDataElement class
    //  ---------------------------

    internal class IdentifierDataElement : DataElement
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal IdentifierDataElement() : base(DataElementType.Identifier) { }

        internal IdentifierDataElement(string value) : base(DataElementType.Identifier) { Value = value; }

        #endregion construction

        #region properties

        //  --------------
        //  Value property
        //  --------------

        internal string Value { set => SetValue(value); }

        #endregion properties
    }
}

// eof "IdentifierDataElement.cs"
