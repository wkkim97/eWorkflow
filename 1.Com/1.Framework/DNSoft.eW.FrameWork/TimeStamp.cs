using System;
using System.IO;

namespace DNSoft.eW.FrameWork
{
	/// <summary>
    /// <b>파일시스템에 TimeStamp를 로그로 작성관리한다.</b>
	/// </summary>
	public class TimeStamp : eWBase
	{
		public TimeStamp() {}	
		private DateTime startTime;

		#region TimeStampStart
		/// <summary>
        /// <b>시작시간담기</b>
		/// </summary>
		public void TimeStampStart()
		{
			startTime = DateTime.Now;
		}	
		#endregion

		#region TimeStampEnd
		/// <summary>
        /// <b>사용자,네임스페이스,서비스네임등등 종료로그찍기</b>
		/// </summary>
		/// <param name="target">오브젝트</param>
		/// <param name="sServiceName">서비스네임</param>
		public void TimeStampEnd(object target,string sServiceName)
		{
			string sLogDir = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/TimeLogPath");
			DateTime endTime = DateTime.Now;
			TimeSpan timeSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
			string sFullPath = string.Format(@"{0}\{1} TimeLog.txt",sLogDir,DateTime.Now.ToString("yyyy-MM-dd")	);
	
			FileStream fs = null;
			StreamWriter sWriter = null;

			try
			{	
				string strUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;	
				string strNamespace = target.GetType().Namespace;
				if(strNamespace.ToUpper().Equals("ASP")) strNamespace = target.GetType().BaseType.ToString(); //웹인경우에는 BaseType에서 Namespace를 뽑는다.

				string sLogInfo = startTime.ToString() + "." + startTime.Millisecond.ToString() + " | " + strUserName + "|" + strNamespace + " | " + target.GetType().Name + " | " + sServiceName + " | " +timeSpan.TotalMilliseconds.ToString();
				if (!Directory.Exists(sLogDir))
					Directory.CreateDirectory(sLogDir);
				
				fs = new FileStream(sFullPath,FileMode.Append,FileAccess.Write,FileShare.Write);		
				sWriter = new StreamWriter(fs,System.Text.Encoding.Default);
				sWriter.WriteLine(sLogInfo);
			}
			catch(Exception ex)
			{
				System.Diagnostics.EventLog.WriteEntry("TimeStamp",ex.ToString(),System.Diagnostics.EventLogEntryType.Error);
			} 	
			finally
			{
				if (sWriter != null) sWriter.Close();
				if (fs != null) fs.Close();
			}
		}

		/// <summary>
        /// <b>사용자,네임스페이스,서비스네임등등 종료로그찍기</b>
		/// </summary>
		/// <param name="target">오브젝트</param>
		/// <param name="sServiceName">서비스네임</param>
		/// <param name="strSPName">SP네임</param>
		/// <param name="pack">SQL파라미터</param>
		public void TimeStampEnd(object target,string sServiceName,string strSPName,System.Data.SqlClient.SqlParameter[] pack)
		{
			string sLogDir = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/TimeLogPath");
			DateTime endTime = DateTime.Now;
			TimeSpan timeSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
			string sFullPath = string.Format(@"{0}\{1} TimeLog.txt",sLogDir,DateTime.Now.ToString("yyyy-MM-dd")	);
			//string sFullPath = sLogDir + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + " TimeLog.txt";
	
			FileStream fs = null;
			StreamWriter sWriter = null;
			string strParameters = "";
			try
			{		
				foreach (System.Data.SqlClient.SqlParameter p in pack)
				{					
					if (p.Value != null)
					{
						strParameters += p.ParameterName + "=" + p.Value.ToString() + ",";						
					}
					else
					{
						strParameters += p.ParameterName + "=" +",";					
					}
				}

				if (strParameters.Length > 0)
					strParameters = strParameters.Remove(strParameters.Length-1,1);
				string strUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;	
				string strNamespace = target.GetType().Namespace;
				if(strNamespace.ToUpper().Equals("ASP")) strNamespace = target.GetType().BaseType.ToString(); //웹인경우에는 BaseType에서 Namespace를 뽑는다.
				
				string sLogInfo = startTime.ToString() + "." + startTime.Millisecond.ToString() + " | " + strUserName + "|" + strNamespace + " | " + target.GetType().Name + " | " + sServiceName + " | " + strSPName + " | " + strParameters + " | " + timeSpan.TotalMilliseconds.ToString();

				
				if (!Directory.Exists(sLogDir))
					Directory.CreateDirectory(sLogDir);
				
				fs = new FileStream(sFullPath,FileMode.Append,FileAccess.Write,FileShare.Write);		
				sWriter = new StreamWriter(fs,System.Text.Encoding.Default);

				sWriter.WriteLine(sLogInfo);
				
			}
			catch(Exception ex)
			{
				System.Diagnostics.EventLog.WriteEntry("TimeStamp",ex.ToString(),System.Diagnostics.EventLogEntryType.Error);
			} 	
			finally
			{
				if (sWriter != null)
					sWriter.Close();
				if (fs != null)
					fs.Close();
			}
		}		
		#endregion
	}
}
