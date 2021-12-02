using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Approval.Manage.Authentication
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string strDomain = string.Empty;
            HttpCookie oIDCookie = null;

            try
            {
                Response.Cookies.Clear();

                strDomain = DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/Domain/LogonDomain");

                oIDCookie = new HttpCookie("USERINFO");
                oIDCookie.Domain = strDomain;
                oIDCookie.Path = "/";
                oIDCookie.Expires = System.DateTime.Now.AddMonths(-1);
                Response.Cookies.Add(oIDCookie);

                System.Web.Security.FormsAuthentication.SignOut();

                this.Session.Abandon();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                try
                {
                    this.Response.Clear();
                    this.Response.Redirect(DNSoft.eW.FrameWork.eWBase.GetConfig("//LogonURL"));
                    this.Response.End();
                }

                catch (Exception)
                {
                }
            }
        }

        #region Web Form 디자이너에서 생성한 코드
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}