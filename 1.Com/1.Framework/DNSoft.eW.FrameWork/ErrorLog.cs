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
// �ּ� �߰� - nettalk@dotnetxpert.com
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
	/// ���� �α׸� ����Ѵ�. �����߻��� ErrorLog Ŭ������ �̺�Ʈ�α׿� ����Ѵ�. 
	/// logFilePath ������ ���� ������ ������ �����Ѵ�. 
	/// </remark>
	public class ErrorLog : System.ComponentModel.Component
	{
		private const string DEFAULT_LOGNAME = "Application";
		private const string DEFAULT_SOURCENAME = "Application Error";
		private const string DESC_LOGNAME = "NT �̺�Ʈ�α��� �̸��� �����Ѵ�.";

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
		/// ErrorLog ������
		/// </summary>
		/// <param name="container"></param>
		public ErrorLog(System.ComponentModel.IContainer container)
		{
			initializeClass();
			container.Add(this);
			InitializeComponent();
		}
 
		/// <summary>
		/// ErrorLog Ŭ���� �����ϰ�, logName = "Application", sourceName = "Application Error" �� �����ȴ�.
		/// </summary>
		public ErrorLog()
		{
			initializeClass();
			InitializeComponent();
		}

		/// <summary>
		/// ErrorLog Ŭ���� ������ ���ÿ� �ʱ�ȭ �Ѵ�. 
		/// </summary>
		/// <param name="sourceName">�ҽ��̸�</param>
		/// <param name="logName">�α��̸�</param>
		/// <param name="logFilePath">�α� ���� �̸� �Ǵ� Null</param>
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
		/// �̺�Ʈ ��ϵǴ� �α� ����Ʈ �̸�����
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
		/// �̺�Ʈ ��ϵǴ� �α׵��� �ҽ�(����) �̸�����
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
				// �̺�Ʈ �ҽ��� ��ϵ��� �ʾ����� �̺�Ʈ�ҽ��� �����ϰ� ����Ѵ�.
				if (EventLog.SourceExists(value) == false)
					EventLog.CreateEventSource(value, LogName);

				_sourceName = value;
			}
		}

		/// <value>
		/// �α����� �̸��� �����ϸ� �α� �޼��� ����� ������ �̺�Ʈ����� ���Ͽ� ��ϵȴ�.
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
		/// �α׸޼����� �ۼ��ϰ� �̺�Ʈ �α׿� �߰��Ѵ�. ���� LogFilePath �� �����Ǿ� ������ 
		/// ������ ���Ͽ��� ������ �޼����� ����Ѵ�. 
		/// �⺻������ �̺�Ʈ�α� ����� �����з�(error) �ν� ���
		/// </summary>
		/// <param name="logMessage">�α� �޼���</param>
		public void WriteLogMessage(string logMessage)
		{
			WriteLogMessage(logMessage, EventLogEntryType.Error );
		}

		/// <summary>
		/// �α׸޼����� �ۼ��ϰ� �̺�Ʈ �α׿� �߰��Ѵ�. ���� LogFilePath �� �����Ǿ� ������ 
		/// ������ ���Ͽ��� ������ �޼����� ����Ѵ�. 
		/// </summary>
		/// <param name="logMessage">�α� �޼���</param>
		/// <param name="entryType">�̺�Ʈ�α� ��Ͻ� �����з�(Infomation, warning, error)</param>
		public void WriteLogMessage(string logMessage, EventLogEntryType entryType)
		{
			Type type = this.GetType();

			try
			{
				// Ŭ������ ��ٴ�.
				Monitor.Enter(type);

				EventLog eventLog = new EventLog(); // �̺�Ʈ�α� ��ü�� �����Ѵ�.
				eventLog.Source = SourceName;
				eventLog.WriteEntry(logMessage, entryType);

				if ( LogFilePath != null )
				{
					string msg = "";

					// ȭ�Ͻ�Ʈ�� ��ü�� �����Ѵ�. ȭ���� �������� ������ ���� ����� �б⾲�� ���� ����.
					FileStream fs 
						= new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, 
						FileShare.Write);

					// stream writer ��ü�� �����. 
					StreamWriter logf = new StreamWriter(fs);
					msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]"
						+ "-" + logMessage + "\r\n";

					//msg += "Log Entry : \r\n\r\n";
					//msg += DateTime.Now.ToLongDateString() + " ";
					//msg += DateTime.Now.ToLongTimeString() + " \r\n\r\n";
					//msg += logMessage;
					//msg += "\r\n-----------------------------------------------------------------------------------\r\n\r\n";

					logf.Write(msg); // �����޽����� �߰��Ѵ�.

					logf.Flush();             // ���۸� ����.
					logf.Close();             // ȭ���� �ݴ´�.
				}
			}
			catch
			{
				//���� ����
			}
			finally
			{
				// Ŭ���� ��ü ����� �����Ѵ�.
				Monitor.Exit(type);
			}
		}

		/// <summary>
		/// ����ó���� �߻��ϴ� �޼����� ���͸��ؼ� ����ڿ��� ģ���� �޼����� �����ش�.
		/// </summary>
		/// <param name="ex">���� ����</param>
		/// <param name="filtering">���͸� ����(true/false)</param>
		/// <returns>���˵� ��� ���ܸ޼���</returns>
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
		/// Exception ������ �α׿� ����Ѵ�. 
		/// ����ڰ� ������ �޽����� �ι��� �Ķ���ͷ� �߰��� ��ϵȴ�.
		/// </summary>
		/// <param name="ex">���� ����</param>
		public string WriteExceptionLog(Exception ex)
		{
			return WriteExceptionLog(ex, null);
		}

		/// <summary>
		/// Exception ������ �α׿� ����Ѵ�. 
		/// ����ڰ� ������ �޽����� �ι��� �Ķ���ͷ� �߰��� ��ϵȴ�.
		/// </summary>
		/// <param name="ex">���� ����</param>
		/// <param name="logMessage">����� ���� �޼���</param>
		/// <returns>��ü ���� �޼���</returns>
		public string WriteExceptionLog(Exception ex, string logMessage)
		{
			string errorMessage = null;

			if ( ex == null )
				return null;

			if ( logMessage != null )
				errorMessage = logMessage + "\r\n";
			errorMessage += Environment.GetEnvironmentVariable("COMPUTERNAME");
			errorMessage += " �� " + ex.Source + " ���� ������ �߻��Ͽ����ϴ�.\r\n\r\n";
			errorMessage += ex.ToString();

			WriteLogMessage(errorMessage);
			return errorMessage;
		}

		/// <summary>
		/// ��ü �ڿ� ���� 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
