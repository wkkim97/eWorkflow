using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CsCreditLimitApprovalDao : DaoBase
    {
        #region [MergeCsCreditLimitApproval]

        public void MergeCsCreditLimitApproval(Dto.DTO_DOC_CS_CREDIT_LIMIT_APPROVAL doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_CREDIT_LIMIT_APPROVAL, parameters);
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
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_CREDIT_LIMIT_APPROVAL>(ApprovalContext.USP_SELECT_CS_CREDIT_LIMIT_APPROVAL, parameters);

                return result.First();
            }
        }
        #endregion

        
    }
}
