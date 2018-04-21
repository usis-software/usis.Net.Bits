using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Framework
{
    //  -----------------------
    //  NamespaceMetadata class
    //  -----------------------

    [XmlRoot(Constants.NamespaceXmlElementName)]
    public sealed class NamespaceMetadata : MetadataContainer
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal NamespaceMetadata() { }

        public NamespaceMetadata(string name) : base(name) { }

        //public NamespaceMetadata(string name, params MetadataBase[] items) : this(name) { AddRange(items); }

        #endregion construction

        public IEnumerable<NamespaceMetadata> Namespaces => ItemsOfType<NamespaceMetadata>();

        #region methods

        //  ----------
        //  Add method
        //  ----------

        public NamespaceMetadata Add(NamespaceMetadata item) { return base.Add(item); }

        public EntityMetadata Add(EntityMetadata item) { return base.Add(item); }

        #endregion methods

        #region overrides

        //  ---------------
        //  WriteXml method
        //  ---------------

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            SerializeItems(writer);
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
            return string.Format(CultureInfo.InvariantCulture, "namespace {0}", Name);
        }

        #endregion overrides
    }
}
