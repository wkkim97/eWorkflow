using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Mail;
using DNSoft.eW.FrameWork;
using System.ServiceModel.Web;
using System.Web.Hosting;
using Bayer.eWF.BSL.Approval.Mgr;
using System.Reflection;
using Bayer.eWF.BSL.Approval.Dto;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;
using SAP.Middleware.Connector;
using System.Web;
using System.ServiceModel.Activation;

namespace Bayer.WCF.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AfterTreatment : IAfterTreatment
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "DoProcessing/{processID}"),]
        public void DoProcessing(string processID)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "MergeMembershipApplication/{processID}"),]
        public void MergeMembershipApplication(string processID)
        {
            try
            {
                using (AfterTreatmentMgr mgr = new AfterTreatmentMgr())
                {
                    mgr.MergeMembershipApplication(processID);
                }
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
        }

        #region [ Debit & Credit Note ]

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendDebitCreditNoteAttendees/{processID}"),]
        public void SendDebitCreditNoteAttendees(string processID)
        {
            System.Xml.XmlDocument doc;
            System.Xml.XmlElement root;
            System.Xml.XmlNode node;
            string documentUri = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/DocumentUrl");
            string serviceUrl = DNSoft.eW.FrameWork.eWBase.GetConfig("//WCFServices/HostURL");
            try
            {
                doc = new System.Xml.XmlDocument();
                doc.Load(serviceUrl + "/MailFormat.xml");
                root = doc.DocumentElement;
                node = root.SelectSingleNode(string.Format("//Items/Item[@MailSendType='{0}']", "DebitAndCreditNode"));
                MailMessage mail = new MailMessage();
                using (CreditDebitNoteMgr mgr = new CreditDebitNoteMgr())
                {
                    DTO_DOC_CREDIT_DEBIT_NOTE document = mgr.SelectCreditDebitNote(processID);
                    string category = document.TYPE;
                    category = char.ToUpper(category[0]) + category.Substring(1).ToLower();
                    string companyId = document.COMPANY_ID;
                    string companyName = "";
                    string docNumber = document.DOC_NUM;

                    string from = node.SelectSingleNode("Sender").InnerText;
                    mail.From = new MailAddress(from);

                    mail.Subject = string.Format(node.SelectSingleNode("Subject").InnerText, category, companyId, companyName, docNumber);
                    bool isExternal = false;
                    foreach (DTO_DOC_CREDIT_DEBIT_NOTE_ATTN attn in mgr.SelectCreditDebitNoteAttn(processID))
                    {
                        if (!attn.ATTN_MAIL_ADDRESS.Contains("@bayer.com"))
                        {
                            isExternal = true;
                            continue;
                        }
                        mail.To.Add(attn.ATTN_MAIL_ADDRESS);
                    }
                    foreach (DTO_DOC_CREDIT_DEBIT_NOTE_CC cc in mgr.SelectCreditDebitNoteCc(processID))
                    {
                        if (!cc.CC_MAIL_ADDRESS.Contains("@bayer.com")) continue;
                        mail.CC.Add(cc.CC_MAIL_ADDRESS);
                    }
                    if (isExternal)
                    {
                        //List<DTO_PROCESS_APPROVAL_LIST> recipients = mgr.SelectRecipientList(processID);

                        //foreach (DTO_PROCESS_APPROVAL_LIST recipient in recipients)
                        //{
                        //    if (recipient.MAIL_ADDRESS.IsNullOrEmptyEx()) continue;
                        //    mail.CC.Add(recipient.MAIL_ADDRESS);
                        //}
                        if(document.REQUESTER_MAIL.IsNotNullOrEmptyEx()) mail.To.Add(document.REQUESTER_MAIL);
                    }else{
                        if (document.REQUESTER_MAIL.IsNotNullOrEmptyEx()) mail.CC.Add(document.REQUESTER_MAIL);
                    }
                    documentUri = documentUri.Replace("Document", "Link");
                    string link = documentUri + "/CreditDebitNoteView.aspx?processid={0}";
                    link = string.Format(link, processID);
                    string body = string.Format(node.SelectSingleNode("Body").InnerText, category, link);
                    mail.IsBodyHtml = true;
                    mail.Body = body;
                }
                //첨부파일
                using (FileMgr mgr = new FileMgr())
                {
                    List<DTO_ATTACH_FILES> attachs = mgr.SelectAttachFileList(processID, "Common");

                    foreach (DTO_ATTACH_FILES file in attachs)
                    {
                        mail.Attachments.Add(new Attachment(file.FILE_PATH));
                    }
                }

                SmtpClient client = SmtpManager.CreateSmtpClientObj();
                client.Send(mail);
                string logMessage = string.Format("ProcessID={0}", processID);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), logMessage, string.Empty);

            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
        }

        #endregion

        #region [ Calleteral Management ]

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "CalculateCreditLimit/{customerCode}/{bgCode}/{statusCode}/{inputAmount}"),]
        public CreditLimit CalculateCreditLimit(string customerCode, string bgCode, string statusCode, string inputAmount)
        {
            string result = "success";
            decimal amount = 0;
            try
            {
                using (MortgageManagementMgr mgr = new MortgageManagementMgr())
                {
                    decimal amt = Convert.ToDecimal(inputAmount);
                    amount = mgr.SelectNewCreditLimit(customerCode, bgCode, statusCode, amt);
                }
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                result = ex.ToString();
            }
            return new CreditLimit(result, amount);
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateCreditLimit/{processID}/{customerCode}/{bgCode}/{statusCode}/{updater}/{amount}/{currentlyId}"),]
        public string UpdateCreditLimit(string processID, string customerCode, string bgCode, string statusCode, string updater, string amount, string currentlyId)
        {
            string result = "success";
            try
            {
                //SAP 연동 추가
                string strSapAmount = SelectSapCreditLimit(customerCode, bgCode);
                if (strSapAmount.Equals("fail")) return result = strSapAmount;

                decimal sapAmount = Convert.ToDecimal(strSapAmount);
                decimal calAmount = Convert.ToDecimal(amount);

                if (sapAmount == calAmount)
                {
                    using (MortgageManagementMgr mgr = new MortgageManagementMgr())
                    {
                        decimal amt = Convert.ToDecimal(amount);
                        mgr.UpdateCreditLimit(processID, customerCode, bgCode, statusCode, updater, amt, currentlyId);
                    }
                }
                else
                {
                    result = "different";
                }
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                result = ex.ToString();
            }
            return result;
        }

        public string SelectSapCreditLimit(string customerCode,string bgcode)
        {
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination("HCP");
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            IRfcFunction BapiGetCompanyList = SapRfcRepository.CreateFunction("RFC_READ_TABLE");

            BapiGetCompanyList.SetValue("QUERY_TABLE", "KNKK");
            IRfcTable fieldtbl = BapiGetCompanyList.GetTable("FIELDS");
            fieldtbl.Append();
            fieldtbl.SetValue(0, "KLIMK");

            IRfcTable optionstbl = BapiGetCompanyList.GetTable("OPTIONS");
            optionstbl.Append();
            //if (bgcode.Trim() == "16")
            //{
            //    optionstbl.SetValue(0, "KKBER EQ 'KO16'");
            //}
            //else
            //{
            //    optionstbl.SetValue(0, "KKBER EQ 'KO16'");
            //}
            optionstbl.SetValue(0, "KKBER EQ 'KOHC'");
            optionstbl.Append();
            optionstbl.SetValue(0, "AND KUNNR EQ '" + customerCode + "'");

            BapiGetCompanyList.Invoke(SapRfcDestination);
            IRfcTable datatbl = BapiGetCompanyList.GetTable("DATA");

            string text_t = datatbl.GetString("WA");
            decimal value = Convert.ToDecimal(text_t) * 100;
            return value.ToString();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetSapCreditLimit/{customerCode}/{BU}"),]
        public string GetSapCreditLimit(string customerCode,string BU)
        {
            try
            {
                return SelectSapCreditLimit(customerCode,BU );
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
            return "fail";
        }

        #endregion


        #region AFTER Free Goods
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdataFreeGoods/{SchemeProcessId}/{Idx}/{HowGrid}/{Status}/{userId}"),]
        public string UpdataFreeGoods(string SchemeProcessId, string Idx, string HowGrid, string Status, string userId)
        {
            string result = "success";
            try
            {
                using (AfterTreatmentMgr mgr = new AfterTreatmentMgr())
                {
                    mgr.UpdateFreeGoods(SchemeProcessId, Idx, HowGrid, Status, userId);
                }
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                result = ex.ToString();
            }
            return result;
        }
        #endregion


        #region [ TravelManagement ]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendToAgencyTravelManagement/{processID}"),]
        public void SendToAgencyTravelManagement(string processID)
        {
            SendToAgency(processID, "CompletedTravelManagement");
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "CancelToAgencyTravelManagement/{processID}"),]
        public string CancelToAgencyTravelManagement(string processID)
        {
            if (SendToAgency(processID, "CancelTravelManagement")) return "success";
            else return "fail";
        }

        private bool SendToAgency(string processID, string mailSendType)
        {
            string sender = DNSoft.eW.FrameWork.eWBase.GetConfig("//SmtpMailInfo/Sender");
            bool isSuccess = true;
            try
            {
                SmtpClient SmtpServer = SmtpManager.CreateSmtpClientObj();

                MailMessage mail = new MailMessage();
                MailingService mailing = new MailingService();
                MailItem item = mailing.GetAgencyMailData(processID, mailSendType);

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

                if (mailSendType.Equals("CompletedTravelManagement"))
                {
                    //첨부파일
                    using (FileMgr mgr = new FileMgr())
                    {
                        List<DTO_ATTACH_FILES> attachs = mgr.SelectAttachFileList(processID, "TravelQuotation");

                        foreach (DTO_ATTACH_FILES file in attachs)
                        {
                            mail.Attachments.Add(new Attachment(file.FILE_PATH));
                        }
                        attachs = mgr.SelectAttachFileList(processID, "TravelApplication");

                        foreach (DTO_ATTACH_FILES file in attachs)
                        {
                            mail.Attachments.Add(new Attachment(file.FILE_PATH));
                        }
                    }
                }
                SmtpServer.Send(mail);
                string logMessage = string.Format("ProcessID={0}/Requester={1}", processID, item.Cc);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), logMessage, string.Empty);
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion

        #region [ Business Card ]

        public MailItem GetBusinessCardMailData(string processID)
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
                node = root.SelectSingleNode(string.Format("//Items/Item[@MailSendType='{0}']", "BusinessCard"));
                using (Bayer.eWF.BSL.Approval.Mgr.BusinessCardMgr mgr = new eWF.BSL.Approval.Mgr.BusinessCardMgr())
                {

                    DTO_DOC_BUSINESS_CARD businessCard = mgr.SelectBusinessCard(processID);
                    StringBuilder sbBusinessCard = new StringBuilder();

                    sbBusinessCard.Append("<table style='position:relative; width:100%; border:1px solid #cfcfcf; border-collapse:collapse; margin-bottom:0px; margin-top:0;'>");
                    sbBusinessCard.Append("<colgroup>");
                    sbBusinessCard.Append("<col style='width: 25%;' />");
                    sbBusinessCard.Append("<col style='width: 35%;' />");
                    sbBusinessCard.Append("<col />");
                    sbBusinessCard.Append("</colgroup>");
                    sbBusinessCard.Append("<thead>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'></th>");
                    sbBusinessCard.Append("    <th class='tbl_title'>English</th>");
                    sbBusinessCard.Append("    <th class='tbl_title'>korean</th>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("</thead>");
                    sbBusinessCard.Append("<tbody>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Name</th>");
                    sbBusinessCard.Append("    <td class='tbl_content'>" + businessCard.ENG_NAME + "</td>");
                    sbBusinessCard.Append("    <td class='tbl_content'>" + businessCard.KOR_NAME + "</td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Title</th>");
                    sbBusinessCard.Append("    <td class='tbl_content'>" + businessCard.ENG_TITLE + "</td>");
                    sbBusinessCard.Append("    <td class='tbl_content'>" + businessCard.KOR_TITLE_NAME + "-"+businessCard.KOR_JOB_TITLE+ "</td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Organization</th>");
                    sbBusinessCard.Append("    <td class='tbl_content'><div style='margin-bottom: 3px;'>");
                    sbBusinessCard.Append("        <span style='display: inline-block; width: 65px; text-align: right'>Department : </span>");
                    sbBusinessCard.Append(businessCard.ENG_DEPARTMENT);
                    sbBusinessCard.Append("        </div>");
                    sbBusinessCard.Append("        <span style='display: inline-block; width: 65px; text-align: right'>Division : </span>");
                    sbBusinessCard.Append(businessCard.ENG_DIVISION_NAME);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("    <td class='tbl_content'><div style='margin-bottom: 3px;'>");
                    sbBusinessCard.Append("        <span style='display: inline-block; width: 65px; text-align: right'>Department : </span>");
                    sbBusinessCard.Append(businessCard.KOR_DEPARTMENT);
                    sbBusinessCard.Append("        </div>");
                    sbBusinessCard.Append("        <span style='display: inline-block; width: 65px; text-align: right'>Division : </span>");
                    sbBusinessCard.Append(businessCard.KOR_DIVISION_NAME);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Color</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.COLOR_CODE);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Tel.of office</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.TEL_OFFICE);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Mobile phone</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.MOBILE);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Fax number</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.FAX);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>E-mail address </th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.E_MAIL);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Address(Eng)</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.ADDRESS);
                    sbBusinessCard.Append("    </td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Quantity</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.QUANTITY.ToString());
                    sbBusinessCard.Append("</td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>Company</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.LEADING_SUBGROUP.ToString());
                    sbBusinessCard.Append("</td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("<tr><th class='tbl_title'>COstCenter</th>");
                    sbBusinessCard.Append("    <td class='tbl_content' colspan='2'>");
                    sbBusinessCard.Append(businessCard.COST_CENTER.ToString());
                    sbBusinessCard.Append("</td>");
                    sbBusinessCard.Append("</tr>");
                    sbBusinessCard.Append("</tbody>");
                    sbBusinessCard.Append("</table>");
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

                    item.Body = string.Format(@node.SelectSingleNode("Body").InnerText, sbCss.ToString(), sbBusinessCard.ToString());
                    item.From = @node.SelectSingleNode("From").InnerText;
                    item.To = @node.SelectSingleNode("To").InnerText;
                    item.Subject = @node.SelectSingleNode("Subject").InnerText;
                    string logMessage = string.Format("ProcessID={0}", processID);
                    Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), logMessage.ToString(), string.Empty);
                }
            }
            catch
            {
                throw;
            }
            return item;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendToAgencyBusinessCard/{processID}"),]
        public string SendToAgencyBusinessCard(string processID)
        {
            string rtnValue = "success";
            try
            {
                SmtpClient SmtpServer = SmtpManager.CreateSmtpClientObj();

                MailMessage mail = new MailMessage();
                MailItem item = GetBusinessCardMailData(processID);

                mail.From = new MailAddress(item.From);
                /* 테스트 이후 주석 풀어야 함. */
                if (!item.To.IsNullOrEmptyEx()) mail.To.Add(item.To);
                //Requester는 Cc에 Recipient는 Bcc에 추가
                string requester = string.Empty;
                using (BusinessCardMgr mgr = new BusinessCardMgr())
                {
                    List<SendToAgencyBusicessCardDto> list = mgr.SelectRequesterAndRecipient(processID);
                    foreach (SendToAgencyBusicessCardDto approver in list)
                    {
                        if (approver.ApproverType.Equals("D"))
                        {
                            requester = approver.EmailAddress;
                            if (!approver.EmailAddress.IsNullOrEmptyEx()) mail.CC.Add(approver.EmailAddress);
                        }
                        else if (approver.ApproverType.Equals("R"))
                        {
                            if (!approver.EmailAddress.IsNullOrEmptyEx()) mail.Bcc.Add(approver.EmailAddress);
                        }
                    }
                }
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

                mail.AlternateViews.Add(view);

                SmtpServer.Send(mail);
                string logMessage = string.Format("ProcessID={0}/Requester={1}", processID, requester);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), logMessage, string.Empty);
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
                rtnValue = "fail";
            }
            return rtnValue;
        }

        #endregion


        #region [ Collateral ]

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateCollateralStatus/{processID}"),]
        public void UpdateCollateralStatus(string processID)
        {
            try
            {
                using (AfterTreatmentMgr mgr = new AfterTreatmentMgr())
                {
                    mgr.UpdateCollateralStatus(processID);
                }
                string logMessage = string.Format("ProcessID={0}", processID);
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("Info", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), logMessage, string.Empty);
            }
            catch (Exception ex)
            {
                Bayer.eWF.BSL.Common.Mgr.SystemLogMgr.InsertSystemLog("error", string.Format("{0}.{1}", this.GetType().Name, MethodInfo.GetCurrentMethod().Name), ex.ToString(), string.Empty);
            }
        } 
        #endregion


       
    }
}
