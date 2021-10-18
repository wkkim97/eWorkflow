using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Reflection;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using Bayer.eWF.BSL.Common.Dto;

public partial class Common_AttachFileDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 버퍼링 없이 바로 다운로드 받게 한다.
        Page.Response.Clear();
        Page.Response.BufferOutput = false;
        string attachIDX = string.Empty;
        string fileName = string.Empty;
        string userId = string.Empty;

        FileInfo ofileinfo = null;
        DTO_ATTACH_FILES fileInfo;
        try
        {
            // 첨부파일 ID	
            attachIDX = Request["IDX"].NullObjectToEmptyEx();
            fileName = Request["FILENAME"].NullObjectToEmptyEx();
            userId = Request["USERID"].NullObjectToEmptyEx();

            if (fileName.IsNullOrEmptyEx())
            {
                using (Bayer.eWF.BSL.Common.Mgr.FileMgr mgr = new Bayer.eWF.BSL.Common.Mgr.FileMgr())
                {
                    fileInfo = mgr.SelectAttachFileInfo(Convert.ToInt32(attachIDX));
                }
                ofileinfo = new FileInfo(fileInfo.FILE_PATH);
            }
            else
            {
                string strTempUploadPath = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/UploadTempFolder");
                strTempUploadPath = string.Format(@"{0}\{1}\{2}\", strTempUploadPath, userId, ApprovalUtil.AttachFileType.Temp.ToString());

                fileInfo = new DTO_ATTACH_FILES();
                fileInfo.ATTACH_FILE_TYPE = ApprovalUtil.AttachFileType.Temp.ToString();
                fileInfo.COMMENT_IDX = 0;   
                fileInfo.DISPLAY_FILE_NAME = fileName;
                ofileinfo = new FileInfo(strTempUploadPath + fileName);
            }

            if (ofileinfo.Exists)
            {
                // 한글명일 경우 깨지지 않게 하기 위해
                fileInfo.DISPLAY_FILE_NAME = HttpUtility.UrlEncode(fileInfo.DISPLAY_FILE_NAME, new System.Text.UTF8Encoding()).Replace("+", "%20");

                 this.Response.Clear();
                this.Response.ContentType = "application/unknown";
                this.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                this.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.DISPLAY_FILE_NAME);
                this.Response.WriteFile(ofileinfo.FullName);
                this.Response.End();
            }
            else
            {
                Response.Write("첨부파일의 존재하지 않거나 접근 권한이 없습니다.");
                Response.End();
            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
        finally
        {
            if (ofileinfo != null)
            {
                ofileinfo = null;
            }
        }
    }
}