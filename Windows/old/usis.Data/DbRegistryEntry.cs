using System;
using System.Data.Linq.Mapping;

namespace usis.Data
{
    [Table(Name = "audiusRegistry")]
    internal class DbRegistryEntry
    {
        [Column(Name = "audiusRegistryId", IsPrimaryKey = true, CanBeNull = false)]
        public Guid Id;

        [Column(Name = "ParentId")]
        public Guid? ParentId;

        [Column(Name = "EntryName")]
        public string Name;

        [Column(Name = "EntryType", CanBeNull = false)]
        public byte EntryType;

        [Column(Name = "EntryData")]
        public string EntryData;

        [Column(Name = "OwnerId")]
        public Guid? OwnerId;

        [Column(Name = "Deleted", CanBeNull = false)]
        public byte Deleted;
    }
}
