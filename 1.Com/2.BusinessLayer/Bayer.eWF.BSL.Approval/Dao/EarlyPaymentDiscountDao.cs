using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class EarlyPaymentDiscountDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeEarlyPaymentDiscount(Dto.DTO_DOC_EARLY_PAYMENT_DISCOUNT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_EARLY_PAYMENT_DISCOUNT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_EARLY_PAYMENT_DISCOUNT SelectEarlyPaymentDiscount(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_EARLY_PAYMENT_DISCOUNT>(ApprovalContext.USP_SELECT_EARLY_PAYMENT_DISCOUNT, parameters);

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
