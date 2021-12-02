using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Docusign_Login : DNSoft.eWF.FrameWork.Web.WebBase.DocBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string state = Request.QueryString["documentId"].ToString();
        string documentNo = Request.QueryString["documentNo"].ToString();
        if (String.IsNullOrEmpty(state)) {
            Page.Header.Controls.Add(
                new LiteralControl(
                    "<script>alert('첨부문서 Id가 없습니다');window.close();</script>"
                )
            );
        }
        else
        {
            string integrationKey = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignIntegrationKey");
            string redirect = DNSoft.eW.FrameWork.eWBase.GetConfig("//eWorkflowInfo/DocusignAuthReturnUrl");
            string userEmail = Sessions.MailAddress;

            this.integrationKey.Value = integrationKey;
            this.redirect.Value = redirect;
            this.userEmail.Value = userEmail;
            this.state.Value = state;
            this.documentNo.Value = documentNo;

            //Response.Redirect("https://account-d.docusign.com/oauth/auth?response_type=code&scope=signature&client_id=" + integrationKey + "&redirect_uri=" + redirect + "&login_hint=" + userEmail + "&state=" + state);
        }
    }
}