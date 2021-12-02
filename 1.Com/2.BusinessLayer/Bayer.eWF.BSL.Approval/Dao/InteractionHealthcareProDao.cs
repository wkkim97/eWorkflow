using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class InteractionHealthcareProDao : DaoBase
    {
        public void MergeInteractionHealthcarePro(Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INTERACTION_HEALTHCARE_PRO, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeInteractionHealthcareProDetail(Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL detail)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(detail);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INTERACTION_HEALTHCARE_PRO_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteInteractionHealthcareProDetailAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_INTERACTION_HEALTHCARE_PRO_DETAIL_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteInteractionHealthcareProDetail(string processId, int index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_INTERACTION_HEALTHCARE_PRO_DETAIL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO SelectInteractionHealthcarePro(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO>(ApprovalContext.USP_SELECT_INTERACTION_HEALTHCARE_PRO, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL> SelectInteractionHealthcareProDetails(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_DETAIL>(ApprovalContext.USP_SELECT_INTERACTION_HEALTHCARE_PRO_DETAIL, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteInteractionHealthcareProCountryAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_INTERACTION_HEALTHCARE_PRO_COUNTRY_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void MergeInteractionHealthcareProCountry(Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY country)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(country);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_INTERACTION_HEALTHCARE_PRO_COUNTRY, parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        public List<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY> SelectInteractionHealthcareProCountry(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_INTERACTION_HEALTHCARE_PRO_COUNTRY>(ApprovalContext.USP_SELECT_INTERACTION_HEALTHCARE_PRO_COUNTRY, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
