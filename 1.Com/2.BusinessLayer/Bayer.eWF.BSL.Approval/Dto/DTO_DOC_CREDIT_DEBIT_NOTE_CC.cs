using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_CREDIT_DEBIT_NOTE_CC
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CC_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CC_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CC_MAIL_ADDRESS { get; set; }

        /// <summary>
        /// I(Internal) E(External)
        /// </summary>
        public string CC_TYPE { get; set; }

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
