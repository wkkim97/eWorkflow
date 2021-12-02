using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using System.Web.Script.Serialization;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;


public partial class Approval_List_WithdrawList : DNSoft.eWF.FrameWork.Web.PageBase
{
    private const string VIEWSTATE_DETAIL_ITEMS_KEY = "VIEWSTATE_DETAIL_ITEMS_KEY";
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                ViewState[VIEWSTATE_DETAIL_ITEMS_KEY] = new List<DTO_PROCESS_PROECESSING_LIST>();
                this.grdSearch.DataSource = (List<DTO_PROCESS_PROECESSING_LIST>)ViewState[VIEWSTATE_DETAIL_ITEMS_KEY];
                this.grdSearch.DataBind();
                InitControls();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        GridSource();
        GridBind();
    }

    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {

        //if (this.hddPoppupItem.Value != "")
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    List<DTO_PROCESS_PROECESSING_LIST> items = (List<DTO_PROCESS_PROECESSING_LIST>)serializer.Deserialize<List<DTO_PROCESS_PROECESSING_LIST>>(this.hddPoppupItem.Value);

        //    ViewState[VIEWSTATE_DETAIL_ITEMS_KEY] = items;
        //    this.grdSearch.DataSource = items;
        //    this.grdSearch.DataBind();
        //}
    }

    public void Approval_List_WithdrawList_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            GridSource();
            GridBind();
        }
    }

    #endregion

    #region GridSource
    private class DetailSearch
    {
        public string DOCUMENT_ID { get; set; }
        public string SUBJECT { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }

    }

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

            grdSearch.DataSource = mgr.SelectApprovalWithDrawList(condition.DOCUMENT_ID, condition.SUBJECT, chk, condition.FROM_DATE, condition.TO_DATE, Sessions.UserID);
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
        GridSource();
    }

    protected void grdSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DTO_PROCESS_PROECESSING_LIST i = e.Item.DataItem as DTO_PROCESS_PROECESSING_LIST;
            GridDataItem item = e.Item as GridDataItem;

            if (i.ATTACHFILEYN.Equals("Y"))
            {
                Image attach = item.FindControl("iconAttach") as Image;
                attach.ImageUrl = "/eWorks/Styles/images/Common/icon_attach.gif";
            }
            if (i.COMMENT != null && i.COMMENT.Length > 0)
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
            GridSource();
            GridBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion
}