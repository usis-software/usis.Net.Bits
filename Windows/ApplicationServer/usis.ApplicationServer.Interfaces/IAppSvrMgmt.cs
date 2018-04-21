//
//  @(#) IAppSvrMgmt.cs
//
//  Project:    usis Application Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2017 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;
using usis.Framework;

namespace usis.ApplicationServer
{
    //  ---------------------
    //  IAppSvrMgmt interface
    //  ---------------------

    /// <summary>
    /// Definies methods to manage an usis Application Server.
    /// </summary>

    [ServiceContract]
    public interface IAppSvrMgmt
    {
        //  -------------------------------
        //  ListApplicationInstances method
        //  -------------------------------

        /// <summary>
        /// Lists the application instances on the Application Server.
        /// </summary>
        /// <returns>
        /// An <see cref="OperationResultList"/> with an enumerator to iterate
        /// the application instances on the Application Server.
        /// </returns>

        [OperationContract]
        OperationResultList<ApplicationInstanceInfo> ListApplicationInstances();

        //  ---------------------------------
        //  GetApplicationInstanceInfo method
        //  ---------------------------------

        /// <summary>
        /// Gets information about an application instance.
        /// </summary>
        /// <param name="instanceId">The identifier of ab application instance.</param>
        /// <returns>
        /// An <see cref="OperationResult{ApplicationInstanceInfo}"/> with the instance information as <see cref="OperationResult{ApplicationInstanceInfo}.ReturnValue"/>.
        /// </returns>

        [OperationContract]
        OperationResult<ApplicationInstanceInfo> GetApplicationInstanceInfo(Guid instanceId);

        //  ---------------------
        //  ExecuteCommand method
        //  ---------------------

        /// <summary>
        /// Executes a command to an application instance.
        /// </summary>
        /// <param name="instanceId">The identifier of ab application instance.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> with the information about the execution.
        /// </returns>

        [OperationContract]
        OperationResult ExecuteCommand(Guid instanceId, ApplicationCommand command);
    }
}

// eof "IAppSvrMgmt.cs"
