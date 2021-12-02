using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class ITResourceDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeITResource(Dto.DTO_DOC_IT_RESOURCE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_IT_RESOURCE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_IT_RESOURCE SelectITResource(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_IT_RESOURCE>(ApprovalContext.USP_SELECT_IT_RESOURCE, parameters);

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
