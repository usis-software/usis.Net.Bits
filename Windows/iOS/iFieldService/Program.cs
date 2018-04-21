//
//  @(#) Program.cs
//
//  Project:    usis Field Service iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using usis.Cocoa.Framework;
using usis.Mobile;

namespace usis.Cocoa.FieldService
{
    //  -------------
    //  Program class
    //  -------------

    internal static class Program
    {
        //  -----------
        //  Main method
        //  -----------

        /// <summary>
        /// This is the main entry point of the application.
        /// </summary>
        /// <param name="args">
        /// The command-line arguments for the current process.
        /// </param>

        internal static void Main(string[] args)
        {
            Application.Run<AppDelegate>(args, typeof(AppLayoutSnapIn), typeof(FieldServiceSnapIn));
        }
    }
}

// eof "Program.cs"
