using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BCSProcessChangeDao : DaoBase
    {
        public void MergeBCSProcessChange (Dto.DTO_DOC_BCS_PROCESS_CHANGE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BCS_PROCESS_CHANGE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        public Dto.DTO_DOC_BCS_PROCESS_CHANGE SelectBCSProcessChange (string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_PROCESS_CHANGE>(ApprovalContext.USP_SELECT_BCS_PROCESS_CHANGE, parameters);

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
