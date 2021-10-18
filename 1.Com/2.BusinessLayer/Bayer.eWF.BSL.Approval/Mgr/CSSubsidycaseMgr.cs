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
    public class CSSubsidycaseMgr : MgrBase
    {
        #region [MergeCSExceptionrequest]
        public string MergeCSSubsidycase(Dto.DTO_DOC_CS_SUBSIDY_CASE doc, List<Dto.DTO_DOC_CS_SUBSIDY_CASE_PRODUCT> products)
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
                    using (Dao.CSSubsidycaseDao dao = new Dao.CSSubsidycaseDao())
                    {
                        dao.MergeCSSubsidycase(doc);
                        dao.DeleteCSSubsidycaseProductALL(processId);

                        foreach (Dto.DTO_DOC_CS_SUBSIDY_CASE_PRODUCT product in products)
                        {                           
                            product.PROCESS_ID = processId;
                            dao.MergeCSSubsidycaseProduct(product);
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

        #region [SelectCSExceptionrequest]
        public Dto.DTO_DOC_CS_SUBSIDY_CASE SelectCSSubsidycase(string processId)
        {
            try
            {
                using (Dao.CSSubsidycaseDao dao = new Dao.CSSubsidycaseDao())
                {
                    return dao.SelectCSSubsidycase(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectCSExceptionrequestProduct]
        public List<Dto.DTO_DOC_CS_SUBSIDY_CASE_PRODUCT> SelectCSSubsidycaseProduct(string processId)
        {
            try
            {
                using (Dao.CSSubsidycaseDao dao = new Dao.CSSubsidycaseDao())
                {
                    return dao.SelectCSSubsidycaseProduct(processId);
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
