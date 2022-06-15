using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Mail;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dto;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Web;
using System.ServiceModel.Web;
using System.Web.Hosting;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using System.Security.Principal;
using System.ServiceModel.Activation;


namespace Bayer.WCF.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MailingService : IMailingServices
    {
        #region SendMail

        //[OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "InvokeSendMail/{processID}/{sendMailType}/{senderAddress}"),]
        public void InvokeSendMail(string processID, string sendMailType, string senderAddress)
        {

            // using (SystemLogDao dao = new SystemLogDao())
            // {
            //     DTO_SYSTEM_LOG log;
            //     log = new DTO_SYSTEM_LOG();
            //     log.TYPE = "check";
            //     log.EVENT_NAME = "wooKyung";
            //     log.MESSAGE = "please";
            //     log.CREATER_ID = "BKKWK";
            //     dao.InsertSystemLog(log);
            // }

           

            MailFormat format = null;
            try
            {
                WriteLog("START ----"+ processID+"/"+ sendMailType+"/"+ senderAddress);
                format = GetMailFormat(sendMailType);                
                List<DTO_SENDMAIL_TO_ADDRESS_LIST> list = GetTargetMailAddress(processID, sendMailType);
                SendSmtpMail(processID, format, senderAddress, sendMailType, list);
                UpdateApproverSentMailStatus(processID, format.ApproverList);
                WriteLog("END ----" + processID + "/" + sendMailType + "/" + senderAddress);
            }
            catch (Exception ex)
            {
                WriteLog("InvokeSendMail-Error" + ex.Message);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                
            
            }
            finally
            {
                if (format != null)
                {
                    format = null;
                }
            }
        }
        public static void WriteLog(string sMessage)
        {
            //test 서버는 f 폴더가 없어서 D 로 변경
            string strLogDir = @"f:\Project\Debug\Log";//20220615 Bob修改，把d盘改为f盘， 生产环境log目录在F盘
            string strLogWriteYN = "Y";

            string strFullPath = string.Format(@"{0}\{1} SendNoticeMailServiceLog.txt", strLogDir, DateTime.Now.ToString("yyyy-MM-dd"));

            FileStream oFs = null;
            StreamWriter oWriter = null;

            try
            {
                // 디렉토리 존재 여부 확인
                if (!Directory.Exists(strLogDir))
                {
                    //디렉토리가 없는 경우에 폴더 생성 여부 확인
                    if (strLogWriteYN.Equals("Y"))
                    {
                        System.IO.Directory.CreateDirectory(strLogDir);
                        //return;
                    }
                }

                oFs = new FileStream(strFullPath, FileMode.Append, FileAccess.Write, FileShare.Write);
                oWriter = new StreamWriter(oFs, System.Text.Encoding.UTF8);
                oWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + sMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (oWriter != null) oWriter.Close();
                if (oFs != null) oFs.Close();
            }
        }
        #endregion

        #region SendNoticeMail
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendNoticeMail/{sendMailType}/{searchdata}/{senderAddress}"),]
        public void SendNoticeMail(string sendMailType, string searchdata, string senderAddress)
        {
            MailFormat format = null;
            try
            {
                List<DTO_SENDMAIL_TO_ADDRESS_LIST> sendlist = null;

                if (sendMailType.Equals("DelayApproval"))
                {
                    DateTime dtbasicdate = Convert.ToDateTime(searchdata);
                    sendlist = GetNoticeMailList(dtbasicdate);
                }
                else if (sendMailType.Equals("ReturnGoods"))
                {
                    sendlist = GetTargetMailAddress(searchdata, sendMailType);
                }
                format = GetMailFormat(sendMailType);

                SendSmtpMailByService(format, sendMailType, sendlist, senderAddress);
            }
            catch (Exception ex)
            {
                WriteLog("DDDDDDDDDDDDD" + ex.Message);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
            finally
            {
                if (format != null)
                {
                    format = null;
                }
            }
        }
        #endregion

        // test
        #region SendNoticeMailApprover
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendNoticeMailApprover/{sendMailType}/{searchdata}/{senderAddress}"),]
        public void SendNoticeMailApprover(string sendMailType, string searchdata, string senderAddress)
        {
            MailFormat format = null;
            try
            {
                List<DTO_SENDMAIL_TO_ADDRESS_LIST> sendlist = null;

                if (sendMailType.Equals("ReturnGoods"))
                {
                    //old
                    //sendlist = GetTargetMailAddressApprover();
                    sendlist = GetTargetMailAddressApprover(searchdata);

                }
                format = GetMailFormat(sendMailType);

                SendSmtpMailByService(format, sendMailType, sendlist, senderAddress);
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
            finally
            {
                if (format != null)
                {
                    format = null;
                }
            }
        }
        #endregion

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "Test"),]
        public string Test()
        {
            return "Test!";
        }

        #region UpdateApproverSentMailStatus
        private void UpdateApproverSentMailStatus(string processid, string ApprList)
        {
            try
            {
                using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new eWF.BSL.Approval.Mgr.CommonMgr())
                {
                    mgr.UpdateApproverSentMail(processid, ApprList, ",");
                }
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
        }
        #endregion

        #region SendMailSMTP

        private string GetInputComment(string processId)
        {
            try
            {
                using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new eWF.BSL.Approval.Mgr.CommonMgr())
                {
                    return mgr.SelectInputComment(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        private RejectDocumentMailCommentDto GetRejecterInfo(string processId)
        {
            try
            {
                using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new eWF.BSL.Approval.Mgr.CommonMgr())
                {
                    return mgr.SelectProcessRejectUser(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 2014.12.26 기존 SendMailSMTP에서 추가로 변경
        /// Dear가 각 메일본문에 누적 안되도록
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="format"></param>
        /// <param name="senderAddress"></param>
        /// <param name="sendMailType"></param>
        /// <returns></returns>
        private string SendSmtpMail(string processId, MailFormat format, string senderAddress, string sendMailType, List<DTO_SENDMAIL_TO_ADDRESS_LIST> list)
        {
            string retValue = string.Empty;
            string documentid = string.Empty;
            string documentName = string.Empty;
            string formName = string.Empty;
            string comment = string.Empty;
            string DearName = string.Empty;
            string documentUri = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/DocumentUrl");
            string sender = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/Sender");
            try
            {
                SmtpClient SmtpServer = SmtpManager.CreateSmtpClientObj();

                MailMessage mail = null;


                format.ApproverList = string.Empty;
                foreach (DTO_SENDMAIL_TO_ADDRESS_LIST approver in list)
                {

                    if (approver.MAIL_ADDRESS.IsNullOrEmptyEx()) continue;
                    if (format.ApproverList.Equals(string.Empty))
                        format.ApproverList = approver.APPROVER_ID;
                    else
                        format.ApproverList = format.ApproverList + "," + approver.APPROVER_ID;

                    if (documentid.Length == 0) documentid = approver.DOCUMENT_ID;
                    if (documentName.Length == 0) documentName = approver.DOC_NAME;
                    if (formName.Length == 0) formName = approver.FORM_NAME;
                    if (comment.Length == 0) comment = approver.COMMENT;
                    DearName = approver.APPROVER_NAME;

                    //string url = string.Format(@"{0}/{1}?processid={2}&documentid={3}&maillink=Y", documentUri, formName, processId, documentid);
                    string url = string.Format(@"{0}/{1}?processid={2}&documentid={3}", documentUri, formName, processId, documentid);

                    mail = new MailMessage();
                    mail.From = new MailAddress(senderAddress);


                    //
                    // 메일 발송 테스트 계정 추후 제거 시작
                    //mail.To.Add(approver.MAIL_ADDRESS);
                    mail.To.Add("wookyung.kim@bayer.com,cheng.yin.ext@bayer.com");
                    //mail.To.Add("zest1116@dotnetsoft.co.kr");
                    //DearName = DearName + "(" + approver.MAIL_ADDRESS + ")";
                    // 메일 발송 테스트 계정 추후 제거 끝

                    mail.Subject = string.Format(format.Subject, documentName);
                    if (sendMailType.Equals("Reject"))
                        mail.Body = string.Format(format.Body, DearName, comment, url);
                    else if (sendMailType.Equals("Withdraw"))
                        mail.Body = string.Format(format.Body, comment, url);
                    else if (sendMailType.Equals("CurrentApprover"))
                    {
                        //만약 Rejected ProcessI가 존재하면 본문 하단에 description을 추가한다.

                        if (approver.REJECTED_PROCESS_ID.IsNullOrEmptyEx())
                            mail.Body = string.Format(format.Body, DearName, url, "");
                        else
                        {
                            string history = "History:<br/>This document was rejected with below comment by {0}<br/>Comment:{1}";
                            RejectDocumentMailCommentDto dto = GetRejecterInfo(processId);
                            history = string.Format(history, dto.REJECTER, dto.COMMENT);
                            mail.Body = string.Format(format.Body, DearName, url, history);
                        }
                    }
                    else if (sendMailType.Equals("InputComment"))
                    {
                        string inputComment = GetInputComment(processId);
                        mail.Body = string.Format(format.Body, DearName, inputComment, url);
                    }
                    else if (sendMailType.Equals("FinalApproval"))
                    {
                        if (approver.APPROVAL_TYPE.Equals("D") || approver.APPROVAL_TYPE.Equals("A")) //Requester Or Approver
                            mail.Body = string.Format(format.Body, DearName, url);
                        else //Recipient Or Reviewer
                        {
                            MailFormat recipientFormat = GetMailFormat("FinalApprovalRecipient");
                            mail.Body = string.Format(recipientFormat.Body, DearName, url);
                        }
                    }
                    else
                    {
                        mail.Body = string.Format(format.Body, DearName, url);
                    }
                    mail.IsBodyHtml = true;

                    //if (documentid.Equals("D0001") && sendMailType.Equals("FinalApproval") && approver.APPROVAL_TYPE.Equals("D")) //출장보고서 이면서
                    if (documentid.Equals("D0001") && sendMailType.Equals("FinalApproval") && approver.APPROVAL_TYPE.Equals("D"))
                    {
                        MemoryStream ms = CreateICalendar(processId);
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain);
                        Attachment attach = new Attachment(ms, ct);
                        attach.ContentDisposition.FileName = string.Format("Travel_{0}.ics", approver.APPROVER_ID);
                        mail.Attachments.Add(attach);
                    }
                    SmtpServer.Send(mail);
                    //email 수신 확인 log 추가
                    Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), string.Format("{0}:{1}->{2}", processId, senderAddress, approver.MAIL_ADDRESS), string.Empty);
                }

                
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), senderAddress, string.Empty);
                //Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), string.Format("{0}:{1}",processId, senderAddress), string.Empty);
            }
            catch (Exception ex)
            {
                retValue = ex.Message;
                WriteLog("error(smtp)" + ex.Message);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }

            return retValue;
        }

        /// <summary>
        /// iCalendar 파일 생성
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        private MemoryStream CreateICalendar(string processId)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                DTO_DOC_TRAVEL_MANAGEMENT_CALENDAR calendar = null;
                using (Bayer.eWF.BSL.Approval.Mgr.TravelManagementMgr mgr = new eWF.BSL.Approval.Mgr.TravelManagementMgr())
                {
                    calendar = mgr.SelectTravelManagementCalendar(processId);
                }

                if (calendar == null) return ms;
                DateTime startDate = calendar.START_DATE;
                DateTime endDate = calendar.END_DATE.AddDays(1);
                string organizer = calendar.REQUESTER_ADDRESS;
                string location = calendar.LOCATION;
                string summary = calendar.SUMMARY;
                string description = calendar.DESCRIPTION;

                string dateFormat = "yyyyMMddTHHmmssZ";
                string allDateFormat = "yyyyMMdd";


                StringBuilder sb = new StringBuilder();

                sb.AppendLine("BEGIN:VCALENDAR");
                sb.AppendLine("VERSION:2.0");
                sb.AppendLine("METHOD:PUBLISH");
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine("ORGANIZER:MAILTO:" + organizer);
                //sb.AppendLine("DTSTART:" + startDate.ToUniversalTime().ToString(dateFormat));
                //sb.AppendLine("DTEND:" + endDate.ToUniversalTime().ToString(dateFormat));
                sb.AppendLine("DTSTART;VALUE=DATE:" + startDate.ToString(allDateFormat));
                sb.AppendLine("DTEND;VALUE=DATE:" + endDate.ToString(allDateFormat));
                sb.AppendLine("LOCATION:" + location);
                sb.AppendLine("UID:" + DateTime.Now.ToUniversalTime().ToString(dateFormat) + "-" + calendar.REQUESTER_ADDRESS);
                sb.AppendLine("DTSTAMP:" + DateTime.Now.ToUniversalTime().ToString(dateFormat));
                sb.AppendLine("SUMMARY:" + summary);
                sb.AppendLine("DESCRIPTION:" + description);
                sb.AppendLine("PRIORITY:5");
                sb.AppendLine("CLASS:PUBLIC");
                sb.AppendLine("END:VEVENT");
                sb.AppendLine("END:VCALENDAR");
                byte[] contentAsBytes = Encoding.UTF8.GetBytes(sb.ToString());
                ms.Write(contentAsBytes, 0, contentAsBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region GetMailData
        private MailFormat GetMailFormat(string sendMailType)
        {
            System.Xml.XmlDocument doc;
            System.Xml.XmlElement root;
            System.Xml.XmlNode n;
            string documentUri = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/DocumentUrl");
            string serviceUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");
            MailFormat format = new MailFormat();
            try
            {
                doc = new System.Xml.XmlDocument();

                doc.Load(serviceUrl + "/MailFormat.xml");
                root = doc.DocumentElement;
                n = root.SelectSingleNode(string.Format("//Items/Item[@MailSendType='{0}']", sendMailType));

                format.Subject = n.SelectSingleNode("Subject").InnerText;
                format.Body = n.SelectSingleNode("Body").InnerText;
                format.DocumentUrl = @"{0}/{1}?processid={2}&documentid={3}";
                return format;
            }
            catch
            {
                throw;
            }
        }

        private MailItem GetMailData(string processid, string sendMailType)
        {

            string documentid = string.Empty;
            string documentName = string.Empty;
            string formName = string.Empty;
            string comment = string.Empty;
            string DearName = string.Empty;
            System.Xml.XmlDocument doc;
            System.Xml.XmlElement root;
            System.Xml.XmlNode n;
            string documentUri = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/DocumentUrl");
            string serviceUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");

            MailItem item = new MailItem();
            try
            {
                item.ApproverList = string.Empty;
                List<DTO_SENDMAIL_TO_ADDRESS_LIST> list = GetTargetMailAddress(processid, sendMailType);

                item.TOAddress = new List<string>();
                foreach (DTO_SENDMAIL_TO_ADDRESS_LIST i in list)
                {
                    item.TOAddress.Add(i.MAIL_ADDRESS);

                    if (item.ApproverList.Equals(string.Empty))
                        item.ApproverList = i.APPROVER_ID;
                    else
                        item.ApproverList = item.ApproverList + "," + i.APPROVER_ID;

                    if (documentid.Length == 0) documentid = i.DOCUMENT_ID;
                    if (documentName.Length == 0) documentName = i.DOC_NAME;
                    if (formName.Length == 0) formName = i.FORM_NAME;
                    if (comment.Length == 0) comment = i.COMMENT;
                    if (DearName.Length == 0) DearName = i.APPROVER_NAME;
                    else DearName = DearName + ", " + i.APPROVER_NAME;
                }

                doc = new System.Xml.XmlDocument();

                //doc.Load(HttpContext.Current.Server.MapPath("/MailFormat.xml"));
                doc.Load(serviceUrl + "/MailFormat.xml");

                root = doc.DocumentElement;
                n = root.SelectSingleNode(string.Format("//Items/Item[@MailSendType='{0}']", sendMailType));
                item.Subject = string.Format(n.SelectSingleNode("Subject").InnerText, documentName);

                string url = string.Format(@"{0}/{1}?processid={2}&documentid={3}", documentUri, formName, processid, documentid);

                if (sendMailType.Equals("Reject"))
                    item.Body = string.Format(n.SelectSingleNode("Body").InnerText, DearName, comment, url);
                else if (sendMailType.Equals("Withdraw"))
                    item.Body = string.Format(n.SelectSingleNode("Body").InnerText, comment, url);
                else
                    item.Body = string.Format(n.SelectSingleNode("Body").InnerText, DearName, url);

                /*
                sbScript.Append("<script type='text/javascript'> ");
                sbScript.Append("function fn_ShowDocument() {");
                sbScript.Append("    var nWidth = 935;");
                sbScript.Append("    var nHeight = 680;");
                sbScript.Append("    var left = (screen.width / 2) - (nWidth / 2);");
                sbScript.Append("    var top = (screen.height / 2) - (nHeight / 2) - 10; ");
                sbScript.AppendFormat(" var url = '{0}';",url);
                sbScript.Append("    window.open(url, '', 'width=' + nWidth + 'px, height=' + nHeight + 'px, top=' + top + 'px, left=' + left + 'px,location=no,titlebar=no,status=no,scrollbars=yes,menubar=no,toolbar=no,directories=no,resizable=no,copyhistory=no');");
                sbScript.Append("}");
                sbScript.Append("</script> ");

                item.Body += sbScript.ToString();
                    **/

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return item;
        }
        #endregion

        #region GetTargetMailAddress
        private List<DTO_SENDMAIL_TO_ADDRESS_LIST> GetTargetMailAddress(string processid, string sendMailType)
        {
            List<DTO_SENDMAIL_TO_ADDRESS_LIST> l;
            //using (var imp = new Impersonation(domainName, userName, password))
            //{


            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new eWF.BSL.Approval.Mgr.CommonMgr())
                {
                    //WriteLog(WindowsIdentity.GetCurrent().Name);
                    l = mgr.SelectSendMailTargetList(processid, sendMailType);
            }
            
          
            return l;
          
            
            //}

        }

        // ReturnGood Approver MailAddress List
        private List<DTO_SENDMAIL_TO_ADDRESS_LIST> GetTargetMailAddressApprover(string searchdata)
        {
            List<DTO_SENDMAIL_TO_ADDRESS_LIST> l;

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new eWF.BSL.Approval.Mgr.CommonMgr())
            {
                l = mgr.SelectSendMailRetrunGoodList(searchdata);
            }
            return l;
        }
        #endregion


        #region GetTargetMailAddress
        private string SendSmtpMailByService(MailFormat format, string sendMailType, List<DTO_SENDMAIL_TO_ADDRESS_LIST> list, string senderAddress)
        {
            StringBuilder log = new StringBuilder();
            string retValue = string.Empty;
            string documentid = string.Empty;
            string documentName = string.Empty;
            string formName = string.Empty;
            string comment = string.Empty;
            string DearName = string.Empty;
            string processId = string.Empty;
            string url = string.Empty;
            string ReturnGoodsUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/ReturnGoodsUrl");
            string documentUri = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/DocumentUrl");
            string sender = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/Sender");
           
            try
            {
                SmtpClient SmtpServer = SmtpManager.CreateSmtpClientObj();
                MailMessage mail = null;

                log.AppendFormat("SendMailType : {0} / ", sendMailType);

                foreach (DTO_SENDMAIL_TO_ADDRESS_LIST approver in list)
                {
                    if (approver.MAIL_ADDRESS.IsNullOrEmptyEx()) continue;
                    processId = approver.PROCESS_ID;
                    documentid = approver.DOCUMENT_ID;
                    documentName = approver.DOC_NAME;
                    formName = approver.FORM_NAME;
                    comment = approver.COMMENT;
                    DearName = approver.APPROVER_NAME;
                    

                    mail = new MailMessage();
                    mail.From = new MailAddress(senderAddress);
                    
                    log.AppendFormat("[sender : {0} / to : {1}]", senderAddress, approver.MAIL_ADDRESS);


                    // 메일 발송 테스트 계정 추후 제거 시작
                    //mail.To.Add(approver.MAIL_ADDRESS);
                    mail.To.Add("wookyung.kim@bayer.com");
                    //mail.To.Add("cypher@dotnetsoft.co.kr");
                    //mail.To.Add("leyou88@dotnetsoft.co.kr");
                    // DearName = DearName + "(" + approver.MAIL_ADDRESS + ")";
                    // 메일 발송 테스트 계정 추후 제거 끝

                    mail.Subject = string.Format(format.Subject, documentName);
                    mail.IsBodyHtml = true;

                    if (sendMailType.Equals("ReturnGoods"))
                    {                       
                        url = ReturnGoodsUrl;
                        mail.Body = string.Format(format.Body, DearName, url);
                    }
                    else
                    {
                        string requester = approver.SENDER_NAME;
                        string requestdate = approver.REQUEST_DATE;
                        url = string.Format(@"{0}/{1}?processid={2}&documentid={3}", documentUri, formName, processId, documentid);
                        mail.Body = string.Format(format.Body, DearName, url, requester, documentName, requestdate);
                    }

                    SmtpServer.Send(mail);
                    if (!sendMailType.Equals("ReturnGoods")) UpdateApproverSentMailStatus(processId, approver.APPROVER_ID);
                }


                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), log.ToString(), string.Empty);
            }
            catch (Exception ex)
            {
                retValue = ex.Message;
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }

            return retValue;
        }

        private List<DTO_SENDMAIL_TO_ADDRESS_LIST> GetNoticeMailList(DateTime dasicdate)
        {
            List<DTO_SENDMAIL_TO_ADDRESS_LIST> result;

            using (Bayer.eWF.BSL.Approval.Mgr.CommonMgr mgr = new eWF.BSL.Approval.Mgr.CommonMgr())
            {
                result = mgr.GetNoticeMailList(dasicdate);
            }
            return result;
        }
        #endregion

        #region [ 출장보고서 ]

        public MailItem GetAgencyMailData(string processID, string mailSendType)
        {

            System.Xml.XmlDocument doc;
            System.Xml.XmlElement root;
            System.Xml.XmlNode node;
            string serviceUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");
            MailItem item = new MailItem();
            try
            {
                doc = new System.Xml.XmlDocument();
                doc.Load(serviceUrl + "/MailFormat.xml");
                root = doc.DocumentElement;
                node = root.SelectSingleNode(string.Format("//Items/Item[@MailSendType='{0}']", mailSendType));

                using (Bayer.eWF.BSL.Approval.Mgr.TravelManagementMgr mgr = new eWF.BSL.Approval.Mgr.TravelManagementMgr())
                {
                    DTO_DOC_TRAVEL_MANAGEMENT travel = mgr.SelectTravelManagement(processID);
                    string purpose = travel.TRIP_PURPOSE;
                    string periodFrom = travel.TRIP_PERIOD_FROM.HasValue ? travel.TRIP_PERIOD_FROM.Value.ToString("yyyy-MM-dd") : string.Empty;
                    string periodTo = travel.TRIP_PERIOD_TO.HasValue ? travel.TRIP_PERIOD_TO.Value.ToString("yyyy-MM-dd") : string.Empty;
                    string period = periodFrom + "~" + periodTo;
                    string detail = travel.TRIP_INFO;
                    string comment = travel.COMMENT_TO_AGENCY;
                    string Contact_point = travel.TRIP_CONTACT_POINT;

                    List<DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER> travelers = mgr.SelectTravelManagementTraveler(processID);
                    var employees = (from employee in travelers
                                     where employee.TRAVELER_TYPE == "I"
                                     select employee).ToList();

                    StringBuilder sbTravelers = new StringBuilder();
                    sbTravelers.Append("<table style='position:relative; width:100%; border:1px solid #cfcfcf; border-collapse:collapse; margin-bottom:0px; margin-top:0;'>");
                    sbTravelers.Append("<colgroup>");
                    sbTravelers.Append("<col style='width: 25%;' />");
                    sbTravelers.Append("  <col />");
                    sbTravelers.Append("<col style='width: 25%;' />");
                    sbTravelers.Append("<col style='width: 25%;' />");
                    sbTravelers.Append("</colgroup>");
                    sbTravelers.Append("<tr><th class='tbl_title'>Name</th><th class='tbl_title'>EmpNo.</th>");
                    sbTravelers.Append("    <th class='tbl_title'>Div/Dept</th><th class='tbl_title'>Cost Center</th></tr>");
                    string empFormat = "<tr><td class='tbl_content'>{0}</td><td class='tbl_content'>{1}</td>"
                                        + " <td class='tbl_content'>{2}</td><td class='tbl_content'>{3}</td></tr>";
                    foreach (DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER e in employees)
                    {
                        sbTravelers.Append(string.Format(empFormat, e.TRAVELER_NAME + "(" + e.SEX + ")", e.EMP_NO, e.DIV_DEPT, e.COST_CODE));
                    }
                    sbTravelers.Append("</table>");

                    var externals = (from external in travelers
                                     where external.TRAVELER_TYPE == "E"
                                     select external).ToList();
                    if (externals.Count > 0)
                    {
                        sbTravelers.Append("<table>");
                        sbTravelers.Append("<colgroup>");
                        sbTravelers.Append("<col style='width: 25%;' />");
                        sbTravelers.Append("  <col />");
                        sbTravelers.Append("<col style='width: 25%;' />");
                        sbTravelers.Append("<col style='width: 25%;' />");
                        sbTravelers.Append("</colgroup>");
                        sbTravelers.Append("<tr><th class='tbl_title'>Guest</th><th class='tbl_title'>Company Org.</th>");
                        sbTravelers.Append("    <th class='tbl_title'>Cost Center</th><th class='tbl_title'>Internal Order</th></tr>");
                        string extFormat = "<tr><td class='tbl_content'>{0}</td><td class='tbl_content'>{1}</td>"
                                            + " <td class='tbl_content'>{2}</td><td class='tbl_content'>{3}</td></tr>";
                        foreach (DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER ext in externals)
                        {
                            sbTravelers.Append(string.Format(extFormat, ext.TRAVELER_NAME + "(" + ext.SEX + ")", ext.COMPANY_ORG, ext.COST_CODE, ext.INTERNAL_ORDER));
                        }
                        sbTravelers.Append("</table>");
                    }

                    List<DTO_DOC_TRAVEL_MANAGEMENT_ROUTE> routes = mgr.SelectTravelManagementRoute(processID);

                    StringBuilder sbRoute = new StringBuilder();
                    sbRoute.Append("<table>");
                    sbRoute.Append("<colgroup>");
                    sbRoute.Append("<col style='width: 10%;' />");
                    sbRoute.Append("<col style='width: 10%;' />");
                    sbRoute.Append("<col style='width: 20%;' />");
                    sbRoute.Append("<col style='width: 20%;' />");
                    sbRoute.Append("<col style='width: 20%;' />");
                    sbRoute.Append("<col style='width: 20%;' />");
                    sbRoute.Append("</colgroup>");
                    sbRoute.Append("<tr><th class='tbl_title'>Departure Dt.</th><th class='tbl_title'>Return Dt.</th>");
                    sbRoute.Append("    <th class='tbl_title'>Departure</th><th class='tbl_title'>Arrival</th>");
                    sbRoute.Append("    <th class='tbl_title'>Trip Type</th><th class='tbl_title'>Class</th></tr>");
                    string routeFormat = "<tr><td class='tbl_content'>{0}</td><td class='tbl_content'>{1}</td>"
                                         + "  <td class='tbl_content'>{2}</td><td class='tbl_content'>{3}</td>"
                                         + "  <td class='tbl_content'>{4}</td><td class='tbl_content'>{5}</td></tr>";
                    foreach (DTO_DOC_TRAVEL_MANAGEMENT_ROUTE route in routes)
                    {
                        string datDep = route.DEPARTURE_DATE.HasValue ? route.DEPARTURE_DATE.Value.ToString("yyyy-MM-dd") : string.Empty;
                        string datRet = route.RETURN_DATE.HasValue ? route.RETURN_DATE.Value.ToString("yyyy-MM-dd") : string.Empty;
                        sbRoute.Append(string.Format(routeFormat, datDep, datRet, route.DEPARTURE_CODE, route.ARRIVAL_CODE, route.TRIP_TYPE_NAME, route.AIRPLANE_CLASS_NAME));
                    }
                    sbRoute.Append("</table>");
                    StringBuilder sbQuotation = new StringBuilder();
                    if (mailSendType.Equals("CompletedTravelManagement") || mailSendType.Equals("CancelTravelManagement"))
                    {
                        sbQuotation.Append("<table>");
                        sbQuotation.Append("<colgroup>");
                        sbQuotation.Append("<col style='width: 25%;' />");
                        sbQuotation.Append("<col />");
                        sbQuotation.Append("</colgroup>");
                        string quotationFormation = "<tr><th class='tbl_title'>Quotaion No.</th>"
                                                   + "    <td class='tbl_content'>{0}</td></tr>"
                                                   + "<tr><th class='tbl_title'>Reason</th>"
                                                   + "    <td class='tbl_content'>{1}</td></tr>";
                        sbQuotation.Append(string.Format(quotationFormation, travel.QUOTATION_NUM, travel.REASON_NAME));
                        sbQuotation.Append("</table>");

                    }
                    StringBuilder sbCss = new StringBuilder();
                    sbCss.Append("<style type='text/css'>");
                    sbCss.Append("html, body, h1, h2, h3, h4, h5, h6, table");
                    sbCss.Append("{font-family:Arial, Helvetica, '맑은 고딕', sans-serif; font-size:12px;  margin: 0; padding: 0; line-height: inherit; color:#555; }");
                    //sbCss.Append("h2 { text-align:left; font-size:24px; background-color:dodgerblue; color:#fff; font-weight:bold; padding-top:5px; }");
                    sbCss.Append("h3{");
                    sbCss.Append("position:relative; background-color:#d8ecc3; border-top:1px solid #81c23d;  border-left:1px solid #81c23d; border-right:1px solid #81c23d;");
                    sbCss.Append("height:30px;  text-align:left; font-size:14px;  color:#446324; font-weight:bold; padding:7px 0 0 10px; margin-bottom:0; ");
                    sbCss.Append("}");
                    sbCss.Append("table	{ position:relative; width:100%; border:1px solid #cfcfcf; border-collapse:collapse; margin-bottom:20px; margin-top:0;}");
                    sbCss.Append(@".tbl_title { border:1px solid #cfcfcf; padding:5px 10px 5px 10px;  text-align:center; background-color:#e7e7e7;}");
                    sbCss.Append(@".tbl_content { border:1px solid #cfcfcf; padding:7px 10px 5px 10px;  text-align:left;}");
                    sbCss.Append("</style>");

                    if (mailSendType.Equals("CompletedTravelManagement") || mailSendType.Equals("CancelTravelManagement"))
                        item.Body = string.Format(@node.SelectSingleNode("Body").InnerText, sbCss.ToString(), sbTravelers.ToString(), purpose, period, detail, sbRoute.ToString(), sbQuotation.ToString(), Contact_point);
                    else
                        item.Body = string.Format(@node.SelectSingleNode("Body").InnerText, sbCss.ToString(), comment, sbTravelers.ToString(), purpose, period, detail, sbRoute.ToString(), Contact_point);
                    item.From = @node.SelectSingleNode("From").InnerText;
                    item.To = @node.SelectSingleNode("To").InnerText;
                    item.Cc = travel.REQUESTER_MAIL;
                    item.Bcc = @node.SelectSingleNode("Bcc").InnerText;
                    item.Subject = @node.SelectSingleNode("Subject").InnerText;

                }
            }
            catch
            {
                throw;
            }
            return item;
        }

        private bool SendAgencyMail(string processID)
        {
            bool isSuccess = true;
            string sender = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/Sender");
            try
            {
                SmtpClient SmtpServer = SmtpManager.CreateSmtpClientObj();

                MailMessage mail = new MailMessage();
                MailItem item = GetAgencyMailData(processID, "RequestToAgency");

                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), item.From, string.Empty);
                mail.From = new MailAddress(item.From);
                /* 테스트 이후 주석 풀어야 함. */
                if (!item.To.IsNullOrEmptyEx()) mail.To.Add(item.To);
                if (!item.Cc.IsNullOrEmptyEx()) mail.CC.Add(item.Cc);
                if (!item.Bcc.IsNullOrEmptyEx()) mail.Bcc.Add(item.Bcc);
                /* */
                // 메일 발송 테스트 계정 추후 제거 시작
                //mail.To.Add("zest1116@dotnetsoft.co.kr");
                //mail.CC.Add(item.Cc);
                // 메일 발송 테스트 계정 추후 제거 끝
                mail.Subject = item.Subject;
                mail.IsBodyHtml = true;
                string path = HttpRuntime.AppDomainAppPath;
                AlternateView view = AlternateView.CreateAlternateViewFromString(
                    item.Body, null, "text/html");
                LinkedResource jquery = new LinkedResource(path + @"\jquery-1.11.1.min.js", "text/javascript");
                jquery.ContentId = "jqueryid";
                jquery.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                LinkedResource popup = new LinkedResource(path + @"\popup.js", "text/javascript");
                popup.ContentId = "popupid";
                popup.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                view.LinkedResources.Add(jquery);
                view.LinkedResources.Add(popup);


                mail.AlternateViews.Add(view);

                SmtpServer.Send(mail);

                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), item.ApproverList, string.Empty);
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                isSuccess = false;
            }
            return isSuccess;
        }

        private void UpdateRequestToAgecyStatus(string processID, string updaterID)
        {
            try
            {
                using (Bayer.eWF.BSL.Approval.Mgr.TravelManagementMgr mgr = new eWF.BSL.Approval.Mgr.TravelManagementMgr())
                {
                    mgr.UpdateTravelManagementRequestToAgency(processID, updaterID);
                }
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendToAgency/{processID}/{updaterID}"),]
        public string SendToAgency(string processID, string updaterID)
        {
            string returnValue = "success";
            try
            {
                bool isSuccess = SendAgencyMail(processID);

                if (isSuccess) UpdateRequestToAgecyStatus(processID, updaterID);
                else returnValue = "fail";
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                returnValue = ex.ToString();
            }
            return returnValue;
        }

        #endregion
    }
}
