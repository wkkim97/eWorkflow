using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_BCS_RETURN_GOODS_LIST
    {
        public DTO_DOC_BCS_RETURN_GOODS_LIST()
        {
            this.CREATE_DATE = DateTime.Now;
            this.UPDATE_DATE = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_NAME { get; set; }

        //eWorkflow Optimization 2020 START
        public string CUSTOMER_NAME_NEW { get; set; }
        //eWorkflow Optimization 2020 END
        /// <summary>
        /// 
        /// </summary> 
        public string CUR_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUR_PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int? CUR_QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUR_REASON { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? CUR_UNIT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? CUR_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REP_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REP_PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int? REP_QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REP_REASON { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? REP_UNIT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? REP_AMOUNT { get; set; }
        //eWorkflow Optimization 2020 START
        public decimal? RETURN_PRICE { get; set; }
        public decimal? RETURN_PRICE_NEW { get; set; }

        public decimal? DIFFERENCE { get; set; }
        public decimal? TOTAL_AMOUNT { get; set; }
        //eWorkflow Optimization 2020 END
        /// <summary>
        /// 
        /// </summary> 
        public string CREATOR_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string UPDATER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? UPDATE_DATE { get; set; }


    }
}
