using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dao;


namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class IncidentReportMgr : MgrBase
    {
        #region MergeIncidentReport
        public string MergeIncidentReport(Dto.DTO_DOC_INCIDENT_REPORT doc)
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

                    using (Dao.IncidentReportDao dao = new Dao.IncidentReportDao())
                    {
                        dao.MergeIncidentReport(doc);
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

        #region SelectIncidentReport
        public Dto.DTO_DOC_INCIDENT_REPORT SelectIncidentReport(string processId)
        {
            try
            {
                using (Dao.IncidentReportDao dao = new Dao.IncidentReportDao())
                {
                    return dao.SelectIncidentReport(processId);
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
