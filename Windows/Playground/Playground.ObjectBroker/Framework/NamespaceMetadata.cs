using System.Xml;
using System.Xml.Serialization;

namespace usis.Framework
{
    //  -----------------------
    //  NamespaceMetadata class
    //  -----------------------

    [XmlRoot("namespace")]
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

        #region methods

        //  ----------
        //  Add method
        //  ----------

        public NamespaceMetadata Add(NamespaceMetadata item)
        {
            return base.Add(item);
        }

        //public void Add(MetadataBase item)
        //{
        //    if (item == null) throw new ArgumentNullException(nameof(item));
        //    _items.Add(item.Name, item);
        //    item.Container = this;
        //}

        //  ---------------
        //  AddRange method
        //  ---------------

        //public void AddRange(IEnumerable<MetadataBase> items)
        //{
        //    if (items == null) throw new ArgumentNullException(nameof(items));
        //    foreach (var item in items) Add(item);
        //}

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

        #endregion overrides
    }
}
