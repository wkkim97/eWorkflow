using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DNSoft.eW.FrameWork;


namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CreditReleaseMgr : MgrBase
    {
        #region [MergeCreditRelease]
        public string MergeCreditRelease(Dto.DTO_DOC_CREDIT_RELEASE doc)
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
                    using (Dao.CreditReleaseDao dao = new Dao.CreditReleaseDao())
                    {
                        dao.MergeCreditRelease(doc);
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

        #region [SelectCreditRelease]
        public Dto.DTO_DOC_CREDIT_RELEASE SelectCreditRelease(string processId)
        {
            try
            {
                using (Dao.CreditReleaseDao dao = new Dao.CreditReleaseDao())
                {
                    return dao.SelectCreditRelease(processId);
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
