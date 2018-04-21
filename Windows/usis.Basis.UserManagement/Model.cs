//
//  @(#) Model.cs
//
//  Project:    Basis - User Management
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using usis.Framework.Entity;

namespace usis.Basis.UserManagement
{
    //  -----------
    //  Model class
    //  -----------

    internal sealed class Model : DBContextModel<DBContext>
    {
        #region overrides

        //  ------------------
        //  NewContext methods
        //  ------------------

        /// <summary>
        /// Creates a new database context object.
        /// </summary>
        /// <returns>
        /// A newly created database context object.
        /// </returns>

        protected override DBContext NewContext()
        {
            return new DBContext(DataSource);
        }

        #endregion overrides

        #region methods

        //  -----------------------------
        //  SignUpWithEmailAddress method
        //  -----------------------------

        internal void SignUpWithEmailAddress(string emailAddress, string password)
        {
            UsingContext((context) =>
            {
                var account = context.Accounts.Find(emailAddress);
                if (account == null)
                {
                    account = new Account() { Name = emailAddress };
                    context.Accounts.Add(account);
                    context.SaveChanges();
                }
            });
        }

        #endregion methods
    }
}

// eof "Model.cs"
