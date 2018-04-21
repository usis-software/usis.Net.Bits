using DevExpress.Xpf.Docking;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using usis.Platform;
using usis.Windows.Framework;
using usis.Windows;

namespace usis.Windows
{
	public class MyWorkspaceViewModel : DocumentWorkspaceViewModel
	{
		#region construction

		public MyWorkspaceViewModel()
		{
            this.SaveAsCommand = new Command();
            this.SaveAsCommand.Executed += SaveAsCommand_Executed;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SaveAsCommand_Executed(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            var result = dialog.ShowDialog(Application.Current.MainWindow);
            if (result.HasValue && result.Value)
            {
                try
                {
                    Debug.Print(dialog.FileName);
                    throw new NotImplementedException();
                }
                catch (Exception exception)
                {
                    Application.Current.ShowErrorDialog(exception);
                }
            }
        }

		#endregion construction

		#region overrides

		protected override IDocumentViewModel CreateDocumentViewModel(IDocument document)
		{
			return new MyDocumentViewModel(document);
		}

		protected override void OnOpenDocumentCommand()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.DefaultExt = ".txt";
			dialog.Filter = "Text documents (.txt)|*.txt";
			Nullable<bool> result = dialog.ShowDialog(Application.Current.MainWindow);
			if (result.HasValue && result.Value)
			{
				MyDocument document = new MyDocument(dialog.FileName);
				this.Workspace.OpenDocument(document);
			}
		}

        #endregion overrides

        public Command SaveAsCommand
        {
            get;
            private set;
        }

	} // MyWorkspaceViewModel

	public class MyDocumentViewModel : usis.DX.Xpf.DocumentViewModel
	{
        public DocumentControl View
        {
            get;
            private set;
        }

        internal MyDocumentViewModel(IDocument document)
            : base(document)
        {
            MVVMHelper.SetTargetName(this, "PanelHost");

            this.View = new DocumentControl(document);

            this.CloseCommand = new Command();
            this.CloseCommand.Executed += CloseCommand_Executed;

            this.SaveAsCommand = new Command();
            this.SaveAsCommand.Executed += SaveAsCommand_Executed;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SaveAsCommand_Executed(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            var result = dialog.ShowDialog(Application.Current.MainWindow);
            if (result.HasValue && result.Value)
            {
                try
                {
                    Debug.Print(dialog.FileName);
                    throw new NotImplementedException();
                }
                catch (Exception exception)
                {
                    Application.Current.ShowErrorDialog(exception);
                }
            }
        }

        void CloseCommand_Executed(object sender, EventArgs e)
        {
            DocumentWorkspace.Current.CloseDocument(this.Document);
        }

        public Command CloseCommand
        {
            get;
            private set;
        }

        public Command SaveAsCommand
        {
            get;
            private set;
        }
    }

} // usis namespace
