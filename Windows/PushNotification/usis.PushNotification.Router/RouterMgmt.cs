//
//  @(#) RouterMgmt.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using usis.Framework;
using usis.Framework.ServiceModel;
using usis.Platform.Data;

namespace usis.PushNotification
{
    //  ----------------
    //  RouterMgmt class
    //  ----------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class RouterMgmt : ServiceBase, IRouterMgmt
    {
        #region IRouterMgmt implementation

        //  ------------------
        //  GetJobState method
        //  ------------------

        OperationResult<JobState> IRouterMgmt.GetJobState(Guid jobId)
        {
            return OperationResult.Invoke(() => Context.With<JobEngine>(true).GetJobState(jobId));
        }

        //  ---------------------
        //  GetJobProgress method
        //  ---------------------

        OperationResult<JobProgress> IRouterMgmt.GetJobProgress(Guid jobId)
        {
            return OperationResult.Invoke(() => Context.With<JobEngine>(true).GetProgress(jobId));
        }

        //  ---------------------------------
        //  LoadRegisteredChannelTypes method
        //  ---------------------------------

        OperationResultList<ChannelType> IRouterMgmt.LoadRegisteredChannelTypes()
        {
            return OperationResultList.Invoke(() => Model.RegisteredChannelTypes);
        }

        //  ---------------------
        //  LoadDataSource method
        //  ---------------------

        OperationResult<DataSource> IRouterMgmt.LoadDataSource()
        {
            return OperationResult.Invoke(() => Model.DataSource);
        }

        //  -------------
        //  Backup method
        //  -------------

        OperationResult<Guid> IRouterMgmt.Backup(string path)
        {
            return OperationResult.Invoke(() => this.RunAsJob((job) => Model.ExportData(job, path)));
        }

        //  --------------
        //  Restore method
        //  --------------

        OperationResult<Guid> IRouterMgmt.Restore(string path)
        {
            return OperationResult.Invoke(() => this.RunAsJob(() => Model.ImportData(path)));
        }

        #endregion IRouterMgmt implementation
    }

    #region RouterMgmtSnapIn class

    //  ----------------------
    //  RouterMgmtSnapIn class
    //  ----------------------

    internal class RouterMgmtSnapIn : NamedPipeServiceHostSnapIn<RouterMgmt, IRouterMgmt>
    {
        public RouterMgmtSnapIn() : base(false) { }
    }

    #endregion RouterMgmtSnapIn class
}

// eof "RouterMgmt.cs"
