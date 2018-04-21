//
//  @(#) IDocSegment.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  ------------------
    //  IDocSegment method
    //  ------------------

    /// <summary>
    /// Represents a data segment in an intermediate document.
    /// </summary>
    /// <remarks>
    /// Each data segment contains a standard header consisting of a sequential segment number,
    /// a description of the segment type and a 1000 character long string field
    /// containing the actual data of the segment.
    /// </remarks>

    public sealed class IDocSegment : IValueStorage
    {
        #region fields

        internal List<IDocSegment> children = new List<IDocSegment>();

        #endregion fields

        #region properties

        //  -------------------
        //  DataRecord property
        //  -------------------

        /// <summary>
        /// Gets the underlying data record of the segment.
        /// </summary>
        /// <value>
        /// The underlying data record of the segment.
        /// </value>

        public IDocDataRecord DataRecord { get; }

        //  ---------------
        //  Parent property
        //  ---------------

        /// <summary>
        /// Gets the parent segment.
        /// </summary>
        /// <value>
        /// The parent segment.
        /// </value>

        public IDocSegment Parent { get; internal set; }

        //  -------------------
        //  Definition property
        //  -------------------

        /// <summary>
        /// Gets the definition of the segment.
        /// </summary>
        /// <value>
        /// The definition of the segment.
        /// </value>

        public IDocSegmentDefinition Definition { get; }

        //  -----------------
        //  Document property
        //  -----------------

        /// <summary>
        /// Gets the IDoc that this segment belongs to.
        /// </summary>
        /// <value>
        /// The IDoc that this segment belongs to.
        /// </value>

        public IDoc Document { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocSegment" /> class
        /// with the specified data record.
        /// </summary>
        /// <param name="document">The IDoc that the segment belong to.</param>
        /// <param name="dataRecord">The data record that represents the IDoc segment.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dataRecord"/>
        /// </exception>
        /// <remarks>
        /// The data record must contain all field data as described in the <c>EDI_DD40</c> structure.
        /// </remarks>

        private IDocSegment(IDoc document, IDocDataRecord dataRecord)
        {
            Document = document;
            DataRecord = dataRecord ?? throw new ArgumentNullException(nameof(dataRecord));
        }

        internal IDocSegment(IDoc document, IDocDataRecord dataRecord, IDocRepository repository) : this(document, dataRecord)
        {
            Definition = repository?.FindSegmentDefinition(document.ControlRecord.DocumentType, InvariantSegmentName(DataRecord.SegmentName));
        }

        internal IDocSegment(IDoc document, IDocSegmentDefinition definition) : this(document, new IDocDataRecord(string.Empty))
        {
            Definition = definition;
            DataRecord.SegmentName = string.IsNullOrWhiteSpace(definition.SegmentDefinition) ? Definition.SegmentType : Definition.SegmentDefinition;
            DataRecord.Client = Document.ControlRecord.Client;
            DataRecord.DocumentNumber = Document.ControlRecord.DocumentNumber;
        }

        #endregion construction

        #region methods

        //  ---------------
        //  AddChild method
        //  ---------------

        /// <summary>
        /// Creates a new segment and adds the segment as a child to the segment.
        /// </summary>
        /// <param name="definition">The definition that specifies the segment type.</param>
        /// <returns>
        /// The newly created segment.
        /// </returns>

        public IDocSegment AddChild(IDocSegmentDefinition definition)
        {
            var segment = new IDocSegment(Document, definition);
            children.Add(segment);
            segment.Parent = this;
            Document.RenumberSegments();
            return segment;
        }

        #region private methods

        //  ---------------------------
        //  InvariantSegmentName method
        //  ---------------------------

        private static string InvariantSegmentName(string name)
        {
            var invariant = name;
            if (name.StartsWith("E2", StringComparison.OrdinalIgnoreCase))
            {
                invariant = string.Concat("E1", name.Remove(0, 2));
            }
            if (name.Length >= 6)
            {
                var s = name.Right(3);
                int v = -1;
                if (int.TryParse(s, out v))
                {
                    invariant = invariant.Remove(name.Length - s.Length, s.Length);
                }
            }
            return invariant;
        }

        #endregion private methods

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

        public override string ToString() { return DataRecord.ToString(); }

        #endregion overrides

        #region IValueStorage implementation

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value storage.
        /// </summary>
        /// <value>
        /// The name of the value storage.
        /// </value>
        /// <remarks>
        /// The name of storage is the segments type name
        /// as returned by the <see cref="IDocSegmentDefinition.SegmentType"/> property.
        /// </remarks>

        public string Name => Definition.SegmentType;

        //  -------------------
        //  ValueNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all field names in the segment.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all field names in the segment.
        /// </value>

        public IEnumerable<string> ValueNames => Definition.FieldNames();

        //  ---------------
        //  Values property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all fields in the segment.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all fields in the segment.
        /// </value>

        public IEnumerable<INamedValue> Values => Definition.GetFields(DataRecord.Data);

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves the fields with the specified name.
        /// </summary>
        /// <param name="name">The name of the field to retrieve.</param>
        /// <returns>
        /// A type that implements <see cref="INamedValue" /> and represents the specified field,
        /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the field does not exist.
        /// </returns>

        public INamedValue GetValue(string name) { return Definition.GetField(name, DataRecord.Data); }

        //  ---------------
        //  SetValue method
        //  ---------------

        /// <summary>
        /// Saves the specified field in the storage.
        /// </summary>
        /// <param name="value">The field to save.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is a <c>null</c> reference.
        /// </exception>

        public void SetValue(INamedValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            DataRecord.Data = Definition.SetField(value.Name, DataRecord.Data, value.Value as string);
        }

        //  ------------------
        //  DeleteValue method
        //  ------------------

        /// <summary>
        /// Deletes the field with the specified name.
        /// </summary>
        /// <param name="name">The name of the field to delete.</param>
        /// <returns>
        /// <c>true</c> when field was deleted
        /// or
        /// <c>false</c> when a field with the specified name does not exist.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// This method is not implemented yet.
        /// </exception>

        public bool DeleteValue(string name) { throw new NotImplementedException(); }

        #endregion IValueStorage implementation
    }
}

// eof "IDocSegment.cs"
