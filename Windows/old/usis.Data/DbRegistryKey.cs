using System;
using System.Collections.Generic;
using usis.Platform;

#pragma warning disable 1591

namespace usis.Data
{
    public sealed class DbRegistryKey : IHierarchicalValueStore
    {
        #region private fields

        private DbRegistryService service;

        private Guid? id;

        #endregion private fields

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the value store.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>

        public string Name
        {
            get; private set;
        }

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves the value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>
        /// A type that implements <b>INamedValue</b> and represents the specified value.
        /// </returns>

        public INamedValue GetValue(string name)
        {
            return id.HasValue ? new DbRegistryValue(service.GetValueEntry(id.Value, null, name)) : null;
        }

        public void SetValue(INamedValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (id.HasValue)
            {
                service.SetValue(id.Value, null, value.Name, value.Value);
            }
        }

        public IEnumerable<IHierarchicalValueStore> Stores
        {
            get
            {
                return EnumerateStores(false);
            }
        }

        public IEnumerable<INamedValue> Values
        {
            get
            {
                foreach (var item in service.GetSubEntries(id, null, false))
                {
                    yield return new DbRegistryValue(item);
                }
            }
        }

        public IEnumerable<string> StoreNames
        {
            get
            {
                return service.GetSubkeyNames(id, null);
            }
        }

        public IEnumerable<string> ValueNames
        {
            get
            {
                return service.GetValueNames(id, null);
            }
        }

        public IHierarchicalValueStore OpenStore(string name, bool writable)
        {
            var entry = service.OpenSubEntry(id, null, name);
            return entry == null ? null : new DbRegistryKey(service, entry);
        }

        public IHierarchicalValueStore CreateStore(string name, bool writable)
        {
            var newId = service.CreateSubkey(this.id, null, name);
            return new DbRegistryKey(service, newId, name);
        }

        public IEnumerable<IHierarchicalValueStore> EnumerateStores(bool writable)
        {
            foreach (var entry in service.GetSubEntries(id, null, true))
            {
                yield return new DbRegistryKey(service, entry);
            }
        }

        #region construction

        private DbRegistryKey(DbRegistryService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            this.service = service;
        }

        internal DbRegistryKey(DbRegistryService service, Guid? id, string name) : this(service)
        {
            this.id = id;
            Name = name;
        }

        internal DbRegistryKey(DbRegistryService service, DbRegistryEntry entry) : this(service, entry.Id, entry.Name) { }

        #endregion construction
    }

    internal class DbRegistryValue : INamedValue
    {
        internal DbRegistryValue(DbRegistryEntry entry)
        {
            Name = entry.Name;

            var valueType = (DbRegistryService.ValueType)entry.EntryType;
            //switch (valueType)
            //{
            //    case DbRegistryService.ValueType.Default:
            //        Type = typeof(string);
            //        break;
            //    case DbRegistryService.ValueType.String:
            //        Type = typeof(string);
            //        break;
            //    case DbRegistryService.ValueType.Byte:
            //        Type = typeof(byte);
            //        break;
            //    case DbRegistryService.ValueType.Int16:
            //        Type = typeof(short);
            //        break;
            //    case DbRegistryService.ValueType.Int32:
            //        Type = typeof(int);
            //        break;
            //    case DbRegistryService.ValueType.Int64:
            //        Type = typeof(long);
            //        break;
            //    case DbRegistryService.ValueType.Boolean:
            //        Type = typeof(bool);
            //        break;
            //    case DbRegistryService.ValueType.LocalizedString:
            //        //Type = typeof(string);
            //        break;
            //    case DbRegistryService.ValueType.DateTime:
            //        Type = typeof(DateTime);
            //        break;
            //    case DbRegistryService.ValueType.Guid:
            //        Type = typeof(Guid);
            //        break;
            //    case DbRegistryService.ValueType.TimeSpan:
            //        Type = typeof(TimeSpan);
            //        break;
            //    case DbRegistryService.ValueType.Color:
            //        //Type = typeof(string);
            //        break;
            //    case DbRegistryService.ValueType.ByteArray:
            //        Type = typeof(byte[]);
            //        break;
            //    case DbRegistryService.ValueType.Decimal:
            //        Type = typeof(decimal);
            //        break;
            //    default:
            //        break;
            //}
            try
            {
                Value = DbRegistryService.ValueFromString(valueType, entry.EntryData);
            }
            catch (NotImplementedException)
            {
                Value = null;
            }
        }
        public string Name
        {
            get; private set;
        }
        //public Type Type
        //{
        //    get; private set;
        //}
        public object Value
        {
            get; private set;
        }
    }
}
