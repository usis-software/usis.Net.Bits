using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Framework
{
    //  --------------------
    //  EntityMetadata class
    //  --------------------

    [XmlRoot("entity")]
    public sealed class EntityMetadata : MetadataBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        //internal EntityMetadata() { }

        public EntityMetadata(string name) : base(name) { }

        public EntityMetadata(string name, Guid id) : this(name) { Id = id; }

        public EntityMetadata(string name, params AttributeMetadata[] attributes) : this(name) { AddRange(attributes); }

        public EntityMetadata(string name, Guid id, params AttributeMetadata[] attributes) : this(name, id) { AddRange(attributes); }

        #endregion construction

        #region fields

        private Dictionary<string, AttributeMetadata> _attributes = new Dictionary<string, AttributeMetadata>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region properties

        //  -----------
        //  Id property
        //  -----------

        public Guid? Id { get; }

        //  -------------------
        //  IsAbstract property
        //  -------------------

        public bool IsAbstract { get; internal set; }

        public EntityMetadata BaseEntity { get; internal set; }

        #endregion properties

        #region methods

        //  ----------
        //  Add method
        //  ----------

        public void Add(AttributeMetadata attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            _attributes.Add(attribute.Name, attribute);
        }

        //  ---------------
        //  AddRange method
        //  ---------------

        public void AddRange(IEnumerable<AttributeMetadata> attributes)
        {
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));
            foreach (var attribute in attributes) Add(attribute);
        }

        #endregion methods

        #region overrides

        //  ---------------
        //  WriteXml method
        //  ---------------

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            if (Id.HasValue) writer.WriteAttributeString(Constants.IdXmlAttributeName, Id.Value.ToString("D"));
            if (IsAbstract) writer.WriteAttributeString(Constants.AbstractXmlAttributeName, true.ToString());
            if (BaseEntity != null) writer.WriteAttributeString(Constants.BaseXmlAttributeName, BaseEntity.BuildQualifiedName());

            _attributes.Values.Serialize(writer);
        }

        #endregion overrides
    }

    //  -----------------------------
    //  AttributeDataType enumeration
    //  -----------------------------

    public enum AttributeDataType
    {
        String,
        Id,
        DateTime,
        Boolean
    }

    //  -----------------------
    //  AttributeMetadata class
    //  -----------------------

    [XmlRoot("attribute")]
    public sealed class AttributeMetadata : MetadataBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        //internal AttributeMetadata() { }

        public AttributeMetadata(string name, AttributeDataType dataType) : base(name) { DataType = dataType; }

        public AttributeMetadata(string name, AttributeDataType dataType, bool nullable) : this(name, dataType) { IsNullable = nullable; }

        #endregion construction

        #region properties

        //  -----------------
        //  DataType property
        //  -----------------

        public AttributeDataType DataType { get; }

        //  -------------------
        //  IsNullable property
        //  -------------------

        public bool IsNullable { get; internal set; }

        #endregion properties

        #region overrides

        //  ---------------
        //  WriteXml method
        //  ---------------

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteAttributeString(Constants.DataTypeXmlAttributeName, DataType.ToString());
            if (IsNullable) writer.WriteAttributeString(Constants.NullableXmlAttributeName, IsNullable.ToString());
        }

        #endregion overrides
    }

    //  ----------------
    //  Extensions class
    //  ----------------

    //internal static class Extensions
    //{
    //    internal static void Serialize<T>(this IEnumerable<T> items, XmlWriter writer)
    //    {
    //        foreach (var item in items)
    //        {
    //            var serializer = new XmlSerializer(item.GetType());
    //            serializer.Serialize(writer, item);
    //        }
    //    }

    //    public static EntityMetadata AsAbstract(this EntityMetadata entity)
    //    {
    //        entity.IsAbstract = true;
    //        return entity;
    //    }

    //    public static AttributeMetadata AsNullable(this AttributeMetadata attribute, bool nullable)
    //    {
    //        attribute.IsNullable = nullable;
    //        return attribute;
    //    }

    //    public static EntityMetadata DerivesFrom(this EntityMetadata entity, EntityMetadata baseEntity)
    //    {
    //        entity.BaseEntity = baseEntity;
    //        return entity;
    //    }

    //    internal static string BuildQualifiedName(this MetadataBase item)
    //    {
    //        var sb = new StringBuilder(item.Name);
    //        var container = item.Container;
    //        while (container != null)
    //        {
    //            sb.Insert(0, '.');
    //            sb.Insert(0, container.Name);
    //            container = container.Container;
    //        }
    //        return sb.ToString();
    //    }
    //}
}
