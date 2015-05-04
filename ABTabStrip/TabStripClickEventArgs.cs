using System;

namespace ABTabStrip
{
    [Serializable()]
    public class TabStripClickEventArgs : EventArgs
    {
        private readonly TabStripItem _tabStripItem;

        public TabStripClickEventArgs()
        {
            _tabStripItem = null;
        }

        public TabStripClickEventArgs(TabStripItem tabStripItem)
        {
            _tabStripItem = tabStripItem;
        }

        public TabStripItem SelectedItem
        {
            get { return _tabStripItem; }
        }
    }
}