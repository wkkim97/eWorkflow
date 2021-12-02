using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_BCS_COMPENSATION
    {
        public DTO_DOC_BCS_COMPENSATION()
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
        public string TITLE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string BU { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPLAINT_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPLAINT_REPORT_NO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REASON { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RECEVING_COMPENSATION_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RECEVING_COMPENSATION_ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPENSATION_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? PRODUCT_AMOUNT { get; set; }
        public decimal? CASH_AMOUNT { get; set; }
        public decimal? TOTAL_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REMARK { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ATTACH_CHECK{ get; set; }

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
