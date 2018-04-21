using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using usis.Platform;
using System.Linq;

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

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface,
        /// you should return null (Nothing in Visual Basic) from this method, and instead,
        /// if specifying a custom schema is required, apply the <see cref="XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="XmlSchema" /> that describes the XML representation of the object that is produced by the
        /// <see cref="IXmlSerializable.WriteXml(XmlWriter)" /> method and consumed by the
        /// <see cref="IXmlSerializable.ReadXml(XmlReader)" /> method.
        /// </returns>

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

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter" /> stream to which the object is serialized.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>writer</c> is a null reference.
        /// </exception>

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

        private readonly MetadataCollection Items = new MetadataCollection();

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
            Items.Add(item);
            return item;
        }

        //  ---------------
        //  AddRange method
        //  ---------------

        internal protected void AddRange(params MetadataBase[] items)
        {
            AddRange(items.AsEnumerable());
        }

        internal protected void AddRange(IEnumerable<MetadataBase> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items) { Items.Add(item); }
        }

        //  ---------------------
        //  SerializeItems method
        //  ---------------------

        internal protected void SerializeItems(XmlWriter writer)
        {
            Items.Serialize(writer);
        }

        //  ------------------
        //  ItemsOfType method
        //  ------------------

        internal protected IEnumerable<T> ItemsOfType<T>() where T : MetadataBase
        {
            return Items.OfType<T>();
        }

        #endregion methods

        internal IEnumerable<MetadataBase> CloneItems()
        {
            return Items.OfType<ICloneable>().Select((c) => { return c.Clone() as MetadataBase; });
        }
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
