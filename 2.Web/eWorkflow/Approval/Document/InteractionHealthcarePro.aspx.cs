using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Approval.Dto;
using System.Web.Script.Serialization;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_InteractionHealthcarePro : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_KEY = "VIEWSTATE_KEY_INTERACTION_HEALTHCARE_PRO";

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

    protected override void OnPreRender(EventArgs e)
    {
        if (!webMaster.DocumentNo.Equals(string.Empty))
        {
            webMaster.SetEnableControls(divYourDoces, true);
        }

        base.OnPreRender(e);
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                ViewState[VIEWSTATE_KEY] = new List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>();
                this.radGrdCostDetail.DataSource = (List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>)ViewState[VIEWSTATE_KEY];
                this.radGrdCostDetail.DataBind();
                this.radDdlCategory.SelectedIndex = 0;
                InitPageInfo();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    void Approval_Document_InteractionHealthcarePro_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Rebind"))
        {
            UpdateGridData();
            this.hddAddRow.Value = "Y";
        }
        else
            this.hddAddRow.Value = "N";
    }

    private void InitPageInfo()
    {
        this.hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        this.hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        this.hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        this.hddDocumentID.Value = "D0021";
        //this.hddProcessID.Value = "P000001503";

        InitControls();

    }

    private void InitControls()
    {
        if (!hddProcessID.Value.IsNullOrEmptyEx())
        {
            string processId = this.hddProcessID.Value;
            using (InteractionHealthcareProMgr mgr = new InteractionHealthcareProMgr())
            {
                Tuple<DTO_DOC_INTERACTION_HEALTHCARE_PRO
                , List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY>> doc = mgr.SelectInteractionHealthcarePro(processId);

                this.radTxtSubject.Text = doc.Item1.TITLE;
                this.radDdlActivity.SelectedValue = doc.Item1.ACTIVITY_CODE;
                this.radDatFromEvent.SelectedDate = doc.Item1.FROM_EVENT_DATE;
                this.radTimFromEvent.SelectedTime = doc.Item1.FROM_EVENT_DATE.HasValue ? doc.Item1.FROM_EVENT_DATE.Value.TimeOfDay : (TimeSpan?)null;
                this.radDatToEvent.SelectedDate = doc.Item1.TO_EVENT_DATE;
                this.radTimToEvent.SelectedTime = doc.Item1.TO_EVENT_DATE.HasValue ? doc.Item1.TO_EVENT_DATE.Value.TimeOfDay : (TimeSpan?)null;
                this.radTxtMeetingVenue.Text = doc.Item1.MEETING_VENUE;
                this.radTxtAddressOfVenue.Text = doc.Item1.ADDRESS_OF_VENUE;
                this.radTxtVenueSelectionReason.Text = doc.Item1.VENUE_SELECTION_REASON_ETC;

                foreach (string reason in doc.Item1.VENUE_SELECTION_REASON.Split(new char[] { ';' }))
                {
                    foreach (Control control in this.divReason.Controls)
                    {
                        if (control is RadButton)
                        {
                            if ((control as RadButton).Value.Equals(reason))
                            {
                                (control as RadButton).Checked = true; break;
                            }
                        }
                    }
                }

                foreach (string purpose in doc.Item1.PURPOSE_OBJECT.Split(new char[] { ';' }))
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

                if (!doc.Item1.DOC_NUM.Equals(string.Empty))
                {
                    divYourDoces.Attributes.CssStyle.Add("display", "block");
                }

                this.radTxtScientificMaterial.Text = doc.Item1.SCIENTIFIC_MATERIAL;

                this.radTxtCompound.Text = doc.Item1.COMPOUNT_STUDY_NUM;
                this.radTxtSponsor.Text = doc.Item1.SPONSOR_OF_STUDY;
                this.radTxtCROName.Text = doc.Item1.CRO_NAME;
                this.radTxtObjective.Text = doc.Item1.OBJECTIVE;

                if (doc.Item1.IS_INVESTIGATOR_MEETING.Equals("Y")) this.radRdoInvestigatorYes.Checked = true;
                else this.radRdoInvestigatorNo.Checked = true;

                if (doc.Item1.IS_MONITORING_MEETING.Equals("Y")) this.radRdoMonitoringYes.Checked = true;
                else this.radRdoMonitoringNo.Checked = true;

                this.radTxtOtherspecify.Text = doc.Item1.OTHER_SPECIFY;

                if (doc.Item1.IS_CONTRACT.Equals("Y")) this.radRdoContractYes.Checked = true;
                else this.radRdoContractNo.Checked = true;

                this.radTxtTargetParticipants.Text = doc.Item1.TARGET_PARTICIPANTS;
                this.radNumGO.Text = doc.Item1.GO_NUM.ToString("#,##0");
                this.radNumNonGO.Text = doc.Item1.NON_GO_NUM.ToString("#,##0");
                this.radNumPrivate.Text = doc.Item1.PRIVATE_NUM.ToString("#,##0");
                this.radNumForeignHCP.Text = doc.Item1.FOREIGN_NUM.ToString("#,##0");
                this.radNumBayer.Text = doc.Item1.BAYER_NUM.ToString();
                this.lblTotalParticipants.Text = (doc.Item1.GO_NUM + doc.Item1.NON_GO_NUM + doc.Item1.PRIVATE_NUM + doc.Item1.FOREIGN_NUM + doc.Item1.BAYER_NUM).ToString("#,##0");

                if (doc.Item1.ACCOMMODATION_NEEDED.Equals("Y")) this.radRdoNeededYes.Checked = true;
                else this.radRdoNeededNo.Checked = true;
                webMaster.DocumentNo = doc.Item1.DOC_NUM;

                

                List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY> countrys = doc.Item2;
                foreach (DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY country in countrys)
                {
                    AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry();
                    entry.Value = country.COUNTRY_CODE + ":" + country.EFPIA_FLAG ;
                    entry.Text = country.COUNTRY_NAME;
                    this.radAcomCountry.Entries.Add(entry);
                }

                List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> details = mgr.SelectInteractionHealthcareProDetails(processId);
                ViewState[VIEWSTATE_KEY] = details;

                this.radGrdCostDetail.DataSource = details;
                this.radGrdCostDetail.DataBind();

            }
        }
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdCostDetail, this.RadAjaxLoadingPanel2);
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddAddRow);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_InteractionHealthcarePro_AjaxRequest;

        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radBtnAddCategory, this.radGrdCostDetail, this.RadAjaxLoadingPanel2);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radGrdCostDetail, this.radGrdCostDetail);


        UpdateGridData();
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + this.radDdlActivity.SelectedValue + "');", true);

    }

    protected void radGrdCostDetail_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        if (e.CommandName.Equals("Remove"))
        {
            UpdateGridData();
            int index = Convert.ToInt32(e.CommandArgument);

            using (InteractionHealthcareProMgr mgr = new InteractionHealthcareProMgr())
            {
                mgr.DeleteInteractionHealthcareProDetail(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> list = (List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>)ViewState[VIEWSTATE_KEY];

            list.RemoveAll(p => p.IDX == index);

            if (list.Count < 1)
            {
                this.radGrdCostDetail.DataSource = new List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>();
                this.radGrdCostDetail.DataBind();
            }
            else
            {
                this.radGrdCostDetail.DataSource = list;
                this.radGrdCostDetail.DataBind();
            }
        }

    }
    protected void radGrdCostDetail_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item is GridEditableItem)
        {

        }
    }

    #region [ Add Button Event ]

    private void UpdateGridData()
    {
        if (this.hddGridItems.Value.IsNullOrEmptyEx()) return;
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> items = (List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>)serializer.Deserialize<List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>>(this.hddGridItems.Value);

        ViewState[VIEWSTATE_KEY] = items;
        this.radGrdCostDetail.DataSource = items;
        this.radGrdCostDetail.DataBind();

    }

    #endregion

    #region [ 상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();

        string message = string.Empty;

        if (this.radDdlActivity.SelectedValue.IsNullOrEmptyEx())
            message += "Activity";
        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (this.radTxtSubject.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Subject";

            if (!this.radDatFromEvent.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Event Date";

            if (this.radTxtMeetingVenue.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Meeting Venue";

            if (this.radTxtAddressOfVenue.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Address of Venue";

            if (this.radDdlActivity.SelectedValue.Equals("PPM"))
            {
                if (!(this.radChkPurpose1.Checked || this.radChkPurpose2.Checked || this.radChkPurpose3.Checked || this.radChkPurpose4.Checked || this.radChkPurpose5.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose / Object";
            }
            else if (this.radDdlActivity.SelectedValue.Equals("CM") || this.radDdlActivity.SelectedValue.Equals("ABM"))
            {
                if (!(this.radChkPurpose1.Checked || this.radChkPurpose2.Checked || this.radChkPurpose3.Checked || this.radChkPurpose4.Checked || this.radChkPurpose5.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose / Object";
            }
            else if (this.radDdlActivity.SelectedValue.Equals("EMT"))
            {
                if (!(this.radChkPurpose1.Checked || this.radChkPurpose2.Checked || this.radChkPurpose3.Checked || this.radChkPurpose4.Checked || this.radChkPurpose5.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose / Object";
            }
            else if (this.radDdlActivity.SelectedValue.Equals("CS") )
            {
                if (this.radTxtCompound.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Compound / Study Number";

                if (this.radTxtSponsor.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Sponsor of Study acc. to protocol";

                if (this.radTxtCROName.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "If outsourced, name of CRO";

                if (this.radTxtObjective.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Objective";

                if (!(this.radRdoInvestigatorYes.Checked || this.radRdoInvestigatorNo.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Investigator Meeting";

                if (!(this.radRdoMonitoringYes.Checked || this.radRdoMonitoringNo.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "On-site monitoring meeting";

                if (!(this.radRdoInvestigatorYes.Checked || this.radRdoInvestigatorNo.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Investigator Meeting";

                if (this.radTxtOtherspecify.Text.Length <= 0)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Other, please specify";

                if (!(this.radRdoContractYes.Checked || this.radRdoContractNo.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Contract";
            }
            else if (this.radDdlActivity.SelectedValue.Equals("PSH"))
            {
                if (!(this.radChkPurpose1.Checked || this.radChkPurpose2.Checked || this.radChkPurpose3.Checked || this.radChkPurpose4.Checked || this.radChkPurpose5.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose / Object";
            }

            else if (this.radDdlActivity.SelectedValue.Equals("PS"))
            {
                if (!(this.radChkPurpose1.Checked || this.radChkPurpose2.Checked || this.radChkPurpose3.Checked || this.radChkPurpose4.Checked || this.radChkPurpose5.Checked))
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose / Object";
            }
        }        
        //if (!(this.radRdoNeededYes.Checked || this.radRdoNeededNo.Checked))
        //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Accommodation Needed?";

        //if (this.radGrdCostDetail.MasterTableView.Items.Count < 1)
        //    message += (message.IsNullOrEmptyEx() ? "" : ",") + "Cost Detail";

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

        DTO_DOC_INTERACTION_HEALTHCARE_PRO doc = new DTO_DOC_INTERACTION_HEALTHCARE_PRO();
        doc.PROCESS_ID = this.hddProcessID.Value;
        if (this.radDatFromEvent.SelectedDate.HasValue)
        doc.SUBJECT = this.radTxtSubject.Text + "/" + this.radDatFromEvent.SelectedDate.Value.ToString("yyyy-MM-dd");
        else
            doc.SUBJECT = this.radTxtSubject.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TITLE = this.radTxtSubject.Text;
        doc.ACTIVITY_CODE = this.radDdlActivity.SelectedValue;
        if (this.radDatFromEvent.SelectedDate.HasValue)
        {
            DateTime dtDate = this.radDatFromEvent.SelectedDate.Value;
            TimeSpan tsTime = this.radTimFromEvent.SelectedTime.HasValue ? this.radTimFromEvent.SelectedTime.Value : new TimeSpan(0, 0, 0);

            doc.FROM_EVENT_DATE = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, tsTime.Hours, tsTime.Minutes, tsTime.Seconds);
        }
        if (this.radDatToEvent.SelectedDate.HasValue)
        {
            DateTime dtDate = this.radDatToEvent.SelectedDate.Value;
            TimeSpan tsTime = this.radTimToEvent.SelectedTime.HasValue ? this.radTimToEvent.SelectedTime.Value : new TimeSpan(0, 0, 0);

            doc.TO_EVENT_DATE = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, tsTime.Hours, tsTime.Minutes, tsTime.Seconds);
        }
        doc.MEETING_VENUE = this.radTxtMeetingVenue.Text;
        doc.ADDRESS_OF_VENUE = this.radTxtAddressOfVenue.Text;
        doc.VENUE_SELECTION_REASON_ETC = this.radTxtVenueSelectionReason.Text;
        string reason = string.Empty;
        foreach (Control control in this.divReason.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    reason += (control as RadButton).Value + ";";
            }
        }
        doc.VENUE_SELECTION_REASON = reason;

        string purpose = string.Empty;
        foreach (Control control in this.divPurpose.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                    purpose += (control as RadButton).Value + ";";
            }
        }
        doc.SCIENTIFIC_MATERIAL = this.radTxtScientificMaterial.Text;
        doc.TARGET_PARTICIPANTS = this.radTxtTargetParticipants.Text;
        doc.GO_NUM = StringToInt(this.radNumGO.Text);
        doc.NON_GO_NUM = StringToInt(this.radNumNonGO.Text);
        doc.PRIVATE_NUM = StringToInt(this.radNumPrivate.Text);
        doc.FOREIGN_NUM = StringToInt(this.radNumForeignHCP.Text);
        doc.BAYER_NUM = StringToInt(this.radNumBayer.Text);
        doc.ACCOMMODATION_NEEDED = this.radRdoNeededYes.Checked ? "Y" : "N";

        doc.PURPOSE_OBJECT = purpose;
        doc.COMPOUNT_STUDY_NUM = this.radTxtCompound.Text;
        doc.SPONSOR_OF_STUDY = this.radTxtSponsor.Text;
        doc.CRO_NAME = this.radTxtCROName.Text;
        doc.OBJECTIVE = this.radTxtObjective.Text;
        doc.IS_INVESTIGATOR_MEETING = this.radRdoInvestigatorYes.Checked ? "Y" : "N";
        doc.IS_MONITORING_MEETING = this.radRdoMonitoringYes.Checked ? "Y" : "N";
        doc.OTHER_SPECIFY = this.radTxtOtherspecify.Text;
        doc.IS_CONTRACT = this.radRdoContractYes.Checked ? "Y" : "N";


        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;


        bool existsEFPIA = false;
        List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY> countrys = new List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY>();
        foreach (AutoCompleteBoxEntry entry in this.radAcomCountry.Entries)
        {
            DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY country = new DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY();

            country.PROCESS_ID = this.hddProcessID.Value;
            string[] values = entry.Value.Split(new char[] { ':' }); 
            country.COUNTRY_CODE = values[0];
            country.COUNTRY_NAME = entry.Text;
            country.EFPIA_FLAG = values[1];
            country.CREATOR_ID = Sessions.UserID;
            if (country.EFPIA_FLAG == "Y")
                existsEFPIA = true;
            countrys.Add(country);
        }

        if (existsEFPIA) doc.EXISTS_EFPIA = "Y";
        else doc.EXISTS_EFPIA = "N";

        List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> details = (List<DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>)ViewState[VIEWSTATE_KEY];

        bool existsHonorarium = false;
        foreach (DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL detail in details)
        {
            if (detail.CATEGORY_CODE == "0008" )
                existsHonorarium = true;
            detail.AMOUNT = detail.QTY * detail.PRICE;
            detail.PROCESS_ID = this.hddProcessID.Value;
            detail.CREATOR_ID = Sessions.UserID;
        }

        if (existsHonorarium) doc.EXISTS_HONORARIUM = "Y";
        else doc.EXISTS_HONORARIUM = "N";

        using (InteractionHealthcareProMgr mgr = new InteractionHealthcareProMgr())
        {
            return mgr.MergeInteractionHealthcarePro(doc, countrys, details);
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