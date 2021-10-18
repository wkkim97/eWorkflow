using DNSoft.eWF.FrameWork.Data.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bayer.eWF.BSL.Reporting.Dto;

namespace Bayer.eWF.BSL.Reporting.Dao
{
    public class ReportingDao : DaoBase
    {
        public List<Dto.DTO_REPORTING_PROGRAM> SelectProgramList(string userId, string location)
        {
            try
            {
                using (context = new ReportingContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@USER_ID", userId);
                    parameters[1] = new SqlParameter("@LOCATION", location);

                    var result = context.Database.SqlQuery<Dto.DTO_REPORTING_PROGRAM>(ReportingContext.USP_SELECT_REPORTING_PROGRAM, parameters);

                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        #region [ Return Goods ]

        /// <summary>
        /// 반품 내역 엑셀 업로드
        /// </summary>
        /// <param name="list"></param>
        public void InsertReturnGoodsExcel(Dto.DTO_REPORTING_RETURN_GOODS_EXCEL list)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[15];
                    parameters[0] = new SqlParameter("@SN", list.SN);
                    parameters[1] = new SqlParameter("@DATE", list.DATE);
                    parameters[2] = new SqlParameter("@DIV", list.DIV);
                    parameters[3] = new SqlParameter("@CUSTOMER_CODE", list.CUSTOMER_CODE);
                    parameters[4] = new SqlParameter("@PRODUCT_CODE", list.PRODUCT_CODE);
                    parameters[5] = new SqlParameter("@BATCH", list.BATCH);
                    parameters[6] = new SqlParameter("@QTY", list.QTY);
                    parameters[7] = new SqlParameter("@EXPIRY", list.EXPIRY);
                    parameters[8] = new SqlParameter("@S1", list.S1);
                    parameters[9] = new SqlParameter("@S2", list.S2);
                    parameters[10] = new SqlParameter("@INVOICE_PRICE", list.INVOICE_PRICE);
                    parameters[11] = new SqlParameter("@REASON", list.REASON);
                    parameters[12] = new SqlParameter("@VC_MANAGER", list.VC_MANAGER);
                    parameters[13] = new SqlParameter("@CREATE_DATE", list.CREATE_DATE);
                    parameters[14] = new SqlParameter("@CREATE_ID", list.CREATE_ID);
                    context.Database.ExecuteSqlCommand(ReturnGoodsContext.USP_INSERT_RETURN_GOODS_EXCEL, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="CREATE_ID"></param>
        /// <returns></returns>
        public List<DTO_REPORTING_RETURN_GOODS> SelectReturnGoodsExcel(string createid)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@CREATE_ID", createid);
                    var result = context.Database.SqlQuery<Dto.DTO_REPORTING_RETURN_GOODS>(ReturnGoodsContext.USP_SELECT_RETURN_GOODS_EXCEL, parameters);
                    return result.ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 삭제
        /// </summary>
        /// <param name="list"></param>
        public void DeleteReturnGoodsExcel(Dto.DTO_REPORTING_RETURN_GOODS list)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@IDX", list.IDX);
                    parameters[1] = new SqlParameter("@CREATE_ID", list.CREATE_ID);
                    context.Database.ExecuteSqlCommand(ReturnGoodsContext.USP_DELETE_RETURN_GOODS_EXCEL, parameters);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="createid"></param>
        /// <returns></returns>
        public void InsertReturnGoods(string createid)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@CREATE_ID", createid);
                    context.Database.ExecuteSqlCommand(ReturnGoodsContext.USP_INSERT_RETURN_GOODS, parameters);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="status"></param>
        /// <param name="userid"></param>
        /// <returns></returns>}
        public List<Dto.DTO_REPORTING_RETURN_GOODS> SelectReturnGoods(string status, string userid, DateTime? date)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@STATUS", status);
                    parameters[1] = new SqlParameter("@USERID", userid);
                    if (date == null)
                        parameters[2] = new SqlParameter("@DATE", System.Data.SqlTypes.SqlDateTime.Null);
                    else
                        parameters[2] = new SqlParameter("@DATE", date);
                    var result = context.Database.SqlQuery<Dto.DTO_REPORTING_RETURN_GOODS>(ReturnGoodsContext.USP_SELECT_RETURN_GOODS, parameters);
                    return result.ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 업로드 진행 내역 조회
        /// </summary>
        /// <param name="status"></param>        
        /// <returns></returns>}
        public List<Dto.DTO_REPORTING_RETURN_GOODS> SelectReturnGoodsPending(string userid , string divtype)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[2];                    
                    parameters[0] = new SqlParameter("@USERID", userid);
                    parameters[1] = new SqlParameter("DIV_TYPE", divtype);
                    var result = context.Database.SqlQuery<Dto.DTO_REPORTING_RETURN_GOODS>(ReturnGoodsContext.USP_SELECT_RETURN_GOODS_PENDING, parameters);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RETRUN_GOODS DIV LIST 조회
        /// </summary>
        /// <param name="status"></param>        
        /// <returns></returns>}
        public List<string> SelectReturnGoodsDiv()
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    var result = context.Database.SqlQuery<string>(ReturnGoodsContext.USP_SELECT_RETURN_GOODS_DIVLIST);
                    return result.ToList();
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 반품 엑셀 업로드 내역 업데이트
        /// </summary>
        /// <param name="item"></param>
        public void UpdateReturnGoods(Dto.DTO_REPORTING_RETURN_GOODS item)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@IDX", item.IDX);
                    parameters[1] = new SqlParameter("@TYPE", item.TYPE);
                    parameters[2] = new SqlParameter("@STATUS", item.STATUS);
                    parameters[3] = new SqlParameter("@SHIPTO_CODE", item.SHIPTO_CODE);
                   // parameters[4] = new SqlParameter("@PRODUCT_NAME", item.PRODUCT_NAME);
                    parameters[4] = new SqlParameter("@INVOICE_PRICE", item.INVOICE_PRICE);
                    parameters[5] = new SqlParameter("@WHOLESALES_MANAGER_STATUS", item.WHOLESALES_MANAGER_STATUS);
                    parameters[6] = new SqlParameter("@WHOLESALES_SPECIALIST_STATUS", item.WHOLESALES_SPECIALIST_STATUS);
                    parameters[7] = new SqlParameter("@SALES_ADMIN_STATUS", item.SALES_ADMIN_STATUS);
                    parameters[8] = new SqlParameter("@UPDATE_ID", item.UPDATE_ID);
                    context.Database.ExecuteSqlCommand(ReturnGoodsContext.USP_UPDATE_RETURN_GOODS, parameters);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// SAP 엑셀 업로드
        /// </summary>
        /// <param name="item"></param>
        public void InsertReturnGoodsSAPExcel(Dto.DTO_REPORTING_RETURN_GOODS_SAP_EXCEL item)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@SHIPTO_CODE", item.SHIPTO_CODE);
                    parameters[1] = new SqlParameter("@PRODUCT_CODE", item.PRODUCT_CODE);
                    parameters[2] = new SqlParameter("@BATCH", item.BATCH);
                    parameters[3] = new SqlParameter("@SN", item.SN);
                    parameters[4] = new SqlParameter("@SAP_AMOUNT", item.SAP_AMOUNT);
                    parameters[5] = new SqlParameter("@UNIT_PRICE", item.UNIT_PRICE);
                    parameters[6] = new SqlParameter("@QTY", item.QTY);
                    parameters[7] = new SqlParameter("@CREATE_DATE", item.CREATE_DATE);
                    parameters[8] = new SqlParameter("@CREATE_ID", item.CREATE_ID);
                    context.Database.ExecuteSqlCommand(ReturnGoodsContext.USP_INSERT_RETURN_GOODS_SAP_EXCEL, parameters);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 반품 엑셀 업로드 내역 조회
        /// </summary>
        /// <param name="CREATE_ID"></param>
        /// <returns></returns>
        public List<DTO_REPORTING_RETURN_GOODS> SelectReturnGoodsSAPExcel(string createid)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@CREATE_ID", createid);
                    var result = context.Database.SqlQuery<Dto.DTO_REPORTING_RETURN_GOODS>(ReturnGoodsContext.USP_SELECT_RETURN_GOODS_SAP_EXCEL, parameters);
                    return result.ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// SAP 엑셀 업로드
        /// </summary>
        /// <param name="item"></param>
        public void UpdateReturnGoodsSAPExcel(string createid)
        {
            try
            {
                using (context = new ReturnGoodsContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@CREATE_ID", createid);
                    context.Database.ExecuteSqlCommand(ReturnGoodsContext.USP_UPDATE_RETURN_GOODS_SAP, parameters);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
