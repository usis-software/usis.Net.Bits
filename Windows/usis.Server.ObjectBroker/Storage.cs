using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using usis.Platform.Data;

namespace usis.Server.ObjectBroker
{
    public interface IStorage
    {
        IEnumerable<EntityMetadata> Entities
        {
            get;
        }
    }

    public class DataSourceStorage : IStorage
    {
        #region constants

        private const string TablesCollectionName = "Tables";
        private const string CollectionNameColumnName = "CollectionName";
        private const string TableCatalogColumnName = "table_catalog";
        private const string TableSchemaColumnName = "table_schema";
        private const string TableNameColumnName = "table_name";

        #endregion constants

        #region fields

        private DataSource dataSource;
        private StorageMetadata metadata;
        private HashSet<string> collectionNames;

        #endregion fields

        #region properties

        //  -------------------
        //  DataSource property
        //  -------------------

        private StorageMetadata Metadata
        {
            get
            {
                if (metadata == null)
                {
                    metadata = new StorageMetadata();
                    SyncMetadata();
                }
                return metadata;
            }
        }

        //  -----------------
        //  Entities property
        //  -----------------

        public IEnumerable<EntityMetadata> Entities { get { return Metadata.Entities; } }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public DataSourceStorage(DataSource dataSource)
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            this.dataSource = dataSource;
        }

        #endregion construction

        #region methods

        //  ------------------------
        //  SerializeMetadata method
        //  ------------------------

        public void SerializeMetadata(TextWriter writer)
        {
            var serializer = new XmlSerializer(typeof(StorageMetadata));
            serializer.Serialize(writer, Metadata);
        }

        //  --------------------------
        //  DeserializeMetadata method
        //  --------------------------

        public void DeserializeMetadata(TextReader reader)
        {
            var serializer = new XmlSerializer(typeof(StorageMetadata));
            metadata = serializer.Deserialize(reader) as StorageMetadata;
        }

        //  -------------------
        //  SyncMetadata method
        //  -------------------

        public void SyncMetadata()
        {
            if (metadata == null) metadata = new StorageMetadata();
            using (var connection = dataSource.OpenConnection())
            {
                foreach (var item in SyncEntities(connection))
                {
                    Debug.Print("SyncMetadata enity '{0}'", item.Name);
                    //SyncAttributes(connection, item);
                }
            }
        }

        #endregion methods

        #region private methods

        //  -------------------
        //  SyncEntities method
        //  -------------------

        private IEnumerable<EntityMetadata> SyncEntities(DbConnection connection)
        {
            foreach (var name in LoadEntityNames(connection))
            {
                var entity = metadata.GetEntity(name);
                if (entity == null)
                {
                    entity = new EntityMetadata()
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };
                    metadata.Entities.Add(entity);
                    yield return entity;
                }
            }
        }

        //  ----------------------
        //  LoadEntityNames method
        //  ----------------------

        private IEnumerable<string> LoadEntityNames(DbConnection connection)
        {
            if (!IsValidCollection(connection, TablesCollectionName)) yield break;
            var collection = connection.GetSchema(TablesCollectionName);
            foreach (DataRow row in collection.Rows)
            {
                var catalog = row[TableCatalogColumnName] as string;
                var schema = row[TableSchemaColumnName] as string;
                var name = row[TableNameColumnName] as string;
                string entityName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", catalog, schema, name);
                if (!string.IsNullOrWhiteSpace(entityName)) yield return entityName;
            }
        }

        //  ------------------------
        //  IsValidCollection method
        //  ------------------------

        private bool IsValidCollection(DbConnection connection, string name)
        {
            if (collectionNames == null)
            {
                collectionNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var schema = connection.GetSchema();
                foreach (DataRow row in schema.Rows)
                {
                    collectionNames.Add(row[CollectionNameColumnName] as string);
                }
            }
            return collectionNames.Contains(name);
        }

        #endregion private methods

        //private static void SyncAttributes(DbConnection connection, EntityMetadata entity)
        //{
        //    var schema = connection.GetSchema();
        //    foreach (DataRow row in schema.Rows)
        //    {
        //    }
        //}
    }
}
