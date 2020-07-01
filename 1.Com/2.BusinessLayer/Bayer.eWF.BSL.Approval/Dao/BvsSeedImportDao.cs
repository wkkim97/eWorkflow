using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BvsSeedImportDao : DaoBase
    {
        #region BvsSeedImport Merge
        public void MergeBvsSeedImport(Dto.DTO_DOC_BVS_SEED_IMPORT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BVS_SEED_IMPORT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void MergeBvsSeedImportProduct(Dto.DTO_DOC_BVS_SEED_IMPORT_PRODUCT doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BVS_SEED_IMPORT_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dto.DTO_DOC_BVS_SEED_IMPORT SelectBvsSeedImport(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BVS_SEED_IMPORT>(ApprovalContext.USP_SELECT_BVS_SEED_IMPORT, parameters);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region SelectBvsSeedImportProduct
        public List<Dto.DTO_DOC_BVS_SEED_IMPORT_PRODUCT> SelectBvsSeedImportProduct(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BVS_SEED_IMPORT_PRODUCT>(ApprovalContext.USP_SELECT_BVS_SEED_IMPORT_PRODUCT
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

        #region [DeleteBvsSeedImportProduct]
        public void DeleteBvsSeedImportProduct(string processId, string index)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", index);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BVS_SEED_IMPORT_PRODUCT, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [DeleteBvsSeedImportProductAll]
        public void DeleteBvsSeedImportProductAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BVS_SEED_IMPORT_PRODUCT_ALL, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region [DeleteBvsSeedImportItemsByIndex]
        public void DeleteBvsSeedImportItemsByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BVS_SEED_IMPORT_PRODUCT, parameters);
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
