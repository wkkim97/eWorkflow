using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Approval.Common
{
    public partial class TopMenu : System.Web.UI.UserControl
    {
         
        protected void Page_Load(object sender, EventArgs e)
        {
            MenuDataBind();
        }

        private void MenuDataBind()
        {
            List<SiteDataItem> siteData = new List<SiteDataItem>();

            siteData.Add(new SiteDataItem(1, null, "New Request", ""));
            siteData.Add(new SiteDataItem(2, null, "Temporary save", ""));
            siteData.Add(new SiteDataItem(3, null, "Todo", ""));
            siteData.Add(new SiteDataItem(4, null, "Approve", ""));
            siteData.Add(new SiteDataItem(5, null, "Complete", ""));
            siteData.Add(new SiteDataItem(8, 5, "Completed", ""));
            siteData.Add(new SiteDataItem(9, 5, "Reject", ""));
            siteData.Add(new SiteDataItem(10, 5, "Withdraw", ""));
            siteData.Add(new SiteDataItem(6, null, "Return Goods", ""));
            siteData.Add(new SiteDataItem(7, null, "Administrators", ""));
            siteData.Add(new SiteDataItem(8, 7, "Configuration", ""));
            siteData.Add(new SiteDataItem(9, 7, "Readers Group", ""));

            radTopMenu.DataTextField = "Text";
            radTopMenu.DataNavigateUrlField = "Url";
            radTopMenu.DataFieldID = "ID";
            radTopMenu.DataFieldParentID = "ParentID";
            radTopMenu.DataSource = siteData;

            radTopMenu.DataBind();
        }

        internal class SiteDataItem
        {
            private string _text;
            private string _url;
            private int _id;
            private int? _parentId;

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            public string Url
            {
                get { return _url; }
                set { _url = value; }
            }

            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }

            public int? ParentID
            {
                get { return _parentId; }
                set { _parentId = value; }
            }

            public SiteDataItem(int id, int? parentId, string text, string url)
            {
                _id = id;
                _parentId = parentId;
                _text = text;
                _url = url;
            }
        }
         
    }
}