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

public partial class Approval_Document_BCSCreditLimitSetting : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        //this.radBtnCustomer.Checked = true;
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0059";
        //hddProcessID.Value = "P000000370";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        this.radtxtCustomer.ReadOnly = true;

        DTO_DOC_CREDIT_LIMIT_SETTING doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CreditLimitSettingMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CreditLimitSettingMgr())
            {
                doc = mgr.SelectCreditLimitSetting(hddProcessID.Value);

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



                    // Divison CS 추가 2017-01-24
                    // Business Group 추가(CP,IB,ES,BVS)
                    // Category 추가 (Collective, New, Increase)
                    //if (doc.BU == this.radBtnBuCS.Value)
                    //{
                    //    this.radBtnBuCS.Checked = true;
                    //    if (doc.BUSINESS_GROUP == this.radBtnSub_CP.Value)
                    //        this.radBtnSub_CP.Checked = true;
                    //    else if (doc.BUSINESS_GROUP == this.radBtnSub_IB.Value)
                    //        this.radBtnSub_IB.Checked = true;
                    //    else if (doc.BUSINESS_GROUP == this.radBtnSub_ES.Value)
                    //        this.radBtnSub_ES.Checked = true;
                    //    
                    //
                    //   if (doc.CATEGORY == this.radBtnCategoryNew.Value)
                    //        this.radBtnCategoryNew.Checked = true;
                    //    else if (doc.CATEGORY == this.radBtnCategoryIncrease.Value)
                    //        this.radBtnCategoryIncrease.Checked = true;
                    //}

                    //eWorkflow Optimization 2020
                    radBtnBuCS.Checked = true;
                    if (doc.BUSINESS_GROUP == this.radBtnSub_CP.Value)
                        this.radBtnSub_CP.Checked = true;
                    else if (doc.BUSINESS_GROUP == this.radBtnSub_IB.Value)
                        this.radBtnSub_IB.Checked = true;
                    else if (doc.BUSINESS_GROUP == this.radBtnSub_ES.Value)
                        this.radBtnSub_ES.Checked = true;

                    if (doc.TYPE == this.radBtnTypeCredit.Value)
                        radBtnTypeCredit.Checked = true;
                    else if (doc.TYPE == this.radBtnTypeMortgage.Value)
                        radBtnTypeMortgage.Checked = true;

                    if (doc.CATEGORY == this.radBtnCategoryNew.Value)
                        this.radBtnCategoryNew.Checked = true;
                    else if (doc.CATEGORY == this.radBtnCategoryIncrease.Value)
                        this.radBtnCategoryIncrease.Checked = true;

                    radtxtCustomer.Text = doc.CUSTOMER_NAME;
                    radtxtAmount.Text = doc.AMOUNT.ToString();
                    radtxtDescription.Text = doc.DESCRIPTION;
                    webMaster.DocumentNo = doc.DOC_NUM;


                    //if (!ClientScript.IsStartupScriptRegistered("setVisibleCategory"))
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleCategory", "setVisibleCategory('CS','" + "" + "','" +"" + "');", true);

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

        //if (!ClientScript.IsStartupScriptRegistered("setVisibleCategory"))
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "setVisibleCategory", "setVisibleCategory('" + GetSelectedDivision() + "','" + "" + "','" + "" + "');", true);


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
            if(!(radBtnAllCustomer.Checked || radBtnCustomer.Checked))
                message += "Type";
            if(this.radBtnCustomer.Checked)
            {
                if (this.radtxtCustomer.Text.Length <= 0)
                    message += message.IsNullOrEmptyEx() ? "Customer No. & Name" : ", Customer No. & Name";
            }
            
            if (radtxtAmount.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Credit Limit Amount" : ", Credit Limit Amount";
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


        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CREDIT_LIMIT_SETTING doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CREDIT_LIMIT_SETTING();
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

        doc.BU = radBtnBuCS.Value;
    
        if (radBtnSub_CP.Checked)
            doc.BUSINESS_GROUP = radBtnSub_CP.Value;
        else if (radBtnSub_IB.Checked)
            doc.BUSINESS_GROUP = radBtnSub_IB.Value;
        else if (radBtnSub_ES.Checked)
            doc.BUSINESS_GROUP = radBtnSub_ES.Value;



        if (radBtnTypeCredit.Checked)
            doc.TYPE = radBtnTypeCredit.Value;
        else if (radBtnTypeMortgage.Checked)
            doc.TYPE = radBtnTypeMortgage.Value;



        if (radBtnCustomer.Checked) { 
            if (radBtnCategoryNew.Checked)
                doc.CATEGORY = radBtnCategoryNew.Value;
            else if (radBtnCategoryIncrease.Checked)
                doc.CATEGORY = radBtnCategoryIncrease.Value;
        }
        

        

        


        doc.SUBJECT = GetSelectedType() + " / " + GetSelectedDivision();

        if (radBtnSub_CP.Checked || radBtnSub_IB.Checked || radBtnSub_ES.Checked )
            doc.SUBJECT = doc.SUBJECT + " / " + doc.BUSINESS_GROUP;

        if ( radBtnCategoryNew.Checked || radBtnCategoryIncrease.Checked)
            doc.SUBJECT = doc.SUBJECT + " / " + doc.CATEGORY;

        if (radBtnAllCustomer.Checked)
        {
            doc.IS_ALL = "Y";
            doc.CUSTOMER_CODE = null;
            doc.CUSTOMER_NAME = null;
        }
        else if (radBtnCustomer.Checked)
        {
            doc.IS_ALL = "N";
            doc.CUSTOMER_CODE = hddCustomerCode.Value;
            doc.CUSTOMER_NAME = radtxtCustomer.Text;
            doc.SUBJECT = doc.SUBJECT + " / " + this.radtxtCustomer.Text; //subject

        }

        webMaster.Subject = doc.SUBJECT;

        if (radtxtAmount.Text.Length > 0)
            doc.AMOUNT = Convert.ToDecimal(radtxtAmount.Text);
        else
            doc.AMOUNT = null;

        doc.DESCRIPTION = radtxtDescription.Text;
       
        using (Bayer.eWF.BSL.Approval.Mgr.CreditLimitSettingMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CreditLimitSettingMgr())
        {
            processID = mgr.MergeCreditLimitSetting(doc);
        }

        return processID;

    }
}
