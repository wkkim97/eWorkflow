using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT
    {
        public DTO_DOC_BKL_SPECIAL_PRICE_AND_DISCOUNT_PRODUCT()
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
        
        public decimal? LIST_PRICE { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public decimal? BASIC_DISCOUNT { get; set; }
        /// <summary>
        /// 
        /// </summary> 
        public decimal? AS_IS_DISCOUNT { get; set; }
        /// <summary>
        /// 
        /// </summary> 
        
        public decimal? TO_BE_DISCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? W_H_DISCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int VOLUME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? NET_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? NET_SELLING_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? DISCOUNT_RATE { get; set; }

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
