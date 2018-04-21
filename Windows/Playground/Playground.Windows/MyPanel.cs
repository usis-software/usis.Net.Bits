using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Playground.Windows
{
    public class MyPanel : Panel
    {
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register("MyPropertyDefaults", typeof(ICollection<string>), typeof(MyPanel), new PropertyMetadata(new List<string>()));

        public ICollection<string> MyProperty
        {
            get
            {
                return GetValue(MyPropertyProperty) as ICollection<string>;
            }

            private set
            {
                this.SetValue(MyPropertyProperty, value);
            }
        }
    }
}
