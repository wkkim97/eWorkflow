using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Reporting.Dto;

public partial class Common_TopMenu : DNSoft.eWF.FrameWork.Web.UserControlsBase
{
    List<SiteDataItem> siteData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetInitControls();
            MenuDataBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void SetInitControls()
    {
        StringBuilder sb = new StringBuilder(128);
        try
        {
            hspanOrgName.InnerText = Sessions.OrgName;
            hspanUser.InnerText = Sessions.UserName;

            if (Sessions.UserRole.Equals(ApprovalUtil.UserRole.Admin) || Sessions.UserRole.Equals(ApprovalUtil.UserRole.Design))
            {
                sb.Append("<a href='/eworks/Configuration/Configuration.aspx' >[ADMIN]</a>&nbsp;&nbsp;|&nbsp;&nbsp;");
            }
            sb.Append("<a href='javascript:openSettings();' >[Configuration]</a>&nbsp;&nbsp;|&nbsp;&nbsp;");

            spanExtendedMenu.InnerHtml = sb.ToString();
            hrefHome.Attributes.Add("href", Sessions.MainFormUrl);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sb != null)
            {
                sb.Clear();
                sb = null;
            }
        }

    }

    private void MenuDataBind()
    {
        siteData = new List<SiteDataItem>();

        siteData.Add(new SiteDataItem(10, null, "Home", Sessions.MainFormUrl));
        siteData.Add(new SiteDataItem(20, null, "Not Submitted", "/eWorks/Approval/List/SavedList.aspx"));
        siteData.Add(new SiteDataItem(30, null, "Approval Queue", "/eWorks/Approval/List/TodoList.aspx"));
        siteData.Add(new SiteDataItem(40, null, "Pending Approval", "/eWorks/Approval/List/ApproveList.aspx"));
        siteData.Add(new SiteDataItem(50, null, "Approved", "#"));
        siteData.Add(new SiteDataItem(51, 50, "Completed", "/eWorks/Approval/List/CompletedList.aspx"));
        siteData.Add(new SiteDataItem(52, 50, "Reject", "/eWorks/Approval/List/RejectList.aspx"));
        siteData.Add(new SiteDataItem(53, 50, "Withdraw", "/eWorks/Approval/List/WithdrawList.aspx"));
        siteData.Add(new SiteDataItem(60, null, "Reporting", "#"));

        int cnt = 0;
        using (Bayer.eWF.BSL.Configuration.Mgr.ConfigurationMgr mgr = new Bayer.eWF.BSL.Configuration.Mgr.ConfigurationMgr())
        {
            cnt = mgr.SelectReadersGroupCount(Sessions.UserID);
        }



        using (Bayer.eWF.BSL.Reporting.Mgr.ReportingMgr mgr = new Bayer.eWF.BSL.Reporting.Mgr.ReportingMgr())
        {
            int index = 61;
            if (cnt > 0)
            {
                List<DTO_REPORTING_PROGRAM> list = mgr.SelectProgramList(Sessions.UserID, "Reporting");

                
                if (list.Count > 0)
                {
                    siteData.Add(new SiteDataItem(index, 60, "Admin View", list[0].URL));
                    index++;
                }
            }

            List<DTO_REPORTING_PROGRAM> listMain = mgr.SelectProgramList(Sessions.UserID, "Main");


            foreach (DTO_REPORTING_PROGRAM program in listMain)
            {
                siteData.Add(new SiteDataItem(index, 60, program.PROGRAM_NAME, program.URL));
                index++;
            }

        }

        //if (Sessions.UserID.Equals("BKANG") || Sessions.UserID.Equals("BKKWK") || Sessions.UserID.Equals("SGWVX"))
        //{
        //    siteData.Add(new SiteDataItem(61, 60, "Return Goods", "/eWorks/ReturnGoods/ReturnGoods.aspx"));
        //    siteData.Add(new SiteDataItem(62, 60, "Collateral management", "/eWorks/Approval/List/CollateralList.aspx"));
        //    siteData.Add(new SiteDataItem(63, 60, "Customer List", "/eWorks/Reporting/CustomerMasterList.aspx"));
        //    siteData.Add(new SiteDataItem(64, 60, "Product List", "/eWorks/Reporting/ProductMasterList.aspx"));
        //    siteData.Add(new SiteDataItem(65, 60, "Admin Document List", "/eWorks/Reporting/AdminDocumentList.aspx"));
        //}
        //siteData.Add(new SiteDataItem(70, null, "Administrators", ""));
        //siteData.Add(new SiteDataItem(71, 70, "Configuration", "/eworks/Configuration/Configuration.aspx"));
        //siteData.Add(new SiteDataItem(72, 70, "Readers Group", ""));


        CreateMenuBar(ulMenubar, null);

    }

    private void CreateMenuBar(HtmlGenericControl head, int? id)
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
                if (siteData.Exists(p => p.ParentID == m.ID))
                {
                    HtmlGenericControl divSub = new HtmlGenericControl("div");
                    string subName = "subGroup_" + m.ID.ToString();

                    HtmlGenericControl ulSub = new HtmlGenericControl("ul");
                    CreateSubMenuBar(ref ulSub, m.ID);
                    ulSub.Attributes.Add("id", subName);

                    li.Controls.Add(ulSub);

                }
                if (m.ID == 60)
                    li.Attributes.Add("class", "last");
                head.Controls.Add(li);
            }

        }

    }

    private HtmlGenericControl CreateSubMenuBar(ref HtmlGenericControl head, int? id)
    {
        List<SiteDataItem> items = siteData.FindAll(m => m.ParentID == id);

        foreach (SiteDataItem m in items)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            HtmlGenericControl anchor = new HtmlGenericControl("a");
            li.ID = "menu_" + m.ID;
            anchor.Attributes.Add("href", m.Url);
            anchor.InnerText = m.Text;
            li.Controls.Add(anchor);
            head.Controls.Add(li);
        }

        return head;
    }



}