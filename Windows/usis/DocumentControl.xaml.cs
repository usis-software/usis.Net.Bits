using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using usis.Windows.Framework;

namespace usis.Windows
{
	/// <summary>
	/// Interaction logic for DocumentControl.xaml
	/// </summary>
	public partial class DocumentControl : UserControl
	{
		public IDocument Document
		{
			get;
			private set;
		}

		public DocumentControl()
		{
			InitializeComponent();
		}

		public DocumentControl(IDocument document)
		{
			this.InitializeComponent();
			this.Document = document;
		}
	}
}
