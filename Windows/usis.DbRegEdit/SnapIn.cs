//
//  @(#) SnapIn.cs
//
//  Project:    usis Database Registry
//  System:     Microsoft Visual Studio 14
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using usis.Platform.Data;
using usis.Windows.Forms;

namespace usis.Data.Registry.Editor
{
    //  ------------
    //  SnapIn class
    //  ------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [SnapInSettings(
        "656CF114-923A-4A4A-80F9-E3841B768C75",
         DisplayName = "Database Registry Editor",
         Description = "The usis Database Registry Editor snap-in provides an editor for database registry keys.",
         Vendor = "usis GmbH")]
    internal sealed class SnapIn : ManagementConsole.SnapIn<RootNode>
    {
        protected override bool OnShowInitializationWizard()
        {
            using (var control = new DataSourceControl())
            {
                control.ConnectionString = "Server=.;Database=audius;Integrated Security=true";
                using (var dialog = new ModalDialog())
                {
                    dialog.Content = control;
                    if (Console.ShowDialog(dialog) == DialogResult.OK)
                    {
                        RootNode.DataSource = control.CreateDataSource();
                        IsModified = true;
                        return true;
                    }
                    else return false;
                }
            }
        }

        protected override byte[] OnSaveCustomData(SyncStatus status)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, RootNode.DataSource);
                return stream.GetBuffer();
            }
        }

        protected override void OnLoadCustomData(AsyncStatus status, byte[] persistenceData)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(persistenceData))
            {
                var dataSource = formatter.Deserialize(stream) as DataSource;
                if (dataSource != null) RootNode.DataSource = dataSource;
            }
        }
    }
}

namespace usis.ManagementConsole
{
    public class SnapIn<TRootNode> : SnapIn where TRootNode : ScopeNode, new()
    {
        public SnapIn()
        {
            RootNode = new TRootNode();
        }

        protected new TRootNode RootNode
        {
            get
            {
                return base.RootNode as TRootNode;
            }
            set
            {
                base.RootNode = value;
            }
        }
    }
}

// eof "SnapIn.cs"
