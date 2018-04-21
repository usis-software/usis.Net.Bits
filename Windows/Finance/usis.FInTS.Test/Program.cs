using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

namespace usis.FinTS.Test
{
    internal static class Program
    {
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.Write(System.String)")]
        internal static void Main()
        {
            //System.Threading.Thread.Sleep(3000);
            Console.Write("Press any key to send ... ");
            Console.ReadKey(true);

            var bankAccess = new BankAccess()
            {
                CountryCode = 280,
                BankCode = "12345678",
                HbciVersion = 300
            };
            var customerSystem = new CustomerSystem()
            {
                BankParameterDataVersion = 0,
                UserParameterDataVersion = 0,
                DialogLanguage = "0",
                ProductDescription = "usis FinTS Library",
                ProductVersion = "1.0"
            };

            using (var client = new TcpClient("localhost", 3000))
            {
                using (var transport = TcpTransport.CreateCustomerTransport(client))
                {
                    using (var dialog = CustomerDialog.Initialize(transport, bankAccess, customerSystem))
                    {
                        dialog.Terminate();
                        Console.WriteLine(dialog);
                    }
                }
            }
            Console.ReadKey(true);
        }
    }
}
