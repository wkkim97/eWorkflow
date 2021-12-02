using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_REBATE_POLICY_PRODUCT
    {        
        public DTO_DOC_REBATE_POLICY_PRODUCT()
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
        public string AS_IS_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AS_IS_PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AS_IS_CHANNEL_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AS_IS_CHANNEL_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AS_IS_DISTRIBUTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AS_IS_LIST { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AS_IS_INVOICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AS_IS_NET1 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AS_IS_NET2 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_BE_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_BE_PRODUCT_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_BE_CHANNEL_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_BE_CHANNEL_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_BE_DISTRIBUTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TO_BE_LIST { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TO_BE_INVOICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TO_BE_NET1 { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TO_BE_NET2 { get; set; }

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


    //eWorkflow Optimization 2020 
    [Serializable]
    public class DTO_DOC_REBATE_POLICY_PRODUCT_NEW
    {
        public DTO_DOC_REBATE_POLICY_PRODUCT_NEW()
        {
            this.CREATE_DATE = DateTime.Now;
            this.UPDATE_DATE = DateTime.Now;
        }
        public string PROCESS_ID { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public decimal? LIST_PRICE { get; set; }
        public decimal? LIST_PRICE_NEW { get; set; }
        public decimal? INVOICE_PRICE { get; set; }
        public decimal? INVOICE_PRICE_NEW { get; set; }
        public decimal? NET_PRICE_NEW { get; set; }
        public decimal? SELLING_DIFF { get; set; }
        public decimal? SELLING_EXPECTED { get; set; }

        public decimal? SELLING_TOTAL { get; set; }
        public decimal? RETURN_PRICE { get; set; }
        public decimal? RETURN_PRICE_NEW { get; set; }
       
        public decimal? RETURN_DIFF { get; set; }
        public decimal? RETURN_EXPECTED { get; set; }

        public decimal? RETURN_TOTAL { get; set; }
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

    public class DTO_REBATE_POLICY_CUSTOMER
    {
        public DTO_REBATE_POLICY_CUSTOMER()
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
        public string CUSTOMER_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_NAME { get; set; }

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
