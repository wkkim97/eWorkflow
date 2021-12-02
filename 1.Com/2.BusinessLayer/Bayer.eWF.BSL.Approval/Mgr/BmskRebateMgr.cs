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
    public class BmskRebateMgr : MgrBase
    {
        #region [MergeBmskRebate]
        public string MergeBmskRebate(Dto.DTO_DOC_BMSK_REBATE doc, List<Dto.DTO_DOC_BMSK_REBATE_PRODUCT> products, List<Dto.DTO_DOC_BMSK_REBATE_CUSTOMER> customers)
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
                    using (Dao.BmskRebateDao dao = new Dao.BmskRebateDao())
                    {
                        dao.MergeBmskRebate(doc);

                        dao.DeleteBmskRebateProductAll(processId);

                        dao.DeleteBmskRebateCustomerAll(processId);

                        foreach (Dto.DTO_DOC_BMSK_REBATE_PRODUCT product in products)
                        {
                            if (product.PRODUCT_CODE.IsNullOrEmptyEx() || product.PRODUCT_NAME.IsNullOrEmptyEx()) continue;
                            product.PROCESS_ID = processId;
                            dao.MergeBmskRebateProduct(product);
                        }

                        foreach (Dto.DTO_DOC_BMSK_REBATE_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.InsertBmskRebateCustomer(customer);
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
        #endregion

        #region [SelectBmskRebate]
        public Dto.DTO_DOC_BMSK_REBATE SelectBmskRebate(string processId)
        {
            try
            {
                using (Dao.BmskRebateDao dao = new Dao.BmskRebateDao())
                {
                    return dao.SelectBmskRebate(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBmskRebateProduct]
        public List<Dto.DTO_DOC_BMSK_REBATE_PRODUCT> SelectBmskRebateProduct(string processId)
        {
            try
            {
                using (Dao.BmskRebateDao dao = new Dao.BmskRebateDao())
                {
                    return dao.SelectBmskRebateProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region[SelectBmskRebateCustomer]
        public List<Dto.DTO_DOC_BMSK_REBATE_CUSTOMER> SelectBmskRebateCustomer(string processId)
        {
            try
            {
                using (Dao.BmskRebateDao dao = new Dao.BmskRebateDao())
                {
                    return dao.SelectBmskRebateCustomer(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBmskRebateProductByIndex]
        public void DeleteBmskRebateProductByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.BmskRebateDao dao = new Dao.BmskRebateDao())
                {
                    dao.DeleteBmskRebateProductByIndex(processId, idx);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
