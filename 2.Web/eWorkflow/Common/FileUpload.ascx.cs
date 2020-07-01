using System;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Common.Dto;
using System.Collections.Generic;

using DNSoft.eW.FrameWork;

public partial class Common_FileUpload : DNSoft.eWF.FrameWork.Web.UserControlsBase
{
    #region 프로퍼티
    private int _iAttachIdx = 0;
    private string _folderPath = string.Empty;
    private string _processID = string.Empty;

    public string Mode { get; set; }

    public string ProcessID
    {
        get
        {
            return _processID;
        }
        set
        {
            _processID = value;
        }
    }

    public string UploadFileTemp
    {
        get
        {
            return hhdUploadFolder.Value;
        }
    }

    public string XmlAttachFiles
    {
        get
        {
            return hhdAttachFiles.Value;
        }
    }
    #endregion

    #region SaveAttach 저장시 이벤트
    public void SaveAttach(string processid)
    {
        List<DTO_ATTACH_FILES> list = JsonConvert.JsonListDeserialize<DTO_ATTACH_FILES>(hhdAttachFiles.Value);

        try
        {
            foreach (DTO_ATTACH_FILES file in list)
            {
                file.PROCESS_ID = processid;
                file.CREATE_DATE = DateTime.Now;
                file.CREATOR_ID = Sessions.UserID;
                file.FILE_PATH = hhdUploadFolder.Value + file.SAVED_FILE_NAME;
                file.ATTACH_FILE_TYPE = ApprovalUtil.AttachFileType.Temp.ToString();
            }

            using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
            {
                mgr.InsertAttachFile(list);
            }
            SetAttachFilesList();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

        string strTempUploadPath = string.Empty;
        string strUserID = this.Sessions.UserID;
        try
        {
            if (Mode.Equals("UPLOAD"))
            {
                if (!this.IsPostBack)
                {
                    strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");
                    hhdUploadFolder.Value = string.Format(@"{0}\{1}\{2}\", strTempUploadPath, strUserID, ApprovalUtil.AttachFileType.Temp.ToString());
                    SetInitControls();
                    this.hddUserID.Value = Sessions.UserID;
                }
                divFIleUloadArea.Style.Add("display", "");
                divFileDownloadArea.Style.Add("display", "none");
            }
            else
            {
                FileListBind();
                divFIleUloadArea.Style.Add("display", "none");
                divFileDownloadArea.Style.Add("display", "");
            }
            //uploadFileAttach.PostbackTriggers = new string[] { "btnAttachFile" };
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }

    }
    #endregion

    #region SetInitControls
    private void SetInitControls()
    {
        SetAttachFilesList();
    }
    #endregion

    #region SetAttachFilesList
    // 조회된 파일리스트 데이터를 hidden값에 저장
    private void SetAttachFilesList()
    {
        List<DTO_ATTACH_FILES> filelist;
        filelist = GetFiles(ProcessID, ApprovalUtil.AttachFileType.Temp.ToString());
        hhdAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(filelist);
    }
    #endregion

    #region GetFiles
    // 첨부 파일 데이터 조회
    private List<DTO_ATTACH_FILES> GetFiles(string processid, string filetype)
    {
        List<DTO_ATTACH_FILES> filelist;
        using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
        {
            filelist = mgr.SelectAttachFileList(ProcessID, filetype);
        }
        return filelist;
    }
    #endregion

    #region FileListBind
    // 첨부 파일
    private void FileListBind()
    {
        viewfileList.DataSource = GetFiles(ProcessID, ApprovalUtil.AttachFileType.Common.ToString());
        viewfileList.DataBind();
    }
    #endregion

    #region FIleUpload
    // 비동기로 데이터 첨부 파일을 Temp 파일로 업로드
    protected void uploadFileAttach_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        string folder = hhdUploadFolder.Value;

        StringBuilder sbAttachFileXml = new StringBuilder(512);
        Stream oStm = null;
        FileStream oFileStream = null;
        byte[] buffer = null;

        try
        {
            buffer = new byte[e.File.ContentLength];
            //if (hhdUploadComplete.Value == "true") return;
            using (oStm = e.File.InputStream)
            {
                int nbytesRead = oStm.Read(buffer, 0, Convert.ToInt32(e.File.ContentLength));
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                oFileStream = new FileStream(folder + e.File.FileName.Replace("+", "_"), FileMode.OpenOrCreate, FileAccess.Write);
                oFileStream.Write(buffer, 0, nbytesRead);

            }


            List<DTO_ATTACH_FILES> list = JsonConvert.JsonListDeserialize<DTO_ATTACH_FILES>(hhdAttachFiles.Value);

            if (list == null) list = new List<DTO_ATTACH_FILES>();

            DTO_ATTACH_FILES f = new DTO_ATTACH_FILES();
            f.SEQ = ++_iAttachIdx;
            f.PROCESS_ID = ProcessID;
            f.DISPLAY_FILE_NAME = e.File.FileName.Replace("+","_");
            f.SAVED_FILE_NAME = e.File.FileName.Replace("+", "_");
            f.FILE_SIZE = e.File.ContentLength;
            f.ATTACH_FILE_TYPE = ApprovalUtil.AttachFileType.Common.ToString();
            f.FILE_PATH = string.Empty;
            f.COMMENT_IDX = 0;

            list.Add(f);
            hhdAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(list);

            hhdUploadComplete.Value = "true";
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

    #region DeleteAttachFile
    private void DeleteAttachFile()
    {
        string deleteList = hhdDeleteList.Value;
        string[] arDelete = null;
        string[] names = null;

        string folderPath = string.Empty;
        string Idx = string.Empty;
        try
        {
            List<DTO_ATTACH_FILES> list = JsonConvert.JsonListDeserialize<DTO_ATTACH_FILES>(hhdAttachFiles.Value);

            folderPath = hhdUploadFolder.Value + @"\";
            if (deleteList.Length > 0)
            {
                arDelete = deleteList.Split('|');
                names = null;

                foreach (string file in arDelete)
                {
                    if (file.Length > 0)
                    {
                        names = file.Split('*');
                        if (Idx.Length > 0)
                            Idx += "|" + names[1].ToString();
                        else
                            Idx += names[1].ToString();

                        if (File.Exists(folderPath + names[0]))
                        {
                            File.Delete(folderPath + names[0]);
                        }

                        DTO_ATTACH_FILES item = list.Find(x => x.IDX == Convert.ToInt32(names[1]));
                        if (item != null)
                            list.Remove(item);
                    }
                }
                using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
                {
                    mgr.DeleteAttachFIles(Idx);
                }
            }

            hhdAttachFiles.Value = JsonConvert.toJson<DTO_ATTACH_FILES>(list);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (arDelete != null)
            {
                arDelete = null;
            }
            if (names != null)
            {
                names = null;
            }
        }
    }
    #endregion

    #region RadAjaxManager1_AjaxRequest
    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        switch (e.Argument.ToString())
        {
            case "DeleteAttachFile":
                DeleteAttachFile();
                break;
        }
    }
    #endregion
}