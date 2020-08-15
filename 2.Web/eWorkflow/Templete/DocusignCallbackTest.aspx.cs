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

public partial class Templete_DocusignCallbackTest : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    string token;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.callbackResult.Value = Request.QueryString["code"];
        this.documentId.Value = Request.QueryString["state"];
        this.cdName.Value = "Wookyung Kim";
        this.cdEmail.Value = "wookyung.kim@bayer.com";
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
    public static string CreateEnvelope(string accessToken, string filePath, string fileName, string cdName, string cdEmail,string documentNo)
    {
        using (Bayer.eWF.BSL.Common.Mgr.DocusignMgr mgr = new Bayer.eWF.BSL.Common.Mgr.DocusignMgr())
        {
            string result = mgr.CreateEnvelope(accessToken, filePath, fileName, cdName, cdEmail, documentNo);
            return result;
        }
    }
}