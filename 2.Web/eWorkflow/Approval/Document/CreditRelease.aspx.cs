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

public partial class Approval_Document_CreditRelease : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                //NumberFormatiChange();
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
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0008";
       // hddProcessID.Value = "P000000366";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        this.radtxtOverDue.ReadOnly = true;
        this.radtxtExceededAmount.ReadOnly = true;
        this.radtxtCreditLimit.ReadOnly = true;
        //this.radtxtCollateral.ReadOnly = true;

        DTO_DOC_CREDIT_RELEASE doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CreditReleaseMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CreditReleaseMgr())
            {
                doc = mgr.SelectCreditRelease(hddProcessID.Value);
            }

            if (doc != null)
            {
                radtxtOrderNo.Text = doc.ORDER_NUM;

                if (doc.BU == "PH")
                    radBtnBuPH.Checked = true;
                else if (doc.BU == "CH") {
                    radBtnBuCH.Checked = true;
                    radBtnBuCH.Visible = true;

                    radBtnBuCC.Checked = false;
                    radBtnBuCC.Visible= false;
                }
                else if (doc.BU == "DC")
                    radBtnBuDC.Checked = true;
                else if (doc.BU == "AH")
                    radBtnBuAH.Checked = true;
                else if (doc.BU == "CC")
                {
                    radBtnBuCH.Checked = false;
                    radBtnBuCH.Visible = false;

                    radBtnBuCC.Checked = true;
                    radBtnBuCC.Visible = true;
                }


                radtxtCustomer.Text = doc.CUSTOMER_NAME;
                radNumAmount.Value = (double?)doc.AMOUNT;
                radtxtInclNote.Text = doc.INCL_NOTE_AMOUNT.ToString();
                radtxtPaymentTerm.Text = doc.PAYMENT_TERM.ToString();
                radtxtAgingDay.Text = doc.AGING_DAY.ToString();
                radtxtOverDue.Text = doc.OVERDUE.ToString();
                radtxtCreditLimit.Text = doc.CREDIT_LIMIT.ToString();
                radtxtOpenOrder.Text = doc.OPEN_ORDER.ToString();
                radtxtOrdered.Text = doc.ORDERED_AMOUNT.ToString();
                radtxtExceededAmount.Text = doc.EXCEEDED_AMOUNT.ToString();
                radtxtCollateral.Text = doc.COLLATERAL_AMOUNT.ToString();
                RadDropReason.SelectedValue = doc.REASON;
                radtxtExplanation.Text = doc.EXPLANATION_OF_REASON;
                webMaster.DocumentNo = doc.DOC_NUM;

            }
        }
    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

    }
    #endregion

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

    protected override void DoApproval()
    {
        // TODO :

        base.DoApproval();
    }

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

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (radtxtOrderNo.Text.Length <= 0)
                message += "SAP Order No";

            if (!(radBtnBuPH.Checked || radBtnBuCH.Checked || radBtnBuDC.Checked || radBtnBuAH.Checked || radBtnBuCC.Checked ))
                message += message.IsNullOrEmptyEx() ? "Division" : ",Division";

            if (radtxtCustomer.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Customer" : ",Customer";

            //Reason dropdown list
            if (this.RadDropReason.SelectedValue.IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "Reason" : ",Reason";
            

            //if (radNumAmount.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Total Recivable" : ",Total Recivable";

            //if (radtxtInclNote.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Incl Note" : ",Incl Note";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private string DocumentSave(string processStatus)
    {


        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CREDIT_RELEASE doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CREDIT_RELEASE();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;

       

        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        doc.ORDER_NUM = radtxtOrderNo.Text;

        if (radBtnBuPH.Checked)
            doc.BU = radBtnBuPH.Value;
        else if (radBtnBuCH.Checked)
            doc.BU = radBtnBuCH.Value;
        else if (radBtnBuDC.Checked)
            doc.BU = radBtnBuDC.Value;
        else if (radBtnBuAH.Checked)
            doc.BU = radBtnBuAH.Value;
        else if (radBtnBuCC.Checked)
            doc.BU = radBtnBuCC.Value;
        
        string customerName = "";
        if (radtxtCustomer.Text.Length > 0)
        {
            customerName = this.radtxtCustomer.Text.Substring(0, radtxtCustomer.Text.LastIndexOf("("));
            doc.SUBJECT = customerName + "/" + doc.BU + "/" + this.radtxtOrderNo.Text;
        }
        else
            doc.SUBJECT = radtxtCustomer.Text + "/" + doc.BU + "/" + this.radtxtOrderNo.Text;
                
         webMaster.Subject = doc.SUBJECT;

        doc.CUSTOMER_NAME = radtxtCustomer.Text;
        doc.CUSTOMER_CODE = hddCustomerCode.Value;
        doc.AMOUNT = (decimal?)radNumAmount.Value;
        doc.INCL_NOTE_AMOUNT = Convert.ToDecimal(radtxtInclNote.Text);
        doc.PAYMENT_TERM = Convert.ToInt32(radtxtPaymentTerm.Text);
        doc.AGING_DAY = Convert.ToInt32(radtxtAgingDay.Text);
        doc.OVERDUE = Convert.ToInt32(radtxtOverDue.Text);
        doc.CREDIT_LIMIT = Convert.ToDecimal(radtxtCreditLimit.Text);
        doc.OPEN_ORDER = Convert.ToDecimal(radtxtOpenOrder.Text);
        doc.ORDERED_AMOUNT = Convert.ToDecimal(radtxtOrdered.Text);
        doc.EXCEEDED_AMOUNT = Convert.ToDecimal(radtxtExceededAmount.Text);
        doc.COLLATERAL_AMOUNT = Convert.ToDecimal(radtxtCollateral.Text);
        doc.REASON = RadDropReason.SelectedValue;
        doc.EXPLANATION_OF_REASON = radtxtExplanation.Text;   

        using (Bayer.eWF.BSL.Approval.Mgr.CreditReleaseMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CreditReleaseMgr())
        {
            processID = mgr.MergeCreditRelease(doc);
        }

        return processID;

    }

    private void NumberFormatiChange()
    {
        this.radtxtOverDue.NumberFormat.DecimalDigits = 0;
        this.radtxtOverDue.NumberFormat.GroupSizes = 3;
        this.radtxtOverDue.NumberFormat.GroupSeparator = ",";
    }

}