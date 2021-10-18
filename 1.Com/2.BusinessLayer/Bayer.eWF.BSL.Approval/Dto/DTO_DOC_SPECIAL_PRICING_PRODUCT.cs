using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_SPECIAL_PRICING_PRODUCT
    {
        public DTO_DOC_SPECIAL_PRICING_PRODUCT()
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
        public string PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? PRICE_BEFORE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int ORDER_QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? PRICE_AFTER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? DC { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TOTAL_SALES { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TOTAL_DC { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? REQUEST_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int REQUEST_QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? REQUEST_PRICE { get; set; }

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
