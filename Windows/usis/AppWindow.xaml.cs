using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using usis.Windows.Controls;

namespace usis.Windows
{
    public partial class AppWindow : Window
    {
        private PrintPreviewControl preview = new PrintPreviewControl();

        public AppWindow()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                //var preview = new PrintPreviewControl();
                //preview.pa
                //preview.PaperSize = PaperSizes.A4Landscape;
                //this.preview.PaperSize = PaperSizes.A4Landscape;
                this.preview.PrintMargins = new Thickness(24);

                this.preview.Width = double.NaN;
                this.preview.Height = double.NaN;

                this.Content = preview;

                var content = new DevExpress.Xpf.PropertyGrid.PropertyGridControl()
                {
                    SelectedObject = this,
                    Width = 500,
                    Height = 1000
                };
                this.preview.AddPrintElement(content);

                this.RestoreWindowState();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.SaveWindowState();
        }

        private void Window_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.preview.PaperSize = this.preview.PaperSize == PaperSizes.A4Portrait ?
                PaperSizes.A4Landscape : PaperSizes.A4Portrait;

            //this.preview.PrintMargins = new Thickness(64);

            //var content = new DevExpress.Xpf.PropertyGrid.PropertyGridControl()
            //{
            //    SelectedObject = this,
            //    Width = 300, //PrintableArea.Width,
            //    Height = 300 //PrintableArea.Height
            //};
            //this.preview.AddPrintElement(content);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => this.LoadCompleted()), DispatcherPriority.ContextIdle, null);
        }

        //  --------------------
        //  LoadCompleted method
        //  --------------------

        private void LoadCompleted()
        {
            var control = new PrintPreviewControl();
            control.PrintMargins = new Thickness(100);
            control.AddPrintElement(new UserControl1());

            new ModalDialog()
            {
                Owner = this,
                Title = "Print Preview Dialog",
                DialogControl = control,
                SettingsName = "PrintPreviewDialog"

            }.ShowDialog();

        } // LoadCompleted method

    }
}
