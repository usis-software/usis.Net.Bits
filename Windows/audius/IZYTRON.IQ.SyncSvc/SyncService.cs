//
//  @(#) SyncService.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using usis.Framework.ServiceModel;

namespace IZYTRON.IQ
{
    //  -----------------
    //  SyncService class
    //  -----------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal sealed class SyncService : ServiceBase<Model>, ISyncService
    {
        //  ---------------
        //  GetState method
        //  ---------------

        SyncState ISyncService.GetState(Guid databaseId)
        {
            return Model.GetClientState(databaseId);
        }

        //  ------------------
        //  ReportState method
        //  ------------------

        void ISyncService.ReportState(Guid databaseId, SyncState state)
        {
            Model.ReportClientState(databaseId, state);
        }
    }
}

// eof "SyncService.cs"
