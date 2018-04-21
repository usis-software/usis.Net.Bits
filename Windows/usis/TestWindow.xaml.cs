using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace usis.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        Dictionary<FrameworkElement, Rect> controls = new Dictionary<FrameworkElement,Rect>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in container.Children)
            {
                var element = item as FrameworkElement;
                if (element == null) continue;

                var rect = new Rect(
                    Canvas.GetLeft(element),
                    Canvas.GetTop(element),
                    element.RenderSize.Width,
                    element.RenderSize.Height);
                    //element.Width,
                    //element.Height);
                
                this.controls.Add(element, rect);

                Debug.Print("{0}: {1}", item.GetType().FullName, rect);
            }
        }
    }
}
