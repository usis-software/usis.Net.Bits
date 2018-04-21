//
//  @(#) MessageReference.cs
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
    //  ----------------------
    //  MessageReference class
    //  ----------------------

    internal class MessageReference : DataElementGroup
    {
        #region fields

        private IdentifierDataElement dialogId = new IdentifierDataElement();
        private NumericDataElement messageNumber = new NumericDataElement(4, LengthQualifier.Maximum);

        #endregion fields

        #region properties

        //  -----------------
        //  DialogId property
        //  -----------------

        internal string DialogId { set => dialogId.Value = value; }

        //  ----------------------
        //  MessageNumber property
        //  ----------------------

        internal int MessageNumber { set => messageNumber.Value = value; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal MessageReference()
        {
            AddElements(dialogId, messageNumber);
        }

        #endregion construction
    }
}

// eof "MessageReference.cs"
