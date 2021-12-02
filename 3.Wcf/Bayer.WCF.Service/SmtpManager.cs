using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.WCF.Service
{
    public class SmtpManager
    {
        public static SmtpClient CreateSmtpClientObj()
        {
            string smtpServer = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/SmtpServer");

            /* 기본 인증으로 처리하기위해 주석처리하고 아래 코드로 변경
            string sender = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/Sender");
            string senderPs = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/SenderPs");
            int port = Convert.ToInt32(DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/Port"));
          
            SmtpClient SmtpServer = new SmtpClient(smtpServer);

            SmtpServer.Port = port;
            string dcriptPW = DNSoft.eW.FrameWork.eWBase.eWDecrypt(senderPs);
            SmtpServer.Credentials = new System.Net.NetworkCredential(sender, dcriptPW);
            SmtpServer.EnableSsl = true;
            */

            SmtpClient client = new SmtpClient(smtpServer);
            client.UseDefaultCredentials = true;

            return client;
        }
    }
}
