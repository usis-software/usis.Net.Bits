//
//  @(#) StorageControl.cs
//
//  Project:    Storage Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using usis.Platform.StructuredStorage;
using usis.Windows.Forms;

namespace usis.StorageEditor
{
    //  --------------------
    //  StorageControl class
    //  --------------------

    internal partial class StorageControl : UserControl, Framework.IView<StorageDocument>
    {
        #region fields

        private StorageDocument document;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        public StorageControl()
        {
            InitializeComponent();
        }

        #endregion construction

        #region properties

        //  --------------
        //  Model property
        //  --------------

        public StorageDocument Model => document;

        //  ------------------
        //  TreeWidth property
        //  ------------------

        public int TreeWidth
        {
            get => treeView.Width;
            set => treeView.Width = value;
        }

        #endregion properties

        #region events

        //  ----------------------
        //  ElementSelected method
        //  ----------------------

        internal event EventHandler<ElementEventArgs> ElementSelected;

        #endregion events

        #region methods

        //  -------------------
        //  LoadDocument method
        //  -------------------

        private StorageDocument LoadDocument(StorageDocument newDocument)
        {
            treeView.Nodes.Clear();
            treeView.Nodes.Add(new ElementNode(newDocument.Root.Statistics, newDocument.Root, newDocument.Title));
            return newDocument;
        }

        //  ---------------------------
        //  TreeViewBeforeExpand method
        //  ---------------------------

        private void TreeViewBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            (e.Node as ElementNode)?.OnBeforeExpand(Model.Root);
        }

        //  --------------------------
        //  TreeViewAfterSelect method
        //  --------------------------

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is ElementNode node)
            {
                ElementSelected?.Invoke(this, new ElementEventArgs(node.Statistics));
            }
        }

        #region IView interface implementation

        //  -------------
        //  Inject method
        //  -------------

        public void Inject(StorageDocument dependency)
        {
            document = LoadDocument(dependency);
        }

        #endregion IView interface implementation

        #endregion methods

        #region ElementNode class

        //  -----------------
        //  ElementNode class
        //  -----------------

        private class ElementNode : TreeNode
        {
            #region properties

            //  -------------------
            //  Statistics property
            //  -------------------

            internal ElementStatistics Statistics { get; }

            #endregion properties

            #region construction

            //  ------------
            //  construction
            //  ------------

            private ElementNode() { }

            internal ElementNode(ElementStatistics statistics, Storage storage = null, string text = null) : base(text ?? statistics.Name)
            {
                Statistics = statistics;

                switch (Statistics.ElementType)
                {
                    case ElementType.None:
                        break;
                    case ElementType.Storage:
                        ImageIndex = SelectedImageIndex = 0;
                        if (storage != null)
                        {
                            if (storage.Elements.Any())
                            {
                                Nodes.Add(new ElementNode());
                            }
                        }
                        break;
                    case ElementType.Stream:
                        ImageIndex = SelectedImageIndex = 2;
                        break;
                    case ElementType.ByteArray:
                        break;
                    case ElementType.PropertySet:
                        break;
                    default:
                        break;
                }
            }

            #endregion construction

            #region methods

            //  ----------------
            //  BuildPath method
            //  ----------------

            private Stack<string> BuildPath()
            {
                var node = this;
                var path = new Stack<string>();
                while (node?.Parent != null)
                {
                    path.Push(node.Statistics.Name);
                    node = node.Parent as ElementNode;
                }
                return path;
            }

            //  ------------------
            //  WithStorage method
            //  ------------------

            private void WithStorage(Storage root, Action<Storage> action)
            {
                var path = BuildPath();
                if (path.Count == 0) action.Invoke(root);
                else
                {
                    Storage storage = null;
                    Storage childStorage = null;
                    do
                    {
                        var name = path.Pop();
                        childStorage = (childStorage ?? root).OpenStorage(name, root.Statistics.Mode);
                        if (storage == null) storage = childStorage;

                    } while (path.Count > 0);

                    action.Invoke(childStorage);
                    storage.Dispose();
                }
            }

            //  -------------------
            //  LoadChildren method
            //  -------------------

            private void LoadChildren(Storage root)
            {
                WithStorage(root, (storage) =>
                {
                    Nodes.AddRange(storage.Streams.Select(e => new ElementNode(e)).ToArray());
                    Nodes.AddRange(storage.Storages.Select(e =>
                    {
                        using (var s = storage.OpenStorage(e.Name))
                        {
                            return new ElementNode(e, s);
                        }
                    }).ToArray());
                });
            }

            //  ---------------------
            //  OnBeforeExpand method
            //  ---------------------

            internal void OnBeforeExpand(Storage root)
            {
                if (this.Children().FirstOrDefault() is ElementNode node && node.Statistics == null)
                {
                    Nodes.Clear();
                    LoadChildren(root);
                }
            }

            #endregion methods
        }

        #endregion ElementNode class
    }

    #region ElementEventArgs class

    //  ----------------------
    //  ElementEventArgs class
    //  ----------------------

    internal class ElementEventArgs : EventArgs
    {
        internal ElementEventArgs(ElementStatistics statistics)
        {
            Statistics = statistics;
        }

        internal ElementStatistics Statistics { get; }
    }

    #endregion ElementEventArgs class
}

//  eof "StorageControl.cs"
