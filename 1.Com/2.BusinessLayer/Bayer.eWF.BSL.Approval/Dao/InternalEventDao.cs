using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class InternalEventDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeInternalEvent(Dto.DTO_DOC_INTERNAL_EVENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INTERNAL_EVENT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertInternalEventCostDetail(Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL cost)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(cost);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_INTERNAL_EVENT_COST_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteInternalEventCostDetailAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_INTERNAL_EVENT_COST_DETAIL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteInternalEventCostDetail(string processId, int index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_INTERNAL_EVENT_COST_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_INTERNAL_EVENT SelectInternalEvent(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_INTERNAL_EVENT>(ApprovalContext.USP_SELECT_INTERNAL_EVENT, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL> SelectInternalEventCostDetail(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                var result = context.Database.SqlQuery<Dto.DTO_DOC_INTERNAL_EVENT_COST_DETAIL>(ApprovalContext.USP_SELECT_INTERNAL_EVENT_COST_DETAIL, parameters);

                return result.ToList();
            }
        }
    }
}
