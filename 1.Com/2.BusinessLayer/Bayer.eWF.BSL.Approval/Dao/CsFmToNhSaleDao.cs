using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class CsFmToNhSaleDao : DaoBase
    {
        #region [MergeCsFmToNhSale]
        public void MergeCsFmToNhSale(Dto.DTO_DOC_CS_FM_TO_NH_SALE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_FM_TO_NH_SALE, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectCsFmToNhSale]

        public Dto.DTO_DOC_CS_FM_TO_NH_SALE SelectCsFmToNhSale(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_FM_TO_NH_SALE>(ApprovalContext.USP_SELECT_CS_FM_TO_NH_SALE, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeCsFmToNhSaleProduct]
        public void MergeCsFmToNhSaleProduct(Dto.DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_CS_FM_TO_NH_SALE_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectCsFmToNhSlaeProduct]
        public List<Dto.DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT> SelectCsFmToNhSaleProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_CS_FM_TO_NH_SALE_PRODUCT>(ApprovalContext.USP_SELECT_CS_FM_TO_NH_SALE_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCsFmToNhSaleProductAll]
        public void DeleteCsFmToNhSaleProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CS_FM_TO_NH_SALE_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteCsFmToNhSaleProductsByIndex]
        public void DeleteCsFmToNhSaleProductsByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_CS_FM_TO_NH_SALE_PRODUCT, parameters);
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
