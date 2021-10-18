using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNSoft.eW.FrameWork
{
	/// <summary>
	/// <b>공통 ExtensionUtils에 대한 요약 설명입니다.</b><br/> 
    /// </summary>
    /// <remarks>
	/// - 작  성  자 : 닷넷소프트 김학진<br/>
	/// - 최초작성일 : 2009.11.12<br/>
	/// - 최종수정자 : <br/>
	/// - 최종수정일 : <br/>
	/// - 주요변경로그<br/>
	/// 2009.11.12 생성<br/>
    /// </remarks>
	public static class ExtensionUtils
    {

        #region NullObjectToEmptyEx
        /// <summary>
		/// <b>null일 경우 string.Empty로 반환한다.</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.15 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static string NullObjectToEmptyEx(this object pobj)
		{
			string strReturn = string.Empty;
			try
			{
				if ((pobj == null) || (pobj.Equals(System.DBNull.Value)) || (pobj.ToString().Length == 0))
				{
					strReturn = string.Empty;
				}
				else
				{
					strReturn = pobj.ToString().Trim();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return strReturn;
		}
		#endregion
         
        #region NullObjectToZeroEx
        /// <summary>
		/// <b>null일 경우 "0"로 반환한다.</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.15 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static string NullObjectToZeroEx(this object pobj)
		{
			string strReturn = string.Empty;
			try
			{
				strReturn = NullObjectToEmptyEx(pobj);
				if (strReturn == string.Empty)
				{
					strReturn = "0";
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return strReturn;
		}
		#endregion

        #region NullObjectToOneEx
        /// <summary>
		/// <b>null일 경우 "1"로 반환한다.</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.21 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static string NullObjectToOneEx(this Object pobj)
		{
			string strReturn = string.Empty;
			try
			{
				strReturn = NullObjectToEmptyEx(pobj);
				if (strReturn == string.Empty)
				{
					strReturn = "1";
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return strReturn;
		}
		#endregion

        #region NullObjectToNumberEx
        /// <summary>
		/// <b>null일 경우 "0"로 반환한다.</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.15 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static int NullObjectToNumberEx(this Object pobj, int defaultNumber)
		{
			string strTemp = string.Empty;
			int nReturn = defaultNumber;

			try
			{
				strTemp = NullObjectToEmptyEx(pobj);
				if (strTemp != string.Empty)
				{
					nReturn = Convert.ToInt32(strTemp);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return nReturn;
		}
		#endregion

        #region NullObjectToStringEx
        /// <summary>
		/// <b>null일 경우 "0"로 반환한다.</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.15 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static string NullObjectToStringEx(this Object pobj, string defaultString)
		{
			string strTemp = string.Empty;
			string nReturn = defaultString;
			
			try
			{
				strTemp = NullObjectToEmptyEx(pobj);
				if (strTemp != string.Empty)
				{
					nReturn = strTemp;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return nReturn;
		}
		#endregion

        #region IsNullOrEmptyEx
        /// <summary>
		/// <b>null일 경우 true를 반환한다..</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.15 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static bool IsNullOrEmptyEx(this object pobj)
		{
			bool bReturn = false;
			try
			{
				if ((pobj == null || (pobj.Equals(System.DBNull.Value)) || (pobj.ToString().Length == 0)))
				{
					bReturn = true;
				}
				
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return bReturn;
		}
		#endregion

		#region IsNotNullOrEmptyEx
		/// <summary>
		/// <b>null일 경우 true를 반환한다..</b><br/>
        /// </summary>
        /// <remarks>
		/// - 최초작성자 : 닷넷소프트 김학진<br/>
		/// - 최초작성일 : 2009.11.12<br/>
		/// - 최종수정일 : 2009.11.12<br/>
		/// - 주요변경로그<br/>
		/// 2005.03.15 생성<br/>
        /// </remarks>
		/// <param name="pobj"></param>
		/// <returns></returns>
		public static bool IsNotNullOrEmptyEx(this object pobj)
		{
			bool bReturn = true;
			try
			{
				if ((pobj == null || (pobj.Equals(System.DBNull.Value)) || (pobj.ToString().Length == 0)))
				{
					bReturn = false;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return bReturn;
		}
		#endregion

        #region UserNameFromCookie
        /// <summary>
        /// <b>쿠키정보(사용자 ID)을 리턴</b><br/>
        /// </summary>
        /// <returns>사용자명</returns>
		public static string UserNameFromCookie()
		{
			return UserInfoFromCookie()[1];
        }
        #endregion

        #region UserInfoFromCookie
        /// <summary>
        /// <b>암호화되어 있는 쿠키정보를 리턴한다</b><br/>
        /// </summary>
        /// <returns>사용자정보셋</returns>
		public static string[] UserInfoFromCookie()
		{
			string strDecoding = string.Empty;
			string[] arrReturn = new String[3]{"", "", ""};

			try
			{
                if (System.Web.HttpContext.Current.Request.Cookies["USERINFO_ENC"].IsNotNullOrEmptyEx())
                {
                    strDecoding = eWBase.UserInfoDecrypt(System.Web.HttpContext.Current.Request.Cookies["USERINFO_ENC"].Value);
                } 
 
				if (strDecoding.IsNotNullOrEmptyEx())
				{
					arrReturn[0] = strDecoding.Split('\\')[0];
					arrReturn[1] = strDecoding.Split('\\')[1].Split(':')[0];
					arrReturn[2] = strDecoding.Split('\\')[1].Split(':')[1];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{

			}

			return arrReturn;
		}
		#endregion
	}
}
