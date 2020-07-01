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
    public class BaseDiscountPriceMgr : MgrBase
    {
        #region [MergeBaseDiscountPrice]
        public string MergeBaseDiscountPrice(Dto.DTO_DOC_BASE_DISCOUNT_PRICE doc
            , List<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> products
            ,List<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER> customers)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;

                        }
                    }
                    using (Dao.BaseDiscountPriceDao dao = new Dao.BaseDiscountPriceDao())
                    {
                        dao.MergeBaseDiscountPrice(doc);
                        dao.DeleteBaseDiscountPriceProductAll(processId);

                        foreach (Dto.DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT product in products)
                        {
                            
                            product.PROCESS_ID = processId;
                            dao.MergeBaseDiscountPriceProduct(product);

                        }

                        dao.DeleteBaseDiscountPriceCustomerAll(processId);
                        foreach (Dto.DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.MergeBaseDiscountPriceCustomer(customer);
                        }
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

        #region [SelectBaseDiscountPrice]
        public Dto.DTO_DOC_BASE_DISCOUNT_PRICE SelectBaseDiscountPrice(string processId)
        {
            try
            {
                using (Dao.BaseDiscountPriceDao dao = new Dao.BaseDiscountPriceDao())
                {
                    return dao.SelectBaseDiscountPrice(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBaseDiscountPriceProduct]
        public List<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> SelectBaseDiscountPriceProduct(string processId)
        {
            try
            {
                using (Dao.BaseDiscountPriceDao dao = new Dao.BaseDiscountPriceDao())
                {
                    return dao.SelectBaseDiscountPriceProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBaseDiscountPriceProductByIndex]
        public void DeleteBaseDiscountPriceProductByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.BaseDiscountPriceDao dao = new Dao.BaseDiscountPriceDao())
                {
                    dao.DeleteBaseDiscountPriceProductByIndex(processId, idx);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public List<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER> SelectBaseDiscountPriceCustomer(string processId)
        {
            try
            {
                using (BaseDiscountPriceDao dao = new BaseDiscountPriceDao())
                {
                    return dao.SelectBaseDiscountPriceCustomer(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
