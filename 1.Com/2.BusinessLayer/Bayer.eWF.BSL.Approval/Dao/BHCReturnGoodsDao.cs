using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BHCReturnGoodsDao : DaoBase
    {
        #region [MergeBHCReturnGoods]
        public void MergeBhcReturnGoods(Dto.DTO_DOC_BHC_RETURN_GOODS doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BHC_RETURN_GOODS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBHCReturnGoods]

        public Dto.DTO_DOC_BHC_RETURN_GOODS SelectBhcReturnGoods(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BHC_RETURN_GOODS>(ApprovalContext.USP_SELECT_BHC_RETURN_GOODS, parameters);

                return result.First();
            }
        }
        #endregion
    }
}
