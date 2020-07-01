using Bayer.eWF.BSL.Approval.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class AHPIActivityMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeAHPIActivity(Dto.DTO_DOC_AH_P_I_ACTIVITY doc, List<Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL> details)
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
                    
                    using (AHPIActivityDao dao = new AHPIActivityDao())
                    {
                        dao.MergeAHPIActivity(doc);

                        dao.DeleteAHPIActivityCostDetailAll(doc.PROCESS_ID);
                        decimal? total = 0;
                        foreach (Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL cost in details)
                        {
                            cost.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertAHPIActivityCostDetail(cost);
                            total += cost.AMOUNT;
                        }
                        doc.TOTAL_AMOUNT = total;
                    }

                    scope.Complete();
                    return doc.PROCESS_ID;
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAHPIActivityCostDetail(string processId, int index)
        {
            try
            {
                using (AHPIActivityDao dao = new AHPIActivityDao())
                {
                    dao.DeleteAHPIActivityCostDetail(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        public Tuple<Dto.DTO_DOC_AH_P_I_ACTIVITY, List<Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL>>
            SelectAHPIActivity(string processId)
        {
            try
            {
                using (AHPIActivityDao dao = new AHPIActivityDao())
                {
                    Dto.DTO_DOC_AH_P_I_ACTIVITY doc = dao.SelectAHPIActivity(processId);

                    List<Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL> details = dao.SelectAHPIActivityCostDetail(processId);

                    return new Tuple<Dto.DTO_DOC_AH_P_I_ACTIVITY, List<Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL>>(doc, details);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
