//
//  @(#) Program.cs
//
//  Project:    IDoc Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using usis.Framework.Windows.Forms;

namespace usis.Middleware.SAP.IDocEditor
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
