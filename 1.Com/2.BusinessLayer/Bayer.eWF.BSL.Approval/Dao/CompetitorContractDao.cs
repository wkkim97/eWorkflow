using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CompetitorContractDao : DaoBase
    {
        public void MergeCompetitorContract(Dto.DTO_DOC_COMPETITOR_CONTRACT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_COMPETITOR_CONTRACT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletCompetitorContractParticipants(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_COMPETITOR_CONTRACT_PARTICIPANTS, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeCompetitorContractParticipants(Dto.DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT participant)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(participant);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_COMPETITOR_CONTRACT_PARTICIPANTS, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_COMPETITOR_CONTRACT SelectCompetitorContract(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_COMPETITOR_CONTRACT>(ApprovalContext.USP_SELECT_COMPETITOR_CONTRACT, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT> SelectCompetitorContractParticipants(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_COMPETITOR_CONTRACT_PARTICIPANT>(ApprovalContext.USP_SELECT_COMPETITOR_CONTRACT_PARTICIPANTS, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
