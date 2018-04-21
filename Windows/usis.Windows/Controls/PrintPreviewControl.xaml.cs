//
//  @(#) PrintPreviewControl.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System.Windows;
using System.Windows.Controls;

namespace usis.Windows.Controls
{
    //  -------------------------
    //  PrintPreviewControl class
    //  -------------------------

    public partial class PrintPreviewControl : UserControl
    {
        #region construction

        public PrintPreviewControl()
        {
            InitializeComponent();
        }

        #endregion construction

        public Thickness PrintMargins
        {
            get
            {
                return this.pagePreview.PrintMargins;
            }
            set
            {
                this.pagePreview.PrintMargins = value;
            }
        }

        public Size PaperSize
        {
            get
            {
                return this.pagePreview.PaperSize;
            }
            set
            {
                this.pagePreview.PaperSize = value;
            }
        }

        public void AddPrintElement(UIElement element)
        {
            this.pagePreview.PrintElement = element;
        }

    } // PrintPreviewControl class

} // namespace usis.Windows.Controls

// eof "PrintPreviewControl.cs"
