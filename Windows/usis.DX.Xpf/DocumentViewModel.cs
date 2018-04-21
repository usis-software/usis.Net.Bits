//
//  @(#) DocumentViewModel.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using DevExpress.Xpf.Docking;
using System.Windows;
using usis.Platform;
using usis.Windows;
using usis.Windows.Framework;

namespace usis.DX.Xpf
{
    //  -----------------------
    //  DocumentViewModel class
    //  -----------------------

    public class DocumentViewModel : DependencyObject, IDocumentViewModel
    {
        #region fields

        private const string stateSettingName = "MDIState";

        #endregion fields

        #region contruction

        //  -----------
        //  contruction
        //  -----------

        protected DocumentViewModel(IDocument document)
            : this(document, MDIState.Maximized)
        {
        } // constructor

        protected DocumentViewModel(IDocument document, MDIState state)
        {
            this.Document = document;
            using (var registryKey = Application.Current.UseSettingsUserRegistryKey())
            {
                this.MDIState = registryKey.GetEnum(stateSettingName, state);
            }

        } // constructor

        #endregion contruction

        #region properties

        //  -----------------
        //  Document property
        //  -----------------

        public IDocument Document
        {
            get;
            private set;

        } // Document property

        //  -----------------
        //  MDIState property
        //  -----------------

        private MDIState state = MDIState.Maximized;

        public MDIState MDIState
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
                using (var registryKey = Application.Current.UseSettingsUserRegistryKey())
                {
                    registryKey.SetValue(stateSettingName, this.state);
                }
            }

        } // MDIState property

        //  --------------------
        //  MDILocation property
        //  --------------------

        private static Point nextLocation;
        private Point? location;

        public Point MDILocation
        {
            get
            {
                if (!this.location.HasValue)
                {
                    this.location = nextLocation;
                    nextLocation = new Point(this.location.Value.X + 24, this.location.Value.Y + 24);
                }
                return this.location.Value;
            }

        } // MDILocation property

        #endregion properties

    } // DocumentViewModel class

} // namespace usis.DX.Xpf

// eof "DocumentViewModel.cs"
