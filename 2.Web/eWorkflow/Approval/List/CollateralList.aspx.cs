using Bayer.eWF.BSL.Approval.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Approval_List_CollateralList : DNSoft.eWF.FrameWork.Web.PageBase
{
    //private const string VIEWSTATE_DETAIL_ITEMS_KEY = "VIEWSTATE_DETAIL_ITEMS_KEY";
    #region Page Load


    protected override void OnInit(EventArgs e)
    {
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            List<string> list = mgr.SelectCollateralBg();

            foreach (string bg in list)
            {
                RadButton btn = new RadButton();
                btn.ButtonType = RadButtonType.ToggleButton;
                btn.ToggleType = ButtonToggleType.CheckBox;
                btn.AutoPostBack = false;
                btn.Text = bg;
                btn.Value = bg;
                btn.ID = "radBtn" + bg;
                btn.Checked = true;

                this.divBG.Controls.Add(btn);
            }
        }
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
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

    #region PageLoadInfo
    private void PageLoadInfo()
    {

        //if (this.hddPoppupItem.Value != "")
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    List<DTO_PROCESS_COMPLETED_LIST> items = (List<DTO_PROCESS_COMPLETED_LIST>)serializer.Deserialize<List<DTO_PROCESS_COMPLETED_LIST>>(this.hddPoppupItem.Value);

        //    ViewState[VIEWSTATE_DETAIL_ITEMS_KEY] = items;
        //    this.grdSearch.DataSource = items;
        //    this.grdSearch.DataBind();
        //}
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.grdSearch);
        //RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_List_CompletedList_AjaxRequest;
        //GridSource();
    }

    //public void Approval_List_CompletedList_AjaxRequest(object sender, AjaxRequestEventArgs e)
    //{
    //    if (e.Argument.StartsWith("Rebind"))
    //    {
    //        GridSource();
    //        GridBind();
    //    }
    //}

    #endregion

    #region InitControls
    private void InitControls()
    {
        
        this.grdSearch.DataSource = string.Empty;
        this.grdSearch.DataBind();

        this.radBtnTypeCurrently.Checked = true;
        //GridSource();
        // GridBind();
    }

    #endregion

    #region GridSource    

    private void GridSource()
    {
        string bg = string.Empty;
        foreach (Control control in divBG.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    bg += (control as RadButton).Value + ",";
            }
        }

        string type = string.Empty;

        if (radBtnTypeCurrently.Checked) type += radBtnTypeCurrently.Value + ",";
        if (radBtnTypeReturned.Checked) type += radBtnTypeReturned.Value;

        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            grdSearch.DataSource = mgr.SelectCollateralCompletedList(bg, type, this.radTxtKeyWord.Text, Sessions.UserID);
            
        }
    }
    #endregion

    #region Grid Event
    protected void grdSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        GridSource();
    }

    

    #endregion
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GridSource();
        grdSearch.DataBind();
    }
    protected void grdSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string fileName = HttpUtility.UrlEncode("Collateral", new System.Text.UTF8Encoding()).Replace("+", "%20");

        grdSearch.ExportSettings.ExportOnlyData = true;
        grdSearch.ExportSettings.IgnorePaging = true;
        grdSearch.ExportSettings.OpenInNewWindow = true;
        grdSearch.ExportSettings.FileName = fileName;
        grdSearch.MasterTableView.ExportToExcel();

    }
    protected void grdSearch_GroupsChanging(object sender, GridGroupsChangingEventArgs e)
    {


    }
}