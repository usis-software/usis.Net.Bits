//
//  @(#) Message.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using usis.FinTS.Base;
using usis.FinTS.Segments;

namespace usis.FinTS
{
    //  -------------
    //  Message class
    //  -------------

    /// <summary>
    /// Provides a base class for FinTS messages.
    /// </summary>

    public abstract class Message
    {
        #region fields

        private List<Segment> segmentList = new List<Segment>();

        #endregion fields

        #region properties

        //  --------------------
        //  HbciVersion property
        //  --------------------

        /// <summary>
        /// Gets the HBCI version of the message.
        /// </summary>
        /// <value>
        /// The HBCI version of the message.
        /// </value>

        public int HbciVersion => Header.HbciVersion;

        //  -----------------
        //  DialogId property
        //  -----------------

        internal string DialogId { set => SetDialogId(value); }

        //  ---------------
        //  Number property
        //  ---------------

        internal int Number { get => Header.MessageNumber; set => Header.MessageNumber = Footer.MessageNumber = value; }

        //  ---------------
        //  Header property
        //  ---------------

        internal MessageHeader Header => segmentList.First() as MessageHeader;

        //  ---------------
        //  Footer property
        //  ---------------

        private MessageFooter Footer => segmentList.Last() as MessageFooter;

        //  --------------------
        //  MessageSize property
        //  --------------------

        private long MessageSize { get => Header.MessageSize; set => Header.MessageSize = value; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class
        /// with the specified message hader.
        /// </summary>
        /// <param name="header">The message header depending on whether the message is a customer message or a bank message.</param>

        internal Message(MessageHeader header)
        {
            segmentList.Add(header);
            Header.Number = 1;

            segmentList.Add(new MessageFooter() { Number = 2 });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class
        /// with the specified HBCI version.
        /// </summary>
        /// <param name="hbciVersion">The HBCI version.</param>
        /// <remarks>This constructor creates a custimer message.</remarks>

        internal Message(int hbciVersion) : this(new MessageHeader(hbciVersion)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class
        /// with the specified segment list.
        /// </summary>
        /// <param name="segments">The segments.</param>

        internal Message(IEnumerable<Segment> segments)
        {
            segmentList.AddRange(segments);
        }

        #endregion construction

        #region methods

        //  ------------------
        //  AddSegments method
        //  ------------------

        internal void AddSegments(params Segment[] segments)
        {
            foreach (var segment in segments) AddSegment(segment);
        }

        //  -----------------
        //  AddSegment method
        //  -----------------

        private void AddSegment(Segment segment)
        {
            System.Diagnostics.Debug.Assert(segmentList.Count > 1);

            var index = segmentList.Count - 1;
            segmentList.Insert(index, segment);
            segment.Number = ++index;
            segmentList.Last().Number = ++index;
        }

        //  ------------------
        //  SetDialogId method
        //  ------------------

        internal virtual void SetDialogId(string dialogId) { Header.DialogId = dialogId; }

        //  -----------
        //  Dump method
        //  -----------

        private long Dump(Stream stream)
        {
            MemoryStream memory = null;
            try
            {
                memory = new MemoryStream();
                using (var writer = new StreamWriter(memory, Encoding.GetEncoding(Constants.MessageCodePage)))
                {
                    memory = null;

                    foreach (var segment in segmentList) segment.Serialize(writer);
                    writer.Flush();
                    if (!Stream.Null.Equals(stream))
                    {
                        writer.BaseStream.Seek(0, SeekOrigin.Begin);
                        writer.BaseStream.CopyTo(stream);
                    }
                    return writer.BaseStream.Length;
                }
            }
            finally
            {
                if (memory != null) memory.Dispose();
            }
        }

        //  ----------------
        //  Serialize method
        //  ----------------

        internal long Serialize(Stream stream)
        {
            if (MessageSize == 0) MessageSize = Dump(Stream.Null);
            return Dump(stream);
        }

        #endregion methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            using (var stream = new MemoryStream())
            {
                Serialize(stream);
                var data = stream.ToArray();
                return Encoding.GetEncoding(Constants.MessageCodePage).GetString(data);
            }
        }

        #endregion overrides
    }

    #region BankMessage class

    //  -----------------
    //  BankMessage class
    //  -----------------

    internal abstract class BankMessage : Message
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal BankMessage(IEnumerable<Segment> segments) : base(segments) { }

        internal BankMessage(int hbciVersion, int referenceMessageNumber) : base(new BankMessageHeader(hbciVersion, referenceMessageNumber)) { }

        #endregion construction
    }

    #endregion BankMessage class
}

// eof "Message.cs"
