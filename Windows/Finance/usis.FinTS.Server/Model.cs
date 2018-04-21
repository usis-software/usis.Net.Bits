//
//  @(#) Model.cs
//
//  Project:    usis.FinTS.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using usis.FinTS.Server.Database;
using usis.Framework.Entity;

namespace usis.FinTS.Server
{
    //  -----------
    //  Model class
    //  -----------

    internal class Model : DBContextModel<DBContext>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Model(Action initialized) : base(initialized) { IsAutomaticMigrationsEnabled = true; IsLoggingEnabled = true; }

        #endregion construction

        #region overrides

        //  -----------------
        //  NewContext method
        //  -----------------

        protected override DBContext NewContext() { return new DBContext("usis.FinTS.Server"); }

        #endregion overrides

        //  ---------------------
        //  ProcessMessage method
        //  ---------------------

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "message")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "dialog")]
        internal Message ProcessMessage(BankDialog dialog, Message message)
        {
            Console.WriteLine(message);

            // cancel dialog
            dialog.Cancel(message);
            return null;
        }
    }
}

// eof "Model.cs"
