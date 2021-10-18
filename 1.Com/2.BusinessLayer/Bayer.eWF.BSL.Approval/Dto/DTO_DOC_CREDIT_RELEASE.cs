using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_CREDIT_RELEASE
    {
        public DTO_DOC_CREDIT_RELEASE()
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
        public string ORDER_NUM { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public string BU { get; set; }

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
        public decimal? AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? INCL_NOTE_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int PAYMENT_TERM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int AGING_DAY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int OVERDUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? CREDIT_LIMIT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? OPEN_ORDER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? ORDERED_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? EXCEEDED_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? COLLATERAL_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REASON { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EXPLANATION_OF_REASON { get; set; }

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
