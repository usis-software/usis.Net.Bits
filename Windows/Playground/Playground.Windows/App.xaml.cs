using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Playground.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //ShowExceptionDialog(e.Exception);
            e.Handled = true;
        }

        //private bool? ShowExceptionDialog(Exception exception)
        //{
        //    var dialog = new usis.Windows.ModalDialog()
        //    {
        //        Owner = MainWindow,
        //        Title = "Ausnahmefehler",
        //        CloseButtonOnly = true,
        //        DialogControl = new usis.Windows.Controls.ExceptionDialogControl(exception)
        //    };
        //    return dialog.ShowDialog();
        //}
    }
}
