using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Configuration.Dto
{
    public class DTO_CONFIG_APPROVER
    {
        /// <summary>
        /// 
        /// </summary> 
        public string DOCUMENT_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPROVAL_LOCATION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int CONDITION_INDEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MAX_CONDITION_INDEX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string IS_MANDATORY { get; set; }

        /// <summary>
        /// 결재자 ID
        /// </summary> 
        public string APPROVER_ID { get; set; }

        /// <summary>
        /// 결재자명
        /// </summary>
        public string APPROVER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DISPLAY_CONDITION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SQL_CONDITION { get; set; }

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
