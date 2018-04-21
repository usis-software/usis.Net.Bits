using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using usis.Platform.Data;

namespace usis.Data.Registry
{
    public partial class DataSourceControl : UserControl
    {
        #region construction

        public DataSourceControl()
        {
            InitializeComponent();

            comboBoxDataProvider.DataSource = DbProviderFactories.GetFactoryClasses();
            comboBoxDataProvider.SelectedValue = "System.Data.SqlClient";
        }

        #endregion construction

        #region properties

        private DataRow SelectedRow
        {
            get
            {
                return (comboBoxDataProvider.SelectedItem as DataRowView).Row;
            }
        }

        public string ConnectionString
        {
            get
            {
                return textBoxConnectionString.Text;
            }
            set
            {
                textBoxConnectionString.Text = value;
            }
        }

        #endregion properties

        #region public methods

        public DataSource CreateDataSource()
        {
            return new DataSource(SelectedRow[2] as string)
            {
                ConnectionString = ConnectionString
            };
        }

        #endregion public methods
    }
}
