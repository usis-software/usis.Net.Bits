//
//  @(#) Identification.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using usis.FinTS.Base;
using usis.FinTS.DataElements;

namespace usis.FinTS.Segments
{
    //  ---------------
    //  DialogEnd class
    //  ---------------

    internal sealed class DialogEnd : Segment
    {
        #region fields

        private IdentifierDataElement dialogId = new IdentifierDataElement();

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public DialogEnd() : base(SegmentIdentifiers.DialogEnd, 1) { AddElements(dialogId); }

        #endregion construction

        #region properties

        //  -----------------
        //  DialogId property
        //  -----------------

        internal string DialogId { set => dialogId.Value = value; }

        #endregion properties
    }
}

// eof "Identification.cs"
