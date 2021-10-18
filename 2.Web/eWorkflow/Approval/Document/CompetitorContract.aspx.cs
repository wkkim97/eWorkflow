using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Common.Dto;

public partial class Approval_Document_CompetitorContract : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
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
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        this.hddDocumentID.Value = "D0017";
        //this.hddProcessID.Value = "P000000952";

        InitControls();

    }

    private void InitControls()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {

            this.radDdlHost.DataSource = mgr.SelectMembershipList("");
            this.radDdlHost.DataBind();
        }

        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (CompetitorContractMgr mgr = new CompetitorContractMgr())
            {
                DTO_DOC_COMPETITOR_CONTRACT doc = mgr.SelectCompetitorContract(this.hddProcessID.Value);

                if (doc != null)
                {
                    this.radDdlCategory.SelectedValue = doc.CATEGORY_CODE;

                    if (this.radBtnWhitelisted.Value == doc.TA_CATEGORY) { this.radBtnWhitelisted.Checked = true; }
                    else if (this.radBtnNon_Whitelisted.Value == doc.TA_CATEGORY) { this.radBtnNon_Whitelisted.Checked = true; }

                    this.radDdlHost.SelectedValue = doc.HOST_CODE;
                    this.radTxtCounterParty.Text = doc.COUNTER_PARTY;
                    this.radTxtContractNo.Text = doc.CONTRACT_NUM;
                    this.radTxtContractPeriod.Text = doc.CONTRACT_PERIOD;
                    this.radDatMeeting.SelectedDate = doc.MEETING_DATETIME;
                    this.radTxtVenue.Text = doc.VENUE;
                    this.radTxtAgenda.Text = doc.AGENDA;
                    this.radTxtProduct.Text = doc.PRODUCT;
                    this.radTxtActivity.Text = doc.ACTIVITY;

                    if (doc.MEETING_DATETIME.IsNotNullOrEmptyEx())
                    {
                        this.radDatMeeting.SelectedDate = doc.MEETING_DATETIME;
                        if (doc.FROM_TIME.IsNotNullOrEmptyEx())
                        {
                            this.radTimStartMeeting.SelectedDate = doc.MEETING_DATETIME;
                            this.radTimStartMeeting.SelectedTime = new TimeSpan(doc.FROM_TIME.Value.Hour, doc.FROM_TIME.Value.Minute, 0);
                        }

                        if (doc.TO_TIME.IsNotNullOrEmptyEx())
                        {
                            this.radTimFinishMeeting.SelectedDate = doc.MEETING_DATETIME;
                            this.radTimFinishMeeting.SelectedTime = new TimeSpan(doc.TO_TIME.Value.Hour, doc.TO_TIME.Value.Minute, 0);
                        }
                    }

                    if (doc.PURPOSE_CODE.IsNotNullOrEmptyEx())
                    {
                        foreach (string purpose in doc.PURPOSE_CODE.Split(new char[] { ';' }))
                        {
                            foreach (Control control in this.divPurpose.Controls)
                            {
                                if (control is RadButton)
                                {
                                    if ((control as RadButton).Value.Equals(purpose))
                                    {
                                        (control as RadButton).Checked = true; break;
                                    }
                                }
                            }
                        }
                    }

                    this.radTxtOtherPurpose.Text = doc.OTHER_PURPOSE;
                    this.radTxtExternal.Text = doc.EXTERNAL_PARTICIPANTS;
                    webMaster.DocumentNo = doc.DOC_NUM;
                }

                //참석자 설정
                List<DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT> participants = mgr.SelectCompetitorContractParticipants(this.hddProcessID.Value);
                foreach (DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT participant in participants)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Text = participant.PARTICIPANT_NAME;
                    if (participant.PARTICIPANT_TYPE.Equals(CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1)))
                    {
                        SmallUserInfoDto value = new SmallUserInfoDto();
                        value.USER_ID = participant.PARTICIPANT_CODE;
                        value.FULL_NAME = participant.PARTICIPANT_NAME;
                        entry.Value = JsonConvert.toJson<SmallUserInfoDto>(value);
                    }
                    this.radAcomParticipants.Entries.Add(entry);
                }
            }
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + this.radDdlCategory.SelectedValue + "');", true);
    }


    #region [ 상단 버튼 ]

    private bool IsCheckedPurpose()
    {
        foreach (Control control in this.divPurpose.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked) return true;
            }
        }
        return false;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (this.radDdlCategory.SelectedValue.IsNullOrEmptyEx())
                message += "Category";

            switch (this.radDdlCategory.SelectedValue)
            {
                case "0001":
                    if (this.radBtnWhitelisted.Checked == false && this.radBtnNon_Whitelisted.Checked == false)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "TA Category";
                    if (!this.radDatMeeting.SelectedDate.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Date";
                    if(!this.radTimStartMeeting.SelectedTime.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Start Time";

                    if (this.radTxtVenue.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Venue";

                    if (this.radTxtAgenda.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Agenda";
                    //if (this.radAcomParticipants.Entries.Count < 1)
                    //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Participants";

                    //if (!IsCheckedPurpose())
                    //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
                    break;
                case "0002":
                    if (this.radTxtCounterParty.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Counter Party";

                    if (!this.radDatMeeting.SelectedDate.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Date";

                    if (!this.radTimStartMeeting.SelectedTime.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Start Time";

                    if (this.radTxtVenue.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Venue";

                    if (this.radTxtAgenda.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Agenda";

                    break;
                case "0003":
                    if (this.radTxtCounterParty.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Counter Party";

                    if (this.radTxtContractNo.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract No.";

                    //if (this.radTxtContractPeriod.Text.IsNullOrEmptyEx())
                    //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract Period";

                    //if (this.radAcomParticipants.Entries.Count < 1)
                    //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Participants";

                    if (this.radTxtProduct.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Product";

                    if (this.radTxtActivity.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Activity";
                    break;
                default:
                    if (this.radTxtCounterParty.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Counter Party";

                    if (!this.radDatMeeting.SelectedDate.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Date";

                    if (!this.radTimStartMeeting.SelectedTime.HasValue)
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Start Time";

                    if (this.radTxtVenue.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Venue";

                    //if (this.radAcomParticipants.Entries.Count < 1)
                    //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Participants";

                    if (this.radTxtAgenda.Text.IsNullOrEmptyEx())
                        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Agenda";

                    break;
            }

        }
        else if (status == ApprovalUtil.ApprovalStatus.Saved)
        {
            if (this.radDdlCategory.SelectedValue.IsNullOrEmptyEx())
                message += "Category";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private void SetMeetingDateTime(DTO_DOC_COMPETITOR_CONTRACT doc)
    {
        DateTime dtMeeting = this.radDatMeeting.SelectedDate.Value.Date;
        doc.MEETING_DATETIME = dtMeeting;
        TimeSpan? timeFrom = this.radTimStartMeeting.SelectedTime;
        if (timeFrom != null) doc.FROM_TIME = new DateTime(dtMeeting.Year, dtMeeting.Month, dtMeeting.Day, timeFrom.Value.Hours, timeFrom.Value.Minutes, 0);
        TimeSpan? timeTo = this.radTimFinishMeeting.SelectedTime;
        if (timeTo != null) doc.TO_TIME = new DateTime(dtMeeting.Year, dtMeeting.Month, dtMeeting.Day, timeTo.Value.Hours, timeTo.Value.Minutes, 0);

    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_COMPETITOR_CONTRACT doc = new DTO_DOC_COMPETITOR_CONTRACT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        if (this.radDdlCategory.SelectedIndex == 0)
        {
            if (this.radBtnNon_Whitelisted.Checked)
                doc.TA_CATEGORY = radBtnNon_Whitelisted.Value;
            else if (this.radBtnWhitelisted.Checked)
                doc.TA_CATEGORY = radBtnWhitelisted.Value;

            doc.SUBJECT = this.radDdlCategory.SelectedText + " [ " + doc.TA_CATEGORY +" ] " + this.radDdlHost.SelectedText;
        }
        else
        {
            doc.SUBJECT = this.radDdlCategory.SelectedText + " / " + this.radTxtCounterParty.Text;
        }
        webMaster.Subject = doc.SUBJECT;       
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CATEGORY_CODE = this.radDdlCategory.SelectedValue;
        switch (this.radDdlCategory.SelectedValue)
        {
            case "0001":
                doc.HOST_CODE = this.radDdlHost.SelectedValue;
                if (this.radDatMeeting.SelectedDate.HasValue)
                    SetMeetingDateTime(doc);
                doc.VENUE = this.radTxtVenue.Text;
                doc.AGENDA = this.radTxtAgenda.Text;
                break;
            case "0002":
                doc.COUNTER_PARTY = this.radTxtCounterParty.Text;
                if (this.radDatMeeting.SelectedDate.HasValue)
                    SetMeetingDateTime(doc);
                doc.VENUE = this.radTxtVenue.Text;
                doc.AGENDA = this.radTxtAgenda.Text;
                break;
            case "0003":
                doc.COUNTER_PARTY = this.radTxtCounterParty.Text;
                doc.CONTRACT_NUM = this.radTxtContractNo.Text;
                doc.CONTRACT_PERIOD = this.radTxtContractPeriod.Text;
                doc.PRODUCT = this.radTxtProduct.Text;
                doc.ACTIVITY = this.radTxtActivity.Text;
                break;
            case "0004":
                doc.COUNTER_PARTY = this.radTxtCounterParty.Text;
                if (this.radDatMeeting.SelectedDate.HasValue)
                    SetMeetingDateTime(doc);
                doc.VENUE = this.radTxtVenue.Text;
                doc.AGENDA = this.radTxtAgenda.Text;
                break;
            default:
                doc.HOST_CODE = this.radDdlHost.SelectedValue;
                if (this.radDatMeeting.SelectedDate.HasValue)
                    SetMeetingDateTime(doc);
                doc.VENUE = this.radTxtVenue.Text;
                doc.AGENDA = this.radTxtAgenda.Text;
                break;
        }

        string purpose = string.Empty;
        foreach (Control control in divPurpose.Controls)
        {
            if (control is Telerik.Web.UI.RadButton)
            {
                if ((control as Telerik.Web.UI.RadButton).Checked)
                    purpose += (control as Telerik.Web.UI.RadButton).Value + ";";
            }
        }
        if (purpose.Length < 2)
            purpose = null;
        doc.PURPOSE_CODE = purpose;
        doc.OTHER_PURPOSE = this.radTxtOtherPurpose.Text;
        doc.EXTERNAL_PARTICIPANTS = this.radTxtExternal.Text;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;


        List<DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT> participants = new List<DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomParticipants.Entries)
        {
            DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT participant = new DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT();
            participant.PROCESS_ID = this.hddProcessID.Value;

            if (entry.Value.NullObjectToEmptyEx().Length > 0)
            {
                SmallUserInfoDto dto = JsonConvert.JsonDeserialize<SmallUserInfoDto>(entry.Value);
                participant.PARTICIPANT_CODE = dto.USER_ID;
                participant.PARTICIPANT_NAME = dto.FULL_NAME;
                participant.PARTICIPANT_TYPE = CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1);
            }
            else
            {
                participant.PARTICIPANT_CODE = entry.Text;
                participant.PARTICIPANT_NAME = entry.Text;
                participant.PARTICIPANT_TYPE = CommonEnum.MailAddressType.External.ToString().Substring(0, 1);
            }

            participant.CREATOR_ID = Sessions.UserID;
            participants.Add(participant);
        }

        using (CompetitorContractMgr mgr = new CompetitorContractMgr())
        {
            return mgr.MergeCompetitorContract(doc, participants);
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
    protected void radDdlCategory_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        //-2014.12.13 Internal/External 분리후 체거
        //this.radAcomParticipants.Entries.Clear();
        //if (e.Index == 1) this.radAcomParticipants.AllowCustomEntry = true;
        //else this.radAcomParticipants.AllowCustomEntry = false;
    }
}