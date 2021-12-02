using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class MembershipApplicationDao : DaoBase
    {
        public void MergeMembershipApplication(Dto.DTO_DOC_MEMBERSHIP_APPLICATION doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_MEMBERSHIP_APPLICATION, parameters);

                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_MEMBERSHIP_APPLICATION SelectMembershipApplication(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_MEMBERSHIP_APPLICATION>(ApprovalContext.USP_SELECT_MEMBERSHIP_APPLICATION, parameters);

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
