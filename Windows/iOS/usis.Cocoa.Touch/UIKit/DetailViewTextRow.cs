//
//  @(#) DetailViewTextRow.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Globalization;
using UIKit;
using usis.Platform.Data;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  -----------------------
    //  DetailViewTextRow class
    //  -----------------------

    public class DetailViewTextRow : DetailViewRow, IDetailViewRowSelectable
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public DetailViewTextRow(UITableViewCellStyle style, string text)
        {
            Style = style;
            Text = text;
            Accessory = UITableViewCellAccessory.None;
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public DetailViewTextRow(UITableViewCellStyle style, string text, string detailText) : this(style, text)
        {
            DetailText = detailText;
        }

        #endregion construction

        #region properties

        //  --------------
        //  Style property
        //  --------------

        public UITableViewCellStyle Style { get; private set; }

        //  -------------
        //  Text property
        //  -------------

        public string Text { get; set; }

        //  -------------------
        //  DetailText property
        //  -------------------

        public string DetailText { get; set; }

        //  ------------------
        //  Accessory property
        //  ------------------

        public UITableViewCellAccessory Accessory { get; set; }

        //  -----------------------
        //  SelectionStyle property
        //  -----------------------

        public UITableViewCellSelectionStyle SelectionStyle { get; set; }

        #endregion properties

        #region overrides

        //  -----------------------
        //  CellIdentifier property
        //  -----------------------

        public override string CellIdentifier => string.Format(CultureInfo.InvariantCulture, "{0}.{1}", GetType().FullName, Style);

        //  -----------------
        //  CreateCell method
        //  -----------------

        public override UITableViewCell CreateCell()
        {
            return new TableViewCell(Style, CellIdentifier);
        }

        //  --------------------
        //  CustomizeCell method
        //  --------------------

        public override void CustomizeCell(UITableViewCell cell)
        {
            if (cell == null) throw new ArgumentNullException(nameof(cell));

            cell.TextLabel.Text = Text;
            if (cell.DetailTextLabel != null) cell.DetailTextLabel.Text = DetailText;

            cell.Accessory = Accessory;
            cell.SelectionStyle = SelectionStyle;
        }

        #endregion overrides

        #region events

        //  --------------
        //  Selected event
        //  --------------

        public event EventHandler<CancelEventArgs> Selected;

        #endregion events

        #region IDetailViewRowSelectable implementation

        //  ------------------
        //  SetSelected method
        //  ------------------

        public bool SetSelected()
        {
            var e = new CancelEventArgs();
            var tmp = Selected;
            tmp?.Invoke(this, e);
            return tmp != null && !e.Cancel;
        }

        #endregion IDetailViewRowSelectable implementation
    }

    #region DetailViewTextRowExtensions class

    //  ---------------------------------
    //  DetailViewTextRowExtensions class
    //  ---------------------------------

    public static class DetailViewTextRowExtensions
    {
        //  ---------------------------
        //  SetDetailTextBinding method
        //  ---------------------------

        public static DetailViewTextRow SetDetailTextBinding(this DetailViewTextRow row, object source, string propertyName)
        {
            return row.SetBinding(nameof(DetailViewTextRow.DetailText), source, propertyName);
        }
    }

    #endregion DetailViewTextRowExtensions class
}

// eof "DetailViewTextRow.cs"
