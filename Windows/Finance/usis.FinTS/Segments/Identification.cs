//
//  @(#) Identification.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;
using usis.FinTS.DataElementGroups;
using usis.FinTS.DataElements;

namespace usis.FinTS.Segments
{
    //  --------------------
    //  Identification class
    //  --------------------

    /// <summary>
    /// The segment that contains information to validate the user's sent authorization.
    /// </summary>
    /// <seealso cref="Segment" />

    internal sealed class Identification : Segment
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Identification"/> class
        /// for anonymous access.
        /// </summary>
        /// <param name="bankAccess">The bank access information.</param>

        internal Identification(BankAccess bankAccess) : base(SegmentIdentifiers.Identification, 2)
        {
            AddElements(
                new BankIdentifier(bankAccess),
                new IdentifierDataElement(Constants.AnonymousCustomerId),
                new IdentifierDataElement(Constants.AnonymousCustomerSystemId),
                new CodeDataElement(1, Constants.AnonymousCustomerSystemStatus));
        }

        public Identification()
        {
            AddElements(
                new BankIdentifier(),
                new IdentifierDataElement(),
                new IdentifierDataElement(),
                new CodeDataElement(1));
        }

        #endregion construction
    }
}

// eof "Identification.cs"
