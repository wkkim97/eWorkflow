using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_TRAVEL_MANAGEMENT
    {
        public DTO_DOC_TRAVEL_MANAGEMENT()
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
        public string TRIP_PURPOSE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TRIP_PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TRIP_PERIOD_FROM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TRIP_PERIOD_TO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TRIP_INFO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TRIP_CONTACT_POINT { get; set; }
        

        /// <summary>
        /// 
        /// </summary> 
        public string REQUESTED_TO_AGENCY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int? QUOTATION_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REASON_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REASON_DESC { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMMENT_TO_AGENCY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AIRFARE_AMOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AIRFARE_COMMENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string IS_DISUSED { get; set; }

        /// <summary>
        /// Quotation첨부
        /// </summary>
        public int QUOTATION_IDX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QUOTATION_FILE_NAME { get; set; }


        /// <summary>
        /// Application첨부
        /// </summary>
        public int APPLICATION_IDX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string APPLICATION_FILE_NAME { get; set; }

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
        /// Agency메일 발송으로 추가
        /// </summary>
        public string REQUESTER_MAIL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string REASON_NAME { get; set; }


    }
}
