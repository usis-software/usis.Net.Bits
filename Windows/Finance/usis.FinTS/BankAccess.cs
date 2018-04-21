//
//  @(#) BankAccess.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

namespace usis.FinTS
{
    //  ----------------
    //  BankAccess class
    //  ----------------

    /// <summary>
    /// Provides context information for a dialog that identifies the user to the bank system.
    /// </summary>

    public sealed class BankAccess
    {
        //  --------------------
        //  CountryCode property
        //  --------------------

        /// <summary>
        /// Gets or sets the country code according to ISO 3166-1.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>

        public int CountryCode { get; set; }

        //  -----------------
        //  BankCode property
        //  -----------------

        /// <summary>
        /// Gets or sets the country specific code that uniquely identifies a bank.
        /// </summary>
        /// <value>
        /// The bank code.
        /// </value>

        public string BankCode { get; set; }

        //  --------------------
        //  HbciVersion property
        //  --------------------

        /// <summary>
        /// Gets or sets the HBCI version to use.
        /// </summary>
        /// <value>
        /// The HBCI version.
        /// </value>

        public int HbciVersion { get; set; }
    }
}

// eof "BankAccess.cs"
