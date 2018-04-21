using System;
using System.Collections.Generic;
using System.Windows;
using usis.Windows.Framework;

namespace usis.Windows
{
	public class App : DocumentApplication
	{
		public static new App Current
		{
			get { return Application.Current as App; }
		}
	}
}
