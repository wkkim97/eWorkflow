using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class IncidentReportDao : DaoBase
    {

        public void MergeIncidentReport(Dto.DTO_DOC_INCIDENT_REPORT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INCIDENT_REPORT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Dto.DTO_DOC_INCIDENT_REPORT SelectIncidentReport (string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_INCIDENT_REPORT>(ApprovalContext.USP_SELECT_INCIDENT_REPORT, parameters);

                return result.First();
            }
        }

    }
}
