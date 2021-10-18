using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.Entity;

using System.Data.SqlClient;

namespace Bayer.WinSvc.PushNoticeMail
{
    public class Common : IDisposable
    {
        private const int TIMEOUT = 600000; //10분

        public Common()
        {
        
        }

          
        #region WriteLog
        /// <summary>
        /// <b>Log Write</b><br/>
        /// - 작  성  자 : 닷넷소프트<br/>
        /// - 최초작성일 : 2010.04.15<br/>
        /// - 최종수정자 : <br/>
        /// - 최종수정일 : <br/>
        /// - 주요변경로그<br/>
        /// 2010.04.15 생성<br/>
        /// </summary>
        /// <param name="sMessage">Log Message</param>
        public static void WriteLog(string sMessage)
        {
            string strLogDir = ConfigurationManager.AppSettings["LogFilePath"];
            string strLogWriteYN = ConfigurationManager.AppSettings["LogWriteYN"];

            string strFullPath = string.Format(@"{0}\{1} SendNoticeMailServiceLog.txt", strLogDir, DateTime.Now.ToString("yyyy-MM-dd"));

            FileStream oFs = null;
            StreamWriter oWriter = null;

            try
            {
                // 디렉토리 존재 여부 확인
                if (!Directory.Exists(strLogDir))
                {
                    //디렉토리가 없는 경우에 폴더 생성 여부 확인
                    if (strLogWriteYN.Equals("Y"))
                    {
                        System.IO.Directory.CreateDirectory(strLogDir);
                        //return;
                    }
                }

                oFs = new FileStream(strFullPath, FileMode.Append, FileAccess.Write, FileShare.Write);
                oWriter = new StreamWriter(oFs, System.Text.Encoding.UTF8);
                oWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + sMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (oWriter != null) oWriter.Close();
                if (oFs != null) oFs.Close();
            }
        }
        #endregion

        #region IDisposable 멤버

        public void Dispose()
        {
 
        }

        #endregion
    }
}
