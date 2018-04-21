//
//  @(#) DefinitionPage.cs
//
//  Project:    audius GuV
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 audius GmbH. All rights reserved.

using System;
using System.ComponentModel;
using usis.Windows.Forms;

namespace audius.GuV.Wizard
{
    //  --------------------
    //  DefinitionPage class
    //  --------------------

    internal sealed class DefinitionPage : WizardPage<DefinitionPageControl, Model>
    {
        //  ---------------------
        //  InitializeView method
        //  ---------------------

        protected override void InitializeView(DefinitionPageControl view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            base.InitializeView(view);

            view.SelectedDefinitionChanged += (sender, e) => Dialog.UpdateButtons();
        }

        public override bool CanGoToNextPage()
        {
            return View.SelectedDefinition != null &&  base.CanGoToNextPage();
        }

        public override bool CanFinish()
        {
            return View.SelectedDefinition != null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public override void OnFinish(CancelEventArgs e)
        {
            Model.SelectDefinition(View.SelectedDefinition);
            Model.DoIt();

           e.Cancel = true;
        }
    }
}

// eof "DefinitionPage.cs"
