//
//  @(#) IDocDataRecord.cs
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
    //  --------------------
    //  IDocDataRecord class
    //  --------------------

    /// <summary>
    /// Prepresents an IDoc data record.
    /// </summary>
    /// <seealso cref="IDocRecord" />

    public sealed class IDocDataRecord : IDocRecord
    {
        #region fields

        //  ----------------
        //  Definition field
        //  ----------------

        /// <summary>
        /// The field definitions of an IDoc data record.
        /// </summary>

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly IDocRecordDefinition Definition = CreateDefinition();

        #endregion fields

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the segment definition name.
        /// </summary>
        /// <value>
        /// The segment definition name.
        /// </value>

        public override string Name => GetDataString(0);

        //  --------------------
        //  SegmentName property
        //  --------------------

        internal string SegmentName
        {
            get => GetDataString(0);
            set => SetDataString(0, value);
        }

        //  ---------------
        //  Client property
        //  ---------------

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>

        public string Client
        {
            get => GetDataString(1);
            set => SetDataString(1, value);
        }

        //  -----------------------
        //  DocumentNumber property
        //  -----------------------

        /// <summary>
        /// Gets the intermediate document number.
        /// </summary>
        /// <value>
        /// The intermediate document number.
        /// </value>

        public string DocumentNumber
        {
            get => GetDataString(2);
            internal set => SetDataString(2, value);
        }

        //  ----------------------
        //  SegmentNumber property
        //  ----------------------

        /// <summary>
        /// Gets the segment number.
        /// </summary>
        /// <value>
        /// The segment number.
        /// </value>

        public int SegmentNumber
        {
            get => Convert.ToInt32(GetDataString(3), CultureInfo.InvariantCulture);
            set => SetDataString(3, value.ToString("D6", CultureInfo.InvariantCulture));
        }

        //  ----------------------------
        //  ParentSegmentNumber property
        //  ----------------------------

        /// <summary>
        /// Gets the number of the superior parent segment.
        /// </summary>
        /// <value>
        /// The number of the superior parent segment.
        /// </value>

        public int ParentSegmentNumber
        {
            get => Convert.ToInt32(GetDataString(4), CultureInfo.InvariantCulture);
            set => SetDataString(4, value.ToString("D6", CultureInfo.InvariantCulture));
        }

        //  -----------------------
        //  HierarchyLevel property
        //  -----------------------

        /// <summary>
        /// Gets the hierarchy level of the segment.
        /// </summary>
        /// <value>
        /// The hierarchy level of the segment.
        /// </value>

        public byte HierarchyLevel
        {
            get => Convert.ToByte(GetDataString(5), CultureInfo.InvariantCulture);
            set => SetDataString(5, value.ToString("D2", CultureInfo.InvariantCulture));
        }

        //  -------------
        //  Data property
        //  -------------

        /// <summary>
        /// Gets the application data.
        /// </summary>
        /// <value>
        /// The application data.
        /// </value>

        public string Data
        {
            get => GetDataString(6);
            set => SetDataString(6, value);
        }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocDataRecord"/> class
        /// with the specified data.
        /// </summary>
        /// <param name="data">The data.</param>

        public IDocDataRecord(string data) : base(data, Definition) { }

        #endregion construction

        #region methods

        //  -----------------------
        //  CreateDefinition method
        //  -----------------------

        private static IDocRecordDefinition CreateDefinition()
        {
            var definition = new IDocRecordDefinition();
            definition.AddField("SEGNAM", 30);
            definition.AddField("MANDT", 3);
            definition.AddField("DOCNUM", 16);
            definition.AddField("SEGNUM", 6);
            definition.AddField("PSGNUM", 6);
            definition.AddField("HLEVEL", 2);
            definition.AddField("SDATA", 1000);
            return definition;
        }

        #endregion methods
    }
}

// eof "IDocDataRecord.cs"
