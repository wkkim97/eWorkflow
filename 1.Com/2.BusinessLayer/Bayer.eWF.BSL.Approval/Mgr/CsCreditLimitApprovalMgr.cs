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
    public class CsCreditLimitApprovalMgr : MgrBase
    {
        #region [MergeCsCreditLimitApproval]
        public string MergeCsCreditLimitApproval(Dto.DTO_DOC_CS_CREDIT_LIMIT_APPROVAL doc)
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
                    using (Dao.CsCreditLimitApprovalDao dao = new Dao.CsCreditLimitApprovalDao())
                    {
                        dao.MergeCsCreditLimitApproval(doc);
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

        #region [SelectCsCreditLimitApproval]
        public Dto.DTO_DOC_CS_CREDIT_LIMIT_APPROVAL SelectCsCreditLimitApproval(string processId)
        {
            try
            {
                using (Dao.CsCreditLimitApprovalDao dao = new Dao.CsCreditLimitApprovalDao())
                {
                    return dao.SelectCsCreditLimitApproval(processId);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        
    }
}
