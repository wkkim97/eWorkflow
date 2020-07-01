using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Common.Dto;
using System.Text;
using System.IO;

public partial class Board_Edit : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        { 
            if (!this.IsPostBack)
            {
                InitControls();
            }
            PageLoadInfo();
        }catch(Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private void PageLoadInfo()
    {

    }

    private void InitControls()
    {
        hspanRegister.InnerHtml = Sessions.UserName;
        hspanRegistDate.InnerHtml = DateTime.Now.ToString("yyyy-MM-dd");
        hddIDX.Value = Request["idx"].NullObjectToEmptyEx();

        if (Sessions.UserRole.Equals(ApprovalUtil.UserRole.Admin) || Sessions.UserRole.Equals(ApprovalUtil.UserRole.Design))
        {
            btnSave.Visible = true;
            if (hddIDX.Value.Length > 0)
            {
                btnDelete.Visible = true;
            }
            else
            {
                btnDelete.Visible = false;
            }
        }
        else
        {
            asyncUpload.Visible = false;
            btnSave.Visible = false;
            btnDelete.Visible = false;
            txtSubject.ReadOnly = true;
            txtSubject.BorderStyle = BorderStyle.None;
            txtBody.EditModes = Telerik.Web.UI.EditModes.Preview;
            txtBody.Enabled = false;
        }
        string strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");
        hddUploadFolder.Value = string.Format(@"{0}\{1}\", strTempUploadPath,"board");
        ControlBind();
    }

    private void ControlBind()
    {
        DTO_BOARD_NOTICE notice = null;
        if (hddIDX.Value.Length > 0)
        {
            int idx = Convert.ToInt32(hddIDX.Value);

            using (Bayer.eWF.BSL.Common.Mgr.NoticeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.NoticeMgr())
            {
                notice = mgr.GetNoticeItem(idx);
            }
            hspanRegister.InnerHtml = notice.CREATE_NAME;
            hspanRegistDate.InnerHtml = notice.CREATE_DATE.ToString("yyyy-MM-dd");
            txtSubject.Text = notice.SUBJECT;
            txtBody.Content = notice.BODY;
            if(notice.FILE_IDX > 0)
            {
                hrefFIle.Attributes.Add("onclick", string.Format("javascript:fn_FileDownload({0})", notice.FILE_IDX));
                hrefFIle.InnerText = notice.DISPLAY_FILE_NAME;
                asyncUpload.Visible = false;
            }
        }
        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        DTO_BOARD_NOTICE notice =new DTO_BOARD_NOTICE();
        int idx = hddIDX.Value.Length > 0 ? Convert.ToInt32(hddIDX.Value) : 0;
        try{
            notice.IDX = idx;
            notice.CREATE_NAME =  hspanRegister.InnerHtml;
            notice.CREATE_DATE = DateTime.Now;
            notice.SUBJECT = txtSubject.Text;
            notice.BODY = txtBody.Content;
            notice.CREATE_ID = Sessions.UserID;
            notice.UPDATE_ID = Sessions.UserID;
            using (Bayer.eWF.BSL.Common.Mgr.NoticeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.NoticeMgr())
            {
                idx = mgr.MergeBoardNotice(notice);
            }
            using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr()){
                DTO_ATTACH_FILES f = JsonConvert.JsonDeserialize<DTO_ATTACH_FILES>(hddAttachFiles.Value);
                mgr.AddAttachFile(idx.ToString(), Sessions.UserID, hddUploadFolder.Value, f); 
            }
            ClientWindowClose("true");
        }
        catch(Exception ex)
        {
            errorMessage = ex.ToString();
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int idx = hddIDX.Value.Length > 0 ? Convert.ToInt32(hddIDX.Value) : 0;
            using (Bayer.eWF.BSL.Common.Mgr.NoticeMgr mgr = new Bayer.eWF.BSL.Common.Mgr.NoticeMgr())
            {
                mgr.DeleteBoardNotice(idx);
            }
            ClientWindowClose("true");
        }
        catch(Exception ex)
        {
            errorMessage = ex.ToString();
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
            f.DISPLAY_FILE_NAME = e.File.FileName;
            f.SAVED_FILE_NAME = e.File.FileName;
            f.FILE_SIZE = e.File.ContentLength;
            f.ATTACH_FILE_TYPE = "Board";
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
}