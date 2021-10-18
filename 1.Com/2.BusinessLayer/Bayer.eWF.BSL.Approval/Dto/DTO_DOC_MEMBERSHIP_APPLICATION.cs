using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    public class DTO_DOC_MEMBERSHIP_APPLICATION
    {
        public DTO_DOC_MEMBERSHIP_APPLICATION()
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
        public string TA_CATEGORY { get; set; }
        

        /// <summary>
        /// 
        /// </summary> 
        public string ENG_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string KOR_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string OBJECTIVE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PURPOSE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PRESIDENT_SECRETARY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? MEMBERSHIP_FEE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string PHONE_NO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string FAX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string HOMEPAGE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string RELEVANT_BUSINESS { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string JOB_FUNCTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SUB_COMMITTE { get; set; }

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
