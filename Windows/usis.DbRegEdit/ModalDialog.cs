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

        private Control content;

        public Control Content
        {
            get
            {
                return content;
            }
            set
            {
                if (content != value)
                {
                    tableLayoutPanel.Controls.Remove(content);
                    if (value != null)
                    {
                        value.Dock = DockStyle.Fill;
                        tableLayoutPanel.Controls.Add(value, 0, 0);
                        value.Select();
                    }
                }
                content = value;
            }
        }

        #endregion DialogControl property

        public TControl GetControl<TControl>() where TControl : class
        {
            return Content as TControl;
        }
    }
}
