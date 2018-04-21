using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using Test.Interfaces;
using usis.Platform.ServiceModel;

namespace Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://stackoverflow.com/questions/777607/the-remote-certificate-is-invalid-according-to-the-validation-procedure-using
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => { return true; };

            //var serviceUrl = new Uri("https://localhost:2303/Test/");
            var serviceUrl = new Uri("https://localhost/Test/");
            using (var client = new ServiceClient<ITest>(CreateEndpoint(typeof(ITest), serviceUrl)))
            {
                Console.WriteLine(client.Service.Hello());
                Console.ReadKey(true);
            }
        }

        static ServiceEndpoint CreateEndpoint(Type channelType, Uri url)
        {
            return new ServiceEndpoint(
                ContractDescription.GetContract(channelType),
                //new BasicHttpBinding(),
                //new WSHttpBinding(),
                new BasicHttpsBinding(),
                new EndpointAddress(url));
        }
    }
}
