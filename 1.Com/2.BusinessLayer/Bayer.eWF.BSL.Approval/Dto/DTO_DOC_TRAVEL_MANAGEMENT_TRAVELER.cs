using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER
    {
        public DTO_DOC_TRAVEL_MANAGEMENT_TRAVELER()
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
        public int IDX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TRAVELER_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TRAVELER_ID { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TRAVELER_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string EMP_NO { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DIV_DEPT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COMPANY_ORG { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COST_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string INTERNAL_ORDER { get; set; }

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
