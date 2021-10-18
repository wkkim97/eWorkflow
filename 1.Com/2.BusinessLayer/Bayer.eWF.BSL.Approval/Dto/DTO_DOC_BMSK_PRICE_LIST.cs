using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_BMSK_PRICE_LIST
    {
        public DTO_DOC_BMSK_PRICE_LIST()
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
        public string MATERIAL_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MATERIAL_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal BASE_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal TARGET_MIN_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal REQUEST_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CURRENCY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? FROM_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TO_DATE { get; set; }

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
