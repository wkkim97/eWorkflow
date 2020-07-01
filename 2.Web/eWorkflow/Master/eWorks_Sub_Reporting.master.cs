using Bayer.eWF.BSL.Reporting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Master_eWorks_Sub_Reporting : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitControls();
    }

    private void InitControls()
    {
        using (Bayer.eWF.BSL.Reporting.Mgr.ReportingMgr mgr = new Bayer.eWF.BSL.Reporting.Mgr.ReportingMgr())
        {
            List<SiteDataItem> siteData = new List<SiteDataItem>();
            List<DTO_REPORTING_PROGRAM> list = mgr.SelectProgramList(this.hddUserId.Value, "Reporting");
            int index = 100;
            foreach (DTO_REPORTING_PROGRAM program in list)
            {
                siteData.Add(new SiteDataItem(index, null, program.PROGRAM_NAME, program.URL));
                index++;
            }
            CreateMenuBar(ulMenubar, siteData, null);
        }

    }

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
