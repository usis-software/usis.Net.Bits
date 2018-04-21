using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;

namespace usis.FieldService.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;
                using (var client = new Platform.ServiceModel.ServiceClient<IServer>(/*CreateEndpoint()*/))
                {
                    MessageBox.Show(this, client.Service.CreateSession());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        //private static ServiceEndpoint CreateEndpoint()
        //{
        //    return CreateEndpoint(typeof(IUserManagement), new Uri("http://localhost/FieldService"));
        //}

        //private static ServiceEndpoint CreateEndpoint(Type channelType, Uri url)
        //{
        //    return new ServiceEndpoint(
        //        ContractDescription.GetContract(channelType),
        //        new BasicHttpBinding(),
        //        //new WSHttpBinding(),
        //        //new BasicHttpsBinding(),
        //        new EndpointAddress(url));
        //}
    }
}
