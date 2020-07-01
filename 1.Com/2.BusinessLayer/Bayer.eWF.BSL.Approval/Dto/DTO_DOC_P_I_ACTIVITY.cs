using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_P_I_ACTIVITY
    {
        public DTO_DOC_P_I_ACTIVITY()
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
        /// //eWorkflow Optimization 2020
        /// </summary> 

        public string REQUEST_TYPE { get; set; }
        
        public string ACTIVITY_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INCENTIVE_AGREEMENT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string INCENTIVE_CHECKLIST { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COST_CENTER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MEETING_VENUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? FROM_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? TO_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ADDRESS_VENUE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RELEVANT_E_WORKFLOW_NO { get; set; }

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
        public int FARMER_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int BAYER_EMPLOYEE_NUM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? TOTAL_AMOUNT { get; set; }

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
        public string EXISTS_PROMOTIONAL { get; set; }

    }
}
