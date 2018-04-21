//
//  @(#) MessageHeader.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using usis.FinTS.Base;
using usis.FinTS.DataElements;

namespace usis.FinTS.Segments
{
    //  -------------------
    //  MessageHeader class
    //  -------------------
    
    /// <summary>
    /// The starting segment of every customer and bank message.
    /// </summary>
    /// <seealso cref="Segment" />

    internal class MessageHeader : Segment
    {
        #region fields

        private DigitsDataElement messageSize = new DigitsDataElement(12, LengthQualifier.Maximum);
        private NumericDataElement hbciVersion = new NumericDataElement(3, LengthQualifier.Maximum);
        private IdentifierDataElement dialogId = new IdentifierDataElement();
        private NumericDataElement messageNumber = new NumericDataElement(4, LengthQualifier.Maximum);

        #endregion fields

        #region properties

        //  --------------------
        //  MessageSize property
        //  --------------------

        internal long MessageSize
        {
            get => messageSize.Value;
            set => messageSize.Value = value;
        }

        //  -----------------
        //  DialogId property
        //  -----------------

        internal string DialogId { set => SetDialogId(value); }

        //  ----------------------
        //  MessageNumber property
        //  ----------------------

        internal int MessageNumber { get => Convert.ToInt32(messageNumber.Value); set => messageNumber.Value = value; }

        //  --------------------
        //  HbciVersion property
        //  --------------------

        /// <summary>
        /// Gets the HBCI version of the message.
        /// </summary>
        /// <value>
        /// The HBCI version of the message.
        /// </value>

        internal int HbciVersion => hbciVersion.Int32Value;

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHeader"/> class
        /// with the specified HBCI version.
        /// </summary>

        internal MessageHeader(int hbciVersion) : base(SegmentIdentifiers.MessageHeader, 3)
        {
            Initialize();

            this.hbciVersion.Value = hbciVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHeader"/> class.
        /// </summary>

        public MessageHeader()
        {
            Initialize();
        }

        //  -----------------
        //  Initialize method
        //  -----------------

        private void Initialize() { AddElements(messageSize, hbciVersion, dialogId, messageNumber); }

        #endregion construction

        #region methods

        //  ------------------
        //  SetDialogId method
        //  ------------------

        internal virtual void SetDialogId(string id)
        {
            dialogId.Value = id;
        }

        #endregion methods
    }
}

// eof "MessageHeader.cs"
