//
//  @(#) AlphanumericDataElement.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;

namespace usis.FinTS.DataElements
{
    //  -----------------------------
    //  AlphanumericDataElement class
    //  -----------------------------

    internal class AlphanumericDataElement : DataElement
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal AlphanumericDataElement(int length, LengthQualifier lengthQualifier) : base(new DataElementType(BaseDataType.Alphanumeric, length, lengthQualifier)) { }

        //internal AlphanumericDataElement(int length, LengthQualifier lengthQualifier, string value) : this(length, lengthQualifier) { Value = value; }

        #endregion construction

        #region Value property

        //  --------------
        //  Value property
        //  --------------

        internal string Value { get => GetValue(); set => SetValue(value); }

        #endregion Value property
    }
}

// eof "AlphanumericDataElement.cs"
