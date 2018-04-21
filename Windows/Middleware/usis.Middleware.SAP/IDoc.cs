//
//  @(#) IDoc.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace usis.Middleware.SAP
{
    //  ----------
    //  IDoc class
    //  ----------

    /// <summary>
    /// Represents an intermediate document (IDoc).
    /// </summary>

    public sealed class IDoc
    {
        #region constants

        /// <summary>
        /// The extension for IDoc file names.
        /// </summary>

        public const string FileNameExtension = "idoc";

        #endregion constants

        #region fields

        private List<IDocSegment> children = new List<IDocSegment>();

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDoc"/> class
        /// with the specified definition.
        /// </summary>
        /// <param name="definition">The IDoc definition.</param>

        public IDoc(IDocDefinition definition) : this(new IDocControlRecord(definition))
        {
            //CreateSegments(childen, definition.Segments);
            ControlRecord.Created = DateTime.UtcNow;
        }

        private IDoc(IDocControlRecord controlRecord)
        {
            ControlRecord = controlRecord;
            ControlRecord.DataStringChanged += (r, e) =>
            {
                if (e.Record is IDocControlRecord record)
                {
                    if (e.Index == 1) foreach (var segment in SegmentSequence) segment.DataRecord.Client = record.Client;
                    if (e.Index == 2) foreach (var segment in SegmentSequence) segment.DataRecord.DocumentNumber = record.DocumentNumber;
                }
            };
        }

        #endregion construction

        #region properties

        //  ----------------------
        //  ControlRecord property
        //  ----------------------

        /// <summary>
        /// Gets the control record of the intermediate document.
        /// </summary>
        /// <value>
        /// The control record of the intermediate document.
        /// </value>

        public IDocControlRecord ControlRecord { get; }

        //  ------------------------
        //  SegmentSequence property
        //  ------------------------

        /// <summary>
        /// Gets an enumerator to iterate all segments of the intermediate document recursively.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all segments of the intermediate document recursively.
        /// </value>

        public IEnumerable<IDocSegment> SegmentSequence => EnumerateSegmentAndChildren(children);

        #endregion properties

        #region methods

        //  --------------------
        //  ReadDocuments method
        //  --------------------

        /// <summary>
        /// Reads intermediate documents from the specified reader.
        /// </summary>
        /// <param name="reader">The reader to read documents from.</param>
        /// <returns>
        /// An enumerator to iterate the intermediate documents.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>

        public static IEnumerable<IDoc> ReadDocuments(IDocReader reader)
        {
            return ReadDocuments(reader, null);
        }

        /// <summary>
        /// Reads intermediate documents from the specified reader.
        /// </summary>
        /// <param name="reader">The reader to read documents from.</param>
        /// <param name="repository">The repository to retrieve definitions from.</param>
        /// <returns>
        /// An enumerator to iterate the intermediate documents.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader" /> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>

        public static IEnumerable<IDoc> ReadDocuments(IDocReader reader, IDocRepository repository)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            IDoc document = null;
            IDocSegment currentSegment = null;

            while (reader.Read())
            {
                var record = reader.CurrentDataRecord;
                if (reader.CurrentControlRecord == null) throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.NoControlRecord, record.DocumentNumber));

                #region handle documents

                if (document == null)
                {
                    // this is the first document
                    document = new IDoc(reader.CurrentControlRecord);
                }
                else if (!document.ControlRecord.DocumentNumber.Equals(reader.CurrentControlRecord.DocumentNumber))
                {
                    // a new document started; return previous one; TODO: was the previous valid?
                    yield return document;

                    // prepare next document
                    document = new IDoc(reader.CurrentControlRecord);
                    currentSegment = null;
                }

                #endregion handle documents

                var segment = new IDocSegment(document, record, repository);

                #region check sequence

                var number = currentSegment == null ? 1 : currentSegment.DataRecord.SegmentNumber + 1;
                if (segment.DataRecord.SegmentNumber != number) throw new Exception(Strings.WrongSegmentNumber);

                #endregion check sequence

                #region repository checks

                //if (reader.Repository != null)
                //{
                //    // TODO: check whether segment is allowed at this point
                //}

                #endregion repository checks

                #region handle segment hierarchy

                ICollection<IDocSegment> collection = null;
                IDocSegment parent = null;

                if (reader.Options.HasFlag(IDocReaderOptions.IgnoreHierarchyLevel))
                {
                    collection = document.children;
                }
                else
                {
                    var level = currentSegment == null ? segment.DataRecord.HierarchyLevel : currentSegment.DataRecord.HierarchyLevel;
                    if (segment.DataRecord.HierarchyLevel == level)
                    {
                        parent = currentSegment?.Parent;
                        collection = parent == null ? document.children : parent.children;
                    }
                    else if (segment.DataRecord.HierarchyLevel == level + 1 && currentSegment != null)
                    {
                        parent = currentSegment;
                        collection = currentSegment.children;
                    }
                    else if (segment.DataRecord.HierarchyLevel > 0 && segment.DataRecord.HierarchyLevel < level && currentSegment != null)
                    {
                        parent = currentSegment.Parent;
                        for (int i = 0; i < level - segment.DataRecord.HierarchyLevel; i++)
                        {
                            if (parent == null) throw new Exception(Strings.InvalidSegmentHierarchy);
                            parent = parent.Parent;
                        }
                        collection = parent.children;
                    }
                    else throw new Exception(Strings.InvalidHierarchyLevel);
                }

                #endregion handle segment hierarchy

                #region add segment

                segment.Parent = parent;
                Debug.Assert(collection != null);
                if (collection != null) collection.Add(segment);

                #endregion add segment

                currentSegment = segment;
            }
            if (document != null) yield return document;
        }

        //  -----------------
        //  AddSegment method
        //  -----------------

        /// <summary>
        /// Creates a new segment and adds the segment to the root segments of the document.
        /// </summary>
        /// <param name="definition">The definition that specifies the segment type.</param>
        /// <returns>
        /// The newly created segment.
        /// </returns>

        public IDocSegment AddSegment(IDocSegmentDefinition definition)
        {
            var segment = new IDocSegment(this, definition);
            children.Add(segment);
            RenumberSegments();
            return segment;
        }

        //  -----------------------
        //  RenumberSegments method
        //  -----------------------

        internal void RenumberSegments()
        {
            int i = 1;
            foreach (var segment in SegmentSequence)
            {
                segment.DataRecord.SegmentNumber = i++;
                segment.DataRecord.ParentSegmentNumber = segment.Parent == null ? 0 : segment.Parent.DataRecord.SegmentNumber;
                segment.DataRecord.HierarchyLevel = (byte)(segment.Parent == null ? 2 : segment.Parent.DataRecord.HierarchyLevel + 1);
            }
        }

        //  ---------------------
        //  CreateSegments method
        //  ---------------------

        //private void CreateSegments(List<IDocSegment> list, IEnumerable<IDocSegmentDefinition> segmentDefinitions)
        //{
        //    foreach (var segmentDefinition in segmentDefinitions)
        //    {
        //        if (segmentDefinition.MinOccurs == 0) continue;
        //        for (int i = 0; i < segmentDefinition.MinOccurs; i++)
        //        {
        //            var segment = new IDocSegment(this, segmentDefinition);
        //            list.Add(segment);
        //            CreateSegments(segment.children, segmentDefinition.Children);
        //        }
        //    }
        //}

        //  ----------------------------------
        //  EnumerateSegmentAndChildren method
        //  ----------------------------------

        private IEnumerable<IDocSegment> EnumerateSegmentAndChildren(IEnumerable<IDocSegment> segments)
        {
            foreach (var segment in segments)
            {
                yield return segment;
                foreach (var child in EnumerateSegmentAndChildren(segment.children))
                {
                    yield return child;
                }
            }
        }

        //  --------------------------------
        //  CreateDocumentDescription method
        //  --------------------------------

        /// <summary>
        /// Creates a string that describes an IDoc.
        /// </summary>
        /// <param name="documentType">The type of the document.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns>
        /// A string that describes an IDoc.
        /// </returns>

        private static string CreateDescription(string documentType, string documentNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}#{1}", documentType, documentNumber);
        }

        //  ---------------------
        //  CreateFileName method
        //  ---------------------

        /// <summary>
        /// Creates a string that providesa file name for an IDoc.
        /// </summary>
        /// <param name="documentType">The type of the document.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns>
        /// A string that provides a file name for an IDoc.
        /// </returns>
        /// <remarks>
        /// The file name consists of a description string as returned by <see cref="CreateDescription(string, string)"/>
        /// combined with the extension <see cref="FileNameExtension"/>.
        /// </remarks>

        internal static string CreateFileName(string documentType, string documentNumber)
        {
            return Path.ChangeExtension(CreateDescription(documentType, documentNumber), FileNameExtension);
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
        /// A <see cref="string"/> in the following format:
        /// <c>
        /// <see cref="IDocControlRecord.DocumentType"/>
        /// <b>#</b>
        /// <see cref="IDocControlRecord.DocumentNumber"/>
        /// </c>.
        /// </returns>

        public override string ToString()
        {
            return CreateDescription(ControlRecord.DocumentType, ControlRecord.DocumentNumber);
        }

        #endregion overrides
    }
}

// eof "IDoc.cs"
