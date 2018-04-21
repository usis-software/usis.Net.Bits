//
//  @(#) SnapIn.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using UIKit;
using usis.Cocoa.UIKit;
using usis.Mobile;
using usis.Platform;

namespace usis.Cocoa.PNRouter
{
    //  ------------
    //  SnapIn class
    //  ------------

    internal class SnapIn : usis.Framework.SnapIn
    {
        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            // create model and add it as an extension
            var model = new Model();
            Application.Extensions.Add(model);

            ViewDescription rootView;

            var layout = new AppLayout(
                rootView = new ViewDescription("root", typeof(ChannelListViewController))
                {
                    Navigation = true
                }
                .Title(Strings.ChannelListViewTitle)
                .RefreshEnabled(true)
                .AddAttribute("ModelCollectionName", "Channels")
                .AddAttribute("DefaultTableViewCellStyle", UITableViewCellStyle.Subtitle)
                .AddCommand("ItemSelectedCommand", new ViewCommandDescription(typeof(PushViewCommand))
                {
                    View = "ChannelDetails"
                }),
                new ViewDescription("ChannelDetails", typeof(ChannelDetailViewController)));

            layout.RootView = rootView.Key;
            this.SetActiveLayout(layout);

            base.OnConnecting(e);
        }
    }

    public class ViewCommandDescription : CommandDescription
    {
        public ViewCommandDescription() { }
        public ViewCommandDescription(Type type) : base(type) { }
        public string View { get; set; }
    }

    public class CommandDescription
    {
        public CommandDescription() { }
        public CommandDescription(Type type)
        {
            TypeName = type.AssemblyQualifiedName;
        }
        public string TypeName { get; set; }
    }

    public class BarItemDescription
    {
    }

    public static class Extensions
    {
        public static ViewDescription Title(this ViewDescription viewDescription, string title)
        {
            return viewDescription.AddAttribute(ViewControllerAttributeName.Title, title);
        }
        public static ViewDescription RefreshEnabled(this ViewDescription viewDescription, bool enabled)
        {
            return viewDescription.AddAttribute(ViewControllerAttributeName.RefreshEnabled, enabled);
        }
        public static ViewDescription AddCommand(this ViewDescription viewDescription, string key, ViewCommandDescription command)
        {
            var store = viewDescription.Attributes.CreateStorage(key, true);
            store.SetValue(nameof(CommandDescription.TypeName), command.TypeName);
            store.SetValue(nameof(ViewCommandDescription.View), command.View);
            return viewDescription;
        }
    }
}

// eof "SnapIn.cs"
