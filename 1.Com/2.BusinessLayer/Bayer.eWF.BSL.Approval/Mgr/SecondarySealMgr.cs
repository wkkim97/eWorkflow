using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bayer.eWF.BSL.Approval.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using DNSoft.eW.FrameWork;
using System.Transactions;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class SecondarySealMgr : MgrBase
    {
        #region MergeSecondarySeal
        public string MergeSecondarySeal(Dto.DTO_DOC_SECONDARY_SEAL doc)
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
                    using(Dao.SecondarySealDao dao = new SecondarySealDao())
                	{
		                dao.MergeSecondarySeal(doc);
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

        #region SelectSecondarySeal
        public Dto.DTO_DOC_SECONDARY_SEAL SelectSecondarySeal(string processId)
        {
            try
            {
                using (Dao.SecondarySealDao dao = new SecondarySealDao())
                {
                    return dao.SelectSecondarySeal(processId);
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
