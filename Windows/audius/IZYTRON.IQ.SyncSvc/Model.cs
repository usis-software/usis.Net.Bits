//
//  @(#) Model.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using usis.Framework.Entity;

namespace IZYTRON.IQ
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

        internal Model() { IsAutomaticMigrationsEnabled = true; }

        #endregion construction

        #region overrides

        //  -----------------
        //  NewContext method
        //  -----------------

        protected override DBContext NewContext()
        {
            return new DBContext("SyncService");
        }

        #endregion overrides

        #region methods

        //  ---------------------
        //  GetClientState method
        //  ---------------------

        internal SyncState GetClientState(Guid databaseId)
        {
            return FindOrCreateClient(databaseId).State;
        }

        //  ------------------------
        //  ReportClientState method
        //  ------------------------

        internal void ReportClientState(Guid databaseId, SyncState state)
        {
            UsingContext(db =>
            {
                var client = FindOrCreateClient(db, databaseId);
                var newState = client.State;
                switch (state)
                {
                    case SyncState.Unknown:
                        break;
                    case SyncState.ConnectionFailed:
                        break;
                    case SyncState.ReadyToUpload:
                        /**/
                        newState = state;
                        /**/
                        break;
                    case SyncState.PreparingToUpload:
                        if (client.State == SyncState.ReadyToUpload) newState = state;
                        else throw new InvalidOperationException();
                        break;
                    case SyncState.Uploading:
                        break;
                    case SyncState.Merging:
                        break;
                    case SyncState.ConflictsPending:
                        break;
                    case SyncState.PreparingToDownload:
                        break;
                    case SyncState.Downloading:
                        break;
                    case SyncState.ApplyingDownload:
                        break;
                    default:
                        break;
                }
                if (client.State != newState)
                {
                    client.State = newState;
                    db.SaveChanges();
                }
            });
        }

        #region private methods

        //  -------------------------
        //  FindOrCreateClient method
        //  -------------------------

        private Client FindOrCreateClient(Guid databaseId)
        {
            return UsingContext(db => { return FindOrCreateClient(db, databaseId); });
        }

        private static Client FindOrCreateClient(DBContext db, Guid databaseId)
        {
            var client = db.Clients.Find(databaseId);
            if (client == null)
            {
                client = Client.NewClient(databaseId);
                db.Clients.Add(client);
                db.SaveChanges();
            }
            return client;
        }

        #endregion private methods

        #endregion methods
    }
}

// eof "Model.cs"
