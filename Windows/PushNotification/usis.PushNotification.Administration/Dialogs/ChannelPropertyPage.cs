//
//  @(#) ChannelPropertyPage.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using usis.Framework.ManagementConsole;
using usis.ManagementConsole;

namespace usis.PushNotification.Administration
{
    //  -------------------------
    //  ChannelPropertyPage class
    //  -------------------------

    internal sealed class ChannelPropertyPage<TControl, TChannelInfo> : PropertyPage<SnapIn>
        where TControl : Control, new()
        where TChannelInfo : IChannelInfo
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ChannelPropertyPage()
        {
            Title = Strings.Channel;
            Control = new TControl();

            if (Control is IPropertyPageControl control) control.PropertyPage = this;
        }

        #endregion construction

        #region properties

        //  -----------------------
        //  ChannelControl property
        //  -----------------------

        private IChannelControl<TChannelInfo> ChannelControl => Control as IChannelControl<TChannelInfo>;

        //  ----------------
        //  Channel property
        //  ----------------

        private TChannelInfo Channel
        {
            get
            {
                var node = ParentSheet.SelectionObject as ResultNode;
                return (TChannelInfo)node?.Tag;
            }
        }

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ChannelControl?.RefreshData(Channel);
            Dirty = false;
        }

        //  -----------
        //  OnOK method
        //  -----------

        protected override bool OnOK()
        {
            if (Dirty)
            {
                return Save();
            }
            else return true;
        }

        //  --------------
        //  OnApply method
        //  --------------

        protected override bool OnApply()
        {
            return Save();
        }

        #endregion overrides

        #region Save method

        //  -----------
        //  Save method
        //  -----------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool Save()
        {
            ChannelControl.UpdateChannel(Channel);
            try
            {
                var result = SnapIn.Router.UpdateChannel(Channel);
                if (result.Succeeded)
                {
                    SnapIn.Router.ReloadChannels(Channel.BaseKey.ChannelType);
                    return true;
                }
                else ParentSheet.ShowDialog(result.ToMessageBoxParameters());
            }
            catch (Exception exception)
            {
                ParentSheet.ShowDialog(exception);
            }
            return false;
        }

        #endregion Save method
    }

    #region IChannelControl interface

    //  -------------------------
    //  IChannelControl interface
    //  -------------------------

    internal interface IChannelControl<TChannelInfo> : IPropertyPageControl
    {
        void RefreshData(TChannelInfo channel);
        void UpdateChannel(TChannelInfo channel);
    }

    #endregion IChannelControl interface

    #region IPropertyPageControl interface

    //  ------------------------------
    //  IPropertyPageControl interface
    //  ------------------------------

    internal interface IPropertyPageControl
    {
        PropertyPage PropertyPage { get; set; }
    }

    #endregion IPropertyPageControl interface

    #region PropertyPage<TSnapIn> class

    //  ---------------------------
    //  PropertyPage<TSnapIn> class
    //  ---------------------------

    internal class PropertyPage<TSnapIn> : PropertyPage where TSnapIn : class
    {
        //  ---------------
        //  SnapIn property
        //  ---------------

        protected TSnapIn SnapIn => ParentSheet.SnapIn as TSnapIn;
    }

    #endregion PropertyPage<TSnapIn> class
}

// eof "ChannelPropertyPage.cs"
