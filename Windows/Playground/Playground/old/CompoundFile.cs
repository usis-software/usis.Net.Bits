using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using usis.Platform.StructuredStorage;

namespace Playground
{
    internal static class CompoundFile
    {
        internal static void Main()
        {
            //using (var storage = Storage.OpenCompoundFile("Z:\\tmp\\agee64.msi", StorageModes.ShareExclusive))
            using (var storage = Storage.CreateCompoundFile(null, StorageModes.ReadWrite | StorageModes.ShareExclusive))
            {
                using (var sub = storage.CreateStorage("test", storage.Mode))
                {
                    sub.SetElementTimes(null,
                        storage.Statistics.Created,
                        DateTime.Now.AddDays(-14),//storage.Statistics.Modified,
                        storage.Statistics.Accessed);
                }
                //storage.Commit();

                using (var sub = storage.OpenStorage("test", storage.Mode))
                {
                    Console.WriteLine("Created : {0}", sub.Statistics.Created);
                    Console.WriteLine("Modified: {0}", sub.Statistics.Modified);
                    Console.WriteLine("Accessed: {0}", sub.Statistics.Accessed);
                }
            }
            PressAnyKey();
        }
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
