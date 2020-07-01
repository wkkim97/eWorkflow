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

public partial class Approval_Document_EarlyPaymentDiscount : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        this.hddDocumentID.Value = "D0022";
        //this.hddProcessID.Value = "P000000388";
    }

    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (EarlyPaymentDiscountMgr mgr = new EarlyPaymentDiscountMgr())
            {
                DTO_DOC_EARLY_PAYMENT_DISCOUNT doc = mgr.SelectEarlyPaymentDiscount(this.hddProcessID.Value);

                this.radNumTotalAmount.Value = (double?)doc.TOTAL_AMOUNT;
                this.radTxtDealersNo.Text = doc.DEALERS_NUM;
                this.radDatPayment.SelectedDate = doc.PAYMENT_DATE;
            }
        }
    }

    #region [ 문서상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;
        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if (!this.radNumTotalAmount.Value.HasValue)
                message += "Total Amount";

            if (this.radTxtDealersNo.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "No. of dealers";

            if (!this.radDatPayment.SelectedDate.HasValue)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Payment Date";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_EARLY_PAYMENT_DISCOUNT doc = new DTO_DOC_EARLY_PAYMENT_DISCOUNT();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string subject = string.Empty;

        if (this.radDatPayment.SelectedDate.HasValue)
            subject = this.radDatPayment.SelectedDate.Value.ToString("yyyy-MM-dd");
        //if (this.radNumTotalAmount.Value.IsNotNullOrEmptyEx() && this.radNumTotalAmount.Value > 0)
        //    subject += "TotalAmount(" + ((decimal)this.radNumTotalAmount.Value).ToString("#,##0") + ")";

        doc.SUBJECT = subject;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.TOTAL_AMOUNT = (decimal?)this.radNumTotalAmount.Value;
        doc.DEALERS_NUM = this.radTxtDealersNo.Text;
        doc.PAYMENT_DATE = this.radDatPayment.SelectedDate;

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        using (EarlyPaymentDiscountMgr mgr = new EarlyPaymentDiscountMgr())
        {
            return mgr.MergeEarlyPaymentDiscount(doc);
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