using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Framework
{
    #region AttributeDataType enumeration

    //  -----------------------------
    //  AttributeDataType enumeration
    //  -----------------------------

    public enum AttributeDataType
    {
        Id,
        String,
        DateTime,
        Boolean
    }

    #endregion AttributeDataType enumeration

    //  -----------------------
    //  AttributeMetadata class
    //  -----------------------

    [XmlRoot(Constants.AttributeXmlElementName)]
    public sealed class AttributeMetadata : MetadataBase, ICloneable
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal AttributeMetadata() { }

        public AttributeMetadata(string name, AttributeDataType dataType) : base(name) { DataType = dataType; }

        #endregion construction

        #region properties

        //  -----------------
        //  DataType property
        //  -----------------

        public AttributeDataType DataType { get; }

        #endregion properties

        #region overrides

        //  ---------------
        //  WriteXml method
        //  ---------------

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteAttributeString(Constants.DataTypeXmlAttributeName, DataType.ToString().ToLower(CultureInfo.InvariantCulture));
        }

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
            return string.Format(CultureInfo.InvariantCulture, "attribute {0}", Name);
        }

        #endregion overrides

        object ICloneable.Clone()
        {
            return new AttributeMetadata(Name, DataType);
        }
    }
}
