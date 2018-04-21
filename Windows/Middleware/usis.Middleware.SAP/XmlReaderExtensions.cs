//
//  @(#) XmlReaderExtensions.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Xml;

namespace usis.Middleware.SAP
{
    //  -------------------------
    //  XmlReaderExtensions class
    //  -------------------------

    internal static class XmlReaderExtensions
    {
        //  ------------------
        //  ReadElement method
        //  ------------------

        internal static void ReadElement(this XmlReader reader, Action read)
        {
            if (reader.NodeType != XmlNodeType.Element) throw new InvalidOperationException();

            var isEmpty = reader.IsEmptyElement;
            var name = reader.Name;
            reader.ReadStartElement();
            if (!isEmpty)
            {
                read();
                if (!name.Equals(reader.Name, StringComparison.Ordinal)) throw new InvalidOperationException();
                reader.ReadEndElement();
            }
        }

        //  -------------------
        //  ReadChildren method
        //  -------------------

        internal static void ReadChildren(this XmlReader reader, Func<bool> readElement)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!readElement())
                    {
                        var isEmpty = reader.IsEmptyElement;
                        reader.ReadStartElement();
                        if (!isEmpty)
                        {
                            reader.ReadChildren(readElement);
                            reader.ReadEndElement();
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement) return;
            }
        }
    }
}

// eof "XmlReaderExtensions.cs"
