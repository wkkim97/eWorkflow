using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_CS_SUBSIDY_CASE
    {
        public DTO_DOC_CS_SUBSIDY_CASE()
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
        public string TITLE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BG { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DISTRIBUTION_CHANNEL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string NH_CHANNEL { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? ADJUST_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT { get; set; }
        public string EXCEPTION_BACKGROUND { get; set; }
        public string EXCEPTION_REASON { get; set; }
        public string EXCEPTION_TITLE { get; set; }
        public string EXCEPTION_PROPOSAL { get; set; }
        public string EXCEPTION_PROCESS { get; set; }
        public string EXCEPTION_FINANCIAL_IMPACK { get; set; }
                      
        
        public string EXCEPTION_COMMENT { get; set; }

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
