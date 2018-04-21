using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using MySql.Data.Entity;

namespace usis.Data.Entity.MySql
{
    public class SqlGenerator : MySqlMigrationSqlGenerator
    {
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            IEnumerable<MigrationStatement> res = base.Generate(migrationOperations, providerManifestToken);
            foreach (MigrationStatement ms in res)
            {
                var sql = ms.Sql;
                System.Diagnostics.Debug.WriteLine(sql);
                ms.Sql = ms.Sql.Replace("dbo.", "");
                System.Diagnostics.Debug.WriteLine(ms.Sql);
                //System.Diagnostics.Debug.Assert(sql == ms.Sql);
            }
            return res;
        }
    }
}
