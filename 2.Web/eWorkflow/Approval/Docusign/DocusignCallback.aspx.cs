using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Bayer.eWF.BSL.Approval.Dao;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Approval.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using Bayer.eWF.BSL.Common.Mgr;

public partial class Docusign_DocusignCallback : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string documentId = Request.QueryString["state"].Split('|')[0];
        this.documentId.Value = documentId;
        //string documentNo = Request.QueryString["documentNo"];
        //state 만 call back 하여 state 에 document no 추가
        this.documentNo.Value = Request.QueryString["state"].Split('|')[1];

        this.callbackResult.Value = Request.QueryString["code"];

        DTO_ATTACH_FILES fileInfo;
        string processID, fileName, filePath, cdName, cdEmail;

        //파일정보 가져오기
        using (FileMgr mgr = new FileMgr())
        {
            fileInfo = mgr.SelectAttachFileInfo(Convert.ToInt32(documentId));
            processID = fileInfo.PROCESS_ID;
            filePath = fileInfo.FILE_PATH;
            fileName = fileInfo.DISPLAY_FILE_NAME;
        }

        this.fileName.Value = fileName;
        this.filePath.Value = filePath;

        //문서정보 가져오기
        using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new Bayer.eWF.BSL.Approval.Mgr.CommonMgr())
        {
            List<DTO_PROCESS_APPROVAL_LIST> list = mgr.SelectProcessApproverList(processID);
            DTO_PROCESS_APPROVAL_LIST one = list.Where(x => x.IDX == 1).FirstOrDefault();
            cdName = one.APPROVER;
            cdEmail = one.MAIL_ADDRESS;
        }

        this.cdName.Value = cdName;
        this.cdEmail.Value = cdEmail;
        //this.cdName.Value = "WooKyung Kim";
        //this.cdEmail.Value = "wookyung.kim@bayer.com";
    }

    [System.Web.Services.WebMethod()]
    public static string GetAccessToken(string redirectToken)
    {
        using (Bayer.eWF.BSL.Common.Mgr.DocusignMgr mgr = new Bayer.eWF.BSL.Common.Mgr.DocusignMgr())
        {
            string result = mgr.GetAccessToken(redirectToken);
            return result;
        }
    }
    [System.Web.Services.WebMethod()]
    public static string GetAccountId(string accessToken)
    {
        using (Bayer.eWF.BSL.Common.Mgr.DocusignMgr mgr = new Bayer.eWF.BSL.Common.Mgr.DocusignMgr())
        {
            string result = mgr.GetAccountId(accessToken);
            return result;
        }
    }
    [System.Web.Services.WebMethod()]
    public static string goLivetest(string accessToken,string envelopeId)
    {
        using (Bayer.eWF.BSL.Common.Mgr.DocusignMgr mgr = new Bayer.eWF.BSL.Common.Mgr.DocusignMgr())
        {
            string result = mgr.goLivetest(accessToken, envelopeId);
            return result;
        }
    }
    [System.Web.Services.WebMethod()]
    public static string CreateEnvelope(string accessToken, string filePath, string fileName, string cdName, string cdEmail, string documentNo)
    {
        using (Bayer.eWF.BSL.Common.Mgr.DocusignMgr mgr = new Bayer.eWF.BSL.Common.Mgr.DocusignMgr())
        {
            string result = mgr.CreateEnvelope(accessToken, filePath, fileName, cdName, cdEmail, documentNo);
            return result;
        }
    }
}