using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Templete_EncryptTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnEncrypt_Click(object sender, EventArgs e)
    {
        lblResponse.Text = DNSoft.eW.FrameWork.eWBase.eWEncrypt(txtInput.Text);
    }
    protected void btnDecrypt_Click(object sender, EventArgs e)
    {
        lblResponse.Text = DNSoft.eW.FrameWork.eWBase.eWDecrypt(txtInput.Text);
    }
}