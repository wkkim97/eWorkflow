using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bayer.eWF.BSL.Configuration.Dao;
using Bayer.eWF.BSL.Configuration.Dto;
using Bayer.eWF.BSL.Configuration.Mgr;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Configuration;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Common.Dto;

public partial class Common_Popup_PopupUserList : DNSoft.eWF.FrameWork.Web.PageBase
{
    string fullname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!this.IsPostBack)
            {
                string allowMultiselect = Request["multiselect"].NullObjectToEmptyEx();

                if (allowMultiselect.ToUpper().Equals("Y"))
                {
                    RadGridUserList.MasterTableView.GetColumn("chkYN").Display = true;
                    RadGridUserList.AllowMultiRowSelection = true;
                }

                InitControls();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }



    #region [ DataBinding ]
    private void InitControls()
    {
        fullname = this.hddSearch.Value;

        RadGridUserList.DataSource = userlist(fullname);
        RadGridUserList.DataBind();
    }

    protected void RadGridUserList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        (sender as RadGrid).DataSource = userlist(fullname);
    }
    public List<SmallUserInfoDto> userlist(string fullname)
    {
        //UserList Binding		
        using (Bayer.eWF.BSL.Common.Dao.UserDao dao = new Bayer.eWF.BSL.Common.Dao.UserDao())
        {
            fullname = hddSearch.Value;
            return dao.SelectUserList(fullname).ToList();

        }
    }
    protected void RadGridUserList_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        InitControls();
    }
    protected void RadGridUserList_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        InitControls();
    }
    protected void RadGridUserList_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        InitControls();
    }
    #endregion

    #region [ checkbox 이벤트 ]
    protected void ToggleSelectedState(object sender, EventArgs e)
    {
        List<Bayer.eWF.BSL.Configuration.Dto.DTO_READERS_GROUP_USER_NAME> userlist = new List<Bayer.eWF.BSL.Configuration.Dto.DTO_READERS_GROUP_USER_NAME>();
        CheckBox headerCheckBox = (sender as CheckBox);
        foreach (GridDataItem dataItem in this.RadGridUserList.MasterTableView.Items)
        {
            (dataItem.FindControl("chkboxbody") as CheckBox).Checked = headerCheckBox.Checked;
            dataItem.Selected = headerCheckBox.Checked;

        }
    }
    protected void ToggleRowSelection(object sender, EventArgs e)
    {
        ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
        bool checkHeader = true;
        foreach (GridDataItem dataItem in this.RadGridUserList.MasterTableView.Items)
        {
            if (!(dataItem.FindControl("chkboxbody") as CheckBox).Checked)
            {
                checkHeader = false;
                break;
            }
        }
        GridHeaderItem headerItem = RadGridUserList.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        (headerItem.FindControl("headerChkbox") as CheckBox).Checked = checkHeader;
    }
    #endregion

    protected void RadbtnMultiCheck_CheckedChanged(object sender, EventArgs e)
    {
        RadButton chkButton = (RadButton)sender;
        if (chkButton.Checked == true)
        {
            RadGridUserList.MasterTableView.GetColumn("chkYN").Display = true;
            RadGridUserList.AllowMultiRowSelection = true;
        }
        else
        {
            RadGridUserList.MasterTableView.GetColumn("chkYN").Display = false;
            RadGridUserList.AllowMultiRowSelection = false;
        }

    }
    protected void RadbtnSearch_Click(object sender, EventArgs e)
    {
        using (ConfigurationDao searchList = new ConfigurationDao())
        {
            this.hddSearch.Value = this.RadSearchBox.Text;
            RadGridUserList.DataSource = searchList.SelectReadersGroupUser(hddSearch.Value);
            RadGridUserList.Rebind();
        }
    }


}