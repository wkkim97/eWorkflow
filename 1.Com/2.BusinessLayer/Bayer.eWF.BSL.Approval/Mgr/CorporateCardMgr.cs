using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Transactions;
using DNSoft.eWF.FrameWork.Data.EF;
using DNSoft.eW.FrameWork;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class CorporateCardMgr : MgrBase
    {
        #region [ selectCorporateCard ]
        public Dto.DTO_DOC_CORPORATE_CARD selectCorporateCard(string processId)
        {
            try
            {
                using (Dao.CorporateCardDao dao = new Dao.CorporateCardDao())
                {
                    return dao.selectCorporateCard(processId);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        #endregion

        #region [ mergeCorporateCard ]
        public string mergeCorporateCard(Dto.DTO_DOC_CORPORATE_CARD doc)
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
                    using (Dao.CorporateCardDao dao = new Dao.CorporateCardDao())
                    {
                        dao.mergeCorporateCard(doc);
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
    }
}
