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
    public class CustomerComplaintHandlingMgr : MgrBase
    {
        #region [MergeCustomerComplaintHandling]
        public string MergeCustomerComplaintHandling(Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING doc, List<Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> lists)
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
                    using (Dao.CustomerComplaintHandlingDao dao = new Dao.CustomerComplaintHandlingDao())
                    {
                        dao.MergeCustomerComplaintHandling(doc);
                        dao.DeleteCustomerComplaintHandlingProductAll(processId);

                        foreach (Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT list in lists)
                        {
                            //if (list.CUSTOMER_CODE.IsNullOrEmptyEx() || list.CUSTOMER_NAME.IsNullOrEmptyEx() || list.CUR_PRODUCT_CODE.IsNullOrEmptyEx() || list.CUR_PRODUCT_NAME.IsNullOrEmptyEx() || list.REP_PRODUCT_CODE.IsNullOrEmptyEx() || list.REP_PRODUCT_NAME.IsNullOrEmptyEx()) continue;
                            list.PROCESS_ID = processId;
                            dao.MergeCustomerComplaintHandlingProduct(list);
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

        #region [SelectCustomerComplaintHandling]
        public Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING SelectCustomerComplaintHandling(string processId)
        {
            try
            {
                using (Dao.CustomerComplaintHandlingDao dao = new Dao.CustomerComplaintHandlingDao())
                {
                    return dao.SelectCustomerComplaintHandling(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectCustomerComplaintHandlingProduct]
        public List<Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> SelectCustomerComplaintHandlingProduct(string processId)
        {
            try
            {
                using (Dao.CustomerComplaintHandlingDao dao = new Dao.CustomerComplaintHandlingDao())
                {
                    return dao.SelectCustomerComplaintHandlingProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteSelectCustomerComplaintHandlingProductByIndex]
        public void DeleteSelectCustomerComplaintHandlingProductByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.CustomerComplaintHandlingDao dao = new Dao.CustomerComplaintHandlingDao())
                {
                    dao.DeleteCustomerComplaintHandlingProductByIndex(processId, idx);
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
