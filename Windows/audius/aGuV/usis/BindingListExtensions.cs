using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace usis.Plaform
{
    internal static class BindingListExtensions
    {
        internal static void Reset<TItem>(this BindingList<TItem> bindingList, Func<IEnumerable<TItem>> loader)
        {
            if (bindingList == null) throw new ArgumentNullException(nameof(bindingList));

            bindingList.RaiseListChangedEvents = false;
            bindingList.Clear();
            foreach (var item in loader.Invoke())
            {
                bindingList.Add(item);
            }
            bindingList.RaiseListChangedEvents = true;
            bindingList.ResetBindings();
        }
    }
}
