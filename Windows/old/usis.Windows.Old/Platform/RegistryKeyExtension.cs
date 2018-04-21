using Microsoft.Win32;
using System;
using System.Globalization;

namespace usis.Platform
{
	public static class RegistryKeyExtension
	{
		public static double GetDouble(this RegistryKey key, string name, double defaultValue)
		{
			if (key == null) throw new ArgumentNullException("key");
			return Convert.ToDouble(key.GetValue(name, defaultValue), CultureInfo.InvariantCulture);
		}
	}
}
