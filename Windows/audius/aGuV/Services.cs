//
//  @(#) Services.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using Sagede.OfficeLine.Data.Configuration;
using Sagede.OfficeLine.Rewe.Bilanz;
using Sagede.OfficeLine.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using usis.Middleware.Sage.OfficeLine.Data;
using usis.Platform;

namespace audius.GuV.Wizard
{
    //  --------------
    //  Services class
    //  --------------

    internal static class Services
    {
        #region public methods

        //  ----------------------
        //  LoadDataSources method
        //  ----------------------

        internal static IEnumerable<DataSource> LoadDataSources()
        {
            var manager = new PublicDataSourcesSettingsManager();
            if (manager.Load(ApplicationToken.Rewe))
            {
                foreach (var settings in manager)
                {
                    yield return new DataSource(settings);
                }
            }
            yield return new DataSource();
        }

        //  ------------------
        //  LoadClients method
        //  ------------------

        internal static IEnumerable<Client> LoadClients(DataSource dataSource)
        {
            using (var context = CreateDataContext(dataSource))
            {
                var query = from clientProperty in context.GetTable<ClientProperty>()
                            where clientProperty.Id == 1
                            select clientProperty;
                foreach (var clientProperty in query) yield return new Client(clientProperty);
            }
        }

        //  ----------------------
        //  LoadDefinitions method
        //  ----------------------

        internal static IEnumerable<Definition> LoadDefinitions(DataSource dataSource, Client client)
        {
            if (client == null) yield break;

            using (var context = CreateDataContext(dataSource))
            {
                var query = from definition in context.GetTable<BalanceSheetDefinition>()
                            where definition.Client == client.Id && definition.Saldo == 0
                            select definition;
                foreach (var item in query) yield return new Definition(item);
            }
        }


        //  ---------------------
        //  ParsePositions method
        //  ---------------------

        internal static IEnumerable<Position> ParsePositions(PositionenCollection positions)
        {
            foreach (var item in positions)
            {
                var position = new Position(item.Name);
                switch (item)
                {
                    case GruppenPosition group:
                        foreach (var subPosition in ParsePositions(item.BilanzPositionen))
                        {
                            position.AddPosition(subPosition);
                        }
                        yield return position;
                        break;
                    case KontenzuordnungsPosition konten:
                        foreach (var range in konten.ZuordnungsDefinition)
                        {
                            Debug.Assert(range.BilanzSaldoValue.IsOneOf(
                                SachkontenBereich.BilanzSaldo.SollHaben,
                                SachkontenBereich.BilanzSaldo.Soll,
                                SachkontenBereich.BilanzSaldo.Haben));
                            //Debug.Assert(konten.VorzeichenTausch == Sagede.OfficeLine.Rewe.Bilanz.Position.BooleanDefault.Nein);
                            position.AddAccounts(range.KontoNummerVon, range.KontoNummerBis);
                        }
                        yield return position;
                        break;
                    case SummePosition summe:
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
        }

        //  --------------------
        //  ExecuteScript method
        //  --------------------

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal static void ExecuteScript(DataSource dataSource, string script)
        {
            using (var connection = CreateConnection(dataSource))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = script;
                    command.ExecuteNonQuery();
                }
            }
        }

        //  --------------------------------
        //  GetManifestResourceString method
        //  --------------------------------

        internal static string GetManifestResourceString(string name)
        {
            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }

        //  -----------------------
        //  FlattenPositions method
        //  -----------------------

        internal static void FlattenPositions(IEnumerable<Position> positions)
        {
            int i = 1;
            foreach (var level0Pos in positions)
            {
                level0Pos.Index = i++;
                if (level0Pos.Positions.Count() == 0)
                {
                    level0Pos.AddPosition(new Position(level0Pos.Name, level0Pos.AccountRanges) { Index = 1 });
                    level0Pos.ClearAccounts();
                }
                else
                {
                    int j = 1;
                    foreach (var level1Pos in level0Pos.Positions)
                    {
                        level1Pos.Index = j++;
                        foreach (var child in WalkChildren(level1Pos))
                        {
                            level1Pos.AddAccounts(child.Item2.AccountRanges);
                        }
                        level1Pos.ClearPositions();
                    }
                }
            }
        }

        //  ----------------------
        //  SaveDefinitions method
        //  ----------------------

        internal static void SaveDefinitions(DataSource dataSource, IEnumerable<Position> positions)
        {
            using (var context = CreateDataContext(dataSource))
            {
                SavePositions(context, positions);
                SaveAccounts(context, positions);
            }
        }

        #endregion public methods

        #region private methods

        //  -----------------------
        //  CreateConnection method
        //  -----------------------

        private static IDbConnection CreateConnection(DataSource dataSource)
        {
            return new SqlConnection(BuildConnectionString(dataSource));
        }

        //  ------------------------
        //  CreateDataContext method
        //  ------------------------

        private static DataContext CreateDataContext(DataSource dataSource)
        {
            return new DataContext(BuildConnectionString(dataSource));
        }

        //  ----------------------------
        //  BuildConnectionString method
        //  ----------------------------

        private static string BuildConnectionString(DataSource dataSource)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = dataSource.ServerName,
                InitialCatalog = dataSource.DatabaseName,
                IntegratedSecurity = true

            }.ConnectionString;
        }

        //  -------------------
        //  WalkChildren method
        //  -------------------

        private static IEnumerable<Tuple<Position, Position>> WalkChildren(Position parent)
        {
            return WalkChildren(parent, parent.Positions);
        }

        private static IEnumerable<Tuple<Position, Position>> WalkChildren(Position parent, IEnumerable<Position> children)
        {
            foreach (var item in children)
            {
                yield return new Tuple<Position, Position>(parent, item);
                foreach (var child in WalkChildren(item, item.Positions)) yield return child;
            }
        }

        //  -------------------
        //  WalkAccounts method
        //  -------------------

        private static IEnumerable<Tuple<Position, GeneralLedgerAccountRange>> WalkAccounts(IEnumerable<Position> positions)
        {
            foreach (var children in WalkChildren(null, positions))
            {
                foreach (var range in children.Item2.AccountRanges)
                {
                    yield return new Tuple<Position, GeneralLedgerAccountRange>(children.Item2, range);
                }
            }
        }

        //  --------------------
        //  SavePositions method
        //  --------------------

        private static void SavePositions(DataContext context, IEnumerable<Position> positions)
        {
            var table = context.GetTable<Data.Position>();

            int id = 1;
            foreach (var child in WalkChildren(null, positions))
            {
                var parent = child.Item1;
                var position = child.Item2;

                position.Id = id++;
                var record = new Data.Position()
                {
                    Id = position.Id.Value,
                    Name = position.Name,
                    Parent = parent?.Id
                };
                if (parent == null)
                {
                    record.Name = string.Format(CultureInfo.CurrentCulture, "{0:00}. {1}", position.Index, position.Name);
                }
                table.InsertOnSubmit(record);
            }
            context.SubmitChanges();
        }

        //  -------------------
        //  SaveAccounts method
        //  -------------------

        private static void SaveAccounts(DataContext context, IEnumerable<Position> positions)
        {
            var table = context.GetTable<Data.GeneralLedgerAccountRange>();

            int id = 1;
            foreach (var range in WalkAccounts(positions))
            {
                if (!range.Item1.Id.HasValue) continue;
                var record = new Data.GeneralLedgerAccountRange() { Id = id++, Position = range.Item1.Id.Value, From = range.Item2.From, To = range.Item2.To };
                table.InsertOnSubmit(record);
            }
            context.SubmitChanges();
        }

        #endregion private methods
    }
}

// eof "Services.cs"
