//
//  @(#) IDocFieldDefinition.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  -------------------------
    //  IDocFieldDefinition class
    //  -------------------------

    /// <summary>
    /// Describes a field in a segment of an intermediate document.
    /// </summary>

    [XmlRoot(XmlElementName.Field)]
    public sealed class IDocFieldDefinition : IXmlSerializable
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal IDocFieldDefinition(string name, int length)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullOrWhiteSpaceException(nameof(name));
            if (length < 1) throw new ArgumentOutOfRangeException(nameof(length));

            Name = name;
            Length = length;
        }

        internal IDocFieldDefinition(string name, IDocFieldDefinition field)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullOrWhiteSpaceException(nameof(name));
            Name = name;
            Length = field.Length;
            StartIndex = field.StartIndex;
        }

        #endregion construction

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>

        public string Name { get; private set; }

        //  ---------------
        //  Length property
        //  ---------------

        /// <summary>
        /// Gets the length of the field in characters.
        /// </summary>
        /// <value>
        /// The length of the field in characters.
        /// </value>

        public int Length { get; private set; }

        //  -------------------
        //  StartIndex property
        //  -------------------

        /// <summary>
        /// Gets the starting position of the field in its segment's data field.
        /// </summary>
        /// <value>
        /// The starting position of the field in its segment's data field.
        /// </value>
        /// <remarks>
        /// The <see cref="StartIndex"/> returns <c>null</c> if the field positions have not yet been calculated.
        /// </remarks>

        internal int? StartIndex { get; set; }

        #endregion properties

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
            return string.Format(CultureInfo.InvariantCulture, "{0} (StartIndex = {1}, Length = {2})", Name, StartIndex, Length);
        }

        #endregion overrides

        #region IXmlSerializable implementation

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="IDocFieldDefinition"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is needed to allow XML serialization.
        /// </remarks>

        internal IDocFieldDefinition() { }

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

            Name = reader.GetAttribute(XmlAttributeName.Name);
            Length = Convert.ToInt32(reader.GetAttribute(XmlAttributeName.Length), CultureInfo.InvariantCulture);

            reader.ReadElement(() =>
            {
                reader.ReadChildren(() => { return false; });
            });
        }

        //  ---------------
        //  WriteXml method
        //  ---------------

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteAttributeString(XmlAttributeName.Name, Name);
            writer.WriteAttributeString(XmlAttributeName.Length, Convert.ToString(Length, CultureInfo.InvariantCulture));
        }

        #endregion IXmlSerializable implementation
    }
}

// eof "IDocFieldDefinition.cs"
