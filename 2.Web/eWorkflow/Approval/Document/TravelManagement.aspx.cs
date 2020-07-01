using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using System.Web.Script.Serialization;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using System.Collections;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Net;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Approval_Document_TravelManagement : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private static List<DTO_CODE_SUB> lstTripType = null;
    private static List<DTO_CODE_SUB> lstClass = null;
    private static List<DTO_CODE_SUB> lstAccommodation = null;
    private const string VIEWSTATE_EMPLOYEE_KEY = "VIEWSTATE_KEY_EMPLOYEE";
    private const string VIEWSTATE_EXTERNAL_KEY = "VIEWSTATE_KEY_EXTERNAL";
    private const string VIEWSTATE_TRIP_ROUTE_KEY = "VIEWSTATE_KEY_TRIP_ROUTE";
    private const string VIEWSTATE_ACCOMMODATION_KEY = "VIEWSTATE_KEY_ACCOMMODATION";
    private const string SESSION_TRIP_TYPE_KEY = "SESSION_TRIP_TYPE";
    private const string SESSION_AIRPLANE_CLASS_KEY = "SESSION_AIRPLANE_CLASS";
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
            if (!(hddReuse.Value.Equals(ApprovalUtil.ApprovalStatus.Completed.ToString()) ||
                hddReuse.Value.Equals(ApprovalUtil.ApprovalStatus.Reject.ToString())))
                webMaster.SetEnableControls(radBtnCancel, true);
        }

        base.OnPreRender(e);
    }
    #endregion

    #region [ Page Load ]

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                InitPageInfo();
            }

            RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdEmployee);
            RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdExternal);
            RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdTripRoute);
            RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddAddRow);
            RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddProcessID);
            RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddUserId);
            RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_TravelManagement_AjaxRequest;
            RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "fn_OnResponseEnd";
            if (this.hddIsAfterSendMail.Value.Equals("Y"))
            {
                RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdEstimationDetails);
                RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddQuotationAttachFiles);
                RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddApplicationAttachFiles);

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
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0001";
        //HddProcessID.Value = "P000000571";

        InitControls();

        string strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");
        hddUploadFolder.Value = string.Format(@"{0}\{1}\", strTempUploadPath, "travel");

    }

    private void SetTripRoutControl()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CodeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CodeMgr())
        {
            lstTripType = mgr.SelectCodeSubList("S012");
            lstClass = mgr.SelectCodeSubList("S013");
            lstAccommodation = mgr.SelectCodeSubList("S014");
        }
    }

    private void SetQuotationReason()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CodeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CodeMgr())
        {
            this.radDdlReason.Items.Clear();
            List<DTO_CODE_SUB> codes = mgr.SelectCodeSubList("S023");
            foreach (DTO_CODE_SUB code in codes)
            {
                this.radDdlReason.Items.Add(new DropDownListItem(code.CODE_NAME, code.SUB_CODE));
            }
        }
    }

    private void DisableControls()
    {


        this.radBtnRequestToAgency.Visible = false;
        this.divAfterSendMail.Visible = true;
        this.radBtnAddEmployee.Visible = false;
        this.radBtnAddExternal.Visible = false;
        this.radBtnAddTrip.Visible = false;

        foreach (GridColumn column in this.radGrdEmployee.Columns)
        {
            if (column is GridTemplateColumn)
            {
                if (column.UniqueName.Equals("REMOVE_BUTTON"))
                    (column as GridTemplateColumn).Visible = false;
                else
                    (column as GridTemplateColumn).ReadOnly = true;

            }
            else
                (column as GridBoundColumn).ReadOnly = true;
        }


        foreach (GridColumn column in this.radGrdExternal.Columns)
        {
            if (column is GridTemplateColumn)
            {
                if (column.UniqueName.Equals("REMOVE_BUTTON"))
                    (column as GridTemplateColumn).Visible = false;
                else
                    (column as GridTemplateColumn).ReadOnly = true;

            }
            else
                (column as GridBoundColumn).ReadOnly = true;
        }

        foreach (GridColumn column in this.radGrdTripRoute.Columns)
        {
            if (column is GridTemplateColumn)
            {
                if (column.UniqueName.Equals("REMOVE_BUTTON"))
                    (column as GridTemplateColumn).Visible = false;
                else
                    (column as GridTemplateColumn).ReadOnly = true;

            }
            else
                (column as GridBoundColumn).ReadOnly = true;
        }

        foreach (DropDownListItem item in this.radDdlPurpose.Items)
            item.Enabled = false;

        this.radDatFrom.DateInput.ReadOnly = true;
        this.radDatFrom.Calendar.Enabled = false;

        this.radDatTo.DateInput.ReadOnly = true;
        this.radDatTo.Calendar.Enabled = false;

        this.radTxtDetailInformation.ReadOnly = true;
        this.radTxtContactPoint.ReadOnly = true;
        this.radTxtCommentToAgency.ReadOnly = true;

    }

    private void SelectSessionUser()
    {
        using (Bayer.eWF.BSL.Common.Mgr.UserMgr mgr = new Bayer.eWF.BSL.Common.Mgr.UserMgr())
        {
            List<SmallUserInfoDto> users = mgr.SelectUserList(Sessions.UserID);
            if (users.Count > 0)
            {
                SmallUserInfoDto user = users[0];
                List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> employees = new List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>();
                DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER logOnUser = new DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER();
                logOnUser.DIV_DEPT = user.ORG_ACRONYM;
                logOnUser.COMPANY_ORG = user.COMPANY_NAME;
                logOnUser.EMP_NO = user.IPIN;
                logOnUser.IDX = 1;
                logOnUser.TRAVELER_ID = user.USER_ID;
                int idx = user.FULL_NAME.LastIndexOf("(");
                string name = user.FULL_NAME;
                if (idx > 0)
                    name = user.FULL_NAME.Substring(0, idx);
                logOnUser.TRAVELER_NAME = name;
                logOnUser.TRAVELER_TYPE = "I";
                logOnUser.COST_CODE = user.COST_CENTER;
                logOnUser.SEX = user.FORM_OF_ADDRESS;
                employees.Add(logOnUser);
                ViewState[VIEWSTATE_EMPLOYEE_KEY] = employees;
                this.radGrdEmployee.DataSource = employees;
                this.radGrdEmployee.DataBind();
            }
        }
    }

    private void InitControls()
    {
        SetQuotationReason();

        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (TravelManagementMgr mgr = new TravelManagementMgr())
            {
                string processId = this.hddProcessID.Value;

                DTO_DOC_TRAVEL_MANAGEMENT doc = mgr.SelectTravelManagement(processId);
                if (doc != null)
                {
                    this.HddProcessStatus.Value = doc.PROCESS_STATUS;
                    this.radDdlPurpose.SelectedValue = doc.TRIP_PURPOSE_CODE;
                    this.radDatFrom.SelectedDate = doc.TRIP_PERIOD_FROM;
                    this.radDatTo.SelectedDate = doc.TRIP_PERIOD_TO;
                    this.radTxtDetailInformation.Text = doc.TRIP_INFO;
                    this.radTxtCommentToAgency.Text = doc.COMMENT_TO_AGENCY;
                    this.hddIsAfterSendMail.Value = doc.REQUESTED_TO_AGENCY;
                    this.radTxtContactPoint.Text = doc.TRIP_CONTACT_POINT;

                    this.radDdlQuotationNo.SelectedValue = doc.QUOTATION_NUM.ToString();
                    this.radDdlReason.SelectedValue = doc.REASON_CODE;
                    this.radTxtDetailDesc.Text = doc.REASON_DESC;
                    this.radNumAmount.Text = doc.AIRFARE_AMOUNT.HasValue ? doc.AIRFARE_AMOUNT.Value.ToString("#,##0") : "0";
                    this.radTxtCommentsIfNeeded.Text = doc.AIRFARE_COMMENT;

                    webMaster.DocumentNo = doc.DOC_NUM;
                }

                //employee
                List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> travelers = mgr.SelectTravelManagementTraveler(processId);
                var employees = (from employee in travelers
                                 where employee.TRAVELER_TYPE == CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1)
                                 select employee).ToList();
                ViewState[VIEWSTATE_EMPLOYEE_KEY] = employees;
                this.radGrdEmployee.DataSource = employees;
                this.radGrdEmployee.DataBind();

                //external
                var externals = (from external in travelers
                                 where external.TRAVELER_TYPE == CommonEnum.MailAddressType.External.ToString().Substring(0, 1)
                                 select external).ToList();
                ViewState[VIEWSTATE_EXTERNAL_KEY] = externals;
                this.radGrdExternal.DataSource = externals;
                this.radGrdExternal.DataBind();

                //route
                List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> routes = mgr.SelectTravelManagementRoute(processId);
                ViewState[VIEWSTATE_TRIP_ROUTE_KEY] = routes;
                this.radGrdTripRoute.DataSource = routes;
                this.radGrdTripRoute.DataBind();

                //
                List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION> accommodations = mgr.SelectTravelManagementAccommodation(processId);
                ViewState[VIEWSTATE_ACCOMMODATION_KEY] = accommodations;
                this.radGrdEstimationDetails.DataSource = accommodations;
                this.radGrdEstimationDetails.DataBind();
                decimal total = 0;

                if (doc.AIRFARE_AMOUNT.HasValue)
                    total += (decimal)doc.AIRFARE_AMOUNT;
                total += accommodations.Sum(a => a.AMOUNT_TOTAL ?? 0);
                this.radNumExtimationTotal.Text = total.ToString("#,##0");

                if (doc.QUOTATION_IDX > 0)
                {
                    using (Bayer.eWF.BSL.Common.Mgr.FileMgr fileMgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
                    {
                        List<DTO_ATTACH_FILES> files = fileMgr.SelectAttachFileList(processId, "TravelQuotation");
                        if (files.Count > 0)
                            this.hddQuotationAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(files[files.Count - 1]);
                    }
                    divLinkQuotation.Style.Remove("display");
                    hrefQuotationFile.Attributes.Add("onclick", string.Format("javascript:fn_FileDownload({0})", doc.QUOTATION_IDX));
                    hrefQuotationFile.InnerText = doc.QUOTATION_FILE_NAME;
                    divUpLoadQuotation.Style["display"] = "none";
                    if (this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Saved.ToString() ||
                        this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Temp.ToString())
                    {
                        btnRemoveQuotation.Visible = true;
                    }
                    else
                        btnRemoveQuotation.Visible = false;
                }
                else
                {
                    divLinkQuotation.Style["display"] = "none";
                    if (!(this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Saved.ToString() ||
                        this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Temp.ToString()))
                        divUpLoadQuotation.Style["display"] = "none";

                }

                if (doc.APPLICATION_IDX > 0)
                {
                    using (Bayer.eWF.BSL.Common.Mgr.FileMgr fileMgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
                    {
                        List<DTO_ATTACH_FILES> files = fileMgr.SelectAttachFileList(processId, "TravelApplication");
                        if (files.Count > 0)
                            this.hddApplicationAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(files[files.Count - 1]);
                    }
                    divLinkApplication.Style.Remove("display");
                    hrefApplicationFile.Attributes.Add("onclick", string.Format("javascript:fn_FileDownload({0})", doc.APPLICATION_IDX));
                    hrefApplicationFile.InnerText = doc.APPLICATION_FILE_NAME;
                    divUpLoadApplication.Style["display"] = "none";
                    if (this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Saved.ToString() ||
                        this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Temp.ToString())
                    {
                        btnRemoveApplication.Visible = true;
                    }
                    else
                        btnRemoveApplication.Visible = false;

                }
                else
                {
                    divLinkApplication.Style["display"] = "none";
                    if (!(this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Saved.ToString() ||
                        this.HddProcessStatus.Value == ApprovalUtil.ApprovalStatus.Temp.ToString()))
                        divUpLoadApplication.Style["display"] = "none";
                }

                if (!(this.hddReuse.Value.Equals("Completed") || this.hddReuse.Value.Equals("Reject"))
                    && this.hddIsAfterSendMail.Value.Equals("Y"))
                    DisableControls();
                else
                {
                    this.radBtnRequestToAgency.Visible = true;
                    this.radBtnRequestToAgency.Enabled = true;
                    this.divAfterSendMail.Visible = false;

                    //SpecialUser인 경우만 Employee를 입력
                    bool isSpecialUser = Sessions.IsSpecialUser.Equals("Y") ? true : false;
                    if (isSpecialUser)
                    {
                        this.radBtnAddEmployee.Visible = true;
                    }
                    else
                    {
                        this.radBtnAddEmployee.Visible = false;
                    }

                }

            }
        }
        else
        {
            this.divAfterSendMail.Visible = false;


            SelectSessionUser();
            //SpecialUser인 경우만 Employee를 입력
            bool isSpecialUser = Sessions.IsSpecialUser.Equals("Y") ? true : false;
            if (isSpecialUser)
            {
                this.radBtnAddEmployee.Visible = true;
            }
            else
            {

                this.radBtnAddEmployee.Visible = false;
                this.radGrdEmployee.MasterTableView.GetColumn("REMOVE_BUTTON").Display = false;

            }

            ViewState[VIEWSTATE_EXTERNAL_KEY] = new List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>();
            this.radGrdExternal.DataSource = (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)ViewState[VIEWSTATE_EXTERNAL_KEY];
            this.radGrdExternal.DataBind();

            ViewState[VIEWSTATE_TRIP_ROUTE_KEY] = new List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>();
            this.radGrdTripRoute.DataSource = (List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>)ViewState[VIEWSTATE_TRIP_ROUTE_KEY];
            this.radGrdTripRoute.DataBind();

            ViewState[VIEWSTATE_ACCOMMODATION_KEY] = new List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>();
            this.radGrdEstimationDetails.DataSource = (List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>)ViewState[VIEWSTATE_ACCOMMODATION_KEY];
            this.radGrdEstimationDetails.DataBind();
        }

        SetTripRoutControl();
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 


        if (!this.hddEmployeeGridItems.Value.IsNullOrEmptyEx()) UpdateEmployeeGridData();
        if (!this.hddExternalGridItems.Value.IsNullOrEmptyEx()) UpdateExternalGridData();
        if (!this.hddTripRouteGridItems.Value.IsNullOrEmptyEx()) UpdateTripRouteGridData();
        if (this.hddIsAfterSendMail.Value.Equals("Y"))
        {
            if (!this.hddAccommodationGridItems.Value.IsNullOrEmptyEx()) UpdateEstimationGridData();
        }
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + this.radDdlQuotationNo.SelectedValue + "');", true);

    }


    void Approval_Document_TravelManagement_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        this.informationMessage = "";
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateEmployeeGridData();
            UpdateExternalGridData();
            UpdateTripRouteGridData();
            if (this.hddIsAfterSendMail.Value.Equals("Y"))
                UpdateEstimationGridData();
        }
        else if (e.Argument.StartsWith("ApplyCity"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            string index = values[1];
            string target = values[2];
            string city = values[3];

            UpdateTripRouteGridData(Convert.ToInt32(index), target, city, true);

        }
        else if (e.Argument.StartsWith("RequestToAgency"))
        {
            UpdateEmployeeGridData();
            UpdateExternalGridData();
            UpdateTripRouteGridData();
            RequestToAgency();
        }
        else if (e.Argument.Equals("DeleteQuotation"))
        {
            UpdateEmployeeGridData();
            UpdateExternalGridData();
            UpdateTripRouteGridData();
            string folderPath = string.Empty;
            try
            {
                DTO_ATTACH_FILES quotation = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(hddQuotationAttachFiles.Value);
                
                if (quotation != null)
                {
                    DeleteAttachFile(quotation.FILE_PATH, quotation.IDX);
                    hddQuotationAttachFiles.Value = string.Empty;
                }
            }
            catch
            {
                throw;
            }

        }
        else if (e.Argument.Equals("DeleteApplication"))
        {
            UpdateEmployeeGridData();
            UpdateExternalGridData();
            UpdateTripRouteGridData();
            string folderPath = string.Empty;
            try
            {
                DTO_ATTACH_FILES application = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(hddApplicationAttachFiles.Value);
                
                if (application != null)
                {
                    DeleteAttachFile(application.FILE_PATH, application.IDX);
                    hddApplicationAttachFiles.Value = string.Empty;
                }
            }
            catch
            {
                throw;
            }
        }

        if (e.Argument.StartsWith("ApplyCity")) this.hddAddRow.Value = "Rebind_TripRoute";
        else this.hddAddRow.Value = e.Argument;
    }

    private void DeleteAttachFile(string filePath, int idx)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
        {
            mgr.DeleteAttachFIles(idx.ToString());
        }
        

    }
    #endregion

    #region [ 문서상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        UpdateGridData();

        string message = string.Empty;

        if (this.radDdlPurpose.SelectedValue.IsNullOrEmptyEx())
            message += "Purpose";
        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
               
        }


        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    decimal StringToDecimal(string text)
    {
        decimal value = 0;
        text = text.Replace(",", "");
        if (text.Trim().Length > 0) value = Convert.ToDecimal(text);
        return value;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_TRAVEL_MANAGEMENT doc = new DTO_DOC_TRAVEL_MANAGEMENT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = this.radDdlPurpose.SelectedText;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TRIP_PURPOSE_CODE = this.radDdlPurpose.SelectedValue;
        doc.TRIP_PURPOSE = this.radDdlPurpose.SelectedText;
        doc.TRIP_PERIOD_FROM = this.radDatFrom.SelectedDate;
        doc.TRIP_PERIOD_TO = this.radDatTo.SelectedDate;
        doc.TRIP_INFO = this.radTxtDetailInformation.Text;
        doc.TRIP_CONTACT_POINT= this.radTxtContactPoint.Text;
        doc.REQUESTED_TO_AGENCY = this.hddIsAfterSendMail.Value;

        //After 
        if (this.radDdlQuotationNo.SelectedIndex < 0)
            doc.QUOTATION_NUM = null;
        else
            doc.QUOTATION_NUM = Convert.ToInt32(this.radDdlQuotationNo.SelectedValue);

        doc.REASON_CODE = this.radDdlReason.SelectedValue;
        doc.REASON_DESC = this.radTxtDetailDesc.Text;
        doc.COMMENT_TO_AGENCY = this.radTxtCommentToAgency.Text;
        doc.AIRFARE_AMOUNT = StringToDecimal(this.radNumAmount.Text);
        doc.AIRFARE_COMMENT = this.radTxtCommentsIfNeeded.Text;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;


        List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> travelers = (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)ViewState[VIEWSTATE_EMPLOYEE_KEY];
        int index = 1;
        foreach (DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER traveler in travelers)
        {
            traveler.IDX = index;
            traveler.TRAVELER_TYPE = CommonEnum.MailAddressType.Internal.ToString().Substring(0, 1);
            traveler.CREATOR_ID = Sessions.UserID;
            index++;
        }

        foreach (DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER traveler in (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)ViewState[VIEWSTATE_EXTERNAL_KEY])
        {
            traveler.IDX = index;
            traveler.TRAVELER_TYPE = CommonEnum.MailAddressType.External.ToString().Substring(0, 1);
            traveler.CREATOR_ID = Sessions.UserID;
            travelers.Add(traveler);
            index++;
        }

        List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> routes = (List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>)ViewState[VIEWSTATE_TRIP_ROUTE_KEY];
        foreach (DTO_DOC_TRAVEL_MANAGEMENT_ROUTE route in routes)
        {

            DTO_CODE_SUB code = lstTripType.Find(p => p.CODE_NAME == route.TRIP_TYPE.Trim());
            if (code != null)
                route.TRIP_TYPE = code.SUB_CODE;
            code = lstClass.Find(p => p.CODE_NAME == route.AIRPLANE_CLASS.Trim());
            if (code != null)
                route.AIRPLANE_CLASS = code.SUB_CODE;

            route.CREATOR_ID = Sessions.UserID;
        }

        List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION> accommodations = (List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>)ViewState[VIEWSTATE_ACCOMMODATION_KEY];
        foreach (DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION accommodation in accommodations)
        {
            DTO_CODE_SUB code = lstAccommodation.Find(p => p.CODE_NAME == accommodation.ACCOMMODATION_NAME.Trim());
            if (code != null)
                accommodation.ACCOMMODATION_CODE = code.SUB_CODE;
            accommodation.CREATOR_ID = Sessions.UserID;
        }

        using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
        {
            DTO_ATTACH_FILES fQuotation = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(this.hddQuotationAttachFiles.Value);
            DTO_ATTACH_FILES fApplication = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(this.hddApplicationAttachFiles.Value);
            mgr.AddAttachFile(this.hddProcessID.Value, Sessions.UserID, hddUploadFolder.Value, fQuotation);
            mgr.AddAttachFile(this.hddProcessID.Value, Sessions.UserID, hddUploadFolder.Value, fApplication);
        }

        using (TravelManagementMgr mgr = new TravelManagementMgr())
        {
            return mgr.MergeTravelManagement(doc, travelers, routes, accommodations);
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
                ApprovalUtil.ApprovalStatus ApprovalStatus = hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved : ApprovalUtil.ApprovalStatus.Temp;
                hddProcessID.Value = SaveDocument(ApprovalStatus);
                webMaster.ProcessID = hddProcessID.Value;
                if (hddQuotationAttachFiles.Value.IsNotNullOrEmptyEx())
                {
                    DTO_ATTACH_FILES file = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(hddQuotationAttachFiles.Value);
                    this.hrefQuotationFile.InnerText = file.DISPLAY_FILE_NAME;

                    divUpLoadQuotation.Style["display"] = "none";
                    divLinkQuotation.Style.Remove("display");
                }
                if (hddApplicationAttachFiles.Value.IsNotNullOrEmptyEx())
                {
                    DTO_ATTACH_FILES file = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(hddApplicationAttachFiles.Value);
                    this.hrefApplicationFile.InnerText = file.DISPLAY_FILE_NAME;
                    divUpLoadApplication.Style["display"] = "none";
                    divLinkApplication.Style.Remove("display");
                }
                base.DoRequest();
                //string start = this.radDatFrom.SelectedDate.Value.ToString("MM/dd/yyyy hh:mm");
                //if (this.radDatFrom.SelectedDate.Value.Hour > 12)
                //    start = start + "PM";
                //else
                //    start = start + "AM";
                //string end = this.radDatTo.SelectedDate.Value.ToString("MM/dd/yyyy hh:mm");
                //if (this.radDatTo.SelectedDate.Value.Hour > 12)
                //    end = end + "PM";
                //else
                //    end = end + "AM";

                //string funName = string.Format("fn_CreateAppointment('{0}','{1}','{2}','{3}','{4}');", "Travel", "Purpose", "Location", start, end);
                //if (!ClientScript.IsStartupScriptRegistered("fn_CreateAppointment"))
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "fn_CreateAppointment", funName, true);
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }

    }


    #endregion

    #region [ Inner Method ]

    private void UpdateGridData()
    {
        UpdateEmployeeGridData();
        UpdateExternalGridData();
        UpdateTripRouteGridData();
        UpdateEstimationGridData();
    }

    #endregion

    #region [ Traveler Information Event ]

    private void UpdateEmployeeGridData()
    {
        UpdateEmployeeGridData(true);
    }

    private void UpdateEmployeeGridData(bool binding)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> items = (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)serializer.Deserialize<List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>>(this.hddEmployeeGridItems.Value);

        ViewState[VIEWSTATE_EMPLOYEE_KEY] = items;

        if (binding)
        {
            this.radGrdEmployee.DataSource = items;
            this.radGrdEmployee.DataBind();
        }
    }

    protected void radGrdEmployee_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateEmployeeGridData(false);
            int index = Convert.ToInt32(e.CommandArgument);

            using (TravelManagementMgr mgr = new TravelManagementMgr())
            {
                mgr.DeleteTravelManagementTraveler(this.hddProcessID.Value, index, "I");
            }

            List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> list = (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)ViewState[VIEWSTATE_EMPLOYEE_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdEmployee.DataSource = list;
            this.radGrdEmployee.DataBind();
        }
    }

    private void UpdateExternalGridData()
    {
        UpdateExternalGridData(true);
    }
    private void UpdateExternalGridData(bool binding)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> items = (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)serializer.Deserialize<List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>>(this.hddExternalGridItems.Value);
        ViewState[VIEWSTATE_EXTERNAL_KEY] = items;
        if (binding)
        {
            this.radGrdExternal.DataSource = items;
            this.radGrdExternal.DataBind();
        }
    }

    protected void radGrdExternal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateExternalGridData(false);
            int index = Convert.ToInt32(e.CommandArgument);

            using (TravelManagementMgr mgr = new TravelManagementMgr())
            {
                mgr.DeleteTravelManagementTraveler(this.hddProcessID.Value, index, "E");
            }

            List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> list = (List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER>)ViewState[VIEWSTATE_EXTERNAL_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdExternal.DataSource = list;
            this.radGrdExternal.DataBind();
        }
    }

    #endregion

    #region [ Trip Route Event ]

    private void UpdateTripRouteGridData()
    {
        UpdateTripRouteGridData(999, string.Empty, string.Empty, true);
    }
    private void UpdateTripRouteGridData(int index, string target, string city, bool binding)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> items = (List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>)serializer.Deserialize<List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>>(this.hddTripRouteGridItems.Value);
        ViewState[VIEWSTATE_TRIP_ROUTE_KEY] = items;

        if (binding)
        {
            if (index < 999)
            {
                var itemRoute = (from item in items
                                 where item.IDX == index
                                 select item).FirstOrDefault();
                if (itemRoute != null)
                {
                    if (target.Equals("departure"))
                        (itemRoute as DTO_DOC_TRAVEL_MANAGEMENT_ROUTE).DEPARTURE_CODE = city;
                    else
                        (itemRoute as DTO_DOC_TRAVEL_MANAGEMENT_ROUTE).ARRIVAL_CODE = city;
                }

            }

            foreach (DTO_DOC_TRAVEL_MANAGEMENT_ROUTE item in items)
            {
                if (item.DEPARTURE_DATE.HasValue && item.DEPARTURE_DATE.Value.Year == 0001)
                    item.DEPARTURE_DATE = null;
                if (item.RETURN_DATE.HasValue && item.RETURN_DATE.Value.Year == 0001)
                    item.RETURN_DATE = null;
            }

            this.radGrdTripRoute.DataSource = items;
            this.radGrdTripRoute.DataBind();
        }
    }

    protected void radGrdTripRoute_PreRender(object sender, EventArgs e)
    {
        if (radGrdTripRoute.FindControl(radGrdTripRoute.MasterTableView.ClientID + "_TRIP_TYPE") != null) //Read Only인경우 Null 이다
        {
            RadDropDownList ddlTripType = radGrdTripRoute.FindControl(radGrdTripRoute.MasterTableView.ClientID + "_TRIP_TYPE").FindControl("radGrdDdlTripType") as RadDropDownList;
            ddlTripType.DataTextField = "CODE_NAME";
            ddlTripType.DataValueField = "SUB_CODE";
            ddlTripType.DataSource = lstTripType;
            ddlTripType.DataBind();

            RadDropDownList ddlClass = radGrdTripRoute.FindControl(radGrdTripRoute.MasterTableView.ClientID + "_AIRPLANE_CLASS").FindControl("radGrdDdlClass") as RadDropDownList;
            ddlClass.DataTextField = "CODE_NAME";
            ddlClass.DataValueField = "SUB_CODE";
            ddlClass.DataSource = lstClass;
            ddlClass.DataBind();
        }
    }

    protected void radGrdTripRoute_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateTripRouteGridData(999, "", "", false);
            int index = Convert.ToInt32(e.CommandArgument);

            using (TravelManagementMgr mgr = new TravelManagementMgr())
            {
                mgr.DeleteTravelManagementRoute(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> list = (List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE>)ViewState[VIEWSTATE_TRIP_ROUTE_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdTripRoute.DataSource = list;
            this.radGrdTripRoute.DataBind();
        }
    }
    #endregion

    #region [ Estimation ]


    private void UpdateEstimationGridData()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION> items = (List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>)serializer.Deserialize<List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>>(this.hddAccommodationGridItems.Value);

        if (items != null)
        {
            foreach (DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION item in items)
            {
                if (item.FROM_DATE.HasValue && item.FROM_DATE.Value.Year == 0001)
                    item.FROM_DATE = null;
                else if (item.TO_DATE.HasValue && item.TO_DATE.Value.Year == 0001)
                    item.TO_DATE = null;

                if (item.FROM_DATE != null && item.TO_DATE != null)
                {
                    if (item.FROM_DATE <= item.TO_DATE)
                    {
                        int idays = (item.TO_DATE.Value - item.FROM_DATE.Value).Days;
                        item.AMOUNT_TOTAL = item.AMOUNT_PER_NIGHT * idays;
                    }
                }
            }

            ViewState[VIEWSTATE_ACCOMMODATION_KEY] = items;
            this.radGrdEstimationDetails.DataSource = items;
            this.radGrdEstimationDetails.DataBind();
        }
    }

    protected void radGrdEstimationDetails_PreRender(object sender, EventArgs e)
    {
        using (Bayer.eWF.BSL.Common.Mgr.CodeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CodeMgr())
        {
            if (radGrdEstimationDetails.FindControl(radGrdEstimationDetails.MasterTableView.ClientID + "_ACCOMMODATION_CODE") != null)
            {
                RadDropDownList ddlAccommodation = radGrdEstimationDetails.FindControl(radGrdEstimationDetails.MasterTableView.ClientID + "_ACCOMMODATION_CODE").FindControl("radGrdDdlAccommodation") as RadDropDownList;
                ddlAccommodation.DataTextField = "CODE_NAME";
                ddlAccommodation.DataValueField = "SUB_CODE";
                ddlAccommodation.DataSource = lstAccommodation;
                ddlAccommodation.DataBind();
            }

        }
    }
    #endregion

    #region [ Request to agency ]

    private void RequestService()
    {
        try
        {
            hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
            this.hddUserId.Value = Sessions.UserID;
            SaveProcessDocument(hddProcessID.Value);

            //using (TravelManagementMgr mgr = new TravelManagementMgr())
            //{
            //    mgr.UpdateTravelManagementRequestToAgency(processId, Sessions.UserID);
            //}


            //string serviceUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");
            //serviceUrl = serviceUrl + string.Format("/MailServices.svc/SendToAgency/{0}/{1}", processId, Sessions.UserID);
            //HttpWebRequest request = WebRequest.Create(serviceUrl) as HttpWebRequest;
            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    if (response.StatusCode != HttpStatusCode.OK)
            //        throw new Exception(String.Format(
            //        "Server error (HTTP {0}: {1}).",
            //        response.StatusCode,
            //        response.StatusDescription));
            //    System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
            //    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
            //    return objResponse.ToString();
            //}            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SaveProcessDocument(string processId)
    {
        DTO_PROCESS_DOCUMENT docProc = new DTO_PROCESS_DOCUMENT();
        docProc.PROCESS_ID = processId;
        docProc.DOCUMENT_ID = this.hddDocumentID.Value;
        docProc.DOC_NAME = "Travel Management";
        docProc.SUBJECT = this.radDdlPurpose.SelectedText;
        docProc.DOC_NUM = "";
        docProc.PROCESS_STATUS = ApprovalUtil.ApprovalStatus.Saved.ToString();
        docProc.REQUEST_DATE = DateTime.Now;
        docProc.COMPANY_CODE = Sessions.CompanyCode;
        docProc.REQUESTER_ID = Sessions.UserID;
        docProc.CURRENT_APPROVER = String.Empty;
        docProc.FINAL_APPROVER = String.Empty;
        docProc.REJECTED_PROCESS_ID = string.Empty;
        docProc.CREATE_DATE = DateTime.Now;


        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            mgr.InsertProcessDocument(docProc);
        }
    }



    private void RequestToAgency()
    {
        try
        {
            this.informationMessage = "";

            RequestService();

        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    #endregion

    protected void radUpLoadQuotation_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        string folder = hddUploadFolder.Value;


        StringBuilder sbAttachFileXml = new StringBuilder(512);
        Stream oStm = null;
        FileStream oFileStream = null;
        byte[] buffer = null;

        try
        {
            buffer = new byte[e.File.ContentLength];

            using (oStm = e.File.InputStream)
            {
                int nbytesRead = oStm.Read(buffer, 0, Convert.ToInt32(e.File.ContentLength));
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                oFileStream = new FileStream(folder + e.File.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                oFileStream.Write(buffer, 0, nbytesRead);

            }

            DTO_ATTACH_FILES f = new DTO_ATTACH_FILES();
            f.SEQ = 1;
            f.DISPLAY_FILE_NAME = e.File.FileName;
            f.SAVED_FILE_NAME = e.File.FileName;
            f.FILE_SIZE = e.File.ContentLength;
            f.ATTACH_FILE_TYPE = "TravelQuotation";
            f.FILE_PATH = string.Empty;
            f.COMMENT_IDX = 0;

            hddQuotationAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(f);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sbAttachFileXml != null) sbAttachFileXml = null;
            if (oStm != null)
            {
                oStm.Close();
                oStm.Dispose();
                oStm = null;
            }

            if (oFileStream != null)
            {
                oFileStream.Close();
                oFileStream.Dispose();
                oFileStream = null;
            }

        }
    }
    protected void radUpLoadApplication_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        string folder = hddUploadFolder.Value;


        StringBuilder sbAttachFileXml = new StringBuilder(512);
        Stream oStm = null;
        FileStream oFileStream = null;
        byte[] buffer = null;

        try
        {
            buffer = new byte[e.File.ContentLength];

            using (oStm = e.File.InputStream)
            {
                int nbytesRead = oStm.Read(buffer, 0, Convert.ToInt32(e.File.ContentLength));
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                oFileStream = new FileStream(folder + e.File.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                oFileStream.Write(buffer, 0, nbytesRead);

            }

            DTO_ATTACH_FILES f = new DTO_ATTACH_FILES();
            f.SEQ = 1;
            f.DISPLAY_FILE_NAME = e.File.FileName;
            f.SAVED_FILE_NAME = e.File.FileName;
            f.FILE_SIZE = e.File.ContentLength;
            f.ATTACH_FILE_TYPE = "TravelApplication";
            f.FILE_PATH = string.Empty;
            f.COMMENT_IDX = 0;

            hddApplicationAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(f);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sbAttachFileXml != null) sbAttachFileXml = null;
            if (oStm != null)
            {
                oStm.Close();
                oStm.Dispose();
                oStm = null;
            }

            if (oFileStream != null)
            {
                oFileStream.Close();
                oFileStream.Dispose();
                oFileStream = null;
            }

        }
    }



    protected void radGrdEstimationDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            UpdateEstimationGridData();
            int index = Convert.ToInt32(e.CommandArgument);

            using (TravelManagementMgr mgr = new TravelManagementMgr())
            {
                mgr.DeleteTravelManagementAccommodation(this.hddProcessID.Value, index);
            }

            List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION> list = (List<DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION>)ViewState[VIEWSTATE_ACCOMMODATION_KEY];

            list.RemoveAll(p => p.IDX == index);

            this.radGrdEstimationDetails.DataSource = list;
            this.radGrdEstimationDetails.DataBind();
        }
    }
}