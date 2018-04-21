using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
	internal class NamedValueTest
	{
		public static void Main()
		{
			var container = new NamedValueContainer();
			container.SetValue("Antwort", false);
			container.SetValue("Antwort", true);
			container.SetValue("Antwort", 42);
			container.SetValue("Hello", "World");
		}
	}

	interface INamedValueMetadata
	{
		string Name
		{
			get;
		}

		Type DataType
		{
			get;
		}
	}

	class NamedValueMetadata<T> : INamedValueMetadata
	{
		public NamedValueMetadata(string name)
		{
			this.Name = name;
		}

		public string Name
		{
			get;
			set;
		}

		public Type DataType
		{
			get
			{
				return typeof(T);
			}
		}
	}

	interface INamedValueItem
	{
		INamedValueMetadata Metadata
		{
			get;
		}
	}

	interface INamedValueItem<T> : INamedValueItem
	{
		T Value
		{
			get;
			set;
		}
	}

	class NamedValueItem<T> : INamedValueItem<T> // where T : struct
	{
		private INamedValueMetadata metadata;

		public T Value
		{
			get;
			set;
		}

		public NamedValueItem(string name, T value)
		{
			this.metadata = new NamedValueMetadata<T>(name);
			this.Value = value;
		}

		public INamedValueMetadata Metadata
		{
			get
			{
				return this.metadata;
			}
		}
	}

	class NamedValueContainer
	{
		private Dictionary<string, INamedValueItem> items;

		public Dictionary<string, INamedValueItem> Items
		{
			get
			{
				if (items == null)
				{
					items = new Dictionary<string, INamedValueItem>();
				}
				return items;
			}
		}

		public void SetValue<T>(string name, T value)
		{
			INamedValueItem item = null;
			if (this.Items.TryGetValue(name, out item))
			{
				if (item.Metadata.DataType == typeof(T))
				{
					var exisitingItem = item as INamedValueItem<T>;
					if (exisitingItem != null)
					{
						exisitingItem.Value = value;
						return;
					}
				}
				this.items.Remove(name);
			}
			this.items.Add(name, new NamedValueItem<T>(name, value));
		}
	}
}

