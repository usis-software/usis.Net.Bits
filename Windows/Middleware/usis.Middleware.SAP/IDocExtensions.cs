//
//  @(#) IDocExtensions.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  --------------------
    //  IDocExtensions class
    //  --------------------

    /// <summary>
    /// Provides extension methods for handling IDocs.
    /// </summary>

    public static class IDocExtensions
    {
        #region IDocRepository extensions

        #region IDocRepository.ReadXmlSchema

        //  --------------------
        //  ReadXmlSchema method
        //  --------------------

        /// <summary>
        /// Reads the definition of an IDoc from the specified XML Schema file.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="path">
        /// The path of the XML Schema file that describes the IDoc definition.
        /// </param>

        public static void ReadXmlSchema(this IDocRepository repository, string path)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            using (var reader = new XmlTextReader(path))
            {
                repository.Read(XmlSchema.Read(reader, null));
            }
        }

        #endregion IDocRepository.ReadXmlSchema

        #region ReadXmlDefinition method

        //  ------------------------
        //  ReadXmlDefinition method
        //  ------------------------

        /// <summary>
        /// Reads the definition of an IDoc from the specified XML file.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="path">The path of the XML file that contains the IDoc XML definition.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="repository"/> is a <c>null</c> reference.
        /// </exception>

        public static void ReadXmlDefinition(this IDocRepository repository, string path)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            var serializer = new XmlSerializer(typeof(IDocDefinition));
            using (var xmlReader = XmlReader.Create(path))
            {
                if (serializer.Deserialize(xmlReader) is IDocDefinition definition) repository.AddDocumentDefinition(definition);
            }
        }

        #endregion ReadXmlDefinition method

        #region IDocRepository.GetSegmentContext method

        //  ------------------------
        //  GetSegmentContext method
        //  ------------------------

        /// <summary>
        /// Gets all field definitions for a specified document type and segment name,
        /// including the field definitions of the parent segments and document control record.
        /// </summary>
        /// <param name="repository">The IDoc repository.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="segmentName">Name of the segment.</param>
        /// <param name="fieldNames">A list of field names that is used as a filter for the result field definition list.</param>
        /// <returns>
        /// An enumerator to iterate the returned field definitions.
        /// </returns>

        public static IEnumerable<IDocFieldDefinition> GetSegmentContext(this IDocRepository repository, string documentType, string segmentName, params string[] fieldNames)
        {
            var names = BuildFieldFilterList(fieldNames);
            var document = repository.FindDocumentDefinition(documentType);
            if (document != null)
            {
                foreach (var field in IDocControlRecord.Definition.Fields.Filter(names, string.Empty)) yield return field;
                var segment = document.FindSegment(segmentName);
                if (segment != null)
                {
                    var list = new List<IDocSegmentDefinition>();
                    do
                    {
                        list.Add(segment);
                        segment = segment.Parent;

                    } while (segment != null);
                    list.Reverse();
                    foreach (var item in list)
                    {
                        foreach (var field in item.Fields.Filter(names, item.SegmentType)) yield return field;
                    }
                }
            }
        }

        #endregion IDocRepository.GetSegmentContext method

        #region IDocRepository.ReadDocumentFiles method

        //  ------------------------
        //  ReadDocumentFiles method
        //  ------------------------

        /// <summary>
        /// Reads all IDoc files contained in a directory with the specified path that match the search pattern.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="path">The path of the directory.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="archiveDirectory">The path of an archive directory to which the readed files are moved.</param>
        /// <returns>An enumerator to iterate through the readed documents.</returns>

        public static IEnumerable<IDoc> ReadDocumentFiles(this IDocRepository repository, string path, string searchPattern, string archiveDirectory)
        {
            foreach (var file in Directory.EnumerateFiles(path, searchPattern))
            {
                foreach (var document in repository.ReadDocuments(file))
                {
                    yield return document;
                }
                if (!string.IsNullOrWhiteSpace(archiveDirectory) && Directory.Exists(archiveDirectory))
                {
                    var fileName = Path.GetFileName(file);
                    var archivePath = Path.Combine(archiveDirectory, fileName);
                    if (File.Exists(archivePath)) File.Delete(archivePath);
                    File.Move(file, archivePath);
                }
            }
        }

        #endregion IDocRepository.ReadDocumentFiles method

        #region IDocRepository.ReadDocuments method

        //  --------------------
        //  ReadDocuments method
        //  --------------------

        /// <summary>
        /// Reads all IDocs from the specified file name.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="file">The path of the file from which IDocs are to be read.</param>
        /// <returns>
        /// An enumerator to iterate through the readed IDocs.
        /// </returns>

        public static IEnumerable<IDoc> ReadDocuments(this IDocRepository repository, string file)
        {
            using (var reader = new IDocFileReader(file))
            {
                foreach (var document in IDoc.ReadDocuments(reader, repository))
                {
                    yield return document;
                }
            }
        }

        #endregion IDocRepository.ReadDocuments method

        #region IDocRepository.ReadDocument method

        //  -------------------
        //  ReadDocument method
        //  -------------------

        /// <summary>
        /// Reads the first IDoc from the specified file name.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="file">The file.</param>
        /// <returns>
        /// The IDoc, or <c>null</c> if the file does not contain an IDoc.
        /// </returns>

        public static IDoc ReadDocument(this IDocRepository repository, string file)
        {
            return repository.ReadDocuments(file).FirstOrDefault();
        }

        #endregion IDocRepository.ReadDocument method

        #region IDocRepository.CreateDocument method

        //  ---------------------
        //  CreateDocument method
        //  ---------------------

        /// <summary>
        /// Creates a new IDoc of the specified document type.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="documentType">The IDoc document type.</param>
        /// <returns>
        /// The newly created IDoc of the specified document type.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="repository" /> is a <c>null</c> reference.</exception>

        public static IDoc CreateDocument(this IDocRepository repository, string documentType)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            var documentDefinition = repository.FindDocumentDefinition(documentType);
            if (documentDefinition == null) return null;
            return new IDoc(documentDefinition);
        }

        #endregion IDocRepository.CreateDocument method

        #endregion IDocRepository extensions

        #region IDoc extension methods

        #region IDoc.EnumerateSegments method

        //  ------------------------
        //  EnumerateSegments method
        //  ------------------------

        /// <summary>
        /// Returns all segments of the specified segment type.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="segmentType">Type of the segment.</param>
        /// <returns>
        /// An enumerator to iterate through all segments of the specified type.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/> is a <c>null</c> reference.
        /// </exception>

        public static IEnumerable<IDocSegment> EnumerateSegments(this IDoc document, string segmentType)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            foreach (var segment in document.SegmentSequence)
            {
                if (segmentType == null || segment.Definition.SegmentType.Equals(segmentType, StringComparison.OrdinalIgnoreCase))
                {
                    yield return segment;
                }
            }
        }

        #endregion IDoc.EnumerateSegments method

        #region IDoc.FindSegments method

        //  -------------------
        //  FindSegments method
        //  -------------------

        //public static IEnumerable<IDocSegment> FindSegments(this IDoc document, string segmentType, StringComparison comparison, params INamedValue[] keyFields)
        //{
        //    foreach (var segment in document.EnumerateSegments(segmentType))
        //    {
        //        var found = true;
        //        foreach (var key in keyFields)
        //        {
        //            found = segment.GetString(key.Name).Equals(key.Value as string, comparison);
        //            if (!found) break;
        //        }
        //        if (found) yield return segment;
        //    }
        //}

        #endregion IDoc.FindSegments method

        #region IDoc.FilterSegments method

        //  ---------------------
        //  FilterSegments method
        //  ---------------------

        /// <summary>
        /// Returns all segments of a specified type that match a given filter criteria.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="segmentType">Type of the segment.</param>
        /// <param name="comparison">Specifies the culture, case, and sort rules used in the key comparison.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An enumerator to iterate through all segments of the specified type that match the filter criteria.
        /// </returns>

        public static IEnumerable<IDocSegment> FilterSegments(
            this IDoc document,
            string segmentType,
            StringComparison comparison,
            params INamedValue[] filter)
        {
            foreach (var segment in document.EnumerateSegments(segmentType))
            {
                var fields = segment.EnumerateContextFields().ToDictionary((f) => f.Name, StringComparer.OrdinalIgnoreCase);
                var found = true;
                foreach (var field in filter)
                {
                    if (fields.TryGetValue(field.Name, out INamedValue v))
                    {
                        found = (v.Value as string).Equals(field.Value as string, comparison);
                    }
                    else found = false;
                    if (!found) break;
                }
                if (found) yield return segment;
            }
        }

        #endregion IDoc.FilterSegments method

        #region IDoc.GetFileName method

        //  ------------------
        //  GetFileName method
        //  ------------------

        /// <summary>
        /// Gets a file name for the IDoc.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The file name for the IDoc.
        /// </returns>

        public static string GetFileName(this IDoc document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            return IDoc.CreateFileName(document.ControlRecord.DocumentType, document.ControlRecord.DocumentNumber);
        }

        #endregion IDoc.GetFileName method

        #region IDoc.Write method

        //  ------------
        //  Write method
        //  ------------

        /// <summary>
        /// Writes the IDoc to specified text writer.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="document"/>
        /// or
        /// <paramref name="writer"/> is a <c>null</c> reference.
        /// </exception>
        /// <remarks>
        /// The IDoc's content is written to the specified text writer as a flat text (one segment per line).
        /// </remarks>

        public static void Write(this IDoc document, TextWriter writer)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.WriteLine(document.ControlRecord.ToString().TrimEnd(CharConstants.Space));
            foreach (var segment in document.SegmentSequence)
            {
                writer.WriteLine(segment.ToString().TrimEnd(CharConstants.Space));
            }
        }

        #endregion IDoc.Write method

        #region IEnumerable<IDoc>.EnumerateSegments method

        //  ------------------------
        //  EnumerateSegments method
        //  ------------------------

        /// <summary>
        /// Enumerates all segments of a specified type in a list of documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="segmentType">Type of the segment.</param>
        /// <returns>
        /// An enumerator to iterate through the segments.
        /// </returns>

        public static IEnumerable<IDocSegment> EnumerateSegments(this IEnumerable<IDoc> documents, string documentType, string segmentType)
        {
            foreach (var document in documents)
            {
                if (documentType != null && !document.ControlRecord.DocumentType.Equals(documentType, StringComparison.OrdinalIgnoreCase)) continue;
                foreach (var segment in document.EnumerateSegments(segmentType)) yield return segment;
            }
        }

        #endregion IEnumerable<IDoc>.EnumerateSegments method

        #endregion IDoc extension methods

        #region IDocSegment extensions

        #region IDocSegment.EnumerateContextFields method

        //  -----------------------------
        //  EnumerateContextFields method
        //  -----------------------------

        /// <summary>
        /// Enumerates the fields in a segment and it's parent segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="fieldNames">The field names to filter the resulting field list.</param>
        /// <returns>
        /// An enumerator to iterate through the field list.
        /// </returns>

        public static IEnumerable<INamedValue> EnumerateContextFields(this IDocSegment segment, params string[] fieldNames)
        {
            var names = BuildFieldFilterList(fieldNames);
            var s = segment;
            do
            {
                foreach (var field in s.Values.Filter(names, s.Name)) yield return field;
                s = s.Parent;

            } while (s != null);
            if (segment.Document != null)
            {
                foreach (var field in segment.Document.ControlRecord.Values.Filter(names, string.Empty)) yield return field;
            }
        }

        #endregion IDocSegment.EnumerateContextFields method

        #region IDocSegment.SetFieldValue method

        //  --------------------------------
        //  IDocSegment.SetFieldValue method
        //  --------------------------------

        /// <summary>
        /// Sets the value of a field in the segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="segment"/> is a <c>nulll</c>reference.</exception>
        /// <exception cref="ArgumentNullOrWhiteSpaceException"><paramref name="name" /> is a <c>null</c> reference or an string that consists only of white space characters.</exception>
        /// <remarks>
        /// The <paramref name="name" /> of the field can be in the form <c>[segment name].[field name]</c>.
        /// This allows to set fields values in the segment hierarchy.
        /// </remarks>

        public static void SetFieldValue(this IDocSegment segment, string name, string value)
        {
            if (segment == null) throw new ArgumentNullException(nameof(segment));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullOrWhiteSpaceException(nameof(name));

            var names = name.Trim(CharConstants.WhiteSpace).Split(CharConstants.Period).Select(v => v.Trim(CharConstants.WhiteSpace)).ToArray();
            string fieldname;
            string segmentType;
            if (names.Length > 1)
            {
                fieldname = names[1];
                segmentType = names[0];
                if (string.IsNullOrEmpty(segmentType))
                {
                    segment.Document.ControlRecord.SetValue(fieldname, value);
                    return;
                }
            }
            else
            {
                fieldname = names[0];
                segmentType = null;
            }
            var targetSegment = segment;
            while (segmentType != null && targetSegment != null && !targetSegment.Name.Equals(segmentType, StringComparison.OrdinalIgnoreCase))
            {
                targetSegment = targetSegment.Parent;
            }
            targetSegment?.SetValue(fieldname, value);
        }

        #endregion IDocSegment.SetFieldValue method

        #endregion IDocSegment extensions

        #region IEnumerable<IDocSegmentDefinition>.EnumerateSegmentDefinitions method

        //  ----------------------------------
        //  EnumerateSegmentDefinitions method
        //  ----------------------------------

        internal static IEnumerable<IDocSegmentDefinition> EnumerateSegmentDefinitions(this IEnumerable<IDocSegmentDefinition> segments)
        {
            foreach (var segment in segments)
            {
                yield return segment;
                foreach (var child in segment.Children.EnumerateSegmentDefinitions())
                {
                    yield return child;
                }
            }
        }

        #endregion IEnumerable<IDocSegmentDefinition>.EnumerateSegmentDefinitions method

        #region private methods

        //  ---------------------------
        //  BuildFieldFilterList method
        //  ---------------------------

        private static HashSet<string> BuildFieldFilterList(string[] names)
        {
            return new HashSet<string>(names.Where(n => !string.IsNullOrWhiteSpace(n)));
        }

        //  -------------
        //  Filter method
        //  -------------

        private static IEnumerable<INamedValue> Filter(this IEnumerable<INamedValue> fields, HashSet<string> names, string prefix)
        {
            return fields.Select(f => CreateField(prefix, f)).Where(f => names.Count == 0 || names.Contains(f.Name));
        }

        private static IEnumerable<IDocFieldDefinition> Filter(this IEnumerable<IDocFieldDefinition> fields, HashSet<string> names, string prefix)
        {
            return fields.Select(f => CreateFieldDefinition(prefix, f)).Where(f => names.Count == 0 || names.Contains(f.Name));
        }

        //  ----------------------------
        //  CreateFieldDefinition method
        //  ----------------------------

        private static IDocFieldDefinition CreateFieldDefinition(string prefix, IDocFieldDefinition field)
        {
            return new IDocFieldDefinition(string.Join(StringConstants.Period, prefix, field.Name), field);
        }

        //  ------------------
        //  CreateField method
        //  ------------------

        private static INamedValue CreateField(string prefix, INamedValue field)
        {
            return new NamedValue(string.Join(StringConstants.Period, prefix, field.Name), field.Value);
        }

        #endregion private methods
    }
}

// eof "IDocExtensions.cs"
