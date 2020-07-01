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
    public class ResaleDataCollectionMgr : MgrBase
    {
        #region MergeResaleDataCollection
        public string MergeResaleDataCollection(Dto.DTO_DOC_RESALE_DATA_COLLECTION doc)
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
                    using (Dao.ResaleDataCollection dao = new ResaleDataCollection())
                    {
                        dao.MergeResaleDataCollection(doc);
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

        public Dto.DTO_DOC_RESALE_DATA_COLLECTION SelectResaleDataCollection(string processId)
        {
            try
            {
                using(Dao.ResaleDataCollection dao = new ResaleDataCollection())
                {
                    return dao.SelectResaleDataCollection(processId);
                }
            }
            catch (Exception ex)
            {   
                throw ex;
            }
        }
    }
}
