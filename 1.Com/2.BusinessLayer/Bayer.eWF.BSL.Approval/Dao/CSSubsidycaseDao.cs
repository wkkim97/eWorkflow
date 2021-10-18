using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CSSubsidycaseDao : DaoBase
    {
        #region [MergeCSSubsidycase]
        public void MergeCSSubsidycase(Dto.DTO_DOC_CS_SUBSIDY_CASE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_SUBSIDY_REQUEST, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCSSubsidycase]

        public Dto.DTO_DOC_CS_SUBSIDY_CASE SelectCSSubsidycase(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_SUBSIDY_CASE>(ApprovalContext.USP_SELECT_CS_SUBSIDY_REQUEST, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeCSSubsidycaseProduct]
        public void MergeCSSubsidycaseProduct(Dto.DTO_DOC_CS_SUBSIDY_CASE_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_SUBSIDY_REQUEST_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectCSSubsidycaseProduct]
        public List<Dto.DTO_DOC_CS_SUBSIDY_CASE_PRODUCT> SelectCSSubsidycaseProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_SUBSIDY_CASE_PRODUCT>(ApprovalContext.USP_SELECT_CS_SUBSIDY_REQUEST_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCSSubsidycaseProductALL]
        public void DeleteCSSubsidycaseProductALL(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CS_SUBSIDY_REQUEST_PRODUCT_ALL, parameters);
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
