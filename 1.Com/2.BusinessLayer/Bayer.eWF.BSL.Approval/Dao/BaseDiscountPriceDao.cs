using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao 
{
    public class BaseDiscountPriceDao : DaoBase
    {
        #region [MergeBaseDiscountPrice]
        public void MergeBaseDiscountPrice(Dto.DTO_DOC_BASE_DISCOUNT_PRICE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BASE_DISCOUNT_PRICE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBaseDiscountPrice]

        public Dto.DTO_DOC_BASE_DISCOUNT_PRICE SelectBaseDiscountPrice(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BASE_DISCOUNT_PRICE>(ApprovalContext.USP_SELECT_BASE_DISCOUNT_PRICE, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeBaseDiscountPriceProduct]
        public void MergeBaseDiscountPriceProduct(Dto.DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BASE_DISCOUNT_PRICE_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBaseDiscountPriceProduct]
        public List<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT> SelectBaseDiscountPriceProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_PRODUCT>(ApprovalContext.USP_SELECT_BASE_DISCOUNT_PRICE_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBaseDiscountPriceProductAll]
        public void DeleteBaseDiscountPriceProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BASE_DISCOUNT_PRICE_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [DeleteBaseDiscountPriceProductByIndex]
        public void DeleteBaseDiscountPriceProductByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BASE_DISCOUNT_PRICE_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [MergeBaseDiscountPriceCustomer]
        public void MergeBaseDiscountPriceCustomer(Dto.DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER customer)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BASE_DISCOUNT_PRICE_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        #region [ DeleteBaseDiscountPriceCustomerAll ]
        public void DeleteBaseDiscountPriceCustomerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BASE_DISCOUNT_PRICE_CUSTOMER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        #region [SelectBaseDiscountPriceCustomer]
        public List<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER> SelectBaseDiscountPriceCustomer(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BASE_DISCOUNT_PRICE_CUSTOMER>(ApprovalContext.USP_SELECT_BASE_DISCOUNT_PRICE_CUSTOMER, parameters);

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
