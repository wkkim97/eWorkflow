using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class BCSReturnGoodsDao : DaoBase
    {
        #region [MergeBcsReturnGoods]
        public void MergeBcsReturnGoods(Dto.DTO_DOC_BCS_RETURN_GOODS doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BCS_RETURN_GOODS, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [SelectBcsReturnGoods]

        public Dto.DTO_DOC_BCS_RETURN_GOODS SelectBcsReturnGoods(string processId)
        {
            using (context = new ApprovalContext())
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_RETURN_GOODS>(ApprovalContext.USP_SELECT_BCS_RETURN_GOODS, parameters);

                return result.First();
            }
        }
        #endregion

        #region [MergeBcsReturnGoodsList]
        public void MergeBcsReturnGoodsList(Dto.DTO_DOC_BCS_RETURN_GOODS_LIST list)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(list);
                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_BCS_RETURN_GOODS_LIST, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [SelectBcsReturnGoodsList]
        public List<Dto.DTO_DOC_BCS_RETURN_GOODS_LIST> SelectBcsReturnGoodsList(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    var result = context.Database.SqlQuery<Dto.DTO_DOC_BCS_RETURN_GOODS_LIST>(ApprovalContext.USP_SELECT_BCS_RETURN_GOODS_LIST, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBcsReturnGoodsListAll]
        public void DeleteBcsReturnGoodsListAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BCS_RETURN_GOODS_LIST_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteBcsReturnGoodsListByIndex]
        public void DeleteBcsReturnGoodsListByIndex(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_BCS_RETURN_GOODS_LIST, parameters);
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
