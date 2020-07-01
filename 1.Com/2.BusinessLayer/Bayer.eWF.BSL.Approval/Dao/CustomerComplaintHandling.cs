using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CustomerComplaintHandlingDao : DaoBase
    {
        #region [MergeCustomerComplaintHandling]
        public void MergeCustomerComplaintHandling(Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CUSTOMER_COMPLAINT_HANDLING, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCustomerComplaintHandling]

        public Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING SelectCustomerComplaintHandling(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING>(ApprovalContext.USP_SELECT_CUSTOMER_COMPLAINT_HANDLING, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeCustomerComplaintHandlingProduct]
        public void MergeCustomerComplaintHandlingProduct(Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT list)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(list);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CUSTOMER_COMPLAINT_HANDLING_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectCustomerComplaintHandlingProduct]
        public List<Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT> SelectCustomerComplaintHandlingProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CUSTOMER_COMPLAINT_HANDLING_PRODUCT>(ApprovalContext.USP_SELECT_CUSTOMER_COMPLAINT_HANDLING_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCustomerComplaintHandlingProductAll]
        public void DeleteCustomerComplaintHandlingProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CUSTOMER_COMPLAINT_HANDLING_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCustomerComplaintHandlingProductByIndex]
        public void DeleteCustomerComplaintHandlingProductByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CUSTOMER_COMPLAINT_HANDLING_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
