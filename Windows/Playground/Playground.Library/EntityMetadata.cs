using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Framework
{
    //  --------------------
    //  EntityMetadata class
    //  --------------------

    [XmlRoot(Constants.EntityXmlElementName)]
    public sealed class EntityMetadata : MetadataContainer, ICloneable
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal EntityMetadata() { }

        private EntityMetadata(string name, IEnumerable<MetadataBase> items) : base(name)
        {
            AddRange(items);
        }

        public EntityMetadata(string name, params AttributeMetadata[] items) : base(name)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (items.Length == 0) throw new ArgumentException("", nameof(items));
            AddRange(items);
        }

        #endregion construction

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
            return string.Format(CultureInfo.InvariantCulture, "entity {0}", Name);
        }

        #endregion overrides

        object ICloneable.Clone()
        {
            return new EntityMetadata(Name, CloneItems());
        }
    }
}
