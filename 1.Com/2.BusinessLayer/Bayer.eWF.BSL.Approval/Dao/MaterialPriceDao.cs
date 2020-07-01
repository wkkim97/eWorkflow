using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class MaterialPriceDao : DaoBase
    {
        #region MergeMaterialPrice
        public void MergeMaterialPrice(Dto.DTO_DOC_MATERIAL_PRICE doc) 
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_MATERIAL_PRICE, parameters);
                }
            }
            catch (Exception ex)
            {   
                throw ex;
            }
        } 
        #endregion

        #region InsertMaterialPriceList
        public void InsertMaterialPriceList(Dto.DTO_DOC_MATERIAL_PRICE_LIST doc)
        {
            try 
	        {	        
		        using(context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_MATERIAL_PRICE_LIST, parameters);
                }
	        }
	        catch (Exception ex)
	        {		
		        throw ex;
	        }
        } 
        #endregion

        #region SelectMaterialPrice
        public Dto.DTO_DOC_MATERIAL_PRICE SelectMaterialPrice(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_MATERIAL_PRICE>(ApprovalContext.USP_SELECT_MATERIAL_PRICE, parameters);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion


        #region SelectMaterialPriceList
        public List<Dto.DTO_DOC_MATERIAL_PRICE_LIST> SelectMaterialPriceList(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_MATERIAL_PRICE_LIST>(ApprovalContext.USP_SELECT_MATERIAL_PRICE_LIST
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

        #region SelectMaterialPriceProduct
        public List<Dto.DTO_DOC_MATERIAL_PRICE_PRODUCT> SelectMaterialPriceProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_MATERIAL_PRICE_PRODUCT>(ApprovalContext.USP_SELECT_MATERIAL_PRICE_PRODUCT
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

        #region DeleteMaterialPriceList
        public void DeleteMaterialPriceList(string processId, int IDX)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", IDX);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_MATERIAL_PRICE_LIST, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteMaterialPriceListAll
        public void DeleteMaterialPriceListAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);                    

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_MATERIAL_PRICE_LIST_ALL, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public void MergeMaterialPriceProduct(Dto.DTO_DOC_MATERIAL_PRICE_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_MATERIAL_PRICE_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteMaterialPriceProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_MATERIAL_PRICE_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
    }
   
}
