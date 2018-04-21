//
//  @(#) JobListView.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using usis.ManagementConsole;

namespace usis.Net.Bits.Administration
{
    //  -----------------
    //  JobListView class
    //  -----------------

    internal class JobListView : ListView<SnapIn>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.JobListViewDisplayName,
            ViewType = typeof(JobListView),
            Options = MmcListViewOptions.SingleSelect
        };

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            Columns[0].SetWidth(192);

            Columns.Add("Description", 72);
            Columns.Add("Type", 72);
            Columns.Add("%", 48);
            Columns.Add("State", 96);
            Columns.Add("priority", 96);
            Columns.Add("Owner", 96);

            base.OnInitialize(status);
        }

        #endregion overrides
    }
}

// eof "JobListView.cs"
