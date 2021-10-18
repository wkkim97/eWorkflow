using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao 
{
    public class ClaimSettlementDao : DaoBase
    {
        #region [MergeClaimSettlement]

        public void MergeClaimSettlement(Dto.DTO_DOC_CLAIM_SETTLEMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CLAIM_SETTLEMENT, parameters);
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
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CLAIM_SETTLEMENT>(ApprovalContext.USP_SELECT_CLAIM_SETTLEMENT, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeClaimSettlementList]
        public void MergeClaimSettlementList(Dto.DTO_DOC_CLAIM_SETTLEMENT_LIST list)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(list);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CLAIM_SETTLEMENT_LIST, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectClaimSettlementList]
        public List<Dto.DTO_DOC_CLAIM_SETTLEMENT_LIST> SelectClaimSettlementList(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CLAIM_SETTLEMENT_LIST>(ApprovalContext.USP_SELECT_CLAIM_SETTLEMENT_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteClaimSettlementList]
        public void DeleteClaimSettlementList(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CLAIM_SETTLEMENT_LIST_ALL, parameters);
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
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CLAIM_SETTLEMENT_LIST_IDX, parameters);
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
