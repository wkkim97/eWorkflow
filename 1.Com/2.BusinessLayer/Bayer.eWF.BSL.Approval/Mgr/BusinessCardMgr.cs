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
    
    public class BusinessCardMgr : MgrBase
    {
        #region MergeBusinessCard
        public string MergeBusinessCard(Dto.DTO_DOC_BUSINESS_CARD doc)
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
                    using (Dao.BusinessCardDao dao = new Dao.BusinessCardDao())
                    {
                        dao.MergeBusinessCard(doc);
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

        #region SelectBusinessCard
        public Dto.DTO_DOC_BUSINESS_CARD SelectBusinessCard(string processId)
        {
            try
            {
                using (Dao.BusinessCardDao dao = new Dao.BusinessCardDao())
                {
                    return dao.SelectBusinessCard(processId);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectBusinessCardDisplayNameCard
        public Dto.DTO_DOC_BUSINESS_CARD SelectBusinessCardDisplayNameCard(string processId)
        {
            try
            {
                using (Dao.BusinessCardDao dao = new Dao.BusinessCardDao())
                {
                    return dao.SelectBusinessCardDisplayNameCard(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public List<Dto.SendToAgencyBusicessCardDto> SelectRequesterAndRecipient(string processId)
        {
            try
            {
                using (Dao.BusinessCardDao dao = new Dao.BusinessCardDao())
                {
                    return dao.SelectRequesterAndRecipient(processId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
