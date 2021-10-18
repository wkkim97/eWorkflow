using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_FREE_GOODS_CHECK_DUPLICATE    
    {
        public DTO_DOC_FREE_GOODS_CHECK_DUPLICATE()
        {
            this.CREATE_DATE = DateTime.Now;            
        }
        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BU { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INSTITUE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INSTITUE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string HCP_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string HCP_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SAP_PRODUCT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

    }
}
