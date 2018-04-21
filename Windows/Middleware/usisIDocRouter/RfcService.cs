//
//  @(#) RfcService.cs
//
//  Project:    usis IDoc Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using usis.Platform;

namespace usis.Middleware.SAP
{
    #region RfcService class

    //  ----------------
    //  RfcService class
    //  ----------------

    internal class RfcService
    {
        #region fields

        private Dictionary<string, List<Type>> registeredHandlers = new Dictionary<string, List<Type>>(StringComparer.Ordinal);
        private Dictionary<string, RfcServer> servers = new Dictionary<string, RfcServer>(StringComparer.Ordinal);

        #endregion fields

        #region public methods

        //  ----------------------
        //  RegisterHandler method
        //  ----------------------

        /// <summary>
        /// Registers the specified type as a handler for a RFC service.
        /// </summary>
        /// <param name="serverName">
        /// The RFC server name.
        /// </param>
        /// <param name="handler">
        /// The type that implements the RFC handler methods.
        /// </param>
        /// <exception cref="ArgumentNullOrWhiteSpaceException">
        /// The <paramref name="serverName"/> is <c>null</c>, empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="handler"/> is a <c>null</c> reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        public void RegisterHandler(string serverName, Type handler)
        {
            if (string.IsNullOrWhiteSpace(serverName)) throw new ArgumentNullOrWhiteSpaceException(nameof(serverName));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (!registeredHandlers.TryGetValue(serverName, out List<Type> handlers))
            {
                handlers = new List<Type>();
                registeredHandlers.Add(serverName, handlers);
            }
            handlers.Add(handler);
        }

        //  ------------
        //  Start method
        //  ------------

        public void Start()
        {
            foreach (var serverName in registeredHandlers.Keys)
            {
                var handlers = registeredHandlers[serverName];
                var server = RfcServerManager.GetServer(serverName, handlers.ToArray());
                servers.Add(serverName, server);
            }
            foreach (var server in servers.Values)
            {
                server.TransactionIDHandler = new TransactionHandler();
                server.Start();
            }
        }

        //  ---------------
        //  Shutdown method
        //  ---------------

        public void Shutdown()
        {
            foreach (var server in servers.Values)
            {
                server.Shutdown(false);
            }
        }

        #endregion public methods
    }

    #endregion RfcService class

    #region TransactionHandler class

    //  ------------------------
    //  TransactionHandler class
    //  ------------------------

    internal class TransactionHandler : ITransactionIDHandler
    {
        #region fields

        private static Dictionary<Guid, Transaction> transactions = new Dictionary<Guid, Transaction>();

        #endregion fields

        #region ITransactionIDHandler implementation

        //  -------------------------
        //  CheckTransactionID method
        //  -------------------------

        bool ITransactionIDHandler.CheckTransactionID(RfcServerContextInfo ctx, RfcTID tid)
        {
            var transaction = RegisterTransaction(tid);
            return !(transaction.State == TransactionState.Committed);
        }

        //  -------------
        //  Commit method
        //  -------------

        void ITransactionIDHandler.Commit(RfcServerContextInfo ctx, RfcTID tid)
        {
            var transaction = FindTransaction(tid);
            if (transaction == null) throw new RfcInvalidStateException("Invalid transaction (TID not registered).");
            transaction.Commit();
        }

        //  ---------------
        //  Rollback method
        //  ---------------

        void ITransactionIDHandler.Rollback(RfcServerContextInfo ctx, RfcTID tid)
        {
            var transaction = FindTransaction(tid);
            if (transaction == null) throw new RfcInvalidStateException("Invalid transaction (TID not registered).");
            transaction.Rollback();
        }

        //  ---------------------------
        //  ConfirmTransactionID method
        //  ---------------------------

        void ITransactionIDHandler.ConfirmTransactionID(RfcServerContextInfo ctx, RfcTID tid)
        {
            UnregisterTransaction(tid);
        }

        #endregion ITransactionIDHandler implementation

        #region private methods

        //  ----------------------
        //  FindTransaction method
        //  ----------------------

        private static Transaction FindTransaction(RfcTID tid)
        {
            lock (transactions)
            {
                return transactions.TryGetValue(tid.GUID, out Transaction transaction) ? transaction : null;
            }
        }

        //  --------------------------
        //  RegisterTransaction method
        //  --------------------------

        private static Transaction RegisterTransaction(RfcTID tid)
        {
            lock (transactions)
            {
                if (!transactions.TryGetValue(tid.GUID, out Transaction transaction))
                {
                    transaction = new Transaction(tid);
                    transactions.Add(transaction.Id, transaction);
                }
                return transaction;
            }
        }

        //  ----------------------------
        //  UnregisterTransaction method
        //  ----------------------------

        private static void UnregisterTransaction(RfcTID tid)
        {
            lock (transactions)
            {
                transactions.Remove(tid.GUID);
            }
        }

        #endregion private methods
    }

    #endregion TransactionHandler class

    #region TransactionState enumeration

    //  ----------------------------
    //  TransactionState enumeration
    //  ----------------------------

    internal enum TransactionState
    {
        Created,
        Committed,
        RolledBack
    }

    #endregion TransactionState enumeration

    #region Transaction class

    //  -----------------
    //  Transaction class
    //  -----------------

    internal class Transaction
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Transaction(RfcTID tid) { Id = tid.GUID; }

        #endregion construction

        #region properties

        //  -----------
        //  Id property
        //  -----------

        internal Guid Id { get; }

        //  --------------
        //  State property
        //  --------------

        internal TransactionState State { get; private set; }

        #endregion properties

        #region methods

        //  -------------
        //  Commit method
        //  -------------

        internal void Commit() { State = TransactionState.Committed; }

        //  ---------------
        //  Rollback method
        //  ---------------

        internal void Rollback() { State = TransactionState.RolledBack; }

        #endregion methods
    }

    #endregion Transaction class
}

// eof "RfcService.cs"
