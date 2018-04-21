//
//  @(#) Form.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using usis.Windows.Forms;

namespace audius.GuV.Wizard
{
    //  ----------
    //  Form class
    //  ----------

    internal partial class Form : WizardDialog<Model>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public Form() : base(new Model())
        {
            DoubleBuffered = true;

            InitializeComponent();

            AddPages(
                new WelcomePage(),
                new DataSourcePage(),
                new ClientPage(),
                new DefinitionPage());
        }

        #endregion construction
    }
}

// eof "Form.cs"
