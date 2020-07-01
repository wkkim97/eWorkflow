using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Reporting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Reporting_DocOwnerView : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitControls();
        }
        PageLoadInfo();
    }

    private void InitControls()
    {
        HtmlInputHidden hddUserId = this.Master.FindControl("hddUserId") as HtmlInputHidden;
        hddUserId.Value = Sessions.UserID;
        this.grdSearch.DataSource = string.Empty;
        this.grdSearch.DataBind();
        //GridSource();
        //GridBind();
    }

    private void PageLoadInfo()
    {
    }


    #region Select Grid

    private void GridSource()
    {
        string chk = null;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        DetailSearch condition = (DetailSearch)serializer.Deserialize<DetailSearch>(this.hddPoppupItem.Value);

        if (condition == null)
        {
            condition = new DetailSearch();
            condition.DOCUMENT_ID = "";
            condition.SUBJECT = "";
            condition.FROM_DATE = new DateTime(1900, 01, 01);
            condition.TO_DATE = new DateTime(2050, 12, 31);
        }
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            if (condition.FROM_DATE.HasValue == false && condition.TO_DATE.HasValue == false)
                chk = "N";
            else chk = "Y";

            grdSearch.DataSource = mgr.SelectAdminViewDocumentList(condition.DOCUMENT_ID, condition.SUBJECT, chk, condition.FROM_DATE, condition.TO_DATE, Sessions.UserID);
        }

    }

    #endregion

    #region GridBind
    private void GridBind()
    {
        grdSearch.DataBind();
    }
    #endregion

    #region Grid Event
    protected void grdSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (this.IsPostBack) GridSource();
    }

    protected void grdSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DTO_ADMIN_DOCUMENT_LIST i = e.Item.DataItem as DTO_ADMIN_DOCUMENT_LIST;
            GridDataItem item = e.Item as GridDataItem;

            //if (i.ATTACHFILEYN.Equals("Y"))
            //{
            //    Image attach = item.FindControl("iconAttach") as Image;
            //    attach.ImageUrl = "/eWorks/Styles/images/Common/icon_attach.gif";
            //}
            if (i.COMMENT.Length > 0)
            {
                Image attach = item.FindControl("iconComment") as Image;
                attach.ImageUrl = "/eWorks/Styles/images/Common/icon_comment.png";
            }
        }
    }

    #endregion

    #region btnSearch_Click
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //GridSource();
            //GridBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion
    protected void ajaxMgr_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            GridSource();
            GridBind();
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string fileName = HttpUtility.UrlEncode("AdminView", new System.Text.UTF8Encoding()).Replace("+", "%20");

        grdSearch.ExportSettings.ExportOnlyData = true;
        grdSearch.ExportSettings.IgnorePaging = true;
        grdSearch.ExportSettings.OpenInNewWindow = true;
        grdSearch.ExportSettings.FileName = fileName;
        grdSearch.MasterTableView.ExportToExcel();

    }



    #region btnDownload_Click

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string chk = null;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        DetailSearch condition = (DetailSearch)serializer.Deserialize<DetailSearch>(this.hddPoppupItem.Value);
        if (condition == null)
        {
            condition = new DetailSearch();
            condition.DOCUMENT_ID = "";
            condition.SUBJECT = "";
            condition.FROM_DATE = new DateTime(1900, 01, 01);
            condition.TO_DATE = new DateTime(2050, 12, 31);
        }

        string filename = "DocumentList.xls";
        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.ClearContent();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        //Response.Charset = "utf-8";

        Response.Charset = "euc-kr";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");

        // Create a dynamic control, populate and render it
        GridView excel = new GridView();
        
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            if (condition.FROM_DATE.HasValue == false && condition.TO_DATE.HasValue == false)
                chk = "N";
            else chk = "Y";

            excel.DataSource = mgr.SelectAdminViewDocumentList(condition.DOCUMENT_ID, condition.SUBJECT, chk, condition.FROM_DATE, condition.TO_DATE, Sessions.UserID);
            //excel.DataSource = mgr.SelectAdminViewDocumentList("D0013", condition.SUBJECT, "N", condition.FROM_DATE, condition.TO_DATE, Sessions.UserID);
        }
        excel.DataBind();
        excel.RenderControl(new HtmlTextWriter(Response.Output));

        Response.Flush();
        Response.End();
    }
    #endregion


    //#region btnDownload_NEW_Click

    //protected void btnDownload_NEW_Click(object sender, EventArgs e)
    //{
    //    string chk = null;
    //    JavaScriptSerializer serializer = new JavaScriptSerializer();
    //    DetailSearch condition = (DetailSearch)serializer.Deserialize<DetailSearch>(this.hddPoppupItem.Value);
    //    if (condition == null)
    //    {
    //        condition = new DetailSearch();
    //        condition.DOCUMENT_ID = "";
    //        condition.SUBJECT = "";
    //        condition.FROM_DATE = new DateTime(1900, 01, 01);
    //        condition.TO_DATE = new DateTime(2050, 12, 31);
    //    }

    //    string filename = "REPORTING_DOCUMENT_" + condition.DOCUMENT_ID.Replace(",","") + ".xls";
    //    // This actually makes your HTML output to be downloaded as .xls file
    //    Response.Clear();
    //    Response.ClearContent();
    //    Response.ContentType = "application/octet-stream";
    //    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
    //    Response.Charset = "utf-8";
    //    //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");

    //    // Create a dynamic control, populate and render it
    //    GridView excel = new GridView();

    //    using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
    //    {
    //        if (condition.FROM_DATE.HasValue == false && condition.TO_DATE.HasValue == false)
    //            chk = "N";
    //        else chk = "Y";

    //        excel.DataSource = mgr.SelectAdminReportingDocumentList(condition.DOCUMENT_ID.Replace(",", ""));
    //        //excel.DataSource = mgr.SelectAdminReportingDocumentList("D0036");
    //    }
    //    excel.DataBind();
    //    excel.RenderControl(new HtmlTextWriter(Response.Output));

    //    Response.Flush();
    //    Response.End();
    //}
    //#endregion
    #region btnREPORT_Click
    protected void btnREPORT_Click(object sender, EventArgs e)
    {
        try
        {
            //GridSource();
            //GridBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion
}