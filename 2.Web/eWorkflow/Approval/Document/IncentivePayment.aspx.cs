using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;

public partial class Approval_Document_IncentivePayment : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        if (!IsPostBack)
        {
            hddSchemeProcessId.Value = Request["SchemeProcessId"].NullObjectToEmptyEx();
            hddSchemeDocumentID.Value = "D0013";
            InitPageInfo();
        }
        PageLoadInfo();
        
        
    }

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현        



    }
    #endregion


    #region InitPageInfo
    private void InitPageInfo()
    {
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processId"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0014";
        
        
        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_INCENTIVE_PAYMENT doc;
        IncentivePaymentMgr mgr;

        // scheme에서 최초 넘어왔을때
        if (this.hddSchemeProcessId.Value.IsNotNullOrEmptyEx())
        {
            using (mgr = new IncentivePaymentMgr())
            {
                doc = mgr.SelectIncentivePayment_I(this.hddSchemeProcessId.Value);
                //subject , BU , doc_num
                RadtxtSubject.Text = doc.SUBJECT_SCHEME;
                RadtxtSettlement.Text = doc.SETTLEMENT_TYPE;
                RadtxtDiv.Text = doc.BU_SCHEME;
                lbDocumentNo.Text = doc.DOC_NUM_SCHEME;
            }
        }

        // payment 조회
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (mgr = new IncentivePaymentMgr())
            {
                doc = mgr.SelectIncentivePayment(hddProcessID.Value);
                if (doc != null)
                {
                    if (doc.PROCESS_STATUS != "Saved")
                    {
                        PaymentReadOnly();    
                    }

                    RadtxtSubject.Text = doc.SUBJECT_SCHEME;
                    RadtxtDiv.Text = doc.BU_SCHEME;
                    lbDocumentNo.Text = doc.DOC_NUM_SCHEME;
                    hddSchemeProcessId.Value = doc.SCHEME_DOC_NUM;

                    RadnuTotalAmount.Value = (double ?)doc.TOTAL_AMOUNT;
                    RadtxtDealer.Text = doc.DEALERS_NUM;
                    RadDatePicker1.SelectedDate = doc.PAYMENT_DATE;
                    //RadtxtPaymentSystem.Text = doc.PAYMENT_SYSTEM;
                    //RadtxtPayto.Text = doc.PAY_TO;
                    RadtxtRemark.Text = doc.REMARK;
                    RadtxtSettlement.Text = doc.SETTLEMENT_TYPE;
                    webMaster.DocumentNo = doc.DOC_NUM;
                }
            }
        }
    }

    private void PaymentReadOnly()
    {
        RadnuTotalAmount.ReadOnly = true;
        RadtxtDealer.ReadOnly = true;
        //RadtxtPaymentSystem.ReadOnly = true;
        //RadtxtPayto.ReadOnly = true;
        RadtxtRemark.ReadOnly = true;
        RadDatePicker1.Enabled = false;
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


    #region Validationcheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;
        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            if (RadnuTotalAmount.Text.Length <= 0)
                message += "Total Amount";
            if (RadtxtDealer.Text.Length <=0 )
                message += message.IsNullOrEmptyEx() ? "No. of dealers" : ",No. of dealers";
            //if (RadtxtPaymentSystem.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Payment system" : "Payment system";
            //if (RadtxtPayto.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Pay to" : "Pay to";
            if (RadtxtRemark.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Remark" : "Remark";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    } 
    #endregion

    #region DocumentSave
    private string DocumentSave(string ApprovalStatus)
    {
        DTO_DOC_INCENTIVE_PAYMENT doc = new DTO_DOC_INCENTIVE_PAYMENT();
        string processId = string.Empty;

        doc.PROCESS_ID = this.hddProcessID.Value;        
        doc.PROCESS_STATUS = ApprovalStatus; //
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        doc.SUBJECT = RadtxtSubject.Text ;  // Subject
        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.IS_DISUSED = "N"; //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;
        
        doc.SCHEME_DOC_NUM = hddSchemeProcessId.Value;  // Scheme ProcessID        
        
        doc.TOTAL_AMOUNT = (decimal?)RadnuTotalAmount.Value;
        doc.DEALERS_NUM = RadtxtDealer.Text;
        doc.PAYMENT_DATE = RadDatePicker1.SelectedDate;
        doc.PAYMENT_SYSTEM = "";
        doc.PAY_TO = "";
        doc.REMARK = RadtxtRemark.Text;
        doc.SETTLEMENT_TYPE = RadtxtSettlement.Text;

        using (IncentivePaymentMgr mgr = new IncentivePaymentMgr())
        {
            processId = mgr.MergeIncentivePayment(doc);
        }
        return processId;
        
    } 
    #endregion
}