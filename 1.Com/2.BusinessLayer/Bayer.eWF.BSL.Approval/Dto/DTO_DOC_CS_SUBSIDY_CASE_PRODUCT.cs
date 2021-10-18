using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_CS_SUBSIDY_CASE_PRODUCT
    {
        public DTO_DOC_CS_SUBSIDY_CASE_PRODUCT()
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
        public int QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? INVOICE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? INVOICE_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
       
        public decimal? VS_INV { get; set; }
        /// 
        public decimal? VOLUME_RB { get; set; }
        public decimal? BASIC_RB { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? ADDITIONAL { get; set; }
        public decimal? PROPOSAL_PRICE { get; set; }
        public decimal? PROPOSAL_AMOUNT { get; set; }
        public decimal? SUBSIDY_UNIT_PRICE { get; set; }
        public decimal? NET2_PRICE { get; set; }


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
