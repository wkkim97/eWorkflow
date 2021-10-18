using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_CREDIT_DEBIT_NOTE
    {
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
        public string TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string COMPANY_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TO_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? INVOICE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? DUE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CURRENCY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TOTAL_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? LOCAL_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DESCRIPTION { get; set; }

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
        public string REQUESTER_MAIL { get; set; }        
    }
}
