using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class IncentivePaymentMgr : MgrBase
    {
        #region MergeIncentivePaymnet
        public string MergeIncentivePayment(Dto.DTO_DOC_INCENTIVE_PAYMENT doc)
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

                    using (Dao.IncentivePaymentDao dao = new Dao.IncentivePaymentDao())
                    {
                        dao.MergeIncentivePayment(doc);
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

        #region SelectIncentiveScheme
        public Dto.DTO_DOC_INCENTIVE_PAYMENT SelectIncentivePayment(string processId)
        {
            try
            {
                using (Dao.IncentivePaymentDao dao = new Dao.IncentivePaymentDao())
                {
                    return dao.SelectIncentivePayment(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectIncentiveScheme_I
        public Dto.DTO_DOC_INCENTIVE_PAYMENT SelectIncentivePayment_I(string processId)
        {
            try
            {
                using (Dao.IncentivePaymentDao dao = new Dao.IncentivePaymentDao())
                {
                    return dao.SelectIncentivePayment_I(processId);
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
