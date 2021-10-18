using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dto;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Configuration.Dao;

public partial class Common_Popup_UserList : DNSoft.eWF.FrameWork.Web.PageBase
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

    private void InitControls()
    {
        fullname = this.hddSearch.Value;

        RadGridUserList.DataSource = userlist(fullname);
        RadGridUserList.DataBind();
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

    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        using (ConfigurationDao searchList = new ConfigurationDao())
        {
            this.hddSearch.Value = this.radTxtKeyword.Text;
            RadGridUserList.DataSource = userlist(this.radTxtKeyword.Text);
            RadGridUserList.Rebind();
        }
    }
    protected void RadGridUserList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        (sender as RadGrid).DataSource = userlist(fullname);
    }

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

}