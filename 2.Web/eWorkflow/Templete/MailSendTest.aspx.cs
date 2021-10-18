using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;
 
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Templete_MailSendTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // DefaultCredentials 사용하여 메일 발송
        string to = "wookyung.kim@bayer.com";
        string from = "youngwoo.lee@bayer.com";
        MailMessage message = new MailMessage(from, to);
        message.Subject = "Using the new SMTP client222.";
        message.IsBodyHtml = true;
        message.Body = @"Using this new feature,<br/> you can send an e-mail message from an application very easily.";

        SmtpClient client = new SmtpClient("smtp.de.bayer.cnb");        

        client.UseDefaultCredentials = true;

        try
        {
            client.Send(message);
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("Exception caught in CreateTestMessage2(): {0}",
                  ex.ToString()));
        }
    }
}