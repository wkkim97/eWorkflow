using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_BASE_PRICE_PRODUCT
    {
        public DTO_DOC_BASE_PRICE_PRODUCT()
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
        public decimal? CURRENT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? CURRENT_PRICE_INCLUDE_VAT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? BASE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? REVISED_PRICE_INCLUDE_VAT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? REVISED_PRICE_EXCLUDE_VAT { get; set; }
                        
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
