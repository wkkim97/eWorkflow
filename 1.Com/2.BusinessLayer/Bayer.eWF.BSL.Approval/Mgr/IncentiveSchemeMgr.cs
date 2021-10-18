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
    public class IncentiveSchemeMgr : MgrBase
    {
        #region MergeIncentiveScheme
        public string MergeIncentiveScheme(Dto.DTO_DOC_INCENTIVE_SCHEME doc)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if( doc.PROCESS_ID.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            doc.PROCESS_ID = dao.GetNewProcessID();
                        }
                    }
                        
                    using(Dao.IncentiveSchemeDao dao = new Dao.IncentiveSchemeDao())
                    {
                        dao.MergeIncentiveScheme(doc);
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

        #region SelectIncentiveScheme
        public Dto.DTO_DOC_INCENTIVE_SCHEME SelectIncentiveScheme(string processId)
        {
            try
            {
                using (Dao.IncentiveSchemeDao dao = new Dao.IncentiveSchemeDao())
                {
                    return dao.SelectIncentiveScheme(processId);
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
