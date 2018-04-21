//
//  @(#) DataElementGroup.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace usis.FinTS.Base
{
    //  ----------------------
    //  DataElementGroup class
    //  ----------------------

    internal abstract class DataElementGroup : ISegmentElement
    {
        #region fields

        private List<DataElementItem> items = new List<DataElementItem>();

        #endregion fields

        #region methods

        //  -----------------
        //  AddElement method
        //  -----------------

        internal protected void AddElement(ISegmentElement element, bool optional)
        {
            items.Add(new DataElementItem(element, optional));
        }

        //  ------------------
        //  AddElements method
        //  ------------------

        internal protected void AddElements(params ISegmentElement[] elements)
        {
            items.AddRange(elements);
        }

        #endregion methods

        #region ISegmentElement implementation

        //  ----------------
        //  Serialize method
        //  ----------------

        void ISegmentElement.Serialize(StreamWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            items.FirstOrDefault()?.Element.Serialize(writer);
            foreach (var item in items.Skip(1))
            {
                writer.Write(Constants.DataElementGroupSeparatorCharacter);
                item.Element.Serialize(writer);
            }
        }

        //  ------------------
        //  Deserialize method
        //  ------------------

        bool ISegmentElement.Deserialize(StreamReader reader, char terminator, bool optional)
        {
            var last = items.LastOrDefault();
            foreach (var item in items)
            {
                var c = item.Equals(last) ? terminator : Constants.DataElementGroupSeparatorCharacter;
                if (!item.Element.Deserialize(reader, c, item.Optional)) break;
            }
            return true;
        }

        #endregion ISegmentElement implementation
    }

    #region DataElementItem class

    //  ---------------------
    //  DataElementItem class
    //  ---------------------

    internal sealed class DataElementItem
    {
        #region properties

        //  ----------------
        //  Element property
        //  ----------------

        internal ISegmentElement Element { get; }

        //  -----------------
        //  Optional property
        //  -----------------

        internal bool Optional { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DataElementItem(ISegmentElement element, bool optional) { Element = element; Optional = optional; }

        #endregion construction
    }

    #endregion DataElementItem class
}

// eof "DataElementGroup.cs"
