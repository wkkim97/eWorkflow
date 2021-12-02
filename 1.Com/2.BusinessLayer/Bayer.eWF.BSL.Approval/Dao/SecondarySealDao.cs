using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class SecondarySealDao : DaoBase
    {
        public void MergeSecondarySeal(Dto.DTO_DOC_SECONDARY_SEAL doc)
        {
            try
            {
                using(context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_SECONDARY_SEAL, parameters);
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public Dto.DTO_DOC_SECONDARY_SEAL SelectSecondarySeal(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_SECONDARY_SEAL>(ApprovalContext.USP_SELECT_SECONDARY_SEAL, parameters);

                    return result.First();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
