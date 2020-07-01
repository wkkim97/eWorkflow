using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using System.Text;

/// <summary>
/// Approval, Reject, Withdraw 처리
/// </summary>
public partial class Approval_Process_Approve : DNSoft.eWF.FrameWork.Web.PageBase
{
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
        hddProcessType.Value = Request["processtype"].NullObjectToEmptyEx();

    } 
    #endregion

    #region Ok Click Event
    protected void btnOk_Click(object sender, EventArgs e)
    {
        string script = string.Empty;
        try
        {
            if (hddProcessType.Value.Equals(ApprovalUtil.ApprovalButtons.WITHDRAW.ToString()))
            {
                DoWithdraw(ApprovalUtil.ApprovalStatus.Withdraw.ToString(), ApprovalUtil.LogType.Withdraw.ToString(), ApprovalUtil.ProcessStatus.ACEPTER);
            }
            else if (hddProcessType.Value.Equals(ApprovalUtil.ApprovalButtons.APPROVAL.ToString()))
            {
                DoApproval(ApprovalUtil.ApprovalStatus.Processing.ToString(), ApprovalUtil.ProcessStatus.ACEPTER);
            }
            else if (hddProcessType.Value.Equals(ApprovalUtil.ApprovalButtons.REJECT.ToString()))
            {
                DoApproval(ApprovalUtil.ApprovalStatus.Reject.ToString(), ApprovalUtil.ProcessStatus.REJECTER);
            }
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
            if (hddProcessType.Value.Equals(ApprovalUtil.ApprovalButtons.WITHDRAW.ToString()))
            {
                mailSendType = ApprovalUtil.SendMailType.Withdraw.ToString();
            }
            else if (hddProcessType.Value.Equals(ApprovalUtil.ApprovalButtons.APPROVAL.ToString()))
            {
                bool isCompleted = false;
                string sName = string.Empty;
                using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
                {
                   isCompleted = mgr.IsCompletedApproval(hddProcessID.Value);
                }
                if (isCompleted)
                {
                    mailSendType = ApprovalUtil.SendMailType.FinalApproval.ToString();
                    // 후처리 작업 진행
                    using(Bayer.eWF.BSL.Configuration.Mgr.ConfigurationMgr mgr = new Bayer.eWF.BSL.Configuration.Mgr.ConfigurationMgr())
                    {
                        sName = mgr.GetAfterTreatementServiceName(hddDocumentID.Value);
                        if( sName.Length > 0)
                        {
                            script.AppendFormat("fn_AfterTreatment('{0}','{1}');",sName, hddProcessID.Value);
                        } 
                    }
                }
                else
                    mailSendType = ApprovalUtil.SendMailType.CurrentApprover.ToString();

                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "AfterTreatment"), string.Format("PROCESS_ID : {0},  IS_Completed : {1}, ServiceName : {2})", hddProcessID.Value, isCompleted, sName), Sessions.UserID);
            }
            else if (hddProcessType.Value.Equals(ApprovalUtil.ApprovalButtons.REJECT.ToString()))
            {
                mailSendType = ApprovalUtil.SendMailType.Reject.ToString();
            }

            if (mailSendType.Length > 0 && hddProcessID.Value.Length > 0)
            {
                script.AppendFormat("fn_sendMail('{0}','{1}', '{2}');", hddProcessID.Value, mailSendType, Sessions.MailAddress);
            }
            #endregion

            return script.ToString();
        }
        catch (Exception ex)
        {
            // 2015-08-27 에러 입력 (Bayer Korea Youngwoo Lee)
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, "AfterTreatmentScript"), string.Format("PROCESS_ID : {0},  mailSendType : {1}, MailAddress : {2})", hddProcessID.Value, mailSendType, Sessions.MailAddress) + ex.ToString(), Sessions.UserID);
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

    #region Dowithdraw
    private void DoWithdraw(string documentStatus, string logType, string approverStatus)
    {
        try
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                //string comment = string.Format("[Withdraw]({0}) : {1} {2}", DateTime.Now.ToString("yyyy-MM-dd"), Sessions.UserName, txtComment.Text);
                string comment = string.Format("{2}", DateTime.Now.ToString("yyyy-MM-dd"), Sessions.UserName, txtComment.Text);
                mgr.InsertWithdraw(hddDocumentID.Value, hddProcessID.Value, comment, documentStatus, Sessions.UserID, logType, approverStatus);
            }
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "DoWithdraw"), string.Format("PROCESS_ID : {0}, Status : {1})", hddProcessID.Value, approverStatus), Sessions.UserID);
        }
        catch(Exception ex)
        {
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, "DoWithdraw"), ex.ToString(), Sessions.UserID);
            throw ex;
        }
    } 
    #endregion

    #region DoApproval
    private void DoApproval(string documentStatus, string approverStatus)
    {
        try
        {
            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.UpdateProcessStatus(hddDocumentID.Value, hddProcessID.Value, txtComment.Text, documentStatus, Sessions.UserID, approverStatus);
            }
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "DoApproval"), string.Format("PROCESS_ID : {0},  ApproverStatus : {1}, DocumentStatus : {2})", hddProcessID.Value, approverStatus, documentStatus), Sessions.UserID);
        }
        catch (Exception ex)
        {
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, "DoApproval"), string.Format("PROCESS_ID : {0},  ApproverStatus : {1}, DocumentStatus : {2})", hddProcessID.Value, approverStatus, documentStatus) + ex.ToString(), Sessions.UserID);
            throw ex;
        }
    } 
    #endregion
     
}