//
//  @(#) IDocTool.cs
//
//  Project:    IDocTool
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using usis.Middleware.SAP;
using usis.Platform;

namespace IDocTool
{
    //  -------------
    //  Program class
    //  -------------

    internal static class Program
    {
        #region Main method

        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Name, Assembly.GetExecutingAssembly().GetFileVersion()));
            if (!DoIt(args))
            {
                Console.WriteLine();
                Console.WriteLine(Strings.Usage);
            }
            if (Debugger.IsAttached) PressAnyKey();
        }

        #endregion Main method

        #region private methods

        //  -----------
        //  DoIt method
        //  -----------

        private static bool DoIt(string[] args)
        {
            var commandLine = new CommandLine(args);
            if (commandLine.HasOption("createDefinition"))
            {
                Console.WriteLine();
                Console.WriteLine(Strings.CreatingDefinition);
                var input = commandLine.GetValue("input");
                if (File.Exists(input))
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Schema, input));
                    var repository = new IDocRepository();
                    repository.ReadXmlSchema(input);
                    var documentType = commandLine.GetValue("documentType");
                    if (documentType != null)
                    {
                        Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.DocumentType, documentType));
                        var docDefinition = repository.FindDocumentDefinition(documentType);
                        var output = commandLine.GetValue("output");
                        if (docDefinition != null && !string.IsNullOrWhiteSpace(output))
                        {
                            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.DefinitionFile, output));
                            docDefinition.SerializeAsXml(output);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //  ------------------
        //  PressAnyKey method
        //  ------------------

        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write(Strings.PressAnyKeyToContinue);
            Console.ReadKey(true);
            Console.WriteLine();
        }

        #endregion private methods
    }
}

// eof "IDocTool.cs"
