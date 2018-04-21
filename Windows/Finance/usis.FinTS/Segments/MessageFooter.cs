//
//  @(#) MessageFooter.cs
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
    //  -------------------
    //  MessageFooter class
    //  -------------------

    /// <summary>
    /// The termination segment of every customer and bank message.
    /// </summary>
    /// <seealso cref="Segment" />

    internal sealed class MessageFooter : Segment
    {
        #region fields

        private NumericDataElement messageNumber = new NumericDataElement(4, LengthQualifier.Maximum);

        #endregion fields

        #region properties

        //  ----------------------
        //  MessageNumber property
        //  ----------------------

        internal int MessageNumber { set => messageNumber.Value = value; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFooter"/> class.
        /// </summary>

        public MessageFooter() : base(SegmentIdentifiers.MessageFooter, 1)
        {
            AddElements(messageNumber);
        }

        #endregion construction
    }
}

// eof "MessageFooter.cs
