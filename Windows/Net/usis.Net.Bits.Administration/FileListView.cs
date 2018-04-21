//
//  @(#) FileListView.cs
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
    //  ------------------
    //  FileListView class
    //  ------------------

    internal sealed class FileListView : ListView<SnapIn, ResultNode, JobNode>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = "Files",
            ViewType = typeof(FileListView),
        };

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            DescriptionBarText = "Files";

            Columns[0].Title = "Local Name";
            Columns[0].SetWidth(256);

            Columns.Add("Remote Name", 256);

            LoadFiles();
        }

        //  ----------------
        //  OnRefresh method
        //  ----------------

        protected override void OnRefresh(AsyncStatus status)
        {
            ResultNodes.Clear();
            LoadFiles();
        }

        #endregion overrides

        #region methods

        //  ----------------
        //  LoadFiles method
        //  ----------------

        private void LoadFiles()
        {
            foreach (var file in ScopeNode.Job.EnumerateFiles())
            {
                var node = new ResultNode()
                {
                    DisplayName = file.LocalName,
                    ImageIndex = ScopeNode.Job.JobType == BackgroundCopyJobType.Download ? 3 : 4
                };
                node.SubItemDisplayNames.Add(file.RemoteName);
                ResultNodes.Add(node);
            }
        }

        //  --------------
        //  Refresh method
        //  --------------

        internal void Refresh(AsyncStatus status) { OnRefresh(status); }

        #endregion methods
    }
}

// eof "FileListView.cs"
