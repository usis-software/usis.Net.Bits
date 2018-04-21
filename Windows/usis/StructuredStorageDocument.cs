using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using usis.Platform.StructuredStorage;
using usis.Windows.Framework;

namespace usis.Windows
{
    public class StructuredStorageDocument : DocumentBase
    {
        private Storage Storage
        {
            get;
            set;
        }

        public StructuredStorageDocument(Storage storage)
        {
            if (storage == null) throw new ArgumentNullException("storage");

            this.Storage = storage;
            this.Title = this.Storage.Statistics.Name;
        }

        public static void ShowOpenStructuredStorageFileDialog()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
            }
        }
    }
}
