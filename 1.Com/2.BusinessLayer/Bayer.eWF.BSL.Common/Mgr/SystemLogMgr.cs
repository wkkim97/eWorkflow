using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Dto;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data;
using System.Xml;
using System.IO;
using DNSoft.eW.FrameWork;
using System.Web;
using DNSoft.eWF.FrameWork.Web;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class SystemLogMgr : MgrBase
    {
        public static void InsertSystemLog(string logtype, string eventName, string message, string userid)
        {
            Dto.DTO_SYSTEM_LOG log;
            try
            {
                string logyn = DNSoft.eW.FrameWork.eWBase.GetConfig("//LogInfo/ApplicationLog/DebugLogYN");
                //string logyn = "Y";
                if (logyn.Equals("Y"))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (SystemLogDao dao = new SystemLogDao())
                        {
                            log = new DTO_SYSTEM_LOG();
                            log.TYPE = logtype;
                            log.EVENT_NAME = eventName;
                            log.MESSAGE = message;
                            log.CREATER_ID = userid;
                            dao.InsertSystemLog(log);
                        }
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
