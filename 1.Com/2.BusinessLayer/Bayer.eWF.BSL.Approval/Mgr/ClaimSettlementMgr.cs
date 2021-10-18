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
    public class ClaimSettlementMgr : MgrBase
    {
        #region [MergeClaimSettlement]
        public string MergeClaimSettlement(Dto.DTO_DOC_CLAIM_SETTLEMENT doc, List<Dto.DTO_DOC_CLAIM_SETTLEMENT_LIST> lists)
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
                    using (Dao.ClaimSettlementDao dao = new Dao.ClaimSettlementDao())
                    {
                        dao.MergeClaimSettlement(doc);
                        dao.DeleteClaimSettlementList(processId);

                        foreach (Dto.DTO_DOC_CLAIM_SETTLEMENT_LIST list in lists)
                        {
                            if (doc.TYPE == "Cash")
                            {
                                if (list.NAME == "" || list.AMOUNT.IsNullOrEmptyEx() || list.BANK_ACCOUNT == " ") continue;
                                list.PROCESS_ID = processId;
                                dao.MergeClaimSettlementList(list);
                            }
                            else if (doc.TYPE == "Commodity")
                            {
                                if (list.PRODUCT_CODE == " ") continue;
                                list.PROCESS_ID = processId;
                                dao.MergeClaimSettlementList(list);
                            }
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

        #region [SelectClaimSettlement]
        public Dto.DTO_DOC_CLAIM_SETTLEMENT SelectClaimSettlement(string processId)
        {
            try
            {
                using (Dao.ClaimSettlementDao dao = new Dao.ClaimSettlementDao())
                {
                    return dao.SelectClaimSettlement(processId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region[SelectClaimSettlementList]
        public List<Dto.DTO_DOC_CLAIM_SETTLEMENT_LIST> SelectClaimSettlementList(string processId)
        {
            try
            {
                using (Dao.ClaimSettlementDao dao = new Dao.ClaimSettlementDao())
                {
                    return dao.SelectClaimSettlementList(processId);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteClaimSettlementListByIndex]
        public void DeleteClaimSettlementListByIndex(string processId, int idx)
        {
            try
            {
                using (Dao.ClaimSettlementDao dao = new Dao.ClaimSettlementDao())
                {
                    dao.DeleteClaimSettlementListByIndex(processId, idx);
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
