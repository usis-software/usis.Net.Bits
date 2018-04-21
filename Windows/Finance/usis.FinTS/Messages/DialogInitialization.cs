//
//  @(#) DialogInitialization.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using usis.FinTS.Base;
using usis.FinTS.Segments;

namespace usis.FinTS.Messages
{
    //  --------------------------
    //  DialogInitialization class
    //  --------------------------

    internal sealed class DialogInitialization : Message
    {
        private Identification identification;

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DialogInitialization(IEnumerable<Segment> segments) : base(segments)
        {
            identification = segments.Find<Identification>();
        }

        internal DialogInitialization(BankAccess bankAccess, CustomerSystem customerSystem) : base(bankAccess.HbciVersion)
        {
            identification = new Identification(bankAccess);

            AddSegments(identification, new ProcessingPreparation(customerSystem));
        }

        #endregion construction
    }
}

// eof "DialogInitialization.cs"
