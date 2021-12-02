using Bayer.eWF.BSL.Common.Dao;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bayer.eWF.BSL.Common.Mgr
{
    public class NoticeMgr : MgrBase
    {
        #region [ SelectNoticeList ]

        public List<Dto.DTO_BOARD_NOTICE> SelectNoticeList(int rowCnt)
        {
            try
            {
                using (NoticeDao dao = new NoticeDao())
                {
                    return dao.SelectNoticeList(rowCnt);
                }
            }
            catch
            {
                throw;
            }
        }


        public Dto.DTO_BOARD_NOTICE GetNoticeItem(int idx)
        {
            try
            {
                using (NoticeDao dao = new NoticeDao())
                {
                    return dao.GetNoticeItem(idx);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
         
        #region MergeBoardNotice
        public int MergeBoardNotice(Dto.DTO_BOARD_NOTICE notice)
        {
            int retValue = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                { 
                    using (Dao.NoticeDao dao = new Dao.NoticeDao())
                    {
                        retValue = dao.MergeBoardNotice(notice);
                    }
                    scope.Complete(); 
                }
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteBoardNotice
        public void DeleteBoardNotice(int idx)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (Dao.NoticeDao dao = new Dao.NoticeDao())
                    {
                        dao.DeleteBoardNotice(idx);
                    }
                    scope.Complete();
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
