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
    public class PHExceptionrequestMgr : MgrBase
    {
        #region [MergePHExceptionrequest]
        public string MergePHExceptionrequest(Dto.DTO_DOC_PH_EXCEPTION_REQUEST doc)
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
                    using (Dao.PHExceptionrequestDao dao = new Dao.PHExceptionrequestDao())
                    {
                        dao.MergePHExceptionrequest(doc);
                        //dao.DeleteCSExceptionrequestProductALL(processId);

                       // foreach (Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product in products)
                       // {                           
                       //     product.PROCESS_ID = processId;
                       //     dao.MergeCSExceptionProduct(product);
                       // }

                        
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

        #region [SelectPHExceptionrequest]
        public Dto.DTO_DOC_PH_EXCEPTION_REQUEST SelectPHExceptionrequest(string processId)
        {
            try
            {
                using (Dao.PHExceptionrequestDao dao = new Dao.PHExceptionrequestDao())
                {
                    return dao.SelectPHExceptionrequest(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

       // #region[SelectCSExceptionrequestProduct]
       // public List<Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> SelectCSExceptionrequestProduct(string processId)
       // {
       //     try
       //     {
       //         using (Dao.CSExceptionrequestDao dao = new Dao.CSExceptionrequestDao())
       //         {
       //             return dao.SelectCSExceptionrequestProduct(processId);
       //         }
       //     }
       //     catch
       //     {
       //         throw;
       //     }
       // }
       // #endregion

       
    }
}
