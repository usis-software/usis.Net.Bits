using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using usis.Windows.Framework;

namespace usis.Windows
{
	public partial class WorkspaceControl : UserControl, IWorkspace
	{
		Dictionary<TabItem, IDocument> documents = new Dictionary<TabItem,IDocument>();
		Dictionary<IDocument, TabItem> tabItems = new Dictionary<IDocument,TabItem>();

		public WorkspaceControl()
		{
			this.InitializeComponent();

			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				this.OpenDocumentCommand = new OpenDocumentCommand(this);
				this.OpenDocumentCommand.Enable();

				this.CloseDocumentCommand = new CloseDocumentCommand(this);

				App.Current.DocumentOpened += Current_DocumentOpened;
				App.Current.DocumentClosed += Current_DocumentClosed;
			}
		}

		#region event handlers

		void Current_DocumentClosed(object sender, DocumentEventArgs e)
		{
			var tabItem = this.tabItems[e.Document];
			this.TabControl.Items.Remove(tabItem);

			this.documents.Remove(tabItem);
			this.tabItems.Remove(e.Document);

			if (this.documents.Count == 0)
			{
				this.CloseDocumentCommand.Disable();
			}
		}

		void Current_DocumentOpened(object sender, DocumentEventArgs e)
		{
			var item = new TabItem();
			item.Header = e.Document.Title;
			item.Content = new DocumentControl();

			this.documents.Add(item, e.Document);
			this.tabItems.Add(e.Document, item);

			this.TabControl.Items.Add(item);
			this.TabControl.SelectedItem = item;

			if (this.documents.Count == 1)
			{
				this.CloseDocumentCommand.Enable();
			}
		}

		#endregion event handlers

		#region properties

		public WorkspaceCommand OpenDocumentCommand
		{
			get;
			private set;
		}

		public WorkspaceCommand CloseDocumentCommand
		{
			get;
			private set;
		}


		public IDocument ActiveDocument
		{
			get
			{
				var tabItem = this.TabControl.SelectedItem as TabItem;
				if (tabItem != null)
				{
					return this.documents[tabItem];
				}
				else return null;
			}
		}

		#endregion properties
	}

	public class OpenDocumentCommand : WorkspaceCommand
	{
		public OpenDocumentCommand(IWorkspace workspace) : base(workspace) {}

		public override void Execute(object parameter)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.DefaultExt = ".txt";
			dialog.Filter = "Text documents (.txt)|*.txt";
			Nullable<bool> result = dialog.ShowDialog(Application.Current.MainWindow);
			if (result.HasValue && result.Value)
			{
				Document document = new Document(dialog.FileName);
				App.Current.OpenDocument(document);
			}
		}
	}

	public class CloseDocumentCommand : WorkspaceCommand
	{
		public CloseDocumentCommand(IWorkspace workspace) : base(workspace) {}

		public override void Execute(object parameter)
		{
			App.Current.CloseDocument(this.Workspace.ActiveDocument);
		}

		public override bool CanExecute(object parameter)
		{
			return this.Workspace.ActiveDocument != null;
		}
	}

	public class Document : IDocument
	{
		private string path;
		public Document(string path)
		{
			this.path = path;
		}

		public string Title
		{
			get
			{
				return System.IO.Path.GetFileNameWithoutExtension(this.path);
			}
		}
	}
}
