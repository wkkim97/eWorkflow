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
    public class BKLPriceAndMargin : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeBKLPriceAndMargin(Dto.DTO_DOC_BKL_PRICE_AND_MARGIN doc
            , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER> customers
            , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL> hospitals
            , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> products)
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
                    using (BKLPriceAndMarginDao dao = new BKLPriceAndMarginDao())
                    {
                        dao.MergeBKLPriceAndMargin(doc);

                        dao.DeleteBKLPriceAndMarginCustomerAll(processId);

                        foreach (Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.MergeBKLPriceAndMarginCustomer(customer);
                        }

                        dao.DeleteBKLPriceAndMarginHospitalAll(processId);

                        foreach (Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL hospital in hospitals)
                        {
                            hospital.PROCESS_ID = processId;
                            dao.MergeBKLPriceAndMarginHospital(hospital);
                        }

                        dao.DeleteBKLPriceAndMarginProductAll(processId);
                        foreach (Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT product in products)
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

        public Tuple<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN
            , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER>
            , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL>
            , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>> SelectBKLPriceAndMargin(string processId)
        {
            try
            {
                using (BKLPriceAndMarginDao dao = new BKLPriceAndMarginDao())
                {
                    Dto.DTO_DOC_BKL_PRICE_AND_MARGIN doc = dao.SelectBKLPriceAndMargin(processId);

                    List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER> customers = dao.SelectBKLPriceAndMarginCustomer(processId);

                    List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL> hospitals = dao.SelectBKLPriceAndMarginHospital(processId);

                    List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT> products = dao.SelectBKLPriceAndMarginProduct(processId);

                    return new Tuple<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN
                    , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_CUSTOMER>
                    , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_HOSPITAL>
                    , List<Dto.DTO_DOC_BKL_PRICE_AND_MARGIN_PRODUCT>>(doc, customers, hospitals, products);
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
                using (BKLPriceAndMarginDao dao = new BKLPriceAndMarginDao())
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
                using (BKLPriceAndMarginDao dao = new BKLPriceAndMarginDao())
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
