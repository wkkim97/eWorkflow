using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BCSCompensationDao : DaoBase
    {
        #region [MergeBCSCompensation]
        public void MergeBCSCompensation(Dto.DTO_DOC_BCS_COMPENSATION doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BCS_COMPENSATION, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBCSCompensation]

        public Dto.DTO_DOC_BCS_COMPENSATION SelectBCSCompensation(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_COMPENSATION>(ApprovalContext.USP_SELECT_BCS_COMPENSATION, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeBCSCompensationProduct]
        public void MergeBCSCompensationProduct(Dto.DTO_DOC_BCS_COMPENSATION_PRODUCT product)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(product);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BCS_COMPENSATION_PRODUCT, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBCSCompensationProduct]
        public List<Dto.DTO_DOC_BCS_COMPENSATION_PRODUCT> SelectBCSCompensationProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_COMPENSATION_PRODUCT>(ApprovalContext.USP_SELECT_BCS_COMPENSATION_PRODUCT, parameters);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBCSCompensationProductAll]
        public void DeleteBCSCompensationProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BCS_COMPENSATION_PRODUCT_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBCSCompensationProductsByIndex]
        public void DeleteBCSCompensationProductsByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BCS_COMPENSATION_PRODUCTS, parameters);
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
