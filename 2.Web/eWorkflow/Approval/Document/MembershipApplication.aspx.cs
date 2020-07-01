using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Mgr;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Web.UI.HtmlControls;

public partial class Approval_Document_MembershipApplication : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        if (!webMaster.DocumentNo.Equals(string.Empty))
        {
            HtmlInputHidden requestid = this.webMaster.FindControl("hddRequester") as HtmlInputHidden;
            this.hddRequestId.Value = requestid.Value;
            if(this.hddRequestId.Value == Sessions.UserID)
                webMaster.SetEnableControls(divCancel, true);
        }

        base.OnPreRender(e);
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
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddReuse.Value = Request["reuse"].NullObjectToEmptyEx();
        hddDocumentID.Value = "D0016";
        //hddProcessID.Value = "P000001500";

        InitControls();
    }

    private void InitControls()
    {
        //기등록 문서 조회
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (MembershipApplicationMgr mgr = new MembershipApplicationMgr())
            {
                DTO_DOC_MEMBERSHIP_APPLICATION doc = mgr.SelectMembershipApplication(this.hddProcessID.Value);

                if (radBtnWhitelisted.Value == doc.TA_CATEGORY) { radBtnWhitelisted.Checked = true;}
                else if (radBtnNon_Whitelisted.Value == doc.TA_CATEGORY) { radBtnNon_Whitelisted.Checked = true; }

                this.radTxtEngName.Text = doc.ENG_NAME;
                this.radTxtKorName.Text = doc.KOR_NAME;
                this.radTxtObjective.Text = doc.OBJECTIVE;
                this.radTxtPurpose.Text = doc.PURPOSE;
                this.radTxtPresident.Text = doc.PRESIDENT_SECRETARY;
                this.radNumFee.Value = (double?)doc.MEMBERSHIP_FEE;
                this.radTxtAddress.Text = doc.ADDRESS;
                this.radTxtPhone.Text = doc.PHONE_NO;
                this.radTxtFax.Text = doc.FAX;
                this.radTxtHomepage.Text = doc.HOMEPAGE;

                if (!doc.DOC_NUM.Equals(string.Empty))
                {
                    divCancel.Attributes.CssStyle.Add("display", "block");
                }

                //this.radDdlBusiness.SelectedValue = doc.RELEVANT_BUSINESS;
                //this.radTxtJobFunction.Text = doc.JOB_FUNCTION;
                //this.radTxtSubCommitte.Text = doc.SUB_COMMITTE;
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


    #region [ 문서상단 버튼 ]

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {

            if (radBtnWhitelisted.Checked == false && radBtnNon_Whitelisted.Checked == false)
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "TA Category";

            if (this.radTxtEngName.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "English Name";

            if (this.radTxtKorName.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Korean Name";

            if(this.radTxtObjective.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Objective of Association";

            if(this.radTxtPurpose.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose of obtaining membership";

            if (this.radTxtPresident.Text.IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "" : "," + "President/Secretary";

            if (this.radNumFee.Value == null || this.radNumFee.Value == 0)
                message += message.IsNullOrEmptyEx() ? "" : "," + "Membership fee";

            if (this.radTxtAddress.Text.IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "" : "," + "Address";

            if (this.radTxtPhone.Text.IsNullOrEmptyEx())
                message += message.IsNullOrEmptyEx() ? "" : "," + "Phone No.";

        }
        else if (status == ApprovalUtil.ApprovalStatus.Saved)
        {
            if (this.radTxtEngName.Text.IsNullOrEmptyEx())
                message += "English Name";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    }

    

    private string SaveDocument(ApprovalUtil.ApprovalStatus status)
    {
        DTO_DOC_MEMBERSHIP_APPLICATION doc = new DTO_DOC_MEMBERSHIP_APPLICATION();
        doc.PROCESS_ID = this.hddProcessID.Value;

        if (this.radBtnNon_Whitelisted.Checked)
            doc.TA_CATEGORY = radBtnNon_Whitelisted.Value;
        else if (this.radBtnWhitelisted.Checked)
            doc.TA_CATEGORY = radBtnWhitelisted.Value;

        doc.SUBJECT = " [ "+ doc.TA_CATEGORY + " ] " + this.radTxtEngName.Text;

        webMaster.Subject = doc.SUBJECT;

        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;



        doc.ENG_NAME = this.radTxtEngName.Text;
        doc.KOR_NAME = this.radTxtKorName.Text;

        doc.OBJECTIVE = this.radTxtObjective.Text;
        doc.PURPOSE = this.radTxtPurpose.Text;
        doc.PRESIDENT_SECRETARY = this.radTxtPresident.Text;
        doc.MEMBERSHIP_FEE = (decimal?)this.radNumFee.Value;
        doc.ADDRESS = this.radTxtAddress.Text;
        doc.PHONE_NO = this.radTxtPhone.Text;
        doc.FAX = this.radTxtFax.Text;
        doc.HOMEPAGE = this.radTxtHomepage.Text;

        doc.RELEVANT_BUSINESS = "";
        doc.JOB_FUNCTION = "";
        doc.SUB_COMMITTE = "";
        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        using (MembershipApplicationMgr mgr = new MembershipApplicationMgr())
        {
            return mgr.MergeMembershipApplication(doc);
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
    protected void RadbtnCancel_Click(object sender, EventArgs e)
    {        
        using(Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new CommonMgr())
        {
            DTO_DOC_LOG log = new DTO_DOC_LOG();
            log.PROCESS_ID = this.hddProcessID.Value;
            log.REGISTER_ID = Sessions.UserID;
            log.LOG_TYPE = "inputcomment";
            log.COMMENT = "CANCELED";
            log.CREATE_DATE = DateTime.Now;

            mgr.InsertDocLog(log);
            string radalertscript = "<script language='javascript'>function f(){radalert('Canceled!', 330, 180, 'Message', function(){ window.close();}); Sys.Application.remove_load(f);}; Sys.Application.add_load(f);</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", radalertscript);  

        }
    }
}