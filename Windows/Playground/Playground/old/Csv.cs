using System;
using System.Diagnostics;
using System.Globalization;
using usis.Platform;
using System.Linq;
using System.IO.Compression;
using System.IO;

namespace Playground
{
    internal static class Csv
    {
        internal static void Main()
        {
            //Roton_BU();
            //Roton_AD();
            //FinTS();
            CsvTest();

            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        internal static void CsvTest()
        {
            foreach (var path in Directory.EnumerateFiles(@"C:\tmp","*.csv"))
            {
                Console.WriteLine(path);
                using (var reader = new CsvReader(path, CsvReaderOptions.HeaderRecord))
                {
                    foreach (var row in reader.Records)
                    {
                        Console.WriteLine("- {0}", row);
                    }
                }
                Console.WriteLine();
            }
        }

        internal static void Roton_BU()
        {
            string input;
            input = "S:\\Roton\\BU22122015155322.csv";
            string output;
            output = "S:\\Roton\\bu_test.csv";

            using (var reader = new CsvReader(input, CsvReaderOptions.HeaderRecord))
            {
                using (var writer = new CsvWriter(output))
                {
                    foreach (var record in reader.Records)
                    {
                        writer.WriteRecord(record);
                    }
                }
            }
        }

        internal static void Roton_AD()
        {
            string input;
            input = "z:\\macintosh hd 2\\tmp\\roton\\bu09092015110206.csv";
            input = "S:\\Roton\\org\\AD_RITS09112015183091.csv";
            input = "S:\\Roton\\_AD_RITS22122015145221.csv";
            input = "Z:\\Macintosh HD 2\\tmp\\ROTON\\AD_RITS22122015155121.csv";

            string output;
            output = "S:\\Roton\\adr_test.csv";
            //output = "S:\\Roton\\bu_test.csv";

            ReadWrite(input, output, CsvReaderOptions.HeaderRecord | CsvReaderOptions.CheckFieldCount);
            //ReadWrite(input, output, CsvReaderOptions.HeaderRecord);

        }

        internal static void ReadWrite(string input, string output, CsvReaderOptions options)
        {
            using (var reader = new CsvReader(input, options))
            {
                Console.WriteLine("Encoding: {0}", reader.CurrentEncoding);
                foreach (var item in reader.ColumnNames)
                {
                    Console.WriteLine("Column: {0}", item);
                }
                Console.WriteLine();
                Console.Write("Press any key to continue ... ");
                Console.ReadKey(true);
                Console.WriteLine();

                using (var writer = new CsvWriter(output, reader.ColumnNames))
                {
                    foreach (var record in reader.Records)
                    {
                        Console.WriteLine();
                        foreach (var columnName in reader.ColumnNames)
                        {
                            object v = record[columnName];
                            //Debug.Assert(v != null, "Field cannot be null.");
                            Console.WriteLine("{0} = {1}", columnName, v);
                        }
                        Debug.Assert(record.Fields.Count == 25, "Wrong number of fields.");
                        Debug.Assert(!string.IsNullOrWhiteSpace(record["Name1"] as string), "Name1 cannot be empty!");

                        //Console.WriteLine();
                        //Console.Write("Press any key to continue ... ");
                        //Console.ReadKey(true);
                        //Console.WriteLine();

                        writer.WriteRecord(record);
                    }
                }
            }
        }

        internal static void FinTS()
        {
            using (var zipArchive = ZipFile.OpenRead("Z:\\usis\\Downloads\\fints_institute.zip"))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    var extension = Path.GetExtension(entry.Name);
                    if (".csv".Equals(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        using (var temp = entry.ExtractToTemporaryFile())
                        {
                            ReadWrite(
                                temp.Path,
                                "C:\\tmp\\fints_institute.csv",
                                CsvReaderOptions.HeaderRecord);
                        }
                    }
                }
            }
        }
    }
}
