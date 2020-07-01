using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class StrategicChangeReportDao : DaoBase
    {
        public void MergeStrategicChangeReport(Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                { 
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_STRATEGIC_CHANGE_REPORT, parameters);

                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT SelectStrategicChangeReport(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_STRATEGIC_CHANGE_REPORT>(ApprovalContext.USP_SELECT_STRATEGIC_CHANGE_REPORT, parameters);

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
