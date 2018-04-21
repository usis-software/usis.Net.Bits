using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;

#pragma warning disable 1591

namespace usis.Data
{
    public class DbRegistryService
    {
        #region construction

        public DbRegistryService(DataSource dataSource)
        {
            DataSource = dataSource;
        }

        #endregion construction

        #region Get...Count methods

        //  ---------------------
        //  GetSubkeyCount method
        //  ---------------------

        public int GetSubkeyCount(Guid id, Guid? owner)
        {
            return GetSubEntryCount(id, owner, true);
        }

        //  --------------------
        //  GetValueCount method
        //  --------------------

        public int GetValueCount(Guid id, Guid? owner)
        {
            return GetSubEntryCount(id, owner, false);
        }

        //  -----------------------
        //  GetSubEntryCount method
        //  -----------------------

        private int GetSubEntryCount(Guid id, Guid? ownerId, bool keys)
        {
            using (var context = GetContext())
            {
                var query = from e in context.Table
                            where e.ParentId == id && e.Deleted == 0
                            select e;
                query = ApplyOwnerFilter(query, ownerId);
                query = ApplyEntryTypeFilter(query, keys);
                return query.Count();
            }
        }

        #endregion Get...Count methods

        #region Get...Names methods

        //  ---------------------
        //  GetSubKeyNames method
        //  ---------------------

        public string[] GetSubkeyNames(Guid? id, Guid? ownerId)
        {
            return GetSubEntryNames(id, ownerId, true).ToArray();
        }

        //  --------------------
        //  GetValueNames method
        //  --------------------

        public string[] GetValueNames(Guid? id, Guid? ownerId)
        {
            return GetSubEntryNames(id, ownerId, false).ToArray(); ;
        }

        //  -----------------------
        //  GetSubEntryNames method
        //  -----------------------

        private IEnumerable<string> GetSubEntryNames(Guid? id, Guid? ownerId, bool keys)
        {
            foreach (var e in GetSubEntries(id, ownerId, keys))
            {
                yield return e.Name;
            }
        }

        #region GetSubEntries method

        //  --------------------
        //  GetSubEntries method
        //  --------------------

        internal IEnumerable<DbRegistryEntry> GetSubEntries(Guid? id, Guid? ownerId, bool keys)
        {
            using (var context = GetContext())
            {
                var query = from e in context.Table
                            where e.Deleted == 0
                            select e;
                query = ApplyParentFilter(query, id);
                query = ApplyOwnerFilter(query, ownerId);
                query = ApplyEntryTypeFilter(query, keys);
                foreach (var e in query)
                {
                    yield return e;
                }
            }
        }

        #endregion GetSubEntries method

        #endregion Get...Names methods

        #region OpenSubkey method

        //  -----------------
        //  OpenSubkey method
        //  -----------------

        public Guid? OpenSubkey(Guid parentId, Guid? ownerId, string name)
        {
            using (var context = GetContext())
            {
                var result = LoadEntry(context, parentId, ownerId, name, false);
                if (result == null) return null;
                else return result.Id;
            }
        }

        internal DbRegistryEntry OpenSubEntry(Guid? parentId, Guid? ownerId, string name)
        {
            using (var context = GetContext())
            {
                return LoadEntry(context, parentId, ownerId, name, false);
            }
        }

        //  ----------------
        //  LoadEntry method
        //  ----------------

        private static DbRegistryEntry LoadEntry(
            Context context,
            Guid? parentId,
            Guid? ownerId,
            string name,
            bool includeDeleted)
        {
            var query = from e in context.Table
                        where e.Name == name && e.EntryType == 0
                        select e;
            query = ApplyParentFilter(query, parentId);
            query = ApplyOwnerFilter(query, ownerId);
            if (!includeDeleted)
            {
                query = query.Where(e => e.Deleted == 0);
            }
            return query.SingleOrDefault();
        }

        #endregion OpenSubkey method

        #region CreateSubkey method

        //  -------------------
        //  CreateSubkey method
        //  -------------------

        public Guid CreateSubkey(Guid? parentId, Guid? ownerId, string name)
        {
            using (var context = GetContext())
            {
                var query = from entity in context.Table
                            where entity.Name == name && entity.EntryType == 0
                            select entity;
                query = ApplyParentFilter(query, parentId);
                query = ApplyOwnerFilter(query, ownerId);

                var result = query.SingleOrDefault();
                if (result == null)
                {
                    result = new DbRegistryEntry()
                    {
                        Id = Guid.NewGuid(),
                        ParentId = parentId,
                        Name = name,
                        EntryType = 0,
                        OwnerId = ownerId,
                        Deleted = 0
                    };
                    context.Table.InsertOnSubmit(result);
                }
                else if (result.Deleted != 0)
                {
                    result.Deleted = 0;
                }
                else return result.Id;

                context.SubmitChanges();

                return result.Id;
            }
        }

        #endregion CreateSubkey method

        #region RenameKey method

        //  ----------------
        //  RenameKey method
        //  ----------------

        public bool RenameKey(Guid id, Guid? ownerId, string newName)
        {
            if (string.IsNullOrEmpty(newName)) return false;

            using (var context = GetContext())
            {
                var query = from e in context.Table
                            where e.Id == id
                            select e;
                var result = query.SingleOrDefault();
                if (result != null)
                {
                    var existing = LoadEntry(context, result.ParentId, ownerId, newName, true);
                    if (existing != null && existing.Id != id)
                    {
                        if (existing.Deleted == 0) return false;

                        // delete physically
                        DeleteKey(context, existing.Id, ownerId, true);
                    }
                    result.Name = newName;
                    context.SubmitChanges();
                    return true;
                }
                else return false;
            }
        }

        #endregion RenameKey method

        public bool MoveKey(Guid id, Guid? ownerId, Guid parentId, Guid? parentOwnerId)
        {
            using (var context = GetContext())
            {
                var query = from e in context.Table
                            where e.Id == id
                            select e;
                query = ApplyOwnerFilter(query, ownerId);
                var result = query.SingleOrDefault();
                if (result != null)
                {
                    // try to move to same location?
                    if (result.ParentId.Equals(parentId)) return false;

                    throw new NotImplementedException();
                }
                else return false;
            }
        }

        #region DeleteKey method

        //  ----------------
        //  DeleteKey method
        //  ----------------

        public void DeleteKey(Guid id, Guid? ownerId)
        {
            using (var context = GetContext())
            {
                DeleteKey(context, id, ownerId, false);
            }
        }

        private void DeleteKey(Context context, Guid id, Guid? ownerId, bool physically)
        {
            var query = from e in context.Table
                        where e.Id == id && e.EntryType == 0
                        select e;
            query = ApplyOwnerFilter(query, ownerId);
            var result = query.SingleOrDefault();
            if (result != null)
            {
                // delete all values

                DeleteValues(context, id, ownerId, physically);

                // delete all subkeys

                query = from e in context.Table
                        where e.ParentId == id && e.EntryType == 0
                        select e;
                query = ApplyOwnerFilter(query, ownerId);
                if (!physically) query = query.Where(e => e.Deleted == 0);
                foreach (var item in query)
                {
                    DeleteKey(context, item.Id, ownerId, physically);
                }

                // delete the key
                if (physically) context.Table.DeleteOnSubmit(result);
                else result.Deleted = byte.MaxValue;
                context.SubmitChanges();
            }
        }

        #endregion DeleteKey method

        #region SetValue method

        //  ---------------
        //  SetValue method
        //  ---------------

        public void SetValue(Guid parentId, Guid? ownerId, string valueName, object value)
        {
            using (var context = GetContext())
            {
                var query = from e in context.Table
                            where e.EntryType != 0
                            select e;
                query = ApplyOwnerFilter(query, ownerId);
                if (string.IsNullOrEmpty(valueName))
                {
                    query = query.Where(e => e.Id == parentId);
                }
                else
                {
                    query = query.Where(e => e.ParentId == parentId && e.Name == valueName);
                }
                var result = query.SingleOrDefault();
                if (result == null)
                {
                    result = new DbRegistryEntry
                    {
                        Id = Guid.NewGuid(),
                        ParentId = parentId,
                        OwnerId = ownerId,
                        Name = valueName,
                        Deleted = 0
                    };
                    context.Table.InsertOnSubmit(result);
                }
                else
                {
                    if (result.Deleted != 0) result.Deleted = 0;
                }
                ValueType entryType = DeterminEntryType(valueName, value);
                result.EntryType = (byte)entryType;
                result.EntryData = StringFromValue(entryType, value);
                context.SubmitChanges();
            }
        }

        #endregion SetValue method

        /*
        internal static void SetValueDescription(Guid id, Guid owner, string valueName, LocalizedString description)
        {
            throw new NotImplementedException();
        }
        */

        #region GetValue method

        //  ---------------
        //  GetValue method
        //  ---------------

        public object GetValue(Guid parentId, Guid? ownerId, string valueName)
        {
            using (var context = GetContext())
            {
                //var query = from e in context.Table
                //            where e.EntryType != 0 && e.Deleted == 0
                //            select e;
                //query = ApplyOwnerFilter(query, ownerId);
                //if (string.IsNullOrEmpty(valueName))
                //{
                //    query = query.Where(e => e.Id == parentId);
                //}
                //else
                //{
                //    query = query.Where(e => e.ParentId == parentId && e.Name == valueName);
                //}
                //var result = query.SingleOrDefault();
                var result = LoadEntry(context, parentId, ownerId, valueName);
                if (result == null) return null;
                else
                {
                    return ValueFromString((ValueType)result.EntryType, result.EntryData);
                }
            }
        }

        internal DbRegistryEntry GetValueEntry(Guid parentId, Guid? ownerId, string valueName)
        {
            using (var context = GetContext())
            {
                return LoadEntry(context, parentId, ownerId, valueName);
            }
        }

        private static DbRegistryEntry LoadEntry(Context context, Guid parentId, Guid? ownerId, string valueName)
        {
            var query = from e in context.Table
                        where e.EntryType != 0 && e.Deleted == 0
                        select e;
            query = ApplyOwnerFilter(query, ownerId);
            if (string.IsNullOrEmpty(valueName))
            {
                query = query.Where(e => e.Id == parentId);
            }
            else
            {
                query = query.Where(e => e.ParentId == parentId && e.Name == valueName);
            }
            return query.SingleOrDefault();
        }
        
        #endregion GetValue method

        /*
        internal static DbRegistryValueDictionary GetValues(Guid id, Guid owner)
        {
            throw new NotImplementedException();
        }
        */

        /*
        internal DbRegistryKeyDictionary GetSubKeys(Guid id, Guid owner, string v)
        {
            throw new NotImplementedException();
        }
        */

        /*
        internal static DbRegistryKey.ValueInfo GetValueInfo(Guid id, Guid owner, string valueName, bool v)
        {
            throw new NotImplementedException();
        }
        */

        /*
        internal static DbRegistryValueInfoDictionary GetValuesInfo(Guid id, Guid owner, bool includeValue)
        {
            throw new NotImplementedException();
        }
        */

        #region DeleteValue method

        //  ------------------
        //  DeleteValue method
        //  ------------------

        public void DeleteValue(Guid parentId, Guid? ownerId, string valueName)
        {
            using (var context = GetContext())
            {
                DeleteValue(context, parentId, ownerId, valueName, false);
            }
        }

        private static void DeleteValue(Context context, Guid parentId, Guid? ownerId, string valueName, bool physically)
        {
            var result = LoadEntry(context, parentId, ownerId, valueName);
            if (result != null)
            {
                if (physically) context.Table.DeleteOnSubmit(result);
                else result.Deleted = byte.MaxValue;
                context.SubmitChanges();
            }
        }

        //  -------------------
        //  DeleteValues method
        //  -------------------

        private static void DeleteValues(Context context, Guid parentId, Guid? ownerId, bool physically)
        {
            var query = from e in context.Table
                        where e.ParentId == parentId && e.EntryType != 0
                        select e;
            query = ApplyOwnerFilter(query, ownerId);
            foreach (var item in query)
            {
                if (physically) context.Table.DeleteOnSubmit(item);
                else if (item.Deleted == 0) item.Deleted = byte.MaxValue;
                context.SubmitChanges();
            }
        }

        #endregion DeleteValue method

        public bool RenameValue(Guid id, Guid? owner, string oldName, string newName)
        {
            using (var context = GetContext())
            {
                throw new NotImplementedException();
            }
        }

        #region helpers

        private static IQueryable<DbRegistryEntry> ApplyOwnerFilter(IQueryable<DbRegistryEntry> query, Guid? ownerId)
        {
            if (ownerId == null)
            {
                return query.Where(e => e.OwnerId == null);
            }
            else
            {
                return query.Where(e => e.OwnerId == ownerId);
            }
        }

        private static IQueryable<DbRegistryEntry> ApplyEntryTypeFilter(IQueryable<DbRegistryEntry> query, bool keys)
        {
            if (keys)
            {
                return query.Where(e => e.EntryType == 0);
            }
            else
            {
                return query.Where(e => e.EntryType != 0);
            }
        }

        private static IQueryable<DbRegistryEntry> ApplyParentFilter(IQueryable<DbRegistryEntry> query, Guid? parentId)
        {
            if (parentId == null)
            {
                return query.Where(e => e.ParentId == null);
            }
            else
            {
                return query.Where(e => e.ParentId == parentId);
            }
        }

        #endregion helpers

        #region private

        private DataSource DataSource
        {
            get; set;
        }

        private DataContext DataContext
        {
            get; set;
        }

        private Context GetContext()
        {
            if (DataContext == null)
            {
                DataContext = new DataContext(DataSource.OpenConnection());
            }
            return new Context(DataContext);
            //return new Context(DataSource.OpenConnection()); ;
        }

        private sealed class Context : IDisposable
        {
            //private DbConnection connection;
            private Table<DbRegistryEntry> table;
            private DataContext context;

            internal Context(DataContext dataContext)
            {
                context = dataContext;
            }
            //internal Context(DbConnection connection)
            //{
            //    this.connection = connection;
            //}
            internal Table<DbRegistryEntry> Table
            {
                get
                {
                    if (table == null)
                    {
                        //context = new DataContext(connection);
                        table = context.GetTable<DbRegistryEntry>();
                    }
                    return table;
                }
            }
            public void Dispose()
            {
                //context.Dispose();
            }
            public void SubmitChanges()
            {
                if (context != null) context.SubmitChanges();
                //context.SubmitChanges();
            }
        }

        internal enum ValueType
        {
            Default,
            String,
            Byte,
            Int16,
            Int32,
            Int64,
            Boolean,
            LocalizedString,
            DateTime,
            Guid,
            TimeSpan,
            Color,
            ByteArray,
            Decimal
        }

        private static ValueType DeterminEntryType(string name, object value)
        {
            if (string.IsNullOrEmpty(name)) return ValueType.Default;
            else if (value is string) return ValueType.String;
            else if (value is byte) return ValueType.Byte;
            else if (value is short) return ValueType.Int16;
            else if (value is int) return ValueType.Int32;
            else if (value is long) return ValueType.Int64;
            else if (value is bool) return ValueType.Boolean;
            //else if (value is LocalizedString) return ValueType.LocalizedString;
            else if (value is DateTime) return ValueType.DateTime;
            else if (value is Guid) return ValueType.Guid;
            else if (value is TimeSpan) return ValueType.TimeSpan;
            //else if (value is Color) return ValueType.Color;
            else if (value is byte[]) return ValueType.ByteArray;
            else if (value is decimal) return ValueType.Decimal;

            throw new InvalidOperationException();
        }

        internal static object ValueFromString(ValueType entryType, string entryData)
        {
            if (!Enum.IsDefined(typeof(ValueType), entryType))
            {
                throw new ArgumentException("Unknown registry entry type.", "entryType");
            }
            if (entryData == null) return null;
            switch (entryType)
            {
                case ValueType.Default:
                case ValueType.String:
                    return entryData;
                case ValueType.Byte:
                    return byte.Parse(entryData, CultureInfo.InvariantCulture);
                case ValueType.Int16:
                    return short.Parse(entryData, CultureInfo.InvariantCulture);
                case ValueType.Int32:
                    return int.Parse(entryData, CultureInfo.InvariantCulture);
                case ValueType.Int64:
                    return long.Parse(entryData, CultureInfo.InvariantCulture);
                case ValueType.Boolean:
                    return bool.Parse(entryData);
                case ValueType.LocalizedString:
                    throw new NotImplementedException();
                case ValueType.DateTime:
                    return DateTime.Parse(entryData, CultureInfo.InvariantCulture);
                case ValueType.Guid:
                    return Guid.Parse(entryData);
                case ValueType.TimeSpan:
                    return TimeSpan.Parse(entryData, CultureInfo.InvariantCulture);
                case ValueType.Color:
                    throw new NotImplementedException();
                case ValueType.ByteArray:
                    return Convert.FromBase64String(entryData);
                case ValueType.Decimal:
                    return decimal.Parse(entryData, CultureInfo.InvariantCulture);
                default:
                    break;
            }
            throw new NotImplementedException();

            #region legacy code

            //if (entryData == null)
            //{
            //    return (entryType != DbRegistryService.valueTypeDefault)
            //        ? string.Empty
            //        : null;
            //}

            //try
            //{
            //    switch (entryType)
            //    {
            //        case DbRegistryService.valueTypeDefault:
            //            return entryData;

            //        case DbRegistryService.valueTypeString:
            //            return entryData;

            //        case DbRegistryService.valueTypeByte:
            //            return Byte.Parse(entryData, CultureInfo.InvariantCulture);

            //        case DbRegistryService.valueTypeInt16:
            //            return Int16.Parse(entryData, CultureInfo.InvariantCulture);

            //        case DbRegistryService.valueTypeInt32:
            //            return Int32.Parse(entryData, CultureInfo.InvariantCulture);

            //        case DbRegistryService.valueTypeInt64:
            //            return Int64.Parse(entryData, CultureInfo.InvariantCulture);

            //        case DbRegistryService.valueTypeBoolean:
            //            return Boolean.Parse(entryData);

            //        case DbRegistryService.valueTypeLocalizedString:
            //            return LocalizedString.Parse(entryData);

            //        case DbRegistryService.valueTypeDateTime:
            //            return DateTime.Parse(entryData, CultureInfo.InvariantCulture);

            //        case DbRegistryService.valueTypeGuid:
            //            return new Guid(entryData);

            //        case DbRegistryService.valueTypeTimeSpan:
            //            return TimeSpan.Parse(entryData);

            //        case DbRegistryService.valueTypeColor:

            //            if (entryData.Length == 0) return null;

            //            string temp = entryData.ToUpper(CultureInfo.InvariantCulture);
            //            if (temp.StartsWith("ARGB(") || temp.StartsWith("RGB("))
            //            {
            //                int rgb = 0;
            //                int start = entryData.IndexOf('(') + 1;
            //                int end = entryData.IndexOf(')');

            //                temp = entryData.Substring(start, end - start);

            //                string[] ar = temp.Split(new char[] { ',' });
            //                foreach (string str in ar)
            //                    rgb = rgb * 0x100 + Convert.ToByte(str, CultureInfo.InvariantCulture);

            //                return Color.FromArgb(rgb);
            //            }

            //            FieldInfo fieldInfo = typeof(KnownColor).GetField(entryData);
            //            if (fieldInfo != null && fieldInfo.MemberType.Equals(typeof(KnownColor)))
            //                return Color.FromKnownColor((KnownColor)fieldInfo.GetValue(null));

            //            return Color.FromName(entryData);

            //        case DbRegistryService.valueTypeByteArray:
            //            return Convert.FromBase64String(entryData);

            //        case DbRegistryService.valueTypeDecimal:
            //            return Decimal.Parse(entryData, CultureInfo.InvariantCulture);
            //    }
            //}
            //catch (FormatException exception)
            //{
            //    Trace.WriteLine(exception.ToString());
            //}
            //catch (OverflowException exception)
            //{
            //    Trace.WriteLine(exception.ToString());
            //}

            #endregion legacy code
        }

        private static string StringFromValue(ValueType entryType, object value)
        {
            if (entryType == ValueType.DateTime)
            {
                DateTime dt = ((DateTime)value).ToUniversalTime();
                return dt.ToString("u", CultureInfo.InvariantCulture);
            }
            else if (entryType == ValueType.ByteArray)
            {
                return Convert.ToBase64String((byte[])value);
            }
            else
            {
                IFormattable formattable = value as IFormattable;
                if (formattable != null)
                {
                    return formattable.ToString(null, CultureInfo.InvariantCulture);
                }
                else return value.ToString();
            }
        }

        //private static string StringFromValue(object value)
        //{
        //    if (value is DateTime)
        //    {
        //        DateTime dt = ((DateTime)value).ToUniversalTime();
        //        return dt.ToString("u", CultureInfo.InvariantCulture);
        //    }
        //    //else if (value is Color)
        //    //{
        //    //    Color col = (Color)value;
        //    //    if (col.IsNamedColor)
        //    //        return col.Name;

        //    //    return String.Format(
        //    //        CultureInfo.InvariantCulture,
        //    //        "ARGB({0},{1},{2},{3})", col.A, col.R, col.G, col.B);
        //    //}
        //    else if (value is byte[])
        //    {
        //        return Convert.ToBase64String((byte[])value);
        //    }
        //    else if (value is IFormattable)
        //    {
        //        return ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture);
        //    }
        //    else return value.ToString();
        //}
        
        #endregion private
    }
}
