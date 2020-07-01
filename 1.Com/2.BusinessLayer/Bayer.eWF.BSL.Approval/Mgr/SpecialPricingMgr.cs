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
    public class SpecialPricingMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeSpecialPricing(Dto.DTO_DOC_SPECIAL_PRICING doc, List<Dto.DTO_DOC_SPECIAL_PRICING_PRODUCT> products)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (processId.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;
                        }
                    }
                    using (SpecialPricingDao dao = new SpecialPricingDao())
                    {
                        dao.MergeSpecialPricing(doc);



                        foreach (Dto.DTO_DOC_SPECIAL_PRICING_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeSpecialPricingProduct(product);
                        }
                    }

                    scope.Complete();
                    return processId;
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_SPECIAL_PRICING SelectSpecialPricing(string processId)
        {
            try
            {
                using (SpecialPricingDao dao = new SpecialPricingDao())
                {
                    return dao.SelectSpecialPricing(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_SPECIAL_PRICING_PRODUCT> SelectSpecialPricingProduct(string processId)
        {
            try
            {
                using (SpecialPricingDao dao = new SpecialPricingDao())
                {
                    return dao.SelectSpecialPricingProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteSpecialPricingProduct(string processId, string productCode)
        {
            try
            {
                using (SpecialPricingDao dao = new SpecialPricingDao())
                {
                    dao.DeleteSpecialPricingProduct(processId, productCode);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
