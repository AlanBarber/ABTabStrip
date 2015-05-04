using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABTabStrip;

namespace DemoWebsite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TabStrip1.DataSource = null;
            TabStrip1.DataBind();

            //lblSelectedTab.Text = string.Format("Selected Tab [{0} - {1}]", TabStrip1.SelectedItem.Text, TabStrip1.SelectedItem.Value);
        }

        protected void TabStrip1_OnClick(object sender, TabStripClickEventArgs e)
        {
            lblSelectedTab.Text = string.Format("Selected Tab [{0} - {1}]", e.SelectedItem.Text, e.SelectedItem.Value);
        }
    }
}