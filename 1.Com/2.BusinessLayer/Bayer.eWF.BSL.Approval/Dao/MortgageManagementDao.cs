using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class MortgageManagementDao : DaoBase
    {
        public void MergeMortgageManagement(Dto.DTO_DOC_MORTGAGE_MANAGMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_MORTGAGE_MANAGMENT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dto.DTO_DOC_MORTGAGE_MANAGMENT SelectMortgageManagement(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_MORTGAGE_MANAGMENT>(ApprovalContext.USP_SELECT_MORTGAGE_MANAGMENT, parameters);

                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal SelectNewCreditLimit(string customer, string bg, string status, decimal amount)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@CUSTOMER_CODE", customer);
                    parameters[1] = new SqlParameter("@BG_CODE", bg);
                    parameters[2] = new SqlParameter("@STATUS_CODE", status);
                    parameters[3] = new SqlParameter("@INPUT_AMOUNT", amount);

                    var result = context.Database.SqlQuery<decimal>(ApprovalContext.USP_SELECT_NEW_CREDIT_LIMIT, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateCreditLimitStatus(string processId, string status, string updater, string currentlyId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@CREDIT_LIMIT_STATUS", status);
                    parameters[2] = new SqlParameter("@UPDATER_ID", updater);
                    parameters[3] = new SqlParameter("@CURRENTLY_PROCESS_ID", currentlyId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_MORTGAGE_MANAGEMENT_CREDIT_LIMIT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateCreditLimit(string customer, string bg, decimal amount)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@CUSTOMER_CODE", customer);
                    parameters[1] = new SqlParameter("@BU", bg);
                    parameters[2] = new SqlParameter("@CREDIT_LIMIT", amount);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_UPDATE_CREDIT_LIMIT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
