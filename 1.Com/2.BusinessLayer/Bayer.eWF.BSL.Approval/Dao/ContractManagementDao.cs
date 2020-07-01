using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class ContractManagementDao : DaoBase
    {
        #region [MergeContractManagement]

        public void MergeContractManagement(Dto.DTO_DOC_CONTRACT_MANAGEMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CONTRACT_MANAGEMENT, parameters);
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
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CONTRACT_MANAGEMENT>(ApprovalContext.USP_SELECT_CONTRACT_MANAGEMENT, parameters);

                return result.First();
            }
        }
        #endregion
    }
}
