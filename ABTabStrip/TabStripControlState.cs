using System;
using System.Collections.Generic;

namespace ABTabStrip
{
    [Serializable()]
    internal sealed class TabStripControlState
    {
        public string Text;
        public List<TabStripItem> Items;
        public TabStripItem SelectedItem;
        public bool UseDefaultStyle;
    }
}