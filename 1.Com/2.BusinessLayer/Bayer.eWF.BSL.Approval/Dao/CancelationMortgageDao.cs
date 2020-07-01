using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CancelationMortgageDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeCancelationMortgage(Dto.DTO_DOC_CANCELATION_MORTGAGE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CANCELATION_MORTGAGE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_CANCELATION_MORTGAGE SelectCancelationMortgage(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result =context.Database.SqlQuery<Dto.DTO_DOC_CANCELATION_MORTGAGE>(ApprovalContext.USP_SELECT_CANCELATION_MORTGAGE, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
