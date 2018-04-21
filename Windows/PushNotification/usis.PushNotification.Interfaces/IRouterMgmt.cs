//
//  @(#) IRouterMgmt.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;
using usis.Framework;
using usis.Platform.Data;

namespace usis.PushNotification
{
    //  ---------------------
    //  IRouterMgmt interface
    //  ---------------------

    /// <summary>
    /// Provides methods to manage a Push Notification Router.
    /// </summary>

    [ServiceContract]
    public interface IRouterMgmt
    {
        //  ---------------------------------
        //  LoadRegisteredChannelTypes method
        //  ---------------------------------

        /// <summary>
        /// Loads the registered channel types.
        /// </summary>
        /// <returns>
        /// An <see cref="OperationResultList"/> with the registered channel types as return value.
        /// </returns>

        [OperationContract]
        OperationResultList<ChannelType> LoadRegisteredChannelTypes();

        //  ---------------------
        //  LoadDataSource method
        //  ---------------------

        /// <summary>
        /// Loads the data source used by the Push Notification Router.
        /// </summary>
        /// <returns>
        /// An <see cref="OperationResult" /> with the data source as return value.
        /// </returns>

        [OperationContract]
        OperationResult<DataSource> LoadDataSource();

        //  -------------
        //  Backup method
        //  -------------

        /// <summary>
        /// Exports the data of the push notification router database
        /// to a file.
        /// </summary>
        /// <param name="path">The path of the archive file to create.</param>
        /// <returns>
        /// An <see cref="OperationResult" /> with informations about the execution.
        /// </returns>

        [OperationContract]
        OperationResult<Guid> Backup(string path);

        //  --------------
        //  Restore method
        //  --------------

        /// <summary>
        /// Restores the data of the push notification router database
        /// from the specified file.
        /// </summary>
        /// <param name="path">The path of the backup file.</param>
        /// <returns>
        /// An <see cref="OperationResult" /> with informations about the execution.
        /// </returns>

        [OperationContract]
        OperationResult<Guid> Restore(string path);

        //  ------------------
        //  GetJobState method
        //  ------------------

        /// <summary>
        /// Gets the state of the job with the specified identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>
        /// The current state of the job.
        /// </returns>

        [OperationContract]
        OperationResult<JobState> GetJobState(Guid jobId);

        //  ---------------------
        //  GetJobProgress method
        //  ---------------------

        /// <summary>
        /// Gets the progress of the job with the specified identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>The current progress of the job,</returns>

        [OperationContract]
        OperationResult<JobProgress> GetJobProgress(Guid jobId);
    }
}

// eof "IRouterMgmt.cs"
