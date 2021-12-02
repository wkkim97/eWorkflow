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
	/// 웹파트들이 상속받는 페이지
	/// </summary>
	[Serializable]
	public class UserControlsBase : System.Web.UI.UserControl
	{
		#region 멤버변수들
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

		protected AccountInfo _Sessions = null;

 
		private bool bSetInitStart = true;

		/// <summary>
		/// 서브팀상태값
		/// </summary>
		private bool bSubStatus = false; //서브팀
		/// <summary>
		/// 로깅레벨
		/// </summary>
		private string strLogLevel = string.Empty;

		#endregion

		#region 생성자
		/// <summary>
		/// 생성자 - 초기화
		/// </summary>
		public UserControlsBase()
		{
			try
			{
				//생성자
				errorMessage = string.Empty;
				informationMessage = string.Empty;
				//windowClose = false;
				//returnValue = string.Empty;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

        #region Sessions 가져오기
        /// <summary>
        /// 사용자 정보를 세션에서 가져온다.<br/>
        /// 세션에 사용자 정보가 없을때만 쿠키에서 사용자 정보를 가져온다.
        /// </summary>
		protected AccountInfo Sessions
		{
			get
			{
				this.SetSession();
				return this._Sessions;
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
			catch (Exception ex)
			{
				throw ex;
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
			catch (Exception ex)
			{
				throw ex;
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
			try
			{ 
				if (this.bSetInitStart)
				{
					if (this.Session != null)
					{
						this._Sessions = (AccountInfo)this.Session["UserSessionClass"];
						this.bSetInitStart = false;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region 공통 UTILS
		/// <summary>
		/// Client Controls에 Focus셋팅
		/// </summary>
		/// <param name="clientControlid">Client Control ID</param>
		/// <example>
		/// 페이지로딩시에 특정콘트롤에 Focus를 두게 한다.
		/// <code>
		///	
		/// </code>
		/// </example>
		protected void SetFocus(string clientControlid)
		{
			System.Text.StringBuilder stbTemp = null;
			try
			{
				//포커스
				stbTemp = new StringBuilder();
				stbTemp.Append("<script language='javascript'>\r\n");
				stbTemp.Append("try{");
				stbTemp.Append("document.all.item('" + clientControlid + "').focus();\r\n");
				stbTemp.Append("}catch(exception){}\r\n");
				stbTemp.Append("</script>\r\n");

				Page.Controls.Add(new LiteralControl(stbTemp.ToString()));
				if (stbTemp != null) stbTemp = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

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

		#region 공통이벤트
		/// <summary>
		/// 렌더링전에공통함수호출
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			SetPageHiddenField(); //공통스크립트와 HiddenField설정
			base.OnPreRender(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			SetLogLevel(); //로깅레벨설정
			SetSession(); //세션클래스생성

			//string strLang = Page.Request["lang"];
			//string strLang = this.Sessions.Language;

			base.OnLoad(e);
		}
         
		/// <summary>
		/// 공통스크립트와 Hidden필드추가
		/// </summary>
		private void SetPageHiddenField()
		{
			string strValue = string.Empty;
			string strReturn = string.Empty;

			try
			{

				Page.ClientScript.RegisterHiddenField("winClosed", strValue);
				Page.ClientScript.RegisterHiddenField("winClosedReturn", strReturn);

				// 2. Behind Code Error Setting
				if (errorMessage.Length > 0) { strValue = errorMessage.Replace("'", "").Replace("\"", ""); }
				else { strValue = ""; }
				Page.ClientScript.RegisterHiddenField("errorMessage", strValue);

				// 3. Behind Code Information Setting
				if (informationMessage.Length > 0) { strValue = informationMessage.Replace("'", "").Replace("\"", ""); }
				else { strValue = ""; }
				Page.ClientScript.RegisterHiddenField("informationMessage", strValue);

				// 4. Behind Code Confirm Setting
				if (confirmMessage.Length > 0) { strValue = confirmMessage.Replace("'", "").Replace("\"", ""); }
				else { strValue = ""; }
				Page.ClientScript.RegisterHiddenField("confirmMessage", strValue);

                Page.ClientScript.RegisterHiddenField("eWLanguage", "ko-KR"); //this.Sessions.Language
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion
	}
}
