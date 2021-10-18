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
    public class SampleRequestMgr : MgrBase
    {
        #region [MergeSampleRequest]
        public string MergeSampleRequest(Dto.DTO_DOC_SAMPLE_REQUEST doc, List<Dto.DTO_DOC_SAMPLE_REQUEST_ITEMS> items)
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
                    using (Dao.SampleRequestDao dao = new Dao.SampleRequestDao())
                    {
                        dao.MergeSampleRequest(doc);
                        dao.DeleteSampleRequestItemsAll(processId);

                        foreach (Dto.DTO_DOC_SAMPLE_REQUEST_ITEMS item in items)
                        {
                            if (item.ITEM_CODE.IsNullOrEmptyEx() || item.ITEM_DESC.IsNullOrEmptyEx()) continue;
                            item.PROCESS_ID = processId;
                            dao.MergeSampleRequestItems(item);
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

        #region [SelectSampleRequet]
        public Dto.DTO_DOC_SAMPLE_REQUEST SelectSampleRequet(string processId)
        {
            try
            {
                using (Dao.SampleRequestDao dao = new Dao.SampleRequestDao())
                {
                    return dao.SelectSampleRequest(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectSampleRequestItems]
        public List<Dto.DTO_DOC_SAMPLE_REQUEST_ITEMS> SelectSampleRequestItems(string processId)
        {
            try
            {
                using (Dao.SampleRequestDao dao = new Dao.SampleRequestDao())
                {
                    return dao.SelectSampleRequestItems(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteSampleRequestItemsByIndex]
        public void DeleteSampleRequestItemsByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.SampleRequestDao dao = new Dao.SampleRequestDao())
                {
                    dao.DeleteSampleRequestItemsByIndex(processId, idx);
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
