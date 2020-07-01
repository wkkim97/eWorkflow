using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CreditLimitSettingDao : DaoBase
    {
        #region [MergeCreditLimitSetting]

        public void MergeCreditLimitSetting(Dto.DTO_DOC_CREDIT_LIMIT_SETTING doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CREDIT_LIMIT_SETTING, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [SelectCreditLimitSetting]

        public Dto.DTO_DOC_CREDIT_LIMIT_SETTING SelectCreditLimitSetting(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CREDIT_LIMIT_SETTING>(ApprovalContext.USP_SELECT_CREDIT_LIMIT_SETTING, parameters);

                return result.First();
            }
        }
        #endregion

        
    }
}
