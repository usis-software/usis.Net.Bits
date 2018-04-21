//
//  @(#) NumericDataElement.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using usis.FinTS.Base;

namespace usis.FinTS.DataElements
{
    //  ------------------------
    //  NumericDataElement class
    //  ------------------------

    internal class NumericDataElement : DataElement
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal NumericDataElement(int length, LengthQualifier lengthQualifier) : base(new DataElementType(BaseDataType.Numeric, length, lengthQualifier)) { Value = 0; }

        //internal NumericDataElement(int length, LengthQualifier lengthQualifier, long value) : this(length, lengthQualifier) { Value = value; }

        #endregion construction

        #region properties

        //  --------------
        //  Value property
        //  --------------

        internal long Value
        {
            get => Convert.ToInt64(GetValue(), CultureInfo.InvariantCulture);
            set => SetValue(value.ToString(CultureInfo.InvariantCulture));
        }

        //  -------------------
        //  Int32Value property
        //  -------------------

        internal int Int32Value => Convert.ToInt32(GetValue(), CultureInfo.InvariantCulture);

        #endregion properties
    }
}

// eof "NumericDataElement.cs"
