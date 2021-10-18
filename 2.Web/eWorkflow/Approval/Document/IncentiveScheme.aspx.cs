using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
 
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;

public partial class Approval_Document_IncentiveScheme : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    protected override void OnPreRender(EventArgs e)
    {
        if (webMaster.DocumentNo != null)
        {
            webMaster.SetEnableControls(payment, true);
        }
 
        base.OnPreRender(e);
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
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0013";
       

 
        rdoDiv01.Text = "SM";
        rdoDiv01.Value = "SM";
        rdoDiv02.Text = "HH";
        rdoDiv02.Value = "HH";
        rdoDiv03.Text = "WH";
        rdoDiv03.Value = "WH";
        rdoDiv04.Text = "R";
        rdoDiv04.Value = "R";
        rdoDiv05.Text = "AH";
        rdoDiv05.Value = "AH";
        rdoDiv06.Text = "CH";
        rdoDiv06.Value = "CH";
        rdoDiv07.Text = "DC";
        rdoDiv07.Value = "DC";
        rdoDiv08.Text = "CC";
        rdoDiv08.Value = "CC";
        

        InitControls();
    }

    private void InitControls()
    {
        DTO_DOC_INCENTIVE_SCHEME doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.IncentiveSchemeMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.IncentiveSchemeMgr())
            {
                doc = mgr.SelectIncentiveScheme(hddProcessID.Value);
            }

            if (doc != null)
            {
                txtSubject.Text = doc.TITLE;
                txtDescription.Text = doc.DESCRIPTION;
                if (rdoDiv01.Value == doc.BU) rdoDiv01.Checked = true;
                else if (rdoDiv02.Value == doc.BU) rdoDiv02.Checked = true;
                else if (rdoDiv03.Value == doc.BU) rdoDiv03.Checked = true;
                else if (rdoDiv04.Value == doc.BU) rdoDiv04.Checked = true;
                else if (rdoDiv05.Value == doc.BU) rdoDiv05.Checked = true;
                else if (rdoDiv06.Value == doc.BU)    // CH
                {
                    rdoDiv06.Visible = true;
                    rdoDiv06.Checked = true;

                    rdoDiv08.Visible = false;
                    rdoDiv08.Checked  =false;
                }
                else if (rdoDiv07.Value == doc.BU)
                {
                    rdoDiv07.Visible = true;
                    rdoDiv07.Checked = true;
                }
                else if (rdoDiv08.Value == doc.BU)    // CC
                {
                    rdoDiv06.Visible = false;
                    rdoDiv06.Checked = false;

                    rdoDiv08.Visible = true;
                    rdoDiv08.Checked = true;
                }



                if (rdoPROGRAM01.Value == doc.PROGRAM) rdoPROGRAM01.Checked =true;
                else if (rdoPROGRAM02.Value == doc.PROGRAM) rdoPROGRAM02.Checked = true;
                else if (rdoPROGRAM03.Value == doc.PROGRAM) rdoPROGRAM03.Checked = true;
                else if (rdoPROGRAM04.Value == doc.PROGRAM)  // CAP
                {
                    rdoPROGRAM04.Checked = true;
                    rdoPROGRAM04.Visible = true;

                    rdoPROGRAM06.Checked = false;
                    rdoPROGRAM06.Visible = false;
                }
                else if (rdoPROGRAM05.Value == doc.PROGRAM)
                    rdoPROGRAM05.Checked = true;
                else if (rdoPROGRAM06.Value == doc.PROGRAM) // Valvet
                {
                    rdoPROGRAM04.Checked = false;
                    rdoPROGRAM04.Visible = false;

                    rdoPROGRAM06.Checked = true;
                    rdoPROGRAM06.Visible = true;
                }
                else if (rdoPROGRAM07.Value == doc.PROGRAM) rdoPROGRAM07.Checked = true;
                else if (rdoPROGRAM08.Value == doc.PROGRAM)  // FAP
                {
                    rdoPROGRAM08.Checked = true;
                    rdoPROGRAM08.Visible = true;

                    rdoPROGRAM06.Checked = false;
                    rdoPROGRAM06.Visible = false;
                }

                if (rdoSettlementType01.Value == doc.SETTLEMENT_TYPE) rdoSettlementType01.Checked = true;
                else if (rdoSettlementType02.Value == doc.SETTLEMENT_TYPE)
                {
                    rdoSettlementType02.Checked = true; 
                    rdoSettlementType02.Visible = true;
                }
                else if (rdoSettlementType03.Value == doc.SETTLEMENT_TYPE) rdoSettlementType03.Checked = true;
                else if (rdoSettlementType04.Value == doc.SETTLEMENT_TYPE) rdoSettlementType04.Checked = true;

                if (doc.DOC_NUM != null)
                {
                    payment.Attributes.CssStyle.Add("display", "block");
                    hddDocpaymnetID.Value = "D0014";
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
        catch(Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
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
            if (txtSubject.Text.Length <= 0)            
                message += "Subject";                
            
            if (!(rdoDiv01.Checked || rdoDiv02.Checked || rdoDiv03.Checked ||
                  rdoDiv04.Checked || rdoDiv05.Checked || rdoDiv06.Checked || rdoDiv07.Checked))            
                message += message.IsNullOrEmptyEx() ? "BU" : ", BU";

            if (!(rdoPROGRAM01.Checked || rdoPROGRAM02.Checked || rdoPROGRAM03.Checked ||
                  rdoPROGRAM04.Checked || rdoPROGRAM05.Checked || rdoPROGRAM06.Checked ||
                  rdoPROGRAM07.Checked || rdoPROGRAM08.Checked))
                message += message.IsNullOrEmptyEx() ? "PROGRAM" : ", PROGRAM";


            if (!(rdoSettlementType01.Checked || rdoSettlementType02.Checked || 
                 rdoSettlementType03.Checked || rdoSettlementType04.Checked))
                message += message.IsNullOrEmptyEx() ? "Settlement Type" : ", Settlement Type";
            
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
        
    }

    private string DocumentSave(string processStstus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_INCENTIVE_SCHEME doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_INCENTIVE_SCHEME();
        doc.PROCESS_STATUS = processStstus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.TITLE = txtSubject.Text;   
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.DESCRIPTION = txtDescription.Text;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";


        if (rdoDiv01.Checked)
            doc.BU = rdoDiv01.Value;
        else if (rdoDiv02.Checked)
            doc.BU = rdoDiv02.Value;
        else if (rdoDiv03.Checked)
            doc.BU = rdoDiv03.Value;
        else if (rdoDiv03.Checked)
            doc.BU = rdoDiv03.Value;
        else if (rdoDiv04.Checked)
            doc.BU = rdoDiv04.Value;
        else if (rdoDiv05.Checked)
            doc.BU = rdoDiv05.Value;
        else if (rdoDiv06.Checked)
            doc.BU = rdoDiv06.Value;
        else if (rdoDiv07.Checked)
            doc.BU = rdoDiv07.Value;
        else if (rdoDiv08.Checked)
            doc.BU = rdoDiv08.Value;


        if (rdoPROGRAM01.Checked)
            doc.PROGRAM = rdoPROGRAM01.Value;
        else if (rdoPROGRAM02.Checked)
            doc.PROGRAM = rdoPROGRAM02.Value;
        else if (rdoPROGRAM03.Checked)
            doc.PROGRAM = rdoPROGRAM03.Value;
        else if (rdoPROGRAM04.Checked)
            doc.PROGRAM = rdoPROGRAM04.Value;
        else if (rdoPROGRAM05.Checked)
            doc.PROGRAM = rdoPROGRAM05.Value;
        else if (rdoPROGRAM06.Checked)
            doc.PROGRAM = rdoPROGRAM06.Value;
        else if (rdoPROGRAM07.Checked)
            doc.PROGRAM = rdoPROGRAM07.Value;
        else if (rdoPROGRAM08.Checked)
            doc.PROGRAM = rdoPROGRAM08.Value;


        if (rdoSettlementType01.Checked)
            doc.SETTLEMENT_TYPE = rdoSettlementType01.Value;
        else if (rdoSettlementType02.Checked)
            doc.SETTLEMENT_TYPE = rdoSettlementType02.Value;
        else if (rdoSettlementType03.Checked)
            doc.SETTLEMENT_TYPE = rdoSettlementType03.Value;
        else if (rdoSettlementType04.Checked)
            doc.SETTLEMENT_TYPE = rdoSettlementType04.Value;

        doc.SUBJECT = doc.BU + "/" + doc.PROGRAM + "/" + doc.SETTLEMENT_TYPE + "/" + txtSubject.Text;
        webMaster.Subject = doc.SUBJECT; 

        using (Bayer.eWF.BSL.Approval.Mgr.IncentiveSchemeMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.IncentiveSchemeMgr())
        {
            processID = mgr.MergeIncentiveScheme(doc);
        } 
      

       return processID;

    }
    
}