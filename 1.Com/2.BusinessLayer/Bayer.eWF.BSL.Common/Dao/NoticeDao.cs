using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dao
{
    public class NoticeDao : DaoBase
    { 
        #region SelectNoticeList
        public List<Dto.DTO_BOARD_NOTICE> SelectNoticeList(int rowCnt)
        {
            try
            {
                using (context = new NoticeContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@ROW_NUM", rowCnt);
                    var result = context.Database.SqlQuery<Dto.DTO_BOARD_NOTICE>(NoticeContext.USP_SELECT_BOARD_NOTICE, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        } 
        #endregion 

        #region GetNoticeItem
        public  Dto.DTO_BOARD_NOTICE GetNoticeItem(int idx)
        {
            try
            {
                using (context = new NoticeContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@IDX", idx);
                    var result = context.Database.SqlQuery<Dto.DTO_BOARD_NOTICE>(NoticeContext.USP_SELECT_BOARD_NOTICE_ITEM, parameters);

                    return result.First();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion 


        #region [MergeBoardNotice]

        public int MergeBoardNotice(Dto.DTO_BOARD_NOTICE notice)
        {
            int idx = 0;
            try
            {
                using (context = new NoticeContext())
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@IDX", notice.IDX);
                    parameters[1] = new SqlParameter("@SUBJECT", notice.SUBJECT);
                    parameters[2] = new SqlParameter("@BODY", notice.BODY);
                    parameters[3] = new SqlParameter("@CREATE_ID", notice.CREATE_ID);
                    parameters[4] = new SqlParameter("@CREATE_DATE", notice.CREATE_DATE);
                    parameters[5] = new SqlParameter("@UPDATE_ID", notice.UPDATE_ID);
                    parameters[6] = new SqlParameter("@UPDATE_DATE", notice.UPDATE_DATE); 

                    idx = Convert.ToInt32(context.Database.SqlQuery<int>(NoticeContext.USP_MERGE_BOARD_NOTICE, parameters).First());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return idx;
        }

        #endregion


        #region [MergeBoardNotice]

        public void DeleteBoardNotice(int idx)
        {
            try
            {
                using (context = new NoticeContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@IDX", idx);
                    context.Database.ExecuteSqlCommand(NoticeContext.USP_DELETE_BOARD_NOTICE_ITEM, parameters);
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
