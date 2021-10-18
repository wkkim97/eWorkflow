using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class StrategicChangeReportMgr : MgrBase
    {
        #region MergeStrategicChangeReport
        public string MergeStrategicChangeReport(Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT doc)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            doc.PROCESS_ID = dao.GetNewProcessID();
                        }
                    }
                    using(Dao.StrategicChangeReportDao dao = new StrategicChangeReportDao())
                	{
		                dao.MergeStrategicChangeReport(doc);
	                }
                    scope.Complete();
                    return doc.PROCESS_ID;
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        } 
        #endregion

        #region MyRegion
        public Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT SelectStrategicChangeReport(string processId)
        {
            try
            {
                using (Dao.StrategicChangeReportDao dao = new StrategicChangeReportDao())
                {
                    return dao.SelectStrategicChangeReport(processId);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #endregion
    }
}
