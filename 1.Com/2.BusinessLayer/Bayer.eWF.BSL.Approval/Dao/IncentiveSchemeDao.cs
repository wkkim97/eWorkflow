using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class IncentiveSchemeDao : DaoBase
    {

        #region MergeIncentiveScheme
        public void MergeIncentiveScheme(Dto.DTO_DOC_INCENTIVE_SCHEME doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INCENTIVE_SCHEME, parameters);
                   
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
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_INCENTIVE_SCHEME>(ApprovalContext.USP_SELECT_INCENTIVE_SCHEME, parameters);

                return result.First();
            }
        } 
        #endregion

    }
}
