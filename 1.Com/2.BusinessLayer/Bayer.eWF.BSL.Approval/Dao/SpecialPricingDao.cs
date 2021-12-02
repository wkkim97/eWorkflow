using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class SpecialPricingDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeSpecialPricing(Dto.DTO_DOC_SPECIAL_PRICING doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_SPECIAL_PRICING, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeSpecialPricingProduct(Dto.DTO_DOC_SPECIAL_PRICING_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_SPECIAL_PRICING_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_SPECIAL_PRICING SelectSpecialPricing(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_SPECIAL_PRICING>(ApprovalContext.USP_SELECT_SPECIAL_PRICING, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_SPECIAL_PRICING_PRODUCT> SelectSpecialPricingProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_SPECIAL_PRICING_PRODUCT>(ApprovalContext.USP_SELECT_SPECIAL_PRICING_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteSpecialPricingProduct(string processId, string productCode)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@PRODUCT_CODE", productCode);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_SPECIAL_PRICING_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
