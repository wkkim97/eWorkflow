using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_INCENTIVE_PAYMENT
    {
        public DTO_DOC_INCENTIVE_PAYMENT()
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
        public string SCHEME_DOC_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TOTAL_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DEALERS_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? PAYMENT_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PAYMENT_SYSTEM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PAY_TO { get; set; }

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

        /// <summary>
        /// 
        /// </summary> 
        public string REMARK { get; set; }

        public string SUBJECT_SCHEME { get; set; }
        public string BU_SCHEME { get; set; }
        public string SETTLEMENT_TYPE { get; set; }
        public string DOC_NUM_SCHEME { get; set; }
    }
}
