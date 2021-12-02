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
    public class BHCPriceAndMargin : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeBHCPriceAndMargin(Dto.DTO_DOC_BHC_PRICE_AND_MARGIN doc
            , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_CUSTOMER> customers
            , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_HOSPITAL> hospitals
            , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_PRODUCT> products)
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
                    using (BHCPriceAndMarginDao dao = new BHCPriceAndMarginDao())
                    {
                        dao.MergeBHCPriceAndMargin(doc);

                        dao.DeleteBHCPriceAndMarginCustomerAll(processId);

                        foreach (Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.MergeBHCPriceAndMarginCustomer(customer);
                        }

                        dao.DeleteBHCPriceAndMarginHospitalAll(processId);

                        foreach (Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_HOSPITAL hospital in hospitals)
                        {
                            hospital.PROCESS_ID = processId;
                            dao.MergeBHCPriceAndMarginHospital(hospital);
                        }

                        dao.DeleteBHCPriceAndMarginProductAll(processId);
                        foreach (Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeBHCPriceAndMarginProduct(product);
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

        public Tuple<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN
            , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_CUSTOMER>
            , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_HOSPITAL>
            , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_PRODUCT>> SelectBHCPriceAndMargin(string processId)
        {
            try
            {
                using (BHCPriceAndMarginDao dao = new BHCPriceAndMarginDao())
                {
                    Dto.DTO_DOC_BHC_PRICE_AND_MARGIN doc = dao.SelectBHCPriceAndMargin(processId);

                    List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_CUSTOMER> customers = dao.SelectBHCPriceAndMarginCustomer(processId);

                    List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_HOSPITAL> hospitals = dao.SelectBHCPriceAndMarginHospital(processId);

                    List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_PRODUCT> products = dao.SelectBHCPriceAndMarginProduct(processId);

                    return new Tuple<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN
                    , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_CUSTOMER>
                    , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_HOSPITAL>
                    , List<Dto.DTO_DOC_BHC_PRICE_AND_MARGIN_PRODUCT>>(doc, customers, hospitals, products);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBHCPriceAndMarginProduct(string processId, int idx)
        {
            try
            {
                using (BHCPriceAndMarginDao dao = new BHCPriceAndMarginDao())
                {
                    dao.DeleteBHCPriceAndMarginProduct(processId, idx);
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
                using (BHCPriceAndMarginDao dao = new BHCPriceAndMarginDao())
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
