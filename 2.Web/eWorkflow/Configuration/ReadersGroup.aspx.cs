using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Configuration.Dao;
using Bayer.eWF.BSL.Configuration.Dto;
using Bayer.eWF.BSL.Configuration.Mgr;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;
using System.Web.Script.Serialization;

public partial class Configuration_ReadersGroup : DNSoft.eWF.FrameWork.Web.PageBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_READERS_GROUP";

    [Serializable]
    public class UserList
    {
        public string USER_ID { get; set; }
        public string FULL_NAME { get; set; }
        public string GROUP_CODE { get; set; }
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

    private void PageLoadInfo()
    {
        if (__VIEWSTATE.Value != "")
        {
            UpdataGridData();
        }
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.RadGridRdsGroup);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_ReaderGroup_AjaxRequest;
    }

    private void Approval_ReaderGroup_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdataGridData();
        }
    }

    private void UpdataGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<UserList> items = (List<UserList>)serializer.Deserialize<List<UserList>>(this.__VIEWSTATE.Value);

        ViewState[VIEWSTATE_KEY] = items;
        this.RadGridRdsGroup.DataSource = items;
        this.RadGridRdsGroup.DataBind();
    }


    #region [ DataBinding ]
    private void InitControls()
    {
        this.hddState.Value = "NEW"; //상태값 : NEW - 신규작성 , OLD - 기존작성에 추가

        GridBindData();
    }

    private void GridBindData()
    {
        // 그룹 리스트
        radGrdGroupList.DataSource = SelectGroupList();
        radGrdGroupList.DataBind();
        GridClear();
    }

    // Readers GroupList Binding 
    //private List<DTO_READERS_GROUP> SelectGroupList()
    //{
    //    using (ConfigurationDao dao = new ConfigurationDao())
    //    {
    //        return dao.SelectReadersGroupList();
    //    }
    //}

    private List<DTO_CONFIG> SelectGroupList()
    {
        using (ConfigurationMgr mgr = new ConfigurationMgr())
        {
            return mgr.SelectConfigurationList();
        }
    }

    protected void RadGridRdsGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        (sender as RadGrid).DataSource = string.Empty;
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string groupCode = this.hddGroupCode.Value;
        //string groupName = this.RadTbGroupName.Text;
        (sender as RadGrid).DataSource = GroupList(groupCode, "");
    }



    // Readers Group List 조회
    protected List<DTO_READERS_GROUP_USER_NAME> GroupList(string group_code, string group_name)
    {
        using (ConfigurationDao gList = new ConfigurationDao())
        {
            var result = gList.SelectReadersGroup(group_code, group_name).ToList();
            return result.ToList();
        }
    }

    #endregion

    #region [ checkbox 이벤트 ]
    protected void ToggleSelectedState(object sender, EventArgs e)
    {
        CheckBox headerCheckBox = (sender as CheckBox);
        foreach (GridDataItem dataItem in this.RadGrid1.MasterTableView.Items)
        {
            dataItem.Selected = headerCheckBox.Checked;
            (dataItem.FindControl("chkboxbody") as CheckBox).Checked = headerCheckBox.Checked;

        }
    }
    protected void ToggleRowSelection(object sender, EventArgs e)
    {
        ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
        bool checkHeader = true;
        foreach (GridDataItem dataItem in this.RadGrid1.MasterTableView.Items)
        {
            if (!(dataItem.FindControl("chkboxbody") as CheckBox).Checked)
            {
                checkHeader = false;
                break;
            }
        }
        GridHeaderItem headerItem = RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        (headerItem.FindControl("headerChkbox") as CheckBox).Checked = checkHeader;
    }
    #endregion

    #region [ Group User Save 이벤트 ]
    protected void RadBtnSave_Click(object sender, EventArgs e)
    {
        if (hddState.Value == "NEW")
        {
            //foreach (GridDataItem item in radGrdGroupList.Items)
            //{
            //    if (item["DOC_NAME"].Text == this.RadTbGroupName.Text)
            //    {
            //        this.informationMessage = "Same Group Name Exist";
            //        return;
            //    }
            //}
        }

        try
        {
            using (ConfigurationMgr Mgr = new ConfigurationMgr())
            {
                //DTO_READERS_GROUP group = new DTO_READERS_GROUP();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + this.hddState.Value + "');", true);
                //if (this.hddState.Value == "NEW")  // 신규 작성시에만 그룹코드 생성
                //{
                //    this.hddGroupCode.Value = Mgr.CreateGroupCode(); //그룹코드생성
                //    group.GROUP_CODE = this.hddGroupCode.Value;
                //}
                //else
                //    group.GROUP_CODE = this.hddGroupCode.Value;
                //group.GROUP_NAME = "";
                //group.CREATOR_ID = this.Sessions.UserID.ToString();
                //group.CREATE_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                List<UserList> SerealizeUserlist = JsonConvert.JsonListDeserialize<UserList>(__VIEWSTATE.Value);
                List<DTO_READERS_GROUP_USER_LIST> userlist = new List<DTO_READERS_GROUP_USER_LIST>();

                foreach (UserList list in SerealizeUserlist)  //?
                {
                    DTO_READERS_GROUP_USER_LIST item = new DTO_READERS_GROUP_USER_LIST();  // 넣을 class 생성

                    item.GROUP_CODE = this.hddGroupCode.Value;
                    item.USER_ID = list.USER_ID;
                    item.CREATOR_ID = this.Sessions.UserID.ToString();
                    userlist.Add(item);
                }

                Mgr.MergeReadersGroup(userlist);
                //RadGrid1.DataSource = GroupList(group.GROUP_CODE, ""); // 그룹의 유저리스트 바인딩
                //RadGrid1.DataBind();
            }

            InitControls(); //다시 초기화 시켜준다.            
            this.lbUserList.ForeColor = System.Drawing.Color.Red;
            //this.lbUserList.Text = "(" + this.RadTbGroupName.Text + ")";
            //this.RadTbGroupName.Text = "";
            this.__VIEWSTATE.Value = "";
            this.informationMessage = "Save Completed";

        }
        catch (Exception ex)
        {
            this.errorMessage = ex.Message;
        }

    }
    #endregion

    #region [ RESET 이벤트 ]
    protected void RadBtnReset_Click(object sender, EventArgs e)
    {
        //this.RadTbGroupName.Text = "";
        GridClear();

        //GridBindData();

    }

    private void GridClear()
    {
        ViewState[VIEWSTATE_KEY] = new List<UserList>();
        this.RadGridRdsGroup.DataSource = (List<UserList>)ViewState[VIEWSTATE_KEY];
        this.RadGridRdsGroup.DataBind();
    }
    #endregion

    #region [ 신규작성 ]
    protected void RadBtnNew_Click(object sender, EventArgs e)
    {
        this.__VIEWSTATE.Value = "";
        //this.RadTbGroupName.Text = "";
        this.lbUserList.Text = "";
        this.hddState.Value = "NEW";
        GridClear();
        //GridBindData();

        // 
        RadGrid1.DataSource = string.Empty;
        RadGrid1.DataBind();
        //this.RadTbGroupName.Focus();
    }
    #endregion

    #region [ Delete Event ]
    //전체 삭제
    protected void RadBtnDel_Click(object sender, EventArgs e)
    {
        try
        {
            using (ConfigurationMgr Mgr = new ConfigurationMgr())
            {
                string Row_group_code = this.hddGroupCode.Value;
                string userid = "";
                Mgr.DeleteReadersGroup(Row_group_code, userid);
            }
            string groupCode = this.hddGroupCode.Value;
            //string groupName = this.RadTbGroupName.Text;
            //this.RadTbGroupName.Text = "";
            this.lbUserList.Text = "";
            RadGrid1.DataSource = GroupList(groupCode, "");
            RadGrid1.Rebind();

            // 실제 그룹 리스트 재 바인딩
            radGrdGroupList.DataSource = SelectGroupList();
            radGrdGroupList.DataBind();
            
            this.informationMessage = "삭제 되었습니다.";
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    // 체크박스 선택 삭제
    protected void RadBtnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            ConfigurationDao Dao = new ConfigurationDao();
            foreach (GridDataItem item in RadGrid1.MasterTableView.Items)
            {
                CheckBox checkBox = item.FindControl("chkboxbody") as CheckBox;
                if (checkBox != null && checkBox.Checked)
                {
                    string user_id = item["USER_ID"].Text;
                    string group_code = this.hddGroupCode.Value;
                    Dao.DeleteReadersGroupUser(group_code, user_id);
                }
            }
            string groupName = lbUserList.Text;
            RadGrid1.DataSource = GroupList(hddGroupCode.Value, groupName); // 그룹의 유저리스트 바인딩
            RadGrid1.Rebind();
            this.informationMessage = "삭제 돼었습니다.";


        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    // 컬럼 삭제
    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = e.Item as GridDataItem;

            using (ConfigurationDao Dao = new ConfigurationDao())
            {
                string group_code = this.hddGroupCode.Value;
                string user_id = item["USER_ID"].Text;
                Dao.DeleteReadersGroupUser(group_code, user_id);
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region 리더스 그룹 Selected Event
    protected void radGrdGroupList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (radGrdGroupList.Items.Count > 0)
        {
            this.hddState.Value = "OLD";

            GridDataItem item = this.radGrdGroupList.SelectedItems[0] as GridDataItem;
            if (item != null)
            {
                string groupName = item["DOC_NAME"].Text;
                RadGrid1.DataSource = GroupList(hddGroupCode.Value, groupName); // 그룹의 유저리스트 바인딩
                RadGrid1.Rebind();
                GridClear();
                this.lbUserList.ForeColor = System.Drawing.Color.Red;
                this.lbUserList.Text = "(" + groupName + ")";
                //this.RadTbGroupName.Text = groupName;
                this.__VIEWSTATE.Value = "";
            }
        }
    } 
    #endregion
}