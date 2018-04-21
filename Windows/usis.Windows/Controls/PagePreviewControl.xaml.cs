//
//  @(#) PagePreviewControl.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace usis.Windows.Controls
{
    #region PaperSizes class

    public static class PaperSizes
    {
        public static readonly Size A4Portrait = SizeInMillimeter(210, 297);
        public static readonly Size A4Landscape = SizeInMillimeter(297, 210);
        public static readonly Size LetterPortrait = SizeInInch(8.5, 11);
        public static readonly Size LetterLandscape = SizeInInch(11, 8.5);

        #region static helper methods

        private static Size SizeInInch(double width, double height)
        {
            return new Size(InchToPixel(width), InchToPixel(height));
        }

        private static Size SizeInMillimeter(double width, double height)
        {
            return new Size(
                InchToPixel(MillimeterToInch(width)),
                InchToPixel(MillimeterToInch(height)));
        }

        private static double MillimeterToInch(double mm)
        {
            return mm / 25.4;
        }

        private static double InchToPixel(double inch)
        {
            return inch * 96;
        }

        #endregion static helper methods
    }

    #endregion PaperSizes class

    //  ------------------------
    //  PagePreviewControl class
    //  ------------------------

    internal partial class PagePreviewControl : UserControl
    {
        #region properties

        //  ------------------
        //  PaperSize property
        //  ------------------

        public static readonly DependencyProperty PaperSizeProperty =
            DependencyProperty.Register("PaperSize", typeof(Size), typeof(PagePreviewControl));

        public Size PaperSize
        {
            get
            {
                return (Size)GetValue(PaperSizeProperty);
            }
            set
            {
                SetValue(PaperSizeProperty, value);
            }
        }

        //  ------------------------
        //  PaperBorderRect property
        //  ------------------------

        public static readonly DependencyProperty PaperBorderRectProperty =
            DependencyProperty.Register("PaperBorderRect", typeof(Rect), typeof(PagePreviewControl));

        public Rect PaperBorderRect
        {
            get
            {
                return (Rect)GetValue(PaperBorderRectProperty);
            }
            set
            {
                SetValue(PaperBorderRectProperty, value);
            }
        }

        //  ---------------------
        //  PrintMargins property
        //  ---------------------

        public static readonly DependencyProperty PrintMarginsProperty =
            DependencyProperty.Register("PrintMargins", typeof(Thickness), typeof(PagePreviewControl));

        public Thickness PrintMargins
        {
            get
            {
                return (Thickness)GetValue(PrintMarginsProperty);
            }
            set
            {
                SetValue(PrintMarginsProperty, value);
            }
        }

        //  -------------------
        //  ZoomFactor property
        //  -------------------

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(PagePreviewControl));

        public double ZoomFactor
        {
            get
            {
                return (double)GetValue(ZoomFactorProperty);
            }
            set
            {
                SetValue(ZoomFactorProperty, value);
            }
        }

        //  ---------------------
        //  PrintElement property
        //  ---------------------

        public UIElement PrintElement
        {
            get
            {
                return this.PrintableArea.Children.Count == 0 ? null : this.PrintableArea.Children[0];
            }
            set
            {
                if (!UIElement.Equals(this.PrintElement, value))
                {
                    this.PrintableArea.Children.Clear();
                    if (value != null)
                    {
                        this.PrintableArea.Children.Add(value);
                    }
                }
            }
        
        } // PrintElement property

        #endregion properties

        #region construction

        //  -----------
        //  constructor
        //  -----------

        public PagePreviewControl()
        {
            this.PaperSize = PaperSizes.A4Portrait;
            this.PrintMargins = new Thickness();

            this.InitializeComponent();

        } // constructor

        #endregion construction

        #region overrides

        //  ------------------------
        //  OnPropertyChanged method
        //  ------------------------

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Equals(PaperSizeProperty))
            {
                if (this.PreviewArea != null) this.AdjustSize(this.PreviewArea.RenderSize);
            }
            base.OnPropertyChanged(e);
        
        } // OnPropertyChanged method

        #endregion overrides

        #region event handlers

        //  ------------------------------
        //  PreviewArea_SizeChanged method
        //  ------------------------------

        private void PreviewArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AdjustSize(e.NewSize);

        } // PreviewArea_SizeChanged method

        #endregion event handlers

        #region private methods

        //  -----------------
        //  AdjustSize method
        //  -----------------

        private void AdjustSize(Size previewAreaSize)
        {
            var paperDimension = Dimension(this.PaperSize);
            var winDimension = Dimension(previewAreaSize);

            bool adjustHeight = paperDimension < winDimension;

            if (adjustHeight)
            {
                this.Border.Height = previewAreaSize.Height;
                this.Border.Width = this.Border.Height * paperDimension;
            }
            else
            {
                this.Border.Width = previewAreaSize.Width;
                this.Border.Height = this.Border.Width / paperDimension;
            }

            this.ZoomFactor = adjustHeight ?
                (previewAreaSize.Height - this.BorderThickness.Top - this.BorderThickness.Bottom) / this.PaperSize.Height :
                (previewAreaSize.Width - this.BorderThickness.Left - this.BorderThickness.Right) / this.PaperSize.Width;

            this.Paper.LayoutTransform = new ScaleTransform(this.ZoomFactor, this.ZoomFactor);

            this.InvalidateMeasure();
            Dispatcher.BeginInvoke(new Action(() => this.CalculatePaperBorderRect()), DispatcherPriority.Loaded, null);

        } // AdjustSize method

        //  -------------------------------
        //  CalculatePaperBorderRect method
        //  -------------------------------

        private void CalculatePaperBorderRect()
        {
            var offset = VisualTreeHelper.GetOffset(this.Border);
            this.PaperBorderRect = new Rect(
                offset.X + 12 + this.Border.BorderThickness.Left,
                offset.Y + 12 + this.Border.BorderThickness.Top,
                this.Border.Width + 24 - this.Border.BorderThickness.Left - this.Border.BorderThickness.Right,
                this.Border.Height + 24 - this.Border.BorderThickness.Top - this.Border.BorderThickness.Bottom);

        } // CalculatePaperBorderRect method

        #endregion private methods

        #region static helper methods

        private static double Dimension(Size size)
        {
            return size.Width / size.Height;
        }

        #endregion static helper methods

    } // PagePreviewControl class

} // namespace usis.Windows.Controls

// eof "PagePreviewControl.cs"
