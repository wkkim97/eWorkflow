using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_FREE_GOODS_CUSTOMER
    {
        public DTO_DOC_FREE_GOODS_CUSTOMER()
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
        public string CUSTOMER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RECEIPTER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAMPLE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAMPLE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string  QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SAP_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string STATES { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string OLD_DOC_NUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATOR_ID { get; set; }

        public DateTime CREATE_DATE { get; set; }
        public DateTime? UPDATE_DATE { get; set; }

    }
}
