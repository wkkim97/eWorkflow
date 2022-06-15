using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;
 
using System.Web.UI;
using System.Web.UI.WebControls;

using Bayer.WCF.Service;//20220615 Bob 添加 修改发送邮件测试功能，添加次引用需把Bayer.WCF.Service.dll和Bayer.WCF.Service.pdb文件复制到改项目Bin目录下

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
            #region //20220615 Bob  修改发送邮件测试功能
            //client.Send(message);//修改前代码
            #region 修改后代码
            MailingService mailingService = new MailingService();
            mailingService.InvokeSendMail("P000050293", "CurrentApprover", "cheng.yin.ext@bayer.com");

            Response.Write(string.Format("Send mail test successful!"));
            #endregion
            #endregion
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("Exception caught in CreateTestMessage2(): {0}",
                  ex.ToString()));
        }
    }
}