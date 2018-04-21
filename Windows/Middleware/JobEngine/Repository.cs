//
//  @(#) Repository.cs
//
//  Project:    usis Job Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using usis.Framework;
using usis.Platform;

namespace usis.JobEngine
{
    //  ----------------
    //  Repository class
    //  ----------------

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Repository : ContextInjectable<IApplication>, IRepository
    {
        //  ---------------
        //  StartJob method
        //  ---------------

        void IRepository.StartJob(string key)
        {
            Context.With<JobRepository>(true).Start(key);
        }
    }
}

// eof "Repository.cs"
