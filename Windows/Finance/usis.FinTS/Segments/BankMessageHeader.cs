//
//  @(#) BankMessageHeader.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.DataElementGroups;

namespace usis.FinTS.Segments
{
    //  -----------------------
    //  BankMessageHeader class
    //  -----------------------

    internal sealed class BankMessageHeader : MessageHeader
    {
        #region fields

        private MessageReference messageReference = new MessageReference();

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public BankMessageHeader() { AddElements(messageReference); }

        internal BankMessageHeader(int hbciVersion, int referenceMessageNumber) : base(hbciVersion)
        {
            AddElements(messageReference);

            messageReference.MessageNumber = referenceMessageNumber;
        }

        #endregion construction

        #region overrides

        //  ------------------
        //  SetDialogId method
        //  ------------------

        internal override void SetDialogId(string id)
        {
            base.SetDialogId(id);

            messageReference.DialogId = id;
        }

        #endregion overrides
    }
}

// eof "BankMessageHeader.cs"
