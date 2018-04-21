//
//  @(#) Program.cs
//
//  Project:    usis
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2017 usis GmbH. All rights reserved.

using System;
using usis.Framework;

namespace usis
{
    //  -------------
    //  Program class
    //  -------------

    internal static class Program
    {
        //  -----------
        //  Main method
        //  -----------

        [STAThread]
        internal static void Main()
        {
            var configuration = Application.ReadConfigurationFile("usis.Windows.Application");
            if (configuration != null)
            {
                new Windows.Framework.Application(configuration).Run();
            }
        }
    }
}

// eof "Program.cs"
