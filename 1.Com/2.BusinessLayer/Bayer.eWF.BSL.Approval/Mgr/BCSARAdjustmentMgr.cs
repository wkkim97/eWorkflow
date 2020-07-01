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
    public class BCSARAdjustmentMgr : MgrBase
    {
        #region [MergeBCSARAdjustment]
        public string MergeBCSARAdjustment(Dto.DTO_DOC_BCS_AR_ADJUSTMENT doc, List<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> products, List<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER> customers)
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
                    using (Dao.BCSARAdjustmentDao dao = new Dao.BCSARAdjustmentDao())
                    {
                        dao.MergeBCSARAdjustment(doc);
                        dao.DeleteBCSARAdjustmentProductAll(processId);

                        foreach (Dto.DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT product in products)
                        {                           
                            product.PROCESS_ID = processId;
                            dao.MergeBCSARAdjustmentProduct(product);
                        }

                        dao.DeleteBCSARAdjustmentCustomerAll(processId);
                        foreach (Dto.DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER customer in customers)
                        {
                            customer.PROCESS_ID = processId;
                            dao.MergeBCSARAdjustmentCustomer(customer);
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

        #region [SelectBCSARAdjustment]
        public Dto.DTO_DOC_BCS_AR_ADJUSTMENT SelectBCSARAdjustment(string processId)
        {
            try
            {
                using (Dao.BCSARAdjustmentDao dao = new Dao.BCSARAdjustmentDao())
                {
                    return dao.SelectBCSARAdjustment(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBCSARAdjustmentProduct]
        public List<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> SelectBCSARAdjustmentProduct(string processId)
        {
            try
            {
                using (Dao.BCSARAdjustmentDao dao = new Dao.BCSARAdjustmentDao())
                {
                    return dao.SelectBCSARAdjustmentProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBCSARAdjustmentCustomer]
        public List<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER> SelectBCSARAdjustmentCustomer(string processId)
        {
            try
            {
                using (BCSARAdjustmentDao dao = new BCSARAdjustmentDao())
                {
                    return dao.SelectBCSARAdjustmentCustomer(processId);
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
