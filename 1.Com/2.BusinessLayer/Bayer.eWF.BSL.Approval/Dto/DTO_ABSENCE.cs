using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_ABSENCE
    {
        /// <summary>
        /// 
        /// </summary> 
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string USER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string APPROVER_NAME { get; set; }

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
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime CREATE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? UPDATE_DATE { get; set; }



    }
}
