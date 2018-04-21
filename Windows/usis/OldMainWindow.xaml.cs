//
//	@(#) OldMainWindow.xaml.cs
//
//	Project:	usis Platform
//	System:		Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking.Base;
using System;
using System.ComponentModel;
using System.Windows;
using usis.Windows.Framework;

namespace usis.Windows
{
	//	----------------
	//	OldMainWindow class
	//	----------------

	public partial class OldMainWindow : Window
	{
		#region constants

        //private const string registryPath = "Software\\usis\\GenericApp\\1.0\\OldMainWindow";

		#endregion constants

		#region construction

		//	-----------
		//	constructor
		//	-----------

		public OldMainWindow()
		{
			InitializeComponent();

			if (!DesignerProperties.GetIsInDesignMode(this))
			{
                this.savedTitle = this.Title;
				this.RestoreWindowState(Application.Current.UseSettingsUserRegistryKey("OldMainWindow"));
			}
		
		} // constructor

		#endregion construction

		#region overrides

		//	---------------
		//	OnClosed method
		//	---------------

		protected override void OnClosed(EventArgs e)
		{
			this.SaveWindowState(Application.Current.UseSettingsUserRegistryKey("OldMainWindow"));

			base.OnClosed(e);

		} // OnClosed method

		#endregion overrides

        private string savedTitle;

		private void DockLayoutManager_DockItemClosed(object sender, DockItemClosedEventArgs e)
		{
			var document = e.Item.DataContext as IDocumentViewModel;
			var workspace = this.DataContext as DocumentWorkspaceViewModel;
			if (document != null && workspace != null)
			{
				workspace.Workspace.CloseDocument(document.Document);
                if (workspace.Documents.Count == 0)
                {
                    this.Title = savedTitle;
                }
			}
		}

		private void DockLayoutManager_MDIItemActivated(object sender, MDIItemActivatedEventArgs ea)
		{
			var document = ea.Item != null ? ea.Item.DataContext as IDocumentViewModel : null;
			var workspace = this.DataContext as DocumentWorkspaceViewModel;
			if (workspace != null)
			{
				workspace.ActiveDocument = document;
			}
		}

        private void DockLayoutManager_Merge(object sender, DevExpress.Xpf.Docking.BarMergeEventArgs e)
        {
            Bar childBar = e.ChildBarManager.Bars["toolBar"];
            Bar parentBar = e.BarManager.Bars["toolBar"];

            if (parentBar != null)
            {
                //childBar.Visible = true;
                parentBar.Visible = true;
                parentBar.Merge(childBar);
                //childBar.Merge(parentBar);
            }
        }

        private void DockLayoutManager_UnMerge(object sender, DevExpress.Xpf.Docking.BarMergeEventArgs e)
        {
            Bar parentBar = e.BarManager.Bars["toolBar"];
            if (parentBar != null)
            {
                parentBar.UnMerge();
                parentBar.Visible = false;
            }
        }

	} // OldMainWindow class

} // usis namespace

// eof "OldMainWindow.xaml.cs"
