using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Cryptography;

using DNSoft.eW.FrameWork;

namespace DNSoft.eWF.FrameWork.Web
{
    /// <summary>
    /// 랜더 타입(출력값)
    /// </summary>
    public enum emXMLOutputType
    {
        ControlRender = 0,
        StringRender
    }

    /// <summary>
    /// Aspx.cs파일에 공통적인 작업이 이루어지는 파생클래스
    /// </summary>
    [Serializable]
    public class PageBase : System.Web.UI.Page
    {
        #region 멤버변수
        /// <summary>
        /// 메뉴 teb 구분 Prefix (Nex1에서 사용)
        /// </summary>
        protected string menuInfo = string.Empty;
        /// <summary>
        /// 에러메시지
        /// </summary>
        protected string errorMessage = string.Empty;
        /// <summary>
        /// 정보메세지창
        /// </summary>
        protected string informationMessage = string.Empty;
        /// <summary>
        /// 확인메세지창
        /// </summary>
        protected string confirmMessage = string.Empty;
        /// <summary>
        /// 원도우닫기
        /// </summary>
        protected bool windowClose = false;
        /// <summary>
        /// 리턴값
        /// </summary>
        protected string returnValue = string.Empty;
        /// <summary>
        /// 세션클래스정의
        /// 2009.11.14(sylee) : AccountInfo추가, 
        /// </summary>
        //protected DNSoft.eW.FrameWork.Web.SessionClass Sessions = null;
        protected AccountInfo Sessions = null;
        /// <summary>
        /// 사용자정보 캐쉬
        /// </summary>
        //		public static Hashtable accountCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 서브팀상태값
        /// </summary>
        private bool bSubStatus = false; //서브팀
        /// <summary>
        /// 로깅레벨
        /// </summary>
        private string strLogLevel = string.Empty;

        protected bool setImgSrc = true;

        protected bool bXmlDomOutput = false;

        protected string strRenderFormID = string.Empty;

        protected string strRenderString = string.Empty;

        protected emXMLOutputType oXMLOutputType = emXMLOutputType.ControlRender;


        /// <summary>
        /// 특정 Page에서 사용되는 스크립트 문자열
        /// </summary>
        private StringBuilder _sbLocalscript = null;
        /// <summary>
        /// 특정 Page에서 사용되는 스크립트 문자열 입력하기
        /// </summary>
        /// <param name="script"></param>
        protected void SetLocalScript(string key, string value)
        {
            try
            {
                if (_sbLocalscript == null)
                {
                    _sbLocalscript = new StringBuilder();
                }

                this._sbLocalscript.Append("    var ");
                this._sbLocalscript.Append(key);
                this._sbLocalscript.Append(" = \"");
                this._sbLocalscript.Append(value);
                this._sbLocalscript.Append("\";\r\n");
            }
            catch
            {
                throw;
            }
        }


        #endregion

        #region 생성자
        /// <summary>
        /// 생성자 - 초기화
        /// </summary>
        public PageBase()
        {
            try
            {
                this.Title = DNSoft.eW.FrameWork.eWBase.GetConfig("//PageUI/Title");

                //생성자
                this.menuInfo = string.Empty;
                errorMessage = string.Empty;
                informationMessage = string.Empty;
                confirmMessage = string.Empty;
                windowClose = false;
                returnValue = string.Empty;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Session Class새로 추가된 부분(2009.11.14:sylee)
        /// <summary>
        /// Sessions 객체에 사용자 정보 데이터를 설정한다.
        /// </summary>
        /// <param name="account">사용자ID</param>
        /// <returns></returns>
        public AccountInfo GetAccountInfo(string account)
        {
            return GetAccountInfo(account, false);
        }


        /// <summary>
        /// Sessions 객체에 사용자 정보 데이터를 설정한다.
        /// </summary>
        /// <remarks>
        /// - 추가수정자 : DotNetSoft 류승태<br/>
        /// - 추가수정일 : 2009-03-17<br/>
        /// - 내      용 : KEPCO 메일환경설정 추가. stMailBasic 구조체에 Data를 가져온다<br/>
        /// - 추가수정자 : 닷넷소프트 김학수<br/>
        /// - 추가수정일 : 2011-09-27<br/>
        /// - 내      용 : 첫 화면 설정 추가<br/>
        /// </remarks>
        /// <param name="account"></param>
        /// <param name="isForce"></param>
        /// <returns></returns>
        public AccountInfo GetAccountInfo(string account, bool isForce)
        {
            AccountInfo user = null;
            DataSet dsTemp = null;
            DataSet dsConfig = null;

            DataRow drTemp = null;
            DataTable dtUser = null;

            DataTable oDtExchInfo = null;

            string strLanguage = string.Empty;
            string strADMailServer = string.Empty;
            string strMailBoxQuota = string.Empty;

            try
            {
                if (account == "")
                {
                    throw new ArgumentException("사용자 계정값이 제공되지 않았습니다", account);
                }

                if (!isForce && this.Session["UserSessionClass"] != null)
                {
                    user = (AccountInfo)this.Session["UserSessionClass"];
                }
                else
                {

                    if (strLanguage.Equals(string.Empty))
                    {
                        if (Request.Cookies["eWLanguage"] != null)
                        {
                            strLanguage = this.Request.Cookies["eWLanguage"].Value;
                        }
                        else
                        {
                            // 무조건 한글로 설정한다.
                            strLanguage = "ko-KR";


                        }
                    }

                    dsTemp = DNSoft.eW.FrameWork.eWDictionary.GetUserInfo(strLanguage, account);

                    if (dsTemp == null || dsTemp.Tables.Count < 1 || dsTemp.Tables[0].Rows.Count < 1)
                    {
                        // 하드코딩
                        throw new Exception("DataBase에 사용자(" + account + ")에 대한 정보가 없습니다.");
                    }

                    drTemp = dsTemp.Tables[0].Rows[0];

                    user = new AccountInfo();
                    user.UserID = drTemp["USER_ID"].ToString();
                    user.UserName = drTemp["FULL_NAME"].ToString();
                    user.CompanyCode = drTemp["COMPANY_CODE"].ToString();
                    user.CompanyName = drTemp["COMPANY_NAME"].ToString();
                    user.OrgName = drTemp["ORG_ACRONYM"].ToString();
                    user.JobTitle = drTemp["JOB_TITLE"].ToString();
                    user.MailAddress = drTemp["MAIL_ADDRESS"].ToString();
                    user.Mobile = drTemp["MOBILE"].ToString();
                    user.Phone = drTemp["PHONE"].ToString();
                    user.Title = drTemp["TITLE"].ToString();
                    user.LeadingSubGroup = drTemp["LEADING_SUBGROUP"].ToString(); 
                    int iDeptCount = dsTemp.Tables[0].Rows.Count;

                    user.ListCount = Convert.ToInt32(DNSoft.eW.FrameWork.eWBase.GetConfig("//UserConfigurationDefaultValue/ListCount", "20"));
                    user.PageCount = Convert.ToInt32(DNSoft.eW.FrameWork.eWBase.GetConfig("//UserConfigurationDefaultValue/PageCount", "10"));

                    user.Language = strLanguage;


                    dsTemp = DNSoft.eW.FrameWork.eWDictionary.GetUserConfig(account);

                    if (dsTemp == null || dsTemp.Tables.Count <= 1 || dsTemp.Tables[0].Rows.Count <= 1)
                    {
                        drTemp = dsTemp.Tables[0].Rows[0];

                        user.MainViewType = drTemp["MAIN_VIEWTYPE"].ToString();
                        user.MainFormUrl = drTemp["MAIN_FORM"].ToString();
                    }

                    dsTemp = DNSoft.eW.FrameWork.eWDictionary.GetUserRole(account);

                    user.IsSpecialUser = "N";

                    if (dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count >= 1)
                    {
                        foreach (DataRow row in dsTemp.Tables[0].Rows)
                        {
                            if (row["USER_ROLE"].ToString().Equals(ApprovalUtil.UserRole.Special))
                            {
                                user.IsSpecialUser = "Y";
                            }
                            user.UserRole = row["USER_ROLE"].ToString();
                        }
                    }
                    else
                    {
                        user.UserRole = ApprovalUtil.UserRole.None;
                    }

                    this.Session["UserSessionClass"] = user;


                }
            }
            catch
            {
                throw;
            }
            finally
            {

                if (dsTemp != null)
                {
                    dsTemp.Dispose();
                    dsTemp = null;
                }

                if (dsConfig != null)
                {
                    dsConfig.Dispose();
                    dsConfig = null;
                }

                if (dtUser != null)
                {
                    dtUser.Dispose();
                    dtUser = null;
                }
                if (oDtExchInfo != null)
                {
                    oDtExchInfo.Dispose();
                    oDtExchInfo = null;
                }
                if (drTemp != null)
                {
                    drTemp.Delete();
                    drTemp = null;
                }
            }
            return user;
        }
        #endregion

        #region 공통Dictionary
        /// <summary>
        /// Dictionary가져오기
        /// </summary>
        /// <param name="constantCode">Dictionary코드</param>
        /// <returns>해당항목</returns>
        protected string Dic(string constantCode)
        {
            string language = "ko-KR";
            if (this.Sessions != null)
                language = this.Sessions.Language;

            return DNSoft.eW.FrameWork.eWDictionary.Dic(language, constantCode);
        }

        /// <summary>
        /// 메세지아이지만으로 해당팀의 메시지 가져오기
        /// </summary>
        /// <param name="messagID">메시지아이디</param>
        protected void Msg(string messageID)
        {
            Msg(messageID, SubSystemType.Common);
        }

        /// <summary>
        /// 서브팀명과 메시지아이디로 메시지 가져오기
        /// </summary>
        /// <param name="subSystemType">서브시스템타입</param>
        /// <param name="messagID">메시지아이디</param>
        protected void Msg(string messageID, SubSystemType subSystemType)
        {
            string[] arrTemp = null;
            try
            {
                //메시지
                arrTemp = Msgs(subSystemType, messageID);
                MsgExecute(arrTemp);
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 문자열 포멧 형식으로 메시지를 만든다. 
        /// </summary>
        /// <param name="messageID">메시지아이디</param>
        /// <param name="subSystemType">서브시스템타입</param>
        /// <param name="msgStrings">{순번}에 들어갈 문자열</param>
        protected void Msg(string messageID, SubSystemType subSystemType, string[] msgStrings)
        {
            string[] arrTemp = null;
            StringBuilder sbMsgString = null;
            StringBuilder sbOldString = null;

            try
            {
                arrTemp = Msgs(subSystemType, messageID);

                if (arrTemp != null)
                {
                    sbMsgString = new StringBuilder(arrTemp[3]);
                    sbOldString = new StringBuilder();

                    for (int i = 0; i < msgStrings.Length; i++)
                    {
                        sbOldString.Append("{").Append(i.ToString()).Append("}");

                        sbMsgString.Replace(sbOldString.ToString(), msgStrings[i]);

                        sbOldString.Remove(0, sbOldString.Length);
                    }

                    arrTemp[3] = sbMsgString.ToString();
                }

                MsgExecute(arrTemp);
            }

            catch
            {
                throw;
            }

            finally
            {
                if (sbMsgString != null)
                {
                    if (sbMsgString.Length > 0)
                    {
                        sbMsgString.Remove(0, sbMsgString.Length);
                    }

                    sbMsgString = null;
                }

                if (sbOldString != null)
                {
                    if (sbOldString.Length > 0)
                    {
                        sbOldString.Remove(0, sbOldString.Length);
                    }

                    sbOldString = null;
                }
            }
        }

        /// <summary>
        /// 문자열 포멧 형식으로 메시지를 리턴한다.
        /// </summary>
        /// <param name="subSystemType">서브시스템명</param>
        /// <param name="messageID">메시지ID</param>
        /// <param name="msgStrings">{0}에 들어갈 파라메터값</param>
        /// <returns>메시지</returns>
        protected string[] Msgs(SubSystemType subSystemType, string messageID, string[] msgStrings)
        {
            string[] astrReturn = null;
            StringBuilder sbMsgString = null;
            StringBuilder sbOldString = null;

            try
            {
                astrReturn = DNSoft.eW.FrameWork.eWDictionary.GetMessage(this.Sessions.Language, subSystemType, messageID);

                if (astrReturn != null)
                {
                    sbMsgString = new StringBuilder(astrReturn[3]);
                    sbOldString = new StringBuilder();

                    for (int i = 0; i < msgStrings.Length; i++)
                    {
                        sbOldString.Append("{").Append(i.ToString()).Append("}");

                        sbMsgString.Replace(sbOldString.ToString(), msgStrings[i]);

                        sbOldString.Remove(0, sbOldString.Length);
                    }

                    astrReturn[3] = sbMsgString.ToString();
                }
            }

            catch
            {
                throw;
            }

            finally
            {
                if (sbMsgString != null)
                {
                    if (sbMsgString.Length > 0)
                    {
                        sbMsgString.Remove(0, sbMsgString.Length);
                    }

                    sbMsgString = null;
                }

                if (sbOldString != null)
                {
                    if (sbOldString.Length > 0)
                    {
                        sbOldString.Remove(0, sbOldString.Length);
                    }

                    sbOldString = null;
                }
            }

            return astrReturn;
        }

        /// <summary>
        /// 메시지 가져오기
        /// </summary>
        /// <remarks>
        /// 해당메시지아이디의 배열을 가져온다.
        /// 배열[0] SubSystemType
        /// 배열[1] MessageID
        /// 배열[2] MessageType
        /// 배열[3] DisplayMessage
        /// 배열[4] SummaryMessage
        /// </remarks>
        /// <param name="messageID">메시지아이디</param>
        /// <returns>메시지배열</returns>
        protected string[] Msgs(string messageID, string[] msgStrings)
        {
            return Msgs(SubSystemType.Common, messageID, msgStrings);
        }

        /// <summary>
        /// 메시지 가져오기
        /// </summary>
        /// <remarks>
        /// 지정팀의 해당메시지아이디 배열을 가져온다.
        /// 배열[0] SubSystemType
        /// 배열[1] MessageID
        /// 배열[2] MessageType
        /// 배열[3] DisplayMessage
        /// 배열[4] SummaryMessage
        /// </remarks>
        /// <param name="subSystemType">서브시스템타입</param>
        /// <param name="messageID">메시지아이디</param>
        /// <returns>메시지배열</returns>
        protected string[] Msgs(SubSystemType subSystemType, string messageID)
        {
            try
            {
                return DNSoft.eW.FrameWork.eWDictionary.GetMessage(this.Sessions.Language, subSystemType, messageID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 메시지 가져오기
        /// </summary>
        /// <remarks>
        /// 해당메시지아이디의 배열을 가져온다.
        /// 배열[0] SubSystemType
        /// 배열[1] MessageID
        /// 배열[2] MessageType
        /// 배열[3] DisplayMessage
        /// 배열[4] SummaryMessage
        /// </remarks>
        /// <param name="messageID">메시지아이디</param>
        /// <returns>메시지배열</returns>
        protected string[] Msgs(string messageID)
        {
            return Msgs(SubSystemType.Common, messageID);
        }

        /// <summary>
        /// 메시지 가져오기
        /// </summary>
        /// <param name="arrTemp">메시지배열</param>
        private void MsgExecute(string[] arrTemp)
        {
            string strTemp = arrTemp[3] + "|^|" + arrTemp[4];
            try
            {
                switch (arrTemp[2])
                {
                    case "01": //information
                        this.informationMessage = strTemp;
                        break;
                    case "02": //confirm
                        this.confirmMessage = strTemp;
                        break;
                    case "03": //alert
                        this.errorMessage = strTemp;
                        break;
                    default:
                        this.informationMessage = strTemp;
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region 로깅
        /// <summary>
        /// 로깅레벨셋팅
        /// </summary>
        private void SetLogLevel()
        {
            try
            {
                if (LogHandler.LogSubSystem)
                {
                    //로깅레벨셋팅
                    string sTemp = string.Empty;
                    sTemp = this.GetType().BaseType.ToString();
                    sTemp = sTemp.Replace("DNSoft.eW.", "");
                    sTemp = sTemp.Substring(0, sTemp.IndexOf("."));
                    //strLogLevel = DNSoft.eW.FrameWork.eWBase.GetConfigString("LogLevel-" + sTemp);
                    strLogLevel = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/SubTeamLogLevel/" + sTemp);
                    switch (strLogLevel)
                    {
                        case "1":
                        case "2":
                            {
                                bSubStatus = true;
                                break;
                            }
                        default:
                            {
                                bSubStatus = false;
                                break;
                            }
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
            }
        }

        /// <summary>
        /// 로깅스타트
        /// </summary>
        /// <param name="ts">TimeStamp개체</param>
        /// <example>
        /// TimeStamp를 넘겨서 로깅을 시작한다.
        /// <code>
        ///		private void test()
        ///		{
        ///			//로깅시작
        ///			TimeStamp ts = null;
        ///			LoggingStart(ref ts);
        ///
        ///			DNSoft.eW.TestTeam.BSL.TestJob01.TestBSLClass_Nx oClass = null;
        ///			try
        ///			{
        ///				using(oClass = new DNSoft.eW.TestTeam.BSL.TestJob01.TestBSLClass_Nx())
        ///				{
        ///					DataGrid1.DataSource = oClass.GetData();
        ///					DataGrid1.DataBind();
        ///					SetFocus(TextBox1.ID.ToString());
        ///				}
        ///		
        ///			}
        ///			catch(Exception ex)
        ///			{
        ///				this.errorMessage = ex.ToString();
        ///			}
        ///			finally
        ///			{
        ///				//로깅끝
        ///				LoggingEnd(ts,this,MethodInfo.GetCurrentMethod().Name);
        ///			}
        ///		}
        /// </code>
        /// </example>
        protected void LoggingStart(ref TimeStamp ts)
        {
            try
            {
                if (bSubStatus)
                {
                    //로깅끝
                    ts = new TimeStamp();
                    ts.TimeStampStart();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 로깅끝
        /// </summary>
        /// <param name="ts">TimeStamp개체</param>
        /// <param name="target">this</param>
        /// <param name="sServiceName">서비스명</param>
        /// <example>
        /// TimeStamp를 넘겨서 로깅을 종료한다.
        /// <code>
        ///		private void test()
        ///		{
        ///			//로깅시작
        ///			TimeStamp ts = null;
        ///			LoggingStart(ref ts);
        ///
        ///			DNSoft.eW.TestTeam.BSL.TestJob01.TestBSLClass_Nx oClass = null;
        ///			try
        ///			{
        ///				using(oClass = new DNSoft.eW.TestTeam.BSL.TestJob01.TestBSLClass_Nx())
        ///				{
        ///					DataGrid1.DataSource = oClass.GetData();
        ///					DataGrid1.DataBind();
        ///					SetFocus(TextBox1.ID.ToString());
        ///				}
        ///		
        ///			}
        ///			catch(Exception ex)
        ///			{
        ///				this.errorMessage = ex.ToString();
        ///			}
        ///			finally
        ///			{
        ///				//로깅끝
        ///				LoggingEnd(ts,this,MethodInfo.GetCurrentMethod().Name);
        ///			}
        ///		}
        /// </code>
        /// </example>
        protected void LoggingEnd(TimeStamp ts, object target, string sServiceName)
        {
            try
            {
                if (bSubStatus && ts != null)
                {
                    //로깅끝
                    ts.TimeStampEnd(target, sServiceName);
                    if (ts != null) ts.Dispose();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 사용자정보셋팅
        /// <summary>
        /// 사용자 존재유무 체킹
        /// </summary>
        /// <returns>true : 존재, false : 없음</returns>
        protected bool IsLogined()
        {
            //			if (Sessions.Account.Length > 0)
            //				return true;
            //			else
            //				return false;
            return true;

        }

        /// <summary>
        /// 세션클래스 활성화
        /// </summary>
        public void SetSession()
        {
            SetSession(false);
        }

        /// <summary>
        ///  세션클래스 활성화
        /// </summary>
        /// <param name="isForce"></param>
        public void SetSession(bool isForce)
        {

            // 로그오프후 다른사용자로 로그인시 쿠키정보가 존재하기 때문에 세션ID로 체크하는 로직 추가
            // strUserName -> arrUserinfo[]로 받음. 쿠키정보를 배열로 받아처리함.
            string strUserName = string.Empty;
            string strUserPwd = string.Empty;
            string[] arrUserInfo = null;

            string mailLink = this.Request["maillink"].NullObjectToEmptyEx();
            bool fromMail = false;

            //윈도우 로그인은 운영체제가 윈도우 7, 10 일때만 사용
            if (Request.UserAgent.IndexOf("Windows NT 6.1") > 0 || Request.UserAgent.IndexOf("Windows NT 10.0") > 0)
            {
                fromMail = mailLink.ToUpper().Equals("Y") ? true : false;
                //가장이후에, 유저 정보를 가지고 오지 못하여, 우선 무조건 login page 로 변경
                fromMail = false;
            }

            try
            {
                //strUserName = DNSoft.eW.FrameWork.ExtensionUtils.UserNameFromCookie();
                arrUserInfo = DNSoft.eW.FrameWork.ExtensionUtils.UserInfoFromCookie();
                strUserName = arrUserInfo[1];
                strUserPwd = arrUserInfo[2];
                

                if (strUserName.IsNullOrEmptyEx())
                {
                    // 개발시 로그인 처리를 하지 않기 위해서 
                    // config정보로 로그인 처리를 한다.
                    // 작성일 : 2010년 12월 21일 
                    // 작성자 : (주) 닷넷 소프트 이명우
                    if (DNSoft.eW.FrameWork.eWBase.GetConfig("//LocalDevelop/UsedYN") != null)
                    {
                        string stUserYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//LocalDevelop/UsedYN");
                        string stUserID = DNSoft.eW.FrameWork.eWBase.GetConfig("//LocalDevelop/UserID");
                        string stUserPwd = DNSoft.eW.FrameWork.eWBase.GetConfig("//LocalDevelop/UserPwd");
                        string strNetbiosDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/NetBiosDomain");
                        string strFullId = string.Empty;

                        if (stUserYN == "Y")
                        {

                            strFullId = strNetbiosDomain + "\\" + stUserID;
                            string strEncode = DNSoft.eW.FrameWork.eWBase.UserInfoEncrypt(strFullId + ":" + stUserPwd);

                            SetCookie("USERINFO_ENC", strEncode, "");

                            strUserName = stUserID;

                            Sessions = GetAccountInfo(strUserName, isForce);
                        }
                        else
                        {
                            if (fromMail)
                            {
                                //System.Security.Principal.WindowsIdentity wi = System.Web.HttpContext.Current.User.Identity.Name;
                                string userName = System.Web.HttpContext.Current.User.Identity.Name;
                                //string userName = this.User.Identity.Name;
                                //string userName = Request.LogonUserIdentity.Name;
                                strFullId = userName;
                                //strFullId = strFullId.Replace("SGWVX", "sgwvx");
                                if (strFullId.Contains("\\"))
                                {
                                    strNetbiosDomain = strFullId.Split('\\')[0];
                                    stUserID = strFullId.Split('\\')[1];
                                }
                                else
                                {
                                    strNetbiosDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/NetBiosDomain");
                                    stUserID = strFullId;
                                }
                                string strEncode = DNSoft.eW.FrameWork.eWBase.UserInfoEncrypt(strFullId + ":1234"); //비밀번호 의미없음

                                SetCookie("USERINFO_ENC", strEncode, "");

                                strUserName = stUserID;
                                
                                
                                Sessions = GetAccountInfo(strUserName, isForce);
                            }
                            else
                            {
                                Response.Redirect(DNSoft.eW.FrameWork.eWBase.GetConfig("//LogonURL") + "?ReturnURL=" + Server.UrlEncode(Request.RawUrl));
                                Response.End();
                            }
                        }

                    }
                }
                //쿠키에 사용자 아이디가 존재할 경우
                else
                {
                    Sessions = GetAccountInfo(strUserName, isForce);

                }
                //Response.Redirect("http://ewf.kr.bayer.cnb/eWorks/Manage/Authentication/Logon.aspx" + "?ReturnURL=" + Server.UrlEncode(Request.RawUrl));
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region 공통 UTILS
        /// <summary>
        /// PostBack만들기
        /// </summary>
        /// <param name="control"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        protected bool ChangePostBack(object control, string functionName)
        {
            bool bReturn = false;
            System.Web.UI.WebControls.WebControl wcTemp = null;

            try
            {
                //포스트백
                wcTemp = (System.Web.UI.WebControls.WebControl)control;
                wcTemp.Attributes.Remove("onclick");
                wcTemp.Attributes.Add("onclick", string.Format("if ({0} == false ) return false;", functionName));
                bReturn = true;
            }
            catch (Exception)
            {
                bReturn = false;
            }
            return bReturn;
        }

        /// <summary>
        /// 웹루트
        /// </summary>
        public string GetWebRoot
        {
            get
            {
                return DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/WebRoot/LogicalPath");
            }
        }

        /// <summary>
        /// 파일서버
        /// </summary>
        public string GetFileServer
        {
            get
            {
                return DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/FileMgr/PhysicalPath");
            }
        }
        #endregion

        #region 공통이벤트 - 실행순서
        /// <summary>
        /// 페이지 PreInit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            SetSession(); //세션클래스생성

            base.OnPreInit(e);
        }

        /// <summary>
        /// 페이지 초기화
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// 페이지 로드
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            SetLogLevel(); //로깅레벨설정

            base.OnLoad(e);
        }

        /// <summary>
        /// 렌더링전에공통함수호출
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            SetPageHiddenField(); //공통스크립트와 HiddenField설정
            base.OnPreRender(e);
        }

        /// <summary>
        /// 공통으로 사용하는 DNSoft.eW.Framework.Scripts 파일들을 랜더링 한다.
        /// </summary>
        protected void RegisterScriptBlock()
        {
            int nHeaderCount = 1;
            string strSessionExpire = null;

            System.Web.UI.LiteralControl oLiCtrl = null;
            System.Web.UI.LiteralControl oLiCtrlBody = null;

            try
            {
                if (this.Header != null)
                {
                    oLiCtrl = new LiteralControl();

                    if (Header.Controls.Count < 1)
                        nHeaderCount = 0;

                    oLiCtrl.Text = GetRegisterScriptBlockString();

                    // 헤더 밑으로 js 파일을 추가한다.
                    this.Header.Controls.AddAt(nHeaderCount, oLiCtrl);
                }

                // 세션만료기간설정
                strSessionExpire = DNSoft.eW.FrameWork.eWBase.GetConfig("//FrameWork/SessionExpire", "");
                if (this.Form != null && strSessionExpire.IsNotNullOrEmptyEx())
                {
                    oLiCtrlBody = new LiteralControl();

                    oLiCtrlBody.Text = "<iframe src=\"" + strSessionExpire + "\" width=\"0\" height=\"0\" frameborder=\"0\" style=\"0 0 0 0;\"></iframe>\r\n";
                    this.Form.Controls.Add(oLiCtrlBody);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 공통으로 사용하는 DNSoft.eW.Framework.Scripts 파일들 불러온다.
        /// </summary>
        protected string GetRegisterScriptBlockString()
        {
            StringBuilder oSbConst = null;

            string strKey = string.Empty;
            string strValue = string.Empty;
            string strType = string.Empty;
            string strLocalDevelopYN = string.Empty;

            string[] arrKey = null;
            string[] arrValue = null;
            string[] arrType = null;

            string strReturn = string.Empty;

            try
            {
                oSbConst = new StringBuilder(1000);

                // js에 필요한 Const 를 추가한다.
                oSbConst.Append("<meta http-equiv='X-UA-Compatible' content='IE=edge' />");
                oSbConst.Append("\r\n<script language='javascript'>\r\n");
                oSbConst.Append("   // 전역적으로 사용되는 스크립트 변수\r\n");

                arrKey = DNSoft.eW.FrameWork.eWBase.GetConfigs("//FrameWork/Scripts/Const/Key");
                arrValue = DNSoft.eW.FrameWork.eWBase.GetConfigs("//FrameWork/Scripts/Const/Value");
                arrType = DNSoft.eW.FrameWork.eWBase.GetConfigs("//FrameWork/Scripts/Const/Type");
                strLocalDevelopYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/LocalDevelopYN");


                for (int i = 0; i < arrKey.Length; i++)
                {
                    // Const Key
                    strKey = arrKey[i];
                    strValue = arrValue[i];
                    strType = arrType[i];

                    if (!String.IsNullOrEmpty(strKey))
                    {
                        // 참조하는 경우에는 참조의 값을 가저온다.
                        if (strType.Equals("Reffer"))
                        {
                            strValue = DNSoft.eW.FrameWork.eWBase.GetConfig(strValue, string.Empty);
                        }

                        // 개발PC의 개발 환경이 파일시스템일 경우
                        if (strLocalDevelopYN == "Y")
                        {
                            if (strKey == "WEBSERVER")
                            {
                                strValue = System.Web.HttpContext.Current.Request.Url.Authority;
                                oSbConst.Append("   var " + strKey + " = \"" + strValue + "\";\r\n");
                            }
                            else
                                oSbConst.Append("   var " + strKey + " = \"" + strValue + "\";\r\n");
                        }
                        else
                            oSbConst.Append("   var " + strKey + " = \"" + strValue + "\";\r\n");

                    }
                }

                if (_sbLocalscript != null)
                {
                    oSbConst.Append("\r\n   // 해당 페이지에 사용되는 스크립트 변수\r\n");
                    oSbConst.Append(_sbLocalscript.ToString());
                }

                oSbConst.Append("</script>\r\n");

                strReturn = oSbConst.ToString();
            }

            catch
            {
                throw;
            }
            finally
            {
                if (oSbConst != null) oSbConst = null;
            }

            return strReturn;
        }


        /// <summary>
        /// 공통스크립트와 Hidden필드추가
        /// </summary>
        private void SetPageHiddenField()
        {
            string strValue = string.Empty;
            string strReturn = string.Empty;
            System.Text.StringBuilder sbTemp = null;

            try
            {
                //공통 Script 인클루드
                this.RegisterScriptBlock();

                // 1. Popup Window Close Setting
                if (windowClose == true)
                {
                    if (returnValue.Equals(""))
                    {
                        strValue = "closed";
                        strReturn = "";
                    }
                    else
                    {
                        strValue = "closed";
                        strReturn = this.returnValue.Replace("\"", "").Replace("'", "");
                    }
                }
                else
                {
                    strValue = "open";
                    strReturn = "";
                }
                Page.ClientScript.RegisterHiddenField("winClosed", strValue);
                Page.ClientScript.RegisterHiddenField("winClosedReturn", strReturn);

                // MenuPrefix Nex1에서 사용
                if (this.menuInfo.Trim().Equals(string.Empty)) { strValue = string.Empty; }
                else { strValue = this.menuInfo.Replace("'", "").Replace("\"", ""); }
                Page.ClientScript.RegisterHiddenField("menuInfo", strValue);

                string errorMessageTit = string.Empty;
                // 2. Behind Code Error Setting
                if (errorMessage.Length > 0)
                {


                    if (errorMessage.IndexOf("System.NullReferenceException:") > 0)
                    {
                        // Exception이 발생할 경우
                        errorMessageTit = errorMessage.Substring(errorMessage.IndexOf("System.NullReferenceException:") + 10);
                    }
                    else if (errorMessage.IndexOf("System.ArgumentException:") > 0)
                    {
                        // 일반 메시지 오류를 던졌을 경우
                        errorMessageTit = errorMessage.Substring(errorMessage.IndexOf("System.ArgumentException:") + 10);
                    }
                    else
                    {
                        errorMessageTit = errorMessage.Substring(errorMessage.IndexOf("Exception:") + 10);
                    }

                    errorMessageTit = errorMessageTit.Substring(0, errorMessageTit.IndexOf('.') + 1);

                    strValue = errorMessageTit + "|^|" + errorMessage.Replace("'", "").Replace("\"", "");
                }
                else { strValue = ""; }
                Page.ClientScript.RegisterHiddenField("errorMessage", strValue);

                // 3. Behind Code Information Setting
                if (informationMessage.Length > 0) { strValue = informationMessage.Replace("'", "").Replace("\"", ""); }
                else { strValue = ""; }

                if (strValue.Contains(this.Msgs("0002")[3].ToString()))
                {
                    string temp = strValue.Replace(this.Msgs("0002")[3].ToString(), "");
                    strValue = this.Msgs("0002")[3].ToString() + "<b>" + temp + "</b>";
                }
                Page.ClientScript.RegisterHiddenField("informationMessage", strValue);

                // 4. Behind Code Confirm Setting
                if (confirmMessage.Length > 0) { strValue = confirmMessage.Replace("'", "").Replace("\"", ""); }
                else { strValue = ""; }
                Page.ClientScript.RegisterHiddenField("confirmMessage", strValue);

                Page.ClientScript.RegisterHiddenField("eWLanguage", "ko-KR");

            }
            catch
            {
                throw;
            }
            finally
            {
                if (sbTemp != null) sbTemp = null;
            }
        }
        #endregion

        #region Page Render
        /// <summary>
        /// 페이지 랜더
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        #endregion

        #region SetCookie 지정한 쿠키를 설정
        /// <summary>
        /// 지정한 쿠키를 설정
        /// </summary>
        /// <param name="cookieName">쿠키명</param>
        /// <param name="cookieValue">쿠키값</param>
        /// <param name="domainName">도메인명</param>
        private void SetCookie(string cookieName, string cookieValue, string domainName)
        {
            System.Web.HttpCookie oMyCookie = null;

            try
            {
                oMyCookie = new System.Web.HttpCookie(cookieName);
                oMyCookie.Value = cookieValue;
                oMyCookie.Domain = domainName;
                oMyCookie.Path = "/";
                //oMyCookie.Expires = DateTime.Now.AddDays(1d);
                Response.Cookies.Add(oMyCookie);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TsLog
        /// <summary>
        /// this에 존재하는 메소드를 실행하고 메소드 실행시간을 로그에 남긴다.<br/> 
        /// 메서드에 매개변수가 없을 경우
        /// </summary>
        /// <param name="stMethodName"></param>
        public void TsLog(string stMethodName)
        {
            //	로깅처리
            TimeStamp tsLog = null;
            LoggingStart(ref tsLog);

            try
            {
                //Method실행
                Type tp = this.GetType();
                MethodInfo mi = tp.GetMethod(stMethodName, BindingFlags.NonPublic | BindingFlags.Instance);

                Object result = mi.Invoke(this, null);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                LoggingEnd(tsLog, this, stMethodName);
            }
        }

        /// <summary>
        /// this에 존재하는 메소드를 실행하고 메소드 실행시간을 로그에 남긴다.<br/> 
        /// 메서드에 매개 변수가 있을경우
        /// </summary>
        /// <param name="stMethodName"></param>
        /// <param name="tpType"></param>
        /// <param name="objValue"></param>
        public void TsLog(string stMethodName, Type[] tpType, Object[] objValue)
        {
            //	로깅처리
            TimeStamp tsLog = null;
            LoggingStart(ref tsLog);

            try
            {
                //Method실행
                Type tp = this.GetType();

                MethodInfo mi = tp.GetMethod(stMethodName, tpType);
                Object result = mi.Invoke(this, objValue);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                LoggingEnd(tsLog, this, stMethodName);
            }
        }
        #endregion

        #region RegistScript

        /// <summary>
        /// CS에서 윈도우(팝업) 창 호출 - Javascript 호출
        /// </summary>
        protected void ClientWindowClose()
        {
            ClientWindowClose("true", string.Empty);
        }

        /// <summary>
        /// CS에서 윈도우(팝업) 창 호출 - Javascript 호출
        /// </summary>
        /// <param name="retValue">returnValue = true or false</param>
        protected void ClientWindowClose(string retValue)
        {
            ClientWindowClose(retValue, string.Empty);
        }

        /// <summary>
        /// CS에서 윈도우(팝업) 창 호출 - Javascript 호출
        /// </summary>
        /// <param name="retValue">returnValue = true or false</param>
        /// <param name="appendJavascript">창닫기전처리 스크립트 호출전달</param>
        protected void ClientWindowClose(string retValue, string appendJavascript)
        {
            StringBuilder script = new StringBuilder(128);
            try
            {
                script.Append("<script language='javascript'>");
                script.Append("function f() {");
                script.Append("var oArg = new Object();");
                script.AppendFormat("oArg.returnValue = {0};", retValue);
                script.Append(appendJavascript);
                script.Append("Sys.Application.remove_load(f);");
                script.Append("var oWnd = GetRadWindow();");
                script.Append("oWnd.close(oArg);");
                script.Append("}");
                script.Append("Sys.Application.add_load(f);");
                script.Append("</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "RequestComplete", script.ToString());
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
            }
            finally
            {
                if (script != null)
                {
                    script.Clear();
                    script = null;
                }
            }
        }
        #endregion

    }
}
