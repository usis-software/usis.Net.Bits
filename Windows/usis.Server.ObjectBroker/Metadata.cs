using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace usis.Server.ObjectBroker
{
    public class EntityMetadata
    {
        public Guid Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        public override string ToString()
        {
            return $"Id={Id}, Name={Name}";
        }
    }

    //  ---------------------
    //  StorageMetadata class
    //  ---------------------

    public class StorageMetadata
    {
        #region fields

        private readonly Dictionary<string, EntityMetadata> entities = new Dictionary<string, EntityMetadata>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public StorageMetadata()
        {
            var entities = new ObservableCollection<EntityMetadata>();
            entities.CollectionChanged += OnCollectionChanged;
            Entities = entities;
        }

        #endregion construction

        #region properties

        //  -----------------
        //  Entities property
        //  -----------------

        public Collection<EntityMetadata> Entities { get; private set; }

        #endregion properties

        #region methods

        //  ----------------
        //  GetEntity method
        //  ----------------

        internal EntityMetadata GetEntity(string name)
        {
            EntityMetadata entity = null;
            entities.TryGetValue(name, out entity);
            return entity;
        }

        //  --------------------------
        //  OnCollectionChanged method
        //  --------------------------

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            EntityMetadata entity = null;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.Cast<EntityMetadata>())
                    {
                        if (!entities.TryGetValue(item.Name, out entity)) entities.Add(item.Name, item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        #endregion methods
    }
}
