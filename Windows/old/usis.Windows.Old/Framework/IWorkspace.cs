using System;
using System.Windows.Input;

namespace usis.Windows.Framework
{
	public interface IWorkspace
	{
		IDocument ActiveDocument { get; }
	}
	
	public abstract class WorkspaceCommand : ICommand
	{
		private bool canExecute = false;

		public IWorkspace Workspace
		{
			get;
			private set;
		}

		protected WorkspaceCommand(IWorkspace workspace)
		{
			this.Workspace = workspace;
		}

		public virtual bool CanExecute(object parameter)
		{
			return this.canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public virtual void Execute(object parameter)
		{
		}

		private void SetEnabled(bool enabled)
		{
			if (this.canExecute != enabled)
			{
				this.canExecute = enabled;
				if (this.CanExecuteChanged != null)
				{
					this.CanExecuteChanged(this, EventArgs.Empty);
				}
			}
		}

		public void Enable()
		{
			this.SetEnabled(true);
		}

		public void Disable()
		{
			this.SetEnabled(false);
		}
	}
}
