//
//  @(#) DialogTermination.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.FinTS.Base;
using usis.FinTS.Segments;

namespace usis.FinTS.Messages
{
    //  -----------------------
    //  DialogTermination class
    //  -----------------------

    internal sealed class DialogTermination : Message
    {
        #region fields

        private DialogEnd dialogEnd;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DialogTermination(IEnumerable<Segment> segments) : base(segments)
        {
            dialogEnd = segments.Find<DialogEnd>() ?? throw new InvalidOperationException();
        }

        internal DialogTermination(BankAccess bankAccess) : base(bankAccess.HbciVersion)
        {
            dialogEnd = new DialogEnd();
            AddSegments(dialogEnd);
        }

        #endregion construction

        #region overrides

        //  ------------------
        //  SetDialogId method
        //  ------------------

        internal override void SetDialogId(string dialogId)
        {
            base.SetDialogId(dialogId);

            dialogEnd.DialogId = dialogId;
        }

        #endregion overrides
    }
}

// eof "DialogTermination.cs"
