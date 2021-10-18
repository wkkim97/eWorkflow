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
    public class CSExceptionrequestMgr : MgrBase
    {
        #region [MergeCSExceptionrequest]
        public string MergeCSExceptionrequest(Dto.DTO_DOC_CS_EXCEPTION_REQUEST doc, List<Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> products)
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
                    using (Dao.CSExceptionrequestDao dao = new Dao.CSExceptionrequestDao())
                    {
                        dao.MergeCSExceptionrequest(doc);
                        dao.DeleteCSExceptionrequestProductALL(processId);

                        foreach (Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product in products)
                        {                           
                            product.PROCESS_ID = processId;
                            dao.MergeCSExceptionProduct(product);
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
        public Dto.DTO_DOC_CS_EXCEPTION_REQUEST SelectCSExceptionrequest(string processId)
        {
            try
            {
                using (Dao.CSExceptionrequestDao dao = new Dao.CSExceptionrequestDao())
                {
                    return dao.SelectCSExceptionrequest(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectCSExceptionrequestProduct]
        public List<Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> SelectCSExceptionrequestProduct(string processId)
        {
            try
            {
                using (Dao.CSExceptionrequestDao dao = new Dao.CSExceptionrequestDao())
                {
                    return dao.SelectCSExceptionrequestProduct(processId);
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
