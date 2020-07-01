using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class ComplianceReviewForPromotionalMaterialDao   : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeComplianceReviewForPromotionalMaterial(Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeComplianceReviewForPromotionalMaterialProduct(Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteComplianceReviewForPromotionalMaterialProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        public Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL SelectComplianceReviewForPromotionalMaterial(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL>(ApprovalContext.USP_SELECT_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT> SelectComplianceReviewForPromotionalMaterialProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT>(ApprovalContext.USP_SELECT_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> SelectBHCPromotionalMaterial(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                var result = context.Database.SqlQuery<Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>(ApprovalContext.USP_SELECT_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL, parameters);

                return result.ToList();
            }
        }

        public void InsertBHCPromotionalMaterial(Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBHCPromotionalMaterialAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBHCPromotionalMaterial(string processId, int index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
