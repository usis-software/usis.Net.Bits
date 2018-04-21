//
//  @(#) XmlExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace usis.Platform
{
    //  -------------------
    //  XmlExtensions class
    //  -------------------

    /// <summary>
    /// Provides extension methods for processing XML.
    /// </summary>

    public static class XmlExtensions
    {
        #region SerializeAsXml method

        //  ---------------------
        //  SerializeAsXml method
        //  ---------------------

        /// <summary>
        /// Serializes the item as XML to a file with the specified XML writer settings.
        /// </summary>
        /// <typeparam name="T">The type of the item to serialize.</typeparam>
        /// <param name="item">The item to serialize.</param>
        /// <param name="path">The path of the file.</param>

        public static void SerializeAsXml<T>(this T item, string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                item.SerializeAsXml(writer);
            }
        }

        /// <summary>
        /// Serializes the item as XML to a file with the specified XML writer settings.
        /// </summary>
        /// <typeparam name="T">The type of the item to serialize.</typeparam>
        /// <param name="item">The item to serialize.</param>
        /// <param name="path">The path of the file.</param>
        /// <param name="settings">The XML writer settings.</param>

        [Obsolete("Because of possible memory leaks, you should not use this method.")]
        public static void SerializeAsXml<T>(this T item, string path, XmlWriterSettings settings)
        {
            item.SerializeAsXml(path, Encoding.UTF8, settings);
        }

        [Obsolete("Because of possible memory leaks, you should not use this method.")]
        internal static void SerializeAsXml<T>(this T item, string path, Encoding encoding, XmlWriterSettings settings = null)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                item.SerializeAsXml(stream, encoding, settings);
            }
        }

        [Obsolete("Because of possible memory leaks, you should not use this method.")]
        internal static void SerializeAsXml<T>(this T item, Stream stream, Encoding encoding, XmlWriterSettings settings = null)
        {
            item.SerializeAsXml(new StreamWriter(stream, encoding), settings);
        }

        [Obsolete("Because of possible memory leaks, you should not use this method.")]
        internal static void SerializeAsXml<T>(this T item, TextWriter writer, XmlWriterSettings settings = null)
        {
            item.SerializeAsXml(XmlWriter.Create(writer, settings ?? new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true }));
        }

        internal static void SerializeAsXml<T>(this T item, XmlWriter xmlWriter)
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(xmlWriter, item);
        }

        //  ------------
        //  ToXml method
        //  ------------

        /// <summary>
        /// Returns an XML representation of the specified item.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>An XML representation of the specified item.</returns>

        public static string ToXml<T>(this T item)
        {
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings() { NewLineHandling = NewLineHandling.None }))
            {
                SerializeAsXml(item, writer);
                return sb.ToString();
            }
        }

        #region IEnumerable<T>

        /// <summary>
        /// Serializes the items the the enumaration as XML using the specified XML writer.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration items.</typeparam>
        /// <param name="items">The enumeration.</param>
        /// <param name="writer">The XML writer.</param>

        public static void SerializeAsXml<T>(this IEnumerable<T> items, XmlWriter writer)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                var serializer = new XmlSerializer(item.GetType());
                serializer.Serialize(writer, item);
            }
        }

        #endregion IEnumerable<T>

        #endregion SerializeAsXml method
    }
}

// eof "XmlExtensions.cs"
