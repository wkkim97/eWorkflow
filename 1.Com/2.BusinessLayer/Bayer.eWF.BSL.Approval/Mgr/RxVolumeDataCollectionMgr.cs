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
    public class RxVolumeDataCollectionMgr : MgrBase
    {
        #region MergeRxVolumeDataCollection
        public string MergeRxVolumeDataCollection(Dto.DTO_DOC_RX_VOLUME_DATA_COLLECTION doc)
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
                    using (Dao.RxVolumeDataCollection dao = new RxVolumeDataCollection())
                    {
                        dao.MergeRxVolumeDataCollection(doc);
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

        #region SelectRxVolumeDataCollection
        public Dto.DTO_DOC_RX_VOLUME_DATA_COLLECTION SelectRxVolumeDataCollection(string processId)
        {
            try
            {
                using(Dao.RxVolumeDataCollection dao = new RxVolumeDataCollection())
                {
                    return dao.SelectRxVolumeDataCollection(processId);
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
