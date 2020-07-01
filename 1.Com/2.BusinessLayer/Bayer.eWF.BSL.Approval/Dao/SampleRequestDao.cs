using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dao
{
    class SampleRequestDao : DaoBase
    {
        #region [MergeSampleRequest]
        public void MergeSampleRequest(Dto.DTO_DOC_SAMPLE_REQUEST doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_SAMPLE_REQUEST, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectSampleRequest]

        public Dto.DTO_DOC_SAMPLE_REQUEST SelectSampleRequest(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_SAMPLE_REQUEST>(ApprovalContext.USP_SELECT_SAMPLE_REQUEST, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeSampleRequestItems]
        public void MergeSampleRequestItems(Dto.DTO_DOC_SAMPLE_REQUEST_ITEMS item)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(item);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_SAMPLE_REQUEST_ITEMS, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectSampleRequestItems]
        public List<Dto.DTO_DOC_SAMPLE_REQUEST_ITEMS> SelectSampleRequestItems(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_SAMPLE_REQUEST_ITEMS>(ApprovalContext.USP_SELECT_SAMPLE_REQUEST_ITEMS, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteSampleRequestItemsAll]
        public void DeleteSampleRequestItemsAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_SAMPLE_REQUEST_ITEMS_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteSampleRequestItemsByIndex]
        public void DeleteSampleRequestItemsByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_SAMPLE_REQUEST_ITEMS, parameters);
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
