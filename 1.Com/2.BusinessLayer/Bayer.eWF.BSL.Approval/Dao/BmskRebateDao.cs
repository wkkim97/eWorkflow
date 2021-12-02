using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BmskRebateDao : DaoBase
    {
        #region [MergeBmskRebate]
        public void MergeBmskRebate(Dto.DTO_DOC_BMSK_REBATE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BMSK_REBATE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBmskRebate]

        public Dto.DTO_DOC_BMSK_REBATE SelectBmskRebate(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BMSK_REBATE>(ApprovalContext.USP_SELECT_BMSK_REBATE, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeBmskRebateProduct]
        public void MergeBmskRebateProduct(Dto.DTO_DOC_BMSK_REBATE_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BMSK_REBATE_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBmskRebateProduct]
        public List<Dto.DTO_DOC_BMSK_REBATE_PRODUCT> SelectBmskRebateProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BMSK_REBATE_PRODUCT>(ApprovalContext.USP_SELECT_BMSK_REBATE_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBmskRebateProductAll]
        public void DeleteBmskRebateProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BMSK_REBATE_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBmskRebateProductByIndex]
        public void DeleteBmskRebateProductByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BMSK_REBATE_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBmskRebateCustomerAll]
        public void DeleteBmskRebateCustomerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BMSK_REBATE_CUSTOMER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBmskRebateCustomer]
        public List<Dto.DTO_DOC_BMSK_REBATE_CUSTOMER> SelectBmskRebateCustomer(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BMSK_REBATE_CUSTOMER>(ApprovalContext.USP_SELECT_BMSK_REBATE_CUSTOMER, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [InsertBmskRebateCustomer]
        public void InsertBmskRebateCustomer(Dto.DTO_DOC_BMSK_REBATE_CUSTOMER customer)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_BMSK_REBATE_CUSTOMER, parameters);
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
