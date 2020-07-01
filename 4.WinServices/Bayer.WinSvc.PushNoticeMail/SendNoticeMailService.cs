using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bayer.WinSvc.PushNoticeMail
{
    public partial class SendNoticeMailService : ServiceBase
    {
        public static string LogTrace = string.Empty;

        public SendNoticeMailService()
        {
            InitializeComponent();
        }

        private void timer1_Elapsed(object source, ElapsedEventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                TimeSpan tSpan = new TimeSpan(e.SignalTime.Hour, e.SignalTime.Minute, 00);
                TimeSpan optSpan = TimeSpan.Parse(ConfigurationManager.AppSettings["operatingtime"]);

                if (tSpan.Equals(optSpan))
                {
                    SendNoticeMail();
                }
                Common.WriteLog("----------------------------------------------");
                Common.WriteLog(String.Format(" >> timer1_Elapsed Time Span : {0} : {1}", tSpan, optSpan));
                Common.WriteLog("----------------------------------------------");
            }
            catch (Exception ex)
            {
                Common.WriteLog("timer1_Elapsed - 오류 발생!!!!!!");
                Common.WriteLog("----------------------------------------------");
                Common.WriteLog("ex - " + ex.ToString());
                Common.WriteLog("----------------------------------------------");
            }
            finally
            {
                this.timer1.Enabled = true;
            }
        }

        protected override void OnStart(string[] args)
        {
            ServiceStart();
        }

        protected override void OnStop()
        {
            TimeSpan tSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            this.timer1.Stop();

            Common.WriteLog("----------------------------------------------");
            Common.WriteLog(String.Format(" >> OnStop Time Span : {0}", tSpan));
            Common.WriteLog("----------------------------------------------");
        }

        #region ServiceStart Methord
        /// <summary>
        /// 서비스 시작
        /// </summary>
        public void ServiceStart()
        {
            try
            {
                TimeSpan tSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 00);
                TimeSpan optSpan = TimeSpan.Parse(ConfigurationManager.AppSettings["operatingtime"]);

                if (tSpan.Equals(optSpan))
                {
                    SendNoticeMail();
                }
                this.timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
                this.timer1.Start();
                Common.WriteLog("----------------------------------------------");
                Common.WriteLog(String.Format(" >> OnStart Time Span : {0}", tSpan));
                Common.WriteLog("----------------------------------------------");
            }
            catch (Exception ex)
            {
                Common.WriteLog("ServiseStart - 오류 발생!!!!!!");
                Common.WriteLog("----------------------------------------------");
                Common.WriteLog("ex - " + ex.ToString());
                Common.WriteLog("----------------------------------------------");
            }
        }
        #endregion

        #region SendNoticeMail
        /// <summary>
        /// 장기간 미 결재자에 메일 발송
        /// </summary>
        public void SendNoticeMail()
        {
            try
            {
                Common.WriteLog("");
                Common.WriteLog("SendNoticeMail - 기준 날짜 계산....");
                SendNoticeMailService.LogTrace = "SendNoticeMail.GetBasicDate";
                double dbasedate = Convert.ToDouble(ConfigurationManager.AppSettings["basicdate"]) * -1;

                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                        dbasedate = dbasedate - 2;

                    string strBasicDate = DateTime.Now.AddDays(dbasedate).ToString("yyyy-MM-dd");

                    Common.WriteLog("SendNoticeMail - 웹서비스 실행..../BasicDate = " + strBasicDate);
                    SendNoticeMailService.LogTrace = "SendNoticeMail.GetNoticeMailList";

                    SendNoticeMail(strBasicDate);

                    Common.WriteLog("SendNoticeMail - 장기간 미 결재자 목록 완료..../DayOfWeek = " + DateTime.Now.DayOfWeek.ToString());
                }
                else
                {
                    Common.WriteLog("SendNoticeMail - 휴일 발송 안함..../DayOfWeek = " + DateTime.Now.DayOfWeek.ToString());
                }
            }
            catch
            {
                throw;
            }
        }

        private void SendNoticeMail(string basicdate)
        {
            try
            {
                SendNoticeMailService.LogTrace = "SendNoticeMail";

                string sendMailType = "DelayApproval";
                string sender = "KR_Workflow@bayer.com";
                string wcfUrl = ConfigurationManager.AppSettings["WCFServiceUrl"];
                string serviceUrl = string.Format("{0}/MailServices.svc/SendNoticeMail/{1}/{2}/{3}", wcfUrl, sendMailType, basicdate, sender);
                Common.WriteLog(string.Format("SendNoticeMail - 발송항목 정보 = basicdate : {0} / url : {1}", basicdate, serviceUrl));

                SendNoticeMailService.LogTrace = "SendNoticeMail.WcfSvc";
                HttpWebRequest request = WebRequest.Create(serviceUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.WriteLog("SendNoticeMail.WcfSvc - 메일 발송 완료....");
                    }
                    else
                    {
                        Common.WriteLog("SendNoticeMail.WcfSvc - 메일 발송 실패!!!!");
                        Common.WriteLog("----------------------------------------------");
                        Common.WriteLog("ex - " + String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                        Common.WriteLog("LogTrace - " + SendNoticeMailService.LogTrace);
                        Common.WriteLog("----------------------------------------------");
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
