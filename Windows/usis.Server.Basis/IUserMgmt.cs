//
//  @(#) IUserMgmt.cs
//
//  Project:    usis.Server.Basis
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Framework.Entity;

namespace usis.Server.Basis
{
    public sealed class Test : IDisposable
    {
        public void Dispose()
        {
        }
    }

    [ServiceContract]
    public interface IUserMgmt
    {
        [OperationContract]
        void RequestAccountWithMailAddress(string mailAddress, string password);
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class UserMgmt : IUserMgmt
    {
        [WebGet]
        public void RequestAccountWithMailAddress(string mailAddress, string password)
        {
            using (var model = new UserMgmtModel())
            {
                // ToDo: check if mail address already exists
                AccountRequest request = model.FindAccountRequestWithMailAddress(/*mailAddress*/);
                if (request == null)
                {
                    // check if it is a valid mail address
                }
            }
            using (var db = new BasisDbContext())
            {
                var request = new AccountRequest()
                {
                    MailAddress = mailAddress,
                    Password = password
                };
                db.AccountRequests.Add(request);
                db.SaveChanges();
            }
        }
    }

    public class SnapIn : usis.Framework.ServiceModel.WebServiceHostSnapIn<UserMgmt, IUserMgmt>
    {
    }

    public class BasisDbContext : DbContext
    {
        public BasisDbContext() : base("name=usis") { }

        public DbSet<AccountRequest> AccountRequests
        {
            get;
            set;
        }
    }

    public sealed class UserMgmtModel : IDisposable
    {
        private BasisDbContext db;

        private BasisDbContext Db
        {
            get
            {
                if (this.db == null)
                {
                    this.db = new BasisDbContext();
                }
                return this.db;
            }
        }

        public void Dispose()
        {
            if (this.db != null) this.db.Dispose();
        }

        public AccountRequest FindAccountRequestWithMailAddress(/*string mailAddress*/)
        {
            return this.Db.AccountRequests.Find(Guid.Empty);
        }
    }

    [Table("AccountRequest")]
    public class AccountRequest : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id
        {
            get;
            set;
        }

        public string MailAddress
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public byte State
        {
            get;
            set;
        }
    }
}

// eof "IUserMgmt.cs"
