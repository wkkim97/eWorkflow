using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class RebatePolicyMgr : MgrBase
    {
        #region MergeRebatePolicy
        public string MergeRebatePolicy(Dto.DTO_DOC_REBATE_POLICY doc, List<Dto.DTO_DOC_REBATE_POLICY_PRODUCT> products)
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
                    using (Dao.RebatePolicyDao dao = new RebatePolicyDao())
                    {
                        dao.MergeRebatePolicy(doc);
                        dao.DeleteRebatePolicyProductAll(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_REBATE_POLICY_PRODUCT product in products)
                        {
                            product.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertRebatePolicyProduct(product);
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

        //eWorkflow Optimization 2020
        public string MergeRebatePolicy_NEW(Dto.DTO_DOC_REBATE_POLICY doc, List<Dto.DTO_DOC_REBATE_POLICY_PRODUCT_NEW> products)
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
                    using (Dao.RebatePolicyDao dao = new RebatePolicyDao())
                    {
                        dao.MergeRebatePolicy(doc);
                        dao.DeleteRebatePolicyProductAll_NEW(doc.PROCESS_ID);

                        foreach (Dto.DTO_DOC_REBATE_POLICY_PRODUCT_NEW product in products)
                        {
                            product.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertRebatePolicyProduct_NEW(product);
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

        #region [SelectRebatePolicy]
        public Dto.DTO_DOC_REBATE_POLICY SelectRebatePolicy(string processId)
        {
            try
            {
                using (Dao.RebatePolicyDao dao = new Dao.RebatePolicyDao())
                {
                    return dao.SelectRebatePolicy(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectRebatePolicyProduct]
        public List<Dto.DTO_DOC_REBATE_POLICY_PRODUCT> SelectRebatePolicyProduct(string processId)
        {
            try
            {
                using (Dao.RebatePolicyDao dao = new Dao.RebatePolicyDao())
                {
                    return dao.SelectRebatePolicyProduct(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //eWorkflow Optimization 2020 
        public List<Dto.DTO_DOC_REBATE_POLICY_PRODUCT_NEW> SelectRebatePolicyProduct_NEW(string processId)
        {
            try
            {
                using (Dao.RebatePolicyDao dao = new Dao.RebatePolicyDao())
                {
                    return dao.SelectRebatePolicyProduct_NEW(processId);
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
