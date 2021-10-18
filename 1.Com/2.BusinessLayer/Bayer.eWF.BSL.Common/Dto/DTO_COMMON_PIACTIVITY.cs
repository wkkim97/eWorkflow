using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Common.Dto
{
    public class DTO_COMMON_PIACTIVITY
    {
        public DTO_COMMON_PIACTIVITY()
        {
            this.CREATE_DATE = DateTime.Now;
            this.UPDATE_DATE = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary> 
        public string ACTIVITY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ACTIVITY_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int YEAR { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string ACTIVITY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal BUDGET { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string COST_CENTER { get; set; }

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
