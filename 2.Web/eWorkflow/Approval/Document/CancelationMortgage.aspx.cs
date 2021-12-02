using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Telerik.Web.UI;
using System.Web.Script.Serialization;

public partial class Approval_Document_CancelationMortgage : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    #region [ PageLoad ]
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
        this.hddDocumentID.Value = "D0032";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //this.hddProcessID.Value = "P000000664";

        InitControls();
    }

    private string GetSelectedType()
    {
        string rtnValue = string.Empty;

        if (this.RadTypeNew.Checked) rtnValue = this.RadTypeNew.Value;
        else if (this.RadTypeReturn.Checked) rtnValue = this.RadTypeReturn.Value;

        return rtnValue;
    }

    private void InitControls()
    {
        if (!this.hddProcessID.Value.IsNullOrEmptyEx())
        {
            using (CancelationMortgageMgr mgr = new CancelationMortgageMgr())
            {
                DTO_DOC_CANCELATION_MORTGAGE doc = mgr.SelectCancelationMortgage(this.hddProcessID.Value);

                if (doc.BU.Equals(this.radRdoCP.Value)) this.radRdoCP.Checked = true;
                else if (doc.BU.Equals(this.radRdoIS.Value)) this.radRdoIS.Checked = true;
                else if (doc.BU.Equals(this.radRdoES.Value)) this.radRdoES.Checked = true;

                if (doc.TYPE.Equals(this.RadTypeNew.Value)) this.RadTypeNew.Checked = true;
                else if (doc.TYPE.Equals(this.RadTypeReturn.Value)) this.RadTypeReturn.Checked = true;

                this.radTxtCustomer.Text = doc.CUSTOMER_NAME;
                this.hddCustomerCode.Value = doc.CUSTOMER_CODE;
                this.radTxtReason.Text = doc.REASON;
                this.radTxtMortgage.Text = doc.MORTGAGE;
                this.radNumAmount.Value = (double?)doc.MORTGAGE_AMOUNT;
                webMaster.DocumentNo = doc.DOC_NUM;
            }
        }
    }

    
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅
        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);
        // 각 문서마다 데이터 로드 조회부분 구현 

    } 
    #endregion

    #region [ 문서상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        
        string message = string.Empty;

        if (GetSelectedBU().IsNullOrEmptyEx())
            message += message.IsNullOrEmptyEx() ? "Business Unit" : ", Business Unit";

        if (this.GetSelectedType().IsNullOrEmptyEx())
            message += message.IsNullOrEmptyEx() ? "Type" : ", Type";

        if (this.radTxtCustomer.Text.IsNullOrEmptyEx())
            message += message.IsNullOrEmptyEx() ? "Customer" : ", Customer";

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {
            if(this.radTxtReason.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Reason";

            if (this.radTxtMortgage.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Mortgage";

            if (!this.radNumAmount.Value.HasValue || this.radNumAmount.Value == 0)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Mortgage Amount";
        }
        
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + message;
            return false;
        }
        else return true;
    }

    /// <summary>
    /// Business Unit 반환
    /// </summary>
    /// <returns></returns>
    private string GetSelectedBU()
    {
        string bg = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    bg = (control as RadButton).Value;
                    break;
                }
            }
        }
        return bg;
    }

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_CANCELATION_MORTGAGE doc = new DTO_DOC_CANCELATION_MORTGAGE();
        string strBU   = GetSelectedBU();
        string strType = GetSelectedType();

        doc.PROCESS_ID = this.hddProcessID.Value;
        doc.SUBJECT = strBU + " / " + strType + " / " + this.radTxtCustomer.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        doc.BU   = strBU;
        doc.TYPE = strType;
        doc.CUSTOMER_CODE = this.hddCustomerCode.Value;
        doc.CUSTOMER_NAME = this.radTxtCustomer.Text;
        doc.REASON = this.radTxtReason.Text;
        doc.MORTGAGE = this.radTxtMortgage.Text;
        doc.MORTGAGE_AMOUNT = (decimal?)this.radNumAmount.Value;
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        using (CancelationMortgageMgr mgr = new CancelationMortgageMgr())
        {
            return mgr.MergeCancelationMortgage(doc);
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