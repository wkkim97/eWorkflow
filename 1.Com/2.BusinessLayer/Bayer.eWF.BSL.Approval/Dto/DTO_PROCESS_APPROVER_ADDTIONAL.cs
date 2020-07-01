using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_PROCESS_APPROVER_ADDTIONAL
    {
        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_ID { get; set; }

         
        /// <summary>
        /// 
        /// </summary> 
        public Int32 IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVAL_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? CREATE_DATE { get; set; }
    }
}
