//
//  @(#) IDocSegmentDefinition.cs
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
using System.Xml.Serialization;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  ---------------------------
    //  IDocSegmentDefinition class
    //  ---------------------------

    /// <summary>
    /// Describes an intermediate document segment.
    /// </summary>

    [XmlRoot(XmlElementName.Segment)]
    public sealed class IDocSegmentDefinition : IDocRecordDefinition, IXmlSerializable
    {
        #region fields

        private List<IDocSegmentDefinition> children = new List<IDocSegmentDefinition>();

        #endregion fields

        #region properties

        //  --------------------
        //  SegmentType property
        //  --------------------

        /// <summary>
        /// Gets the type name of the segment definition.
        /// </summary>
        /// <value>
        /// The type name of the segment definition.
        /// </value>

        public string SegmentType { get; private set; }

        #region TODO

        //  --------------------
        //  Description property
        //  --------------------

        //internal string Description { get; set; }

        #endregion TODO

        //  --------------------------
        //  SegmentDefinition property
        //  --------------------------

        internal string SegmentDefinition { get; private set; }

        //  ---------------
        //  Parent property
        //  ---------------

        /// <summary>
        /// Gets the parent segment.
        /// </summary>
        /// <value>
        /// The parent segment.
        /// </value>

        public IDocSegmentDefinition Parent { get; private set; }

        //  -----------------
        //  Children property
        //  -----------------

        /// <summary>
        /// Gets the subordinate segment definitions of this segment definition.
        /// </summary>
        /// <value>
        /// A collection of the subordinate segment definitions of this segment definition.
        /// </value>

        internal ICollection<IDocSegmentDefinition> Children => children;

        //  ------------------
        //  MinOccurs property
        //  ------------------

        internal int MinOccurs { get; private set; }

        //  ------------------
        //  MaxOccurs property
        //  ------------------

        internal int MaxOccurs { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        private IDocSegmentDefinition(string segmentType) { SegmentType = segmentType; }

        internal IDocSegmentDefinition(XmlSchemaElement element, IDocSegmentDefinition parent) : this(element.Name)
        {
            Parent = parent;
            MinOccurs = Convert.ToInt32(element.MinOccurs);
            MaxOccurs = Convert.ToInt32(element.MaxOccurs);
        }

        #endregion construction

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
            return string.Concat(
                string.Format(CultureInfo.InvariantCulture, "SegmentType=\"{0}\"; ", SegmentType),
                base.ToString(),
                string.Format(CultureInfo.InvariantCulture, "; Children.Count = {0}", children.Count));
        }

        #endregion overrides

        #region IXmlSerializable implementation

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocSegmentDefinition"/> class.
        /// </summary>
        /// <remarks>This constructor is needed to allow XML serialization.</remarks>

        internal IDocSegmentDefinition() { }

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

            SegmentType = reader.GetAttribute(XmlAttributeName.Type);
            SegmentDefinition = reader.GetAttribute(XmlAttributeName.Definition);
            MinOccurs = GetInt32Attribute(reader, XmlAttributeName.MinOccurs, 1);
            MaxOccurs = GetInt32Attribute(reader, XmlAttributeName.MaxOccurs, 1);

            var segmentSerializer = new XmlSerializer(typeof(IDocSegmentDefinition));
            var fieldSerializer = new XmlSerializer(typeof(IDocFieldDefinition));

            reader.ReadElement(() =>
            {
                reader.ReadChildren(() =>
                {
                    if (XmlElementName.Segment.Equals(reader.Name, StringComparison.Ordinal))
                    {
                        if (segmentSerializer.Deserialize(reader) is IDocSegmentDefinition segment)
                        {
                            children.Add(segment);
                            segment.Parent = this;
                        }
                    }
                    else if (XmlElementName.Field.Equals(reader.Name, StringComparison.Ordinal))
                    {
                        AddField(fieldSerializer.Deserialize(reader) as IDocFieldDefinition);
                    }
                    else return false;
                    return true;
                });
            });
        }

        //  ---------------
        //  WriteXml method
        //  ---------------

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString(XmlAttributeName.Type, SegmentType);
            if (!string.IsNullOrWhiteSpace(SegmentDefinition)) writer.WriteAttributeString(XmlAttributeName.Definition, SegmentDefinition);
            if (MinOccurs != 1) writer.WriteAttributeString(XmlAttributeName.MinOccurs, MinOccurs.ToString(CultureInfo.InvariantCulture));
            if (MaxOccurs > 1) writer.WriteAttributeString(XmlAttributeName.MaxOccurs, MaxOccurs.ToString(CultureInfo.InvariantCulture));

            Fields.SerializeAsXml(writer);
            Children.SerializeAsXml(writer);
        }

        //  ------------------------
        //  GetInt32Attribute method
        //  ------------------------

        private static int GetInt32Attribute(XmlReader reader, string name, int defaultValue)
        {
            var s = reader.GetAttribute(name);
            return string.IsNullOrWhiteSpace(s) ? defaultValue : Int32.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        #endregion IXmlSerializable implementation
    }
}

// eof "IDocSegmentDefinition.cs"
