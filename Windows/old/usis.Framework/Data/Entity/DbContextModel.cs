//
//  @(#) DbContextModel.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

namespace usis.Framework.Data.Entity
{
    //  ------------------------------
    //  DbContextModel<TContext> class
    //  ------------------------------

    internal abstract class DbContextModel<TContext> : DataSourceModel where TContext : DbContext
    {
        protected abstract TContext CreateDbContext();
    }
}

// eof "DbContextModel.cs"
