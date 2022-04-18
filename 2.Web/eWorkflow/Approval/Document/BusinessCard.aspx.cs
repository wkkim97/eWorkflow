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
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_BusinessCard : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    private Control HolderDocumentBody;

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

    #region OnPreRender
    protected override void OnPreRender(EventArgs e)
    {
        radBtnDisplayNameCard.Visible = true;
        radBtnDisplayNameCard.ReadOnly = false;
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
        hddDocumentID.Value = "D0002";
        //hddProcessID.Value = "P000000384";




        InitControls();
    }
    #endregion

    #region InitControls
    private void InitControls()
    {
        this.radtxtEnOrg.ReadOnly = true;
        this.radtxtKoOrg.ReadOnly = true;

        radtxtEnName.Text = Sessions.UserName;
        radtxtTitle.Text = Sessions.Title;
        radtxtEmailAddress.Text = Sessions.MailAddress;
        //radlbLogo.Text = Sessions.LeadingSubGroup; //2015-12-16 (sub group logo 사용금지 guide 에 맞춰 삭제함)

        using (CodeMgr mgr = new CodeMgr())
        {
            //korea title
            List<DTO_CODE_SUB> titles = mgr.SelectCodeSubList("S011");
            this.radDropKorTitle.DataSource = titles;
            this.radDropKorTitle.DataBind();
            if (this.radDropKorTitle.Items.Count > 0) this.radDropKorTitle.SelectedIndex = 0;
        }

        using (Bayer.eWF.BSL.Common.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Common.Mgr.CommonMgr())
        {
            //Address&Zip
            List<DTO_BUSINESS_PLACE> places = mgr.SelectBusinessPlace(Sessions.CompanyCode);
            this.radDropAddZip.Items.Add(new DropDownListItem(""));

            this.radDropAddZip.DataSource = places;
            this.radDropAddZip.DataBind();
            if (this.radDropAddZip.Items.Count > 0) this.radDropAddZip.SelectedIndex = 0;
        }

        DTO_DOC_BUSINESS_CARD doc;
        if (!hddProcessID.Value.Equals(String.Empty))
        {
            using (Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr())
            {
                doc = mgr.SelectBusinessCard(hddProcessID.Value);
            }

            if (doc != null)
            {
                string telnum = doc.TEL_OFFICE;
                string mobilenum = doc.MOBILE;
                string faxnum = doc.FAX;
                char sp = ' ';
                string[] telstring = telnum.Split(sp);
                string[] mobilestring = mobilenum.Split(sp);
                string[] faxstring = faxnum.Split(sp);

                hddProcessStatus.Value = doc.PROCESS_STATUS;
                radtxtEnName.Text = doc.ENG_NAME;
                radtxtTitle.Text = doc.ENG_TITLE;
                radtxtEnOrg.Text = doc.ENG_DIVISION_NAME;
                radtxtEnDepartment.Text = doc.ENG_DEPARTMENT;
                radtxtKoName.Text = doc.KOR_NAME;
                RadDropDownColor.SelectedText = doc.COLOR_CODE;
                radtxtKOR_JOB_TITLE.Text = doc.KOR_JOB_TITLE;
                radDropKorTitle.SelectedValue = doc.KOR_TITLE;
                radtxtKoOrg.Text = doc.KOR_DIVISION_NAME;
                radtxtKoDepartment.Text = doc.KOR_DEPARTMENT;
                radDropTelCode.SelectedText = telstring[0];
                radTxtTelNum.Text = GetPhoneNumber(telstring);
                radDropMobileCode.SelectedText = mobilestring[0];
                radTxtMobileNum.Text = GetPhoneNumber(mobilestring);
                radDropFaxCode.SelectedText = faxstring[0];
                radTxtFaxNum.Text = GetPhoneNumber(faxstring);
                radtxtEmailAddress.Text = doc.E_MAIL;
                radDropAddZip.SelectedValue = doc.ADDRESS_CODE;
                radtxtQuantity.Text = doc.QUANTITY.ToString();
                //radlbLogo.Text = doc.LEADING_SUBGROUP; //2015-12-16 (sub group logo 사용금지 guide 에 맞추어서 삭제함.
                webMaster.DocumentNo = doc.DOC_NUM;

            }
        }
    }

    private string GetPhoneNumber(string[] array)
    {
        string rtnValue = string.Empty;
        for (int i = 1; i < array.Length;i++ )
        {
            rtnValue += array[i];
            if (i < array.Length - 1) rtnValue += " ";

        }

        return rtnValue;
    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        // 마스터 페이지 컨트롤에 셋팅

        this.InitMasterPageInfo(hddDocumentID.Value, hddProcessID.Value, this.hddReuse.Value);


        // 각 문서마다 데이터 로드 조회부분 구현 
        RadAjaxManager.GetCurrent(this).AjaxSettings.AddAjaxSetting(RadAjaxManager.GetCurrent(this), this.hddProcessID);
        RadAjaxManager.GetCurrent(this).AjaxRequest += Approval_Document_BusinessCard_AjaxRequest;
        RadAjaxManager.GetCurrent(this).ClientEvents.OnResponseEnd = "OpenDisplayNameCard";

    }

    void Approval_Document_BusinessCard_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.Equals("Display"))
        {
            string status = this.hddProcessStatus.Value;
            if (status.IsNullOrEmptyEx() || status.Equals(ApprovalUtil.ApprovalStatus.Saved.ToString()) || status.Equals(ApprovalUtil.ApprovalStatus.Temp.ToString()))
                DocumentSave(ApprovalUtil.ApprovalStatus.Saved.ToString());
        }
    }
    #endregion

    protected override void DoReUse()
    {
        try
        {
            //if (webMaster.DocumentNo != null)
            //{
            //    Control parent =  this.webMaster.FindControl("HolderDocumentBody") as ContentPlaceHolder;
            //    webMaster.SetEnableControls(parent, true);
            //}
            base.DoReUse();
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
            if (radtxtEnName.Text.Length <= 0 || radtxtKoName.Text.Length <= 0)
                message += "Name";
            if (this.radtxtTitle.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Title" : ", Title";

            if (radtxtEnOrg.Text.Length <= 0 || radtxtKoOrg.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Division" : ",Division";

            //<% --2022.04.15 comment out below line, INC15142408-- %> comment out, start
 
            //if (radTxtTelNum.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Tel.of office" : ",Tel.of office";

            //<% --2022.04.15 comment out below line, INC15142408-- %> comment out, end

            //if (radTxtMobileNum.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Mobile phone" : ",Mobile phone";

            //if (radTxtFaxNum.Text.Length <= 0)
            //    message += message.IsNullOrEmptyEx() ? "Fax number" : ",Fax number";

            if (radtxtEmailAddress.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "E-mail address" : ",E-mail address";

            if (radtxtQuantity.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Quantity" : ",Quantity";
        }

        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;

    }

    private void DocumentSave()
    {
        DocumentSave(string.Empty);
    }

    private string DocumentSave(string processStatus)
    {


        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BUSINESS_CARD doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_BUSINESS_CARD();
        doc.PROCESS_STATUS = processStatus;
        doc.PROCESS_ID = hddProcessID.Value;
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = Sessions.UserID;
        doc.SUBJECT = this.radtxtEnName.Text;
        webMaster.Subject = doc.SUBJECT;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.CREATE_DATE = DateTime.Now;
        doc.LIFE_CYCLE = webMaster.LifeCycle;
        doc.CREATOR_ID = Sessions.UserID;
        doc.IS_DISUSED = "N";

        doc.ENG_NAME = radtxtEnName.Text;
        doc.ENG_TITLE = radtxtTitle.Text;
        doc.ENG_DIVISION_CODE = hddEngDivisionCode.Value;
        doc.ENG_DIVISION_NAME = radtxtEnOrg.Text;

        if (radtxtEnDepartment.Text.Equals(null))
        {
            doc.ENG_DEPARTMENT = null;
        }
        else
        {
            doc.ENG_DEPARTMENT = radtxtEnDepartment.Text;
        }

        doc.KOR_NAME = radtxtKoName.Text;
        doc.COLOR_CODE = RadDropDownColor.SelectedText;

        if (radDropKorTitle.SelectedValue.Equals(null))
        {
            doc.KOR_TITLE_NAME = null;
        }
        else
        {
            doc.KOR_TITLE = radDropKorTitle.SelectedValue;
        }

        doc.KOR_TITLE_NAME = radDropKorTitle.SelectedText;
        doc.KOR_DIVISION_CODE = hddKorDivisionCode.Value;
        doc.KOR_DIVISION_NAME = radtxtKoOrg.Text;

        if (radtxtKoDepartment.Text.Equals(null))
        {
            doc.KOR_DEPARTMENT = null;
        }
        else
        {
            doc.KOR_DEPARTMENT = radtxtKoDepartment.Text;
        }

        doc.TEL_OFFICE = radDropTelCode.SelectedText + " " + radTxtTelNum.Text;
        doc.MOBILE = radDropMobileCode.SelectedText + " " + radTxtMobileNum.Text;
        doc.FAX = radDropFaxCode.SelectedText + " " + radTxtFaxNum.Text;
        doc.E_MAIL = radtxtEmailAddress.Text;
        doc.ADDRESS_CODE = radDropAddZip.SelectedValue;
        doc.ADDRESS = radDropAddZip.SelectedText;
        doc.QUANTITY = Convert.ToInt32(radtxtQuantity.Text);

        if (radtxtKOR_JOB_TITLE.Text.Equals(null))
        {
            doc.KOR_JOB_TITLE = null;
        }
        else
        {
            doc.KOR_JOB_TITLE = radtxtKOR_JOB_TITLE.Text;
        } 
        doc.KOR_JOB_TITLE = radtxtKOR_JOB_TITLE.Text;

        using (Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr())
        {
            processID = mgr.MergeBusinessCard(doc);
        }
        this.hddProcessID.Value = processID;
        return processID;

    }

    protected void radBtnDisplayNameCard_Click(object sender, EventArgs e)
    {
        if (!ClientScript.IsStartupScriptRegistered("OpenDisplayNameCard"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenDisplayNameCard", "OpenDisplayNameCard();", true);
    }

    //public Control HolderDocumentBody { get; set; }
}