//
//  @(#) DataElementType.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;

namespace usis.FinTS.Base
{
    //  ---------------------
    //  DataElementType class
    //  ---------------------

    internal sealed class DataElementType
    {
        #region properties

        //  ---------------------
        //  BaseDataType property
        //  ---------------------

        internal BaseDataType BaseDataType { get; }

        //  ---------------
        //  Length property
        //  ---------------

        internal int? Length { get; }

        //  ------------------------
        //  LengthQualifier property
        //  ------------------------

        internal LengthQualifier LengthQualifier { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DataElementType(BaseDataType baseDataType, int length, LengthQualifier lengthQualifier)
        {
            if (lengthQualifier == LengthQualifier.None)
            {
                throw new ArgumentException(Strings.LengthQualifierCannotBeNone, nameof(lengthQualifier));
            }
            BaseDataType = baseDataType;
            Length = length;
            LengthQualifier = lengthQualifier;
        }

        private DataElementType(DerivedDataType derivedDataType) : this(derivedDataType, null) { }

        private DataElementType(DerivedDataType derivedDataType, int? length)
        {
            switch (derivedDataType)
            {
                case DerivedDataType.None:
                    throw new ArgumentOutOfRangeException(nameof(derivedDataType));
                case DerivedDataType.Boolean:
                    BaseDataType = BaseDataType.Alphanumeric;
                    Length = 1;
                    LengthQualifier = LengthQualifier.Exact;
                    break;
                case DerivedDataType.Code:
                    BaseDataType = BaseDataType.Alphanumeric;
                    Length = length;
                    break;
                case DerivedDataType.Date:
                    BaseDataType = BaseDataType.Numeric;
                    Length = 8;
                    LengthQualifier = LengthQualifier.Exact;
                    break;
                case DerivedDataType.VirtualDate:
                    BaseDataType = BaseDataType.Numeric;
                    Length = 8;
                    LengthQualifier = LengthQualifier.Exact;
                    break;
                case DerivedDataType.Time:
                    BaseDataType = BaseDataType.Digits;
                    Length = 6;
                    LengthQualifier = LengthQualifier.Exact;
                    break;
                case DerivedDataType.Identifier:
                    BaseDataType = BaseDataType.Alphanumeric;
                    Length = 30;
                    LengthQualifier = LengthQualifier.Maximum;
                    break;
                case DerivedDataType.CountryCode:
                    BaseDataType = BaseDataType.Digits;
                    Length = 3;
                    LengthQualifier = LengthQualifier.Exact;
                    break;
                case DerivedDataType.Currency:
                    BaseDataType = BaseDataType.Digits;
                    Length = 3;
                    LengthQualifier = LengthQualifier.Exact;
                    break;
                case DerivedDataType.Value:
                    BaseDataType = BaseDataType.Float;
                    Length = 15;
                    LengthQualifier = LengthQualifier.Maximum;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion construction

        #region methods

        //  ---------------
        //  Validate method
        //  ---------------

        internal string Validate(string value)
        {
            switch (LengthQualifier)
            {
                case LengthQualifier.None:
                    break;
                case LengthQualifier.Exact:
                    if (value.Length != Length.Value) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ValueExactLength, Length.Value), nameof(value));
                    break;
                case LengthQualifier.Maximum:
                    if (value.Length > Length.Value) throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ValueMaximumLength, Length.Value), nameof(value));
                    break;
                default:
                    break;
            }
            return value;
        }

        #endregion methods

        #region static fields

        //internal static readonly DataElementType Boolean = new DataElementType(DerivedDataType.Boolean);
        internal static readonly DataElementType Identifier = new DataElementType(DerivedDataType.Identifier);
        //internal static readonly DataElementType Date = new DataElementType(DerivedDataType.Date);
        internal static readonly DataElementType CountryCode = new DataElementType(DerivedDataType.CountryCode);

        #endregion static fields

        #region static methods

        internal static DataElementType CreateCodeDataElementType(int length) { return new DataElementType(DerivedDataType.Code, length); }

        #endregion static methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}({1})", BaseDataType, Length);
        }

        #endregion overrides
    }

    #region BaseDataType enumeration

    //  ------------------------
    //  BaseDataType enumeration
    //  ------------------------

    internal enum BaseDataType
    {
        Alphanumeric,
        Text,
        DTA,
        Numeric,
        Digits,
        Float,
        Binary,
    }

    #endregion BaseDataType enumeration

    #region DerivedDataType enumeration

    //  ---------------------------
    //  DerivedDataType enumeration
    //  ---------------------------

    internal enum DerivedDataType
    {
        None,
        Boolean,
        Code,
        Date,
        VirtualDate,
        Time,
        Identifier,
        CountryCode,
        Currency,
        Value
    }

    #endregion DerivedDataType enumeration

    #region LengthQualifier enumeration

    //  ---------------------------
    //  LengthQualifier enumeration
    //  ---------------------------

    internal enum LengthQualifier
    {
        None,
        Exact,
        Maximum
    }

    #endregion LengthQualifier enumeration
}

// eof "DataElementType.cs"
