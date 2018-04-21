//
//  @(#) Program.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using usis.Platform.Windows;

namespace usis.Server
{
    //  -------------
    //  Program class
    //  -------------

    internal class Program
    {
        #region Main method

        //  -----------
        //  Main method
        //  -----------

        internal static int Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine(Strings.UsisServer);
            Console.WriteLine(Strings.Copyright);
            Console.WriteLine();

            return ServicesHost.StartServices(args);
        }

        #endregion Main method
    }
}

// eof "Program.cs"
