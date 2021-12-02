using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DNSoft.eWF.FrameWork.Data.EF;

namespace Bayer.eWF.BSL.Reporting
{
    public class ReturnGoodsContext : WorkflowContext
    {
        /// <summary>
        /// 반품 내역 엑셀 업로드
        /// </summary>
        public const string USP_INSERT_RETURN_GOODS_EXCEL = "[dbo].[USP_INSERT_RETURN_GOODS_EXCEL] @SN, @DATE, @DIV, @CUSTOMER_CODE, @PRODUCT_CODE, @BATCH, @QTY, @EXPIRY, @S1, @S2, @INVOICE_PRICE, @REASON, @VC_MANAGER, @CREATE_DATE, @CREATE_ID";

        /// <summary>
        /// 반품 엑셀 업로드 한 내역 조회
        /// </summary>
        public const string USP_SELECT_RETURN_GOODS_EXCEL = "[dbo].[USP_SELECT_RETURN_GOODS_EXCEL] @CREATE_ID";

        /// <summary>
        /// 반품 엑셀 업로드 한 내역 삭제
        /// </summary>
        public const string USP_DELETE_RETURN_GOODS_EXCEL = "[dbo].[USP_DELETE_RETURN_GOODS_EXCEL] @IDX, @CREATE_ID";

        /// <summary>
        /// 반품 엑셀 업로드 한 내역 추가
        /// </summary>
        public const string USP_INSERT_RETURN_GOODS = "[dbo].[USP_INSERT_RETURN_GOODS] @CREATE_ID";

        /// <summary>
        /// 반품 내역 조회
        /// </summary>
        public const string USP_SELECT_RETURN_GOODS = "[dbo].[USP_SELECT_RETURN_GOODS] @STATUS, @USERID, @DATE";

        /// <summary>
        /// 진행 내역 조회
        /// </summary>
        public const string USP_SELECT_RETURN_GOODS_PENDING = "[dbo].[USP_SELECT_RETURN_GOODS_PENDING] @USERID, @DIV_TYPE";

        /// <summary>
        /// 진행 내역 조회
        /// </summary>
        public const string USP_SELECT_RETURN_GOODS_DIVLIST = "[dbo].[USP_SELECT_RETURN_GOODS_DIVLIST]";

        /// <summary>
        /// 반품 내역 업데이트
        /// </summary>
        public const string USP_UPDATE_RETURN_GOODS = "[dbo].[USP_UPDATE_RETURN_GOODS] @IDX, @TYPE, @STATUS, @SHIPTO_CODE, @INVOICE_PRICE, @WHOLESALES_MANAGER_STATUS, @WHOLESALES_SPECIALIST_STATUS, @SALES_ADMIN_STATUS, @UPDATE_ID";

        /// <summary>
        /// SAP 엑셀 업로드 
        /// </summary>
        public const string USP_INSERT_RETURN_GOODS_SAP_EXCEL = "[dbo].[USP_INSERT_RETURN_GOODS_SAP_EXCEL] @SHIPTO_CODE, @PRODUCT_CODE, @BATCH, @SN,  @SAP_AMOUNT, @UNIT_PRICE, @QTY, @CREATE_DATE, @CREATE_ID";
    
        /// <summary>
        /// SAP 엑셀 업로드 한 내역 조회
        /// </summary>
        public const string USP_SELECT_RETURN_GOODS_SAP_EXCEL = "[dbo].[USP_SELECT_RETURN_GOODS_SAP_EXCEL] @CREATE_ID";

        /// <summary>
        /// SAP 엑셀 항목 업데이트
        /// </summary>
        public const string USP_UPDATE_RETURN_GOODS_SAP = "[dbo].[USP_UPDATE_RETURN_GOODS_SAP] @CREATE_ID";
    }
}
