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
using Telerik.Web.UI;

public partial class Approval_Document_CsCreditLimitApproval : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        this.radBtnCustomer.Checked = true;
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0046";
        //hddProcessID.Value = "P000000370";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        this.radtxtCustomer.ReadOnly = true;

        DTO_DOC_CS_CREDIT_LIMIT_APPROVAL doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CsCreditLimitApprovalMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CsCreditLimitApprovalMgr())
            {
                doc = mgr.SelectCsCreditLimitApproval(hddProcessID.Value);

                if (doc.IS_ALL.Equals("Y"))
                {
                    this.radBtnAllCustomer.Checked = true;
                    this.radBtnCustomer.Checked = false;
                }
                else
                {
                    this.radBtnAllCustomer.Checked = false;
                    this.radBtnCustomer.Checked = true;
                }

                if (doc != null)
                {

                    if (doc.BU == "CP")
                        radBtnBuCP.Checked = true;
                    else if (doc.BU == "IB")
                        radBtnBuIB.Checked = true;
                    else if (doc.BU == "ES")
                        radBtnBuES.Checked = true;
                    else if (doc.BU == "BVS")
                        radBtnBuBVS.Checked = true;

                    radtxtCustomer.Text = doc.CUSTOMER_NAME;
                    radtxtAmount.Text = doc.AMOUNT.ToString();
                    radtxtDescription.Text = doc.DESCRIPTION;
                    webMaster.DocumentNo = doc.DOC_NUM;

                }

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
        string isAll = "Y";
        if (this.radBtnAllCustomer.Checked) isAll = "Y";
        else isAll = "N";
        if (!ClientScript.IsStartupScriptRegistered("setVisibleControl"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleControl", "setVisibleControl('" + isAll + "');", true);

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
            if (!(radBtnAllCustomer.Checked || radBtnCustomer.Checked))
                message += "Type";
            if (this.radBtnCustomer.Checked)
            {
                if (this.radtxtCustomer.Text.Length <= 0)
                    message += message.IsNullOrEmptyEx() ? "Customer code & name" : ", Customer code & name";
            }
            if (!(radBtnBuCP.Checked || radBtnBuIB.Checked || radBtnBuES.Checked || radBtnBuBVS.Checked))
                message += message.IsNullOrEmptyEx() ? "Business Group" : ", Business Group";
            if (radtxtAmount.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Requested Credit Limit" : ", Requested Credit Limit";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private string GetSelectedType()
    {
        string type = string.Empty;
        if (this.radBtnAllCustomer.Checked) type = this.radBtnAllCustomer.Value;
        else if (this.radBtnCustomer.Checked) type = this.radBtnCustomer.Value;
        return type;
    }

    private string GetSelectedDivision()
    {
        string bu = string.Empty;
        foreach (Control control in divBU.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    bu = (control as RadButton).Value;
                    break;
                }
            }
        }
        return bu;
    }

    private string DocumentSave(string processStatus)
    {


        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CS_CREDIT_LIMIT_APPROVAL doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CS_CREDIT_LIMIT_APPROVAL();
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

        if (radBtnAllCustomer.Checked)
        {
            doc.IS_ALL = "Y";
            doc.CUSTOMER_CODE = null;
            doc.CUSTOMER_NAME = null;
            doc.SUBJECT = GetSelectedType() + " / " + GetSelectedDivision();
        }
        else if (radBtnCustomer.Checked)
        {
            doc.IS_ALL = "N";
            doc.CUSTOMER_CODE = hddCustomerCode.Value;
            doc.CUSTOMER_NAME = radtxtCustomer.Text;
            doc.SUBJECT = GetSelectedType() + " / " + GetSelectedDivision() + " / " + this.radtxtCustomer.Text; //subject
        }

        webMaster.Subject = doc.SUBJECT;

        if (radBtnBuCP.Checked)
            doc.BU = radBtnBuCP.Value;
        else if (radBtnBuIB.Checked)
            doc.BU = radBtnBuIB.Value;
        else if (radBtnBuES.Checked)
            doc.BU = radBtnBuES.Value;
        else if (radBtnBuBVS.Checked)
            doc.BU = radBtnBuBVS.Value;

        if (radtxtAmount.Text.Length > 0)
            doc.AMOUNT = Convert.ToDecimal(radtxtAmount.Text);
        else
            doc.AMOUNT = null;

        doc.DESCRIPTION = radtxtDescription.Text;

        using (Bayer.eWF.BSL.Approval.Mgr.CsCreditLimitApprovalMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CsCreditLimitApprovalMgr())
        {
            processID = mgr.MergeCsCreditLimitApproval(doc);
        }

        return processID;

    }
}
