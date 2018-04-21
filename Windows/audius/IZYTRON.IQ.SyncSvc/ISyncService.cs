//
//  @(#) ISyncService.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.ServiceModel;

namespace IZYTRON.IQ
{
    #region SyncState enumeration

    //  ---------------------
    //  SyncState enumeration
    //  ---------------------

    /// <summary>
    /// Specifies the current synchronization state of a client database.
    /// </summary>

    public enum SyncState
    {
        /// <summary>
        /// The service is ready for uploading the database from a client
        /// </summary>

        ReadyForUpload,

        /// <summary>
        /// The upload of a client database is in progress.
        /// </summary>

        Uploading,

        Merging,
        ConflictsPending,
        ReadyForDownload,
        Downloading
    }

    #endregion SyncState enumeration

    //  ----------------------
    //  ISyncService interface
    //  ----------------------

    /// <summary>
    /// Defines methods for a client to communicate with the IZYTRON.IQ synchronization service.
    /// </summary>

    [ServiceContract]
    public interface ISyncService
    {
        //  ---------------
        //  GetState method
        //  ---------------

        /// <summary>
        /// Gets the synchronization state of the client with the specified database identifier.
        /// </summary>
        /// <param name="databaseId">The database identifier.</param>
        /// <returns>
        /// The current synchronization state for the client.
        /// </returns>

        [OperationContract]
        SyncState GetState(Guid databaseId);
    }
}

// eof "ISyncService.cs"
