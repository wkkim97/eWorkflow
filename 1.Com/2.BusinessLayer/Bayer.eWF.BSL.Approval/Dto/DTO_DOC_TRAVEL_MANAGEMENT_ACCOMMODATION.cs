using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION
    {
        public DTO_DOC_TRAVEL_MANAGEMENT_ACCOMMODATION()
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
        public string ACCOMMODATION_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ACCOMMODATION_NAME { get; set; }

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
        public decimal? AMOUNT_PER_NIGHT { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AMOUNT_TOTAL { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string REASON_HOTEL { get; set; }

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
