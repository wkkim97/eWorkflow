using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_P_I_ACTIVITY_COST_DETAIL
    {
        public DTO_DOC_P_I_ACTIVITY_COST_DETAIL()
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
        public string CATEGORY_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CATEGORY_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int QTY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? UNIT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? AMOUNT { get; set; }

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
