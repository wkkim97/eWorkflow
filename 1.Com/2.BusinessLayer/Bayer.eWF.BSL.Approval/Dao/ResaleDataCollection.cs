using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class ResaleDataCollection :DaoBase
    {
        public void MergeResaleDataCollection(Dto.DTO_DOC_RESALE_DATA_COLLECTION doc) 
        {
            try
            {
                using(context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_RESALE_DATA_COLLECTION, parameters);
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public Dto.DTO_DOC_RESALE_DATA_COLLECTION SelectResaleDataCollection(string processId)
        {
            try
            {
                using (context= new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_RESALE_DATA_COLLECTION>(ApprovalContext.USP_SELECT_RESALE_DATA_COLLECTION, parameters);

                    return result.First();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
