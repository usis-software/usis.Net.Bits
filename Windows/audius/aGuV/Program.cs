//
//  @(#) Program.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using System;
using System.Windows.Forms;

namespace audius.GuV.Wizard
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        internal static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form());
        }

        #endregion Main method
    }
}

// eof "Program.cs"
