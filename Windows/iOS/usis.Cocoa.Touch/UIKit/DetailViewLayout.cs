//
//  @(#) DetailViewLayout.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using UIKit;
using usis.Platform.Data;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  ----------------------
    //  DetailViewLayout class
    //  ----------------------

    public class DetailViewLayout
    {
        #region fields

        private List<IDetailViewSection> sections = new List<IDetailViewSection>();

        #endregion fields

        #region properties

        //  -----------------
        //  Sections property
        //  -----------------

        public IEnumerable<IDetailViewSection> Sections => sections;

        //  -----------------------
        //  PadSettingsStyle method
        //  -----------------------

        public bool PadSettingsStyle { get; set; }

        //  -------
        //  Indexer
        //  -------

        internal IDetailViewSection this[nint section] => sections[(int)section];

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public DetailViewLayout()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    PadSettingsStyle = true;
                }
            }
        }

        #endregion construction

        #region methods

        //  ----------
        //  Add method
        //  ----------

        public DetailViewLayout AddSections(params IDetailViewSection[] section)
        {
            sections.AddRange(section); return this;
        }

        #endregion methods
    }

    #region IDetailViewSection interface

    //  ----------------------------
    //  IDetailViewSection interface
    //  ----------------------------

    public interface IDetailViewSection
    {
        //  -------------
        //  Rows property
        //  -------------

        IEnumerable<IDetailViewRow> Rows { get; }

        //  --------------------
        //  HeaderTitle property
        //  --------------------

        string HeaderTitle { get; set; }

        //  -------------------
        //  FooterTitle propery
        //  -------------------

        string FooterTitle { get; set; }

        //  --------------
        //  AddRows method
        //  --------------

        IDetailViewSection AddRows(params IDetailViewRow[] row);
    }

    #endregion IDetailViewSection interface

    #region DetailViewSection class

    //  -----------------------
    //  DetailViewSection class
    //  -----------------------

    public class DetailViewSection : IDetailViewSection
    {
        #region fields

        private List<IDetailViewRow> rows = new List<IDetailViewRow>();

        #endregion fields

        #region properties

        //  -------------
        //  Rows property
        //  -------------

        public IEnumerable<IDetailViewRow> Rows => rows;

        //  --------------------
        //  HeaderTitle property
        //  --------------------

        public string HeaderTitle { get; set; }

        //  -------------------
        //  FooterTitle propery
        //  -------------------

        public string FooterTitle { get; set; }

        #endregion properties

        #region methods

        //  --------------
        //  AddRows method
        //  --------------

        public IDetailViewSection AddRows(params IDetailViewRow[] row)
        {
            rows.AddRange(row); return this;
        }

        #endregion methods
    }

    #endregion DetailViewSection class

    #region IDetailViewRow interface

    //  ------------------------
    //  IDetailViewRow interface
    //  ------------------------

    public interface IDetailViewRow
    {
        //  -----------------------
        //  CellIdentifier property
        //  -----------------------

        string CellIdentifier { get; }

        //  -----------------
        //  CreateCell method
        //  -----------------

        UITableViewCell CreateCell();

        //  --------------------
        //  CustomizeCell method
        //  --------------------

        void CustomizeCell(UITableViewCell cell);
    }

    #endregion IDetailViewRow interface

    #region IDetailViewRowSelectable interface

    //  ----------------------------------
    //  IDetailViewRowSelectable interface
    //  ----------------------------------

    public interface IDetailViewRowSelectable
    {
        //  ------------------
        //  SetSelected method
        //  ------------------

        bool SetSelected();
    }

    #endregion IDetailViewRowSelectable interface

    #region DetailViewRow class

    //  -------------------
    //  DetailViewRow class
    //  -------------------

    public abstract class DetailViewRow : BindingTarget, IDetailViewRow
    {
        //  -----------------------
        //  CellIdentifier property
        //  -----------------------

        public virtual string CellIdentifier => GetType().FullName;

        //  -----------------
        //  CreateCell method
        //  -----------------

        public virtual UITableViewCell CreateCell()
        {
            return new TableViewCell(UITableViewCellStyle.Default, CellIdentifier);
        }

        //  --------------------
        //  CustomizeCell method
        //  --------------------

        public virtual void CustomizeCell(UITableViewCell cell) { }
    }

    #endregion DetailViewRow class

    #region DetailViewLayoutExtensions class

    //  --------------------------------
    //  DetailViewLayoutExtensions class
    //  --------------------------------

    public static class DetailViewLayoutExtensions
    {
        #region WithHeader method

        //  -----------------
        //  WithHeader method
        //  -----------------

        public static IDetailViewSection WithHeader(this IDetailViewSection section, string headerTitle)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            section.HeaderTitle = headerTitle; return section;
        }

        #endregion WithHeader method
    }

    #endregion DetailViewLayoutExtensions class
}

// eof "DetailViewLayout.cs"
