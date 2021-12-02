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

public partial class Approval_Document_ResaleDataCollection : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
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
        hddDocumentID.Value = "D0018";        

        InitControls();
    } 
    #endregion

    #region InitControls
    private void InitControls()
    {
        DTO_DOC_RESALE_DATA_COLLECTION doc;

        if (!hddProcessID.Value.Equals(string.Empty))
        {
            using(ResaleDataCollectionMgr mgr = new ResaleDataCollectionMgr())
            {
                doc = mgr.SelectResaleDataCollection(hddProcessID.Value);
            }
            if (doc != null)
            {
                this.RadtxtDataSource.Text = doc.DATA_SOURCE;
                this.RadtxtResaleData.Text = doc.RESALE_DATA_DETAILS;               
                this.RadtxtPurpose.Text = doc.COLLECTION_PURPOSE;
                this.RadtxtArchiving.Text = doc.DATA_ARCHIVING;
                //LPC-forMonsanto User
                this.RadtxtEmployee.Text = doc.Employee;
                if (RadrdoPriceValue_Y.Value == doc.PRICEVALUE) { RadrdoPriceValue_Y.Checked = true; }
                else if (RadrdoPriceValue_N.Value == doc.PRICEVALUE) RadrdoPriceValue_N.Checked = true;
                //LPC-forMonsanto User
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
        Bayer.eWF.BSL.Approval.Dto.DTO_DOC_RESALE_DATA_COLLECTION doc = null;
        string processID = string.Empty;
        doc = new Bayer.eWF.BSL.Approval.Dto.DTO_DOC_RESALE_DATA_COLLECTION();
        doc.PROCESS_ID = hddProcessID.Value;
        doc.PROCESS_STATUS = ApprovalStatus; //
        doc.REQUEST_DATE = DateTime.Now;
        doc.REQUESTER_ID = this.Sessions.UserID.ToString();
        doc.SUBJECT = this.RadtxtDataSource.Text;   // category:Type
        webMaster.Subject = doc.SUBJECT;
        doc.COMPANY_CODE = this.Sessions.CompanyCode.ToString();
        doc.ORGANIZATION_NAME = this.Sessions.OrgName.ToString();
        doc.LIFE_CYCLE = webMaster.LifeCycle.ToString();
        doc.IS_DISUSED = "N"; //보존연한 
        doc.CREATOR_ID = this.Sessions.UserID.ToString();
        doc.CREATE_DATE = DateTime.Now;

        doc.DATA_SOURCE = this.RadtxtDataSource.Text;
        doc.RESALE_DATA_DETAILS = this.RadtxtResaleData.Text;
        doc.COLLECTION_PURPOSE = this.RadtxtPurpose.Text;
        doc.DATA_ARCHIVING = this.RadtxtArchiving.Text;
        //LPC-forMonsanto User
        doc.Employee = this.RadtxtEmployee.Text;
        if (RadrdoPriceValue_Y.Checked)
            doc.PRICEVALUE = RadrdoPriceValue_Y.Value;
        else if (RadrdoPriceValue_N.Checked)
            doc.PRICEVALUE = RadrdoPriceValue_N.Value;
        //LPC-forMonsanto User
        using (ResaleDataCollectionMgr mgr = new ResaleDataCollectionMgr())
        {
            processID = mgr.MergeResaleDataCollection(doc);
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
            if (RadtxtDataSource.Text.Length <= 0)
                message += "Data Source";

            if (RadtxtResaleData.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Details of Resale Data" : ",Details of Resale Data";

            if (RadtxtPurpose.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Purpose of Collection" : ",Purpose of Collection";

            if (RadtxtArchiving.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Archiving of Data" : ",Archiving of Data";

            if (RadtxtEmployee.Text.Length <= 0)
                message += message.IsNullOrEmptyEx() ? "Employee" : ",Employee";
            if (RadrdoPriceValue_Y.Checked == false && RadrdoPriceValue_N.Checked == false )
                message += message.IsNullOrEmptyEx() ? "Price and Value" : ",Price and Valuea";
            
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