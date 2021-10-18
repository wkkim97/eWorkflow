using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_INTERACTION_HEALTHCARE_PRO
    {
        public DTO_DOC_INTERACTION_HEALTHCARE_PRO()
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
        public string ACTIVITY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? FROM_EVENT_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TO_EVENT_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MEETING_VENUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ADDRESS_OF_VENUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string VENUE_SELECTION_REASON { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string VENUE_SELECTION_REASON_ETC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string COMPOUNT_STUDY_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SPONSOR_OF_STUDY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CRO_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OBJECTIVE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IS_INVESTIGATOR_MEETING { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IS_MONITORING_MEETING { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OTHER_SPECIFY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IS_CONTRACT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE_OBJECT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SCIENTIFIC_MATERIAL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TARGET_PARTICIPANTS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int GO_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int NON_GO_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int PRIVATE_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int FOREIGN_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int BAYER_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ACCOMMODATION_NEEDED { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXISTS_EFPIA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXISTS_HONORARIUM { get; set; }

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
