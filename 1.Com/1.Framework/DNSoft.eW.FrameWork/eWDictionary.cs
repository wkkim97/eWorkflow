using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DNSoft.eW.FrameWork
{
    /// <summary>
    /// <b>다국어 관리 클래스</b><br/>
    /// </summary>
	public class eWDictionary  : eWBase
	{
		/// <summary>
		/// Dictionary리스트를 담고 있는 파일
		/// </summary>
		public static System.Collections.Hashtable hstConstList = null;
		public static Hashtable msgCache = Hashtable.Synchronized(new Hashtable());
		
        /// <summary>
        /// 다국어 관리 클래스 생성자 
        /// </summary>
		public eWDictionary(){}

		#region Dictionary항목 불러오기
		/// <summary>
        /// <b>해쉬테이블에 해당 Dictionary불러오기</b><br/>
		/// </summary>
		/// <param name="constCode">해당Dictionary코드</param>
		/// <returns>해당Dictionary항목</returns>
		public static string Dic(string language, string constCode)
		{
			try
			{
				if (hstConstList == null) { hstConstList = new Hashtable(); }
				if (hstConstList[language] == null) { GetConstList(language); }
				if(((Hashtable)hstConstList[language])[constCode] == null)
					return string.Empty;
				else
					return (string)((Hashtable)hstConstList[language])[constCode].ToString();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		/// <summary>
		/// Dictionar테이블에서 리스트가져오기
		/// </summary>
		private static void GetConstList(string language)
		{
			if(hstConstList[language] == null) 
			{
				System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
				System.Data.DataSet dsTemp = null;
				try
				{
					//해쉬테이블에 Dictionary항목을 담는다.
					hstConstList[language] = new Hashtable();
					dsTemp = new DataSet();
					sdaTemp = new SqlDataAdapter("eManage.DBO.USP_SELECT_DICTIONARY", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
					sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;

					sdaTemp.SelectCommand.Parameters.AddWithValue("@VNCTYPE", language);
					sdaTemp.Fill(dsTemp);
					sdaTemp.SelectCommand.Parameters.Clear();

					for(int i = 0 ; i < dsTemp.Tables[0].Rows.Count ; i++)
					{
						if(!((Hashtable)hstConstList[language]).Contains(dsTemp.Tables[0].Rows[i][0].ToString())) {
							((Hashtable)hstConstList[language]).Add(dsTemp.Tables[0].Rows[i][0].ToString(),dsTemp.Tables[0].Rows[i][1].ToString());
						}
					}
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
					if(sdaTemp != null) sdaTemp.Dispose();
					if(dsTemp != null) dsTemp.Dispose();
				}
			}
		}

		#endregion

		#region Message항목 불러오기
		/// <summary>
        /// <b>Message항목 불러오기</b><br/>
		/// </summary>
		/// <param name="subType">서브시스템타입</param>
		/// <param name="messageItem">메시지아이템</param>
		/// <param name="messageID">메시지아이디</param>
		/// <returns>해당메시지</returns>
		public static string GetMessage(string language, string subType, MessageItem messageItem, string messageID )
		{
			string[] strTemp = null;

			try
			{
				strTemp = GetMessage(language, ReturnSubSystemType(subType), messageID);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return strTemp[Convert.ToInt32(messageItem)];
		}
		/// <summary>
		/// Message항목 불러오기
		/// </summary>
		/// <param name="subType">서브시스템타입</param>
		/// <param name="messageItem">메시지아이템</param>
		/// <param name="messageID">메시지아이디</param>
		/// <returns>해당메시지</returns>
		public static string GetMessage(string language, SubSystemType subType, MessageItem messageItem, string messageID )
		{
			string[] strTemp = null;
			try
			{
				strTemp = GetMessage(language, subType, messageID);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return strTemp[Convert.ToInt32(messageItem)];
		}


		/// <summary>
		/// Message항목 배열형태로 불러오기
		/// </summary>
		/// <param name="subType">서브시스템타입</param>
		/// <param name="messageID">메시지아이디</param>
		/// <returns>해당메시지배열</returns>
		public static string[] GetMessage(string language, string subType, string messageID)
		{
			string[] strTemp = null;
			try
			{
				strTemp = GetMessage(language, ReturnSubSystemType(subType), messageID);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return strTemp;
		}

		/// <summary>
		/// Message항목 불러오기
		/// </summary>
		/// <param name="subType">서브시스템타입</param>
		/// <param name="messageID">메세지아이디</param>
		/// <returns>해당메세지배열</returns>
		public static string[] GetMessage(string language, SubSystemType subType, string messageID)
		{
			System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
			System.Data.DataSet dsTemp = null;
			string strSubType = string.Empty;
			string[] strImsi = null;

			try
			{
				//Language셋팅
				if (language == null || language.Equals(string.Empty))
				{
					language = "ko-KR";
				}

				strSubType = GetSubSystemCode(subType);

				strImsi = (string[])msgCache[language + "_" + strSubType + "_" + messageID];

				if (strImsi == null)
				{

					dsTemp = new DataSet();
					strImsi = new string[5];

					//연결정보가져오기
					sdaTemp = new SqlDataAdapter("eManage.DBO.USP_SELECT_MESSAGE", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
					sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;

					sdaTemp.SelectCommand.Parameters.AddWithValue("@nvcLanguageSet", language);
					sdaTemp.SelectCommand.Parameters.AddWithValue("@ncSubSystemType", strSubType);
					sdaTemp.SelectCommand.Parameters.AddWithValue("@ncMessageID",messageID);

					sdaTemp.Fill(dsTemp);
					sdaTemp.SelectCommand.Parameters.Clear();

					if(dsTemp.Tables[0].Rows.Count > 0)
					{
						strImsi[0] = dsTemp.Tables[0].Rows[0][0].ToString();
						strImsi[1] = dsTemp.Tables[0].Rows[0][1].ToString();
						strImsi[2] = dsTemp.Tables[0].Rows[0][2].ToString();
						strImsi[3] = dsTemp.Tables[0].Rows[0][3].ToString();
						strImsi[4] = dsTemp.Tables[0].Rows[0][4].ToString();
					}

					else //결과물이 없을때
					{
						strImsi[0] = strSubType;
						strImsi[1] = messageID;
						strImsi[2] = "03";
						strImsi[3] = "No Item!";
						strImsi[4] = "No Item!";
					}

					msgCache[language + "_" + strSubType + "_" + messageID] = strImsi;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(sdaTemp != null) sdaTemp.Dispose();
				if(dsTemp != null) dsTemp.Dispose();
			}
			return (string[])strImsi.Clone();
		}
		#endregion

		#region 사용자정보 불러오기
		/// <summary>
        /// <b>아이디에따른 해당사용자 정보불러오기</b>
		/// </summary>
        /// <param name="language"></param>
		/// <param name="userAccount">사용자아이디</param>
		/// <returns></returns>
		public static DataSet GetUserInfo(string language, string userAccount)
		{
			System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
			System.Data.DataSet dsTemp = null;
			try
			{
				dsTemp = new DataSet();
				//연결정보가져오기
				sdaTemp = new SqlDataAdapter("eManage.DBO.USP_SELECT_USER", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
				sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;
				//Language셋팅
				if (language == null || language.Equals(string.Empty)) language = "en-US";
				sdaTemp.SelectCommand.Parameters.AddWithValue("@NVCLANGUAGESET", language);
				sdaTemp.SelectCommand.Parameters.AddWithValue("@NVCUSERACCOUNT", userAccount);
				sdaTemp.Fill(dsTemp);
				sdaTemp.SelectCommand.Parameters.Clear();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(sdaTemp != null) sdaTemp.Dispose();
			}
			return dsTemp;
		}

        /// <summary>
        /// <b>아이디에따른 해당사용자 정보불러오기</b>
        /// </summary>
        /// <param name="userAccount">사용자아이디</param>
        /// <returns></returns>
        public static DataSet GetUserConfig(string userAccount)
        {
            System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
            System.Data.DataSet dsTemp = null;
            try
            {
                dsTemp = new DataSet();
                //연결정보가져오기
                sdaTemp = new SqlDataAdapter("eManage.DBO.USP_SELECT_USER_CONFIG", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
                sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;

                sdaTemp.SelectCommand.Parameters.AddWithValue("@USER_ID", userAccount);
                sdaTemp.Fill(dsTemp);
                sdaTemp.SelectCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdaTemp != null) sdaTemp.Dispose();
            }
            return dsTemp;
        }

        /// <summary>
        /// <b>아이디에따른 해당사용자 정보불러오기</b>
        /// </summary>
        /// <param name="userAccount">사용자아이디</param>
        /// <returns></returns>
        public static DataSet GetUserRole(string userAccount)
        {
            System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
            System.Data.DataSet dsTemp = null;
            try
            {
                dsTemp = new DataSet();
                //연결정보가져오기
                sdaTemp = new SqlDataAdapter("eManage.DBO.USP_SELECT_USER_ROLE", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
                sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;

                sdaTemp.SelectCommand.Parameters.AddWithValue("@USER_ID", userAccount);
                sdaTemp.Fill(dsTemp);
                sdaTemp.SelectCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdaTemp != null) sdaTemp.Dispose();
            }
            return dsTemp;
        }
         
		#endregion


        #region 사용자정보 불러오기
        /// <summary>
        /// <b>아이디에따른 해당사용자 정보불러오기</b>
        /// </summary> 
        /// <param name="userAccount">사용자아이디</param>
        /// <returns></returns>
        public static DataSet GetUserLogin( string userAccount)
        {
            System.Data.SqlClient.SqlDataAdapter sdaTemp = null;
            System.Data.DataSet dsTemp = null;
            try
            {
                dsTemp = new DataSet();
                //연결정보가져오기
                sdaTemp = new SqlDataAdapter("eManage.DBO.USP_SELECT_LOGIN_CHECK", DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"));
                sdaTemp.SelectCommand.CommandType = CommandType.StoredProcedure;
  
                sdaTemp.SelectCommand.Parameters.AddWithValue("@NVCUSERACCOUNT", userAccount);
                sdaTemp.Fill(dsTemp);
                sdaTemp.SelectCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sdaTemp != null) sdaTemp.Dispose();
            }
            return dsTemp;
        }

        #endregion
 
	}
}
