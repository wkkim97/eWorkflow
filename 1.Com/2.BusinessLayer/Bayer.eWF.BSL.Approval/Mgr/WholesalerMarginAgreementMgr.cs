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
    public class WholesalerMarginAgreementMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeWholesalerMarginAgreement(Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT doc
            , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER> customers
            , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL> hospitals
            , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> products)
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
                    using (WholesalerMarginAgreementDao dao = new WholesalerMarginAgreementDao())
                    {
                        dao.MergeWholesalerMarginAgreement(doc);

                        dao.DeleteWholesalerMarginAgreementCustomerAll(processId);

                        foreach (Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.MergeWholesalerMarginAgreementCustomer(customer);
                        }

                        dao.DeleteWholesalerMarginAgreementHospitalAll(processId);

                        foreach (Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL hospital in hospitals)
                        {
                            hospital.PROCESS_ID = processId;
                            dao.MergeWholesalerMarginAgreementHospital(hospital);
                        }

                        dao.DeleteWholesalerMarginAgreementProductAll(processId);
                        foreach (Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeWholesalerMarginAgreementProduct(product);
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

        public Tuple<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT
            , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER>
            , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL>
            , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>> SelectWholesalerMarginAgreement(string processId)
        {
            try
            {
                using (WholesalerMarginAgreementDao dao = new WholesalerMarginAgreementDao())
                {
                    Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT doc = dao.SelectWholesalerMarginAgreement(processId);

                    List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER> customers = dao.SelectWholesalerMarginAgreementCustomer(processId);

                    List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL> hospitals = dao.SelectWholesalerMarginAgreementHospital(processId);

                    List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> products = dao.SelectWholesalerMarginAgreementProduct(processId);

                    return new Tuple<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT
                    , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER>
                    , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL>
                    , List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>>(doc, customers, hospitals, products);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteWholesalerMarginAgreementProduct(string processId, int idx)
        {
            try
            {
                using (WholesalerMarginAgreementDao dao = new WholesalerMarginAgreementDao())
                {
                    dao.DeleteWholesalerMarginAgreementProduct(processId, idx);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
