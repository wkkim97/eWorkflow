using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_TRAVEL_MANAGEMENT_ROUTE
    {
        public DTO_DOC_TRAVEL_MANAGEMENT_ROUTE()
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
        public DateTime? DEPARTURE_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public DateTime? RETURN_DATE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DEPARTURE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ARRIVAL_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string TRIP_TYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TRIP_TYPE_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string AIRPLANE_CLASS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AIRPLANE_CLASS_NAME { get; set; }

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
