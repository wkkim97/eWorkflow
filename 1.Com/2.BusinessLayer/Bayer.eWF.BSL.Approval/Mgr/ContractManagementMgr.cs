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
    public class ContractManagementMgr : MgrBase
    {
        #region [MergeContractManagement]
        public string MergeContractManagement(Dto.DTO_DOC_CONTRACT_MANAGEMENT doc)
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
                    using (Dao.ContractManagementDao dao = new Dao.ContractManagementDao())
                    {
                        dao.MergeContractManagement(doc);
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

        #region [SelectContractManagement]
        public Dto.DTO_DOC_CONTRACT_MANAGEMENT SelectContractManagement(string processId)
        {
            try
            {
                using (Dao.ContractManagementDao dao = new Dao.ContractManagementDao())
                {
                    return dao.SelectContractManagement(processId);
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
