using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using usis.Framework;

namespace Playground.ObjectBroker
{
    class Program
    {
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)")]
        static void Main()
        {
            var root = new NamespaceMetadata("usis");
            var framework = root.Add(new NamespaceMetadata("Framework"));
            framework.Add(new EntityMetadata("EntityBase",
                new AttributeMetadata("Created", AttributeDataType.DateTime),
                new AttributeMetadata("Changed", AttributeDataType.DateTime)));
            var ns = root.Add(new NamespaceMetadata("Finance"));
            ns.Add(new EntityMetadata("FinancialAccount",
                new AttributeMetadata("Id", AttributeDataType.Id),
                new AttributeMetadata("Name", AttributeDataType.String),
                new AttributeMetadata("Currency", AttributeDataType.String)));

            #region make XML

            using (var writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                var serializer = new XmlSerializer(typeof(NamespaceMetadata));
                serializer.Serialize(writer, root);
                Console.WriteLine(writer);
            }

            #endregion make XML

            //var sql = new CreateStatementBuilder(activity.Name);
            //Console.WriteLine();
            //Console.WriteLine(sql);

            var repository = new MetadataRepository();
            repository.Import(root);
            repository.Import(root);

            PressAnyKey();
        }

        #region press any key...

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)")]
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key ... ");
            Console.ReadKey(true);
        }

        #endregion press any key...

        #region old

        //internal static AttributeMetadata NewIdAttribute()
        //{
        //    return new AttributeMetadata("Id", AttributeDataType.Id);
        //}

        //internal static void Old()
        //{
        //    var baseEntity =
        //        new EntityMetadata("EntityBase",
        //            new AttributeMetadata("Created", AttributeDataType.DateTime),
        //            new AttributeMetadata("Changed", AttributeDataType.DateTime).AsNullable(true),
        //            new AttributeMetadata("Deleted", AttributeDataType.Boolean))
        //            .AsAbstract();

        //    EntityMetadata activity;

        //    var ns =
        //        new NamespaceMetadata("usis",
        //            new NamespaceMetadata("Data.Entity", baseEntity),
        //            new NamespaceMetadata("Basis",
        //                activity = new EntityMetadata("Activity", new Guid("85810107-4b14-466c-b5f8-808bef684a09"),
        //                    new AttributeMetadata("Id", AttributeDataType.Id),
        //                    new AttributeMetadata("Subject", AttributeDataType.String))
        //                    .DerivesFrom(baseEntity),
        //                new EntityMetadata("Contact",
        //                    new AttributeMetadata("Id", AttributeDataType.Id))
        //                    .DerivesFrom(baseEntity),
        //                new EntityMetadata("User",
        //                    NewIdAttribute())
        //                    .DerivesFrom(baseEntity),
        //                new EntityMetadata("Device",
        //                    NewIdAttribute())
        //                    .DerivesFrom(baseEntity)));
        //}

        #endregion old
    }

    #region CreateStatementBuilder class

    //internal class CreateStatementBuilder
    //{
    //    public CreateStatementBuilder(string tableName) { TableName = tableName; }

    //    public string TableName { get; }

    //    //private Dictionary<string, ColumnDescription> columns = new Dictionary<string, ColumnDescription>(StringComparer.OrdinalIgnoreCase);

    //    [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Text.StringBuilder.AppendFormat(System.String,System.Object)")]
    //    public override string ToString()
    //    {
    //        var sb = new StringBuilder();
    //        sb.AppendFormat("CREATE {0}", TableName);
    //        return sb.ToString();
    //    }
    //}

    #endregion CreateStatementBuilder class
}
