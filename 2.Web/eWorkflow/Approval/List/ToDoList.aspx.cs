using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;

public partial class Approval_List_ToDoList : DNSoft.eWF.FrameWork.Web.PageBase
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                InitControls();
            }
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
     
    #region GridSource
    private void GridSource()
    {
        string chk = SearchBar.UseDateYN;
        List<DTO_PROCESS_TODO_LIST> list;

        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            list = mgr.SelectApprovalTodoList(SearchBar.SearchType, SearchBar.SearchText, chk, SearchBar.StartDate, SearchBar.EndDate, Sessions.UserID);
        }
        grdSearch.DataSource = list;
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
            DTO_PROCESS_TODO_LIST i = e.Item.DataItem as DTO_PROCESS_TODO_LIST;
            GridDataItem item = e.Item as GridDataItem;

            if (i.ATTACHFILEYN.Equals("Y"))
            {
                Image attach = item.FindControl("iconAttach") as Image;
                attach.ImageUrl = "/eWorks/Styles/images/Common/icon_attach.gif";
            }
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