using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using System.Text;
using Telerik.Web.UI;

public partial class Approval_Process_Forward : DNSoft.eWF.FrameWork.Web.PageBase
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
    }
    #endregion

    #region Forward Click Event
    protected void btnForward_Click(object sender, EventArgs e)
    {
        string script = string.Empty;
        try
        {
            this.btnForward.Enabled = false;
            DoForward();
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

            mailSendType = ApprovalUtil.SendMailType.Forward.ToString();

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

    #region DoForward
    private void DoForward()
    {
        AutoCompleteBoxEntryCollection EntryList = UserAutoCompleteBox.GetEntries();
        List<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_COMPLETED> add;
        Bayer.eWF.BSL.Common.Dto.SmallUserInfoDto userInfo;
        add = new List<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_COMPLETED>();

        try
        {
            string userName = string.Empty;
            foreach (AutoCompleteBoxEntry entry in EntryList)
            {

                Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_COMPLETED item = new Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_COMPLETED();
                userInfo = JsonConvert.JsonDeserialize<Bayer.eWF.BSL.Common.Dto.SmallUserInfoDto>(entry.Value);

                item.PROCESS_ID = hddProcessID.Value;

                item.APPROVAL_TYPE = ApprovalUtil.ApprovalType.REVIEWER;
                item.APPROVAL_SEQ = 0;
                item.APPROVER_TYPE = ApprovalUtil.ApproverType.FORWARD;
                item.APPROVER_ID = userInfo.USER_ID;
                item.APPROVER_ORG_NAME = "";
                item.ABSENCE_APPROVER_ID = userInfo.USER_ID;
                item.ABSENCE_APPROVER_ORG_NAME = "";
                item.STATUS = ApprovalUtil.ProcessStatus.AWAITER;
                item.SENT_MAIL = ApprovalUtil.SentMail.NONE;
                item.COMMENT = "";
                userName += userInfo.FULL_NAME + "/";
                add.Add(item);
            }
            if (userName.Length > 1)
                userName = userName.Substring(0, userName.Length - 1);
            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                //string comment = string.Format("[Forward]({0}) : \"{1}\" is forwarded to \"{2}\"", DateTime.Now.ToString("yyyy-MM-dd"), Sessions.UserName, userName);
                string comment = string.Format("\"{1}\" To \"{2}\"", DateTime.Now.ToString("yyyy-MM-dd"), Sessions.UserName, userName);
                mgr.InsertForward(add, comment, Sessions.UserID, ApprovalUtil.LogType.Forward.ToString());
            }
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "DoForward"), string.Format("PROCESS_ID : {0}", hddProcessID.Value), Sessions.UserID);
        }
        catch (Exception ex)
        {
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, "DoForward"), JsonConvert.toJson<Bayer.eWF.BSL.Approval.Dto.DTO_PROCESS_APPROVER_COMPLETED>(add), Sessions.UserID);
            throw ex;
        }
        finally
        {
            if (add != null)
            {
                add.Clear();
                add = null;
            }
        }
    }
    #endregion

}