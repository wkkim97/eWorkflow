using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_CUSTOMER_COMPLAINT_HANDLING
    {
        public DTO_DOC_CUSTOMER_COMPLAINT_HANDLING()
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
        public string SUBJECT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DOC_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PROCESS_STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REQUESTER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime REQUEST_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ORGANIZATION_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string LIFE_CYCLE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BG { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CUSTOMER_TYPE { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string ISSUE_AREA { get; set; }
        public string FARMER_NAME { get; set; }
        public string FARMER_TEL_NO { get; set; }
        public string FARMER_ADDRESS { get; set; }


        public DateTime? ISSUE_DATE { get; set; }
        public string ISSUE_EXPIRED_PERIOD { get; set; }
        public string ISSUE_COLLECTING_METHOD { get; set; }
        public string ACTION_AFTER_RECEPTION { get; set; }
        public string CONTACT { get; set; }
        public string EMPLOYEE { get; set; }
        public string ACTION_AFTER_VISIT { get; set; }
        public string ISSUE_CODE { get; set; }
        public string ISSUE_DESC { get; set; }
        public string ISSUE_REMARK { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public string IS_DISUSED { get; set; }

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
