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


public partial class Approval_Document_PIActivity : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
                List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL> initItem = new List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>();

                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>();
                this.radGrdCostDetail.DataSource = (List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>)ViewState[VIEWSTATE_KEY];
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
        this.hddDocumentID.Value = "D0036";
        //this.HddProcessID.Value = "P000000982";

        InitControls();
    }

    private void InitControls()
    {
        String sDate = DateTime.Now.ToString();
        DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

        int year = Convert.ToInt32(datevalue.Year.ToString());

        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr commonmgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            List<DTO_COMMON_PIACTIVITY> common = commonmgr.SelectCommonPIActivity(year);
            this.radDropCostCenter.DataSource = common;
            this.radDropCostCenter.DataBind();
            if (this.radDropCostCenter.Items.Count > 0) this.radDropCostCenter.SelectedIndex = 0;
        }

        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (PIActivityMgr mgr = new PIActivityMgr())
            {
                Tuple<DTO_DOC_P_I_ACTIVITY, List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>> documents = mgr.SelectPIActivity(this.hddProcessID.Value);

                DTO_DOC_P_I_ACTIVITY doc = documents.Item1;
                this.radTxtTitle.Text = doc.TITLE;
                if (doc.BU.Equals(this.radBtnCP.Value)) this.radBtnCP.Checked = true;
                else if (doc.BU.Equals(this.radBtnIS.Value)) this.radBtnIS.Checked = true;
                else if (doc.BU.Equals(this.radBtnBVS.Value))
                { this.radBtnBVS.Checked = true;
                    
                    radBtnBVS.Checked = true;
                    radBtnBVS.Visible = true;
                }
                else if (doc.BU.Equals(this.radBtnES.Value)) this.radBtnES.Checked = true;
                else if (doc.BU.Equals(this.radBtnMKR.Value)) this.radBtnMKR.Checked = true;

                //eWorkflow Optimization 2020
                if (doc.REQUEST_TYPE.Equals(this.radBtnRequestTypeActivity.Value)) this.radBtnRequestTypeActivity.Checked = true;
                else if (doc.REQUEST_TYPE.Equals(this.radBtnRequestTypePlan.Value)) this.radBtnRequestTypePlan.Checked = true;
                //eWorkflow Optimization 2020

                if (doc.ACTIVITY_TYPE.Equals(this.radBtnProfessional.Value)) this.radBtnProfessional.Checked = true;
                else if (doc.ACTIVITY_TYPE.Equals(this.radBtnIncentive.Value)) this.radBtnIncentive.Checked = true;
                else if (doc.ACTIVITY_TYPE.Equals(this.radBtnProduct.Value)) this.radBtnProduct.Checked = true;
                else if (doc.ACTIVITY_TYPE.Equals(this.radBtnCampaign.Value)) this.radBtnCampaign.Checked = true;
                else if (doc.ACTIVITY_TYPE.Equals(this.radBtnTradePromotion.Value)) this.radBtnTradePromotion.Checked = true;

                if (doc.ACTIVITY_TYPE.Equals("IP"))
                {
                    this.radChkIncentiveAgree.Checked = doc.INCENTIVE_AGREEMENT == "Y" ? true : false;
                    this.radChkIncentiveCheck.Checked = doc.INCENTIVE_CHECKLIST == "Y" ? true : false;
                }
                this.radDropCostCenter.SelectedValue = doc.COST_CENTER;
                this.radTxtMeetingVenue.Text = doc.MEETING_VENUE;
                this.radTxtAddressVenue.Text = doc.ADDRESS_VENUE;
                if (doc.FROM_DATE.HasValue)
                {
                    this.radDatFromMeeting.SelectedDate = doc.FROM_DATE.Value.Date;
                    this.radTimeFromMeeting.SelectedDate = doc.FROM_DATE.Value;
                }
                if (doc.TO_DATE.HasValue)
                {
                    this.radDateToMeeting.SelectedDate = doc.TO_DATE.Value.Date;
                    this.radTimeToMeeting.SelectedDate = doc.TO_DATE.Value;
                }
                this.radTxtPurpose.Text = doc.PURPOSE;
                if (doc.RELEVANT_E_WORKFLOW_NO.IsNullOrEmptyEx())
                {
                    this.radTxtRelevant_e_WorkflowNo.Text = " ";
                }
                else
                    this.radTxtRelevant_e_WorkflowNo.Text = doc.RELEVANT_E_WORKFLOW_NO;

                this.radNumGO.Text = doc.GO_NUM.ToString("#,##0");
                this.radNumNonGO.Text = doc.NON_GO_NUM.ToString("#,##0");
                this.radNumFarmer.Text = doc.FARMER_NUM.ToString("#,##0");
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
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_PIActivity_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

        string activityType = this.radBtnIncentive.Checked ? this.radBtnIncentive.Value : "0000";
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + activityType + "');", true);


    }

    void Approval_Document_PIActivity_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
        List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL> items = (List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>)serializer.Deserialize<List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>>(this.hddGridItems.Value);

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

            using (PIActivityMgr mgr = new PIActivityMgr())
            {
                mgr.DeletePIActivityCostDetail(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL> list = (List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdCostDetail.DataSource = list;
            this.radGrdCostDetail.DataBind();
        }
    }
    #endregion

    #region [ 문서상단 버튼 ]

    private string GetBU()
    {
        string rtnValue = string.Empty;

        if (this.radBtnCP.Checked) rtnValue = this.radBtnCP.Value;
        else if (this.radBtnIS.Checked) rtnValue = this.radBtnIS.Value;
        else if (this.radBtnBVS.Checked) rtnValue = this.radBtnBVS.Value;
        else if (this.radBtnES.Checked) rtnValue = this.radBtnES.Value;
        else if (this.radBtnMKR.Checked) rtnValue = this.radBtnMKR.Value;

        return rtnValue;
    }
    //eWorkflow Optimization 2020
    private string GetRequestType()
    {
        string rtnValue = string.Empty;

        if (this.radBtnRequestTypeActivity.Checked) rtnValue = this.radBtnRequestTypeActivity.Value;
        else if (this.radBtnRequestTypePlan.Checked) rtnValue = this.radBtnRequestTypePlan.Value;

        return rtnValue;
    }
    //eWorkflow Optimization 2020

    private string GetActivityType()
    {
        string rtnValue = string.Empty;

        if (this.radBtnProfessional.Checked) rtnValue = this.radBtnProfessional.Value;
        else if (this.radBtnIncentive.Checked) rtnValue = this.radBtnIncentive.Value;
        else if (this.radBtnProduct.Checked) rtnValue = this.radBtnProduct.Value;
        else if (this.radBtnCampaign.Checked) rtnValue = this.radBtnCampaign.Value;
        else if (this.radBtnTradePromotion.Checked) rtnValue = this.radBtnTradePromotion.Value;

        return rtnValue;
    }

    
    private string GetActivityTypeText()
    {
        string rtnValue = string.Empty;

        if (this.radBtnProfessional.Checked) rtnValue = this.radBtnProfessional.Text;
        else if (this.radBtnIncentive.Checked) rtnValue = this.radBtnIncentive.Text;
        else if (this.radBtnProduct.Checked) rtnValue = this.radBtnProduct.Text;
        else if (this.radBtnCampaign.Checked) rtnValue = this.radBtnCampaign.Text;

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
            if (GetBU().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "BG";
            if (GetActivityType().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Activity Type";
            //eWorkflow Optimization 2020
            if (GetRequestType().IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Reqeust Type";
            //eWorkflow Optimization 2020

            //if (this.radBtnIncentive.Checked == true && this.radChkIncentiveAgree.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Incentive Agreement";
            //if (this.radBtnIncentive.Checked == true && radChkIncentiveCheck.Checked == false)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Incentive Checklist";
            if (radDropCostCenter.SelectedIndex == -1)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Cost Center & Approval Budget";
            //if (this.radTxtMeetingVenue.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting venue";
            //if (!this.radDatFromMeeting.SelectedDate.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "From Date";
            //if (!this.radTimeFromMeeting.SelectedTime.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Form Time";
            //if (!this.radDateToMeeting.SelectedDate.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "To Date";
            //if (!this.radTimeToMeeting.SelectedTime.HasValue)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "To Time";

            //if (this.radTxtAddressVenue.Text.Length <= 0)
            //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Address venue";
            if (this.radTxtPurpose.Text.Length <= 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose / Objective";
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
        DTO_DOC_P_I_ACTIVITY doc = new DTO_DOC_P_I_ACTIVITY();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = this.radTxtTitle.Text + "/" + GetBU() + "/" + GetActivityTypeText();
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TITLE = this.radTxtTitle.Text;
        doc.BU = GetBU();
        doc.ACTIVITY_TYPE = GetActivityType();
        //eWorkflow Optimization 2020
        doc.REQUEST_TYPE = GetRequestType();
        //eWorkflow Optimization 2020

        String sDate = DateTime.Now.ToString();
        DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

        int year = Convert.ToInt32(datevalue.Year.ToString());

        if (doc.ACTIVITY_TYPE == "IP")
        {
            doc.INCENTIVE_AGREEMENT = this.radChkIncentiveAgree.Checked ? "Y" : "N";
            doc.INCENTIVE_CHECKLIST = this.radChkIncentiveCheck.Checked ? "Y" : "N";
        }
        else
        {
            doc.INCENTIVE_AGREEMENT = null;
            doc.INCENTIVE_CHECKLIST = null;
        }


        doc.COST_CENTER = this.radDropCostCenter.SelectedValue;
        doc.MEETING_VENUE = this.radTxtMeetingVenue.Text;
        if (this.radDatFromMeeting.SelectedDate.HasValue)
        {
            DateTime dt = this.radDatFromMeeting.SelectedDate.Value.Date;

            if (this.radTimeFromMeeting.SelectedDate.HasValue)
                doc.FROM_DATE = new DateTime(dt.Year, dt.Month, dt.Day, this.radTimeFromMeeting.SelectedDate.Value.Hour, this.radTimeFromMeeting.SelectedDate.Value.Minute, 0);
            else
                doc.FROM_DATE = dt;

        }

        if (this.radDateToMeeting.SelectedDate.HasValue)
        {
            DateTime dt = this.radDateToMeeting.SelectedDate.Value.Date;

            // if (this.radDateToMeeting.SelectedDate.HasValue)
            if (this.radTimeToMeeting.SelectedDate.HasValue)
                doc.TO_DATE = new DateTime(dt.Year, dt.Month, dt.Day, this.radTimeToMeeting.SelectedDate.Value.Hour, this.radTimeToMeeting.SelectedDate.Value.Minute, 0);
            else
                doc.TO_DATE = dt;

        }

        doc.ADDRESS_VENUE = this.radTxtAddressVenue.Text;
        doc.PURPOSE = this.radTxtPurpose.Text;
        doc.RELEVANT_E_WORKFLOW_NO = this.radTxtRelevant_e_WorkflowNo.Text;
        doc.GO_NUM = StringToInt(this.radNumGO.Text);
        doc.NON_GO_NUM = StringToInt(this.radNumNonGO.Text);
        doc.FARMER_NUM = StringToInt(this.radNumFarmer.Text);
        doc.BAYER_EMPLOYEE_NUM = StringToInt(this.radNumBayerEmp.Text);

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL> details = (List<DTO_DOC_P_I_ACTIVITY_COST_DETAIL>)ViewState[VIEWSTATE_KEY];
        bool existsPromotional = false;
        decimal? total = 0;
        foreach (DTO_DOC_P_I_ACTIVITY_COST_DETAIL detail in details)
        {
            if (detail.CATEGORY_CODE == "0004" && detail.UNIT_PRICE >=100000)
                existsPromotional = true;
            total += (detail.AMOUNT.HasValue ? detail.AMOUNT : 0);
            detail.PROCESS_ID = this.hddProcessID.Value;
            detail.CREATOR_ID = Sessions.UserID;
        }

        if (existsPromotional) doc.EXISTS_PROMOTIONAL = "Y";
        else doc.EXISTS_PROMOTIONAL = "N";

        doc.TOTAL_AMOUNT = total;
        using (PIActivityMgr mgr = new PIActivityMgr())
        {
            return mgr.MergePIActivity(doc, details);
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