using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eW.FrameWork;
using System.Data;

using System.Web.Security;

namespace Approval.Manage.Authentication
{
    public partial class Logon : System.Web.UI.Page
    {

        #region 페이지로드
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                hacConfirm.ServerClick += new EventHandler(hacConfirm_Click);

                if (!IsPostBack)
                {

                    //쿠키에 사용자 아이가 남아 있으면 아이디를 기본으로 설정해준다.
                    //if (Request.Cookies["IDSAVE"] != null)
                    //{
                    //    if (!Request.Cookies["USESAVE"].IsNullOrEmptyEx() && Request.Cookies["USESAVE"].Value.ToLower().Equals("true"))
                    //    {
                    //       // this.htxtUserID.Value = Request.Cookies["IDSAVE"].Value;
                    //        this.hchSave.Checked = true;
                    //    }
                    //}
                    //else
                    //{
                    //   // this.htxtUserID.Value = string.Empty;
                    //    this.hchSave.Checked = false;
                    //}

                    if (Request["ReturnURL"] != null)
                    {
                        this.hReturnURL.Value = Request["ReturnURL"];
                    }
                }

                Session.Abandon();

                // 엔터키를 칠경우 로그인 처리

                // this.htxtUserPassword.Attributes["onkeypress"] = " if( event.keyCode == 13 ){" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(hacConfirm)) + "; return true;}";

                if (this.htxtUserID.Value.Length > 0)
                {
                    string strDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/LogonDomain");
                    if (!Request.Cookies["USESAVE"].IsNullOrEmptyEx() && Request.Cookies["USESAVE"].Value.ToLower().Equals("true"))
                    {
                        SetCookie("IDSAVE", this.htxtUserID.Value, strDomain);
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorMessage.Value = ex.ToString();
            }

        }


        #endregion

        protected void hacConfirm_Click(object sender, EventArgs e)
        {

            string strLogonDomain = string.Empty;

            try
            {
                if (this.htxtUserID.Value.ToString().Trim() == string.Empty)
                {
                    informationMessage.Value = "사용자ID를 입력해주세요.";

                }
                else if (this.htxtUserPassword.Value.ToString().Trim() == string.Empty)
                {
                    informationMessage.Value = "비밀번호를 입력해주세요.";
                }
                else
                {

                    #region 사용자아이디 쿠키에 담아두기


                    strLogonDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/LogonDomain");


                    // 기존 쿠키가 있는 경우 업데이트 한다.
                    if (Request.Cookies["IDSAVE"].IsNullOrEmptyEx())
                    {
                        //if (hchSave.Checked)
                        //{
                        //    SetCookie("IDSAVE", this.htxtUserID.Value.ToString().Trim(), strLogonDomain);
                        //}
                    }

                    // SetCookie("USESAVE", this.hchSave.Checked.ToString().ToLower().Trim(), strLogonDomain);

                    #endregion

                    //로그인프로세스를 진행한다.
                    LogOnProcess();
                }

            }
            catch (Exception ex)
            {
                this.errorMessage.Value = ex.ToString();
            }
        }


        #region LogOnProcess 로그인 프로세서진행
        public void LogOnProcess()
        {
            #region 지역

            DNSoft.eW.FrameWork.Common.EA.ActiveDirectory oAd = null;
 
            DataSet dsCon = null;

            string strDomain = string.Empty;
            string strNetbiosDomain = string.Empty;
            string strUserID = string.Empty;
            string strHomeMDB = string.Empty;
            string strUserPassword = string.Empty;
            string strFullUserID = string.Empty;
            string strEncode = string.Empty;
            string strLocalDevelopYN = string.Empty;
            string strCookieDomain = string.Empty;
            string strDeleteDate = string.Empty;
            bool bRet = false;
            string strReturnURL = string.Empty; 

            #endregion

            try
            {

                dsCon = new DataSet();
                strDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/ADDomain");
                strNetbiosDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/NetBiosDomain");
                strCookieDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/LogonDomain");
                strLocalDevelopYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//LocalDevelop/UsedYN");

                strUserID = this.htxtUserID.Value.ToString().Trim();
                strUserPassword = this.htxtUserPassword.Value.ToString().Trim();
                strReturnURL = this.hReturnURL.Value.ToString().Trim();

                #region AD체크
                using (oAd = new DNSoft.eW.FrameWork.Common.EA.ActiveDirectory())
                {
                    //bRet = oAd.IsAuthenticated(strDomain, strUserID, strUserPassword);
                    bRet = true;

                    if (bRet)
                    { 
                        strFullUserID = strNetbiosDomain + "\\" + strUserID;
                        dsCon = DNSoft.eW.FrameWork.eWDictionary.GetUserLogin(strUserID);
                        if (dsCon == null || dsCon.Tables.Count < 1 || dsCon.Tables[0].Rows.Count < 1)
                        {
                            htxtMessage.Value = "해당 사용자가 없습니다.";
                        } 
                        else
                        {
                            //foreach (DataRow oDr in dsCon.Tables[0].Rows)
                            //{
 
                                /* EW_USERINFO_ENC 암호화 방식 */
                                strEncode = DNSoft.eW.FrameWork.eWBase.UserInfoEncrypt(strFullUserID + ":" + strUserPassword);

                                // 로그온 도메인이 다를때
                                //if (strLocalDevelopYN == "Y")
                                    SetCookie("USERINFO_ENC", strEncode, "");
                                //else
                                //    SetCookie("USERINFO_ENC", strEncode, strCookieDomain);
                                             
                                if (strReturnURL != null && strReturnURL.Length > 0)
                                    Response.Redirect(strReturnURL, false);
                                else
                                {
                                    string strWebRoot = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/WebServer/WebRoot/LogicalPath");
                                    Response.Redirect(strWebRoot + "Document/main.aspx", false);
                                }       
                            //}
                        }
                         
                    }
                    else
                    {
                        this.htxtMessage.Value = "아이디 또는 비밀번호를 확인하여 주십시오";
                        this.htxtUserID.Value = "";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                htxtMessage.Value = ex.Message;
            }
            finally
            {
                if (oAd != null) oAd = null;
            }
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
            HttpCookie oMyCookie = null;

            try
            {
                oMyCookie = new HttpCookie(cookieName);
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
    }
}