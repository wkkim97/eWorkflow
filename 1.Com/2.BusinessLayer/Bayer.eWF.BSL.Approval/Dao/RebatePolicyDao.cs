using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class RebatePolicyDao : DaoBase
    {
        #region RebatePolicy Merge
        public void MergeRebatePolicy(Dto.DTO_DOC_REBATE_POLICY doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_REBATE_POLICY, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void InsertRebatePolicyProduct(Dto.DTO_DOC_REBATE_POLICY_PRODUCT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_REBATE_POLICY_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //eWorkflow Optimization 2020
        public void InsertRebatePolicyProduct_NEW(Dto.DTO_DOC_REBATE_POLICY_PRODUCT_NEW doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_REBATE_POLICY_PRODUCT_NEW, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dto.DTO_DOC_REBATE_POLICY SelectRebatePolicy(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_REBATE_POLICY>(ApprovalContext.USP_SELECT_REBATE_POLICY, parameters);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region SelectRebatePolicyProduct
        public List<Dto.DTO_DOC_REBATE_POLICY_PRODUCT> SelectRebatePolicyProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_REBATE_POLICY_PRODUCT>(ApprovalContext.USP_SELECT_REBATE_POLICY_PRODUCT
                    , parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //eWorkflow Optimization 2020
        public List<Dto.DTO_DOC_REBATE_POLICY_PRODUCT_NEW> SelectRebatePolicyProduct_NEW(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_REBATE_POLICY_PRODUCT_NEW>(ApprovalContext.USP_SELECT_REBATE_POLICY_PRODUCT_NEW
                    , parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [DeleteRebatePolicyProduct]
        public void DeleteRebatePolicyProduct(string processId, string productcode)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@PRODUCT_CODE", productcode);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_REBATE_POLICY_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //eWorkflow Optimization 2020 
        public void DeleteRebatePolicyProduct_NEW(string processId, string productcode)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@PRODUCT_CODE", productcode);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_REBATE_POLICY_PRODUCT_NEW, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [DeleteRebatePolicyProductAll]
        public void DeleteRebatePolicyProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_REBATE_POLICY_PRODUCT_ALL, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //eWorkflow Optimization 2020
        public void DeleteRebatePolicyProductAll_NEW(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_REBATE_POLICY_PRODUCT_ALL_NEW, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
