using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usis.Data.Entity
{
    public sealed class DBMigrationsConfiguration<TDBContext> : DbMigrationsConfiguration<TDBContext> where TDBContext : DbContext
    {
        public DBMigrationsConfiguration()
        {
        }
    }
}
