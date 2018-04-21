//
//  @(#) Program.cs
//
//  Project:    usis Job Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

namespace usis.JobEngine
{
    //  -------------
    //  Program class
    //  -------------

    internal class Program
    {
        //  -----------
        //  Main method
        //  -----------

        internal static void Main(/*string[] args*/)
        {
            StartJob("test");
        }

        #region private methods

        //  ---------------
        //  StartJob method
        //  ---------------

        private static void StartJob(string key)
        {
            using (var client = new Platform.ServiceModel.ServiceClient<IRepository>())
            {
                client.Service.StartJob(key);
            }
        }

        #endregion private methods
    }
}

// eof "Program.cs"
