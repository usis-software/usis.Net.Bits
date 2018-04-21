//
//  @(#) Configuration.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace usis.PushNotification.Migrations
{
    //  -------------------
    //  Configuration class
    //  -------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class Configuration : DbMigrationsConfiguration<DBContext>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        #endregion construction
    }
}

// eof "Configuration.cs"
