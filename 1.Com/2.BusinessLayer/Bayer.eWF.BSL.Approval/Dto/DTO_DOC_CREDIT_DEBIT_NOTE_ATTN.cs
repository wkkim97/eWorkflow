using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_CREDIT_DEBIT_NOTE_ATTN
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ATTN_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ATTN_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ATTN_MAIL_ADDRESS { get; set; }

        /// <summary>
        /// I(Internal) E(External)
        /// </summary>
        public string ATTN_TYPE { get; set; }

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
