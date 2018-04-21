using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using usis.Framework.Entity;

namespace usis.Server.FieldService
{
    internal class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("name=usisFieldService") { }

        public DbSet<Assignment> Assignments { get; set; }
    }

    [Table(nameof(Assignment))]
    internal class Assignment : SyncEntity
    {
        public string Subject { get; set; }

        //public virtual IEnumerable<AssigmentImage> Subscribers { get; set; }

        public static Assignment NewAssignment()
        {
            return new Assignment()
            {
                SyncId = Guid.NewGuid()
            };
        }
    }

    internal class AssigmentImage : ImageEntity
    {
        [Key]
        [Column(Order = 0)]
        public Guid SyncId { get; protected set; }

        [Key]
        [Column(Order = 1)]
        public Guid SubscriberId { get; set; }

        public int Updates { get; set; }
    }

    internal abstract class SyncEntity : EntityBase
    {
        [Key]
        public Guid SyncId { get; protected set; }

        public int Updates { get; set; }
    }

    internal abstract class ImageEntity
    {
    }
}
