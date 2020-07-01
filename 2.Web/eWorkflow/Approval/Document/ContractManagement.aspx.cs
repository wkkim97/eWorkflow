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

public partial class Approval_Document_ContractManagement : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0024";
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        //hddProcessID.Value = "P000000489";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {

        DTO_DOC_CONTRACT_MANAGEMENT doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.ContractManagementMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.ContractManagementMgr())
            {
                doc = mgr.SelectContractManagement(hddProcessID.Value);
            }

            if (doc != null)
            {
                if (doc.TYPE == radRdoStandard.Value)
                    this.radRdoStandard.Checked = true;
                else if (doc.TYPE == radRdoNonStandard.Value)
                    this.radRdoNonStandard.Checked = true;
                if (doc.ESSENTIAL_CONTRACT == "Y")
                    this.radRdoEssYes.Checked = true;                     
                else if (doc.ESSENTIAL_CONTRACT == "N")
                    this.radRdoEssNo.Checked = true;
                if (doc.INTRAGROUP == "Y")
                    this.radRdoIntraYes.Checked = true;
                else if (doc.INTRAGROUP == "N")
                    this.radRdoIntraNo.Checked = true;
                if (doc.CROSS_BORDER == "Y")
                    this.radRdoCrossYes.Checked = true;
                else if (doc.CROSS_BORDER == "N")
                    this.radRdoCrossNo.Checked = true;
                //if (doc.CONTRACT_TOTAL_VALUE == radRdoTotalValue1.Value)
                //    this.radRdoTotalValue1.Checked = true;
                //else if (doc.CONTRACT_TOTAL_VALUE == radRdoTotalValue2.Value)
                //    this.radRdoTotalValue2.Checked = true;

                this.radTxtTitle.Text = doc.TITLE;
                this.radDateFrom.SelectedDate = doc.TERM_FROM;
                this.radDateTo.SelectedDate = doc.TERM_TO;
                this.radTxtContractPartner.Text = doc.CONTRACT_PARTNER;
                this.radTxtContractValue.Value = (double?)doc.CONTRACT_VALUE;

                if (doc.CONTRACT_CATEGORY == radRdoCategory1.Value)
                    this.radRdoCategory1.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory2.Value)
                    this.radRdoCategory2.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory3.Value)
                    this.radRdoCategory3.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory4.Value)
                    this.radRdoCategory4.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory5.Value)
                    this.radRdoCategory5.Checked = true;

                if (doc.PRIVACY_INFORMATION == "Y" || doc.PRIVACY_INFORMATION == "N" )
                {
                    this.radRdoPrivacyInformationYes.Visible = true;
                    this.radRdoPrivacyInformationNo.Visible  = true;

                    if (doc.PRIVACY_INFORMATION == radRdoPrivacyInformationYes.Value)
                        this.radRdoPrivacyInformationYes.Checked = true;
                    else if (doc.PRIVACY_INFORMATION == radRdoPrivacyInformationNo.Value)
                        this.radRdoPrivacyInformationNo.Checked = true;
                }

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
            if(!(radRdoStandard.Checked || radRdoNonStandard.Checked))
                message += "Contract Type";
            if(!(radRdoEssYes.Checked || radRdoEssNo.Checked))
                message += message.IsNullOrEmptyEx() ? "Essiontial Contract" : ",Essiontial Contract";
            if (!(radRdoIntraYes.Checked || radRdoIntraNo.Checked))
                message += message.IsNullOrEmptyEx() ? "Intragroup" : ",Intragroup";
            if(!(radRdoCrossYes.Checked || radRdoCrossNo.Checked))
                message += message.IsNullOrEmptyEx() ? "Cross-Border" : ",Cross_Border";
            if (!(radRdoCategory1.Checked || radRdoCategory2.Checked || radRdoCategory3.Checked || radRdoCategory4.Checked || radRdoCategory5.Checked))
                message += message.IsNullOrEmptyEx() ? "Contract Category" : ", Contrac Category";
            //if (!(radRdoPrivacyInformationYes.Checked || radRdoPrivacyInformationNo.Checked))
            //    message += message.IsNullOrEmptyEx() ? "Contracts related privacy information" : ", Contracts related privacy informationn";
            if(this.radTxtTitle.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Title" : ",Title";
            if (!radDateFrom.SelectedDate.HasValue)
                message += message.IsNullOrEmptyEx() ? "Term From" : ",Term From";
            //if(!radDateTo.SelectedDate.HasValue)
            //    message += message.IsNullOrEmptyEx() ? "Term To" : ",Term To";
            if (radTxtContractPartner.Text.Length <= 0)
            message += message.IsNullOrEmptyEx() ? "Contract Partner" : ",Contract Partner";
            if (radTxtContractValue.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Contract Value" : ",Contract Value";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private string GetType()
    {
        string type = string.Empty;
        if (this.radRdoStandard.Checked) type = this.radRdoStandard.Text;
        else if (this.radRdoNonStandard.Checked) type = this.radRdoNonStandard.Text;
        return type;
    }

    private string GetCategory()
    {
        string category = string.Empty;
        if (this.radRdoCategory1.Checked) category = this.radRdoCategory1.Text;
        else if (this.radRdoCategory2.Checked) category = this.radRdoCategory2.Text;
        else if (this.radRdoCategory3.Checked) category = this.radRdoCategory3.Text;
        else if (this.radRdoCategory4.Checked) category = this.radRdoCategory4.Text;
        else if (this.radRdoCategory5.Checked) category = this.radRdoCategory5.Text;
        return category;
    }

    private string DocumentSave(string processStatus)
    {

        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CONTRACT_MANAGEMENT doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_CONTRACT_MANAGEMENT();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = GetType() + "/" + GetCategory() + "/" + this.radTxtTitle.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        if (this.radRdoStandard.Checked)
            doc.TYPE = radRdoStandard.Value;
        else if (this.radRdoNonStandard.Checked)
            doc.TYPE = radRdoNonStandard.Value;

        if (this.radRdoEssYes.Checked)
            doc.ESSENTIAL_CONTRACT = radRdoEssYes.Value;
        if (this.radRdoEssNo.Checked)
            doc.ESSENTIAL_CONTRACT = radRdoEssNo.Value;

        if (this.radRdoIntraYes.Checked)
            doc.INTRAGROUP = radRdoIntraYes.Value;
        if (this.radRdoIntraNo.Checked)
            doc.INTRAGROUP = radRdoIntraNo.Value;

        if (this.radRdoCrossYes.Checked)
            doc.CROSS_BORDER = radRdoCrossYes.Value;
        if (this.radRdoCrossNo.Checked)
            doc.CROSS_BORDER = radRdoCrossNo.Value;

        //if (this.radRdoTotalValue1.Checked)
        //    doc.CONTRACT_TOTAL_VALUE = radRdoTotalValue1.Value;
        //else if (this.radRdoTotalValue2.Checked)
        //    doc.CONTRACT_TOTAL_VALUE = radRdoTotalValue2.Value;
        doc.CONTRACT_TOTAL_VALUE = null;
        doc.TITLE = radTxtTitle.Text;
        doc.TERM_FROM = radDateFrom.SelectedDate;
        doc.TERM_TO = radDateTo.SelectedDate;
        doc.CONTRACT_PARTNER = radTxtContractPartner.Text;
        doc.CONTRACT_VALUE = Convert.ToDecimal(radTxtContractValue.Text);

        if (this.radRdoCategory1.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory1.Value;
        else if (this.radRdoCategory2.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory2.Value;
        else if (this.radRdoCategory3.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory3.Value;
        else if (this.radRdoCategory4.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory4.Value;
        else if (this.radRdoCategory5.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory5.Value;

        if (this.radRdoPrivacyInformationYes.Checked)
            doc.PRIVACY_INFORMATION = radRdoPrivacyInformationYes.Value;
        else if (this.radRdoPrivacyInformationNo.Checked)
            doc.PRIVACY_INFORMATION = radRdoPrivacyInformationNo.Value;



        using (Bayer.eWF.BSL.Approval.Mgr.ContractManagementMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.ContractManagementMgr())
        {
            processID = mgr.MergeContractManagement(doc);
        }

        return processID;

    }
}