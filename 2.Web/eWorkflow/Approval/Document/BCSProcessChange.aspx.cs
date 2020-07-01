using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
using Telerik.Web.UI;
using Bayer.eWF.BSL.Approval.Mgr;

public partial class Approval_Document_BCSProcessChange : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
                this.hddCompanyCode.Value = Sessions.CompanyCode; //회사코드 설정
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
        hddDocumentID.Value = "D0042";
        //hddProcessID.Value = "P000000356";

        //hddProcessStatus.Value = ApprovalUtil.ApprovalStatus.Temp.ToString();

        InitControls();
    }

    private void InitControls()
    {
        if (this.hddProcessID.Value.IsNotNullOrEmptyEx())
        {
            using (BCSProcessChangeMgr mgr = new BCSProcessChangeMgr())
            {
                DTO_DOC_BCS_PROCESS_CHANGE doc = mgr.SelectBCSProcessChange(this.hddProcessID.Value);
                if (doc == null) return;
                this.hddProcessStatus.Value = doc.PROCESS_STATUS;

                this.radTextTitle.Text = doc.TITLE;

                string durationType = doc.DURATION_TYPE;
                string purposeType  = doc.PURPOSE_TYPE;
                
                string[] Catetory = new string[] { };
                if (doc.CATEGORY.IsNotNullOrEmptyEx())
                {
                    Catetory = doc.CATEGORY.Split(new string[] { "/" }, StringSplitOptions.None);
                }

                for (int i = 0; i < Catetory.Length; i++)
                {
                    if (Catetory[i].ToString().Equals(this.radChkCategory1.Value))
                        this.radChkCategory1.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory2.Value))
                        this.radChkCategory2.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory3.Value))
                        this.radChkCategory3.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory4.Value))
                        this.radChkCategory4.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory5.Value))
                        this.radChkCategory5.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory6.Value))
                        this.radChkCategory6.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory7.Value))
                        this.radChkCategory7.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory8.Value))
                        this.radChkCategory8.Checked = true;
                    if (Catetory[i].ToString().Equals(this.radChkCategory9.Value))
                        this.radChkCategory9.Checked = true;

                }

                //Purpose_Type
                foreach (Control control in this.divPurposeType.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(purposeType))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }

                //Duration_Type
                foreach (Control control in this.divDuration.Controls)
                {
                    if (control is RadButton)
                    {
                        if ((control as RadButton).Value.Equals(doc.DURATION_TYPE))
                        {
                            (control as RadButton).Checked = true; break;
                        }
                    }
                }

                if (durationType == "Temporary")
                {
                    this.divFromTo.Attributes.Add("style", "display: inline; visibility: visible");

                    if (doc.FROM_DATE.IsNotNullOrEmptyEx())
                    {
                        this.RadDateFrom.SelectedDate = doc.FROM_DATE;
                    }

                    if (doc.TO_DATE.IsNotNullOrEmptyEx())
                    {
                        this.RadDateTo.SelectedDate = doc.TO_DATE;
                    }
                       
                }
                else
                {
                    this.divFromTo.Attributes.Add("style", "display: none; visibility: visible");
                }

                this.radTextBrief_Description.Text   = doc.BRIEF_DESCRIPTION;
                this.radTextEffect_Before_After.Text = doc.EFFECT_BEFORE_AFTER;
                this.radTextAffectedProduct.Text     = doc.AFFECTED_PRODUCT;


                if (doc.PROPOSED_DUE_DATE.IsNotNullOrEmptyEx())
                {
                    this.RadDateProposedDue.SelectedDate = doc.PROPOSED_DUE_DATE;
                }

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


    /// <summary>
    /// Category 반환
    /// </summary>
    /// <returns></returns>
    private string GetChkCategory()
    {
        string Category = string.Empty;
        if (radChkCategory1.Checked) Category = Category + radChkCategory1.Value + "/";
        if (radChkCategory2.Checked) Category = Category + radChkCategory2.Value + "/";
        if (radChkCategory3.Checked) Category = Category + radChkCategory3.Value + "/";
        if (radChkCategory4.Checked) Category = Category + radChkCategory4.Value + "/";
        if (radChkCategory5.Checked) Category = Category + radChkCategory5.Value + "/";
        if (radChkCategory6.Checked) Category = Category + radChkCategory6.Value + "/";
        if (radChkCategory7.Checked) Category = Category + radChkCategory7.Value + "/";
        if (radChkCategory8.Checked) Category = Category + radChkCategory8.Value + "/";
        if (radChkCategory9.Checked) Category = Category + radChkCategory9.Value + "/";
        return Category;
    }

    private string GetSelectedPurposeType()
    {
        string PurposeType = string.Empty;
        foreach (Control control in divPurposeType.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    PurposeType = (control as RadButton).Value;
                    break;
                }
            }
        }
        return PurposeType;
    }

    private string GetSelectedDuration()
    {
        string Duration = string.Empty;
        foreach (Control control in divDuration.Controls)
        {
            if (control is RadButton)
            {
                if ((control as RadButton).Checked)
                {
                    Duration = (control as RadButton).Value;
                    break;
                }
            }
        }
        return Duration;
    }

    private bool ValidationCheck(ApprovalUtil.ApprovalStatus status)
    {
        string message = string.Empty;

        if (status == ApprovalUtil.ApprovalStatus.Request)
        {

            //Title
            if (radTextTitle.Text.IsNullOrEmptyEx())
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Title";
            
            //Category
            if (GetChkCategory().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Category";
            }

            //PurpsoeType
            if (GetSelectedPurposeType().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Purpose";
            }

            //DurationType
            if (GetSelectedDuration().IsNullOrEmptyEx())
            {
                message += (message.IsNullOrEmptyEx() ? "" : ",") + "Duration";
            }
            
            //Duration Form To Date
            if (GetSelectedDuration() == "Temporary")
            {
                //Duration From Date
                if (!this.RadDateFrom.SelectedDate.HasValue)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "From Date";

                //Duration To Date
                if (!this.RadDateTo.SelectedDate.HasValue)
                    message += (message.IsNullOrEmptyEx() ? "" : ",") + "To Date";

                    this.divFromTo.Attributes.Add("style", "display: inline; visibility: visible");

            }
            else {
                this.divFromTo.Attributes.Add("style", "display: none; visibility: visible");
            }


        }
        else if (status == ApprovalUtil.ApprovalStatus.Saved)
        {
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
        DTO_DOC_BCS_PROCESS_CHANGE doc = new DTO_DOC_BCS_PROCESS_CHANGE();
        doc.PROCESS_ID = this.hddProcessID.Value;
        string subject = string.Empty;
        
        string Title = this.radTextTitle.Text;
        string Category = GetChkCategory();
        string PurposeType = GetSelectedPurposeType();
        string Duration = GetSelectedDuration();

        doc.SUBJECT = Title + "/" + PurposeType + "/" + Duration;
        webMaster.Subject = doc.SUBJECT;
        doc.DOC_NUM = string.Empty;
        doc.PROCESS_STATUS = status.ToString();
        doc.REQUESTER_ID = Sessions.UserID;
        doc.REQUEST_DATE = DateTime.Now;
        doc.COMPANY_CODE = Sessions.CompanyCode;
        doc.ORGANIZATION_NAME = Sessions.OrgName;
        doc.LIFE_CYCLE = webMaster.LifeCycle;

        doc.TITLE = Title;
        doc.CATEGORY = Category;
        doc.PURPOSE_TYPE = PurposeType;
        doc.DURATION_TYPE = Duration;

        if (Duration == "Temporary")
        {

            if (this.RadDateFrom.SelectedDate.HasValue)
            {
                //doc.FROM_DATE = this.RadDateFrom.SelectedDate.Value;
                doc.FROM_DATE = Convert.ToDateTime(this.RadDateFrom.SelectedDate.Value.ToShortDateString());
            }

            if (this.RadDateTo.SelectedDate.HasValue)
            {
                //doc.TO_DATE = this.RadDateTo.SelectedDate.Value;
                doc.TO_DATE = Convert.ToDateTime(this.RadDateTo.SelectedDate.Value.ToShortDateString());
            }

        }

        doc.BRIEF_DESCRIPTION = this.radTextBrief_Description.Text;
        doc.EFFECT_BEFORE_AFTER = this.radTextEffect_Before_After.Text;
        doc.AFFECTED_PRODUCT = this.radTextAffectedProduct.Text;


        if (this.RadDateProposedDue.SelectedDate.HasValue)
        {
            //doc.PROPOSED_DUE_DATE = this.RadDateProposedDue.SelectedDate.Value;
            doc.PROPOSED_DUE_DATE = Convert.ToDateTime(this.RadDateProposedDue.SelectedDate.Value.ToShortDateString());
        }

        doc.IS_DISUSED = "N";
        doc.CREATOR_ID = Sessions.UserID;

        using (BCSProcessChangeMgr mgr = new BCSProcessChangeMgr())
        {
            return mgr.MergeBCSProcessChange(doc);
        }

    }
    protected override void DoSave()
    {
        try
        {
            if (ValidationCheck(ApprovalUtil.ApprovalStatus.Saved))
            {
                hddProcessID.Value = SaveDocument(ApprovalUtil.ApprovalStatus.Saved);
                webMaster.ProcessID = this.hddProcessID.Value;
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