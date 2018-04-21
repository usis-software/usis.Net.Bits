using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using usis.Platform;

namespace usis.Framework
{
    #region MetadataBase class

    //  ------------------
    //  MetadataBase class
    //  ------------------

    public abstract class MetadataBase : IXmlSerializable
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal MetadataBase() { }

        internal protected MetadataBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullOrWhiteSpaceException(nameof(name));
            Name = name;
        }

        #endregion construction

        #region properties

        //  -------------
        //  Name property
        //  -------------

        public string Name { get; }

        //  ------------------
        //  Container property
        //  ------------------

        //internal MetadataBase Container { get; set; }

        #endregion properties

        #region IXmlSerializable implementation

        //  ----------------
        //  GetSchema method
        //  ----------------

        public XmlSchema GetSchema() { return null; }

        //  --------------
        //  ReadXml method
        //  --------------

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        //  ---------------
        //  WriteXml method
        //  ---------------

        public virtual void WriteXml(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            writer.WriteAttributeString(Constants.NameXmlAttributeName, Name);
        }

        #endregion IXmlSerializable implementation
    }

    #endregion MetadataBase class

    #region MetadataContainer class

    //  -----------------------
    //  MetadataContainer class
    //  -----------------------

    public abstract class MetadataContainer : MetadataBase
    {
        #region fields

        private MetadataCollection items = new MetadataCollection();

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal MetadataContainer() { }

        internal MetadataContainer(string name) : base(name) { }

        #endregion construction

        #region methods

        //  ----------
        //  Add method
        //  ----------

        internal protected T Add<T>(T item) where T : MetadataBase
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            items.Add(item);
            return item;
        }

        //  ---------------------
        //  SerializeItems method
        //  ---------------------

        internal protected void SerializeItems(XmlWriter writer)
        {
            items.Serialize(writer);
        }

        #endregion methods
    }

    #endregion MetadataContainer class

    #region MetadataCollection class

    //  ------------------------
    //  MetadataCollection class
    //  ------------------------

    internal class MetadataCollection : KeyedCollection<string, MetadataBase>
    {
        #region overrides

        //  --------------------
        //  GetKeyForItem method
        //  --------------------

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>

        protected override string GetKeyForItem(MetadataBase item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return item.Name;
        }

        #endregion overrides
    }

    #endregion MetadataCollection class

    #region Extensions class

    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        //  ----------------
        //  Serialize method
        //  ----------------

        internal static void Serialize<T>(this IEnumerable<T> items, XmlWriter writer)
        {
            foreach (var item in items)
            {
                var serializer = new XmlSerializer(item.GetType());
                serializer.Serialize(writer, item);
            }
        }
    }

    #endregion Extensions class
}
