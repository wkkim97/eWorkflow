using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_COUNTRY
    {
        public DTO_DOC_COMPLIANCE_REVIEW_FOR_PROMOTIONAL_MATERIAL_COUNTRY()
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
        public string COUNTRY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COUNTRY_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ISO_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EFPIA_FLAG { get; set; }

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
