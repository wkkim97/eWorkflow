using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayer.eWF.BSL.Approval.Dto
{
    [Serializable]
    public class DTO_DOC_MATERIAL_PRICE_LIST
    {
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
        public string MATERIAL_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MATERIAL_NAME { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CURRENCY { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? UNIT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public decimal? TO_BE_UNIT_PRICE { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string MANUFACTURER { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string CREATOR_ID { get; set; }

    }
}
