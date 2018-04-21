//
//  @(#) PrintMarginsControl.cs
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
    //  ------------------------
    //  PrintMarginsControl class
    //  ------------------------

    internal partial class PrintMarginsControl : UserControl
    {
        #region properties

        //  ---------------------
        //  PrintMargins property
        //  ---------------------

        public static readonly DependencyProperty PrintMarginsProperty =
            DependencyProperty.Register("PrintMargins", typeof(Thickness), typeof(PrintMarginsControl));

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
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(PrintMarginsControl));

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

        #endregion properties

        #region construction

        public PrintMarginsControl()
        {
            InitializeComponent();

            this.PrintMargins = new Thickness();
            this.ZoomFactor = 1;
        }

        #endregion construction

        #region overrides

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Equals(PrintMarginsProperty) ||
                e.Property.Equals(ZoomFactorProperty))
            {
                this.UpdateSplitter();
            }
        }

        #endregion overrides

        #region private methods

        private void UpdateMargins()
        {
            var factor = this.ZoomFactor;

            this.PrintMargins = new Thickness(
                (this.leftMargin.Width.Value - 12) / factor,
                (this.topMargin.Height.Value - 12) / factor,
                (this.rightMargin.Width.Value - 12) / factor,
                (this.bottomMargin.Height.Value - 12) / factor);
        }

        private void UpdateSplitter()
        {
            var margins = this.PrintMargins;
            var factor = this.ZoomFactor;

            this.leftMargin.Width = new GridLength(12 + margins.Left * factor);
            this.topMargin.Height = new GridLength(12 + margins.Top * factor);
            this.rightMargin.Width = new GridLength(12 + margins.Right * factor);
            this.bottomMargin.Height = new GridLength(12 + margins.Bottom * factor);
        }

        #endregion private methods

        #region event handlers

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateMargins();
        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.topMargin.MaxHeight = e.NewSize.Height - this.bottomMargin.Height.Value;
            this.leftMargin.MaxWidth = e.NewSize.Width - this.rightMargin.Width.Value;
        }

        #endregion event handlers

    } // PrintMarginsControl class

} // namespace usis.Windows.Controls

// eof "PrintMarginsControl.cs"
