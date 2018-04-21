using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using usis.Middleware.SAP;
using usis.Platform;

namespace Playground
{
    internal static class IDoc
    {
        internal static void Main()
        {
            var path = "..\\..\\..\\Playground\\etc\\IDoc";
            //var path = "Z:\\Macintosh HD 2\\tmp\\IDocs";

            //ReadIDoc(
            //    Path.Combine(path, "DEBMAS07.xsd"),
            //    Path.Combine(path, "DEBMAS.txt"));

            //var repository = new IntermediateDocumentRepository(Path.Combine(path, "_DEBMAS07.xsd"));
            //repository.Read(Path.Combine(path, "_ARTMAS05.xsd"));

            //var file = Path.Combine(path, "Empty.txt");
            //var file = Path.Combine(path, "DEBMAS.txt");
            //var file = Path.Combine(path, "Artmas20140604-171832-755.txt");
            foreach (var file in Directory.EnumerateFiles(path, "DEBMAS_*"))
            {
                using (var reader = new IDocFileReader(/*repository, */file))
                {
                    foreach (var document in usis.Middleware.SAP.IDoc.ReadDocuments(reader))
                    {
                        #region dump segments

                        Console.WriteLine("Document: {0}", document);
                        //foreach (var segment in document.Segments)
                        //{
                        //    Console.Write(segment.Number.ToString("000000", CultureInfo.InvariantCulture));
                        //    var s = new string(' ', segment.HierarchyLevel);
                        //    Console.Write(s);
                        //    Console.WriteLine(segment.Name);

                        //    //foreach (var field in segment.Definition.Fields)
                        //    //{
                        //    //    var dataStartIndex = IntermediateDocumentSegmentDefinition.HeaderLength;
                        //    //    int endIndex = field.StartIndex.Value + field.Length - 1;
                        //    //    Console.WriteLine("      {0}- {1} (start = {2}, length = {3}, End = {4} - Field: {5}",
                        //    //        s,
                        //    //        field.Name,
                        //    //        field.StartIndex + dataStartIndex,
                        //    //        field.Length,
                        //    //        endIndex + dataStartIndex,
                        //    //        segment.GetValue(field.Name));
                        //    //}
                        //    //foreach (var field in segment.Values)
                        //    //{
                        //    //    Console.WriteLine("      {0}- {1}\t= \"{2}\"", s, field.Name, field.Value);
                        //    //}
                        //}

                        #endregion

                        foreach (var segment in document.SegmentSequence)
                        {
                            Console.WriteLine(segment);
                            //if (segment.Definition != null && segment.Definition.Name.Equals("E1KNVKM", StringComparison.OrdinalIgnoreCase))
                            //{
                            //    //Console.WriteLine("Field: {0}", GetValue(segment, "..\\NAME1".Split(new char[] {'\\'})));
                            //    //Console.WriteLine(segment.DumpFields(segment.Definition));
                            //    //Console.WriteLine(segment.Get<string>("SDATA"));
                            //    //Debug.Assert(segment.Get<string>("SDATA").Equals(segment.DumpFields(segment.Definition)));
                            //}
                        }
                        Console.WriteLine();
                    }
                }
                //PressAnyKey();
            }
            PressAnyKey();
        }


        //public static INamedValue GetValue(IDocSegment segment, string[] path)
        //{
        //    if (path == null || path.Length == 0) throw new ArgumentNullOrEmptyException(nameof(path));
        //    var name = path.Last();
        //    for (int i = path.Length - 1; i >= 0; i--)
        //    {
        //        if (path[i].Equals("..", StringComparison.OrdinalIgnoreCase))
        //        {
        //            if (segment.Parent != null) segment = segment.Parent;
        //        }
        //    }
        //    return segment.GetValue(name);
        //}

        static void ReadIDoc(/*string schema, */string data)
        {
            //var repository = new IntermediateDocumentRepository(schema);
            //repository.Read(schema);
            using (var reader = new IDocFileReader(/*repository, */data))
            {
                //foreach (var segment in reader.Segments)
                //IDocSegment segment = null;
                while (reader.Read())
                {
                    Console.WriteLine(reader.CurrentDataRecord);
                }
            }
        }

        private static void ReadAllFiles()
        {
            var path = "..\\..\\..\\Playground\\etc\\IDoc";

            var repository = new IDocRepository();
            foreach (var file in Directory.EnumerateFiles(path, "*.xsd"))
            {
                using (var reader = new XmlTextReader(file))
                {
                    repository.Read(XmlSchema.Read(reader, null));
                }
            }

            #region old

            //using (var reader = new XmlTextReader(Path.Combine(path, "DEBMAS07.xsd")))
            //{
            //    var schema = XmlSchema.Read(reader, null);

            //    IntermediateDocumentRepository.Default.Read(schema);

            //    #region eval

            //    //foreach (var idoc in IterateIDocs(schema))
            //    //{
            //    //    Console.WriteLine("IDoc: {0}", idoc.Item1.Name);

            //    //    foreach (var segment in IterateSegments(idoc.Item2))
            //    //    {
            //    //        if (segment.Name.Equals("EDI_DC40", StringComparison.OrdinalIgnoreCase)) continue;

            //    //        ReadSegment(segment);
            //    //        foreach (var item in IterateSegments(segment))
            //    //        {
            //    //            ReadSegment(item);
            //    //        }
            //    //    }
            //    //}

            //    #endregion eval

            //    #region old

            //    //string currentSegment = null;
            //    //foreach (var element in IterateSchema(xsd))
            //    //{
            //    //    var attributes = GetAttributes(element);
            //    //    if (attributes.ContainsKey("SEGMENT"))
            //    //    {
            //    //        Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, "Segment: {0}", element.Name));
            //    //        currentSegment = element.Name;
            //    //    }
            //    //    if (!string.IsNullOrWhiteSpace(currentSegment))
            //    //    {
            //    //        if (element.SchemaType is XmlSchemaSimpleType)
            //    //        {
            //    //            Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, "Field: {0}", element.Name));
            //    //        }
            //    //    }
            //    //}

            //    #endregion old
            //}
            //PressAnyKey();

            #endregion old

            foreach (var file in Directory.EnumerateFiles(path, "DEBMAS*008.txt"))
            {
                Console.WriteLine(file);
                PressAnyKey();
                ReadIDoc(/*repository, */file);
            }
        }

        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        //private static void ReadIDoc(/*IntermediateDocumentRepository repository, */string path)
        //{
        //    using (var reader = new IDocFileReader(/*repository, */path))
        //    {
        //        DumpRecord(reader.CurrentControlRecord);
        //        //foreach (var segment in reader.Segments)
        //        //IDocSegment segment = null;
        //        while (reader.Read())
        //        {
        //            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}", reader.CurrentDataRecord));
        //            //DumpRecord(reader.CurrentSegment);
        //            PressAnyKey();
        //        }
        //    }
        //}

        private static void DumpRecord(IDocRecord record)
        {
            foreach (var field in record.Values)
            {
                if (field.Name.Equals("SDATA")) continue;
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}", field));
            }
            Console.WriteLine(string.Empty);
        }

        #region IDoc schema reading evaluation

        private static void ReadSegment(XmlSchemaElement segment)
        {
            Console.WriteLine("Segment: {0}", segment.Name);
            foreach (var field in IterateFields(segment))
            {
                if (field.SchemaType is XmlSchemaSimpleType simpleType)
                {
                    if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
                    {
                        foreach (var item in restriction.Facets)
                        {
                            if (item is XmlSchemaMaxLengthFacet maxLength)
                            {
                                Console.WriteLine("Field: {0} ({1})", field.Name, maxLength.Value);
                            }
                        }
                    }
                }
            }
        }
        private static IEnumerable<XmlSchemaElement> IterateFields(XmlSchemaElement segment)
        {
            foreach (var element in IterateSchemaElements(segment))
            {
                if (element.SchemaType is XmlSchemaSimpleType)
                {
                    yield return element;
                }
            }
            //yield break;
        }
        private static IEnumerable<XmlSchemaElement> IterateSegments(XmlSchemaElement element)
        {
            foreach (var segment in IterateSchemaElements(element))
            {
                var attributes = GetAttributes(segment);
                if (attributes.ContainsKey("SEGMENT"))
                {
                    yield return segment;
                }
            }
        }
        private static IEnumerable<Tuple<XmlSchemaElement, XmlSchemaElement>> IterateIDocs(XmlSchema schema)
        {
            foreach (var item in IterateSchemaElements(schema.Items))
            {
                var name = item.Name;
                foreach (var subItem in IterateSchemaElements(item))
                {
                    if (subItem.Name.Equals("IDOC", StringComparison.OrdinalIgnoreCase))
                    {
                        var attributes = GetAttributes(subItem);
                        if (attributes.ContainsKey("BEGIN"))
                        {
                            yield return new Tuple<XmlSchemaElement, XmlSchemaElement>(item, subItem);
                        }
                    }
                }
            }
        }
        private static IEnumerable<XmlSchemaElement> IterateSchema(XmlSchema schema)
        {
            return IterateSchemaElements(schema.Items);
        }
        private static IEnumerable<XmlSchemaElement> IterateSchemaElements(XmlSchemaElement element)
        {
            if (element.SchemaType is XmlSchemaComplexType complexType)
            {
                if (complexType.Particle is XmlSchemaSequence sequence)
                {
                    foreach (var item in IterateSchemaElements(sequence.Items))
                    {
                        yield return item;
                    }
                }
            }
        }
        private static IEnumerable<XmlSchemaElement> IterateSchemaElements(XmlSchemaObjectCollection items)
        {
            foreach (XmlSchemaObject item in items)
            {
                if (item is XmlSchemaElement element) yield return element;

                //var complexType = element.SchemaType as XmlSchemaComplexType;
                //if (complexType != null)
                //{
                //    var sequence = complexType.Particle as XmlSchemaSequence;
                //    if (sequence != null)
                //    {
                //        foreach (var subItem in IterateSchemaElements(sequence.Items))
                //        {
                //            yield return subItem;
                //        }
                //    }
                //}
            }
        }
        private static XmlSchemaObjectCollection GetItems(XmlSchemaElement element)
        {
            if (element.SchemaType is XmlSchemaComplexType complexType)
            {
                if (complexType.Particle is XmlSchemaSequence sequence)
                {
                    return sequence.Items;
                }
            }
            return null;
        }
        private static Dictionary<string, XmlSchemaAttribute> GetAttributes(XmlSchemaElement element)
        {
            var attributes = new Dictionary<string, XmlSchemaAttribute>(StringComparer.OrdinalIgnoreCase);
            foreach (var attribute in IterateAttributes(element))
            {
                attributes.Add(attribute.Name, attribute);
            }
            return attributes;
        }
        private static IEnumerable<XmlSchemaAttribute> IterateAttributes(XmlSchemaElement element)
        {
            var complexType = element.SchemaType as XmlSchemaComplexType;
            if (complexType == null) yield break;
            foreach (var item in complexType.Attributes)
            {
                if (item is XmlSchemaAttribute attribute) yield return attribute;
            }
        }

        #endregion IDoc schema reading evaluation
    }
}
