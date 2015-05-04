using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABTabStrip
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TabStrip runat=\"server\" id=\"TabStrip1\" Text=\"TabStrip1\"></{0}:TabStrip>")]
    [PersistChildren(true)]
    [ParseChildren(false)]
    public class TabStrip : WebControl, IPostBackEventHandler
    {
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
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }

        public override void DataBind()
        {
            if (DataSource != null)
            {
                // overwrite items with data source
                // set selected item to first in list
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
            OnClick(new TabStripClickEventArgs(new TabStripItem()));
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

        #region Internal Functions

        private void GenerateInlineStyle(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Style);
            output.WriteLine(".BWCTabStripWrapper            {}");
            output.WriteLine(".BWCTabStripHeader             {}");
            output.WriteLine(".BWCTabStripHeader ul          { list-style: none; padding: 0; margin: 0; }");
            output.WriteLine(".BWCTabStripHeader li          { float: left; border: 1px solid #bbb; border-bottom-width: 0; margin: 0; }");
            output.WriteLine(".BWCTabStripHeader a           { text-decoration: none; display: block; background: #d3d3d3; padding: 0.24em 1em; color: #000; width: auto; text-align: center; }");
            output.WriteLine(".BWCTabStripHeader a:hover     { background: #ddf; }");
            output.WriteLine(".BWCTabStripHeader .selected   { border-color: #000; }");
            output.WriteLine(".BWCTabStripHeader .selected a { position: relative; top: 1px; background: #fff; color: #000; font-weight: bold; }");
            output.WriteLine(".BWCTabStripContent            { clear: both; padding: 0; border: 1px solid black; }");
            output.RenderEndTag();
        }

        private void GenerateTabStripHTML(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Ul);

            if (Items != null)
            {
                foreach (var i in Items)
                {
                    if (true)
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
