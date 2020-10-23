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
using System.IO;
using Bayer.eWF.BSL.Common.Dto;
using Bayer.eWF.BSL.Approval.Dto;
using System.Web.Script.Serialization;

public partial class Approval_Process_InputComment : DNSoft.eWF.FrameWork.Web.PageBase
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

        string strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");
        hddUploadFolder.Value = string.Format(@"{0}\{1}\{2}\", strTempUploadPath, Sessions.UserID, hddProcessID.Value);

        SelectApprover(hddProcessID.Value);
    } 
    #endregion

    private void SelectApprover(string processId)
    {
        List<DTO_PROCESS_APPROVAL_LIST> appr;
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            appr = mgr.SelectProcessApproverList(processId);

            this.radListApprover.DataSource = appr;
            this.radListApprover.DataBind();
        }
    }

    #region asyncUpload Uploaded Event
    protected void asyncUpload_FileUploaded(object sender, Telerik.Web.UI.FileUploadedEventArgs e)
    {
        string folder = hddUploadFolder.Value;

        StringBuilder sbAttachFileXml = new StringBuilder(512);
        Stream oStm = null;
        FileStream oFileStream = null;
        byte[] buffer = null;

        try
        {
            buffer = new byte[e.File.ContentLength];

            using (oStm = e.File.InputStream)
            {
                int nbytesRead = oStm.Read(buffer, 0, Convert.ToInt32(e.File.ContentLength));
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                oFileStream = new FileStream(folder + e.File.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                oFileStream.Write(buffer, 0, nbytesRead);

            }

            DTO_ATTACH_FILES f = new DTO_ATTACH_FILES();
            f.SEQ = 1;
            f.PROCESS_ID = hddProcessID.Value;
            f.DISPLAY_FILE_NAME = e.File.FileName; 
            f.SAVED_FILE_NAME = e.File.FileName;
            f.FILE_SIZE = e.File.ContentLength;
            f.ATTACH_FILE_TYPE = ApprovalUtil.AttachFileType.Comment.ToString();
            f.FILE_PATH = string.Empty;
            f.COMMENT_IDX = 0;

            hddAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(f);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sbAttachFileXml != null) sbAttachFileXml = null;
            if (oStm != null)
            {
                oStm.Close();
                oStm.Dispose();
                oStm = null;
            }

            if (oFileStream != null)
            {
                oFileStream.Close();
                oFileStream.Dispose();
                oFileStream = null;
            }

        }
    }
    #endregion

    #region btnSaved Click Event
    protected void btnSaved_Click(object sender, EventArgs e)
    {
        string script = string.Empty;
        try
        {
            this.btnSaved.Enabled = false;
            DoInputComment();
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

            mailSendType = ApprovalUtil.SendMailType.InputComment.ToString();

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                mgr.UpdateApproverSentMailComment(hddProcessID.Value, "R", this.hddSelectedId.Value, ",");
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

    #region DoInputComment
    private void DoInputComment()
    {
        try
        {

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
            {
                string comment = string.Format("{0}", txtComment.Text);
                DTO_ATTACH_FILES f = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(hddAttachFiles.Value);
                mgr.InsertInputComment(hddProcessID.Value, comment, Sessions.UserID, ApprovalUtil.LogType.InputComment.ToString(), hddUploadFolder.Value, f);
            }
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, "DoInputComment"), string.Format("PROCESS_ID : {0}", hddProcessID.Value), Sessions.UserID);
        }
        catch(Exception ex)
        {
            Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, "DoInputComment"), string.Format("PROCESS_ID : {0}, Comment : {1}", hddProcessID.Value, txtComment.Text), Sessions.UserID);
            throw ex;
        }
    }  
    #endregion

    protected void radListApprover_ItemDataBound(object sender, RadListBoxItemEventArgs e)
    {
        
    }
}