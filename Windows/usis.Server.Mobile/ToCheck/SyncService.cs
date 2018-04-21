//
//  @(#) SyncService.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Platform;

namespace usis.Server.Mobile
{
    #region ISyncService interface

    //  ----------------------
    //  ISyncService interface
    //  ----------------------

    [ServiceContract]
    public interface ISyncService
    {
        [OperationContract]
        [WebInvoke(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        SyncConfirmation EntitiesChanged(SyncDelta delta);

    } // ISyncService interface

    #endregion ISyncService interface

    #region SyncService class

    //  -----------------
    //  SyncService class
    //  -----------------

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SyncService : ISyncService
    {
        public SyncConfirmation EntitiesChanged(SyncDelta delta)
        {
            if (delta == null) throw new ArgumentNullException("delta");

            foreach (var item in delta.Inserts)
            {
                Debug.Print("Insert: {0}", item);
            }
            foreach (var item in delta.Updates)
            {
                Debug.Print("Update: {0}", item);
            }
            foreach (var item in delta.Deletes)
            {
                Debug.Print("Delete: {0}", item);
            }
            return new SyncConfirmation(delta);
        }

    } // SyncService class

    #endregion SyncService class

    #region SyncDelta class

    //  ---------------
    //  SyncDelta class
    //  ---------------

    [DataContract]
    public class SyncDelta
    {
        public SyncDelta()
        {
            Inserts = new Collection<JsonDictionary>();
            Updates = new Collection<JsonDictionary>();
            Deletes = new Collection<JsonDictionary>();
        }

        [DataMember]
        public Collection<JsonDictionary> Inserts
        {
            get;
            private set;
        }
        [DataMember]
        public Collection<JsonDictionary> Updates
        {
            get;
            private set;
        }
        [DataMember]
        public Collection<JsonDictionary> Deletes
        {
            get;
            private set;
        }

    } // SyncDelta class

    #endregion SyncDelta class

    #region SyncConfirmation class

    //  ----------------------
    //  SyncConfirmation class
    //  ----------------------

    [DataContract]
    public class SyncConfirmation : SyncDelta
    {
        internal SyncConfirmation(SyncDelta delta)
        {
            foreach (var item in delta.Inserts)
            {
                Inserts.Add(item);
            }
            foreach (var item in delta.Updates)
            {
                Updates.Add(item);
            }
            foreach (var item in delta.Deletes)
            {
                Deletes.Add(item);
            }
        }

    } // SyncConfirmation class

    #endregion SyncConfirmation class

} // namespace usis.Server.Mobile

// eof "SyncService.cs"
