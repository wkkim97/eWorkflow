using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class DonationDao :DaoBase
    {
        #region [MergeDonation]

        public void MergeDonation(Dto.DTO_DOC_DONATION doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_DONATION, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectDonation]

        public Dto.DTO_DOC_DONATION SelectDonation(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_DONATION>(ApprovalContext.USP_SELECT_DONATION, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeDonationProduct]
        public void MergeDonationProduct(Dto.DTO_DOC_DONATION_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_DONATION_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region [SelectDonationProduct]
        public List<Dto.DTO_DOC_DONATION_PRODUCT> SelectDonationProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_DONATION_PRODUCT>(ApprovalContext.USP_SELECT_DONATION_PRODUCT, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteDonationProduct]
        public void DeleteDonationProduct (string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_DONATION_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region [DeleteDonationProductByIndex]
        public void DeleteDonationProductByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_DONATION_PRODUCT_IDX, parameters);
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
