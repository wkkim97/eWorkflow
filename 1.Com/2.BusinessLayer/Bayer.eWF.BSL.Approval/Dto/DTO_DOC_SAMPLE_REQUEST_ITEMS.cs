using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_SAMPLE_REQUEST_ITEMS
    {
        public DTO_DOC_SAMPLE_REQUEST_ITEMS()
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
        public string ITEM_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ITEM_DESC { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? PRODUCT_RB { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int ADDITIONAL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AFFECTED_PERCENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? ADDITION_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COST_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COST_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LOCATION_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LOCATION_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int SAP_QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAP_ORDER_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REMARK { get; set; }

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
