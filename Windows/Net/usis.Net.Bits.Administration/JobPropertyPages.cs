//
//  @(#) JobPropertyPages.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using usis.ManagementConsole;

namespace usis.Net.Bits.Administration
{
    #region JobGeneralPropertyPage class

    //  ---------------------
    //  JobGeneralPropertyPage class
    //  ---------------------

    internal sealed class JobGeneralPropertyPage : JobPropertyPageBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal JobGeneralPropertyPage(JobNode node) : base(node)
        {
            Title = Strings.JobGeneralPropertyPageTitle;
            Control = new JobPropertiesControl();
        }

        #endregion construction

        #region overrides

        //  --------------
        //  OnApply method
        //  --------------

        protected override bool OnApply() { return SaveChanges(); }

        //  -----------
        //  OnOK method
        //  -----------

        protected override bool OnOK() { return SaveChanges(); }

        #endregion overrides

        #region methods

        //  ------------------
        //  SaveChanges method
        //  ------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool SaveChanges()
        {
            try
            {
                ApplyChanges(Node?.Job);
                return true;
            }
            catch (Exception exception)
            {
                ParentSheet.ShowDialog(exception);
                return false;
            }
        }

        #endregion methods
    }

    #endregion JobGeneralPropertyPage class

    #region JobProgressPropertyPage class

    //  -----------------------------
    //  JobProgressPropertyPage class
    //  -----------------------------

    internal sealed class JobProgressPropertyPage : JobPropertyPageBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal JobProgressPropertyPage(JobNode node) : base(node)
        {
            Title = "Progress";
            Control = new JobProgressControl();
        }

        #endregion construction
    }

    #endregion JobProgressPropertyPage class

    #region JobPropertyPageBase class

    //  -------------------------
    //  JobPropertyPageBase class
    //  -------------------------

    internal abstract class JobPropertyPageBase : PropertyPage<JobNode>
    {
        #region properties

        protected JobNode Node { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        protected JobPropertyPageBase(JobNode node) { Node = node; }

        #endregion construction

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void OnInitialize()
        {
            base.OnInitialize();

            try { InjectProperties(Node); }
            catch (Exception exception) { ParentSheet.ShowDialog(exception); }
        }

        #endregion overrides
    }

    #endregion JobPropertyPageBase class
}

// eof "JobPropertyPages.cs"
