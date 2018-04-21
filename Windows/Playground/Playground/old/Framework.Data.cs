using System;
using System.Collections.Generic;
using System.Globalization;

namespace usis.Playground.Data
{
    internal static class Program
    {
        internal static void Main()
        {
            Console.WriteLine(Microsoft.VisualBasic.DateAndTime.DatePart("ww", "2012-12-31", Microsoft.VisualBasic.FirstDayOfWeek.Monday));

            var data = Platform.Portable.HexString.ToBytes("000102030405060708090a0b0c0d0e0f");
            Console.WriteLine(Platform.Portable.HexString.FromBytes(data));
            Console.WriteLine("================================");

            var spt = new PropertyType<string>("Name");
            var et = new EntityType();
            et.AddProperty(spt);

            var entity = new Entity();

            entity.SetPropertyValue("Name", "Udo Schäfer");
            entity.SetPropertyValue("Age", 49);
            entity.SetPropertyValue("City", "Winnenden");

            Console.WriteLine("Name: {0}", entity.GetPropertyValue<string>("Name"));
            Console.WriteLine("Age:  {0}", entity.GetPropertyValue<int>("Age"));
            Console.WriteLine("City: {0}", entity.GetPropertyValue<string>("City"));

            entity.DeletePropertyValue("City");

            Console.WriteLine();
            Console.WriteLine("=== Dump ===");
            Dump(entity);

            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
        }

        internal static void Dump(IEntity entity)
        {
            foreach (var item in entity.PropertyNames)
            {
                Console.WriteLine("{0} = \'{1}\'", item, entity.GetPropertyValue(item));
            }
        }
    }

    public class PropertyType<T> : PropertyType
    {
        public PropertyType(string name)
            : base(name, typeof(T))
        {
        }
    }

    public class PropertyType
    {
        public PropertyType(string name, Type type)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            this.Name = name;

            if (type == null) throw new ArgumentNullException("type");
            this.DataType = type;
        }

        public string Name
        {
            get;
            private set;
        }

        public Type DataType
        {
            get;
            private set;
        }
    }

    public class EntityType
    {
        private Dictionary<string, PropertyType> properties;

        public void AddProperty(PropertyType propertyType)
        {
            if (propertyType == null) throw new ArgumentNullException("propertyType");
            if (this.properties == null) this.properties = new Dictionary<string,PropertyType>();
            this.properties.Add(propertyType.Name, propertyType);
        }
    }

    public interface IEntity
    {
        IEnumerable<string>PropertyNames
        {
            get;
        }

        T GetPropertyValue<T>(string propertyName);
        void SetPropertyValue<T>(string propertyName, T value);

        void DeletePropertyValue(string propertyName);

        object GetPropertyValue(string propertyName);
    }

    internal class Entity : IEntity
    {
        #region Properties property

        private Dictionary<string, IProperty> properties;

        private Dictionary<string, IProperty> Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string,IProperty>();
                }
                return this.properties;
            }
        }

        #endregion Properties property

        #region private methods

        private IProperty GetProperty(string propertyName)
        {
            IProperty property;
            if (this.Properties.TryGetValue(propertyName, out property))
            {
                return property;
            }
            string message = string.Format(
                CultureInfo.CurrentCulture,
                "Entity does not contain a property named \"{0}\"",
                propertyName);
            throw new ArgumentException(message, "propertyName");
        }

        private void SetProperty(IProperty property)
        {
            this.Properties.Add(property.Name, property);
        }

        private interface IProperty
        {
            string Name
            {
                get;
            }

            object GenericValue
            {
                get;
            }
        }

        private class Property<T> : IProperty
        {
            internal Property(string name, T value)
            {
                this.Name = name;
                this.Value = value;
            }

            public string Name
            {
                get;
                private set;
            }

            public T Value
            {
                get;
                set;
            }

            public object GenericValue
            {
                get
                {
                    return Value;
                }
            }
        }

        #endregion private methods

        #region IEntity methods

        public IEnumerable<string> PropertyNames
        {
            get
            {
                foreach (var item in this.properties.Keys)
                {
                    yield return item;
                }
            }
        }

        public T GetPropertyValue<T>(string propertyName)
        {
            var property = this.GetProperty(propertyName) as Property<T>;
            if (property != null)
            {
                return property.Value;
            }
            string message = string.Format(
                CultureInfo.CurrentCulture,
                "The property named \"{0}\" is of a diffent type.",
                propertyName);
            throw new ArgumentException(message, "propertyName");
        }
        public void SetPropertyValue<T>(string propertyName, T value)
        {
            var property = new Property<T>(propertyName, value);
            this.SetProperty(property);
        }

        public void DeletePropertyValue(string propertyName)
        {
            this.Properties.Remove(propertyName);
        }

        public object GetPropertyValue(string propertyName)
        {
            return this.GetProperty(propertyName).GenericValue;
        }

        #endregion IEntity methods
    }
}
