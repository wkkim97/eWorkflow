using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dao;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class EarlyPaymentDiscountMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeEarlyPaymentDiscount(Dto.DTO_DOC_EARLY_PAYMENT_DISCOUNT doc)
        {
            try
            {
                if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                {
                    using (Dao.CommonDao dao = new Dao.CommonDao())
                    {
                        doc.PROCESS_ID = dao.GetNewProcessID();
                    }
                }

                using (EarlyPaymentDiscountDao dao = new EarlyPaymentDiscountDao())
                {
                    dao.MergeEarlyPaymentDiscount(doc);
                }
                return doc.PROCESS_ID;
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_EARLY_PAYMENT_DISCOUNT SelectEarlyPaymentDiscount(string processId)
        {
            try
            {
                using (EarlyPaymentDiscountDao dao = new EarlyPaymentDiscountDao())
                {
                    return dao.SelectEarlyPaymentDiscount(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
