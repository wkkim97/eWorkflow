//=======================================================================
//   .netXpert ErrorLog Component Ver 1.1
//
//   DNSoft.eW.FrameWork.Diagnostics.ErrorLog
// 
//=======================================================================
//  Written By Puzzle ( puzzle@dotnetXpert.com )
//  
//  2008. 10. 01
//=======================================================================
// 주석 추가 - nettalk@dotnetxpert.com
// 2009.08.23 
//=======================================================================

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Threading;

namespace DNSoft.eW.FrameWork.Diagnostics
{
	/// <remark>
	/// 에러 로그를 요약한다. 에러발생시 ErrorLog 클래스는 이벤트로그에 기록한다. 
	/// logFilePath 유무에 따라서 동일한 정보를 저장한다. 
	/// </remark>
	public class ErrorLog : System.ComponentModel.Component
	{
		private const string DEFAULT_LOGNAME = "Application";
		private const string DEFAULT_SOURCENAME = "Application Error";
		private const string DESC_LOGNAME = "NT 이벤트로그의 이름을 지정한다.";

		private string _logName;
		private string _sourceName;
		private string _logFilePath;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private void initializeClass()
		{
			_logName = DEFAULT_LOGNAME;
			_sourceName = DEFAULT_SOURCENAME;
			_logFilePath = null;
		}

		/// <summary>
		/// ErrorLog 생성자
		/// </summary>
		/// <param name="container"></param>
		public ErrorLog(System.ComponentModel.IContainer container)
		{
			initializeClass();
			container.Add(this);
			InitializeComponent();
		}
 
		/// <summary>
		/// ErrorLog 클래스 생성하고, logName = "Application", sourceName = "Application Error" 로 지정된다.
		/// </summary>
		public ErrorLog()
		{
			initializeClass();
			InitializeComponent();
		}

		/// <summary>
		/// ErrorLog 클래스 생성과 동시에 초기화 한다. 
		/// </summary>
		/// <param name="sourceName">소스이름</param>
		/// <param name="logName">로그이름</param>
		/// <param name="logFilePath">로그 파일 이름 또는 Null</param>
		public ErrorLog(string sourceName, string logName, string logFilePath)
		{
			LogName = logName;
			SourceName = sourceName;
			LogFilePath = logFilePath;

			InitializeComponent();
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		/// <value>
		/// 이벤트 기록되는 로그 리스트 이름지정
		/// </value>
		[Description(DESC_LOGNAME)]
		[Category("Data")]
		[Browsable(true)]
		public string LogName
		{
			get
			{
				return _logName;
			}
			set
			{
				if ( value != null && value.Length < 1)
				{
					value = DEFAULT_LOGNAME;
				}
				_logName = value;
			}
		}


		/// <value>
		/// 이벤트 기록되는 로그들이 소스(원본) 이름지정
		/// </value>
		[Description("")]
		[Category("Data")]
		[Browsable(true)]
		public string SourceName
		{
			get
			{
				return _sourceName;
			}
			set
			{
				if ( value != null && value.Length < 1)
				{
					value = DEFAULT_SOURCENAME;
				}
				// 이벤트 소스가 등록되지 않았으면 이벤트소스를 생성하고 등록한다.
				if (EventLog.SourceExists(value) == false)
					EventLog.CreateEventSource(value, LogName);

				_sourceName = value;
			}
		}

		/// <value>
		/// 로그파일 이름을 지정하면 로그 메세지 저장시 동일한 이벤트기록이 파일에 기록된다.
		/// </value>
		[Description("")]
		[Category("Data")]
		[Browsable(true)]
		[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design", 
			 typeof(UITypeEditor))]
		public string LogFilePath
		{
			get
			{
				return _logFilePath;
			}
			set
			{
				if ( value != null && value.Length < 1)
				{
					value = null;
				}
				_logFilePath = value;
			}
		}

		/// <summary>
		/// 로그메세지를 작성하고 이벤트 로그에 추가한다. 만일 LogFilePath 가 지정되어 있으면 
		/// 지정된 파일에도 동일한 메세지를 기록한다. 
		/// 기본적으로 이벤트로그 기록은 정보분류(error) 로써 기록
		/// </summary>
		/// <param name="logMessage">로그 메세지</param>
		public void WriteLogMessage(string logMessage)
		{
			WriteLogMessage(logMessage, EventLogEntryType.Error );
		}

		/// <summary>
		/// 로그메세지를 작성하고 이벤트 로그에 추가한다. 만일 LogFilePath 가 지정되어 있으면 
		/// 지정된 파일에도 동일한 메세지를 기록한다. 
		/// </summary>
		/// <param name="logMessage">로그 메세지</param>
		/// <param name="entryType">이벤트로그 기록시 정보분류(Infomation, warning, error)</param>
		public void WriteLogMessage(string logMessage, EventLogEntryType entryType)
		{
			Type type = this.GetType();

			try
			{
				// 클래스를 잠근다.
				Monitor.Enter(type);

				EventLog eventLog = new EventLog(); // 이벤트로그 객체를 생성한다.
				eventLog.Source = SourceName;
				eventLog.WriteEntry(logMessage, entryType);

				if ( LogFilePath != null )
				{
					string msg = "";

					// 화일스트림 객체를 생성한다. 화일이 존재하지 않으면 새로 만들고 읽기쓰기 모드로 연다.
					FileStream fs 
						= new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, 
						FileShare.Write);

					// stream writer 객체를 만든다. 
					StreamWriter logf = new StreamWriter(fs);
					msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]"
						+ "-" + logMessage + "\r\n";

					//msg += "Log Entry : \r\n\r\n";
					//msg += DateTime.Now.ToLongDateString() + " ";
					//msg += DateTime.Now.ToLongTimeString() + " \r\n\r\n";
					//msg += logMessage;
					//msg += "\r\n-----------------------------------------------------------------------------------\r\n\r\n";

					logf.Write(msg); // 에러메시지를 추가한다.

					logf.Flush();             // 버퍼를 비운다.
					logf.Close();             // 화일을 닫는다.
				}
			}
			catch
			{
				//에러 무시
			}
			finally
			{
				// 클래스 객체 잠금을 해제한다.
				Monitor.Exit(type);
			}
		}

		/// <summary>
		/// 예외처리시 발생하는 메세지를 필터링해서 사용자에게 친숙한 메세지를 보여준다.
		/// </summary>
		/// <param name="ex">예외 정보</param>
		/// <param name="filtering">필터링 여부(true/false)</param>
		/// <returns>포맷된 결과 예외메세지</returns>
		public string FormatException(Exception ex, bool filtering)
		{
			StringBuilder sb = new StringBuilder();

			if ( filtering == false )
			{
				sb.Append("ErrorMessage : ");
				sb.Append(ex.Message);
				sb.Append("\r\n");
				sb.Append(ex.StackTrace);
				return sb.ToString();
			}

			Exception ex1 = ex;
			while(ex1 != null)
			{
				sb.Append("ErrorMessage : ");
				sb.Append(ex1.Message);
				sb.Append("\r\n");

				StringReader reader = new StringReader(ex1.StackTrace);
				string line;
				while( (line = reader.ReadLine()) != null )
				{
					if ( line.IndexOf("   at System") != 0 )
					{
						sb.Append(line).Append("\r\n");
					}
				}

				sb.Append("-----------------------------------------------------\r\n");
				ex1 = ex1.InnerException;
			}

			return sb.ToString();
		}


		/// <summary>
		/// Exception 정보를 로그에 기록한다. 
		/// 사용자가 지정한 메시지를 두번쨰 파라미터로 추가로 기록된다.
		/// </summary>
		/// <param name="ex">예외 정보</param>
		public string WriteExceptionLog(Exception ex)
		{
			return WriteExceptionLog(ex, null);
		}

		/// <summary>
		/// Exception 정보를 로그에 기록한다. 
		/// 사용자가 지정한 메시지를 두번쨰 파라미터로 추가로 기록된다.
		/// </summary>
		/// <param name="ex">예외 정보</param>
		/// <param name="logMessage">사용자 지정 메세지</param>
		/// <returns>전체 에러 메세지</returns>
		public string WriteExceptionLog(Exception ex, string logMessage)
		{
			string errorMessage = null;

			if ( ex == null )
				return null;

			if ( logMessage != null )
				errorMessage = logMessage + "\r\n";
			errorMessage += Environment.GetEnvironmentVariable("COMPUTERNAME");
			errorMessage += " 의 " + ex.Source + " 에서 에러가 발생하였습니다.\r\n\r\n";
			errorMessage += ex.ToString();

			WriteLogMessage(errorMessage);
			return errorMessage;
		}

		/// <summary>
		/// 객체 자원 해제 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
