using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using DNSoft.eWF.FrameWork.Web;

public partial class Master_eWorks_Sub_ReturnGoods : System.Web.UI.MasterPage
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        List<SiteDataItem> siteData = new List<SiteDataItem>();

        siteData.Add(new SiteDataItem(65, null, "Return Goods", "/eWorks/ReturnGoods/ReturnGoods.aspx"));
        siteData.Add(new SiteDataItem(66, null, "File Upload", "/eWorks/ReturnGoods/FileUpload.aspx"));
        siteData.Add(new SiteDataItem(67, null, "SAP 연동", "/eWorks/ReturnGoods/SAPAmount.aspx"));
        siteData.Add(new SiteDataItem(68, null, "통계", "/eWorks/ReturnGoods/Statistics.aspx"));
        siteData.Add(new SiteDataItem(69, null, "Pending", "/eWorks/ReturnGoods/ReturnPending.aspx"));

        CreateMenuBar(ulMenubar, siteData, null);
    } 
    #endregion

    #region CreateMenuBar
    private void CreateMenuBar(HtmlGenericControl head, List<SiteDataItem> siteData, int? id)
    {
        foreach (SiteDataItem m in siteData)
        {
            if (m.ParentID == id)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                li.ID = "menu_" + m.ID;
                anchor.Attributes.Add("href", m.Url);
                anchor.InnerText = m.Text;

                li.Controls.Add(anchor);

                head.Controls.Add(li);
            }

        }
    } 
    #endregion
}
