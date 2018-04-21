using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using usis.Middleware.SAP;

namespace Playground
{
    internal static class Program
    {
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        internal static void Main()
        {
            var documentType = "WVINVE03";

            var repository = new IDocRepository();
            //repository.ReadXmlSchema($@"Data\Schema\{documentType}.XML");
            repository.ReadXmlDefinition($@"Data\Definition\{documentType}.Definition.xml");

            var definition = repository.FindDocumentDefinition(documentType);
            //var definitionFile = @".\WVINVE03.Definition.xml";
            //definition.SerializeAsXml(definitionFile);
            //repository.ReadXmlDefinition(definitionFile);

            DumpIDocDefinition(definition);
            PressAnyKey();

            var documentNumber = "0000000000000016";
            //var document = repository.CreateDocument(documentType);
            //document.ControlRecord.DocumentNumber = "12345678901234567890";
            var document = repository.ReadDocument($@"Data\{documentType}#{documentNumber}.idoc");
            //Console.Write(document.ToText());
            //Console.WriteLine("---");
            //foreach (var s in document.SegmentSequence)
            //{
            //    Console.WriteLine(s);
            //    foreach (var f in s.Values)
            //    {
            //        Console.WriteLine(f);
            //    }
            //}
            Console.WriteLine(document);
            PressAnyKey();

            ////var segmentType = "E1MAKTM";
            ////var segmentType = "E1MARAM";
            //var segmentType = "E1WVINI";

            ////var fieldList = ".DOCNUM,E1MARAM.MATNR,E1MAKTM.MAKTX,E1MAKTM.SPRAS_ISO";
            //var fieldList = "E1WVINI.ZEILI,E1WVINH.IBLNR,E1WVINH.INVNU,E1WVINH.LGORT,E1WVINI.ARTNR,E1WVINI.MAKTX,E1WVINI.ERFME,E1WVINI.ERFMG,E1WVINI.XNULL,E1WVINI.CHARG,E1WVINI.BTEXT";

            //foreach (var field in repository.GetSegmentContext(documentType, segmentType, fieldList.Split(',')))
            //{
            //    Console.WriteLine(field);
            //}

            //var document = repository.ReadDocument(@"Data\WVINVE03#0000000065240378.idoc");
            //document.ControlRecord.DocumentNumber = "12345678901234567890";
            //document.ControlRecord.DocumentNumber = "XxxxxxxxxxxxxxxXxxxx";
            //Console.WriteLine(document);
            //var key1 = new NamedValue("E1WVINH.IBLNR", "0100284922");
            //var key2 = new NamedValue("e1WvInI.ARTNR", "000000003315927000");
            //foreach (var segment in document.FilterSegments("E1WVINI", StringComparison.Ordinal, key1, key2))
            //{
            //    Console.WriteLine(segment);
            //}
            //PressAnyKey();
        }

        #region SerializeAsXml method

        //  ---------------------
        //  SerializeAsXml method
        //  ---------------------

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void SerializeAsXml<T>(this T item, XmlWriter xmlWriter)
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(xmlWriter, item);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void SerializeAsXml<T>(this T item, TextWriter writer, XmlWriterSettings settings = null)
        {
            item.SerializeAsXml(XmlWriter.Create(writer, settings ?? new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true }));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void SerializeAsXml<T>(this T item, Stream stream, Encoding encoding, XmlWriterSettings settings = null)
        {
            item.SerializeAsXml(new StreamWriter(stream, encoding), settings);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void SerializeAsXml<T>(this T item, string path, Encoding encoding, XmlWriterSettings settings = null)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                item.SerializeAsXml(stream, encoding, settings);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void SerializeAsXml<T>(this T item, string path, XmlWriterSettings settings = null)
        {
            item.SerializeAsXml(path, Encoding.UTF8, settings);
        }

        #endregion SerializeAsXml method

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void SaveIDocDefinition(IDocRepository repository, string documentType, string path)
        {
            var documentDefinition = repository.FindDocumentDefinition(documentType);
            documentDefinition.SerializeAsXml(path);
            Console.WriteLine(File.ReadAllText(path));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void DumpIDocDefinition(IDocDefinition definition)
        {
            var path = "tmp.xml";
            definition.SerializeAsXml(path);
            Console.WriteLine(File.ReadAllText(path));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static IDocDefinition LoadIDocDefinition(string path)
        {
            var serializer = new XmlSerializer(typeof(IDocDefinition));
            using (var xmlReader = XmlReader.Create(path))
            {
                return serializer.Deserialize(xmlReader) as IDocDefinition;
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString(System.String)")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String,System.Object)")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //internal static void ReadIDocInboundFolder(IDocRepository repository, string documentType, string segmentType, string fieldList)
        //{
        //    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IDocRouter", "Inbound", "MATMAS05#*.idoc");
        //    var file = Path.GetFileName(path);
        //    var directory = Path.GetDirectoryName(path);

        //    foreach (var segment in repository.ReadDocumentFiles(directory, file, @"C:\ProgramData\IDocRouter\Inbound\processed").EnumerateSegments(documentType, segmentType))
        //    {
        //        Console.WriteLine(segment.Document.ControlRecord.DocumentNumber);
        //        foreach (var field in segment.EnumerateContextFields(fieldList.Split(',')))
        //        {
        //            Console.WriteLine(field);
        //        }
        //    }
        //    PressAnyKey();
        //}

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static void ReadMaterialMaster(IDocRepository repository)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IDocRouter", "Inbound");
            foreach (var file in Directory.EnumerateFiles(path, "MATMAS05#*.idoc"))
            {
                Console.WriteLine(file);
                using (var reader = new IDocFileReader(file))
                {
                    foreach (var document in IDoc.ReadDocuments(reader, repository))
                    {
                        Console.WriteLine(document);
                        if (document.SegmentSequence.Count() == 0) continue;

                        var groundLevel = document.SegmentSequence.First().DataRecord.HierarchyLevel;
                        foreach (var segment in document.SegmentSequence)
                        {
                            Console.Write("{0} ", segment.DataRecord.SegmentNumber.ToString("D6"));
                            var level = segment.DataRecord.HierarchyLevel - groundLevel;
                            Console.Write(new string('-', level));
                            //Console.WriteLine(segment.DataRecord.SegmentName);
                            Console.WriteLine(segment.Definition.SegmentType);
                        }
                    }
                }
            }
        }

        #region PressAnyKey method

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)")]
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        #endregion PressAnyKey method
    }
}
