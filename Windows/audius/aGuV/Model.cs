//
//  @(#) Model.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using Sagede.OfficeLine.Rewe.Bilanz;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using usis.Plaform;

namespace audius.GuV.Wizard
{
    //  -----------
    //  Model class
    //  -----------

    internal sealed class Model
    {
        #region fields

        private BindingList<DataSource> dataSources;
        private BindingList<Client> clients;
        private BindingList<Definition> definitions;

        #endregion fields

        #region properties

        //  --------------------
        //  DataSources property
        //  --------------------

        public IList<DataSource> DataSources
        {
            get
            {
                if (dataSources == null)
                {
                    dataSources = new BindingList<DataSource>(Services.LoadDataSources().ToList());
                }
                return dataSources;
            }
        }

        //  ---------------------------
        //  SelectedDataSource property
        //  ---------------------------

        public DataSource SelectedDataSource { get; private set; }

        //  ----------------
        //  Clients property
        //  ----------------

        public IList<Client> Clients
        {
            get
            {
                if (clients == null)
                {
                    clients = new BindingList<Client>(Services.LoadClients(SelectedDataSource).ToList());
                }
                return clients;
            }
        }

        //  -----------------------
        //  SelectedClient property
        //  -----------------------

        public Client SelectedClient { get; private set; }

        //  --------------------
        //  Definitions property
        //  --------------------

        public IList<Definition> Definitions
        {
            get
            {
                if (definitions == null)
                {
                    definitions = new BindingList<Definition>(Services.LoadDefinitions(SelectedDataSource, SelectedClient).ToList());
                }
                return definitions;
            }
        }

        //  ---------------------------
        //  SelectedDefinition property
        //  ---------------------------

        public Definition SelectedDefinition { get; private set; }

        #endregion properties

        #region methods

        //  -----------------------
        //  SelectDataSource method
        //  -----------------------

        public void SelectDataSource(DataSource dataSource)
        {
            SelectedDataSource = dataSource;

            clients?.Reset(() => Services.LoadClients(dataSource));
            SelectClient(null);
        }

        //  -------------------
        //  SelectClient method
        //  -------------------

        public void SelectClient(Client client)
        {
            SelectedClient = client;

            definitions?.Reset(() => Services.LoadDefinitions(SelectedDataSource, client));
        }

        //  -----------------------
        //  SelectDefinition method
        //  -----------------------

        public void SelectDefinition(Definition definition)
        {
            SelectedDefinition = definition;
        }

        #endregion methods

        internal void DoIt()
        {
            Debug.WriteLine(SelectedDefinition);

            List<Position> positions;
            using (var auswertung = new Auswertung())
            {
                auswertung.LoadArchivFromString(SelectedDefinition.Text);
                positions = new List<Position>(Services.ParsePositions(auswertung.Positionen));
            }

            Services.ExecuteScript(SelectedDataSource, Services.GetManifestResourceString("audius.GuV.Wizard.DropViews.sql"));
            Services.ExecuteScript(SelectedDataSource, Services.GetManifestResourceString("audius.GuV.Wizard.CreateTables.sql"));
            Services.ExecuteScript(SelectedDataSource, Services.GetManifestResourceString("audius.GuV.Wizard.CreateViews.sql"));

            Services.FlattenPositions(positions);

            Services.SaveDefinitions(SelectedDataSource, positions);
        }
    }
}

// eof "Model.cs"
