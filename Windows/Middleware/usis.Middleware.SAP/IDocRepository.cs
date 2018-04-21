//
//  @(#) IDocRepository.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace usis.Middleware.SAP
{
    //  --------------------
    //  IDocRepository class
    //  --------------------

    // function module to read IDoc metadata: IDOC_TYPE_COMPLETE_READ

    /// <summary>
    /// Provides a repository with definitions that describe the structure of
    /// intermediate documents (IDocs) and their data segments.
    /// </summary>

    public sealed class IDocRepository
    {
        #region fields

        /// <summary>
        /// The dictionary of segment definitions indexed by document type and segment type.
        /// </summary>

        private Dictionary<string, Dictionary<string, IDocSegmentDefinition>> segmentDictionary = new Dictionary<string, Dictionary<string, IDocSegmentDefinition>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The dictionary of document definitions indexed by document type.
        /// </summary>

        private Dictionary<string, IDocDefinition> documents = new Dictionary<string, IDocDefinition>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocRepository"/> class.
        /// </summary>

        public IDocRepository() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocRepository"/> class
        /// with a path for a XML schema file to read metadata from.
        /// </summary>
        /// <param name="path">
        /// The path for the XML schema file that describes an intermediate document.
        /// </param>

        [Obsolete("Use the ReadXmlSchema extension method insted.")]
        public IDocRepository(string path) { Read(path); }

        #endregion construction

        #region methods

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads the definition of intermediate documents from the specified XML schema.
        /// </summary>
        /// <param name="schema">The XML schema that describes the intermediate documents.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="schema"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public void Read(XmlSchema schema)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));

            foreach (var idoc in IterateIDocs(schema))
            {
                var document = new IDocDefinition(idoc.Item1.Name);
                foreach (var segment in IterateSegments(idoc.Item2))
                {
                    if (segment.Name.Equals(IDocControlRecord.TableName, StringComparison.Ordinal))
                    {
                        // ignore the IDoc control record
                        continue;
                    }
                    document.AddSegment(ReadSegment(idoc.Item1.Name, null, segment));
                }
                documents[idoc.Item1.Name] = document;
            }
        }

        /// <summary>
        /// Reads the definition of intermediate documents from the specified file.
        /// </summary>
        /// <param name="path">
        /// The path for the XML schema file that describes intermediate documents.
        /// </param>

        [Obsolete("Use the ReadXmlSchema extension method insted.")]
        public void Read(string path)
        {
            using (var reader = new XmlTextReader(path))
            {
                Read(XmlSchema.Read(reader, null));
            }
        }

        //  ----------------------------
        //  AddDocumentDefinition method
        //  ----------------------------

        internal void AddDocumentDefinition(IDocDefinition definition)
        {
            System.Diagnostics.Debug.Assert(definition != null);

            foreach (var segment in definition.Segments.EnumerateSegmentDefinitions())
            {
                AddSegmentDefinition(definition.DocumentType, segment);
            }
            documents[definition.DocumentType] = definition;
        }

        //  ----------------------------
        //  FindSegmentDefinition method
        //  ----------------------------

        internal IDocSegmentDefinition FindSegmentDefinition(string documentType, string segmentName)
        {
            if (segmentDictionary.TryGetValue(documentType, out Dictionary<string, IDocSegmentDefinition> allRecords))
            {
                if (allRecords.TryGetValue(segmentName, out IDocSegmentDefinition definition))
                {
                    return definition;
                }
            }
            return null;
        }

        //  -----------------------------
        //  FindDocumentDefinition method
        //  -----------------------------

        /// <summary>
        /// Retrieves the definition for the specified IDoc type.
        /// </summary>
        /// <param name="documentType">Type of the IDoc.</param>
        /// <returns>
        /// The definition for the specified IDoc type, or <c>null</c> if the defintion was not found.
        /// </returns>

        public IDocDefinition FindDocumentDefinition(string documentType)
        {
            if (documentType == null) throw new ArgumentNullException(nameof(documentType));
            return documents.TryGetValue(documentType, out IDocDefinition definition) ? definition : null;
        }

        #endregion methods

        #region private methods

        //  ---------------------------
        //  AddSegmentDefinition method
        //  ---------------------------

        private void AddSegmentDefinition(string documentType, IDocSegmentDefinition definition)
        {
            if (!segmentDictionary.TryGetValue(documentType, out Dictionary<string, IDocSegmentDefinition> segments))
            {
                segments = new Dictionary<string, IDocSegmentDefinition>(StringComparer.OrdinalIgnoreCase);
                segmentDictionary.Add(documentType, segments);
            }
            segments[definition.SegmentType] = definition;
        }

        #region private methods to parse XML schemas

        //  ------------------
        //  ReadSegment method
        //  ------------------

        private IDocSegmentDefinition ReadSegment(string documentType, IDocSegmentDefinition parent, XmlSchemaElement segment)
        {
            var definition = new IDocSegmentDefinition(segment, parent);
            BuildRecordDefinition(definition, segment);
            foreach (var item in IterateSegments(segment))
            {
                IDocSegmentDefinition child = ReadSegment(documentType, definition, item);
                definition.Children.Add(child);
            }
            AddSegmentDefinition(documentType, definition);
            return definition;
        }

        //  ----------------------------
        //  BuildRecordDefinition method
        //  ----------------------------

        private static void BuildRecordDefinition(IDocRecordDefinition record, XmlSchemaElement segment)
        {
            foreach (var field in IterateFields(segment))
            {
                if (field.SchemaType is XmlSchemaSimpleType simpleType)
                {
                    if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
                    {
                        foreach (var item in restriction.Facets)
                        {
                            if (item is XmlSchemaMaxLengthFacet maxLength)
                            {
                                record.AddField(field.Name, int.Parse(maxLength.Value, CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
            record.ReIndex();
        }

        //  --------------------
        //  IterateFields method
        //  --------------------

        private static IEnumerable<XmlSchemaElement> IterateFields(XmlSchemaElement segment)
        {
            foreach (var element in IterateSchemaElements(segment))
            {
                if (element.SchemaType is XmlSchemaSimpleType)
                {
                    yield return element;
                }
            }
        }

        //  -------------------
        //  IterateIDocs method
        //  -------------------

        private static IEnumerable<Tuple<XmlSchemaElement, XmlSchemaElement>> IterateIDocs(XmlSchema schema)
        {
            foreach (var item in IterateSchemaElements(schema.Items))
            {
                var name = item.Name;
                foreach (var subItem in IterateSchemaElements(item))
                {
                    if (subItem.Name.Equals("IDOC", StringComparison.OrdinalIgnoreCase))
                    {
                        var attributes = GetAttributes(subItem);
                        if (attributes.ContainsKey("BEGIN"))
                        {
                            yield return new Tuple<XmlSchemaElement, XmlSchemaElement>(item, subItem);
                        }
                    }
                }
            }
        }

        //  ----------------------
        //  IterateSegments method
        //  ----------------------

        private static IEnumerable<XmlSchemaElement> IterateSegments(XmlSchemaElement element)
        {
            foreach (var segment in IterateSchemaElements(element))
            {
                var attributes = GetAttributes(segment);
                if (attributes.ContainsKey("SEGMENT"))
                {
                    yield return segment;
                }
            }
        }

        //  ----------------------------
        //  IterateSchemaElements method
        //  ----------------------------

        private static IEnumerable<XmlSchemaElement> IterateSchemaElements(XmlSchemaElement element)
        {
            if (element.SchemaType is XmlSchemaComplexType complexType)
            {
                if (complexType.Particle is XmlSchemaSequence sequence)
                {
                    foreach (var item in IterateSchemaElements(sequence.Items))
                    {
                        yield return item;
                    }
                }
            }
        }

        private static IEnumerable<XmlSchemaElement> IterateSchemaElements(XmlSchemaObjectCollection items)
        {
            foreach (XmlSchemaObject item in items)
            {
                if (item is XmlSchemaElement element) yield return element;
            }
        }

        //  --------------------
        //  GetAttributes method
        //  --------------------

        private static Dictionary<string, XmlSchemaAttribute> GetAttributes(XmlSchemaElement element)
        {
            var attributes = new Dictionary<string, XmlSchemaAttribute>(StringComparer.OrdinalIgnoreCase);
            foreach (var attribute in IterateAttributes(element))
            {
                attributes.Add(attribute.Name, attribute);
            }
            return attributes;
        }

        //  ------------------------
        //  IterateAttributes method
        //  ------------------------

        private static IEnumerable<XmlSchemaAttribute> IterateAttributes(XmlSchemaElement element)
        {
            var complexType = element.SchemaType as XmlSchemaComplexType;
            if (complexType == null) yield break;
            foreach (var item in complexType.Attributes)
            {
                if (item is XmlSchemaAttribute attribute) yield return attribute;
            }
        }

        #endregion private methods to parse XML schemas

        #endregion private methods
    }
}

// eof "IDocRepository.cs"
