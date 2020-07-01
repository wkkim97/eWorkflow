using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNSoft.eWF.FrameWork.Data.EF;
using System.Data.SqlClient;

namespace Bayer.eWF.BSL.Approval.Dao
{
    public class FreeGoodsDao : DNSoft.eWF.FrameWork.Data.EF.DaoBase
    {
        #region Merge & Insert
        public void MergerFreeGoods(Dto.DTO_DOC_FREE_GOODS doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_MERGE_FREE_GOODS, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertFreeGoodsInfo(Dto.DTO_DOC_FREE_GOODS_INFO doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_FREE_GOODS_INFO, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void InsertFreeGoodsCustomer(Dto.DTO_DOC_FREE_GOODS_CUSTOMER doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_FREE_GOODS_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion

        #region Delete
        public void DeleteFreeGoodsInfo(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_FREE_GOODS_INFO, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteFreeGoodsCustomer(string processId, int idx)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);
                    parameters[1] = new SqlParameter("@IDX", idx);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_FREE_GOODS_CUSTOMER, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteFreeGoodsInfoAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_FREE_GOODS_INFO_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteFreeGoodsCustomerAll(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_FREE_GOODS_CUSTOMER_ALL, parameters);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Select
        public Dto.DTO_DOC_FREE_GOODS SelectFreeGoods(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_FREE_GOODS>(ApprovalContext.USP_SELECT_FREE_GOODS, parameters);
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dto.DTO_DOC_FREE_GOODS_INFO> SelectFreeGoodsInfo(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_FREE_GOODS_INFO>(ApprovalContext.USP_SELECT_FREE_GOODS_INFO
                    , parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dto.DTO_DOC_FREE_GOODS_CUSTOMER> SelectFreeGoodsCustomer(string processId)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@PROCESS_ID", processId);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_FREE_GOODS_CUSTOMER>(ApprovalContext.USP_SELECT_FREE_GOODS_CUSTOMER
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

        #region Free Goods Check Duplicate
        public void DeleteFreeGoodsCheckDuplicate(string userId )
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@USER_ID", userId);                    

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_DELETE_FREE_GOODS_CHECK_DUPLICATE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        //Insert
        public void InsertFreeGoodsCheckDuplicate(Dto.DTO_DOC_FREE_GOODS_CHECK_DUPLICATE doc)
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = ParameterMapper.Mapping(doc);

                    context.Database.ExecuteSqlCommand(ApprovalContext.USP_INSERT_FREE_GOODS_CHECK_DUPLICATE, parameters);
                }
            }
            catch
            {
                throw;
            }
        }

        //Select 중복된 리스트를 받아온다
        public List<Dto.DTO_DOC_FREE_GOODS_CHECK_DUPLICATE> SelectFreeGoodsCheckDuplicate(string userId , string purpose, string bu )
        {
            try
            {
                using (context = new ApprovalContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@USER_ID", userId);
                    parameters[1] = new SqlParameter("@PURPOSE_CODE", purpose);
                    parameters[2] = new SqlParameter("@BU", bu);

                    var result = context.Database.SqlQuery<Dto.DTO_DOC_FREE_GOODS_CHECK_DUPLICATE>(ApprovalContext.USP_SELECT_FREE_GOODS_DUPLICATE
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
    }
}
