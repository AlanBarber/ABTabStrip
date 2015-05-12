using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABTabStrip
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TabStrip runat=\"server\" id=\"TabStrip1\" Text=\"TabStrip1\"></{0}:TabStrip>")]
    [ToolboxBitmap(typeof(ResourceFinder), "ABTabStrip.toolbox.bmp")]
    [PersistChildren(true)]
    [ParseChildren(false)]
    public class TabStrip : WebControl, IPostBackEventHandler
    {
        #region Properties

        private TabStripControlState _tabStripControlState = new TabStripControlState();

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get { return _tabStripControlState.Text; }
            set { _tabStripControlState.Text = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool UseDefaultStyle
        {
            get { return _tabStripControlState.UseDefaultStyle; }
            set { _tabStripControlState.UseDefaultStyle = value; }
        }

        public List<TabStripItem> Items
        {
            get { return _tabStripControlState.Items; }
        }

        public TabStripItem SelectedItem
        {
            get { return _tabStripControlState.SelectedItem; }
        }

        public object DataSource;

        #endregion

        #region Control Setup & Event Handlers

        /// <summary>
        /// Initializes a new instance of the <see cref="TabStrip"/> class.
        /// </summary>
        public TabStrip() {}

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            // Setup for Control
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
            // Load with default from text if we have no items or data source loaded (if there is no text set text to control ID)
            if (string.IsNullOrEmpty(Text))
                _tabStripControlState.Text = ID;

            _tabStripControlState.UseDefaultStyle = true;

            if (_tabStripControlState.Items == null)
                _tabStripControlState.Items = new List<TabStripItem>();

            if (_tabStripControlState.Items.Count <= 0)
            {
                _tabStripControlState.Items.Add(new TabStripItem() { Text = _tabStripControlState.Text, Value = string.Empty });
            }

            _tabStripControlState.SelectedItem = _tabStripControlState.Items.First();
        }
        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public override void DataBind()
        {
            if (DataSource != null)
            {
                // Parse out the type of DataSource so we can load it into our Items property
                // Formats we will accept are Array, List, HashSet of type TabStripItem or Dictionary<string, string>
                if (DataSource is TabStripItem[] 
                    || DataSource is List<TabStripItem>
                    || DataSource is HashSet<TabStripItem>)
                {
                    _tabStripControlState.Items = new List<TabStripItem>((IEnumerable<TabStripItem>) DataSource);
                }
                else if (DataSource is Dictionary<string, string>)
                {
                    _tabStripControlState.Items = new List<TabStripItem>()
                    {
                        new TabStripItem() {Text = "Dictionary", Value = string.Empty}
                    };
                }
                else
                {
                    throw new Exception();
                }

                _tabStripControlState.SelectedItem = _tabStripControlState.Items.First();
            }
            base.DataBind();
        }

        /// <summary>
        /// Occurs when [click].
        /// </summary>
        public event EventHandler<TabStripClickEventArgs> Click;

        /// <summary>
        /// Raises the <see cref="E:Click" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TabStripClickEventArgs"/> instance containing the event data.</param>
        protected virtual void OnClick(TabStripClickEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A <see cref="T:System.String" /> that represents an optional event argument to be passed to the event handler.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RaisePostBackEvent(string eventArgument)
        {
            _tabStripControlState.SelectedItem = _tabStripControlState.Items.FirstOrDefault(i => i.Text == eventArgument);
            OnClick(new TabStripClickEventArgs(_tabStripControlState.SelectedItem));
        }

        /// <summary>
        /// Saves any server control state changes that have occurred since the time the page was posted back to the server.
        /// </summary>
        /// <returns>
        /// Returns the server control's current state. If there is no state associated with the control, this method returns null.
        /// </returns>
        protected override object SaveControlState()
        {
            var baseControlState = base.SaveControlState();
            return new Pair(baseControlState, _tabStripControlState);
        }

        /// <summary>
        /// Loads the state of the control.
        /// </summary>
        /// <param name="controlState">The state.</param>
        protected override void LoadControlState(object controlState)
        {
            if (controlState != null)
            {
                Pair p = controlState as Pair;
                if (p != null)
                {
                    base.LoadControlState(p.First);
                    _tabStripControlState = p.Second as TabStripControlState;
                }
                else
                {
                    base.LoadControlState(controlState);
                }

            }
        }

        /// <summary>
        /// Renders the contents.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            if(_tabStripControlState.UseDefaultStyle)
                GenerateInlineStyle(output);
            // Wrapper
            output.AddAttribute("id", ClientID);
            output.AddAttribute("class", "ABTabStripWrapper");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            // Header
            output.AddAttribute("class", "ABTabStripHeader");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            GenerateTabStripHTML(output);
            output.RenderEndTag(); // Div (Header)
            // Content (Calls base to render nested controls inside of tab strip)
            output.AddAttribute("class", "ABTabStripContent");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderContents(output);
            output.RenderEndTag(); // Div (Content)
            output.RenderEndTag(); // Div (Wrapper)
        }
        
        #endregion

        #region Internal Functions

        private void GenerateInlineStyle(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Style);
            output.WriteLine(".ABTabStripWrapper            {}");
            output.WriteLine(".ABTabStripHeader             {}");
            output.WriteLine(".ABTabStripHeader ul          { list-style: none; padding: 0; margin: 0; }");
            output.WriteLine(".ABTabStripHeader li          { float: left; border: 1px solid #bbb; border-bottom-width: 0; margin: 0; }");
            output.WriteLine(".ABTabStripHeader a           { text-decoration: none; display: block; background: #d3d3d3; padding: 0.24em 1em; color: #000; width: auto; text-align: center; }");
            output.WriteLine(".ABTabStripHeader a:hover     { background: #ddf; }");
            output.WriteLine(".ABTabStripHeader .selected   { border-color: #000; }");
            output.WriteLine(".ABTabStripHeader .selected a { position: relative; top: 1px; background: #fff; color: #000; font-weight: bold; }");
            output.WriteLine(".ABTabStripContent            { clear: both; padding: 0; border: 1px solid black; }");
            output.RenderEndTag();
        }

        private void GenerateTabStripHTML(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Ul);

            if (Items != null)
            {
                foreach (var i in Items)
                {
                    if (i == SelectedItem)
                        output.AddAttribute("class", "selected");

                    output.RenderBeginTag(HtmlTextWriterTag.Li);

                    output.AddAttribute(HtmlTextWriterAttribute.Href,
                        string.Format("javascript:{0}", Page.ClientScript.GetPostBackEventReference(this, i.Text)));
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                    output.Write(i.Text);
                    output.RenderEndTag(); // A

                    output.RenderEndTag(); // Li
                }
            }

            output.RenderEndTag(); // Ul
        }

        #endregion
    }
}
