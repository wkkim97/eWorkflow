using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;


public partial class Approval_List_SavedList : DNSoft.eWF.FrameWork.Web.PageBase
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
            list = mgr.SelectApprovalSavedList(SearchBar.SearchType, SearchBar.SearchText, chk, SearchBar.StartDate, SearchBar.EndDate, Sessions.UserID);
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

    #region [ Delete ]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string processIds = string.Empty;
            foreach (string idx in this.grdSearch.SelectedIndexes)
            {
                processIds += this.grdSearch.Items[Convert.ToInt32(idx)]["PROCESS_ID"].Text + ",";
            }

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.DeleteDocumentProcess(processIds);
            }

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