using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CSSalesRecognitionDao : DaoBase
    {
        #region [MergeCSSalesRecognition]
        public void MergeCSSalesRecognition(Dto.DTO_DOC_CS_SALES_RECOGNITION doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_SALES_RECOGNITION, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCSSalesRecognition]

        public Dto.DTO_DOC_CS_SALES_RECOGNITION SelectCSSalesRecognition(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_SALES_RECOGNITION>(ApprovalContext.USP_SELECT_CS_SALES_RECOGNITION, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeCSSalesRecognitionProduct]
        public void MergeCSSalesRecognitionProduct(Dto.DTO_DOC_CS_SALES_RECOGNITION_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_SALES_RECOGNITION_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectCSSalesRecognitionProduct]
        public List<Dto.DTO_DOC_CS_SALES_RECOGNITION_PRODUCT> SelectCSSalesRecognitionProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_SALES_RECOGNITION_PRODUCT>(ApprovalContext.USP_SELECT_CS_SALES_RECOGNITION_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCSSalesRecognitionProductALL]
        public void DeleteCSSalesRecognitionProductALL(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CS_SALES_RECOGNITION_PRODUCT_ALL, parameters);
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
