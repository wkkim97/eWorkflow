using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_MORTGAGE_MANAGMENT
    {
        public DTO_DOC_MORTGAGE_MANAGMENT()
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
        public string CUSTOMER_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CUSTOMER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string STATUS_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BG_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MORTGAGE_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? BOOK_VALUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? REVALUTION_VALUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? NEW_CREDIT_LIMIT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? RECEIVED_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? RETURN_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? ISSUE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? DUE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PUBLISHER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PUBLISHED_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? CREDIT_LIMIT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CURRENTLY_PROCESS_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string APPLY_MASTER { get; set; }

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
