using Bayer.eWF.BSL.Reporting.Dao;
using Bayer.eWF.BSL.Reporting.Dto;
using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bayer.eWF.BSL.Reporting.Mgr
{
    public class ReportingMgr : MgrBase
    {

        public List<Dto.DTO_REPORTING_PROGRAM> SelectProgramList(string userId, string location)
        {
            try
            {
                using (Dao.ReportingDao dao = new ReportingDao())
                {
                    return dao.SelectProgramList(userId, location);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 반품 내역 엑셀 업로드
        /// </summary>
        /// <param name="list"></param>
        public void InsertReturnGoodsExcel(List<DTO_REPORTING_RETURN_GOODS_EXCEL> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ReportingDao dao = new ReportingDao())
                    {
                        foreach (DTO_REPORTING_RETURN_GOODS_EXCEL item in list)
                        {
                            dao.InsertReturnGoodsExcel(item);
                        }

                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="list"></param>
        public List<Dto.DTO_REPORTING_RETURN_GOODS> SelectReturnGoodsExcel(string createid)
        {
            List<Dto.DTO_REPORTING_RETURN_GOODS> list = null;
            try
            {
                using (ReportingDao dao = new ReportingDao())
                {
                    list = dao.SelectReturnGoodsExcel(createid);
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 삭제
        /// </summary>
        /// <param name="list"></param>
        public void DeleteReturnGoodsExcel(List<DTO_REPORTING_RETURN_GOODS> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ReportingDao dao = new ReportingDao())
                    {
                        foreach (DTO_REPORTING_RETURN_GOODS item in list)
                        {
                            dao.DeleteReturnGoodsExcel(item);
                        }

                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 추가
        /// </summary>
        /// <param name="list"></param>
        public void InsertReturnGoods(string createid)
        {
            try
            {
                using (ReportingDao dao = new ReportingDao())
                {
                    dao.InsertReturnGoods(createid);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="list"></param>
        public List<Dto.DTO_REPORTING_RETURN_GOODS> SelectReturnGoods(string status, string userid, DateTime? date = null)
        {
            List<Dto.DTO_REPORTING_RETURN_GOODS> list = null;
            try
            {
                using (ReportingDao dao = new ReportingDao())
                {
                    list = dao.SelectReturnGoods(status, userid, date);
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        
        
        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="list"></param>
        public List<Dto.DTO_REPORTING_RETURN_GOODS> SelectReturnGoodsPending(string userid , string divtype )
        {
            List<Dto.DTO_REPORTING_RETURN_GOODS> list = null;
            try
            {
                using (ReportingDao dao = new ReportingDao())
                {
                    list = dao.SelectReturnGoodsPending(userid, divtype);
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// TB_REPORTING_RETURN_GOODS 테이블에 대한 DIV 목록 반환
        /// </summary>
         public List<string> SelectReturnGoodsDiv()
        {
            try
            {
                using (Dao.ReportingDao dao = new Dao.ReportingDao())
                {
                    return dao.SelectReturnGoodsDiv();
                }
            }
            catch
            {
                throw;
            }
        }

        


        /// <summary>
        /// 반품 내역 업데이트
        /// </summary>
        /// <param name="list"></param>
        public void UpdateReturnGoods(List<Dto.DTO_REPORTING_RETURN_GOODS> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ReportingDao dao = new ReportingDao())
                    {
                        foreach (DTO_REPORTING_RETURN_GOODS item in list)
                        {
                            dao.UpdateReturnGoods(item);
                        }

                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// SAP 엑셀 업데이트
        /// </summary>
        /// <param name="list"></param>
        public void InsertReturnGoodsSAPExcel(List<Dto.DTO_REPORTING_RETURN_GOODS_SAP_EXCEL> list)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ReportingDao dao = new ReportingDao())
                    {
                        foreach (DTO_REPORTING_RETURN_GOODS_SAP_EXCEL item in list)
                        {
                            dao.InsertReturnGoodsSAPExcel(item);
                        }

                        scope.Complete();
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// SAP 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="list"></param>
        public List<Dto.DTO_REPORTING_RETURN_GOODS> SelectReturnGoodsSAPExcel(string createid)
        {
            List<Dto.DTO_REPORTING_RETURN_GOODS> list = null;
            try
            {
                using (ReportingDao dao = new ReportingDao())
                {
                    list = dao.SelectReturnGoodsSAPExcel(createid);
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// SAP 엑셀 업데이트
        /// </summary>
        /// <param name="list"></param>
        public void UpdateReturnGoodsSAPExcel(string createid)
        {
            try
            {
                using (ReportingDao dao = new ReportingDao())
                {
                    dao.UpdateReturnGoodsSAPExcel(createid);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
