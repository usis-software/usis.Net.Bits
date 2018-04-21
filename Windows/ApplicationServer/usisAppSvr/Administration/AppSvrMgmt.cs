//
//  @(#) AppSvrMgmt.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using usis.Framework;

namespace usis.ApplicationServer
{
    //  ----------------
    //  AppSvrMgmt class
    //  ----------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class AppSvrMgmt : Platform.ContextInjectable<IApplication>, IAppSvrMgmt
    {
        #region methods

        //  -------------------------------
        //  ListApplicationInstances method
        //  -------------------------------

        OperationResultList<ApplicationInstanceInfo> IAppSvrMgmt.ListApplicationInstances()
        {
            return OperationResultList.Invoke(() => Service.Applications);
        }

        //  ---------------------------------
        //  GetApplicationInstanceInfo method
        //  ---------------------------------

        OperationResult<ApplicationInstanceInfo> IAppSvrMgmt.GetApplicationInstanceInfo(Guid instanceId)
        {
            return OperationResult.Invoke(() => Service.GetApplicationInstanceInfo(instanceId));
        }

        //  ---------------------
        //  ExecuteCommand method
        //  ---------------------

        OperationResult IAppSvrMgmt.ExecuteCommand(Guid instanceId, ApplicationCommand command)
        {
            return OperationResult.Invoke(() => Service.ExecuteCommand(instanceId, command));
        }

        #endregion methods

        #region private properties

        //  ----------------
        //  Service property
        //  ----------------

        private Service Service => Context.Extensions.Find<AdminExtension>().Service.Service;

        #endregion private properties
    }
}

// eof "AppSvrMgmt.cs"
