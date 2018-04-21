//
//  @(#) Configuration.cs
//
//  Project:    usis Workflow Engine
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Diagnostics.CodeAnalysis;

namespace usis.Workflow.Engine.Migrations
{
    //  -------------------
    //  Configuration class
    //  -------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class Configuration : DbMigrationsConfiguration<DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

            SetSqlGenerator("MySql.Data.MySqlClient", new SqlGenerator());
            CodeGenerator = new MySql.Data.Entity.MySqlMigrationCodeGenerator();
        }
    }

    internal class SqlGenerator : MySql.Data.Entity.MySqlMigrationSqlGenerator
    {
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            IEnumerable<MigrationStatement> res = base.Generate(migrationOperations, providerManifestToken);
            foreach (MigrationStatement ms in res)
            {
                ms.Sql = ms.Sql.Replace("dbo.", "");
                System.Diagnostics.Debug.WriteLine(ms.Sql);
            }
            return res;
        }
    }
}

// eof "Configuration.cs"
