using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using usis.Platform.VB6;

namespace Playground
{
	internal static class PropertyBagPlayground
	{
		static void Main()
		{
			var writeBag = new PropertyBag();
			writeBag.WriteString("Property1", "Value1", null);
			writeBag.Save("C:\\tmp\\properties.dat");

			//bag.Load("C:\\tmp\\properties.dat");
			//object o = bag.ReadVariant("Property2", null);
			//Console.WriteLine(o);

			var ReadBag = PropertyBag.Load("C:\\tmp\\properties.dat");
			string s = ReadBag.ReadString("Property1", null);
			
			Console.WriteLine(s);

			Console.ReadKey(true);
		}
	}
}
