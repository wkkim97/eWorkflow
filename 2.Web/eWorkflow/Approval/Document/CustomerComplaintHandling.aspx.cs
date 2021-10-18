using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;

using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Web.Script.Serialization;
using Telerik.Web.UI;

public partial class Approval_Document_CustomerComplaintHandling : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private const string VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY = "VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY";
    private static List<DTO_CODE_SUB> demand = null;

    string Radio;
    string type;

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

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                ViewState[VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY] = new List< DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>();
                this.radGrdCustomerComplaintHandling.DataSource = (List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>)ViewState[VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY];
                this.radGrdCustomerComplaintHandling.DataBind();

                InitPageInfo();
            }
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0055";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //hddProcessID.Value = "";

        InitControls();
    }
    #endregion


    private void SetDropdownControl()
    {
        using (Bayer.eWF.BSL.Common.Mgr.CodeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CodeMgr())
        {
            demand = mgr.SelectCodeSubList("S028");
        }
    }


    #region InitControls
    private void InitControls()
    {
        DTO_DOC_CUSTOMER_COMPLAINT_HANDLING doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            string processId = this.hddProcessID.Value;
            using (Bayer.eWF.BSL.Approval.Mgr.CustomerComplaintHandlingMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CustomerComplaintHandlingMgr())
            {
                doc = mgr.SelectCustomerComplaintHandling(hddProcessID.Value);
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                if (doc != null)
                {
                    if (doc.BG == radRdoBG1.Value)
                        this.radRdoBG1.Checked = true;
                    else if (doc.BG == radRdoBG2.Value)
                        this.radRdoBG2.Checked = true;
                    else if (doc.BG == radRdoBG4.Value)
                        this.radRdoBG4.Checked = true;

                    this.radTxtDemand.Text = doc.COMMENT;

                    if (doc.TYPE == radRdoType1.Value)
                        this.radRdoType1.Checked = true;
                    else if (doc.TYPE == radRdoType2.Value)
                        this.radRdoType2.Checked = true;
                    else if (doc.TYPE == radRdoType3.Value)
                        this.radRdoType3.Checked = true;

                    if (doc.CUSTOMER_TYPE == radRdoNH.Value)
                        this.radRdoNH.Checked = true;
                    else if (doc.CUSTOMER_TYPE == radRdoFM.Value)
                        this.radRdoFM.Checked = true;
                    else if (doc.CUSTOMER_TYPE == radRdoOthers.Value)
                        this.radRdoOthers.Checked = true;
                    else if (doc.CUSTOMER_TYPE == radRdoFarmer.Value)
                        this.radRdoFarmer.Checked = true;

                    this.hddCustomerCode.Value = doc.CUSTOMER_CODE;
                    this.radGrdTxtCustomer.Text = doc.CUSTOMER_NAME;
                    this.radTxtIssueArea.Text = doc.ISSUE_AREA;

                    //this.radTxtFarmerName.Text = doc.FARMER_NAME;
                    //this.radTxtFarmerTelNo.Text = doc.FARMER_TEL_NO;
                    //this.radTxtFarmerAddress.Text = doc.FARMER_ADDRESS;


                    if (doc.ISSUE_DATE.HasValue )
                    {
                        this.radDatIssueDate.SelectedDate = doc.ISSUE_DATE.Value.Date;
                    }
                    this.radTxtIssueExpiredPeriod.Text = doc.ISSUE_EXPIRED_PERIOD;

                    if (doc.ISSUE_COLLECTING_METHOD == radRdoMail.Value)
                        this.radRdoMail.Checked = true;
                    else if (doc.ISSUE_COLLECTING_METHOD == radRdoTel.Value)
                        this.radRdoTel.Checked = true;
                    else if (doc.ISSUE_COLLECTING_METHOD == radRdoRegularVist.Value)
                        this.radRdoRegularVist.Checked = true;

                    this.radTxtActionAfterReception.Text = doc.ACTION_AFTER_RECEPTION;
                    this.radTxtContact.Text = doc.CONTACT;
                    this.radTxtEmployee.Text = doc.EMPLOYEE;
                    this.radTxtActionAfterVisit.Text = doc.ACTION_AFTER_VISIT;


                    if (doc.TYPE == radRdoType1.Value)
                    {
                        this.radRdoType1.Checked = true;
                        this.RadDropEffect.SelectedValue = doc.ISSUE_CODE;
                        this.RadDropEffect.SelectedText  = doc.ISSUE_DESC;
                    }
                    else if (doc.TYPE == radRdoType2.Value)
                    {
                        this.radRdoType2.Checked = true;
                        this.RadDropQuality.SelectedValue = doc.ISSUE_CODE;
                        this.RadDropQuality.SelectedText = doc.ISSUE_DESC;
                    }
                    else if (doc.TYPE == radRdoType3.Value)
                    {
                        this.radRdoType3.Checked = true;
                        this.RadDropDelivery.SelectedValue = doc.ISSUE_CODE;
                        this.RadDropDelivery.SelectedText = doc.ISSUE_DESC;
                    }

                    this.radTxtRemark.Text = doc.ISSUE_REMARK;

                    //Product
                    List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> products = mgr.SelectCustomerComplaintHandlingProduct(this.hddProcessID.Value);
                    ViewState[VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY] = products;

                    this.radGrdCustomerComplaintHandling.DataSource = products;
                    this.radGrdCustomerComplaintHandling.DataBind();


                    if (!ClientScript.IsStartupScriptRegistered("setVisible"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + doc.BG + "','" + doc.TYPE + "');", true);
                    webMaster.DocumentNo = doc.DOC_NUM;

                    webMaster.DocumentNo = doc.DOC_NUM;
                }                
            }
        }
        SetDropdownControl();
    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 
        if (this.hddGridItems.Value != "")
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> products = (List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>>(this.hddGridItems.Value);

            ViewState[VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY] = products;
            this.radGrdCustomerComplaintHandling.DataSource = products;
            this.radGrdCustomerComplaintHandling.DataBind();
        }

        
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.radGrdCustomerComplaintHandling);
        //RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(this.radGrdCustomerComplaintHandling, this.radGrdCustomerComplaintHandling);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_CustomerComplaintHandling_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "openGridRowForEdit";

    }
    #endregion

    #region Get Radio Value
    private string GetSelectedBG()
    {
        string Bg = string.Empty;
        foreach (Control control in divBG.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Bg = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Bg;
    }

    private string GetSelectedType()
    {
        string type = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    type = (control as RadButton).Value;
                    break;
                }
            }
        }
        return type;
    }

    private string GetSelectedTypeText()
    {
        string type = string.Empty;
        foreach (Control control in divType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    type = (control as RadButton).Text;
                    break;
                }
            }
        }
        return type;
    }

    private string GetSelectedCustomerType()
    {
        string type = string.Empty;
        foreach (Control control in divCustomerType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    type = (control as RadButton).Value;
                    break;
                }
            }
        }
        return type;
    }

    private string GetSelectedIssueCollectionMethod()
    {
        string Method = string.Empty;
        foreach (Control control in divIssueCollectigMethod.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Method = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Method;
    }


    #endregion

    void Approval_Document_CustomerComplaintHandling_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.StartsWith("Rebind"))
        {
            UpdateCurProductGridData(999, string.Empty, string.Empty);
        }
        if (e.Argument.StartsWith("SetCurProduct"))
        {
            string[] values = e.Argument.Split(new char[] { ':' });
            if (values.Length > 5)
            {
                
                string idx = values[2];
                string code = values[3];
                string name = values[4];
                

                UpdateCurProductGridData(Convert.ToInt32(idx), code, name);
            }
        }
    }
    private void UpdateGridData()
    {
        
    }
    #region [ Add Row ]

    // Product Update Grid 
    private void UpdateCurProductGridData(int idx, string productcode, string productname)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> items = (List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>>(this.hddGridItems.Value);
               
        if (idx < 999)
        {
            var list = (from item in items
                        where item.IDX == idx                         
                        select item).FirstOrDefault();
            if (list != null)
            {
                list.PRODUCT_CODE = productcode;
                list.PRODUCT_NAME = productname;

            }
        }

        ViewState[VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY] = items;
        this.radGrdCustomerComplaintHandling.DataSource = items;
        this.radGrdCustomerComplaintHandling.DataBind();

    }

    #endregion

    // Grid ReSET
    protected void radGrdCustomerComplaintHandling_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Remove"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                using (CustomerComplaintHandlingMgr Mgr = new CustomerComplaintHandlingMgr())
                {
                    Mgr.DeleteSelectCustomerComplaintHandlingProductByIndex  (this.hddProcessID.Value, index);
                }
                List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> list = (List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>)ViewState[VIEWSTATE_CUSTOMER_COMPLAINT_HANDLING_KEY];
                list.RemoveAll(p => p.IDX == index);

                this.radGrdCustomerComplaintHandling.DataSource = list;
                this.radGrdCustomerComplaintHandling.DataBind();
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
                string ApprovalStatus = hddProcessID.Value.Length > 0 ? ApprovalUtil.ApprovalStatus.Saved.ToString() : ApprovalUtil.ApprovalStatus.Temp.ToString();
                hddProcessID.Value = DocumentSave(ApprovalStatus);
                webMaster.ProcessID = hddProcessID.Value;
                base.DoRequest();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }


    #region Approval
    protected override void DoApproval()
    {
        // TODO :

        base.DoApproval();
    }
    #endregion


    #region DoSave
    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = DocumentSave(ApprovalUtil.ApprovalStatus.Saved.ToString());
                webMaster.ProcessID = hddProcessID.Value;
                base.DoSave();
            }
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    } 
    #endregion

    #region Validation Check
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (this.GetSelectedBG().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "사업그룹" : ", 사업그룹";

            if (this.GetSelectedType().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "불만의 종류" : ", 불만의 종류";
 
            if (this.radTxtDemand.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "영업지점의 요구사항" : ", 영업지점의 요구사항";

            if (this.GetSelectedCustomerType().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "고객 유형" : ", 고객 유형";

            if (this.radRdoFM.Checked || this.radRdoNH.Checked)
            {
                if (this.radGrdTxtCustomer.Text.IsNullOrEmptyEx())
                    message += message.IsNullOrEmptyEx() ? "농협/시판 Code/Name" : ", 농협/시판 Code/Name";
            }

            if (this.radTxtIssueArea.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "발생지역" : ", 발생지역";

            if (!this.radDatIssueDate.SelectedDate.HasValue)
                message += message.IsNullOrEmptyEx() ? "불만 최초 접수일" : ", 불만 최초 접수일";

            if (this.radTxtIssueExpiredPeriod.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "경과일" : ", 영업지점의 요구사항";

            if (this.GetSelectedIssueCollectionMethod().IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "불만접수 방법" : ", 불만접수 방법";

            if (this.radTxtActionAfterReception.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "불만접수 후 조치사항" : ", 불만접수 후 조치사항";

            if (this.radTxtContact.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "접촉 대상자" : ", 접촉 대상자";

            if (this.radTxtEmployee.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "동행직원" : ", 동행직원";

            if (this.radTxtActionAfterVisit.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "방문하여 조치한 내용" : ", 방문하여 조치한 내용";

            string SelectedType = GetSelectedType();

            if (!ClientScript.IsStartupScriptRegistered("setVisible"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisible", "setVisible('" + "" + "','" + SelectedType + "');", true);

            if (SelectedType == "Effect")
            {
                if (RadDropEffect.SelectedIndex == -1)
                    message += message.IsNullOrEmptyEx() ? "불만의 구체적인 내용" : ", 불만의 구체적인 내용";
            }
            else if (SelectedType == "Quality")
            {
                if (RadDropQuality.SelectedIndex == -1)
                    message += message.IsNullOrEmptyEx() ? "불만의 구체적인 내용" : ", 불만의 구체적인 내용";
            }
            else if (SelectedType == "Delivery")
            {
                if (RadDropDelivery.SelectedIndex == -1)
                    message += message.IsNullOrEmptyEx() ? "불만의 구체적인 내용" : ", 불만의 구체적인 내용";
            }

            if (this.radTxtRemark.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Remarks" : ", Remarks";

            //else if (this.radRdoFarmer.Checked)
            //{
            //    if (this.radTxtFarmerName.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "농민/소비자 정보(이름)" : ", 농민/소비자 정보(이름)";

            //    if (this.radTxtFarmerTelNo.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "농민/소비자 정보(전화번호)" : ", 농민/소비자 정보(전화번호)";

            //    if (this.radTxtFarmerAddress.Text.Length <= 0)
            //        message += message.IsNullOrEmptyEx() ? "농민/소비자 정보(주소)" : ", 농민/소비자 정보(주소)";
            //}
            //if (this.radGrdCustomerComplaintHandling.Items.Count < 1)
            //    message += message.IsNullOrEmptyEx() ? "Grid Item" : ",Grid Item";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    } 
    #endregion

    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;

        string SelectedBG = "";

        if (this.radRdoBG1.Checked == true)
            SelectedBG = this.radRdoBG1.Text;
        else if (this.radRdoBG2.Checked == true)
            SelectedBG = this.radRdoBG2.Text;
        else if (this.radRdoBG4.Checked == true)
            SelectedBG = this.radRdoBG4.Text;

        doc.SUBJECT = SelectedBG + " / " + GetSelectedTypeText();   //subject            
        webMaster.Subject = doc.SUBJECT;


        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        doc.BG = GetSelectedBG();

        string SelectedType = GetSelectedType();
        doc.TYPE = SelectedType;

        doc.COMMENT = this.radTxtDemand.Text;

        doc.CUSTOMER_TYPE = GetSelectedCustomerType();

        doc.CUSTOMER_CODE = this.hddCustomerCode.Value;
        doc.CUSTOMER_NAME = this.radGrdTxtCustomer.Text;

        doc.ISSUE_AREA = this.radTxtIssueArea.Text;

        //doc.FARMER_NAME = this.radTxtFarmerName.Text;
        //doc.FARMER_TEL_NO = this.radTxtFarmerTelNo.Text;
        //doc.FARMER_ADDRESS = this.radTxtFarmerAddress.Text;

        if (this.radDatIssueDate.SelectedDate.HasValue)
        {
            doc.ISSUE_DATE = this.radDatIssueDate.SelectedDate.Value.Date;
        }

        doc.ISSUE_EXPIRED_PERIOD = this.radTxtIssueExpiredPeriod.Text;
        doc.ISSUE_COLLECTING_METHOD = GetSelectedIssueCollectionMethod();
        doc.ACTION_AFTER_RECEPTION = this.radTxtActionAfterReception.Text;
        doc.CONTACT = this.radTxtContact.Text;
        doc.EMPLOYEE = this.radTxtEmployee.Text;
        doc.ACTION_AFTER_VISIT = this.radTxtActionAfterVisit.Text;

        if (SelectedType == "Effect")
        {
            doc.ISSUE_CODE = this.RadDropEffect.SelectedValue;
            doc.ISSUE_DESC = this.RadDropEffect.SelectedText;
        }
        else if (SelectedType == "Quality")
        {
            doc.ISSUE_CODE = this.RadDropQuality.SelectedValue;
            doc.ISSUE_DESC = this.RadDropQuality.SelectedText;
        }
        else if (SelectedType == "Delivery")
        {
            doc.ISSUE_CODE = this.RadDropDelivery.SelectedValue;
            doc.ISSUE_DESC = this.RadDropDelivery.SelectedText;
        }

        doc.ISSUE_REMARK = this.radTxtRemark.Text;


        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> products = (List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>)serializer.Deserialize<List<DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>>(this.hddGridItems.Value);
        foreach (DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT product in products)
        {

            DTO_CODE_SUB code = demand.Find(p => p.CODE_NAME == product.DEMAND_NAME.Trim());
            if (code != null)
            {
                product.DEMAND_CODE = code.SUB_CODE;
            }
            product.PROCESS_ID = this.hddProcessID.Value;
            product.CREATOR_ID = Sessions.UserID;
        }

 

        using (Bayer.eWF.BSL.Approval.Mgr.CustomerComplaintHandlingMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CustomerComplaintHandlingMgr())
        {
            processID = mgr.MergeCustomerComplaintHandling(doc, products);
        }
        return processID;
    }

    protected void radGrdCustomerComplaintHandling_PreRender(object sender, EventArgs e)
    {


        if (radGrdCustomerComplaintHandling.FindControl(radGrdCustomerComplaintHandling.MasterTableView.ClientID + "_DEMAND_NAME") != null)
        {
            RadDropDownList DropLocation = radGrdCustomerComplaintHandling.FindControl(radGrdCustomerComplaintHandling.MasterTableView.ClientID + "_DEMAND_NAME").FindControl("radDropDemand") as RadDropDownList;
            DropLocation.DataTextField = "CODE_NAME";
            DropLocation.DataValueField = "SUB_CODE";
            DropLocation.DataSource = demand;
            DropLocation.DataBind();
        }

    }

    protected void radGrdCustomerComplaintHandling_ItemBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = e.Item as GridEditableItem;

        }
    }

}
