//
//  @(#) Program.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using usis.Cocoa.Framework;
using usis.Mobile;

namespace usis.Cocoa.PNRouter
{
    //  -------------
    //  Program class
    //  -------------

    internal static class Program
    {
        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            Application.Run<AppDelegate>(args,
                typeof(AppLayoutSnapIn),
                typeof(SnapIn));
        }
    }
}

// eof "Program.cs"
