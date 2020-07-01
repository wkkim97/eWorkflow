using System;
using System.IO;

namespace DNSoft.eW.FrameWork.Common
{
	/// <summary>
	/// <b>LogManager에 대한 요약 설명입니다.</b><br/>
    /// </summary>
    /// <remarks>
	/// - 작  성  자 : 인터데브 유승호<br/>
	/// - 최초작성일 : 2009년 01월 16일<br/>
	/// - 최종수정자 : 유승호<br/>
	/// - 최종수정일 : 2009년 01월 16일<br/>
	/// - 주요변경로그<br/>
	/// 2009.01.16 생성<br/>
    /// </remarks>
	public class LogManager
	{
		private LogManager()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//
		}

		/// <summary>
		/// <b>로그기록</b><br/>
        /// </summary>
        /// <remarks>
		/// - 작  성  자 : 인터데브 유승호<br/>
		/// - 최초작성일 : 2009년 01월 16일<br/>
		/// - 최종수정자 : 유승호<br/>
		/// - 최종수정일 : 2009년 01월 16일<br/>
		/// - 주요변경로그<br/>
		/// 2009.01.16 생성<br/>
        /// </remarks>
		/// <param name="type">개체타입</param>
		/// <param name="serviceName">서비스명</param>
		/// <param name="keys">키값</param>
		/// <param name="data">테이타</param>
		/// <param name="desc">Description</param>
		/// <returns>결과(성공:true,실패:false)</returns>
		public static bool WriteLog(Type type, string serviceName, string[] keys, string data, string desc)
		{
			bool bReturn = false;
			System.IO.FileStream oFs = null;
			System.IO.StreamWriter oWriter = null;

			string strProcName = "";
			string strTime = "";
			string[] strNameSpaces = null;
			string fPath = "";
			string strKey = "";

			try
			{
				strNameSpaces = type.Namespace.Split(new char[1]{'.'});

				strProcName = AppDomain.CurrentDomain.FriendlyName;
				strTime = string.Format("{0}-{1}-{2}", DateTime.Now.Year.ToString(),
					DateTime.Now.Month.ToString(),
					DateTime.Now.Day.ToString());

				fPath = string.Format(@"{0}\{1}\", @"D:\TimeLog", strProcName);
				//				if ( strNameSpaces.Length > 4 )			
				//					fPath = string.Format(@"{0}\SO_{1}_Task_Log\", @"D:\TimeLog", strNameSpaces[3]);
				//				else
				//					fPath = string.Format(@"{0}\{1}\", @"D:\TimeLog", "eManage_All_Task_Log");

				if ( !System.IO.Directory.Exists(fPath) )
					System.IO.Directory.CreateDirectory(fPath);

				fPath += strTime + ".txt";

				oFs = new System.IO.FileStream(fPath, FileMode.Append,FileAccess.Write,FileShare.Write);
				oWriter = new System.IO.StreamWriter(oFs, System.Text.Encoding.Default);

				// Key 값 만들기
				if (keys != null)
				{
					for (int i = 0; i < keys.Length; i++)
					{
						strKey += keys[i] + "&";
					}

					if (strKey.Length > 0)
					{
						strKey = strKey.Substring(0, strKey.Length - 1);
					}
				}

				oWriter.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
					DateTime.Now.ToString("yyyy-MM-dd HHmmss"),
					type.Namespace,
					type.Name,
					serviceName,
					strKey,
					data,
					desc));
				
				bReturn = true;
			}
			catch
			{
				bReturn = false;
			}
			finally
			{
				if ( oWriter != null )
				{
					oWriter.Flush();
					oWriter.Close();
				}
				if ( oFs != null )
				{
					oFs.Close();
				}
			}
			return bReturn;
		}

		/// <summary>
		/// <b>로그기록</b><br/>
        /// </summary>
        /// <remarks>
		/// - 작  성  자 : 인터데브 유승호<br/>
		/// - 최초작성일 : 2009년 01월 16일<br/>
		/// - 최종수정자 : 유승호<br/>
		/// - 최종수정일 : 2009년 01월 16일<br/>
		/// - 주요변경로그<br/>
		/// 2009.01.16 생성<br/>
        /// </remarks>
		/// <param name="prjName">프로젝트이름</param>
		/// <param name="className">클래스이름</param>
		/// <param name="serviceName">서비스명</param>
		/// <param name="keys">키값</param>
		/// <param name="data">데이타</param>
		/// <param name="desc">Description</param>
		/// <returns>결과(성공:true,실패:false)</returns>
		public static bool WriteLog(string prjName, string className, string serviceName, string[] keys, string data, string desc)
		{
			bool bReturn = false;
			System.IO.FileStream oFs = null;
			System.IO.StreamWriter oWriter = null;

			string[] strNameSpaces = null;
			string fPath = "";
			string strKey = "";

			try
			{
				strNameSpaces = prjName.Split(new char[1]{'.'});

				if ( strNameSpaces.Length > 4 )			
					fPath = string.Format(@"{0}\Common_{1}_Task_Log\", @"D:\TimeLog", strNameSpaces[3]);
				else
					fPath = string.Format(@"{0}\{1}\", @"D:\TimeLog", "Common_All_Task_Log");

				if ( !System.IO.Directory.Exists(fPath) )
					System.IO.Directory.CreateDirectory(fPath);

				if ( !System.IO.Directory.Exists(fPath) )
					System.IO.Directory.CreateDirectory(fPath);

				fPath += "TaskLog.txt";

				oFs = new System.IO.FileStream(fPath, FileMode.Append,FileAccess.Write,FileShare.Write);
				oWriter = new System.IO.StreamWriter(oFs, System.Text.Encoding.Default);

				// Key 값 만들기
				if (keys != null)
				{
					for (int i = 0; i < keys.Length; i++)
					{
						strKey += keys[i] + "&";
					}

					if (strKey.Length > 0)
					{
						strKey = strKey.Substring(0, strKey.Length - 1);
					}
				}

				oWriter.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
					DateTime.Now.ToString("yyyy-MM-dd HHmmss"),
					prjName,
					className,
					serviceName,
					strKey,
					data,
					desc));
				
				bReturn = true;
			}
			catch
			{
				bReturn = false;
			}
			finally
			{
				if ( oWriter != null )
				{
					oWriter.Flush();
					oWriter.Close();
				}
				if ( oFs != null )
				{
					oFs.Close();
				}
			}
			return bReturn;
		}


		/// <summary>
		/// <b>로그기록</b><br/>
        /// </summary>
        /// <remarks>
		/// - 작  성  자 : 인터데브 유승호<br/>
		/// - 최초작성일 : 2009년 01월 16일<br/>
		/// - 최종수정자 : 유승호<br/>
		/// - 최종수정일 : 2009년 01월 16일<br/>
		/// - 주요변경로그<br/>
		/// 2009.01.16 생성<br/>
        /// </remarks>
		/// <param name="type">개체타입</param>
		/// <param name="serviceName">서비스명</param>
		/// <param name="keys">키값</param>
		/// <param name="data">테이타</param>
		/// <param name="desc">Description</param>
		/// <param name="folder">폴더</param>
		/// <param name="fileName">파일명</param>
		/// <returns>결과(성공:true,실패:false)</returns>
		public static bool WriteLog(Type type, string serviceName, string[] keys, string data, string desc, string folder, string fileName)
		{
			bool bReturn = false;
			System.IO.FileStream oFs = null;
			System.IO.StreamWriter oWriter = null;
			
			string fPath = "";
			string strKey = "";

			try
			{
				if (System.IO.Directory.Exists(folder))
				{
					fPath = string.Format(@"{0}\{1}_{2}.log", folder, fileName, DateTime.Now.ToString("yyyy-MM-dd"));

					oFs = new System.IO.FileStream(fPath, FileMode.Append, FileAccess.Write, FileShare.Write);
					oWriter = new System.IO.StreamWriter(oFs, System.Text.Encoding.Default);

					// Key 값 만들기
					if (keys != null)
					{
						for (int i = 0; i < keys.Length; i++)
						{
							strKey += keys[i] + "&";
						}

						if (strKey.Length > 0)
						{
							strKey = strKey.Substring(0, strKey.Length - 1);
						}
					}

					oWriter.WriteLine(string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n{8}",
						"==========================================================================================",
						DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
						type.Namespace,
						type.Name,
						serviceName,
						strKey,
						data,
						desc,
						"=========================================================================================="
						));

					bReturn = true;
				}
				else
				{
					bReturn = false;
				}
			}
			catch
			{
				bReturn = false;
			}
			finally
			{
				if (oWriter != null)
				{
					oWriter.Flush();
					oWriter.Close();
				}
				if (oFs != null)
				{
					oFs.Close();
				}
			}
			return bReturn;
		}

        /// <summary>
        /// <b>Interface 로그기록</b><br/>
        /// </summary>
        /// <remarks>
        /// - 작  성  자 : 닷넷소프트 박항록<br/>
        /// - 최초작성일 : 2011년 02월 9일<br/>
        /// - 최종수정자 : 닷넷소프트 박항록<br/>
        /// - 최종수정일 : 2011년 02월 9일<br/>
        /// - 주요변경로그<br/>
        /// 2009.01.16 생성<br/>
        /// </remarks>
        /// <param name="type">개체타입</param>
        /// <param name="serviceName">서비스명</param>
        /// <param name="keys">키값</param>
        /// <param name="data">테이타</param>
        /// <param name="desc">Description</param>
        /// <param name="folder">폴더</param>
        /// <param name="fileName">파일명</param>
        /// <returns>결과(성공:true,실패:false)</returns>
        public static bool WriteLogIF(string serviceName, string[] keys, string data, string desc, string folder, string fileName, DateTime startDate, DateTime endDate)
        {
            bool bReturn = false;
            System.IO.FileStream oFs = null;
            System.IO.StreamWriter oWriter = null;

            string strLogDir = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/InterfaceLog/InterfaceLogPath");   //  로그 경로
            string strDataLogYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/InterfaceLog/DataLogYN");    //  
            string strTimeLogYN = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/InterfaceLog/TimeLogYN");       //  실행 로그 사용여부
            string fPath = "";
            string strKey = "";
            string fFolder = strLogDir + @"\" + folder;
            try
            {
                string strAddDate = "";
                string strAddXmlData = "";

                if (strTimeLogYN.Equals("Y"))
                {
                    strAddDate = string.Format(" StartDate : {0}\r\n EndDate : {1}\r\n", startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (strDataLogYN.Equals("Y"))
                {
                    strAddXmlData = string.Format("{0}\r\n", data);
                }

                if (!System.IO.Directory.Exists(fFolder))
                {
                    System.IO.Directory.CreateDirectory(fFolder);
                }

                if (strTimeLogYN.Equals("Y") || strDataLogYN.Equals("Y"))
                {
                    fPath = strLogDir + string.Format(@"\{0}\{1}_{2}.log", folder, fileName, DateTime.Now.ToString("yyyy-MM-dd"));

                    oFs = new System.IO.FileStream(fPath, FileMode.Append, FileAccess.Write, FileShare.Write);
                    oWriter = new System.IO.StreamWriter(oFs, System.Text.Encoding.Default);

                    // Key 값 만들기
                    if (keys != null)
                    {
                        for (int i = 0; i < keys.Length; i++)
                        {
                            strKey += keys[i] + "\r\n";
                        }

                        if (strKey.Length > 0)
                        {
                            strKey = strKey.Substring(0, strKey.Length - 1);
                        }
                    }
                    string strWrite = string.Empty;


                    strWrite = string.Format("{0}\r\n Date : {1}\r\n{2} ServiceName : {3}\r\n Key : {4}\r\n{5}\r\n{6}\r\n{7}",
                         "==========================================================================================",
                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                         strAddDate,
                         serviceName,
                         strKey,
                         strAddXmlData,
                         desc,
                         "=========================================================================================="
                         );

                    oWriter.WriteLine(strWrite);

                    bReturn = true;

                }
            }
            catch
            {
                bReturn = false;
            }
            finally
            {
                if (oWriter != null)
                {
                    oWriter.Flush();
                    oWriter.Close();
                }
                if (oFs != null)
                {
                    oFs.Close();
                }
            }
            return bReturn;
        }
	}
}
