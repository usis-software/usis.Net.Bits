//
//	@(#) ModelBase.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 audius GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.Windows.Framework
{
    //  ---------------
    //  ModelBase class
    //  ---------------

    public class ModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged implementation

        #region methods

        //  ------------------------
        //  OnPropertyChanged method
        //  ------------------------

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        } // OnPropertyChanged method

        #endregion methods

    } // ModelBase class

} // namespace usis.Windows.Framework

// eof "ModelBase.cs"
