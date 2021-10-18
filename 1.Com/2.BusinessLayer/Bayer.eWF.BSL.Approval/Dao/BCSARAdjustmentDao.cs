using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BCSARAdjustmentDao : DaoBase
    {
        #region [MergeBCSARAdjustment]
        public void MergeBCSARAdjustment(Dto.DTO_DOC_BCS_AR_ADJUSTMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_AR_ADJUSTMENT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBCSARAdjustment]

        public Dto.DTO_DOC_BCS_AR_ADJUSTMENT SelectBCSARAdjustment(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_AR_ADJUSTMENT>(ApprovalContext.USP_SELECT_AR_ADJUSTMENT, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeBCSARAdjustmentProduct]
        public void MergeBCSARAdjustmentProduct(Dto.DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_AR_ADJUSTMENT_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBCSARAdjustmentProduct]
        public List<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT> SelectBCSARAdjustmentProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_PRODUCT>(ApprovalContext.USP_SELECT_AR_ADJUSTMENT_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBCSARAdjustmentProductAll]
        public void DeleteBCSARAdjustmentProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_AR_ADJUSTMENT_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [MergeBCSARAdjustmentCustomer]
        public void MergeBCSARAdjustmentCustomer(Dto.DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER customer)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_AR_ADJUSTMENT_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [ DeleteBCSARAdjustmentCustomerAll ]
        public void DeleteBCSARAdjustmentCustomerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_AR_ADJUSTMENT_CUSTOMER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBCSARAdjustmentCustomer]
        public List<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER> SelectBCSARAdjustmentCustomer(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_AR_ADJUSTMENT_CUSTOMER>(ApprovalContext.USP_SELECT_AR_ADJUSTMENT_CUSTOMER, parameters);

                    return result.ToList();
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
