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
    public class BKLPromotionalActivityMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeBKLPromotionalActivity(Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY doc, List<Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY_COST_DETAIL> details)
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
                    
                    using (BKLPromotionalActivityDao dao = new BKLPromotionalActivityDao())
                    {
                        dao.MergeBKLPromotionalActivity(doc);

                        dao.DeleteBKLPromotionalActivityCostDetailAll(doc.PROCESS_ID);
                        decimal? total = 0;
                        foreach (Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY_COST_DETAIL cost in details)
                        {
                            cost.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertBKLPromotionalActivityCostDetail(cost);
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

        public void DeleteBKLPromotionalActivityCostDetail(string processId, int index)
        {
            try
            {
                using (BKLPromotionalActivityDao dao = new BKLPromotionalActivityDao())
                {
                    dao.DeleteBKLPromotionalActivityCostDetail(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        public Tuple<Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY, List<Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY_COST_DETAIL>>
            SelectBKLPromotionalActivity(string processId)
        {
            try
            {
                using (BKLPromotionalActivityDao dao = new BKLPromotionalActivityDao())
                {
                    Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY doc = dao.SelectBKLPromotionalActivity(processId);

                    List<Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY_COST_DETAIL> details = dao.SelectBKLPromotionalActivityCostDetail(processId);

                    return new Tuple<Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY, List<Dto.DTO_DOC_BKL_PROMOTIONAL_ACTIVITY_COST_DETAIL>>(doc, details);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
