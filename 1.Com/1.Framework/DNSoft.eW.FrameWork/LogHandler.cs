using System;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DNSoft.eW.FrameWork
{
	/// <summary>
    /// <b>로그핸들러</b>
	/// </summary>	
	public class LogHandler : eWBase
	{
		public LogHandler(){}

		#region 로깅설정
		/// <summary>
        /// <b>공통로깅레벨설정</b>
		/// </summary>
		public static bool LogStandard
		{
			get
			{
				string strTemp = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/SystemLogLevel");
				switch(strTemp)
				{
					case "0": return false;
					case "1": return true;
					case "2": return true;
					case "3": return false;
					default: return false;
				}
			}
		}

		/// <summary>
        /// <b>서브팀로깅레벨설정</b>
		/// </summary>
		public static bool LogSubSystem
		{
			get
			{
				string strTemp = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/SystemLogLevel");
				switch(strTemp)
				{
					case "0": return false;
					case "1": return true;
					case "2": return false;
					case "3": return true;
					default: return false;
				}
			}
		}
		#endregion
	
		#region Publish Exception기록
		/// <summary>
        /// <b>Exception기록</b>
		/// </summary>
		/// <param name="SystemName">서브시스템명</param>
		/// <param name="Layer">레이어명</param>
		/// <param name="exception">Exception</param>
		public static Exception Publish(string SystemName,string Layer, System.Exception exception)
		{
			string tmp = string.Empty;
			StringBuilder msg = null;
			StackTrace st1 = null;
			StackTrace st2 = null;
			StackFrame sf = null;
			try
			{
				//Stack과 Exception을 하나의 msg에 담는다.
				msg = new StringBuilder();
				//Stack트래이싱하는부분
				st1 = new StackTrace(1, true);
				msg.Append("==============[eW System Error Tracing]==============\r\n");
				msg.Append("[CallStackTrace]\r\n");
				for(int i =0; i< st1.FrameCount; i++ )
				{
					sf = st1.GetFrame(i);
					tmp = sf.GetMethod().DeclaringType.FullName + "." + sf.GetMethod().Name;
					if(tmp.IndexOf("System") != 0 && tmp.IndexOf("Microsoft") != 0 )
					{
						msg.Append(tmp);
						msg.Append(" : ("+sf.GetFileLineNumber()+")");
						msg.Append("\r\n");
					}
				}

				//Exception을 트래이싱하는부분
				st2 = new StackTrace(exception, true);
				msg.Append("\r\n[ErrStackTrace]\r\n");
				for(int i =0; i< st2.FrameCount; i++ )
				{
					sf = st2.GetFrame(i);
					tmp = sf.GetMethod().DeclaringType.FullName+"."+sf.GetMethod().Name;
					if(tmp.IndexOf("System") != 0 && tmp.IndexOf("Microsoft") != 0 )
					{
						msg.Append(tmp);
						msg.Append(" : ("+sf.GetFileLineNumber()+")");
						msg.Append("\r\n");
					}
				}

				//msg에 현재 트래이싱된 시간을 담도록 한다.
				msg.Append("\r\n[DateTime] : "+DateTime.Now.ToString("yyyy-MM-dd") +"\r\n");

				//만약 Sql관련 Exception이면 추가항목을 넣어주도록 한다.
				if (exception.GetType() == typeof(System.Data.SqlClient.SqlException))
				{
					SqlException sqlErr = (SqlException)exception;
					msg.Append("\r\n[SqlException] ");
					msg.Append("\r\nException Type: ").Append(sqlErr.GetType());
					msg.Append("\r\nErrors: ").Append(sqlErr.Errors);
					msg.Append("\r\nClass: ").Append(sqlErr.Class);
					msg.Append("\r\nLineNumber: ").Append(sqlErr.LineNumber);
					msg.Append("\r\nMessage: ").Append("{" + sqlErr.Message + "}");
					msg.Append("\r\nNumber: ").Append(sqlErr.Number);
					msg.Append("\r\nProcedure: ").Append(sqlErr.Procedure);
					msg.Append("\r\nServer: ").Append(sqlErr.Server);
					msg.Append("\r\nState: ").Append(sqlErr.State);
					msg.Append("\r\nSource: ").Append(sqlErr.Source);
					msg.Append("\r\nTargetSite: ").Append(sqlErr.TargetSite);
					msg.Append("\r\nHelpLink: ").Append(sqlErr.HelpLink);
				}
					//Sql관련Exception외에 작업들..
				else
				{
					msg.Append("\r\n[Exception] ");
					msg.Append("\r\n" + "DetailMsg: {" + exception.Message + "}");
				}
				// Create the source, if it does not already exist.
				WriteLog(msg.ToString(),SystemName,Layer);
			}
			catch(Exception)
			{
				//이넘은 throw하면 안된다, 이넘 자체가 Exception을 처리하는 넘이기때문에...
			}
			return new Exception(msg.ToString());
		}

		/// <summary>
        /// <b>Exception기록</b>
		/// </summary>
		/// <param name="systemName">서브시스템명</param>
		/// <param name="layer">레이어명</param>
		/// <param name="message">메세지</param>
		/// <param name="eventId">이벤트아이디</param>
		/// <param name="category">카테고리</param>
//        public static void Publish(string systemName,string layer, string message, int eventId, short category)
//		{
//			StringBuilder stb = new StringBuilder();
//			stb.Append("\r\n[EventInfo]:");
//			stb.Append("\r\nEventid:").Append( eventId );
//			stb.Append("\tCategory:").Append( category );
//
//			stb.Append("\r\n\r\n[Information]:");
//			stb.Append("\r\nBiz:").Append( systemName);
//			stb.Append("\tLayer:").Append( layer );
//			stb.Append("\r\nMessage:");
//			stb.Append("\r\n").Append( message);
//
//			WriteLog(stb.ToString(), systemName, layer, eventId, category);
//		}
		#endregion

		#region WriteLog 실제로그기록
		/// <summary>
        /// <b>실제 이벤트로그기록</b>
		/// </summary>
		/// <param name="sMessage">에러메세지</param>
		/// <param name="sSubSystem">서브시스템명</param>
		/// <param name="sLayer">레이어명</param>
		private static void WriteLog(string sMessage,string sSubSystem,string sLayer)
		{
			string sSource = string.Empty;
			EventLog Log = null;
			try
			{
				sSubSystem = sSubSystem.Substring(0,4);
				sSubSystem = sSubSystem.Replace(".","");
				sSource = sSubSystem + "." + sLayer; //이벤트로그명 결정
				if (!EventLog.SourceExists(sSource))
				{
					EventLog.CreateEventSource(sSource,sSource); //없으면 만든다.
				}
								
				// Inserting into event log
				Log = new EventLog();
				Log.Source = sSource;
				Log.WriteEntry(sMessage, EventLogEntryType.Error);		
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				Log.Dispose();
				Log.Close();
			}
		}

		#endregion
	}
}
