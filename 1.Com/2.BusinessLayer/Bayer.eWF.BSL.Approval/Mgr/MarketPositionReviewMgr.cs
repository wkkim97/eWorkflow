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
    public class MarketPositionReviewMgr : MgrBase
    {
        #region MergeMarketPositionReview
        public string MergeMarketPositionReview(Dto.DTO_DOC_MARKET_POSITION_REVIEW doc)
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
                    using (Dao.MarketPositionReviewDao dao = new MarketPositionReviewDao())
                    {
                        dao.MergeMarketPositionReview(doc);
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
        public Dto.DTO_DOC_MARKET_POSITION_REVIEW SelectMarketPositionReview(string processId)
        {
            try
            {
                using (Dao.MarketPositionReviewDao dao = new MarketPositionReviewDao())
                {
                    return dao.SelectMarketPositionReview(processId);
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
