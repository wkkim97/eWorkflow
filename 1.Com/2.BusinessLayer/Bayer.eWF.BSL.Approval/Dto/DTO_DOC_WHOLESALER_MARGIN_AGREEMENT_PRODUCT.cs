using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT
    {
        public DTO_DOC_WHOLESALER_MARGIN_AGREEMENT_PRODUCT()
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
        public string PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? BASE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? MARGIN_AS_IS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? MARGIN_TO_BE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? DISCOUNT_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int QTY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REMARKS { get; set; }

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
