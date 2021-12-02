using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Templete_DocusignTest : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string state = Request.QueryString["documentId"].ToString();
        if (String.IsNullOrEmpty(state)) {
            Page.Header.Controls.Add(
                new LiteralControl(
                    "<script>alert('첨부문서 Id가 없습니다');window.close();</script>"
                )
            );
        }
        else
        {
            string integrationKey = "f37e262a-8a5e-426d-b31d-d2844d43b26e";
            string redirect = "http://localhost:56680/eWorks/Templete/DocusignCallbackTest.aspx";
            string userEmail = "bumyoung.kim@bayer.com";
            Response.Redirect("https://account-d.docusign.com/oauth/auth?response_type=code&scope=signature&client_id=" + integrationKey + "&redirect_uri=" + redirect + "&login_hint=" + userEmail + "&state=" + state);
        }
    }
}