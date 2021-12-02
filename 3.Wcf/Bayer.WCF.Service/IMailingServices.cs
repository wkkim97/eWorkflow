using Bayer.eWF.BSL.Approval.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Bayer.WCF.Service
{
    [ServiceContract]
    public interface IMailingServices
    {

        // [OperationContract]
        // string Test();
        

        [OperationContract]         
        void InvokeSendMail(string processID, string sendMailType, string senderAddress);
  
        [OperationContract]
        void SendNoticeMail(string sendMailType, string searchdata, string senderAddress);

        [OperationContract]
        void SendNoticeMailApprover(string sendMailType, string searchdata, string senderAddress);
 
        [OperationContract]
        string SendToAgency(string processID, string updaterID);
 
    }

    [DataContract]
    public class MailFormat
    {
        public string ApproverList { get; set; }
        private string subject = string.Empty;
        private string body = string.Empty;
        private string documentUrl = string.Empty;

        [DataMember]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        [DataMember]
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        [DataMember]
        public string DocumentUrl
        {
            get { return documentUrl; }
            set { documentUrl = value; }
        }
    }

    [DataContract]
    public class MailItem
    {
        public string ApproverList { get; set; }
        private List<string> toAddress = null;
        private string subject = string.Empty;
        private string body = string.Empty;
        private bool isHtml = true;
        private bool isTravelManagement = false;
        private string from = string.Empty;
        private string to = string.Empty;
        private string cc = string.Empty;
        private string bcc = string.Empty;

        [DataMember]
        public List<string> TOAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }

        [DataMember]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        [DataMember]
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        [DataMember]
        public bool ISHtml
        {
            get { return isHtml; }
            set { isHtml = value; }
        }

        [DataMember]
        public bool IsTravelManagement
        {
            get { return isTravelManagement; }
            set { isTravelManagement = value; }
        }

        [DataMember]
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        [DataMember]
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        [DataMember]
        public string Cc
        {
            get { return cc; }
            set { cc = value; }
        }

        [DataMember]
        public string Bcc
        {
            get { return bcc; }
            set { bcc = value; }
        }
    }
}
