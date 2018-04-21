//
//  @(#) BankIdentifier.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;
using usis.FinTS.DataElements;

namespace usis.FinTS.DataElementGroups
{
    //  --------------------
    //  BankIdentifier class
    //  --------------------

    internal class BankIdentifier : DataElementGroup
    {
        #region fields

        private CountryCodeDataElement countryCode = new CountryCodeDataElement();
        private AlphanumericDataElement bankCode = new AlphanumericDataElement(30, LengthQualifier.Maximum);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BankIdentifier()
        {
            AddElements(countryCode, bankCode);
        }

        internal BankIdentifier(BankAccess bankAccess) : this()
        {
            countryCode.Value = bankAccess.CountryCode;
            bankCode.Value = bankAccess.BankCode;
        }

        #endregion construction
    }
}

// eof "BankIdentifier.cs"
