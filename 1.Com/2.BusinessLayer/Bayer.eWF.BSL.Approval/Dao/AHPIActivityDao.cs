using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class AHPIActivityDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeAHPIActivity(Dto.DTO_DOC_AH_P_I_ACTIVITY doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_AH_P_I_ACTIVITY, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertAHPIActivityCostDetail(Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL cost)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(cost);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_AH_P_I_ACTIVITY_COST_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAHPIActivityCostDetailAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_AH_P_I_ACTIVITY_COST_DETAIL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAHPIActivityCostDetail(string processId, int index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_AH_P_I_ACTIVITY_COST_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_AH_P_I_ACTIVITY SelectAHPIActivity(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_AH_P_I_ACTIVITY>(ApprovalContext.USP_SELECT_AH_P_I_ACTIVITY, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL> SelectAHPIActivityCostDetail(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                var result = context.Database.SqlQuery<Dto.DTO_DOC_AH_P_I_ACTIVITY_COST_DETAIL>(ApprovalContext.USP_SELECT_AH_P_I_ACTIVITY_COST_DETAIL, parameters);

                return result.ToList();
            }
        }
    }
}
