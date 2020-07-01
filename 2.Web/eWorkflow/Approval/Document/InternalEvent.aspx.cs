using Bayer.eWF.BSL.Approval.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using System.Web.Script.Serialization;
using Bayer.eWF.BSL.Approval.Mgr;
using Telerik.Web.UI;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;


public partial class Approval_Document_InternalEvent : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY";
    #region OnPreInit Event
    /// <summary>
    /// 최초 페이지 로드시 호출
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        var masterPage = Master;
        this.webMaster = (masterPage as Master_eWorks_Document);


        base.OnPreInit(e);
    }
    #endregion

    #region [ Page Load ]

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL> initItem = new List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>();

                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>();
                this.radGrdCostDetail.DataSource = (List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>)ViewState[VIEWSTATE_KEY];
                this.radGrdCostDetail.DataBind();

                InitPageInfo();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void InitPageInfo()
    {
        this.hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        this.hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        this.hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        this.hddDocumentID.Value = "D0057";

        InitControls();
    }

    private void InitControls()
    {
        String sDate = DateTime.Now.ToString();
        DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

        int year = Convert.ToInt32(datevalue.Year.ToString());

        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (InternalEventMgr mgr = new InternalEventMgr())
            {
                Tuple<DTO_DOC_INTERNAL_EVENT, List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>> documents = mgr.SelectInternalEvent(this.hddProcessID.Value);

                DTO_DOC_INTERNAL_EVENT doc = documents.Item1;
                this.radTxtTitle.Text = doc.TITLE;
                if (doc.BU.Equals(this.radBtnPH.Value)) this.radBtnPH.Checked = true;
                else if (doc.BU.Equals(this.radBtnCH.Value)) this.radBtnCH.Checked = true;
                else if (doc.BU.Equals(this.radBtnCS.Value)) this.radBtnCS.Checked = true;
                else if (doc.BU.Equals(this.radBtnAH.Value)) this.radBtnAH.Checked = true;
                else if (doc.BU.Equals(this.radBtnCPL.Value)) this.radBtnCPL.Checked = true;
                                
                this.radTxtVenue.Text = doc.MEETING_VENUE;
                if (doc.FROM_DATE.HasValue)
                {
                    this.radDatFrom.SelectedDate = doc.FROM_DATE.Value.Date;
                }
                if (doc.TO_DATE.HasValue)
                {
                    this.radDateTo.SelectedDate = doc.TO_DATE.Value.Date;
                }
                this.radTxtComment.Text = doc.PURPOSE;

                //if (doc.RELEVANT_E_WORKFLOW_NO.IsNullOrEmptyEx())
                //{
                //    this.radTxtRelevant_e_WorkflowNo.Text = " ";
                //}
                //else
                //    this.radTxtRelevant_e_WorkflowNo.Text = doc.RELEVANT_E_WORKFLOW_NO;

                this.radNumExternalGuest.Text = doc.GO_NUM.ToString("#,##0");
                this.radNumBayerEmp.Text = doc.BAYER_EMPLOYEE_NUM.ToString("#,##0");
                this.lblTotalParticipants.Text = (doc.GO_NUM + doc.NON_GO_NUM + doc.FARMER_NUM + doc.BAYER_EMPLOYEE_NUM).ToString("#,##0");

                this.radGrdCostDetail.DataSource = documents.Item2;
                this.radGrdCostDetail.DataBind();
                webMaster.DocumentNo = doc.DOC_NUM;
            }
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        if (!this.hddGridItems.Value.IsNullOrEmptyEx()) UpdateGridData();
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdCostDetail);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radGrdCostDetail, this.radGrdCostDetail);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_Internal_Event_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        //if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + activityType + "');", true);


    }

    void Approval_Document_Internal_Event_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind"))
        {
            UpdateGridData();
        }

    }

    #endregion

    #region [ Add Row Event ]

    private void UpdateGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL> items = (List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>)serializer.Deserialize<List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdCostDetail.DataSource = items;
        this.radGrdCostDetail.DataBind();

    }


    #endregion

    #region [ Grid Event ]
    protected void radGrdCostDetail_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateGridData();
            int index = Convert.ToInt32(e.CommandArgument);

            using (AHPIActivityMgr mgr = new AHPIActivityMgr())
            {
                mgr.DeleteAHPIActivityCostDetail(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL> list = (List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdCostDetail.DataSource = list;
            this.radGrdCostDetail.DataBind();
        }
    }
    #endregion

    #region [ 문서상단 버튼 ]

    private string GetDivision()
    {
        string rtnValue = string.Empty;
        if (this.radBtnPH.Checked) rtnValue = this.radBtnPH.Value;
        else if (this.radBtnCH.Checked) rtnValue = this.radBtnCH.Value;
        else if (this.radBtnCS.Checked) rtnValue = this.radBtnCS.Value;
        else if (this.radBtnAH.Checked) rtnValue = this.radBtnCH.Value;
        else if (this.radBtnCPL.Checked) rtnValue = this.radBtnCPL.Value;

        return rtnValue;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();

        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (this.radTxtTitle.Text.IsNullOrEmptyEx())
                message += "Subject";
            if (GetDivision().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "BU";
            if (this.radTxtVenue.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Venue";
            //if (this.radBtnIncentive.Checked == true && this.radChkIncentiveAgree.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Incentive Agreement";
            //if (this.radBtnIncentive.Checked == true && radChkIncentiveCheck.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Incentive Checklist";
            //if (this.radTxtMeetingVenue.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting venue";
            if (!this.radDatFrom.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "From Date";
            //if (!this.radTimeFromMeeting.SelectedTime.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Form Time";
            //if (!this.radDateTo.SelectedDate.HasValue)
            //message += (message.IsNullOrEmptyEx() ? "" : ",") + "To Date";
            //if (!this.radTimeToMeeting.SelectedTime.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "To Time";

            //if (this.radTxtAddressVenue.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Address venue";
            if (this.radDateTo.SelectedDate.HasValue) {
                if (DateTime.Compare(this.radDatFrom.SelectedDate.Value, this.radDateTo.SelectedDate.Value) > 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "From Date cannot be earlier than End Date";
            }
            List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL> details = (List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>)ViewState[VIEWSTATE_KEY];
            bool hasDesc = true;
            foreach (DTO_DOC_INTERNAL_EVENT_COST_DETAIL detail in details)
            {
                if (detail.DESCRIPTION.IsNullOrEmptyEx())
                {
                    hasDesc = false;
                    break;
                }
            }
            if (details.Count() == 0)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Category";
            }
            if (!hasDesc)
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Category description";
            }
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    int StringToInt(string text)
    {
        int value = 0;
        text = text.Replace(",", "");
        if (text.Trim().Length > 0) value = Convert.ToInt32(text);
        return value;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_INTERNAL_EVENT doc = new DTO_DOC_INTERNAL_EVENT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = this.radTxtTitle.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TITLE = this.radTxtTitle.Text;
        doc.BU = GetDivision();

        String sDate = DateTime.Now.ToString();
        DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

        int year = Convert.ToInt32(datevalue.Year.ToString());

        doc.INCENTIVE_AGREEMENT = null;
        doc.INCENTIVE_CHECKLIST = null;

        doc.COST_CENTER = null;
        doc.MEETING_VENUE = this.radTxtVenue.Text;

        if (this.radDatFrom.SelectedDate.HasValue)
        {
            DateTime dt = this.radDatFrom.SelectedDate.Value.Date;
            doc.FROM_DATE = dt;
            doc.TO_DATE = dt;
        }

        if (this.radDateTo.SelectedDate.HasValue)
        {
            DateTime dt = this.radDateTo.SelectedDate.Value.Date;
            doc.TO_DATE = dt;
        }

        doc.ADDRESS_VENUE = null;
        doc.PURPOSE = this.radTxtComment.Text;
        doc.GO_NUM = StringToInt(this.radNumExternalGuest.Text);
        doc.NON_GO_NUM = 0;
        doc.FARMER_NUM = 0;
        doc.BAYER_EMPLOYEE_NUM = StringToInt(this.radNumBayerEmp.Text);

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL> details = (List<DTO_DOC_INTERNAL_EVENT_COST_DETAIL>)ViewState[VIEWSTATE_KEY];
        decimal? total = 0;
        foreach (DTO_DOC_INTERNAL_EVENT_COST_DETAIL detail in details)
        {
            total += (detail.AMOUNT.HasValue ? detail.AMOUNT : 0);
            detail.PROCESS_ID = this.hddProcessID.Value;
            detail.CREATOR_ID = Sessions.UserID;
        }

        doc.EXISTS_PROMOTIONAL = "N";
        doc.TOTAL_AMOUNT = total;
        using (InternalEventMgr mgr = new InternalEventMgr())
        {
            return mgr.MergeInternalEvent(doc, details);
        }
    }

    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoSave();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    protected override void DoRequest()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Request))
            {
                hddProcessID.Value = SaveDocument(hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved : ApprovalUtil.ApprovalStatus.Temp);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }

        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #endregion
}