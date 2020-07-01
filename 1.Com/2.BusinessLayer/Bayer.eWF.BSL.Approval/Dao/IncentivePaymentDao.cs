using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class IncentivePaymentDao : DaoBase
    {
        public void MergeIncentivePayment(Dto.DTO_DOC_INCENTIVE_PAYMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INCENTIVE_PAYMENT, parameters);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public Dto.DTO_DOC_INCENTIVE_PAYMENT SelectIncentivePayment(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_INCENTIVE_PAYMENT>(ApprovalContext.USP_SELECT_INCENTIVE_PAYMENT, parameters);

                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dto.DTO_DOC_INCENTIVE_PAYMENT SelectIncentivePayment_I(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_INCENTIVE_PAYMENT>(ApprovalContext.USP_SELECT_INCENTIVE_PAYMENT_I, parameters);

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
