using Microsoft.Win32;
using System;
using System.Globalization;
using System.Windows;
using usis.Platform;

namespace usis.Windows
{
	public partial class MainWindow : Window
	{
		private const string registryPath = "Software\\usis\\GenericApp\\1.0\\MainWindow";

		public MainWindow()
		{
			InitializeComponent();

			this.RestoreWindowState(registryPath);
		}

		#region event handlers

		//	--------------------
		//	Window_Closed method
		//	--------------------

		private void Window_Closed(object sender, EventArgs e)
		{
			this.SaveWindowStata(registryPath);
		
		} // Window_Closed method

		#endregion event handlers

		private void RestoreWindowState(string registryKeyPath)
		{
			using (var key = Registry.CurrentUser.CreateSubKey(registryKeyPath))
			{
				this.Top = key.GetDouble("Top", this.Top);
				this.Left = key.GetDouble("Left", this.Left);
				this.Width = key.GetDouble("Width", this.Width);
				this.Height = key.GetDouble("Height", this.Height);
				WindowState state = (WindowState)Convert.ToInt32(key.GetValue("WindowState", this.WindowState), CultureInfo.InvariantCulture);
				this.WindowState = state == WindowState.Minimized ? WindowState.Normal : state;
			}
		}

		private void SaveWindowStata(string registryKeyPath)
		{
			using (var key = Registry.CurrentUser.CreateSubKey(registryKeyPath))
			{
				key.SetValue("Top", this.Top, RegistryValueKind.DWord);
				key.SetValue("Left", this.Left, RegistryValueKind.DWord);
				key.SetValue("Width", this.Width, RegistryValueKind.DWord);
				key.SetValue("Height", this.Height, RegistryValueKind.DWord);
				key.SetValue("WindowState", this.WindowState, RegistryValueKind.DWord);
			}
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
