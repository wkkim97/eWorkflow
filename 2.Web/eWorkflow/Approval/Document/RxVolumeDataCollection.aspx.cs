using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;

public partial class Approval_Document_RxVolumeDataCollection : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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

    #region Page_Load
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
        hddDocumentID.Value = "D0054";        

        InitControls();
    } 
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_RX_VOLUME_DATA_COLLECTION doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using(RxVolumeDataCollectionMgr mgr = new RxVolumeDataCollectionMgr())
            {
                doc = mgr.SelectRxVolumeDataCollection(hddProcessID.Value);
            }
            if (doc != null)
            {
                this.RadtxtCollectedProduct.Text = doc.COLLECTED_PRODUCT;
                this.RadtxtDataSource.Text = doc.DATA_SOURCE;
                this.RadtxtDetailsOfRxVolumeData.Text = doc.DETAILS_OF_RX_VOLUME_DATA;

                if (doc.DETAILS_OF_RX_VOLUME_DATA_YN == "Y")
                {
                    this.radChkDetailsOfRxVolumeData_Yes.Checked = true;
                    this.radChkDetailsOfRxVolumeData_No.Checked = false;
                }
                else if (doc.DETAILS_OF_RX_VOLUME_DATA_YN == "N")
                {
                    this.radChkDetailsOfRxVolumeData_Yes.Checked = false;
                    this.radChkDetailsOfRxVolumeData_No.Checked = true;
                }

                if (doc.INCLUDING_COMPETITIVE_DATA_AND_DETAILS_YN == "Y")
                {
                    this.radChkIncludingCompetitiveDataAndDetails_Yes.Checked = true;
                    this.radChkIncludingCompetitiveDataAndDetails_No.Checked = false;
                }
                else if (doc.INCLUDING_COMPETITIVE_DATA_AND_DETAILS_YN == "N")
                {
                    this.radChkIncludingCompetitiveDataAndDetails_Yes.Checked = false;
                    this.radChkIncludingCompetitiveDataAndDetails_No.Checked = true;
                }

                this.RadtxtIncludingCompetitiveDataAndDetails.Text = doc.INCLUDING_COMPETITIVE_DATA_AND_DETAILS;
                this.RadtxtPurpose.Text = doc.PURPOSE_OF_COLLECTION;
                this.RadtxtCollectionMethod.Text = doc.COLLECTION_METHOD;

                this.radDateValidityDate.SelectedDate = doc.VALIDITY_DATE.Date;

                this.RadtxtArchiving.Text = doc.ARCHIVING_OF_DATA;
                this.RadtxtAccessRight.Text = doc.ACCESS_RIGHT;
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

    #region DoRequest
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
    #endregion

    #region DoSave
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
    #endregion

    #region DocumentSave
    private string DocumentSave(string ApprovalStatus)
    {
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_RX_VOLUME_DATA_COLLECTION doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_RX_VOLUME_DATA_COLLECTION();
        doc.PROCESS_ID = hddProcessID.Value;
        doc.PROCESS_STATUS = ApprovalStatus; //
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        doc.SUBJECT = this.RadtxtCollectedProduct.Text + " / " + this.RadtxtDataSource.Text;   
        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.IS_DISUSED = "N"; //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;


        doc.COLLECTED_PRODUCT = this.RadtxtCollectedProduct.Text;
        doc.DATA_SOURCE = this.RadtxtDataSource.Text;

        doc.DETAILS_OF_RX_VOLUME_DATA = this.RadtxtDetailsOfRxVolumeData.Text;
        if (this.radChkDetailsOfRxVolumeData_Yes.Checked)
        {
            doc.DETAILS_OF_RX_VOLUME_DATA_YN = this.radChkDetailsOfRxVolumeData_Yes.Text;
        }
        else if (this.radChkIncludingCompetitiveDataAndDetails_Yes.Checked)
        {
            doc.DETAILS_OF_RX_VOLUME_DATA_YN = this.radChkDetailsOfRxVolumeData_No.Value;
        }

        doc.INCLUDING_COMPETITIVE_DATA_AND_DETAILS = this.RadtxtIncludingCompetitiveDataAndDetails.Text;
        if (this.radChkIncludingCompetitiveDataAndDetails_Yes.Checked)
        {
            doc.INCLUDING_COMPETITIVE_DATA_AND_DETAILS_YN = this.radChkIncludingCompetitiveDataAndDetails_Yes.Text;
        }
        else if (this.radChkIncludingCompetitiveDataAndDetails_No.Checked)
        {
            doc.INCLUDING_COMPETITIVE_DATA_AND_DETAILS_YN = this.radChkIncludingCompetitiveDataAndDetails_Yes.Text;
        }


        doc.PURPOSE_OF_COLLECTION = this.RadtxtPurpose.Text;
        doc.COLLECTION_METHOD = this.RadtxtCollectionMethod.Text;

        if (this.radDateValidityDate.SelectedDate.HasValue)
        {
            DateTime dt = this.radDateValidityDate.SelectedDate.Value.Date;

            if (this.radDateValidityDate.SelectedDate.HasValue)
                doc.VALIDITY_DATE = this.radDateValidityDate.SelectedDate.Value;
            else
                doc.VALIDITY_DATE = dt;
        }

        doc.ARCHIVING_OF_DATA = this.RadtxtArchiving.Text;
        doc.ACCESS_RIGHT = this.RadtxtAccessRight.Text;


        using (RxVolumeDataCollectionMgr mgr = new RxVolumeDataCollectionMgr())
        {
            processID = mgr.MergeRxVolumeDataCollection (doc);
        }
        return processID;
    } 
    #endregion

    #region ValidationCheck
    private bool ValidationCheck(ApprovalUtil.ApprovalStatus approvalStatus)
    {
        string message = string.Empty;

        if (approvalStatus == ApprovalUtil.ApprovalStatus.Request)
        {
            if (RadtxtCollectedProduct.Text.Length <= 0)
                message += "Collected Product";

            if (RadtxtDataSource.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Data Source" : ", Data Source";

            if (RadtxtDetailsOfRxVolumeData.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Details of Rx Volume Data" : ", Details of Rx Volume Data";

            if (this.radChkIncludingCompetitiveDataAndDetails_Yes.Checked)
            {
                if (RadtxtIncludingCompetitiveDataAndDetails.Text.Length <= 0)
                    message += message.IsNullOrEmptyEx() ? "Including Competitive Data And Details" : ", Including Competitive Data And Details";
            }

            if (RadtxtPurpose.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Purpose of Collection" : ", Purpose of Collection";

            if (RadtxtCollectionMethod.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Collection Method" : ", Collection Method";

            if (!this.radDateValidityDate.SelectedDate.HasValue)
                message += message.IsNullOrEmptyEx() ? "Validity Date" : ", Validity Date ";

            if (RadtxtArchiving.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Archiving of Data" : ", Archiving of Data";

            if (RadtxtAccessRight.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Access Right" : ", Access Right";
        }
        if (message.Length > 0)
        {
            this.informationMessage = this.Msgs("0002")[3].ToString() + Environment.NewLine + message;
            return false;
        }
        else return true;
    } 
    #endregion
}