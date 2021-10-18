using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao 
{
    public class BasePriceDao : DaoBase
    {
        #region MergeBasePrice
        public void MergeBasePrice(Dto.DTO_DOC_BASE_PRICE doc) 
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BASE_PRICE, parameters);
                }
            }
            catch (Exception ex)
            {   
                throw ex;
            }
        } 
        #endregion

        #region Insert BasePriceProduct
        public void InsertBasePriceProduct(Dto.DTO_DOC_BASE_PRICE_PRODUCT doc)
        {
            try 
	        {	        
		        using(context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_BASE_PRICE_PRODUCT, parameters);
                }
	        }
	        catch (Exception ex)
	        {		
		        throw ex;
	        }
        } 
        #endregion

        #region SelectBasePrice
        public Dto.DTO_DOC_BASE_PRICE SelectBasePrice(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BASE_PRICE>(ApprovalContext.USP_SELECT_BASE_PRICE, parameters);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion


        #region SelectBasePriceProduct
        public List<Dto.DTO_DOC_BASE_PRICE_PRODUCT> SelectBasePriceProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BASE_PRICE_PRODUCT>(ApprovalContext.USP_SELECT_BASE_PRICE_PRODUCT
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

        #region [DeleteBasePriceProduct]
        public void DeleteBasePriceProduct(string processId, string productcode)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@PRODUCT_CODE", productcode);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BASE_PRICE_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [DeleteBasePriceProductAll]
        public void DeleteBasePriceProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);                    

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BASE_PRICE_PRODUCT_ALL, parameters);
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
