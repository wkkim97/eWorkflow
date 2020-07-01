using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CSExceptionrequestDao : DaoBase
    {
        #region [MergeCSExceptionrequest]
        public void MergeCSExceptionrequest(Dto.DTO_DOC_CS_EXCEPTION_REQUEST doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_EXCEPTION_REQUEST, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCSExceptionrequest]

        public Dto.DTO_DOC_CS_EXCEPTION_REQUEST SelectCSExceptionrequest(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_EXCEPTION_REQUEST>(ApprovalContext.USP_SELECT_CS_EXCEPTION_REQUEST, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeCSExceptionrequestProduct]
        public void MergeCSExceptionProduct(Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_EXCEPTION_REQUEST_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectCSExceptionrequestProduct]
        public List<Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT> SelectCSExceptionrequestProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_EXCEPTION_REQUEST_PRODUCT>(ApprovalContext.USP_SELECT_CS_EXCEPTION_REQUEST_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCSExceptionrequestProductALL]
        public void DeleteCSExceptionrequestProductALL(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CS_EXCEPTION_REQUEST_PRODUCT_ALL, parameters);
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
