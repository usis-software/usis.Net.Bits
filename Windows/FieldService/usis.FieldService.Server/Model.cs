//
//  @(#) Model.cs
//
//  Project:    usis.FieldService.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using usis.Framework.Data.Entity;
using System.Linq;

namespace usis.FieldService.Server
{
    //  -----------
    //  Model class
    //  -----------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class Model : DBContextModel<DBContext>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public Model() { IsLoggingEnabled = false; IsAutomaticMigrationsEnabled = true; }

        #endregion construction

        #region overrides

        //  -----------------
        //  NewContext method
        //  -----------------

        protected override DBContext NewContext()
        {
            return new DBContext(DataSource);
        }

        #endregion overrides

        //protected override void OnStart()
        //{
        //    base.OnStart();

        //    Test();
        //}

        //[SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        //internal void Test()
        //{
        //    var clientId = Guid.Parse("53bc049b-596c-4c40-b323-afa14be5eb86");
        //    var userId = Guid.Parse("a2b57cdc-780b-4066-b05f-2b95745b4fa2");
        //    var sid = "S-1-5-21-2793813511-2749632680-2756891147-500";
        //    UsingContext(db =>
        //    {
        //        var client = db.Clients.Find(clientId);
        //        if (client == null) db.Clients.Add(client = new Client() { Id = clientId, Name = "usis GmbH" });
        //        var user = db.Users.Find(userId);
        //        if (user == null) db.Users.Add(user = new User() { Id = userId, ClientId = clientId });
        //        var windowsUser = db.WindowsUsers.Find(userId, sid);
        //        if (windowsUser == null) db.WindowsUsers.Add(windowsUser = new WindowsUser() { UserId = userId, Sid = sid });
        //        db.SaveChanges();
        //    });
        //}

        private static IEnumerable<Guid> EnumerateUserIdsForSid(DBContext db, string sid)
        {
            return from wu in db.WindowsUsers where wu.Sid == sid && wu.Deleted == 0 select wu.UserId;
        }

        private static Guid? FindUserIdBySid(DBContext db, string sid)
        {
            var userIds = EnumerateUserIdsForSid(db, sid).Take(2).ToArray();
            switch (userIds.Length)
            {
                case 0:
                    return null;
                case 1:
                    return userIds[0];
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Guid? GetClientIdForUser(DBContext db, Guid userId)
        {
            return db.Users.Find(userId)?.ClientId;
        }

        internal Guid? GetClientIdForUser(Guid userId)
        {
            return UsingContext(db => GetClientIdForUser(db, userId));
        }

        internal Guid? FindUserIdBySid(string sid)
        {
            return UsingContext(db => FindUserIdBySid(db, sid));
        }
    }
}

// eof "Model.cs"
