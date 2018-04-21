//
//  @(#) IDocControlRecord.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace usis.Middleware.SAP
{
    //  -----------------------
    //  IDocControlRecord class
    //  -----------------------

    /// <summary>
    /// Defines the contents, structure, sender, receiver and current status
    /// of an intermediate document.
    /// </summary>

    public sealed class IDocControlRecord : IDocRecord
    {
        #region constants

        internal const string TableName = "EDI_DC40";

        #endregion constants

        #region fields

        //  ----------------
        //  Definition field
        //  ----------------

        /// <summary>
        /// The field definitions of an IDoc control record.
        /// </summary>

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly IDocRecordDefinition Definition = CreateDefinition();

        #endregion fields

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the table structure for the IDoc control record.
        /// </summary>
        /// <value>
        /// The name of the table structure for the IDoc control record.
        /// </value>
        /// <remarks>
        /// <para>
        /// This property corresponds to the <c>TABNAM</c> field.<br/>
        /// It contains the constant <c>"EDI_DC40"</c>.
        /// </para>
        /// </remarks>

        public override string Name => GetDataString(0);

        //  ---------------
        //  Client property
        //  ---------------

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        /// <remarks>This property corresponds to the <c>MANDT</c> field.</remarks>

        public string Client
        {
            get => GetDataString(1);
            set => SetDataString(1, value);
        }

        //  -----------------------
        //  DocumentNumber property
        //  -----------------------

        /// <summary>
        /// Gets or sets the number of the IDoc.
        /// </summary>
        /// <value>
        /// The number of the IDoc.
        /// </value>
        /// <remarks>This property corresponds to the <c>DOCNUM</c> field.</remarks>

        public string DocumentNumber
        {
            get => GetDataString(2);
            set => SetDataString(2, value);
        }

        //  ----------------
        //  Release property
        //  ----------------

        /// <summary>
        /// Gets or sets the SAP Release for the intermediate document.
        /// </summary>
        /// <value>
        /// The SAP Release for the intermediate document.
        /// </value>
        /// <remarks>This property corresponds to the <c>DOCREL</c> field.</remarks>

        public string Release
        {
            get => GetDataString(3);
            set => SetDataString(3, value);
        }

        //  ---------------
        //  Status property
        //  ---------------

        /// <summary>
        /// Gets the status of the intermediate document.
        /// </summary>
        /// <value>
        /// The status of the intermediate document.
        /// </value>
        /// <remarks>
        /// This property corresponds to the <c>STATUS</c> field.<para/>
        /// The SAP table TEDS1 contains a list of all status codes.
        /// The table can be displayed with the transaction SE16.
        /// </remarks>

        public string Status => GetDataString(4);

        //  ------------------
        //  Direction property
        //  ------------------

        /// <summary>
        /// Gets or sets the direction of the IDoc transmission.
        /// </summary>
        /// <value>
        /// The direction of the IDoc transmission.
        /// </value>
        /// <remarks>This property corresponds to the <c>DIRECT</c> field.</remarks>

        public IDocDirection Direction
        {
            get
            {
                switch (GetDataString(5))
                {
                    case "1":
                        return IDocDirection.Outbound;
                    case "2":
                        return IDocDirection.Inbound;
                    default:
                        return IDocDirection.Undefined;
                }
            }
            set
            {
                string s = string.Empty;
                switch (value)
                {
                    case IDocDirection.Undefined:
                        break;
                    case IDocDirection.Outbound:
                        s = "1";
                        break;
                    case IDocDirection.Inbound:
                        s = "2";
                        break;
                    default:
                        break;
                }
                SetDataString(5, s);
            }
        }

        //  -------------------
        //  OutputMode property
        //  -------------------

        /// <summary>
        /// Gets the output mode.
        /// </summary>
        /// <value>
        /// The output mode.
        /// </value>
        /// <remarks>
        /// <para>
        /// This property corresponds to the <c>OUTMOD</c> field.
        /// </para>
        /// <para>
        /// In outbound processing, this value determines whether the IDocs are sent to the external system individually or in a packet.
        /// In inbound processing, this field should be left <see cref="IDocOutputMode.Undefined"/> by the external system.
        /// </para>
        /// </remarks>

        public IDocOutputMode OutputMode
        {
            get
            {
                switch (GetDataString(6))
                {
                    case "1":
                        return IDocOutputMode.TransferImmediatelyStartSubsystem;
                    case "2":
                        return IDocOutputMode.TransferImmediately;
                    case "3":
                        return IDocOutputMode.CollectTransferStartSubsystem;
                    case "4":
                        return IDocOutputMode.CollectTransfer;
                    default:
                        return IDocOutputMode.Undefined;
                }
            }
        }

        //  ---------------------
        //  DocumentType property
        //  ---------------------

        /// <summary>
        /// Gets the name of the base type.
        /// </summary>
        /// <value>
        /// The name of the base type.
        /// </value>
        /// <remarks>This property corresponds to the <c>IDOCTYP</c> field.</remarks>

        public string DocumentType
        {
            get => GetDataString(9);
            private set => SetDataString(9, value);
        }

        //  --------------------
        //  MessageType property
        //  --------------------

        /// <summary>
        /// Gets the message type of the IDoc.
        /// </summary>
        /// <value>
        /// The message type of the IDoc.
        /// </value>
        /// <remarks>This property corresponds to the <c>MESTYP</c> field.</remarks>

        public string MessageType => GetDataString(11);

        //  -----------------------
        //  EDIMessageType property
        //  -----------------------

        /// <summary>
        /// Gets the EDI message type.
        /// </summary>
        /// <value>
        /// The EDI message type.
        /// </value>
        /// <remarks>This property corresponds to the <c>STDMES</c> field.</remarks>

        public string EDIMessageType => GetDataString(16);

        //  -------------------
        //  SenderPort property
        //  -------------------

        /// <summary>
        /// Gets the Sender port (SAP System, external subsystem).
        /// </summary>
        /// <value>
        /// The Sender port.
        /// </value>
        /// <remarks>This property corresponds to the <c>SNDPOR</c> field.</remarks>

        public string SenderPort => GetDataString(17);

        //  --------------------------
        //  SenderPartnerType property
        //  --------------------------

        /// <summary>
        /// Gets the partner type of the Sender.
        /// </summary>
        /// <value>
        /// The partner type of the Sender.
        /// </value>
        /// <remarks>This property corresponds to the <c>SNDPRT</c> field.</remarks>

        public string SenderPartnerType => GetDataString(18);

        //  ------------------------------
        //  SenderPartnerFunction property
        //  ------------------------------

        /// <summary>
        /// Gets the Partner Function of the Sender.
        /// </summary>
        /// <value>
        /// The Partner Function of the Sender.
        /// </value>
        /// <remarks>This property corresponds to the <c>SNDPFC</c> field.</remarks>

        public string SenderPartnerFunction => GetDataString(19);

        //  ----------------------------
        //  SenderPartnerNumber property
        //  ----------------------------

        /// <summary>
        /// Gets the Partner Number of the Sender.
        /// </summary>
        /// <value>
        /// The Partner Number of the Sender.
        /// </value>
        /// <remarks>
        /// This property corresponds to the <c>SNDPRN</c> field.<br/>
        /// It contains the partner number of the sender,
        /// as entered in the SAP master data (via internal number assignment).
        /// </remarks>

        public string SenderPartnerNumber => GetDataString(20);

        //  ---------------------
        //  ReceiverPort property
        //  ---------------------

        /// <summary>
        /// Gets the Receiver port.
        /// </summary>
        /// <value>
        /// The Receiver port.
        /// </value>
        /// <remarks>This property corresponds to the <c>RCVPOR</c> field.</remarks>

        public string ReceiverPort => GetDataString(23);

        //  ----------------------------
        //  ReceiverPartnerType property
        //  ----------------------------

        /// <summary>
        /// Gets the Partner Type of the Receiver.
        /// </summary>
        /// <value>
        /// The Partner Type of the Receiver.
        /// </value>
        /// <remarks>This property corresponds to the <c>RCVPRT</c> field.</remarks>

        public string ReceiverPartnerType => GetDataString(24);

        //  --------------------------------
        //  ReceiverPartnerFunction property
        //  --------------------------------

        /// <summary>
        /// Gets the Partner Function of the Receiver.
        /// </summary>
        /// <value>
        /// The Partner Function of the Receiver.
        /// </value>
        /// <remarks>This property corresponds to the <c>RCVPFC</c> field.</remarks>

        public string ReceiverPartnerFunction => GetDataString(25);

        //  ------------------------------
        //  ReceiverPartnerNumber property
        //  ------------------------------

        /// <summary>
        /// Gets the Partner Number of the Receiver.
        /// </summary>
        /// <value>
        /// The Partner Number of the Receiver.
        /// </value>
        /// <remarks>This property corresponds to the <c>RCVPRN</c> field.</remarks>

        public string ReceiverPartnerNumber => GetDataString(26);

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets date and time when the document was created.
        /// </summary>
        /// <value>
        /// The date and time when the document was created.
        /// </value>
        /// <remarks>This property corresponds to the <c>CREDAT</c> and <c>CRETIM</c> fields.</remarks>

        public DateTime Created
        {
            get { return DateTime.ParseExact(GetDataString(29) + GetDataString(30), DateTimeFormat, CultureInfo.InvariantCulture); }
            set
            {
                var s = value.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
                SetDataString(29, s.Substring(0, 8));
                SetDataString(30, s.Substring(8, 6));
            }
        }

        private const string DateTimeFormat = "yyyyMMddHHmmss";

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocControlRecord"/> class
        /// woth the specified data.
        /// </summary>
        /// <param name="data">
        /// The data for intermediate document control record.
        /// </param>
        /// <exception cref="FormatException">
        /// The record data does not start with "<c>EDI_DC40</c>".
        /// </exception>

        public IDocControlRecord(string data) : base(data, Definition)
        {
            string name = GetDataString(0);
            if (name == null || !name.StartsWith(TableName, StringComparison.Ordinal))
            {
                throw new FormatException(string.Format(CultureInfo.CurrentCulture, Strings.WrongControlRecord, TableName));
            }
        }

        internal IDocControlRecord(IDocDefinition definition) : this(TableName)
        {
            DocumentType = definition.DocumentType;
        }

        #endregion construction

        #region methods

        //  -----------------------
        //  CreateDefinition method
        //  -----------------------

        /// <summary>
        /// Creates the record definition according to the structure EDI_DC40
        /// (IDoc Control Record for Interface to External System) in the SAP Dictionary.
        /// </summary>
        /// <returns>
        /// The record definition according to the structure EDI_DC40.
        /// </returns>

        private static IDocRecordDefinition CreateDefinition()
        {
            var definition = new IDocRecordDefinition();
            definition.AddField("TABNAM", 10);
            definition.AddField("MANDT", 3);
            definition.AddField("DOCNUM", 16);
            definition.AddField("DOCREL", 4);
            definition.AddField("STATUS", 2);
            definition.AddField("DIRECT", 1);
            definition.AddField("OUTMOD", 1);
            definition.AddField("EXPRSS", 1);
            definition.AddField("TEST", 1);
            definition.AddField("IDOCTYP", 30);
            definition.AddField("CIMTYP", 30);
            definition.AddField("MESTYP", 30);
            definition.AddField("MESCOD", 3);
            definition.AddField("MESFCT", 3);
            definition.AddField("STD", 1);
            definition.AddField("STDVRS", 6);
            definition.AddField("STDMES", 6);
            definition.AddField("SNDPOR", 10);
            definition.AddField("SNDPRT", 2);
            definition.AddField("SNDPFC", 2);
            definition.AddField("SNDPRN", 10);
            definition.AddField("SNDSAD", 21);
            definition.AddField("SNDLAD", 70);
            definition.AddField("RCVPOR", 10);
            definition.AddField("RCVPRT", 2);
            definition.AddField("RCVPFC", 2);
            definition.AddField("RCVPRN", 10);
            definition.AddField("RCVSAD", 21);
            definition.AddField("RCVLAD", 70);
            definition.AddField("CREDAT", 8);
            definition.AddField("CRETIM", 6);
            definition.AddField("REFINT", 14);
            definition.AddField("REFGRP", 14);
            definition.AddField("REFMES", 14);
            definition.AddField("ARCKEY", 70);
            definition.AddField("SERIAL", 20);
            definition.ReIndex();
            return definition;
        }

        #endregion methods
    }
}

// eof "IDocControlRecord.cs"
