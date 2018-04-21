//
//  @(#) Program.cs
//
//  Project:    Storage Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using usis.Windows.Forms.Framework;

namespace usis.StorageEditor
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
        internal static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(args, typeof(SnapIn));
        }
    }
}

// eof "Program.cs"
