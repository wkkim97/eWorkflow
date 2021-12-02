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
    public class BCSCompensationMgr : MgrBase
    {
        #region [MergeBCSCompensation]
        public string MergeBCSCompensation(Dto.DTO_DOC_BCS_COMPENSATION doc, List<Dto.DTO_DOC_BCS_COMPENSATION_PRODUCT> products)
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
                    using (Dao.BCSCompensationDao dao = new Dao.BCSCompensationDao())
                    {
                        dao.MergeBCSCompensation(doc);
                        dao.DeleteBCSCompensationProductAll(processId);

                        foreach (Dto.DTO_DOC_BCS_COMPENSATION_PRODUCT product in products)
                        {                           
                            product.PROCESS_ID = processId;
                            dao.MergeBCSCompensationProduct(product);
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

        #region [SelectBCSCompensation]
        public Dto.DTO_DOC_BCS_COMPENSATION SelectBCSCompensation(string processId)
        {
            try
            {
                using (Dao.BCSCompensationDao dao = new Dao.BCSCompensationDao())
                {
                    return dao.SelectBCSCompensation(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectBCSCompensationProduct]
        public List<Dto.DTO_DOC_BCS_COMPENSATION_PRODUCT> SelectBCSCompensationProduct(string processId)
        {
            try
            {
                using (Dao.BCSCompensationDao dao = new Dao.BCSCompensationDao())
                {
                    return dao.SelectBCSCompensationProduct(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBCSCompensationProductsByIndex]
        public void DeleteBCSCompensationProductsByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.BCSCompensationDao dao = new Dao.BCSCompensationDao())
                {
                    dao.DeleteBCSCompensationProductsByIndex(processId, idx);
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
