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
    public class BKLSpecialPriceandDiscountMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeBKLPriceAndMargin(Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT doc
            , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER> customers
            , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL> hospitals
            , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> products)
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
                    using (BKLSpecialPriceandDiscountDao dao = new BKLSpecialPriceandDiscountDao())
                    {
                        dao.MergeBKLPriceAndMargin(doc);

                        dao.DeleteBKLPriceAndMarginCustomerAll(processId);

                        foreach (Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.MergeBKLPriceAndMarginCustomer(customer);
                        }

                        dao.DeleteBKLPriceAndMarginHospitalAll(processId);

                        foreach (Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL hospital in hospitals)
                        {
                            hospital.PROCESS_ID = processId;
                            dao.MergeBKLPriceAndMarginHospital(hospital);
                        }

                        dao.DeleteBKLPriceAndMarginProductAll(processId);
                        foreach (Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeBKLPriceAndMarginProduct(product);
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

        public Tuple<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT
            , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER>
            , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL>
            , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>> SelectBKLPriceAndMargin(string processId)
        {
            try
            {
                using (BKLSpecialPriceandDiscountDao dao = new BKLSpecialPriceandDiscountDao())
                {
                    Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT doc = dao.SelectBKLPriceAndMargin(processId);

                    List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER> customers = dao.SelectBKLPriceAndMarginCustomer(processId);

                    List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL> hospitals = dao.SelectBKLPriceAndMarginHospital(processId);

                    List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT> products = dao.SelectBKLPriceAndMarginProduct(processId);

                    return new Tuple<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT
                    , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_CUSTOMER>
                    , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_HOSPITAL>
                    , List<Dto.DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT>>(doc, customers, hospitals, products);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBKLPriceAndMarginProduct(string processId, int idx)
        {
            try
            {
                using (BKLSpecialPriceandDiscountDao dao = new BKLSpecialPriceandDiscountDao())
                {
                    dao.DeleteBKLPriceAndMarginProduct(processId, idx);
                }
            }
            catch
            {
                throw;
            }
        }
        public void Update_Dealno(string processid, string dealo, string user_id)
        {
            try
            {
                using (BKLSpecialPriceandDiscountDao dao = new BKLSpecialPriceandDiscountDao())
                {
                    dao.Update_Dealno(processid, dealo, user_id);
                }
            }
            catch
            {
                throw;
            }

        }
    }
}
