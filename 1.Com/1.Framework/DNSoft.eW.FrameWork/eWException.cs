using System;
using System.Reflection;

namespace DNSoft.eW.FrameWork
{
	/// <summary>
    /// <b>예외 처리 메소드 집합이다.</b>
	/// </summary>	
	public class eWException  : eWBase
	{
        /// <summary>
        /// <b> eWexception 생성자</b>
        /// </summary>
		public eWException(){}  

		#region HandleDALException
		/// <summary>
        /// <b>Data Access Layer 예외 처리 공통 메소드</b>
		/// </summary>
		/// <param name="sub">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		public static Exception HandleDALException(SubSystemType sub, System.Exception ex, Type ObjType)
		{
			return HandleDALException(sub, ex,ObjType, "#");
		} 

		/// <summary>
        /// <b>DAL영역의 예외처리</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		/// <returns>throw Exception</returns>
		public static Exception HandleDALException(SubSystemType sub, System.Exception ex, Type ObjType, string errorCode)
		{
			if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException"))//SQL관련 Exception
			{
				HandleException (sub, LayerType.SQL, "#", ex, ObjType);
			} // 그 외의 다른 Error 처리 부문
			else
			{	// 새로운 예외를 발생		
				HandleException (sub, LayerType.DAL, "#", ex, ObjType);
			}
			return null;
		}		
		#endregion

		#region HandleBSLException
		/// <summary>
        /// <b>Business Service Layer 예외 처리 공통 메소드</b>
		/// </summary>
		/// <param name="sub">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		public static void HandleBSLException (SubSystemType sub, System.Exception ex,Type ObjType )
		{
			HandleException (sub, LayerType.BSL, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>MQ Service Layer 예외 처리 공통 메소드</b>
		/// </summary>
		/// <param name="sub">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleBSLException (SubSystemType sub, System.Exception ex,Type ObjType , string errorCode)
		{
			HandleException (sub, LayerType.MQ, errorCode, ex, ObjType);
		}
		#endregion

		#region HandleMQException
		/// <summary>
        /// <b>MQ Service Layer 예외 처리 공통 메소드</b>
		/// </summary>
		/// <param name="sub">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		public static void HandleMQException (SubSystemType sub, System.Exception ex,Type ObjType )
		{
			HandleException (sub, LayerType.MQ, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>Business Service Layer 예외 처리 공통 메소드</b>
		/// </summary>
		/// <param name="sub">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleMQException (SubSystemType sub, System.Exception ex,Type ObjType , string errorCode)
		{
			HandleException (sub, LayerType.MQ, errorCode, ex, ObjType);
		}
		#endregion

		#region HandleEAEXCHException
		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="Biz">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="methodName">메소드명</param>
		public static void HandleEAEXCHException (SubSystemType sub, System.Exception ex, Type ObjType)
		{
			HandleException (sub, LayerType.EXCH, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleEAEXCHException (SubSystemType sub, System.Exception ex, Type ObjType, string errorCode)
		{
			HandleException (sub, LayerType.EXCH, errorCode, ex, ObjType);
		}
		#endregion

		#region HandleEAADException
		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="Biz">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="methodName">메소드명</param>
		public static void HandleEAADException (SubSystemType sub, System.Exception ex, Type ObjType)
		{
			HandleException (sub, LayerType.AD, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleEAADException (SubSystemType sub, System.Exception ex, Type ObjType, string errorCode)
		{
			HandleException (sub, LayerType.AD, errorCode, ex, ObjType);
		}
		#endregion

		#region HandleEASPSException
		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="Biz">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="methodName">메소드명</param>
		public static void HandleEASPSException (SubSystemType sub, System.Exception ex, Type ObjType)
		{
			HandleException (sub, LayerType.SPS, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleEASPSException (SubSystemType sub, System.Exception ex, Type ObjType, string errorCode)
		{
			HandleException (sub, LayerType.SPS, errorCode, ex, ObjType);
		}
		#endregion

		#region HandleDSLException
		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="Biz">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="methodName">메소드명</param>
		public static void HandleDSLException (SubSystemType sub, System.Exception ex, Type ObjType)
		{
			HandleException (sub, LayerType.DSL, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleDSLException (SubSystemType sub, System.Exception ex, Type ObjType, string errorCode)
		{
			HandleException (sub, LayerType.DSL, errorCode, ex, ObjType);
		}
		#endregion

		#region HandleWINException
		/// <summary>
        /// <b>Win Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="Biz">업무구분</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="methodName">메소드명</param>
		public static void HandleWINException (SubSystemType sub, System.Exception ex, Type ObjType)
		{
			HandleException (sub, LayerType.WIN, "#", ex, ObjType);
		}

		/// <summary>
        /// <b>Data Service Layer 예외 처리를 위한 메소드</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">오브젝트타입</param>
		/// <param name="errorCode">에러코드</param>
		public static void HandleWINException (SubSystemType sub, System.Exception ex, Type ObjType, string errorCode)
		{
			HandleException (sub, LayerType.WIN, errorCode, ex, ObjType);
		}
		#endregion

		#region WriteLog
		/// <summary>
        /// <b>Exception에 대한 로그를 기록한다.</b>
		/// </summary>
		/// <param name="SystemName">서브시스템이름</param>
		/// <param name="Layer">Layer타입</param>
		/// <param name="ex">예외개체</param>
		protected static Exception WriteLog(string SystemName, string Layer, System.Exception ex)
		{
			return LogHandler.Publish(SystemName,Layer, ex);
		}  //로그
		#endregion

		#region HandleException Exception처리공통
		/// <summary>
        /// <b>예외 일반 또는 UserException 처리를 위한 Protected 메소드</b>
		/// </summary>
		/// <param name="sub">서브시스템타입</param>
		/// <param name="Layer">Layer타입</param>
		/// <param name="Errcode">에러코드</param>
		/// <param name="ex">예외개체</param>
		/// <param name="ObjType">클래스타입</param>
		protected static void HandleException(SubSystemType sub, LayerType Layer, string Errcode, System.Exception ex, Type ObjType)
		{
			string subSystemName = ReturnFileName(sub);
			string layerName = GetLayerString(Layer);
			string strTemp = ex.ToString();
			int nTemp = strTemp.IndexOf("eW System Error Tracing");
			if(nTemp == -1 || Layer == LayerType.MQ)
				throw WriteLog(subSystemName, layerName, ex);
			else
				throw new Exception(ex.Message, ex);
		}
		#endregion
		
		#region 메소드명 뜯어오는 지역클래스
		/// <summary>
        /// <b>System.Type에서 메소드을 가져오기 위한 클래스</b>
		/// </summary>
		public class  ClsType
		{
			public ClsType(){}

			/// <summary>
			/// 목적 : Type을 통하여 현재의 메소드명 가져오기
			/// </summary>
			/// <param name="aType">Type명</param>
			/// <returns>메소드명</returns>
			public string  GetMethodName (Type aType)
			{
				try
				{
					
					MethodInfo[] mInfo = aType.GetMethods();
					for ( int i=0; i < mInfo.Length; i++ )
					{        
						if ( mInfo[i].DeclaringType == aType && !mInfo[i].IsSpecialName  &&	(this.GetType().ToString() != aType.ToString()))
						{         
							return  mInfo[i].Name.ToString();
						}
					}
					return "";
				}
				catch(Exception ex)
				{
					throw ex;
				}
			}
		}
		#endregion
	}
}
