//
//  @(#) DigitsDataElement.cs
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
    //  -----------------------
    //  DigitsDataElement class
    //  -----------------------

    internal class DigitsDataElement : DataElement
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DigitsDataElement(int length, LengthQualifier lengthQualifier) : base(new DataElementType(BaseDataType.Digits, length, lengthQualifier)) { Value = 0; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitsDataElement"/> class
        /// with the specified data element type.
        /// </summary>
        /// <param name="dataElementType">The data element type.</param>
        /// <exception cref="ArgumentException"><paramref name="dataElementType"/> has not the base data type <see cref="BaseDataType.Digits"/>.</exception>
        /// <remarks>This constructor is used to create data elements for derived data element types.</remarks>

        internal protected DigitsDataElement(DataElementType dataElementType) : base(dataElementType)
        {
            if (dataElementType.BaseDataType != BaseDataType.Digits)
            {
                throw new ArgumentException(Strings.WrongDataElementType, nameof(dataElementType));
            }
        }

        #endregion construction

        #region properties

        //  --------------
        //  Value property
        //  --------------

        internal long Value
        {
            get => Convert.ToInt64(GetValue(), CultureInfo.InvariantCulture);
            set => SetValue(value.ToString(
                DataElementType.Length.HasValue ? string.Format(CultureInfo.InvariantCulture, "D{0}", DataElementType.Length.Value) : "G",
                CultureInfo.InvariantCulture));
        }

        #endregion properties
    }
}

// eof "DigitsDataElement.cs"
