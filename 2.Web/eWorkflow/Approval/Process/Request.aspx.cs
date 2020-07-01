using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Common.Dto;
using System.Text;
using System.Reflection;

/// <summary>
/// 결재 요청 처리 화면
/// </summary>
public partial class Approval_Process_Request : DNSoft.eWF.FrameWork.Web.PageBase
{
    #region DTO_APPROVAL LIST
    private List<DTO_APPROVAL> list;
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PageLoadInfo();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }

    }
    #endregion

    #region PageLoadInfo
    private void PageLoadInfo()
    {
        
        hddDocumentID.Value = Request["documentid"].NullObjectToEmptyEx();
        hddProcessID.Value = Request["processid"].NullObjectToEmptyEx();
        hddDocumentName.Value = Request["documentname"].NullObjectToEmptyEx();
        hddSubject.Value = Request["subject"].NullObjectToEmptyEx();
        hddFolderPath.Value = Request["folderpath"].NullObjectToEmptyEx();
        //hddXmlFiles.Value = Request["attachfiles"].NullObjectToEmptyEx();

        hddRejectedProcessId.Value = Request["rejectedid"].NullObjectToEmptyEx();
        ContentsBind();
    }
    #endregion

    #region ContentsBind
    private void ContentsBind()
    {
        List<DTO_APPROVAL> appLine;
        List<DTO_APPROVAL> recipient;
        List<DTO_APPROVAL> reviewer;


        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            list = mgr.GetApprovalLine(hddDocumentID.Value, Sessions.UserID, hddProcessID.Value);
            appLine = new List<DTO_APPROVAL>();
            recipient = new List<DTO_APPROVAL>();
            reviewer = new List<DTO_APPROVAL>();

            foreach (DTO_APPROVAL item in list)
            {
                if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.DRAFTER) || item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.APPROVER))
                {
                    appLine.Add(item);
                }
                else if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.RECIPIENT))
                {
                    recipient.Add(item);
                }
                else if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.REVIEWER))
                {
                    reviewer.Add(item);
                }
            }
        }

        grdApprovalLIne.DataSource = appLine;
        grdApprovalLIne.DataBind();

        viewRecipient.DataSource = recipient;
        viewRecipient.DataBind();

        viewReviewer.DataSource = reviewer;
        viewReviewer.DataBind();

    }

    #endregion

    #region btnRequest Click Event
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        string script = string.Empty;
        try
        {
            CreateProcessApproval();
            RegisteAttachFiles();
            script = AfterTreatmentScript();
            ClientWindowClose("true", script);
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
        finally
        {
            if (script != null)
                script = null;
        }
    }
    #endregion

    #region AfterTreatmentScript 후처리 호출

    /// <summary>
    /// 후처리 호출
    /// </summary>
    /// <returns></returns>
    private string AfterTreatmentScript()
    {
        StringBuilder script = new StringBuilder(128);
        string mailSendType = string.Empty;
        try
        {
            #region SendMail Process

            mailSendType = ApprovalUtil.SendMailType.CurrentApprover.ToString();

            if (mailSendType.Length > 0 && hddProcessID.Value.Length > 0)
            {
                script.AppendFormat("fn_sendMail('{0}','{1}', '{2}');", hddProcessID.Value, mailSendType, Sessions.MailAddress);
            }
            #endregion

            return script.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (script != null)
            {
                script.Clear();
                script = null;
            }
        }
    }
    #endregion

    #region RegisteAttachFiles
    private void RegisteAttachFiles()
    {
        string files =  HttpUtility.UrlDecode(hddXmlFiles.Value);
        using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
        {
            List<DTO_ATTACH_FILES> list = JsonConvert.JsonListDeserialize<DTO_ATTACH_FILES>(files);
            mgr.AddAttachFile(hddProcessID.Value, Sessions.UserID, hddFolderPath.Value, list);
        }
    }
    #endregion

    #region CreateProcessApproval
    private void CreateProcessApproval()
    {
        List<DTO_PROCESS_APPROVER> apprList = new List<DTO_PROCESS_APPROVER>();
        DateTime requestDate = DateTime.Now;
        long? nextSeq = 0;
        string currentApprover = string.Empty;
        string finalApprover = string.Empty;
        try
        {
            foreach (DTO_APPROVAL item in list)
            {
                DTO_PROCESS_APPROVER i = new DTO_PROCESS_APPROVER();
                i.PROCESS_ID = hddProcessID.Value;
                i.APPROVAL_TYPE = item.APPROVAL_TYPE;
                i.APPROVAL_SEQ = item.APPROVAL_SEQ;
                i.APPROVER_TYPE = item.APPROVER_TYPE;
                i.APPROVER_ID = item.USER_ID;
                i.APPROVER_ORG_NAME = item.ORG_NAME;
                i.ABSENCE_APPROVER_ID = item.USER_ID;
                i.ABSENCE_APPROVER_ORG_NAME = item.ORG_NAME;
                //i.ABSENCE_APPROVER_ID = item.ABSENCE_USER_ID;
                //i.ABSENCE_APPROVER_ORG_NAME = item.ABSENCE_ORG_NAME;

                // 결재 기안자 일경우
                if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.DRAFTER) && item.USER_ID.Equals(Sessions.UserID))
                {
                    i.STATUS = ApprovalUtil.ProcessStatus.ACEPTER;
                    i.COMMENT = txtComment.Text;
                    i.APPROVAL_DATE = requestDate;
                    nextSeq = item.IDX + 1;
                }
                else
                {
                    // 승인자가 다음 결재 순서인경우
                    if (nextSeq == item.IDX)
                    {
                        i.STATUS = ApprovalUtil.ProcessStatus.CURRENT_APPROVER;
                        currentApprover = item.USER_ID;
                    }
                    else
                    {
                        i.STATUS = ApprovalUtil.ProcessStatus.AWAITER;
                    }

                    if (item.APPROVAL_TYPE.Equals(ApprovalUtil.ApprovalType.APPROVER))
                    {
                        finalApprover = item.USER_ID;
                    }
                    i.COMMENT = String.Empty;
                }
                i.SENT_MAIL = ApprovalUtil.SentMail.NONE;
                i.CREATE_DATE = requestDate;
                apprList.Add(i);
            }

            DTO_PROCESS_DOCUMENT docProc = new DTO_PROCESS_DOCUMENT();
            docProc.PROCESS_ID = hddProcessID.Value;
            docProc.DOCUMENT_ID = hddDocumentID.Value;
            docProc.DOC_NAME = hddDocumentName.Value;
            docProc.SUBJECT = hddSubject.Value;
            docProc.DOC_NUM = "";
            docProc.PROCESS_STATUS = ApprovalUtil.ApprovalStatus.Request.ToString();
            docProc.REQUEST_DATE = requestDate;
            docProc.COMPANY_CODE = Sessions.CompanyCode;
            docProc.REQUESTER_ID = Sessions.UserID;
            docProc.CURRENT_APPROVER = currentApprover;
            docProc.FINAL_APPROVER = finalApprover;
            docProc.REJECTED_PROCESS_ID = hddRejectedProcessId.Value;
            docProc.CREATE_DATE = requestDate;


            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.InsertProcessApprove(apprList, docProc, hddDocumentID.Value, ApprovalUtil.ApprovalStatus.Request.ToString(), Sessions.UserID);
            }
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "CreateProcessApproval"), string.Format("Approver Request!( PROCESS_ID : {0} )", hddProcessID.Value), Sessions.UserID);
        }
        catch (Exception ex)
        {
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, ex.TargetSite.Name), ex.ToString(), Sessions.UserID);
            throw ex;
        }
    }
    #endregion

}
