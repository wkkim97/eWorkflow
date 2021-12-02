using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data;

public partial class Approval_Common_PopupAbsence : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.SelectAbsenceList();
        }
    }

    private void SelectAbsenceList()
    {
        SelectAbsenceList(true);
        using (UserMgr mgr = new UserMgr())
        {
            List<SmallUserInfoDto> users = mgr.SelectDelegationUserList(Sessions.UserID);
            this.radDduserlist.DataSource = users;
            this.radDduserlist.DataBind();
            //if (this.radDduserlist.Items.Count > 0) this.radDduserlist.SelectedIndex = 0;
        }
    }

    private void SelectAbsenceList(bool isInit)
    {
        try
        {
            using (AbsenceMgr mgr = new AbsenceMgr())
            {
                List<DTO_ABSENCE> list = mgr.SelectAbsenceList(Sessions.UserID);

                this.RadAbsenceList.DataSource = list;
                if (isInit)
                    this.RadAbsenceList.DataBind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }

    }

    protected void radBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int index = 0;
            if (string.IsNullOrEmpty(this.hddIndex.Value) || this.hddIndex.Value.Equals("0"))
                index = 0;
            else
                index = Convert.ToInt32(this.hddIndex.Value);
            if (index == 0)
            {
                using (Bayer.eWF.BSL.Approval.Mgr.AbsenceMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.AbsenceMgr())
                {
                    DTO_ABSENCE absence = new DTO_ABSENCE();
                    absence.USER_ID = Sessions.UserID;
                    absence.APPROVER_ID = this.radDduserlist.SelectedValue;
                    absence.APPROVER_NAME = this.radDduserlist.SelectedText;
                    absence.FROM_DATE = this.radFromDate.SelectedDate;
                    absence.TO_DATE = this.radToDate.SelectedDate;
                    absence.DESCRIPTION = this.radtxtDescription.Text;
                    mgr.InsertAbsence(absence);
                }
            }
            else if (index > 0)
            {
                using (Bayer.eWF.BSL.Approval.Mgr.AbsenceMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.AbsenceMgr())
                {
                    DTO_ABSENCE absence = new DTO_ABSENCE();
                    absence.IDX = index;
                    absence.APPROVER_ID = this.radDduserlist.SelectedValue;
                    absence.APPROVER_NAME = this.radDduserlist.SelectedText;
                    absence.FROM_DATE = this.radFromDate.SelectedDate;
                    absence.TO_DATE = this.radToDate.SelectedDate;
                    absence.DESCRIPTION = this.radtxtDescription.Text;
                    mgr.UpdateAbsence(absence);
                }
            }
            this.informationMessage = "저장 되었습니다.";
            RadAbsenceList.MasterTableView.SortExpressions.Clear();
            RadAbsenceList.MasterTableView.GroupByExpressions.Clear();
            RadAbsenceList.Rebind();
            InitScreen();
        }

        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }


    protected void RadAbsenceList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.RadAbsenceList.SelectedItems.Count > 0)
        {
            GridDataItem item = this.RadAbsenceList.SelectedItems[0] as GridDataItem;
            this.hddIndex.Value = item["IDX"].Text;

            int index = Convert.ToInt32(this.hddIndex.Value);


            using (AbsenceMgr mgr = new AbsenceMgr())
            {
                DTO_ABSENCE absence = mgr.SelectAbsence(index);

                if (absence != null)
                {
                    this.radDduserlist.SelectedValue = absence.APPROVER_ID;
                    this.radFromDate.SelectedDate = absence.FROM_DATE;
                    this.radToDate.SelectedDate = absence.TO_DATE;
                    this.radtxtDescription.Text = absence.DESCRIPTION;
                }
                else
                {
                    this.informationMessage = "";
                }
            }
        }
    }
    protected void RadAbsenceList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        this.SelectAbsenceList(false);
    }

    private void InitScreen()
    {
        this.radDduserlist.SelectedIndex = -1;
        this.radFromDate.SelectedDate = null;
        this.radToDate.SelectedDate = null;
        this.radtxtDescription.Text = "";
    }


    protected void radBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            GridDataItem item = this.RadAbsenceList.SelectedItems[0] as GridDataItem;

            int index = 0;
            if (string.IsNullOrEmpty(this.hddIndex.Value) || this.hddIndex.Value.Equals("0"))
                index = 0;
            else
                index = Convert.ToInt32(this.hddIndex.Value);
            if (index > 0)
            {

                using (Bayer.eWF.BSL.Approval.Mgr.AbsenceMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.AbsenceMgr())
                {
                    DTO_ABSENCE absence = new DTO_ABSENCE();
                    mgr.DeleteAbsence(index);

                    InitScreen();
                }
                RadAbsenceList.MasterTableView.SortExpressions.Clear();
                RadAbsenceList.MasterTableView.GroupByExpressions.Clear();
                RadAbsenceList.Rebind();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    protected void radBtnReset_Click(object sender, EventArgs e)
    {
        InitScreen();
    }
}