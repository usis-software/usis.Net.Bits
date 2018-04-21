using VB6Wrapper;

namespace usis.Platform.VB6
{
	public class PropertyBag
    {
		private VB6PropertyBag wrapper;

		public PropertyBag()
		{
			this.wrapper = new VB6PropertyBag();
		}

		public static PropertyBag Load(string fileName)
		{
			var propertyBag = new PropertyBag();
			propertyBag.wrapper.Load(fileName);
			return propertyBag;
		}

		public string ReadString(string name, string defaultValue)
		{
			return this.wrapper.ReadVariant(name, defaultValue) as string;
		}

		public void WriteString(string name, string value, string defaultValue)
		{
			this.wrapper.WriteVariant(name, value, defaultValue);
		}

		public void Save(string fileName)
		{
			this.wrapper.Save(fileName);
		}
    }
}
