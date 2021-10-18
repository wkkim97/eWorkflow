using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;

public partial class Approval_Document_MortgageManagement : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    protected override void OnPreRender(EventArgs e)
    {
        if (!webMaster.DocumentNo.IsNullOrEmptyEx())
        {
            //Currently인 경우
            if (this.lblStatus.Text.Equals("Currently"))
            {
              //  webMaster.SetEnableControls(this.divReturn, true); //하단 Returned표시
               // if (!this.hddApplyMaster.Value.Equals("Y") && this.radChkCredit.Checked)
                  //  webMaster.SetEnableControls(this.radBtnCreditLimit, true);
            }
            else if (this.lblStatus.Text.Equals("Returned"))
            {
                if (this.hddApplyMaster.Value.Equals("Y"))
                    webMaster.SetEnableControls(this.divReturn, false);
                else
                {
                    if (hddIsCalledReturn.Value == "Y") //Currently에서 Return을 클릭한 경우
                    {
                        webMaster.SetEnableControls(this.radBtnCalNewCreditLimit, true);
                        webMaster.SetEnableControls(this.radBtnPHRDC, false);
                        webMaster.SetEnableControls(this.radBtnCC, false);
                        webMaster.SetEnableControls(this.radBtnAH, false);
                        webMaster.SetEnableControls(this.RadbtnSearch, false);
                        webMaster.SetEnableControls(this.radDropMortgageType, false);
                        webMaster.SetEnableControls(this.RadNubookValue, false);
                        webMaster.SetEnableControls(this.RadNuRevaluation, false);
                        webMaster.SetEnableControls(this.RadNuRevaluation, false);
                        webMaster.SetEnableControls(this.RadNuNewCreditLimit, false);
                        webMaster.SetEnableControls(this.RadDateReceived, false);
                        webMaster.SetEnableControls(this.RadDateIssue, false);
                        webMaster.SetEnableControls(this.RadDateDue, false);
                        webMaster.SetEnableControls(this.RadtxtPublisher, false);
                        webMaster.SetEnableControls(this.RadtxtPublished, false);
                    }
                    //else
                       // webMaster.SetEnableControls(this.radBtnCreditLimit, true);
                }
            }
        }


        base.OnPreRender(e);
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                hddUserId.Value = Sessions.UserID;

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

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        this.radBtnCalNewCreditLimit.Visible = this.radChkCredit.Checked;

    }
    #endregion

    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddCurrentlyProcessID.Value = Request["CurrentlyProcessId"].NullObjectToEmptyEx();
        if (hddCurrentlyProcessID.Value.IsNullOrEmptyEx()) hddIsCalledReturn.Value = "N";
        else hddIsCalledReturn.Value = "Y";
        hddDocumentID.Value = "D0003";
        //hddProcessID.Value = "P000000506";

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        using (CodeMgr mgr = new CodeMgr())
        {
            List<DTO_CODE_SUB> titles = mgr.SelectCodeSubList("S017");
            this.radDropMortgageType.DataSource = titles;
            this.radDropMortgageType.DataBind();
            if (this.radDropMortgageType.Items.Count > 0) this.radDropMortgageType.SelectedIndex = -1;
        }

        DTO_DOC_MORTGAGE_MANAGMENT doc;
        if (!hddProcessID.Value.IsNullOrEmptyEx() || !hddCurrentlyProcessID.Value.IsNullOrEmptyEx())
        {
            using (MortgageManagementMgr mgr = new MortgageManagementMgr())
            {
                string processID = hddProcessID.Value;
                if (processID.IsNullOrEmptyEx()) processID = hddCurrentlyProcessID.Value;
                doc = mgr.SelectMortgageManagement(processID);
            }
            if (doc != null)
            {
                this.RadtxtCustomer.Text = doc.CUSTOMER_NAME;
                this.hddCustomerCode.Value = doc.CUSTOMER_CODE;
                if (doc.BG_CODE == this.radBtnPHRDC.Value) this.radBtnPHRDC.Checked = true;
                else if (doc.BG_CODE == this.radBtnCC.Value) this.radBtnCC.Checked = true;
                else if (doc.BG_CODE == this.radBtnAH.Value) this.radBtnAH.Checked = true;
                this.lblStatus.Text = doc.STATUS_CODE;
                this.radDropMortgageType.SelectedValue = doc.MORTGAGE_TYPE;
                this.RadNubookValue.Value = (double?)doc.BOOK_VALUE;
                this.RadNuRevaluation.Value = (double?)doc.REVALUTION_VALUE;
                if (doc.NEW_CREDIT_LIMIT.HasValue)
                    this.RadNuNewCreditLimit.Value = (double?)doc.NEW_CREDIT_LIMIT.Value;
                else
                {
                    this.radChkCredit.Checked = false;
                    this.RadNuNewCreditLimit.Value = null;
                }
                this.RadDateReceived.SelectedDate = doc.RECEIVED_DATE;
                this.RadDateReturned.SelectedDate = doc.RETURN_DATE;
                this.RadDateIssue.SelectedDate = doc.ISSUE_DATE;
                this.RadDateDue.SelectedDate = doc.DUE_DATE;
                this.RadtxtPublisher.Text = doc.PUBLISHER;
                this.RadtxtPublished.Text = doc.PUBLISHED_NUM;
                this.RadtxtComment.Text = doc.COMMENT;
                //완료된 문서에서 Return버튼 클릭시 넘어오는 hddcurrentlyProcessId때문
                if (!doc.STATUS_CODE.Equals("Returned") && !doc.STATUS_CODE.Equals("Termination"))
                {
                    if (this.hddCurrentlyProcessID.Value.IsNullOrEmptyEx())
                        this.lblStatus.Text = "Currently";
                    else
                        this.lblStatus.Text = "Returned";
                }

                if (hddCurrentlyProcessID.Value.IsNullOrEmptyEx())
                {
                    if (doc.APPLY_MASTER.Equals("Y"))
                    {
                        this.hddApplyMaster.Value = "Y";
                        this.lblValidated.Text = "Validated by " + doc.UPDATER_ID;
                        this.lblValidated.Attributes.Add("style", "visibility:visible");
                        this.lblValidated.Attributes.Add("style", "color:red");

                    }
                    else
                    {
                        this.hddApplyMaster.Value = "N";
                        this.lblValidated.Visible = false;
                    }
                    if (this.lblStatus.Text == "Currently")
                        this.divReturn.Attributes.CssStyle.Add("display", "block");

                    this.hddCurrentlyProcessID.Value = doc.CURRENTLY_PROCESS_ID; //기등록자료
                }
                else
                {
                    this.hddApplyMaster.Value = "N"; //Pre
                    this.lblValidated.Visible = false;

                }

                webMaster.DocumentNo = doc.DOC_NUM;
            }
        }
        else
            this.RadDateReceived.SelectedDate = DateTime.Now;

    }
    #endregion

    #region DoRequest
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
    #endregion

    #region DoApproval
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

    #region Validation
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            //if (RadtxtCustomer.Text.Length <= 0)
            //    message += "Customer";
            //if (radDropMortgageType.SelectedValue == null)
            //    message += message.IsNullOrEmptyEx() ? "Mortgage Type" : ",Mortgage Type";
            //if (!RadNubookValue.Value.HasValue)
            //    message += message.IsNullOrEmptyEx() ? "Book Value" : ",Book Value";
            //if (!RadNubookValue.Value.HasValue)
            //    message += message.IsNullOrEmptyEx() ? "Revaluation" : ",Revaluation";
            //if (!RadDateReceived.SelectedDate.HasValue)
            //    message += message.IsNullOrEmptyEx() ? "Received Date" : ",Received Date";
            //if (!RadDateIssue.SelectedDate.HasValue)
            //    message += message.IsNullOrEmptyEx() ? "Issue Date" : ",Issue Date";
            //if (!RadDateDue.SelectedDate.HasValue)
            //    message += message.IsNullOrEmptyEx() ? "Due Date" : ",Due Date";
            //if (RadtxtPublisher.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Publisher" : ",Publisher";
            //if (RadtxtPublished.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Published No" : ",Published No";
            //if (this.lblStatus.Text.Equals("Returned"))
            //{
            //    if (!RadDateReturned.SelectedDate.HasValue)
            //        message += (message.IsNullOrEmptyEx() ? "" : ",") + "Returned Date";
            //}
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }
    #endregion

    #region Document Save
    private string DocumentSave(string processStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_MORTGAGE_MANAGMENT doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_MORTGAGE_MANAGMENT();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = this.RadtxtCustomer.Text.Trim() + "/" + this.lblStatus.Text + "/" + this.radDropMortgageType.SelectedText;  // Subject : Customer Name / Status
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        doc.CUSTOMER_CODE = this.hddCustomerCode.Value;
        doc.CUSTOMER_NAME = this.RadtxtCustomer.Text;
        if (this.radBtnPHRDC.Checked) doc.BG_CODE = this.radBtnPHRDC.Value;
        else if (this.radBtnCC.Checked) doc.BG_CODE = this.radBtnCC.Value;
        else if (this.radBtnAH.Checked) doc.BG_CODE = this.radBtnAH.Value;

        doc.STATUS_CODE = this.lblStatus.Text;
        if (this.radDropMortgageType.SelectedValue.Equals(null)) doc.MORTGAGE_TYPE = null; else doc.MORTGAGE_TYPE = this.radDropMortgageType.SelectedValue;

        doc.BOOK_VALUE = (decimal?)this.RadNubookValue.Value;
        doc.REVALUTION_VALUE = (decimal?)this.RadNuRevaluation.Value;
        if (this.radChkCredit.Checked)
            doc.NEW_CREDIT_LIMIT = (decimal?)this.RadNuNewCreditLimit.Value;
        else doc.NEW_CREDIT_LIMIT = null;

        doc.RECEIVED_DATE = this.RadDateReceived.SelectedDate.Value;

        if (this.RadDateReturned.SelectedDate == null) doc.RETURN_DATE = null; else doc.RETURN_DATE = this.RadDateReturned.SelectedDate.Value;
        if (this.RadDateIssue.SelectedDate == null) doc.ISSUE_DATE = null; else doc.ISSUE_DATE = this.RadDateIssue.SelectedDate.Value;
        if (this.RadDateDue.SelectedDate == null) doc.DUE_DATE = null; else doc.DUE_DATE = this.RadDateDue.SelectedDate.Value;

        doc.PUBLISHER = this.RadtxtPublisher.Text;
        doc.PUBLISHED_NUM = this.RadtxtPublished.Text;
        doc.COMMENT = this.RadtxtComment.Text;
        doc.APPLY_MASTER = "N";             //TB_DOC_CANCELATION_MORTGAGE 후처리 반영여부
        doc.CURRENTLY_PROCESS_ID = this.hddCurrentlyProcessID.Value;

        using (Bayer.eWF.BSL.Approval.Mgr.MortgageManagementMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.MortgageManagementMgr())
        {
            processID = mgr.MergeMortgageManagement(doc);
        }

        return processID;

    }
    #endregion
}