using System.Windows.Controls;

namespace usis.Windows
{
    public partial class WorkspaceControl : UserControl
    {
        public WorkspaceControl()
        {
            InitializeComponent();

            this.testControl.Focus();
        }
    }
}
