using System;

namespace ABTabStrip
{
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

        public TabStripItem TabStripItem
        {
            get { return _tabStripItem; }
        }
    }
}