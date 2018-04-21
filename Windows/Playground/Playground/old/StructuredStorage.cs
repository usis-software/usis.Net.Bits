using System;
using System.Globalization;
using System.IO;
using usis.Platform.StructuredStorage;

namespace Playground
{
	internal static class StructuredStorage
	{
		public static void Main()
		{
            //string fileName = System.Environment.ExpandEnvironmentVariables("%TEMP%");
            //fileName = Path.Combine(fileName, "RFC.dat");
			//fileName = Path.Combine(fileName, "test.dat");
            string fileName = "Z:\\tmp\\TRO007_11.dat";
            //string fileName = "Z:\\tmp\\TRO007_11 - Copy (2).dat";
            //string fileName = "C:\\tmp\\TRO007_11.dat";
            //string fileName = "C:\\tmp\\SERVER_2009.03.02  01.42.04.267__dods Data Transmission from AUDIUSID (207)__{F231209F-A7DA-4496-8D9C-A78510B2BE4C}.dlt";


			//string name = Guid.NewGuid().ToString();
			//Console.WriteLine(name);
			//string[] names = name.Split(new char[] { '-' });
			//foreach (var s in names)
			//{
			//	Console.WriteLine(s);
			//}

			//Test(fileName);
			//TargetFieldsStorageTest(fileName);

			if (File.Exists(fileName))
			{
				if (Storage.IsStorageFile(fileName))
				{
					//TargetFieldsStorageTest(fileName);
					Console.WriteLine();
					Console.WriteLine("Dump:");
					Dump(fileName);
				}
				else Console.WriteLine("No structured storage file!");
			}
			else Console.WriteLine("File does not exist!");

			Console.WriteLine();
			Console.Write("Press any key to continue ... ");
			Console.ReadKey(true);
		}

		private static void Dump(string fileName)
		{
			using (var storage = Storage.OpenStorageFile(fileName, StorageModes.ShareExclusive))
			{
                foreach (var item in Storage.WalkStorages(storage))
                {
                    Console.WriteLine(item.Path);
                }
                Console.WriteLine("=== streams ===");
                foreach (var item in Storage.WalkStorageStreams(storage))
                {
                    Console.Write("Stream: ");
                    Console.Write(item.Statistics.Name);
                    Console.Write(" size: ");
                    Console.WriteLine(item.Statistics.Size);

                    byte[] data = new byte[item.Statistics.Size];
                    item.Read(data, 0, (int)item.Statistics.Size);
                    Console.WriteLine(BitConverter.ToString(data));
                }
			}
		}

        private static void Test(string fileName)
        {
            //using (var storage = Storage.CreateCompoundFile(fileName, StorageModes.Create | StorageModes.ShareExclusive | StorageModes.ReadWrite))
            using (var storage = Storage.CreateStorageFile(fileName, StorageModes.Create | StorageModes.ShareExclusive | StorageModes.ReadWrite))
            {
                //string name = Guid.NewGuid().ToString();
                //string name = "1234567890-1234567890-1234567890";
                Guid id = Guid.NewGuid();
                byte[] bytes = id.ToByteArray();
                string name = Convert.ToBase64String(bytes);
                using (var subStorage = storage.CreateStorage(name, StorageModes.ShareExclusive | StorageModes.ReadWrite))
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "string '{0}' - {1} length", name, name.Length));
                }
            }
        }
    }

	public sealed class StorageItem
	{
		public Storage Storage
		{
			get;
			private set;
		}

		internal StorageItem(Storage storage)
		{
			this.Storage = storage;
		}
	}

	public static class GuidExtension
	{
		public static string ToBase64String(this Guid value)
		{
			return Convert.ToBase64String(value.ToByteArray());
		}
	}
}
