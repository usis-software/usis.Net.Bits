//
//  @(#) ListViewController.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UIKit;
using usis.Framework;
using usis.Mobile;
using usis.Platform;

#pragma warning disable 1591

namespace usis.Cocoa.UIKit
{
    //  -------------------------------
    //  ListViewController<TItem> class
    //  -------------------------------

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class ListViewController<TItem> : TableViewController
    {
        #region properties

        //  --------------
        //  Items property
        //  --------------

        protected IEnumerable<TItem> Items { get; set; }

        //  ---------------------------
        //  ItemSelectedCommand command
        //  ---------------------------

        protected ICommand ItemSelectedCommand { get; set; }

        //  --------------------------
        //  ModelCollectionName method
        //  --------------------------

        private string ModelCollectionName { get; set; }

        //  --------------
        //  Model property
        //  --------------

        private AppModel Model => Context?.With<AppModel>();

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public ListViewController() { }

        public ListViewController(IEnumerable<TItem> items) { Items = items; }

        #endregion construction

        #region methods

        //  --------------------
        //  CreateSource methods
        //  --------------------

        protected virtual UITableViewSource CreateSource()
        {
            return new TableViewSource<TItem>(Items, Attributes);
        }

        #endregion methods

        #region overrides

        //  ------------------
        //  ViewDidLoad method
        //  ------------------

        public override void ViewDidLoad()
        {
            // set table view source and call base class implementation (attach source change handlers)
            if (TableView.Source == null) TableView.Source = CreateSource();
            base.ViewDidLoad();
        }

        //  --------------------
        //  SetAttributes method
        //  --------------------

        public override void SetAttributes(IValueStorage attributes)
        {
            base.SetAttributes(attributes);
            ModelCollectionName = attributes.GetString(nameof(ModelCollectionName));
        }

        //  ---------------
        //  OnInject method
        //  ---------------

        protected override void OnInject(IApplication application)
        {
            if (!string.IsNullOrWhiteSpace(ModelCollectionName))
            {
                Items = Model?.GetCollection(ModelCollectionName) as IEnumerable<TItem>;
            }
            base.OnInject(application);
        }

        //  ---------------------
        //  RefreshContent method
        //  ---------------------

        protected override void RefreshContent()
        {
            if (Items is IReloadable reloadable)
            {
                reloadable.Reload(CompleteRefresh);
            }
            else base.RefreshContent();
        }

        #endregion overrides
    }
}

// eof "ListViewController.cs"
