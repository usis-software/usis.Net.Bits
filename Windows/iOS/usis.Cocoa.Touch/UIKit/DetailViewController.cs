//
//  @(#) DetailViewController.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using CoreGraphics;
using Foundation;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using UIKit;
using usis.Framework;
using usis.Platform;
using usis.Platform.Data;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  --------------------------
    //  DetailViewController class
    //  --------------------------

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class DetailViewController : UITableViewController, IContextInjectable<IApplication>
    {
        #region fields

        private DetailViewLayout layout = new DetailViewLayout();

        #endregion fields

        #region properties

        //  ---------------
        //  Layout property
        //  ---------------

        protected DetailViewLayout Layout => layout;

        //  ----------------------------
        //  UsePadSettingsStyle property
        //  ----------------------------

        [Obsolete("Use Layout.PadSettingsStyle instead.")]
        protected virtual bool UsePadSettingsStyle => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
        
        #endregion properties

        #region overrides

        //  ---------------
        //  LoadView method
        //  ---------------

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void LoadView()
        {
            UITableView detailView = new UITableView(CGRect.Empty, UITableViewStyle.Grouped);
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                detailView.CellLayoutMarginsFollowReadableWidth = false;
            }
            detailView.AllowsSelectionDuringEditing = true;
            detailView.Source = new DetailViewTableSource(Layout);
            if (Layout.PadSettingsStyle)
            {
                View = new CanvasView(detailView);
            }
            else View = detailView;
        }

        //  ------------------
        //  ViewDidLoad method
        //  ------------------

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            foreach (var section in layout.Sections)
            {
                (section as IBindingTarget)?.Bind(CultureInfo.CurrentCulture);
                foreach (var row in section.Rows.Cast<IBindingTarget>())
                {
                    row.Bind(CultureInfo.CurrentCulture);
                }
            }
        }

        //  ---------------------
        //  ViewWillAppear method
        //  ---------------------

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (View is CanvasView canvasView)
            {
                canvasView.DetailView.DeselectRow(canvasView.DetailView.IndexPathForSelectedRow, true);
            }
        }

        //  --------------------
        //  ViewDidAppear method
        //  --------------------

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            (View as CanvasView)?.FlashScrollIndicators();
        }

        //  ------------------------
        //  ViewWillDisappear method
        //  ------------------------

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (Layout.PadSettingsStyle) View.SetNeedsLayout();
        }

        #endregion overrides

        #region IContextInjectable implementation

        //  ----------------
        //  Context property
        //  ----------------

        public IApplication Context { get; private set; }

        //  -------------
        //  Inject method
        //  -------------

        public void Inject(IApplication dependency)
        {
            if (Context != dependency)
            {
                Context = dependency;
                OnInject(dependency);
            }
        }

        //  ---------------
        //  OnInject method
        //  ---------------

        protected virtual void OnInject(IApplication application) { }

        #endregion IContextInjectable implementation

        #region DetailViewTableSource class

        //  ---------------------------
        //  DetailViewTableSource class
        //  ---------------------------

        private sealed class DetailViewTableSource : UITableViewSource
        {
            #region properties

            //  ---------------
            //  Layout property
            //  ---------------

            public DetailViewLayout Layout
            {
                get; private set;
            }

            #endregion properties

            #region construction

            //  ------------
            //  construction
            //  ------------

            public DetailViewTableSource(DetailViewLayout layout)
            {
                Layout = layout;
            }

            #endregion construction

            #region overrides

            //  --------------
            //  GetCell method
            //  --------------

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (tableView == null) throw new ArgumentNullException(nameof(tableView));

                UITableViewCell cell = null;

                var row = GetRow(indexPath);
                if (row.CellIdentifier != null) cell = tableView.DequeueReusableCell(row.CellIdentifier);
                if (cell == null) cell = row.CreateCell();
                row.CustomizeCell(cell);

                if (Layout.PadSettingsStyle)
                {
                    using (var backgroundView = new CellBackgroundView(cell.Frame))
                    {
                        backgroundView.Color = UIColor.White;
                        backgroundView.SeparatorColor = tableView.SeparatorColor;
                        cell.BackgroundView = backgroundView;
                    }
                    using (var backgroundView = new CellBackgroundView(cell.Frame))
                    {
                        backgroundView.Color = UIColor.FromWhiteAlpha(.82f, 1);
                        backgroundView.SeparatorColor = tableView.SeparatorColor;
                        cell.SelectedBackgroundView = backgroundView;
                    }
                    cell.BackgroundColor = UIColor.Clear;
                }

                return cell;
            }

            //  -----------------------
            //  NumberOfSections method
            //  -----------------------

            public override nint NumberOfSections(UITableView tableView)
            {
                return Layout.Sections.Count();
            }

            //  --------------------
            //  RowsInSection method
            //  --------------------

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return Layout[section].Rows.Count();
            }

            //  ---------------------
            //  TitleForHeader method
            //  ---------------------

            public override string TitleForHeader(UITableView tableView, nint section)
            {
                return Layout[section].HeaderTitle;
            }

            //  ------------------
            //  WillDisplay method
            //  ------------------

            public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
            {
                if (cell == null) throw new ArgumentNullException(nameof(cell));

                if (Layout.PadSettingsStyle)
                {
                    var style = GetCellBackgroundStyle(tableView, indexPath);
                    var backgroundView = cell.BackgroundView as CellBackgroundView;
                    backgroundView.Style = style;
                    backgroundView = cell.SelectedBackgroundView as CellBackgroundView;
                    backgroundView.Style = style;
                }
            }

            //  ------------------
            //  RowSelected method
            //  ------------------

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                if (tableView == null) throw new ArgumentNullException(nameof(tableView));
                if (GetRow(indexPath) is IDetailViewRowSelectable row && row.SetSelected()) return;
                tableView.DeselectRow(indexPath, true);
            }

            #endregion overrides

            #region private methods

            //  -------------
            //  GetRow method
            //  -------------

            private IDetailViewRow GetRow(NSIndexPath indexPath)
            {
                var section = Layout.Sections.ElementAtOrDefault(indexPath.Section);
                return section?.Rows.ElementAtOrDefault(indexPath.Row);
            }

            //  -----------------------------
            //  GetCellBackgroundStyle method
            //  -----------------------------

            private static CellBackgroundStyle GetCellBackgroundStyle(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Row == 0 && indexPath.Row == tableView.NumberOfRowsInSection(indexPath.Section) - 1)
                {
                    return CellBackgroundStyle.Standalone;
                }
                else if (indexPath.Row == 0)
                {
                    return CellBackgroundStyle.Top;
                }
                else if (indexPath.Row == tableView.NumberOfRowsInSection(indexPath.Section) - 1)
                {
                    return CellBackgroundStyle.Bottom;
                }
                else
                {
                    return CellBackgroundStyle.Middle;
                }
            }

            #endregion private methods
        }

        #endregion DetailViewTableSource class

        #region CanvasView class

        //  ----------------
        //  CanvasView class
        //  ----------------

        private sealed class CanvasView : UIScrollView
        {
            #region properties

            //  -------------------
            //  DetailView property
            //  -------------------

            internal UITableView DetailView => Subviews.FirstOrDefault() as UITableView;

            #endregion properties

            #region construction

            //  ------------
            //  construction
            //  ------------

            public CanvasView(UITableView detailView) : base(detailView.Frame)
            {
                detailView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                AddSubview(detailView);

                BackgroundColor = detailView.BackgroundColor;
                ShowsVerticalScrollIndicator = true;
                IndicatorStyle = detailView.IndicatorStyle;
                AlwaysBounceVertical = true;
                detailView.ShowsVerticalScrollIndicator = false;
                detailView.ScrollEnabled = false;
            }

            #endregion construction

            #region overrides

            //  ---------------------
            //  LayoutSubviews method
            //  ---------------------

            public override void LayoutSubviews()
            {
                nfloat boarder = 28;
                CGRect frame = new CGRect(Bounds.X + boarder, Bounds.Y, Bounds.Width - boarder * 2, Bounds.Height);
                DetailView.Frame = frame;
                DetailView.LayoutIfNeeded();
                ContentSize = new CGSize(Bounds.Size.Width, DetailView.ContentSize.Height);
                DetailView.ContentOffset = ContentOffset;
            }

            #endregion overrides
        }

        #endregion CanvasView class

        #region CellBackgroundStyle enumeration

        //  -------------------------------
        //  CellBackgroundStyle enumeration
        //  -------------------------------

        private enum CellBackgroundStyle
        {
            Standalone,
            Top,
            Middle,
            Bottom
        }

        #endregion CellBackgroundStyle enumeration

        #region CellBackgroundView

        //  -------------------------
        //  CellBackgroundView class
        //  -------------------------

        private sealed class CellBackgroundView : UIView
        {
            #region construction

            //  ------------
            //  construction
            //  ------------

            public CellBackgroundView(CGRect frame) : base(frame)
            {
                BackgroundColor = UIColor.Clear;
            }

            #endregion construction

            #region properties

            //  --------------
            //  Color property
            //  --------------

            public UIColor Color { get; set; }

            //  -----------------------
            //  SeparatorColor property
            //  -----------------------

            public UIColor SeparatorColor { get; set; }

            //  --------------
            //  Style property
            //  --------------

            public CellBackgroundStyle Style { get; set; }

            #endregion properties

            #region overrides

            //  -----------
            //  Draw method
            //  -----------

            public override void Draw(CGRect rect)
            {
                var context = UIGraphics.GetCurrentContext();

                nfloat cornerRadius = 4;
                CGRect bounds = Bounds;

                using (CGPath path = new CGPath())
                {
                    switch (Style)
                    {
                        case CellBackgroundStyle.Top:
                            path.MoveToPoint(bounds.GetMinX(), bounds.GetMaxY());
                            path.AddArcToPoint(bounds.GetMinX(), bounds.GetMinY(), bounds.GetMidX(), bounds.GetMinY(), cornerRadius);
                            path.AddArcToPoint(bounds.GetMaxX(), bounds.GetMinY(), bounds.GetMaxX(), bounds.GetMidY(), cornerRadius);
                            path.AddLineToPoint(bounds.GetMaxX(), bounds.GetMaxY());
                            break;
                        case CellBackgroundStyle.Middle:
                            path.AddRect(bounds);
                            break;
                        case CellBackgroundStyle.Bottom:
                            path.MoveToPoint(bounds.GetMinX(), bounds.GetMinY());
                            path.AddArcToPoint(bounds.GetMinX(), bounds.GetMaxY(), bounds.GetMidX(), bounds.GetMaxY(), cornerRadius);
                            path.AddArcToPoint(bounds.GetMaxX(), bounds.GetMaxY(), bounds.GetMaxX(), bounds.GetMidY(), cornerRadius);
                            path.AddLineToPoint(bounds.GetMaxX(), bounds.GetMinY());
                            break;
                        case CellBackgroundStyle.Standalone:
                        default:
                            path.AddRoundedRect(bounds, cornerRadius, cornerRadius);
                            break;
                    }
                    Color.SetFill();
                    context.AddPath(path);
                    context.FillPath();
                }
                if (Style == CellBackgroundStyle.Top || Style == CellBackgroundStyle.Middle)
                {
                    SeparatorColor.SetFill();
                    nfloat lineHeight = 1 / UIScreen.MainScreen.Scale;
                    context.FillRect(new CGRect(bounds.GetMinX() + 15, bounds.Size.Height - lineHeight, bounds.Size.Width - 15, lineHeight));
                }
            }

            #endregion overrides
        }

        #endregion CellBackgroundView
    }
}

// eof "DetailViewController.cs"
