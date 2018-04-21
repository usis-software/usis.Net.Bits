//
//  @(#) Entities.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using Sagede.OfficeLine.Data.Configuration;
using System.Collections.Generic;
using System.Globalization;
using usis.Middleware.Sage.OfficeLine.Data;

namespace audius.GuV.Wizard
{
    #region DataSource class

    //  ----------------
    //  DataSource class
    //  ----------------

    internal sealed class DataSource
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DataSource(DataSourceSettings settings)
        {
            Description = settings.Description;
            ServerName = settings.ServerName;
            DatabaseName = settings.DatabaseName;
        }

        internal DataSource()
        {
            Description = "Testdatenbank (AUSAGE-OL2016 - OLaudiusGroupTest)";
            ServerName = "AUSAGE-OL2016";
            DatabaseName = "OLaudiusGroupTest";
        }

        #endregion construction

        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        public string Description { get; }

        //  -------------------
        //  ServerName property
        //  -------------------

        public string ServerName { get; }

        //  ---------------------
        //  DatabaseName property
        //  ---------------------

        public string DatabaseName { get; }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString() { return Description; }

        #endregion overrides
    }

    #endregion DataSource class

    #region Client class

    //  ------------
    //  Client class
    //  ------------

    internal sealed class Client
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Client(ClientProperty clientProperty)
        {
            Id = clientProperty.Client;
            Name = clientProperty.Value;
        }

        #endregion construction

        #region properties

        //  -----------
        //  Id property
        //  -----------

        public short Id { get; }

        //  -------------
        //  Name property
        //  -------------

        public string Name { get; }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} - {1}", Id, Name);
        }

        #endregion overrides
    }

    #endregion Client class

    #region Definition class

    //  ----------------
    //  Definition class
    //  ----------------

    internal sealed class Definition
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Definition(BalanceSheetDefinition definition)
        {
            Description = definition.Description;
            Text = definition.Definition;
        }

        #endregion construction

        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        public string Description { get; }

        public string Text { get; }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString() { return Description; }

        #endregion overrides
    }

    #endregion Definition class

    #region Position class

    //  --------------
    //  Position class
    //  --------------

    internal sealed class Position
    {
        #region fields

        private List<GeneralLedgerAccountRange> ranges = new List<GeneralLedgerAccountRange>();
        private List<Position> positions = new List<Position>();

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Position(string name) { Name = name; }

        internal Position(string name, IEnumerable<GeneralLedgerAccountRange> accounts) : this(name)
        {
            AddAccounts(accounts);
        }

        #endregion construction

        #region properties

        //  -----------
        //  Id property
        //  -----------

        internal int? Id { get; set; }

        //  --------------
        //  Index property
        //  --------------

        internal int? Index { get; set; }

        //  -------------
        //  Name property
        //  -------------

        internal string Name { get; }

        //  ----------------------
        //  AccountRanges property
        //  ----------------------

        internal IEnumerable<GeneralLedgerAccountRange> AccountRanges => ranges;

        //  ------------------
        //  Positions property
        //  ------------------

        internal IEnumerable<Position> Positions => positions;

        #endregion properties

        #region methods

        //  ------------------
        //  AddAccounts method
        //  ------------------

        internal void AddAccounts(string from, string to)
        {
            ranges.Add(new GeneralLedgerAccountRange(from, to));
        }

        internal void AddAccounts(IEnumerable<GeneralLedgerAccountRange> accounts)
        {
            ranges.AddRange(accounts);
        }

        //  --------------------
        //  ClearAccounts method
        //  --------------------

        internal void ClearAccounts() { ranges.Clear(); }

        //  ------------------
        //  AddPosition method
        //  ------------------

        internal void AddPosition(Position position) { positions.Add(position); }

        //  ---------------------
        //  ClearPositions method
        //  ---------------------

        internal void ClearPositions() { positions.Clear(); }

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString() { return string.Format(CultureInfo.CurrentCulture, "Id={0}:{1} (Index={2})", Id, Name, Index); }

        #endregion methods
    }

    #endregion Position class

    #region GeneralLedgerAccountRange class

    //  -------------------------------
    //  GeneralLedgerAccountRange class
    //  -------------------------------

    internal sealed class GeneralLedgerAccountRange
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal GeneralLedgerAccountRange(string from, string to) { From = from; To = to; }

        #endregion construction

        #region properties

        //  -------------
        //  From property
        //  -------------

        public string From { get; }

        //  -----------
        //  To property
        //  -----------

        public string To { get; }

        #endregion properties

        #region methods

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0}-{1}", From, To);

        #endregion methods
    }

    #endregion GeneralLedgerAccountRange class
}

// eof "Entities.cs"
