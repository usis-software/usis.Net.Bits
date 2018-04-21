using System.Windows.Forms;

namespace usis.Windows.Forms
{
    public partial class ModalDialog : Form
    {
        #region construction

        public ModalDialog()
        {
            InitializeComponent();
        }

        #endregion construction

        #region DialogControl property

        //  ----------------------
        //  DialogControl property
        //  ----------------------

        private Control dialogControl;

        public Control DialogControl
        {
            get
            {
                return dialogControl;
            }
            set
            {
                if (dialogControl != value)
                {
                    tableLayoutPanel.Controls.Remove(dialogControl);
                    if (value != null)
                    {
                        value.Dock = DockStyle.Fill;
                        tableLayoutPanel.Controls.Add(value, 0, 0);
                        value.Select();
                    }
                }
                dialogControl = value;
            }
        }

        #endregion DialogControl property
    }
}
