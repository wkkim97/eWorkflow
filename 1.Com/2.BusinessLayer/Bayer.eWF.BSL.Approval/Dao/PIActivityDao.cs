using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class PIActivityDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergePIActivity(Dto.DTO_DOC_P_I_ACTIVITY doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_P_I_ACTIVITY, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertPIActivityCostDetail(Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL cost)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(cost);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_P_I_ACTIVITY_COST_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePIActivityCostDetailAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_P_I_ACTIVITY_COST_DETAIL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePIActivityCostDetail(string processId, int index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_P_I_ACTIVITY_COST_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_P_I_ACTIVITY SelectPIActivity(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_P_I_ACTIVITY>(ApprovalContext.USP_SELECT_P_I_ACTIVITY, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL> SelectPIActivityCostDetail(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                var result = context.Database.SqlQuery<Dto.DTO_DOC_P_I_ACTIVITY_COST_DETAIL>(ApprovalContext.USP_SELECT_P_I_ACTIVITY_COST_DETAIL, parameters);

                return result.ToList();
            }
        }
    }
}
