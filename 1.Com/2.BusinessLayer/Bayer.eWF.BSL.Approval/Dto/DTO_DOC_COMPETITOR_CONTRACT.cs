using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_COMPETITOR_CONTRACT
    {
        public DTO_DOC_COMPETITOR_CONTRACT()
        {
            this.MEETING_DATETIME = DateTime.Now;
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
        public string CATEGORY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TA_CATEGORY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string HOST_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? MEETING_DATETIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? FROM_TIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? TO_TIME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string VENUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AGENDA { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COUNTER_PARTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CONTRACT_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CONTRACT_PERIOD { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRODUCT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ACTIVITY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string OTHER_PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXTERNAL_PARTICIPANTS { get; set; }

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
