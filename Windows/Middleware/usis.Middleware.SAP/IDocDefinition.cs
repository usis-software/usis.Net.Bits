//
//  @(#) IDocDefinition.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using System.Globalization;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  --------------------
    //  IDocDefinition class
    //  --------------------

    /// <summary>
    /// Provides metadata information about the structure of an IDoc type.
    /// </summary>

    [XmlRoot(XmlElementName.IDoc)]
    public sealed class IDocDefinition : IXmlSerializable
    {
        #region fields

        private List<IDocSegmentDefinition> children = new List<IDocSegmentDefinition>();
        private Dictionary<string, IDocSegmentDefinition> segmentDictionary = new Dictionary<string, IDocSegmentDefinition>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region properties

        //  ---------------------
        //  DocumentType property
        //  ---------------------

        internal string DocumentType { get; private set; }

        #region TODO

        //  --------------------
        //  Description property
        //  --------------------

        //internal string Description { get; set; }

        #endregion TODO

        //  -----------------
        //  Segments property
        //  -----------------

        /// <summary>
        /// Gets the root segment definitions of an IDoc definition.
        /// </summary>
        /// <value>
        /// The root segment definitions of an IDoc definition.
        /// </value>

        public IEnumerable<IDocSegmentDefinition> Segments => children.AsEnumerable();

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal IDocDefinition(string documentType) { DocumentType = documentType; }

        #endregion construction

        #region methods

        //  -----------------
        //  AddSegment method
        //  -----------------

        internal void AddSegment(IDocSegmentDefinition segment)
        {
            children.Add(segment);
            segmentDictionary.Add(segment.SegmentType, segment);

            foreach (var dependant in EnumerateSegments(segment.Children))
            {
                segmentDictionary.Add(dependant.SegmentType, dependant);
            }
        }

        //  ------------------------
        //  EnumerateSegments method
        //  ------------------------

        private IEnumerable<IDocSegmentDefinition> EnumerateSegments(IEnumerable<IDocSegmentDefinition> segments)
        {
            foreach (var segment in segments)
            {
                yield return segment;
                foreach (var child in EnumerateSegments(segment.Children))
                {
                    yield return child;
                }
            }
        }

        //  ------------------
        //  FindSegment method
        //  ------------------

        /// <summary>
        /// Finds the segment definition for the specified segment type.
        /// </summary>
        /// <param name="segmentType">Type of the segment.</param>
        /// <returns>
        /// The segment definition for the specified segment type, or <c>null</c> if the segment definition was not found.
        /// </returns>

        public IDocSegmentDefinition FindSegment(string segmentType)
        {
            return segmentDictionary.TryGetValue(segmentType, out IDocSegmentDefinition definition) ? definition : null;
        }

        #endregion methods

        #region IXmlSerializable implementation

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocDefinition"/> class.
        /// </summary>
        /// <remarks>This constructor is needed to allow XML serialization.</remarks>

        internal IDocDefinition() { }

        //  ----------------
        //  GetSchema method
        //  ----------------

        XmlSchema IXmlSerializable.GetSchema() { return null; }

        //  --------------
        //  ReadXml method
        //  --------------

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            DocumentType = reader.GetAttribute(XmlAttributeName.DocumentType);

            var serializer = new XmlSerializer(typeof(IDocSegmentDefinition));
            reader.ReadElement(() =>
            {
                reader.ReadChildren(() =>
                {
                    if (XmlElementName.Segment.Equals(reader.Name))
                    {
                        AddSegment(serializer.Deserialize(reader) as IDocSegmentDefinition);
                        return true;
                    }
                    return false;
                });
            });
        }

        //  ---------------
        //  WriteXml method
        //  ---------------

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString(XmlAttributeName.DocumentType, DocumentType);
            children.SerializeAsXml(writer);
        }

        #endregion IXmlSerializable implementation

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
            return string.Format(CultureInfo.CurrentCulture, "{0}: DocumentType = '{1}'", nameof(IDocDefinition), DocumentType);
        }

        #endregion overrides
    }
}

// eof "IDocDefinition.cs"
