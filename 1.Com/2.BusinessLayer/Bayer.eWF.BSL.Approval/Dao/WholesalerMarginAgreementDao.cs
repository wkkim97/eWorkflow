using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class WholesalerMarginAgreementDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        public void MergeWholesalerMarginAgreement(Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_WHOLESALER_MARGIN_AGREEMENT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeWholesalerMarginAgreementCustomer(Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER customer)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(customer);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeWholesalerMarginAgreementHospital(Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL hospital)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(hospital);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeWholesalerMarginAgreementProduct(Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_WHOLESALER_MARGIN_AGREEMENT_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteWholesalerMarginAgreementCustomerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteWholesalerMarginAgreementHospitalAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteWholesalerMarginAgreementProduct(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_WHOLESALER_MARGIN_AGREEMENT_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT SelectWholesalerMarginAgreement(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT>(ApprovalContext.USP_SELECT_WHOLESALER_MARGIN_AGREEMENT, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER> SelectWholesalerMarginAgreementCustomer(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER>(ApprovalContext.USP_SELECT_WHOLESALER_MARGIN_AGREEMENT_CUSTOMER, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL> SelectWholesalerMarginAgreementHospital(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL>(ApprovalContext.USP_SELECT_WHOLESALER_MARGIN_AGREEMENT_HOSPITAL, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT> SelectWholesalerMarginAgreementProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT>(ApprovalContext.USP_SELECT_WHOLESALER_MARGIN_AGREEMENT_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteWholesalerMarginAgreementProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_WHOLESALER_MARGIN_AGREEMENT_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
