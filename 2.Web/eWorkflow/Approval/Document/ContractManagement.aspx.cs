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
                else if (doc.TYPE == radRdoD2D.Value)
                    this.radRdoD2D.Checked = true;
                else if (doc.TYPE == radRdoPIPA.Value)
                    this.radRdoPIPA.Checked = true;

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
                else if (doc.CONTRACT_CATEGORY == radRdoCategory6.Value)
                    this.radRdoCategory6.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory7.Value)
                    this.radRdoCategory7.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory8.Value)
                    this.radRdoCategory8.Checked = true;
                else if (doc.CONTRACT_CATEGORY == radRdoCategory9.Value)
                    this.radRdoCategory9.Checked = true;                
                //INC0004937695
                else if (doc.CONTRACT_CATEGORY == radRdoCategory10.Value)
                    this.radRdoCategory10.Checked = true;

                if (doc.PRIVACY_INFORMATION == "Y" || doc.PRIVACY_INFORMATION == "N" )
                {
                    this.radRdoPrivacyInformationYes.Visible = true;
                    this.radRdoPrivacyInformationNo.Visible  = true;

                    if (doc.PRIVACY_INFORMATION == radRdoPrivacyInformationYes.Value)
                        this.radRdoPrivacyInformationYes.Checked = true;
                    else if (doc.PRIVACY_INFORMATION == radRdoPrivacyInformationNo.Value)
                        this.radRdoPrivacyInformationNo.Checked = true;
                }
                if (doc.TYPE == radRdoPIPA.Value)
                {
                    this.radTxtContractValue.Value = 0;
                    if (doc.PIPA_PURPOSE == radRdoPIPAPurpose1.Value)
                        this.radRdoPIPAPurpose1.Checked=true;
                    else if (doc.PIPA_PURPOSE == radRdoPIPAPurpose2.Value)
                        this.radRdoPIPAPurpose2.Checked = true;
                    else if (doc.PIPA_PURPOSE == radRdoPIPAPurpose3.Value)
                        this.radRdoPIPAPurpose3.Checked = true;

                    
                    radTextPIPAEvent.Text = doc.PIPA_EVENT ;
                    radTextPIPAContract.Text = doc.PIPA_CONTRACT;

                    string[] pipa_purpose_pis = new string[] { };
                    string[] pipa_targets = new string[] { };
                    string[] pipa_collections = new string[] { };
                    string[] pipa_archivings = new string[] { };

                    if (doc.PIPA_PURPOSE_PI.IsNotNullOrEmptyEx())
                    {
                        pipa_purpose_pis = doc.PIPA_PURPOSE_PI.Split(new string[] { "|" }, StringSplitOptions.None);
                    }
                    if (doc.PIPA_TARGET.IsNotNullOrEmptyEx())
                    {
                        pipa_targets = doc.PIPA_TARGET.Split(new string[] { "|" }, StringSplitOptions.None);
                    }
                    if (doc.PIPA_COLLECTION.IsNotNullOrEmptyEx())
                    {
                        pipa_collections = doc.PIPA_COLLECTION.Split(new string[] { "|" }, StringSplitOptions.None);
                    }
                    if (doc.PIPA_ARCHIVING.IsNotNullOrEmptyEx())
                    {
                        pipa_archivings = doc.PIPA_ARCHIVING.Split(new string[] { "|" }, StringSplitOptions.None);
                    }
                    for (int i = 0; i < pipa_purpose_pis.Length; i++)
                    {
                        if (pipa_purpose_pis[i].ToString().Equals(this.radChkPIPAPI1.Value))
                            this.radChkPIPAPI1.Checked = true;
                        if (pipa_purpose_pis[i].ToString().Equals(this.radChkPIPAPI2.Value))
                            this.radChkPIPAPI2.Checked = true;
                        if (pipa_purpose_pis[i].ToString().Equals(this.radChkPIPAPI3.Value))
                            this.radChkPIPAPI3.Checked = true;
                        if (pipa_purpose_pis[i].ToString().Equals(this.radChkPIPAPI4.Value))
                            this.radChkPIPAPI4.Checked = true;
                        if (pipa_purpose_pis[i].ToString().Equals(this.radChkPIPAPI5.Value))
                            this.radChkPIPAPI5.Checked = true;
                    }

                    for (int i = 0; i < pipa_targets.Length; i++)
                    {
                        if (pipa_targets[i].ToString().Equals(this.radChkPIPATarget1.Value))
                            this.radChkPIPATarget1.Checked = true;
                        if (pipa_targets[i].ToString().Equals(this.radChkPIPATarget2.Value))
                            this.radChkPIPATarget2.Checked = true;
                        if (pipa_targets[i].ToString().Equals(this.radChkPIPATarget3.Value))
                            this.radChkPIPATarget3.Checked = true;
                        if (pipa_targets[i].ToString().Equals(this.radChkPIPATarget4.Value))
                            this.radChkPIPATarget4.Checked = true;
                        if (pipa_targets[i].ToString().Equals(this.radChkPIPATarget5.Value))
                            this.radChkPIPATarget5.Checked = true;
                    }
                    for (int i = 0; i < pipa_collections.Length; i++)
                    {
                        if (pipa_collections[i].ToString().Equals(this.radChkPIPACollection1.Value))
                            this.radChkPIPACollection1.Checked = true;
                        if (pipa_collections[i].ToString().Equals(this.radChkPIPACollection2.Value))
                            this.radChkPIPACollection2.Checked = true;
                        if (pipa_collections[i].ToString().Equals(this.radChkPIPACollection3.Value))
                            this.radChkPIPACollection3.Checked = true;
                        if (pipa_collections[i].ToString().Equals(this.radChkPIPACollection4.Value))
                            this.radChkPIPACollection4.Checked = true;                        
                    }
                    for (int i = 0; i < pipa_archivings.Length; i++)
                    {
                        if (pipa_archivings[i].ToString().Equals(this.radCheckPIPAArchiving1.Value))
                            this.radCheckPIPAArchiving1.Checked = true;
                        if (pipa_archivings[i].ToString().Equals(this.radCheckPIPAArchiving2.Value))
                            this.radCheckPIPAArchiving2.Checked = true;
                        if (pipa_archivings[i].ToString().Equals(this.radCheckPIPAArchiving3.Value))
                            this.radCheckPIPAArchiving3.Checked = true;
                    }
                    
                    radTextPIPAPI6.Text = doc.PIPA_PURPOSE_PI_OTHER ;
                    radTextPIPAArchiving4.Text = doc.PIPA_ARCHIVING_OTHER;
                    radTextPIPAPermission.Text = doc.PIPA_PERMISSION ;
                    radTextPIPAVolume.Text = doc.PIPA_VOLUMN ;
                    radTextPIPARetention.Text = doc.PIPA_RETENTION ;
                    radTextPIPA3RDPARTY.Text = doc.PIPA_3RDPARTY ;
                    radTextPIPAOversea.Text = doc.PIPA_OVERSEA ;
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
            if(!(radRdoStandard.Checked || radRdoNonStandard.Checked || radRdoD2D.Checked || radRdoPIPA.Checked) )
                message += "Contract Type";
            if(!(radRdoEssYes.Checked || radRdoEssNo.Checked))
                message += message.IsNullOrEmptyEx() ? "Essiontial Contract" : ",Essiontial Contract";
            if (!(radRdoIntraYes.Checked || radRdoIntraNo.Checked))
                message += message.IsNullOrEmptyEx() ? "Intragroup" : ",Intragroup";
            if(!(radRdoCrossYes.Checked || radRdoCrossNo.Checked))
                message += message.IsNullOrEmptyEx() ? "Cross-Border" : ",Cross_Border";
            //PIPA 의 경우느 아래 내용 check 안함
            if (!radRdoPIPA.Checked)
            {
                //radRdoCategory10
                if (!(radRdoCategory1.Checked || radRdoCategory2.Checked || radRdoCategory3.Checked || radRdoCategory4.Checked || radRdoCategory5.Checked || radRdoCategory6.Checked || radRdoCategory7.Checked || radRdoCategory8.Checked || radRdoCategory9.Checked || radRdoCategory10.Checked))
                    message += message.IsNullOrEmptyEx() ? "Contract Category" : ", Contrac Category";
                //if (!(radRdoPrivacyInformationYes.Checked || radRdoPrivacyInformationNo.Checked))
                //    message += message.IsNullOrEmptyEx() ? "Contracts related privacy information" : ", Contracts related privacy informationn";
                if (this.radTxtTitle.Text.Length <= 0)
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
        else if (this.radRdoCategory6.Checked) category = this.radRdoCategory6.Text;
        else if (this.radRdoCategory7.Checked) category = this.radRdoCategory7.Text;
        else if (this.radRdoCategory8.Checked) category = this.radRdoCategory8.Text;
        else if (this.radRdoCategory9.Checked) category = this.radRdoCategory9.Text;
        //INC0004937695 
        else if (this.radRdoCategory10.Checked) category = this.radRdoCategory10.Text;

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
        else if (this.radRdoD2D.Checked)
            doc.TYPE = radRdoD2D.Value;
        else if (this.radRdoPIPA.Checked)
            doc.TYPE = radRdoPIPA.Value;

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
        doc.CONTRACT_VALUE = 0;
        if (radTxtContractValue.Text.Length > 0)
        {
            doc.CONTRACT_VALUE = Convert.ToDecimal(radTxtContractValue.Text);
        }
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
        else if (this.radRdoCategory6.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory6.Value;
        else if (this.radRdoCategory7.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory7.Value;
        else if (this.radRdoCategory8.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory8.Value;
        else if (this.radRdoCategory9.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory9.Value;
        //INC0004937695 
        else if (this.radRdoCategory10.Checked)
            doc.CONTRACT_CATEGORY = radRdoCategory10.Value;



        if (this.radRdoPrivacyInformationYes.Checked)
            doc.PRIVACY_INFORMATION = radRdoPrivacyInformationYes.Value;
        else if (this.radRdoPrivacyInformationNo.Checked)
            doc.PRIVACY_INFORMATION = radRdoPrivacyInformationNo.Value;


        //PIPA 추가
        if (this.radRdoPIPA.Checked)
        {
            if (this.radRdoPIPAPurpose1.Checked)
                doc.PIPA_PURPOSE = radRdoPIPAPurpose1.Value;
            else if(this.radRdoPIPAPurpose2.Checked)
                doc.PIPA_PURPOSE = radRdoPIPAPurpose2.Value;
            else if (this.radRdoPIPAPurpose3.Checked)
                doc.PIPA_PURPOSE = radRdoPIPAPurpose3.Value;

            doc.PIPA_EVENT = radTextPIPAEvent.Text;
            doc.PIPA_CONTRACT = radTextPIPAContract.Text;
            doc.PIPA_PURPOSE_PI = GetChkPIPAPI();
            doc.PIPA_PURPOSE_PI_OTHER = radTextPIPAPI6.Text;
            doc.PIPA_TARGET = GetChkPIPATarget();
            doc.PIPA_COLLECTION = GetChkPIPACollection();
            doc.PIPA_ARCHIVING = GetChkPIPAArchiving();
            doc.PIPA_ARCHIVING_OTHER = radTextPIPAArchiving4.Text;
            doc.PIPA_PERMISSION = radTextPIPAPermission.Text;
            doc.PIPA_VOLUMN = radTextPIPAVolume.Text;
            doc.PIPA_RETENTION = radTextPIPARetention.Text;
            doc.PIPA_3RDPARTY = radTextPIPA3RDPARTY.Text;
            doc.PIPA_OVERSEA = radTextPIPAOversea.Text;


        }
        


            using (Bayer.eWF.BSL.Approval.Mgr.ContractManagementMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.ContractManagementMgr())
        {
            processID = mgr.MergeContractManagement(doc);
        }

        return processID;

    }

    private string GetChkPIPAPI()
    {
        string type = string.Empty;
        if (radChkPIPAPI1.Checked) type = type + radChkPIPAPI1.Value + "|";
        if (radChkPIPAPI2.Checked) type = type + radChkPIPAPI2.Value + "|";
        if (radChkPIPAPI3.Checked) type = type + radChkPIPAPI3.Value + "|";
        if (radChkPIPAPI4.Checked) type = type + radChkPIPAPI4.Value + "|"; 
        if (radChkPIPAPI5.Checked) type = type + radChkPIPAPI5.Value + "|";

        return type;
    }
    private string GetChkPIPATarget()
    {
        string type = string.Empty;
        if (radChkPIPATarget1.Checked) type = type + radChkPIPATarget1.Value + "|";
        if (radChkPIPATarget2.Checked) type = type + radChkPIPATarget2.Value + "|";
        if (radChkPIPATarget3.Checked) type = type + radChkPIPATarget3.Value + "|";
        if (radChkPIPATarget4.Checked) type = type + radChkPIPATarget4.Value + "|";
        if (radChkPIPATarget5.Checked) type = type + radChkPIPATarget5.Value + "|";

        return type;
    }
    private string GetChkPIPACollection()
    {
        string type = string.Empty;
        if (radChkPIPACollection1.Checked) type = type + radChkPIPACollection1.Value + "|";
        if (radChkPIPACollection2.Checked) type = type + radChkPIPACollection2.Value + "|";
        if (radChkPIPACollection3.Checked) type = type + radChkPIPACollection3.Value + "|";
        if (radChkPIPACollection4.Checked) type = type + radChkPIPACollection4.Value + "|";
        

        return type;
    }
    private string GetChkPIPAArchiving()
    {
        string type = string.Empty;
        if (radCheckPIPAArchiving1.Checked) type = type + radCheckPIPAArchiving1.Value + "|";
        if (radCheckPIPAArchiving2.Checked) type = type + radCheckPIPAArchiving2.Value + "|";
        if (radCheckPIPAArchiving3.Checked) type = type + radCheckPIPAArchiving3.Value + "|";
        


        return type;
    }
}