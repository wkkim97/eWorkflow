using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class ListPriceDao : DaoBase
    {
        #region MergeListPrice
        public void MergeListPrice(Dto.DTO_DOC_LIST_PRICE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_LIST_PRICE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Insert ListPriceProduct
        public void InsertListPriceProduct(Dto.DTO_DOC_LIST_PRICE_PRODUCT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_LIST_PRICE_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SelectListPrice
        public Dto.DTO_DOC_LIST_PRICE SelectListPrice(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_LIST_PRICE>(ApprovalContext.USP_SELECT_LIST_PRICE, parameters);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region SelectListPriceProduct
        public List<Dto.DTO_DOC_LIST_PRICE_PRODUCT> SelectListPriceProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_LIST_PRICE_PRODUCT>(ApprovalContext.USP_SELECT_LIST_PRICE_PRODUCT
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

        #region [DeleteListPriceProduct]
        public void DeleteListPriceProduct(string processId, string productcode)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@PRODUCT_CODE", productcode);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_LIST_PRICE_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [DeleteListPriceProductAll]
        public void DeleteListPriceProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_LIST_PRICE_PRODUCT_ALL, parameters);
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
