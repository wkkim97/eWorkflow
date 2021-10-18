using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Approval.Dao;

namespace Bayer.eWF.BSL.Approval.Mgr
{
    public class ComplianceReviewForPromotionalMaterialMgr : DNSoft.eWF.FrameWork.Data.EF.MgrBase
    {
        public string MergeComplianceReviewForPromotionalMaterial(Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL doc, 
                                                                  List<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT> products,
                                                                  List<Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> details )
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string processId = doc.PROCESS_ID;
                    if (processId.NullObjectToEmptyEx().Length == 0)
                    {
                        using (Dao.CommonDao dao = new Dao.CommonDao())
                        {
                            processId = dao.GetNewProcessID();
                            doc.PROCESS_ID = processId;
                        }
                    }

                    using (ComplianceReviewForPromotionalMaterialDao dao = new ComplianceReviewForPromotionalMaterialDao())
                    {

                        dao.DeleteBHCPromotionalMaterialAll(doc.PROCESS_ID);

                        decimal? total = 0;
                        foreach (Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL cost in details)
                        {
                            cost.PROCESS_ID = doc.PROCESS_ID;
                            dao.InsertBHCPromotionalMaterial(cost);
                            total += cost.AMOUNT;
                        }
                        doc.TOTAL_AMOUNT = total;

                        dao.MergeComplianceReviewForPromotionalMaterial(doc);
                        dao.DeleteComplianceReviewForPromotionalMaterialProductAll(processId);

                        foreach (Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT product in products)
                        {
                            product.PROCESS_ID = processId;
                            dao.MergeComplianceReviewForPromotionalMaterialProduct(product);
                        }
                    }


                    scope.Complete();
                    return processId;
                }
            }
            catch
            {
                throw;
            }
        }


        public Tuple<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL, 
               List<Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>>  SelectCompliacneReviewForPromotionalMaterial(string processId)
        {
            try
            {

                using (ComplianceReviewForPromotionalMaterialDao dao = new ComplianceReviewForPromotionalMaterialDao())
                {
                    Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL doc = dao.SelectComplianceReviewForPromotionalMaterial(processId);

                    List<Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL> details = dao.SelectBHCPromotionalMaterial(processId);

                    return new Tuple<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL, List<Dto.DTO_DOC_COMPLIANCE_BHC_PROMOTIONAL_MATERIAL>>(doc, details);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_PRODUCT> SelectCompliacneReviewForPromotionalMaterialProduct(string processId)
        {
            try
            {
                using (ComplianceReviewForPromotionalMaterialDao dao = new ComplianceReviewForPromotionalMaterialDao())
                {
                    return dao.SelectComplianceReviewForPromotionalMaterialProduct(processId);
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
                using (ComplianceReviewForPromotionalMaterialDao dao = new ComplianceReviewForPromotionalMaterialDao())
                {
                    dao.DeleteBHCPromotionalMaterial(processId, index);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
