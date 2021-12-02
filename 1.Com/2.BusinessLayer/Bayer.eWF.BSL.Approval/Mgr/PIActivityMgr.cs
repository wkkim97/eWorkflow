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
    public class PIActivityMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergePIActivity(Dto.DTO_DOC_P_I_ACTIVITY doc, List<Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL> details)
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
                    
                    using (PIActivityDao dao = new PIActivityDao())
                    {
                        dao.MergePIActivity(doc);

                        dao.DeletePIActivityCostDetailAll(doc.PROCESS_ID);
                        decimal? total = 0;
                        foreach (Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL cost in details)
                        {
                            cost.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertPIActivityCostDetail(cost);
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

        public void DeletePIActivityCostDetail(string processId, int index)
        {
            try
            {
                using (PIActivityDao dao = new PIActivityDao())
                {
                    dao.DeletePIActivityCostDetail(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        public Tuple<Dto.DTO_DOC_P_I_ACTIVITY, List<Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL>>
            SelectPIActivity(string processId)
        {
            try
            {
                using (PIActivityDao dao = new PIActivityDao())
                {
                    Dto.DTO_DOC_P_I_ACTIVITY doc = dao.SelectPIActivity(processId);

                    List<Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL> details = dao.SelectPIActivityCostDetail(processId);

                    return new Tuple<Dto.DTO_DOC_P_I_ACTIVITY, List<Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL>>(doc, details);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
