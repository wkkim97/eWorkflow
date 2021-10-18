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
    public class InternalEventMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeInternalEvent(Dto.DTO_DOC_INTERNAL_EVENT doc, List<Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL> details)
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
                    
                    using (InternalEventDao dao = new InternalEventDao())
                    {
                        dao.MergeInternalEvent(doc);

                        dao.DeleteInternalEventCostDetailAll(doc.PROCESS_ID);
                        decimal? total = 0;
                        foreach (Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL cost in details)
                        {
                            cost.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertInternalEventCostDetail(cost);
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

        public void DeleteInternalEventCostDetail(string processId, int index)
        {
            try
            {
                using (InternalEventDao dao = new InternalEventDao())
                {
                    dao.DeleteInternalEventCostDetail(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

        public Tuple<Dto.DTO_DOC_INTERNAL_EVENT, List<Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL>>
            SelectInternalEvent(string processId)
        {
            try
            {
                using (InternalEventDao dao = new InternalEventDao())
                {
                    Dto.DTO_DOC_INTERNAL_EVENT doc = dao.SelectInternalEvent(processId);

                    List<Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL> details = dao.SelectInternalEventCostDetail(processId);

                    return new Tuple<Dto.DTO_DOC_INTERNAL_EVENT, List<Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL>>(doc, details);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
